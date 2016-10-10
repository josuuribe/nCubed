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
    [InheritedExport(typeof(IEventSourcedRepository))]
    public interface IEventSourcedRepository
    {
        /// <summary>
        /// Apply all events for entity given.
        /// </summary>
        /// <typeparam name="T">A <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Events.IEventSourced"/> object.</typeparam>
        /// <param name="entity">Entity storing events.</param>
        void Load<T>(T entity) where T: IEventSourced;
        /// <summary>
        /// Saves the event sourced entity with correlationId.
        /// </summary>
        /// <typeparam name="T">A <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Events.IEventSourced"/> object.</typeparam>
        /// <param name="eventSourced">The entity.</param>
        /// <param name="correlationId">A correlation id to use when publishing events.</param>
        void Save<T>(T eventSourced, string correlationId) where T : IEventSourced;
        /// <summary>
        /// Saves the event sourced entity without correlationId.
        /// </summary>
        /// <typeparam name="T">A <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Events.IEventSourced"/> object.</typeparam>
        /// <param name="eventSourced">The entity.</param>
        void Save<T>(T eventSourced) where T : IEventSourced;
    }
}
