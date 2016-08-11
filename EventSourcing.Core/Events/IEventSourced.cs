using RaraAvis.nCubed.EventSourcing.Core.Mementos;
using System;
using System.Collections.Generic;

namespace RaraAvis.nCubed.EventSourcing.Core.Events
{
    /// <summary>
    /// Represents an identifiable entity that is event sourced.
    /// </summary>
    public interface IEventSourced
    {
        /// <summary>
        /// Gets the entity identifier.
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// Gets the entity's version. As the entity is being updated and events being generated, the version is incremented.
        /// </summary>
        int Version { get; }
        /// <summary>
        /// Gets the collection of new events since the entity was loaded, as a consequence of command handling.
        /// </summary>
        Queue<IVersionedEvent> Events { get; }
        /// <summary>
        /// Apply a sequence of events for a given object.
        /// </summary>
        /// <param name="events">Sequence of objects to be processed.</param>
        void ApplyEvents(IEnumerable<IVersionedEvent> events);
    }
}
