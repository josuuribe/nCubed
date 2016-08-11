using RaraAvis.nCubed.Core.Messaging;
using RaraAvis.nCubed.Core.Messaging.Messages;
using System;
using System.ComponentModel.Composition;

namespace RaraAvis.nCubed.EventSourcing.Core.Events
{
    /// <summary>
    /// Represents an event message.
    /// </summary>
    [InheritedExport(typeof(IEvent))]
    public interface IEvent : IMessage
    {
        /// <summary>
        /// Gets the identifier of the source originating the event.
        /// </summary>
        Guid SourceId { get; set; }
    }
}
