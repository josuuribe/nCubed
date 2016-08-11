using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RaraAvis.nCubed.EventSourcing.Test.FakeObjects;
using RaraAvis.nCubed.EventSourcing.Core;
using RaraAvis.nCubed.Core.Serialization;
using RaraAvis.nCubed.EventSourcing.Core.Services;
using RaraAvis.nCubed.EventSourcing.Test.FakeObjects.Events;
using RaraAvis.nCubed.EventSourcing.Core.Events;
using RaraAvis.nCubed.EventSourcing.Core.Exceptions;
using RaraAvis.nCubed.Core.Exceptions;

namespace RaraAvis.nCubed.EventSourcing.Test
{
    /// <summary>
    /// Summary description for Core
    /// </summary>
    [TestClass]
    public class Core
    {
        public FakeEventDto CustomConvertToDTO(FakeEventDto command, object state)
        {
            command.TestString = "As Dto";
            return command;
        }

        public FakeEvent CustomConvertToCommand(FakeEvent command, object state)
        {
            command.TestString = "As Event";
            return command;
        }

        [TestMethod]
        public void ESTestProjections()
        {
            FakeEventDto evDto = new FakeEventDto();
            evDto.TestString = "Testing";

            FakeEvent fc = evDto.ProjectAsEvent<FakeEventDto, FakeEvent>();
            Assert.AreEqual<string>(fc.TestString, evDto.TestString, "Same Projection Id To DTO");


            fc.TestString = "String changed";
            evDto = fc.ProjectAsDto<FakeEvent, FakeEventDto>();
            Assert.AreEqual<string>(fc.TestString, evDto.TestString, "Same Projection Id From DTO");


            fc.TestString = "Testing collection";
            List<FakeEvent> events = new List<FakeEvent>();
            events.Add(fc);
            var dtos = new List<FakeEventDto>(events.ProjectAsDto<FakeEvent, FakeEventDto>());
            Assert.AreEqual<string>(events[0].TestString, dtos[0].TestString, "Same Projection Id To Collection DTO");


            evDto.TestString = "String collection changed";
            List<FakeEventDto> fakeDTOs = new List<FakeEventDto>();
            fakeDTOs.Add(evDto);
            List<FakeEvent> fakeCommands = new List<FakeEvent>(fakeDTOs.ProjectAsEvent<FakeEventDto, FakeEvent>());
            Assert.AreEqual<string>(events[0].TestString, dtos[0].TestString, "Same Projection Id From Collection DTO");


            var fcdto = fc.ProjectAsDto<FakeEvent, FakeEventDto>(CustomConvertToDTO);
            Assert.AreEqual<string>(fcdto.TestString, "As Dto", "Same custom Projection string From DTO");

            fc = fcdto.ProjectAsEvent<FakeEventDto, FakeEvent>(CustomConvertToCommand);
            Assert.AreEqual<string>(fc.TestString, "As Event", "Same custom Projection string From command");

            dtos = new List<FakeEventDto>(events.ProjectAsDto<FakeEvent, FakeEventDto>(CustomConvertToDTO));
            Assert.AreEqual<string>(dtos[0].TestString, "As Dto", "Same custom Projection string From DTO");

            fakeCommands = new List<FakeEvent>(fakeDTOs.ProjectAsEvent<FakeEventDto, FakeEvent>(CustomConvertToCommand));
            Assert.AreEqual<string>(fakeCommands[0].TestString, "As Event", "Same custom Projection Id From Collection DTO");
        }


        [TestMethod]
        public void ESRaiseEvents()
        {
            int expectedEvents = 3;
            int expectedVersion = 3;

            string one = "1+";
            string two = "1=";
            string three = "2";

            Queue<IVersionedEvent> queue = new Queue<IVersionedEvent>();
            queue.Enqueue(new FakeEvent());

            FakeVersionedEntity fve = new FakeVersionedEntity();
            FakeEvent fe1 = new FakeEvent() { TestString = one };
            FakeEvent fe2 = new FakeEvent() { TestString = two };
            FakeEvent fe3 = new FakeEvent() { TestString = three };
            fve.RaiseFakeEvent(fe1);
            fve.RaiseFakeEvent(fe2);
            fve.RaiseFakeEvent(fe3);

            List<FakeEvent> fes = new List<FakeEvent>() { fe1, fe2, fe3 };

            Assert.AreEqual(fve.FakeString, one + two + three, "Event not applied");
            Assert.AreEqual(fve.Version, expectedVersion, "Version does not mismatch");
            Assert.AreEqual(fve.Events.Count, expectedEvents, "Count events expected");
        }

        [TestMethod]
        public void ESApplyEventsUsingRaise()
        {
            int expectedEvents = 0;
            int expectedVersion = 3;

            string one = "1+";
            string two = "1=";
            string three = "2";

            Queue<IVersionedEvent> queue = new Queue<IVersionedEvent>();
            queue.Enqueue(new FakeEvent());

            FakeVersionedEntity fve = new FakeVersionedEntity();
            FakeEvent fe1 = new FakeEvent() { TestString = one };
            fe1.Version = 1;
            FakeEvent fe2 = new FakeEvent() { TestString = two };
            fe2.Version = 2;
            FakeEvent fe3 = new FakeEvent() { TestString = three };
            fe3.Version = 3;

            List<FakeEvent> fes = new List<FakeEvent>() { fe1, fe2, fe3 };

            fve.ApplyEvents(fes);

            Assert.AreEqual(fve.FakeString, one + two + three, "Event not applied");
            Assert.AreEqual(fve.Version, expectedVersion, "Version does not mismatch");
            Assert.AreEqual(fve.Events.Count, expectedEvents, "There are pending events");
        }

        [TestMethod]
        public void ESProcessException()
        {
            string expected = "ok";
            string actual = string.Empty;

            EventSourcingExceptionProcessor.Process(() =>
                {
                    actual = "ok";
                });

            Assert.AreEqual(expected, actual, "Expected string different than actual.");
        }

        [TestMethod]
        public void ESProcessExceptionReturnValue()
        {
            int expectedInteger = 3;
            string expectedString = new String("test".ToCharArray());

            int actualInteger = EventSourcingExceptionProcessor.Process<int>(() =>
            {
                return 3;
            });

            string actualString = EventSourcingExceptionProcessor.Process<String>(() =>
            {
                return "test";
            });

            Assert.AreEqual(expectedInteger, actualInteger, "Expected int different than actual.");
            Assert.AreEqual(expectedString, actualString, "Expected string different than actual.");
        }
    }
}
