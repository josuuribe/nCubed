using RaraAvis.nCubed.Core.Messaging;
using RaraAvis.nCubed.Core.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.CQRS.Core.Commands
{
    /// <summary>
    /// Interface needed for commands wrapped by envelopes.
    /// </summary>
    /// <typeparam name="T">An <see cref="T:RaraAvis.nCubed.Core.Messaging.Messages.Envelope{T}"/> with <see cref="T:RaraAvis.nCubed.CQRS.Core.Commands.ICommand"/> object.</typeparam>
    internal interface IEnvelopedCommandHandler<T> : ICommandHandler
        where T : ICommand
    {
        /// <summary>
        /// Handles envelopes with ICommand.
        /// </summary>
        /// <param name="envelope">Envelope object storing a ICommand object.</param>
        void Handle(Envelope<T> envelope);
    }
}
