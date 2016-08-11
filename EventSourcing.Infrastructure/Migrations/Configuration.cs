namespace RaraAvis.nCubed.EventSourcing.Infrastructure.Migrations
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
    internal sealed class Configuration : DbMigrationsConfiguration<RaraAvis.nCubed.EventSourcing.Infrastructure.EventSourcingContext>
    {
        /// <summary>
        /// Base constructor.
        /// </summary>
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "nCubed.EventSourcing";
        }
        /// <summary>
        /// This method will be called after migrating to the latest version.
        /// </summary>
        /// <param name="context">A <see cref="T:RaraAvis.nCubed.EventSourcing.Infrastructure.EventSourcingContext"/>.</param>
        protected override void Seed(RaraAvis.nCubed.EventSourcing.Infrastructure.EventSourcingContext context)
        {
        }
    }
}
