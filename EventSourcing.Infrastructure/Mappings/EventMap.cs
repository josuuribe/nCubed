using Optimissa.nCubed.EventSourcing.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Optimissa.nCubed.EventSourcing.Infrastructure.Mappings
{
    /// <summary>
    /// Class that maps events table.
    /// </summary>
    sealed internal class EventMap : EntityTypeConfiguration<Event>
    {
        /// <summary>
        /// Base constructor.
        /// </summary>
        public EventMap() {
            this.Ignore(p => p.SourceId);
            this.Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("N3_Events");
            });
        }
    }
}
