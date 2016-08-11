using Microsoft.VisualStudio.TestTools.UnitTesting;
using RaraAvis.nCubed.Core.Containers.DI;
using RaraAvis.nCubed.Core.Exceptions;
using RaraAvis.nCubed.Core.Messaging.Bus;
using RaraAvis.nCubed.Core.Serialization;
using RaraAvis.nCubed.Core.Testing;
using RaraAvis.nCubed.EventSourcing.Core.Entities;
using RaraAvis.nCubed.EventSourcing.Core.Events;
using RaraAvis.nCubed.EventSourcing.Core.RepositoryContracts;
using RaraAvis.nCubed.EventSourcing.Infrastructure;
using RaraAvis.nCubed.EventSourcing.Test.FakeObjects;
using RaraAvis.nCubed.EventSourcing.Test.FakeObjects.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.EventSourcing.Test
{
    [TestClass]
    public class Infrastructure
    {
        static TestAction testAction = null;

        public static IBus<EventDispatcher, IEventHandler> EventBus
        {
            get
            {
                return SystemFactory<EventSourcingContainer<JsonTextSerializer>>.Container.EventBus;
            }
        }

        public IEventSourcedRepository<FakeVersionedEntity> FakeEventSourcedRepository
        {
            get
            {
                return SystemFactory<EventSourcingContainer<JsonTextSerializer>>.Container.CreateEventSourced<FakeVersionedEntity>();
            }
        }

        public IEventSourcedRepository<FakeMementoVersionedEntity> FakeMementoEventSourcedRepository
        {
            get
            {
                return SystemFactory<EventSourcingContainer<JsonTextSerializer>>.Container.CreateEventSourced<FakeMementoVersionedEntity>();
            }
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            testAction = new TestAction(context);

            //fjsonContainer = new FakeJsonContainer();
            //MemoryCache mc = new MemoryCache("TestCache");
        }

        [TestInitialize]
        public virtual void TestInitialize()
        {
            testAction.TestInitialize();
            testAction.UseRegisterTestDbSet<EventData>();
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            testAction.TestCleanup();
        }

        [ClassCleanup]
        public static void ClassCleanUp()
        {
            testAction.ClassCleanup();
            EventBus.Dispose();
        }

        [TestMethod]
        public void ESSaveEntity()
        {
            FakeEvent fe = new FakeEvent();

            FakeVersionedEntity fee = new FakeVersionedEntity();
            fee.Events.Enqueue(fe);
            FakeEventSourcedRepository.Save(fee);

            Assert.IsTrue(fee.Events.Count == 0, "Events in queue");
            Assert.IsTrue(String.IsNullOrEmpty(Store.EnvelopeId), "Different IDs");
            Assert.AreEqual(Store.Id, fee.Id, "Different IDs");
        }

        [TestMethod]
        [ExpectedException(typeof(SemanticException))]
        public void ESSaveEntityFailedPublishing()
        {
            Guid g = Guid.NewGuid();
            FakeVersionedEntity fee = new FakeVersionedEntity();
            fee.Events.Enqueue(new FakeEvent());
            fee.Events.Enqueue(new FakeFailedEvent());
            fee.Events.Enqueue(new FakeEvent());
            FakeEventSourcedRepository.Save(fee);

            Assert.IsTrue(fee.Events.Count == 1, "One event pending in queue");
            Assert.AreEqual(Store.EnvelopeId.ToString(), typeof(FakeFailedEvent).Name);
        }

        [TestMethod]
        [ExpectedException(typeof(SemanticException))]
        public void ESSaveEntityFailedSavingAfterFailedPublishing()
        {
            testAction.MarkExceptionSavingDbSet<Exception>(1, false);

            FakeVersionedEntity fee = new FakeVersionedEntity();
            fee.Events.Enqueue(new FakeEvent());
            fee.Events.Enqueue(new FakeFailedEvent());
            fee.Events.Enqueue(new FakeEvent());
            FakeEventSourcedRepository.Save(fee);

            Assert.IsTrue(fee.Events.Count == 1, "One event pending in queue");
            Assert.AreEqual(Store.EnvelopeId.ToString(), typeof(FakeFailedEvent).Name);
        }

        [TestMethod]
        [ExpectedException(typeof(SemanticException))]
        public void ESSaveEntityFailedPublishSuceeded()
        {
            testAction.MarkExceptionSavingDbSet<Exception>(1, false);

            FakeVersionedEntity fee = new FakeVersionedEntity();
            fee.Events.Enqueue(new FakeEvent());
            FakeEventSourcedRepository.Save(fee);

            Assert.IsTrue(fee.Events.Count == 0, "Must no be events pending in queue");
            Assert.AreEqual(Store.EnvelopeId.ToString(), fee.Id.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(SemanticException))]
        public void ESSaveEntityFailedSaving()
        {
            testAction.MarkExceptionSavingDbSet<TimeoutException>(1, true);

            FakeVersionedEntity fee = new FakeVersionedEntity();
            fee.Events.Enqueue(new FakeEvent());
            FakeEventSourcedRepository.Save(fee);
        }

        [TestMethod]
        public void ESEventBusPublish()
        {
            IEnumerable<FakeEvent> fakeEvents = new List<FakeEvent> { new FakeEvent(), new FakeEvent(), new FakeEvent() };

            EventBus.Publish(fakeEvents);

            Assert.IsNull(Store.EnvelopeId, "Envelope Id must be null.");
        }

        [TestMethod]
        public void ESEventBusPublishAsync()
        {
            IEnumerable<FakeEvent> fakeEvents = new List<FakeEvent> { new FakeEvent(), new FakeEvent(), new FakeEvent() };

            Task t = EventBus.PublishAsync(fakeEvents);

            t.Wait();

            Assert.IsNull(Store.EnvelopeId, "Envelope Id must be null.");
        }

        [TestMethod]
        public void ESLoadEvents()
        {
            FakeVersionedEntity feeOriginal = new FakeVersionedEntity();

            FakeVersionedEntity feeRestored = new FakeVersionedEntity(feeOriginal.Id);

            feeOriginal.RaiseFakeEvent(new FakeEvent() { TestString = "a" });
            feeOriginal.RaiseFakeEvent(new FakeEvent() { TestString = "b" });
            feeOriginal.RaiseFakeEvent(new FakeEvent() { TestString = "c" });

            FakeEventSourcedRepository.Save(feeOriginal);

            FakeEventSourcedRepository.Load(feeRestored);

            Assert.AreEqual(feeOriginal.FakeString, feeRestored.FakeString, "Not the same string.");
            Assert.AreEqual(feeOriginal.Events.Count, 0, "Original with events.");
            Assert.AreEqual(feeRestored.Events.Count, 0, "Restored with events.");
        }

        [TestMethod]
        public void ESTestMemento()
        {
            FakeMementoVersionedEntity feeOriginal = new FakeMementoVersionedEntity();

            FakeMementoVersionedEntity feeRestored = new FakeMementoVersionedEntity(feeOriginal.Id);

            feeOriginal.RaiseFakeEvent(new FakeEvent() { TestString = "a" });
            feeOriginal.RaiseFakeEvent(new FakeEvent() { TestString = "b" });
            feeOriginal.RaiseFakeEvent(new FakeEvent() { TestString = "c" });

            FakeMementoEventSourcedRepository.Save(feeOriginal);

            FakeMementoEventSourcedRepository.Load(feeRestored);

            Assert.AreEqual(feeOriginal.FakeString, feeRestored.FakeString, "Not the same string.");
            Assert.AreEqual(feeOriginal.Events.Count, 0, "Original with events.");
            Assert.AreEqual(feeRestored.Events.Count, 0, "Restored with events.");
        }
    }
}
