using RaraAvis.nCubed.Core.Messaging;
using RaraAvis.nCubed.Core.Messaging.Messages;
using System;
using System.ComponentModel.Composition;

namespace RaraAvis.nCubed.EventSourcing.Core.Events
{
    /// <summary>
    /// Marker interface that makes it easier to discover handlers via reflection.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix"), InheritedExport]
    public interface IEventHandler : IHandler { }
    /// <summary>
    /// Interface implemented by event handlers.
    /// </summary>
    /// <typeparam name="T">An <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Events.IEvent"/> object.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
    public interface IEventHandler<T> : IEventHandler
        where T : IEvent
    {
        /// <summary>
        /// Handler that handles event.
        /// </summary>
        /// <param name="eventMesage">Event to handle.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Mesage")]
        void Handle(T eventMesage);
    }
}
