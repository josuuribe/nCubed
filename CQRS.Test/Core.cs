using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RaraAvis.nCubed.CQRS.Core;
using RaraAvis.nCubed.CQRS.Test.FakeObjects;
using System.Collections.Generic;
using RaraAvis.nCubed.CQRS.Test;
using RaraAvis.nCubed.Core.Messaging;
using RaraAvis.nCubed.Core.Messaging.Messages;
using RaraAvis.nCubed.CQRS.Core.Services;
using EmitMapper.MappingConfiguration;
using EmitMapper.MappingConfiguration.MappingOperations;
using EmitMapper;
using RaraAvis.nCubed.Core.Messaging.Bus;
using RaraAvis.nCubed.CQRS.Infrastructure;
using RaraAvis.nCubed.Core.Serialization;
using RaraAvis.nCubed.Core.Containers.DI;
using RaraAvis.nCubed.CQRS.Core.Exceptions;
using RaraAvis.nCubed.Core.Exceptions;
using RaraAvis.nCubed.CQRS.Core.Commands;

namespace RaraAvis.nCubed.CQRS.Test
{
    [TestClass]
    public class Core
    {
        public static IBus<CommandDispatcher, ICommandHandler> CommandBus
        {
            get
            {
                return SystemFactory<CQRSContainer<JsonTextSerializer>>.Container.CommandBus;
            }
        }

        [ClassInitialize]
        public static void OnClassInitialize(TestContext context)
        {
            //fakeJSonContainer = new FakeJsonContainer();
        }

        [ClassCleanup]
        public static void OnClassCleanup()
        {
            // runs after the last test in this class
            //fakeJSonContainer.CQRSContainer.Dispose();
            CommandBus.Dispose();
        }

        public FakeCommandDto CustomConvertToDTO(FakeCommandDto command, object state)
        {
            command.TestString = "As Dto";
            return command;
        }

        public FakeCommand CustomConvertToCommand(FakeCommand command, object state)
        {
            command.TestString = "As Command";
            return command;
        }

        [TestMethod]
        public void CQRSProjections()
        {
            FakeCommandDto fcDto = new FakeCommandDto();
            fcDto.TestString = "Testing";
            FakeCommand fc = fcDto.ProjectAsCommand<FakeCommandDto, FakeCommand>();
            Assert.AreEqual<string>(fc.TestString, fcDto.TestString, "Same Projection Id To DTO");


            fc.TestString = "String changed";
            fcDto = fc.ProjectAsDto<FakeCommand, FakeCommandDto>();
            Assert.AreEqual<string>(fc.TestString, fcDto.TestString, "Same Projection Id From DTO");


            fc.TestString = "Testing collection";
            List<FakeCommand> commands = new List<FakeCommand>();
            commands.Add(fc);
            var dtos = new List<FakeCommandDto>(commands.ProjectAsDto<FakeCommand, FakeCommandDto>());
            Assert.AreEqual<string>(commands[0].TestString, dtos[0].TestString, "Same Projection Id To Collection DTO");


            fcDto.TestString = "String collection changed";
            List<FakeCommandDto> fakeDTOs = new List<FakeCommandDto>();
            fakeDTOs.Add(fcDto);
            List<FakeCommand> fakeCommands = new List<FakeCommand>(fakeDTOs.ProjectAsCommand<FakeCommandDto, FakeCommand>());
            Assert.AreEqual<string>(commands[0].TestString, dtos[0].TestString, "Same Projection Id From Collection DTO");


            var fcdto = fc.ProjectAsDto<FakeCommand, FakeCommandDto>(CustomConvertToDTO);
            Assert.AreEqual<string>(fcdto.TestString, "As Dto", "Same custom Projection string From DTO");

            fc = fcdto.ProjectAsCommand<FakeCommandDto, FakeCommand>(CustomConvertToCommand);
            Assert.AreEqual<string>(fc.TestString, "As Command", "Same custom Projection string From command");

            dtos = new List<FakeCommandDto>(commands.ProjectAsDto<FakeCommand, FakeCommandDto>(CustomConvertToDTO));
            Assert.AreEqual<string>(dtos[0].TestString, "As Dto", "Same custom Projection string From DTO");

            fakeCommands = new List<FakeCommand>(fakeDTOs.ProjectAsCommand<FakeCommandDto, FakeCommand>(CustomConvertToCommand));
            Assert.AreEqual<string>(fakeCommands[0].TestString, "As Command", "Same custom Projection Id From Collection DTO");
        }

