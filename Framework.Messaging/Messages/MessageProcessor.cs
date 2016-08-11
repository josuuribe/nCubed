using RaraAvis.nCubed.Core.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace RaraAvis.nCubed.Core.Messaging.Messages
{
    /// <summary>
    /// Base class for processor messages.
    /// </summary>
    /// <typeparam name="T">A Dispatcher type.</typeparam>
    /// <typeparam name="U">A Handler type.</typeparam>
    [InheritedExport(typeof(MessageProcessor<,>))]
    public sealed class MessageProcessor<T, U>
        where T : IDispatcher<U>, new()
        where U : IHandler
    {
        private readonly IDispatcher<U> messageDispatcher;
        /// <summary>
        /// Constructor with serializer.
        /// </summary>
        [ImportingConstructor]
        public MessageProcessor()
        //: base(contextFactory, serializer)
        {
            this.messageDispatcher = new T();
        }
        /// <summary>
        /// Register a handler inside dispatcher.
        /// </summary>
        /// <param name="handlers">Handler to be register.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Error is intentional")]
        public void Register(IEnumerable<U> handlers)
        {
            foreach (var handler in handlers)
            {
                this.messageDispatcher.Register(handler);
            }
        }
        /// <summary>
        /// Dispatchs the message.
        /// </summary>
        /// <typeparam name="V">Message type.</typeparam>
        /// <param name="message">Message to be processed.</param>
        public void DispatchMessage<V>(Envelope<V> message) where V : IMessage
        {
            this.messageDispatcher.DispatchMessage<V>(message);
        }
    }
}
