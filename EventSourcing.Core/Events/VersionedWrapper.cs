using RaraAvis.nCubed.EventSourcing.Core.Events;
using RaraAvis.nCubed.EventSourcing.Core.Mementos;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace RaraAvis.nCubed.EventSourcing.Core.Events
{
    /// <summary>
    /// Base class for entities.
    /// </summary>
    /// <typeparam name="T">A <see cref="RaraAvis.nCubed.EventSourcing.Core.Events.IEventSourced" /> type to wrap.</typeparam>
    [Serializable]
    public sealed class VersionedWrapper<T>
        where T : IEventSourced
    {
        private T entity;
        /// <summary>
        /// Queue with <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Events.IVersionedEvent"/>.
        /// </summary>
        private Queue<IVersionedEvent> events = new Queue<IVersionedEvent>();

        #region ·   Constructors    ·

        private VersionedWrapper()
        {
            this.Version = 0;
        }
        /// <summary>
        /// Constructor that wraps an entity.
        /// </summary>
        /// <param name="entity">Entity wrapped to be versioned.</param>
        public VersionedWrapper(T entity)
            : this()
        {
            this.entity = entity;
            this.events = new Queue<IVersionedEvent>();
        }
        #endregion

        #region ·   Fields  ·
        /// <summary>
        /// Dictionary with types and actions.
        /// </summary>
        [NonSerialized]
        private readonly Dictionary<Type, Action<IVersionedEvent>> handlers = new Dictionary<Type, Action<IVersionedEvent>>();
        #endregion

        #region ·   EventSource ·
        /// <summary>
        /// Gets the collection of new events since the entity was loaded, as a consequence of command handling.
        /// </summary>
        public Queue<IVersionedEvent> Events
        {
            get { return this.events; }
        }
        /// <summary>
        /// Gets the entity's version. As the entity is being updated and events being generated, the version is incremented.
        /// </summary>
        public int Version
        {
            get;
            private set;
        }
        /// <summary>
        /// Configures a handler for an event. 
        /// </summary>
        /// <typeparam name="TEvent">A <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Events.IEvent"/>.</typeparam>
        /// <param name="handler">Handler to handle this event.</param>
        public void Handles<TEvent>(Action<TEvent> handler)
            where TEvent : IEvent
        {
            this.handlers.Add(typeof(TEvent), @event => handler((TEvent)@event));
        }
        /// <summary>
        /// Load from a sequence <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Events.IVersionedEvent"/> given.
        /// </summary>
        /// <param name="pastEvents">A sequence objects given.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Error is intentional")]
        public void ApplyEvents(IEnumerable<IVersionedEvent> pastEvents)
        {
            foreach (var e in pastEvents)
            {
                this.handlers[e.GetType()].Invoke(e);
                Version = e.Version;
            }
        }
        /// <summary>
        /// Update with specified version.
        /// </summary>
        /// <param name="versionedEvent">A <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Events.IVersionedEvent"/> object.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Error is intentional")]
        public void Update(IVersionedEvent versionedEvent)
        {
            versionedEvent.SourceId = entity.Id;
            versionedEvent.Version = Version + 1;
            this.handlers[versionedEvent.GetType()].Invoke(versionedEvent);
            Version = versionedEvent.Version;
            this.events.Enqueue(versionedEvent);
        }
        #endregion
    }
}