        [TestMethod]
        public void CQRSPublishCommand()
        {
            FakeCommand fc = new FakeCommand();

            CommandBus.Publish(fc);
        }

        [TestMethod]
        public void CQRSPublishEnvelopeCommand()
        {
            FakeCommand fc = new FakeCommand();

            Envelope<FakeCommand> env = new Envelope<FakeCommand>(fc);

            CommandBus.Publish<FakeCommand>(env);
        }

        [TestMethod]
        public void CQRSPublishEnvelopeCommandList()
        {
            FakeCommand fc = new FakeCommand();

            Envelope<FakeCommand> env = new Envelope<FakeCommand>(fc);

            List<Envelope<FakeCommand>> envelopes = new List<Envelope<FakeCommand>>();
            envelopes.Add(env);

            CommandBus.Publish<FakeCommand>(envelopes);
        }

        [TestMethod]
        public void CQRSPublishCommandNotRegistered()
        {
            FakeNoCommand fc = new FakeNoCommand();

            CommandBus.Publish<FakeNoCommand>(fc);
        }

        [TestMethod]
        public void CQRSTestExceptions()
        {
            Exception outEx = null;
            CQRSExceptionProcessor.HandleException(new Exception("ThrowException"), out outEx);
            Assert.IsTrue(outEx is SemanticException, "Invalid exception.");

            try
            {
                CQRSExceptionProcessor.Process(() =>
                {
                    throw new Exception("ThrowException");
                });
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is SemanticException, "Invalid exception.");
            }

            bool error = false;

            CQRSExceptionProcessor.Process(() =>
            {
                try
                {
                    throw new Exception("ThrowException");
                }
                catch (Exception)
                {
                    error = true;
                }
            });

            Assert.IsTrue(error, "Error must be manage inside code.");


            try
            {
                CQRSExceptionProcessor.Process(() =>
                {
                    throw new Exception("ThrowException");
                });
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is SemanticException, "Invalid exception.");
            }

            int i = 5;
            FakeCommand fc = null;


            error = false;

            i = CQRSExceptionProcessor.Process<int>(() =>
            {
                try
                {
                    throw new Exception("ThrowException");
                }
                catch (Exception)
                {
                    error = true;
                }
                return i;
            });

            Assert.IsTrue(error, "Error must be manage inside code.");

            try
            {
                i = CQRSExceptionProcessor.Process<int>(() =>
                {
                    throw new Exception("ThrowException");
                });
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is SemanticException, "Invalid exception.");
                Assert.AreEqual(i, 5, "int must be 0.");
            }

            error = false;

            fc = CQRSExceptionProcessor.Process<FakeCommand>(() =>
            {
                try
                {
                    throw new Exception("ThrowException");
                }
                catch (Exception)
                {
                    error = true;
                }
                return fc;
            });

            Assert.IsTrue(error, "Error must be manage inside code.");

            try
            {
                fc = CQRSExceptionProcessor.Process<FakeCommand>(() =>
                {
                    throw new Exception("ThrowException");
                });
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is SemanticException, "Invalid exception.");
                Assert.IsNull(fc, "object must be null.");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "The command handled by the received handler already has a registered handler.")]
        public void CQRSTestDuplicatedRegisteredCommandHandler()
        {
            CommandDispatcher cd = new CommandDispatcher();
            cd.Register(new FakeTestCommandHandler());
            cd.Register(new FakeTestCommandHandler());
        }
    }
}
