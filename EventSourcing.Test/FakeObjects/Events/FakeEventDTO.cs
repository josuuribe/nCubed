using RaraAvis.nCubed.EventSourcing.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.EventSourcing.Test.FakeObjects.Events
{
    public class FakeEventDto : EventDto
    {
        public FakeEventDto() { }

        public string TestString { get; set; }
    }
}
