using RaraAvis.nCubed.Core.Messaging;
using RaraAvis.nCubed.Core.Messaging.Messages;
using RaraAvis.nCubed.EventSourcing.Core.Events;
using RaraAvis.nCubed.EventSourcing.Test.FakeObjects;
using RaraAvis.nCubed.EventSourcing.Test.FakeObjects.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.EventSourcing.Test.Handlers
{
    public class TestEventHandler : 
        IEventHandler<FakeEvent>, 
        IEnvelopedEventHandler<FakeEvent>,
        IEventHandler<FakeFailedEvent>,
        IEventHandler<IEvent>
    {
        public void Handle(FakeEvent eventMesage)
        {
            
        }

        public void Handle(Envelope<FakeEvent> envelope)
        {
            Store.EnvelopeId = envelope.CorrelationId;
            Store.Id = envelope.Body.SourceId;
        }

        public void Handle(FakeFailedEvent eventMesage)
        {
            Store.EnvelopeId = "Failed";

            throw new Exception(eventMesage.GetType().Name);
        }

        public void Handle(IEvent eventMesage)
        {
            Store.Message = "Generic";
        }
    }
}
