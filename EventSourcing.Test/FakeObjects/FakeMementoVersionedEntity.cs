using RaraAvis.nCubed.EventSourcing.Core.Events;
using RaraAvis.nCubed.EventSourcing.Core.Mementos;
using RaraAvis.nCubed.EventSourcing.Test.FakeObjects.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaraAvis.nCubed.EventSourcing.Core.Services;
using RaraAvis.nCubed.EventSourcing.Core.Entities;

namespace RaraAvis.nCubed.EventSourcing.Test.FakeObjects
{
    public class FakeMementoVersionedEntity : IEventSourced, IMementoOriginator
    {
        private readonly EventSourced eventSourced;

        public string FakeString { get; set; }

        private EventSourced EventSourced
        {
            get
            {
                return eventSourced;
            }
        }

        public Guid Id
        {
            get
            {
                return EventSourced.Id;
            }
        }

        public int Version
        {
            get
            {
                return EventSourced.Version;
            }
        }

        public Queue<IVersionedEvent> Events
        {
            get
            {
                return EventSourced.Events;
            }
        }

        public FakeMementoVersionedEntity(Guid id)
        {
            this.eventSourced = new EventSourced(id);
            this.EventSourced.Handles<FakeEvent>(OnFakeEvent);
        }

        public FakeMementoVersionedEntity()
            : this(Guid.NewGuid())
        {

        }

        public void RaiseFakeEvent(FakeEvent @event)
        {
            this.EventSourced.Update(@event);
        }

        private void OnFakeEvent(FakeEvent @event)
        {
            this.FakeString += @event.TestString;
        }

        public void ApplyEvents(IEnumerable<IVersionedEvent> events)
        {
            EventSourced.ApplyEvents(events);
        }

        public IMemento Memento
        {
            get
            {
                return new FakeMemento()
                {
                    Version = this.EventSourced.Version,
                    FakeString = this.FakeString
                };
            }
            set
            {
                FakeMemento fm = value as FakeMemento;
                this.FakeString = this.FakeString;
            }
        }

        public class FakeMemento : IMemento
        {
            public string FakeString { get; set; }

            public int Version
            {
                get;
                internal set;
            }
        }
    }
}
