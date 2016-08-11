using RaraAvis.nCubed.Core.Messaging.Messages;
using RaraAvis.nCubed.Core.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Messaging.Bus
{
    /// <summary>
    /// Class that sends messages between components.
    /// </summary>
    /// <typeparam name="TDispatcher">The <see cref="T:RaraAvis.nCubed.Core.Messaging.Messages.IDispatcher`1"/> type.</typeparam>
    /// <typeparam name="THandler">The <see cref="T:RaraAvis.nCubed.Core.Messaging.Messages.IHandler"/> handler.</typeparam>
    public class CommonBus<TDispatcher, THandler> : IBus<TDispatcher, THandler>
        where TDispatcher : IDispatcher<THandler>, new()
        where THandler : IHandler
    {
        private MessageProcessor<TDispatcher, THandler> processor;
        /// <summary>
        /// Base constructor.
        /// </summary>
        /// <param name="processor">A <see cref="T:RaraAvis.nCubed.Core.Messaging.Messages.MessageProcessor`2"/>MessageProcessor that dispatchs messages.</param>
        [ImportingConstructor]
        public CommonBus(MessageProcessor<TDispatcher, THandler> processor)
        {
            this.processor = processor;
        }
        /// <summary>
        /// Publish an event with envelope.
        /// </summary>
        /// <typeparam name="T">The message type.</typeparam>
        /// <param name="message">The event type.</param>
        public void Publish<T>(Envelope<T> message) where T : IMessage
        {
            processor.DispatchMessage<T>(message);
        }
        /// <summary>
        /// Publish a messages collection.
        /// </summary>
        /// <typeparam name="T">The message type.</typeparam>
        /// <param name="messages">The event type.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Error is intentional")]
        public void Publish<T>(IEnumerable<Envelope<T>> messages) where T : IMessage
        {
            foreach (var message in messages)
            {
                processor.DispatchMessage<T>(message);
            }
        }
        /// <summary>
        /// Publish an event asynchronously with envelope.
        /// </summary>
        /// <typeparam name="T">The message type.</typeparam>
        /// <param name="message">The event type.</param>
        /// <returns>Task associated.</returns>
        public async Task PublishAsync<T>(Envelope<T> message) where T : IMessage
        {
            await Task.Run(() => Publish<T>(message)).ConfigureAwait(false);
        }
        /// <summary>
        /// Publish a messages collection asynchronously with envelope.
        /// </summary>
        /// <typeparam name="T">The message type.</typeparam>
        /// <param name="messages">The event type.</param>
        /// <returns>Task associated.</returns>
        public async Task PublishAsync<T>(IEnumerable<Envelope<T>> messages) where T : IMessage
        {
            await Task.Run(() => Publish<T>(messages)).ConfigureAwait(false);
        }
        /// <summary>
        /// Publish a messages collection in paralell with envelope.
        /// </summary>
        /// <typeparam name="T">The message type.</typeparam>
        /// <param name="messages">The event type.</param>
        public void PublishParalell<T>(IEnumerable<Envelope<T>> messages) where T : IMessage
        {
            Parallel.ForEach(messages, (x) =>
            {
                this.Publish(x);
            });
        }
        /// <summary>
        /// Publish an event.
        /// </summary>
        /// <typeparam name="T">The message type.</typeparam>
        /// <param name="message">The event type.</param>
        public void Publish<T>(T message) where T : IMessage
        {
            this.Publish<T>(new Envelope<T>(message));
        }
        /// <summary>
        /// Publish a messages collection.
        /// </summary>
        /// <typeparam name="T">The message type.</typeparam>
        /// <param name="messages">The event type.</param>
        public void Publish<T>(IEnumerable<T> messages) where T : IMessage
        {
            this.Publish<T>(messages.Select(x => new Envelope<T>(x)));
        }
        /// <summary>
        /// Publish an event asynchronously.
        /// </summary>
        /// <typeparam name="T">The message type.</typeparam>
        /// <param name="message">The event type.</param>
        /// <returns>Task associated.</returns>
        public async Task PublishAsync<T>(T message) where T : IMessage
        {
            await this.PublishAsync<T>(new Envelope<T>(message));
        }
        /// <summary>
        /// Publish an messages collection asynchronously.
        /// </summary>
        /// <typeparam name="T">The message type.</typeparam>
        /// <param name="messages">The event type.</param>
        /// <returns>Task associated.</returns>
        public async Task PublishAsync<T>(IEnumerable<T> messages) where T : IMessage
        {
            await this.PublishAsync<T>(messages.Select(x => new Envelope<T>(x)));
        }
        /// <summary>
        /// Publish a messages collection in paralell.
        /// </summary>
        /// <typeparam name="T">The msesage type.</typeparam>
        /// <param name="messages">The event type.</param>
        public void PublishParalell<T>(IEnumerable<T> messages) where T : IMessage
        {
            PublishParalell(messages.Select(x => new Envelope<T>(x)));
        }

        #region ·   IDisposable Members ·
        bool disposed = false;
        /// <summary>
        /// <see cref="M:System.IDisposable.Dispose"/>
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// <see cref="M:System.IDisposable.Dispose"/>
        /// </summary>
        /// <param name="disposing">False if unmanaged resources must be disposed, false otherwise.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                this.processor = null;
            }
            disposed = true;
        }
        #endregion
    }
}
