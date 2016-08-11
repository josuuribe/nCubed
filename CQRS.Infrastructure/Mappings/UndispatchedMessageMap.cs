using RaraAvis.nCubed.CQRS.Core.Entities;
using System.Data.Entity.ModelConfiguration;
using System.Diagnostics.CodeAnalysis;

namespace RaraAvis.nCubed.CQRS.Infrastructure.Mappings
{
    /// <summary>
    /// Class that maps UndispatchedMessage table.
    /// </summary>
    [ExcludeFromCodeCoverage]
    sealed internal class UndispatchedMessageMap : EntityTypeConfiguration<UndispatchedMessage>
    {
        /// <summary>
        /// Base constructor.
        /// </summary>
        public UndispatchedMessageMap() {
            this.HasKey(p => p.Id);
            this.ToTable("N3_UndispatchedMessages");
        }
    }
}
