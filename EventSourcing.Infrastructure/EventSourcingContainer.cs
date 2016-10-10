using RaraAvis.nCubed.Core.Configurations;
using RaraAvis.nCubed.Core.Containers;
using RaraAvis.nCubed.Core.Messaging.Bus;
using RaraAvis.nCubed.Core.Messaging.Messages;
using RaraAvis.nCubed.Core.Serialization;
using RaraAvis.nCubed.EventSourcing.Core.Events;
using RaraAvis.nCubed.EventSourcing.Core.RepositoryContracts;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.Caching;
using System.ComponentModel.Composition.Registration;
using RaraAvis.nCubed.Core.Configurations.Common.Types;
using System.Collections.Generic;
using RaraAvis.nCubed.Core.Containers.DI;
using System;

namespace RaraAvis.nCubed.EventSourcing.Infrastructure
{
    /// <summary>
    /// Class that manages EventSourcing types.
    /// </summary>
    /// <typeparam name="T">A <see cref="ITextSerializer"/> object.</typeparam>
    public sealed class EventSourcingContainer<T> : SerializerContainer<T>
        where T : ITextSerializer, new()
    {
        private MessageProcessor<EventDispatcher, IEventHandler> eventProcessor;
        /// <summary>
        /// Event bus that sends events.
        /// </summary>
        public IBus<EventDispatcher, IEventHandler> EventBus
        {
            get;
            private set;
        }
        /// <summary>
        /// Base constructor.
        /// </summary>
        public EventSourcingContainer()
            : base()
        {
            DIManager.Registering += DIManager_Registering;
            DIManager.Registered += EventSourcingContainer_Registered;
            base.Register();
        }

        void DIManager_Registering(object sender, RegisteringEventArgs e)
        {
            e.Export<Guid>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "MEF")]
        private void EventSourcingContainer_Registered(object sender, RegisteredEventArgs e)
        {//Needed for EventSourcedRepository<,>
            var serializer = e.ContainerSimple.GetExportedValue<ITextSerializer>();

            serializer.LoadType(typeof(IVersionedEvent));
            serializer.LoadType(typeof(IEvent));

            e.ContainerSimple.ComposeExportedValue<ObjectCache>(new MemoryCache("EventBus"));
            eventProcessor = e.ContainerSimple.GetExportedValue<MessageProcessor<EventDispatcher, IEventHandler>>();

            this.EventBus = e.ContainerSimple.GetExportedValue<IBus<EventDispatcher, IEventHandler>>();
            eventProcessor.Register(e.ContainerSimple.GetExportedValues<IEventHandler>());
        }
        /// <summary>
        /// Returns or creates a <see cref="T:RaraAvis.nCubed.EventSourcing.Core.RepositoryContracts.IEventSourcedRepository"/>.  
        /// </summary>
        /// <returns>A <see cref="T:RaraAvis.nCubed.EventSourcing.Core.RepositoryContracts.IEventSourcedRepository"/>.</returns>
        public IEventSourcedRepository CreateEventSourced()
        {
            return base.DIManager.CreateRequiredObject<IEventSourcedRepository>();
        }
        /// <summary>
        /// All Event Sourcing types.
        /// </summary>
        protected override IEnumerable<TypesElement> AllTypes
        {
            get
            {
                return N3Section.Section.ES.TypesConfiguration.AllTypes;
            }
        }
    }
}
