using RaraAvis.nCubed.DDD.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.DDD.Test.FakeObjects
{
    public class SampleEntity
        : Entity
    {
        public SampleEntity(Guid guid) : base(guid) { }

        public SampleEntity() { }
        public string SampleProperty { get; set; }
    }
}
