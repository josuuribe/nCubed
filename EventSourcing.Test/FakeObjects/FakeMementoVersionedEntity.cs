using RaraAvis.nCubed.EventSourcing.Core.Events;
using RaraAvis.nCubed.EventSourcing.Core.Mementos;
using RaraAvis.nCubed.EventSourcing.Test.FakeObjects.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.EventSourcing.Test.FakeObjects
{
    public class FakeMementoVersionedEntity : IEventSourced, IMementoOriginator
    {
        private Guid id;
        private VersionedWrapper<FakeMementoVersionedEntity> wrapper;
        public string FakeString { get; set; }

        

        public FakeMementoVersionedEntity(Guid id)
            : this()
        {
            this.id = id;
        }

        public FakeMementoVersionedEntity()
        {
            id = Guid.NewGuid();
            wrapper = new VersionedWrapper<FakeMementoVersionedEntity>(this);
            wrapper.Handles<FakeEvent>(OnFakeEvent);
        }

        public Guid Id
        {
            get { return id; }
        }

        public int Version
        {
            get
            {
                return wrapper.Version;
            }
        }

        public Queue<IVersionedEvent> Events
        {
            get { return wrapper.Events; }
        }

        public void ApplyEvents(IEnumerable<IVersionedEvent> events)
        {
            wrapper.ApplyEvents(events);
        }

        private void OnFakeEvent(FakeEvent @event)
        {
            this.FakeString += @event.TestString;
        }

        public void RaiseFakeEvent(FakeEvent @event)
        {
            wrapper.Update(@event);
        }

        public IMemento Memento
        {
            get
            {
                return new FakeMemento()
                {
                    Version = this.Version,
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
