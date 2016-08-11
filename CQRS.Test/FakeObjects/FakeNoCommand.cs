using RaraAvis.nCubed.CQRS.Core;
using RaraAvis.nCubed.CQRS.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.CQRS.Test.FakeObjects
{
    public class FakeNoCommand : ICommand
    {
        public Guid Id
        {
            get { return Guid.NewGuid(); }
        }

        public string TestNoString { get; set; }
    }
}
