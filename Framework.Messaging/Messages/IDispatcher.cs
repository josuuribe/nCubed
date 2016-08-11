using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Messaging.Messages
{
    /// <summary>
    /// Interface  that dispatchs objects.
    /// </summary>
    /// <typeparam name="T">Handler that dispatchs this message.</typeparam>
    public interface IDispatcher<T>
        where T : IHandler
    {
        /// <summary>
        /// Message enveloped to be dispatched.
        /// </summary>
        /// <typeparam name="U">Message type to be dispatched.</typeparam>
        /// <param name="message">Messages to be dispatched.</param>
        void DispatchMessage<U>(Envelope<U> message) where U : IMessage;
        /// <summary>
        /// Registers a handler.
        /// </summary>
        /// <param name="handler">Handler to be register.</param>
        void Register(T handler);
    }
}
