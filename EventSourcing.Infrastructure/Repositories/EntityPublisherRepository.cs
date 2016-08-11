using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Optimissa.nCubed.EventSourcing.Core.Events;
using Optimissa.nCubed.Core.Messaging;
using Optimissa.nCubed.Core.Exceptions;
using Optimissa.nCubed.Core.Exceptions.Core;


namespace Optimissa.nCubed.EventSourcing.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for entities that must raises events before saved
    /// </summary>
    /// <typeparam name="TContext">Context where events be saved.</typeparam>
    /// <typeparam name="TEntity">Class that implements <see cref="T:Optimissa.nCubed.EventSourcing.Core.Events.IEventPublisher"/></typeparam>
    internal class EntityPublisherRepository<TContext, TEntity>
        where TContext : EventSourcingContext, new()
        where TEntity : class,IEventPublisher
    {
        /// <summary>
        /// An event bus.
        /// </summary>
        IBus eventBus;

        #region ·   Constructor ·
        /// <summary>
        /// Create a new instance of repository
        /// </summary>
        /// <param name="eventBus">Associated EventBus</param>
        public EntityPublisherRepository(IBus eventBus)
            : base()
        {
            this.eventBus = eventBus;
        }
        #endregion

        #region ·   Methods ·
        /// <summary>
        /// Save entities.
        /// </summary>
        /// <param name="versionedEntity"></param>
        public void Save(TEntity versionedEntity)
        {
            CoreExceptionProcessor.ProcessInfrastructure(() =>
                {
                    using (var tc = new TContext())
                    {
                        tc.Set<TEntity>().Add(versionedEntity);
                        tc.SaveChanges();

                        var eventPublisher = versionedEntity as IEventPublisher;
                        this.eventBus.Publish(eventPublisher.Events.Select(x => new Envelope<IEvent>(x)));
                    }
                });
        }
        #endregion
    }
}
