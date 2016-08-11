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
    /// An implementation of <see cref="T:RaraAvis.nCubed.EventSourcing.Core.RepositoryContracts.IEventSourcedRepository`1"/>.
    /// </summary>
    /// <typeparam name="T">The entity type to persist.</typeparam>
    public class EventSourcedRepository<T> : IEventSourcedRepository<T>
    where T : IEventSourced
    {
        private readonly string sourceType = typeof(T).Name;
        private readonly IBus<EventDispatcher, IEventHandler> eventBus;
        private readonly ITextSerializer serializer;
        private readonly ObjectCache cache;
        private readonly MessagingRandomRetry messagingRandomRetry;
        private readonly SqlIncrementalRetry sqlIncrementalRetry;
        private readonly Action<T> assignMemento;
        private readonly Action<T> saveMemento;
        /// <summary>
        /// Base constructor.
        /// </summary>
        /// <param name="eventBus">Event Bus.</param>
        /// <param name="serializer">Serializer.</param>
        /// <param name="cache">Cache to store objects.</param>
        [ImportingConstructor]
        public EventSourcedRepository([Import]IBus<EventDispatcher, IEventHandler> eventBus, [Import]ITextSerializer serializer, [Import]ObjectCache cache)
        {
            this.eventBus = eventBus;
            this.serializer = serializer;
            this.cache = cache;

            this.messagingRandomRetry = new MessagingRandomRetry();
            this.sqlIncrementalRetry = new SqlIncrementalRetry();

            if (typeof(IMementoOriginator).IsAssignableFrom(typeof(T)))
            {
                assignMemento = (originator) =>
                    {
                        (originator as IMementoOriginator).Memento = (IMemento)this.cache.Get(PartitionKey(originator.Id));
                    };

                saveMemento = (originator) =>
                    {
                        var memento = (originator as IMementoOriginator).Memento;
                        this.cache.Set(PartitionKey(originator.Id),
                            memento,
                            new CacheItemPolicy { AbsoluteExpiration = DateTime.UtcNow.AddMinutes(((ConfigurationManager.GetSection(N3Section.N3_NAME) as N3Section).ES.TypesConfiguration.CacheExpires)) });
                    };
            }
            else
            {
                assignMemento = (originator) => { };
                saveMemento = (originator) => { };
            }
        }
        private string PartitionKey(Guid id)
        {
            return sourceType + "_" + id;
        }
        /// <summary>
        /// Load an entity applying all events, using <see cref="RaraAvis.nCubed.EventSourcing.Core.Mementos.IMemento"/> if available. 
        /// </summary>
        /// <param name="entity">Entity to apply events.</param>
        public void Load(T entity)
        {
            CoreExceptionProcessor.ProcessInfrastructure(() =>
            {
                assignMemento(entity);

                using (var context = new EventSourcingContext())
                {
                    var deserialized = context.Set<EventData>()
                        .Where(x => x.AggregateId == entity.Id && x.AggregateType == sourceType && x.Version > entity.Version)
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
        /// <param name="eventSourced">A source object.</param>
        /// <param name="correlationId">Related correlation id.</param>
        public void Save(T eventSourced, string correlationId)
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
                            this.messagingRandomRetry.ExecuteAction(() => this.eventBus.Publish(new Envelope<IEvent>(versionedEvent) { CorrelationId = correlationId }));
                            eventsSet.Add(this.Serialize(versionedEvent, correlationId));
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
                        saveMemento(eventSourced);
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
        /// <param name="eventSourced">A source object.</param>
        public void Save(T eventSourced)
        {
            this.Save(eventSourced, String.Empty);
        }
        private EventData Serialize(IVersionedEvent e, string correlationId)
        {
            EventData serialized;

            string s = this.serializer.Serialize<IVersionedEvent>(e);
            serialized = new EventData
            {
                AggregateId = e.SourceId,
                AggregateType = sourceType,
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

