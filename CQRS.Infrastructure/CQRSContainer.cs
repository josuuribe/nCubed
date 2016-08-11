using RaraAvis.nCubed.Core.Configurations;
using RaraAvis.nCubed.Core.Containers;
using RaraAvis.nCubed.Core.Messaging;
using RaraAvis.nCubed.Core.Messaging.Bus;
using RaraAvis.nCubed.Core.Messaging.Messages;
using RaraAvis.nCubed.Core.Serialization;
using RaraAvis.nCubed.CQRS.Core.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using System.Linq;
using RaraAvis.nCubed.CQRS.Core.RepositoryContracts;
using RaraAvis.nCubed.CQRS.Core.ProcessManager;
using RaraAvis.nCubed.CQRS.Infrastructure.Repositories;
using System.Data.Entity;
using RaraAvis.nCubed.Core.Configurations.Common.Types;
using RaraAvis.nCubed.Core.Containers.DI;
using RaraAvis.nCubed.CQRS.Core;

namespace RaraAvis.nCubed.CQRS.Infrastructure
{
    /// <summary>
    /// Class that manages CQRS types.
    /// </summary>
    /// <typeparam name="T">A <see cref="ITextSerializer"/> object.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "CQRS")]
    public sealed class CQRSContainer<T> : SerializerContainer<T>
        where T : ITextSerializer, new()
    {
        private MessageProcessor<CommandDispatcher, ICommandHandler> commandProcessor;
        /// <summary>
        /// CommandBus that sends commands.
        /// </summary>
        public IBus<CommandDispatcher, ICommandHandler> CommandBus
        {
            get;
            private set;
        }
        /// <summary>
        /// Base constructor.
        /// </summary>
        public CQRSContainer()
            : base()
        {
            DIManager.Registering += DIManager_Registering;
            DIManager.Registered += CQRSContainer_Registered;
            base.Register();
        }

        void DIManager_Registering(object sender, RegisteringEventArgs e)
        {
            e.Export<Guid>();
        }
        private void CQRSContainer_Registered(object sender, RegisteredEventArgs e)
        {
            var serializer = e.ContainerSimple.GetExportedValue<ITextSerializer>();
            serializer.LoadType(typeof(ICommand));

            commandProcessor = e.ContainerSimple.GetExportedValue<MessageProcessor<CommandDispatcher, ICommandHandler>>();
            this.CommandBus = e.ContainerSimple.GetExportedValue<IBus<CommandDispatcher, ICommandHandler>>();
            var all = e.ContainerSimple.GetExportedValues<ICommandHandler>();
            commandProcessor.Register(all);
        }
        /// <summary>
        /// Returns or creates a <see cref="T:RaraAvis.nCubed.CQRS.Core.ProcessManager.IProcessManager"/>.
        /// </summary>
        /// <typeparam name="TProcessManager">A <see cref="T:RaraAvis.nCubed.CQRS.Core.ProcessManager.IProcessManager"/> to be returned.</typeparam>
        /// <returns>A <see cref="T:RaraAvis.nCubed.CQRS.Infrastructure.Repositories.ProcessManagerRepository`1"/> object.</returns>
        public IProcessManagerRepository<TProcessManager> CreateProcessManager<TProcessManager>()
            where TProcessManager : class, IProcessManager
        {
            return base.DIManager.CreateObject<IProcessManagerRepository<TProcessManager>>();
        }
        /// <summary>
        /// All CQRS types.
        /// </summary>
        protected override IEnumerable<TypesElement> AllTypes
        {
            get { return N3Section.Section.CQRS.TypesConfiguration.AllTypes; }
        }
    }
}
