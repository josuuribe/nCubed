using RaraAvis.nCubed.Core.Messaging;
using RaraAvis.nCubed.Core.Messaging.Messages;
using System;
using System.ComponentModel.Composition;

namespace RaraAvis.nCubed.CQRS.Core.Commands
{
    /// <summary>
    /// Interface for commands.
    /// </summary>
    [InheritedExport]
    public interface ICommand : IMessage
    {
        /// <summary>
        /// Gets the command identifier.
        /// </summary>
        Guid Id { get; }
    }
}
