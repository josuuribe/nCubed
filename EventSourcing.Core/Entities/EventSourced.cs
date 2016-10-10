using RaraAvis.nCubed.EventSourcing.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.EventSourcing.Core.Entities
{
    /// <summary>
    /// Base class for entities that supports EventSourced.
    /// </summary>
    public class EventSourced : IEventSourced
    {
        /// <summary>
        /// Dictionary with types and actions.
        /// </summary>
        [NonSerialized]
        private readonly Dictionary<Type, Action<IVersionedEvent>> handlers = new Dictionary<Type, Action<IVersionedEvent>>();
        /// <summary>
        /// Gets the collection of new events since the entity was loaded, as a consequence of command handling.
        /// </summary>
        public Queue<IVersionedEvent> Events
        {
            get;
            private set;
        }
        /// <summary>
        /// Id for this EventSourced.
        /// </summary>
        public Guid Id
        {
            get;
            internal set;
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
        /// Constructor with a given Id.
        /// </summary>
        /// <param name="id">The id to be initialized.</param>
        public EventSourced(Guid id)
        {
            this.Id = id;
            this.Version = 0;
            this.Events = new Queue<IVersionedEvent>();
        }
        /// <summary>
        /// Default constructor
        /// </summary>
        public EventSourced() : this(Guid.NewGuid())
        {
        }
        /// <summary>
        /// Configures a handler for an event. 
        /// </summary>
        /// <typeparam name="TEvent">A <see cref="IEvent"/>.</typeparam>
        /// <param name="handler">Handler to handle this event.</param>
        public void Handles<TEvent>(Action<TEvent> handler)
            where TEvent : IEvent
        {
            this.handlers.Add(typeof(TEvent), @event => handler((TEvent)@event));
        }
        /// <summary>
        /// Load from a sequence <see cref="IVersionedEvent"/> given.
        /// </summary>
        /// <param name="pastEvents">A sequence objects given.</param>
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
        /// <typeparam name="TVersionedEvent">A <see cref="IVersionedEvent"/>.</typeparam>
        /// <param name="versionedEvent">A <see cref="IVersionedEvent"/> object.</param>
        public void Update<TVersionedEvent>(TVersionedEvent versionedEvent)
            where TVersionedEvent : IVersionedEvent
        {
            versionedEvent.SourceId = this.Id;
            versionedEvent.Version = Version + 1;
            this.handlers[versionedEvent.GetType()].Invoke(versionedEvent);
            Version = versionedEvent.Version;
            this.Events.Enqueue(versionedEvent);
        }
    }
}
