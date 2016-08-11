using System.ComponentModel.Composition;
namespace RaraAvis.nCubed.EventSourcing.Core.Events
{
    /// <summary>
    /// Represents an event message that belongs to an ordered event stream.
    /// </summary>
    [InheritedExport]
    public interface IVersionedEvent : IEvent
    {
        /// <summary>
        /// Gets the version or order of the event in the stream.
        /// </summary>
        int Version { get; set; }
    }
}
