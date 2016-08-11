using RaraAvis.nCubed.Core.Messaging;
using RaraAvis.nCubed.Core.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.CQRS.Core.Commands
{
    /// <summary>
    /// Marker interface that makes it easier to discover handlers via reflection.
    /// </summary>
    [InheritedExport]
    public interface ICommandHandler : IHandler
    { }
    /// <summary>
    /// Interface implemented by command handlers.
    /// </summary>
    /// <typeparam name="T">An <see cref="T:RaraAvis.nCubed.CQRS.Core.Commands.ICommand"/> object.</typeparam>
    public interface ICommandHandler<T> : ICommandHandler
        where T : ICommand
    {
        /// <summary>
        /// Handles a command.
        /// </summary>
        /// <param name="command">Command to handle.</param>
        void Handle(T command);
    }
}
