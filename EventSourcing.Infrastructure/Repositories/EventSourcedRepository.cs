using RaraAvis.nCubed.EventSourcing.Core.Entities;
using RaraAvis.nCubed.EventSourcing.Core.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Data.Entity;
using RaraAvis.nCubed.Core.Serialization;
using RaraAvis.nCubed.Core.Expressions;
using RaraAvis.nCubed.EventSourcing.Core.RepositoryContracts;
using RaraAvis.nCubed.Core.Messaging;
using RaraAvis.nCubed.Core.Exceptions;
using RaraAvis.nCubed.EventSourcing.Core.Exceptions;
using RaraAvis.nCubed.Core.Exceptions.Core;
using RaraAvis.nCubed.EventSourcing.Core.Mementos;
using System.Runtime.Caching;
using RaraAvis.nCubed.Core.Messaging.StrategyErrors;
using RaraAvis.nCubed.Core.Infrastructure.StrategyErrors;
using RaraAvis.nCubed.Core.Configurations;
using System.Threading.Tasks;
using System.Configuration;
using RaraAvis.nCubed.Core.Messaging.Bus;
using RaraAvis.nCubed.Core.Messaging.Messages;

namespace RaraAvis.nCubed.EventSourcing.Infrastructure.Repositories
{
    /// <summary>
    /// An implementation of <see cref="T:RaraAvis.nCubed.EventSourcing.Core.RepositoryContracts.IEventSourcedRepository"/>.
    /// </summary>
    public class EventSourcedRepository : IEventSourcedRepository
    {
        private readonly IBus<EventDispatcher, IEventHandler> eventBus;
        private readonly ITextSerializer serializer;
        private readonly ObjectCache cache;
        private readonly MessagingRandomRetry messagingRandomRetry;
        private readonly SqlIncrementalRetry sqlIncrementalRetry;
        /// <summary>
        /// Base constructor.
        /// </summary>
        /// <param name="eventBus">Event Bus.</param>
        /// <param name="serializer">Object that serializes.</param>
        /// <param name="cache">Cache to store objects.</param>
        [ImportingConstructor]
        public EventSourcedRepository([Import]IBus<EventDispatcher, IEventHandler> eventBus, [Import]ITextSerializer serializer, [Import]ObjectCache cache)
        {
            this.eventBus = eventBus;
            this.serializer = serializer;
            this.cache = cache;

            this.messagingRandomRetry = new MessagingRandomRetry();
            this.sqlIncrementalRetry = new SqlIncrementalRetry();
        }
        private static string PartitionKey<T>(Guid id) where T : IEventSourced
        {
            return typeof(T).Name + "_" + id;
        }

        private void AssignMemento<T>(T entity) where T : IEventSourced
        {
            if (typeof(IMementoOriginator).IsAssignableFrom(typeof(T)))
            {
                IMementoOriginator originator = (IMementoOriginator)entity;
                originator.Memento = (IMemento)this.cache.Get(PartitionKey<T>(entity.Id));
            }
        }

        private void SaveMemento<T>(T entity) where T : IEventSourced
        {
            if (typeof(IMementoOriginator).IsAssignableFrom(typeof(T)))
            {
                var memento = (entity as IMementoOriginator).Memento;
                this.cache.Set(PartitionKey<T>(entity.Id),
                    memento,
                    new CacheItemPolicy { AbsoluteExpiration = DateTime.UtcNow.AddMinutes(((ConfigurationManager.GetSection(N3Section.N3_NAME) as N3Section).ES.TypesConfiguration.CacheExpires)) });
            }
        }
        /// <summary>
        /// Load an entity applying all events, using <see cref="RaraAvis.nCubed.EventSourcing.Core.Mementos.IMemento"/> if available. 
        /// </summary>
        /// <typeparam name="T">The entity type to persist.</typeparam>
        /// <param name="entity">Entity to apply events.</param>
        public void Load<T>(T entity) where T : IEventSourced
        {
            CoreExceptionProcessor.ProcessInfrastructure(() =>
            {
                AssignMemento<T>(entity);

                using (var context = new EventSourcingContext())
                {
                    var deserialized = context.Set<EventData>()
                        .Where(x => x.AggregateId == entity.Id && x.AggregateType == typeof(T).Name && x.Version > entity.Version)
                        .OrderBy(x => x.Version)
                        .AsEnumerable()
                        .Select(this.Deserialize)
                        .AsCachedAnyEnumerable();

                    if (deserialized.Any())
                    {
                        entity.ApplyEvents(deserialized);
                    }
                }
            });
        }
        /// <summary>
        /// Save event source object using <see cref="RaraAvis.nCubed.EventSourcing.Core.Mementos.IMemento"/> if available.
        /// </summary>
        /// <typeparam name="T">The entity type to persist.</typeparam>
        /// <param name="eventSourced">A source object.</param>
        /// <param name="correlationId">Related correlation id.</param>
        public void Save<T>(T eventSourced, string correlationId) where T : IEventSourced
        {
            CoreExceptionProcessor.ProcessInfrastructure(() =>
            {
                using (var context = new EventSourcingContext())
                {
                    var eventsSet = context.Set<EventData>();
                    while (eventSourced.Events.Count() > 0)
                    {
                        try
                        {
                            var versionedEvent = eventSourced.Events.Dequeue();
                            versionedEvent.SourceId = versionedEvent.SourceId == Guid.Empty ? eventSourced.Id : versionedEvent.SourceId;
                            EventData ed = this.Serialize(versionedEvent, typeof(T).Name, correlationId);
                            this.messagingRandomRetry.ExecuteAction(() => this.eventBus.Publish(new Envelope<IEvent>(versionedEvent) { CorrelationId = ed.CorrelationId }));
                            eventsSet.Add(ed);
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                this.sqlIncrementalRetry.ExecuteAction(() => { context.SaveChanges(); }); //Save processed store events
                            }
                            catch (Exception exSql)
                            {
                                Exception exSqlOut = null;
                                EventSourcingExceptionProcessor.HandleException(exSql, out exSqlOut);
                                throw exSqlOut;
                            }
                            Exception exOut = null;
                            EventSourcingExceptionProcessor.HandleException(ex, out exOut);
                            throw exOut;
                        }
                    }
                    try
                    {
                        this.sqlIncrementalRetry.ExecuteAction(() => { context.SaveChanges(); });
                        SaveMemento<T>(eventSourced);
                    }
                    catch (Exception ex)
                    {
                        Exception exOut = null;
                        EventSourcingExceptionProcessor.HandleException(ex, out exOut);
                        throw exOut;
                    }
                }
            });
        }
        /// <summary>
        /// Save event source object using <see cref="RaraAvis.nCubed.EventSourcing.Core.Mementos.IMemento"/> if available.
        /// </summary>
        /// <typeparam name="T">The entity type to persist.</typeparam>
        /// <param name="eventSourced">A source object.</param>
        public void Save<T>(T eventSourced) where T : IEventSourced
        {
            this.Save<T>(eventSourced, String.Empty);
        }
        private EventData Serialize(IVersionedEvent e, string typeName, string correlationId)
        {
            EventData serialized;

            string s = this.serializer.Serialize<IVersionedEvent>(e);
            serialized = new EventData
            {
                AggregateId = e.SourceId,
                AggregateType = typeName,
                Version = e.Version,
                Payload = s,
                CorrelationId = correlationId
            };

            return serialized;
        }
        private IVersionedEvent Deserialize(EventData @event)
        {
            return (IVersionedEvent)this.serializer.Deserialize<IVersionedEvent>(@event.Payload);
        }
    }
}

