using System;

namespace RaraAvis.nCubed.EventSourcing.Core.Entities
{
    /// <summary>
    /// Base class for event sourced.
    /// </summary>
    public class EventData
    {
        /// <summary>
        /// Entity id.
        /// </summary>
        public Guid AggregateId { get; set; }
        /// <summary>
        /// Entity type.
        /// </summary>
        public string AggregateType { get; set; }
        /// <summary>
        /// Event version.
        /// </summary>
        public int Version { get; set; }
        /// <summary>
        /// Event payload.
        /// </summary>
        public string Payload { get; set; }
        /// <summary>
        /// Event correlation id.
        /// </summary>
        public string CorrelationId { get; set; }
    }
}
