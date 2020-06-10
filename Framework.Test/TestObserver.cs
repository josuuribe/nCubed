using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Test
{
    public class TestObserver : IObserver<EventEntry>
    {
        public EventEntry Entry
        {
            get; private set;
        }

        public void OnCompleted()
        {

        }

        public void OnError(Exception error)
        {
            throw new Exception("Error logging");
        }

        public void OnNext(EventEntry value)
        {
            this.Entry = value;
        }
    }
}
