using RaraAvis.nCubed.Core.Messaging;
using RaraAvis.nCubed.EventSourcing.Core.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.EventSourcing.Core.RepositoryContracts
{
    /// <summary>
    /// Interface for event source repository.
    /// </summary>
    /// <typeparam name="T">A <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Events.IEventSourced"/> object.</typeparam>
    [InheritedExport(typeof(IEventSourcedRepository<>))]
    public interface IEventSourcedRepository<T> where T : IEventSourced
    {
        /// <summary>
        /// Apply all events for entity given.
        /// </summary>
        /// <param name="entity">Entity storing events.</param>
        void Load(T entity);
        /// <summary>
        /// Saves the event sourced entity with correlationId.
        /// </summary>
        /// <param name="eventSourced">The entity.</param>
        /// <param name="correlationId">A correlation id to use when publishing events.</param>
        void Save(T eventSourced, string correlationId);
        /// <summary>
        /// Saves the event sourced entity without correlationId.
        /// </summary>
        /// <param name="eventSourced">The entity.</param>
        void Save(T eventSourced);
    }
}
