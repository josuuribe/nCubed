using RaraAvis.nCubed.EventSourcing.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.EventSourcing.Test.FakeObjects.Events
{
    public class FakeEvent : IVersionedEvent
    {
        public FakeEvent() { }

        public Guid SourceId
        {
            get;
            set;
        }

        public string TestString { get; set; }

        public int Version
        {
            get;set;
        }
    }
}
