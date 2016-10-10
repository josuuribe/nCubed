using RaraAvis.nCubed.EventSourcing.Core.Entities;
using RaraAvis.nCubed.EventSourcing.Core.Events;
using RaraAvis.nCubed.EventSourcing.Test.FakeObjects.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.EventSourcing.Test.FakeObjects
{
    public class FakeSimpleEntity : IEventSourced
    {
        private EventSourced EventSourced { get; set; }
        public string FakeString { get; set; }
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

        public FakeSimpleEntity()
        {
            this.EventSourced = new EventSourced();
            this.EventSourced.Handles<FakeEvent>(OnFakeEvent);
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
    }
}
