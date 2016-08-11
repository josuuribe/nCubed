using System;
using System.Runtime.Serialization;

namespace RaraAvis.nCubed.Core.Messaging.Messages
{
    /// <summary>
    /// Static factory class for <see cref="Envelope{T}"/>.
    /// </summary>
    [Serializable]
    public abstract class Envelope
    {
        /// <summary>
        /// Creates an envelope for the given body.
        /// </summary>
        /// <typeparam name="T">Create a message with default envelope.</typeparam>
        /// <param name="body">Message inside envelope.</param>
        /// <returns>The enveloped message.</returns>
        public static Envelope<T> Create<T>(T body) where T : IMessage
        {
            return new Envelope<T>(body);
        }
    }
    /// <summary>
    /// Provides the envelope for an object that will be sent to a bus.
    /// </summary>
    /// <typeparam name="T">Message type to envelope.</typeparam>
    [Serializable]
    public class Envelope<T> : Envelope where T : IMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Envelope{T}"/> class.
        /// </summary>
        /// <param name="body">Message body.</param>
        public Envelope(T body)
        {
            this.Body = body;
        }
        /// <summary>
        /// Gets the body.
        /// </summary>
        public T Body { get; private set; }

        /// <summary>
        /// Gets or sets the delay for sending, enqueing or processing the body.
        /// </summary>
        public TimeSpan Delay { get; set; }

        /// <summary>
        /// Gets or sets the time to live for the message in the queue.
        /// </summary>
        public TimeSpan TimeToLive { get; set; }

        /// <summary>
        /// Gets the correlation id.
        /// </summary>
        public string CorrelationId { get; set; }

        /// <summary>
        /// Gets the correlation id.
        /// </summary>
        public string MessageId { get; set; }

        /// <summary>
        /// Crates an <see cref="T:RaraAvis.nCubed.Core.Messaging.Messages.Envelope"/> object.
        /// </summary>
        /// <param name="body">Body for the envelope.</param>
        /// <returns>An <see cref="T:RaraAvis.nCubed.Core.Messaging.Messages.Envelope"/> object.</returns>
        public static implicit operator Envelope<T>(T body)
        {
            return Envelope.Create(body);
        }
    }
}
