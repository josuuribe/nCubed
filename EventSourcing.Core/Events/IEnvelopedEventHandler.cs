using RaraAvis.nCubed.Core.Messaging;
using RaraAvis.nCubed.Core.Messaging.Messages;
using RaraAvis.nCubed.EventSourcing.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.EventSourcing.Core.Events
{
    /// <summary>
    /// Interface needed for events wrapped by envelopes.
    /// </summary>
    /// <typeparam name="T">A <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Events.IEvent"/> object.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
    public interface IEnvelopedEventHandler<T> : IEventHandler
        where T : IEvent
    {
        /// <summary>
        /// Handles enveloped messages.
        /// </summary>
        /// <param name="envelope">A <see cref="T:RaraAvis.nCubed.Core.Messaging.Messages.Envelope`1"/> object.</param>
        void Handle(Envelope<T> envelope);
    }
}
