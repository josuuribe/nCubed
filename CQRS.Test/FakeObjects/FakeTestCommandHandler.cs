using RaraAvis.nCubed.CQRS.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.CQRS.Test.FakeObjects
{
    public class FakeTestCommandHandler : ICommandHandler<FakeCommand>
    {
        public void Handle(FakeCommand command)
        {
        }

        public void Dispose()
        {            
        }
    }
}
