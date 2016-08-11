

using RaraAvis.nCubed.Core.Messaging.Messages;
using RaraAvis.nCubed.Core.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Messaging.Bus
{
    /// <summary>
    /// Interface for Bus classes.
    /// </summary>
    /// <typeparam name="TDispatcher">A <see cref="T:RaraAvis.nCubed.Core.Messaging.Messages.IDispatcher`1"/> object.</typeparam>
    /// <typeparam name="THandler">A <see cref="T:RaraAvis.nCubed.Core.Messaging.Messages.IHandler"/> object.</typeparam>
    [InheritedExport(typeof(IBus<,>))]
    public interface IBus<TDispatcher, THandler> : IDisposable
        where TDispatcher : IDispatcher<THandler>, new()
        where THandler : IHandler
    {
        /// <summary>
        /// Publish an event.
        /// </summary>
        /// <typeparam name="T">The message type.</typeparam>
        /// <param name="message">The event type.</param>
        void Publish<T>(T message) where T : IMessage;
        /// <summary>
        /// Publish a events collection.
        /// </summary>
        /// <typeparam name="T">The message type.</typeparam>
        /// <param name="messages">The event type.</param>
        void Publish<T>(IEnumerable<T> messages) where T : IMessage;
        /// <summary>
        /// Publish an event asynchronously.
        /// </summary>
        /// <typeparam name="T">The message type.</typeparam>
        /// <param name="message">The event type.</param>
        /// <returns>Task associated.</returns>
        Task PublishAsync<T>(T message) where T : IMessage;
        /// <summary>
        /// Publish an events collection asynchronously.
        /// </summary>
        /// <typeparam name="T">The message type.</typeparam>
        /// <param name="messages">The event type.</param>
        /// <returns>Task associated.</returns>
        Task PublishAsync<T>(IEnumerable<T> messages) where T : IMessage;
        /// <summary>
        /// Publish a events collection in paralell.
        /// </summary>
        /// <typeparam name="T">The msesage type.</typeparam>
        /// <param name="messages">The event type.</param>
        void PublishParalell<T>(IEnumerable<T> messages) where T : IMessage;
        /// <summary>
        /// Publish an event with envelope.
        /// </summary>
        /// <typeparam name="T">The message type.</typeparam>
        /// <param name="message">The event type.</param>
        void Publish<T>(Envelope<T> message) where T : IMessage;
        /// <summary>
        /// Publish a events collection.
        /// </summary>
        /// <typeparam name="T">The message type.</typeparam>
        /// <param name="messages">The event type.</param>
        void Publish<T>(IEnumerable<Envelope<T>> messages) where T : IMessage;
        /// <summary>
        /// Publish an event asynchronously with envelope.
        /// </summary>
        /// <typeparam name="T">The message type.</typeparam>
        /// <param name="message">The event type.</param>
        /// <returns>Task associated.</returns>
        Task PublishAsync<T>(Envelope<T> message) where T : IMessage;
        /// <summary>
        /// Publish a events collection asynchronously with envelope.
        /// </summary>
        /// <typeparam name="T">The message type.</typeparam>
        /// <param name="messages">The event type.</param>
        /// <returns>Task associated.</returns>
        Task PublishAsync<T>(IEnumerable<Envelope<T>> messages) where T : IMessage;
        /// <summary>
        /// Publish a events collection in paralell with envelope.
        /// </summary>
        /// <typeparam name="T">The message type.</typeparam>
        /// <param name="messages">The event type.</param>
        void PublishParalell<T>(IEnumerable<Envelope<T>> messages) where T : IMessage;
    }
}
