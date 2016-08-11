using Optimissa.nCubed.CQRS.Core.Entities;
using Optimissa.nCubed.CQRS.Core.ProcessManager.Fakes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimissa.nCubed.CQRS.Test.FakeObjects.Infrastructure
{
    public class FakeUndispatchedMessageDbSet : FakeDbSet<UndispatchedMessage>
    {
        public FakeUndispatchedMessageDbSet() { }

        public override UndispatchedMessage Find(params object[] keyValues)
        {
            Guid guid = (Guid)keyValues.ElementAt(0);
            return _data.FirstOrDefault(p => p.Id == guid);
        }
    }
}
