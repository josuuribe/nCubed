using RaraAvis.nCubed.CQRS.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.CQRS.Test.FakeObjects
{
    public class FakeCommandDto : CommandDto
    {
        public FakeCommandDto() { }
        public string TestString { get; set; }
    }
}
