namespace RaraAvis.nCubed.EventSourcing.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Diagnostics.CodeAnalysis;
    /// <summary>
    /// ES 1.0.1 Migration.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class ES_101 : DbMigration
    {
        /// <summary>
        /// Up Method.
        /// </summary>
        public override void Up()
        {
            CreateTable(
                "dbo.N3_EventSourced",
                c => new
                    {
                        AggregateId = c.Guid(nullable: false),
                        AggregateType = c.String(nullable: false, maxLength: 128),
                        Version = c.Int(nullable: false),
                        Payload = c.String(),
                        CorrelationId = c.String(),
                    })
                .PrimaryKey(t => new { t.AggregateId, t.AggregateType, t.Version });
            
        }
        /// <summary>
        /// Down method.
        /// </summary>
        public override void Down()
        {
            DropTable("dbo.N3_EventSourced");
        }
    }
}
