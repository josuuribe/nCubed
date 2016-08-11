using RaraAvis.nCubed.Core.Logging;
using RaraAvis.nCubed.Core.Messaging;
using RaraAvis.nCubed.Core.Messaging.Messages;
using RaraAvis.nCubed.CQRS.Core.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RaraAvis.nCubed.CQRS.Core.Commands
{
    /// <summary>
    /// Class for dispatch commands.
    /// </summary>
    public sealed class CommandDispatcher : IDispatcher<ICommandHandler>
    {
        private Dictionary<Type, ICommandHandler> handlers = new Dictionary<Type, ICommandHandler>();
        /// <summary>
        /// Registers the specified command handler.
        /// </summary>
        /// <param name="handler">Handler to register.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification="Error is intentional")]
        public void Register(ICommandHandler handler)
        {
            var genericHandler = typeof(ICommandHandler<>);
            var supportedCommandTypes = handler.GetType()
                .GetInterfaces()
                .Where(iface => iface.IsGenericType && iface.GetGenericTypeDefinition() == genericHandler)
                .Select(iface => iface.GetGenericArguments()[0])
                .ToList();

            if (handlers.Keys.Any(registeredType => supportedCommandTypes.Contains(registeredType)))
                throw new ArgumentException("The command handled by the received handler already has a registered handler.");

            foreach (var commandType in supportedCommandTypes)
            {// Register this handler for each of he handled types.
                this.handlers.Add(commandType, handler);
            }
        }
        /// <summary>
        /// Class that dispatchs a single <see cref="T:RaraAvis.nCubed.Core.Messaging.Messages.Envelope"/> object.
        /// </summary>
        /// <typeparam name="T">A <see cref="T:RaraAvis.nCubed.Core.Messaging.Messages.IMessage"/> object.</typeparam>
        /// <param name="message">A <see cref="T:RaraAvis.nCubed.Core.Messaging.Messages.Envelope"/> object.</param>
        /// <returns>True if handler found, false otherwise.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Error is intentional, otherwise constructor is implicit and with no default arguments, always must be a body.")]
        public void DispatchMessage<T>(Envelope<T> message) where T : IMessage
        {
            var commandType = message.Body.GetType();
            ICommandHandler handler = null;

            if (this.handlers.TryGetValue(commandType, out handler))
            {
                ((dynamic)handler).Handle((dynamic)message.Body);
            }
            else
            {
                FrameworkLogging.LogMessage(String.Format(System.Globalization.CultureInfo.InvariantCulture, Resources.HandlerNotFound, commandType.Name));
            }
        }
    }
}
