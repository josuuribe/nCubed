using RaraAvis.nCubed.EventSourcing.Core.Entities;
using System.Data.Entity.ModelConfiguration;
using System.Diagnostics.CodeAnalysis;

namespace RaraAvis.nCubed.EventSourcing.Infrastructure.Mappings
{
    /// <summary>
    /// Class that maps EventSourced table.
    /// </summary>
    [ExcludeFromCodeCoverage]
    sealed internal class EventSourcedMap : EntityTypeConfiguration<EventData>
    {
        /// <summary>
        /// Base constructor.
        /// </summary>
        public EventSourcedMap()
        {
            this.HasKey(x => new { x.AggregateId, x.AggregateType, x.Version }).ToTable("N3_EventSourced");
        }
    }
}
