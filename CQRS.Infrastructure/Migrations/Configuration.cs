namespace RaraAvis.nCubed.CQRS.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <summary>
    /// Configuration class.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "It is used by EF Infrastructure"), ExcludeFromCodeCoverage]
    internal sealed class Configuration : DbMigrationsConfiguration<CQRSContext>
    {
        /// <summary>
        /// Base constructor.
        /// </summary>
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            this.ContextKey = "nCubed.CQRS";
        }
        /// <summary>
        /// This method will be called after migrating to the latest version.
        /// </summary>
        /// <param name="context">A <see cref="CQRSContext"/>.</param>
        protected override void Seed(CQRSContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
