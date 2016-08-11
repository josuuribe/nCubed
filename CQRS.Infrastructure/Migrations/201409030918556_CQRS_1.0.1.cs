namespace RaraAvis.nCubed.CQRS.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Diagnostics.CodeAnalysis;
    /// <summary>
    /// Class that implents migration.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class CQRS_101 : DbMigration
    {
        /// <summary>
        /// Up method.
        /// </summary>
        public override void Up()
        {
            CreateTable(
                "dbo.N3_UndispatchedMessages",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Commands = c.String(),
                        CreatedTime = c.Time(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        /// <summary>
        /// Down method.
        /// </summary>
        public override void Down()
        {
            DropTable("dbo.N3_UndispatchedMessages");
        }
    }
}
