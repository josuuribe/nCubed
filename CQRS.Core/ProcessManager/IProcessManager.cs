using RaraAvis.nCubed.Core.Messaging;
using RaraAvis.nCubed.Core.Messaging.Messages;
using RaraAvis.nCubed.CQRS.Core.Commands;
using System;
using System.Collections.Generic;

namespace RaraAvis.nCubed.CQRS.Core.ProcessManager
{
    /// <summary>
    /// Interface implemented by process managers (also known as Sagas in the CQRS community) that 
    /// publish commands to the command bus.
    /// </summary>
    public interface IProcessManager
    {
        /// <summary>
        /// Gets the process manager identifier.
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// Gets a value indicating whether the process manager workflow is completed and the state can be archived.
        /// </summary>
        bool Completed { get; }
        /// <summary>
        /// Gets a collection of commands that need to be sent when the state of the process manager is persisted.
        /// </summary>
        Queue<Envelope<ICommand>> Commands { get; }
    }
}
