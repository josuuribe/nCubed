using RaraAvis.nCubed.DDD.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.DDD.Test.FakeObjects
{
    class SelfReference
        : ValueObject<SelfReference>
    {
        public SelfReference()
        {
        }
        public SelfReference(SelfReference value)
        {
            Value = value;
        }
        public SelfReference Value { get; set; }
    }
}
