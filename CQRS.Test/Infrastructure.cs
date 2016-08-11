using Microsoft.VisualStudio.TestTools.UnitTesting;
using RaraAvis.nCubed.Core.Containers.DI;
using RaraAvis.nCubed.Core.Exceptions;
using RaraAvis.nCubed.Core.Messaging.Messages;
using RaraAvis.nCubed.Core.Serialization;
using RaraAvis.nCubed.Core.Testing;
using RaraAvis.nCubed.CQRS.Core.Commands;
using RaraAvis.nCubed.CQRS.Core.Entities;
using RaraAvis.nCubed.CQRS.Core.ProcessManager.Fakes;
using RaraAvis.nCubed.CQRS.Core.RepositoryContracts;
using RaraAvis.nCubed.CQRS.Infrastructure;
using RaraAvis.nCubed.CQRS.Test.FakeObjects;
using System;
using System.Collections.Generic;
using System.Data.Entity.Fakes;

namespace RaraAvis.nCubed.CQRS.Test
{
    [TestClass]
    public class Infrastructure
    {
        static TestAction testAction = null;
        static StubIProcessManager stubProcessManager;

        public IProcessManagerRepository<StubIProcessManager> StubProcessManagerRepository
        {
            get
            {
                return SystemFactory<CQRSContainer<JsonTextSerializer>>.Container.CreateProcessManager<StubIProcessManager>();
            }
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            // runs before the first test in this class

            //fakeJSonContainer = new FakeJsonContainer();
            //pmr = fakeJSonContainer.CQRSContainer.CreateProcessManager<TestDbContext, StubIProcessManager>();
            testAction = new TestAction(context);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            stubProcessManager = new StubIProcessManager();

            testAction.TestInitialize();
            testAction.UseRegisterTestDbSet<StubIProcessManager>();
            testAction.UseRegisterTestDbSet<UndispatchedMessage>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // runs after every test
            testAction.TestCleanup();
        }

        [ClassCleanup]
        public static void ClassCleanUp()
        {
            // runs after the last test in this class

            testAction.ClassCleanup();
        }

        [TestMethod]
        public void CQRSTestProcessManagerSaveMethodWithNullCommnds()
        {
            int expected = 1;

            StubProcessManagerRepository.Save(stubProcessManager);

            Assert.AreEqual<int>(expected, testAction.GetSet<StubIProcessManager>().Local.Count, "Expected one process manager saved.");
        }

        [TestMethod]
        public void CQRSTestProcessManagerSaveMethodWithOneCommand()
        {
            int expected = 0;

            Queue<Envelope<ICommand>> queue = new Queue<Envelope<ICommand>>();
            queue.Enqueue(new FakeCommand());

            stubProcessManager.CommandsGet = () =>
            {
                return queue;
            };


            StubProcessManagerRepository.Save(stubProcessManager);

            Assert.AreEqual<int>(expected, queue.Count, "Expected save with no commands left.");
        }

        [ExpectedException(typeof(SemanticException), "Exception saving expected.")]
        [TestMethod]
        public void CQRSTestProcessManagerSaveMethodWithException()
        {
            Queue<Envelope<ICommand>> queue = new Queue<Envelope<ICommand>>();
            queue.Enqueue(new FakeCommand());

            stubProcessManager.CommandsGet = () =>
            {
                return queue;
            };

            int i = 0;

            ShimDbContext.AllInstances.SaveChanges = (x) =>
            {
                i++;
                if (i == 1)
                    throw new Exception("Intentional exception raised.");
                return 0;
            };

            StubProcessManagerRepository.Save(stubProcessManager);
        }

        [TestMethod]
        public void CQRSTestProcessManagerUpdateMethod()
        {
            StubProcessManagerRepository.Save(stubProcessManager);

            stubProcessManager.IdGet = () =>
            {
                return Guid.Parse("5112E1E6-DAB6-4F8E-906E-A22BD94B231A");
            };

            StubProcessManagerRepository.Update(stubProcessManager);

            var pmr2 = StubProcessManagerRepository.Find(Guid.Parse("5112E1E6-DAB6-4F8E-906E-A22BD94B231A"));

            Assert.AreEqual<Guid>(pmr2.IdGet.Invoke(), Guid.Parse("5112E1E6-DAB6-4F8E-906E-A22BD94B231A"), "Update process manager with different Guid as expected.");
        }

        [TestMethod]
        public void CQRSTestProcessManagerFindMethod()
        {
            StubProcessManagerRepository.Save(stubProcessManager);

            Guid guid = Guid.Parse("5A8CEF18-B904-4359-9425-CA970DCA66DB");

            stubProcessManager.IdGet = () =>
            {
                return guid;
            };

            var pm = StubProcessManagerRepository.Find(guid);

            Assert.IsNotNull(pm, "Process manager found, it must exists.");
        }

        [TestMethod]
        public void CQRSTestProcessManagerFindMethodWithLambda()
        {
            StubProcessManagerRepository.Save(stubProcessManager);

            stubProcessManager.IdGet = () =>
            {
                return Guid.Parse("5A8CEF18-B904-4359-9425-CA970DCA66DB");
            };

            var pm = StubProcessManagerRepository.Find(x => x.IdGet.Invoke() == Guid.Parse("5A8CEF18-B904-4359-9425-CA970DCA66DB"));

            Assert.IsNotNull(pm, "Process manager found, it must exists.");
        }

        [TestMethod]
        public void CQRSTestProcessManagerFindMethodWithLambdaNotFound()
        {
            stubProcessManager.CompletedGet = () =>
            {
                return true;
            };

            StubProcessManagerRepository.Save(stubProcessManager);

            stubProcessManager.IdGet = () =>
            {
                return Guid.Parse("5A8CEF18-B904-4359-9425-CA970DCA66DB");
            };

            var pm = StubProcessManagerRepository.Find(x => x.IdGet.Invoke() == Guid.Parse("5A8CEF18-B904-4359-9425-CA970DCA66DB"), false);

            Assert.IsNull(pm, "Process manager not found, it must be null.");
        }

        [TestMethod]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void CQRSDisposeCommandBus()
        {
            var bus = SystemFactory<CQRSContainer<JsonTextSerializer>>.Container.CommandBus;
            bus.Dispose();

            FakeCommand fc = new FakeCommand();

            bus.Publish<FakeCommand>(fc);
        }

        [TestMethod]
        public void CQRSTestCheckEmptyCommand()
        {
            UndispatchedMessage ud = new UndispatchedMessage();
            Assert.AreEqual(DateTime.Now.Year, ud.CreatedTime.Year,"Time is not now.");
            Assert.AreEqual(DateTime.Now.Month, ud.CreatedTime.Month, "Time is not now.");
            Assert.AreEqual(DateTime.Now.Day, ud.CreatedTime.Day, "Time is not now.");
        }
    }
}
