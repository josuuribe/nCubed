using RaraAvis.nCubed.EventSourcing.Core.Events;
using RaraAvis.nCubed.EventSourcing.Test.FakeObjects.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.EventSourcing.Test.FakeObjects
{
    public class FakeVersionedEntity : IEventSourced
    {
        private Guid id;
        private VersionedWrapper<FakeVersionedEntity> wrapper;
        public string FakeString { get; set; }

        

        public FakeVersionedEntity(Guid id)
            : this()
        {
            this.id = id;
        }

        public FakeVersionedEntity()
        {
            id = Guid.NewGuid();
            wrapper = new VersionedWrapper<FakeVersionedEntity>(this);
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
    }
}
