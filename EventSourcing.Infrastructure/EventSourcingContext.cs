using RaraAvis.nCubed.Core.Infrastructure;
using RaraAvis.nCubed.Core.Infrastructure.Sql;
using RaraAvis.nCubed.EventSourcing.Core.Entities;
using RaraAvis.nCubed.EventSourcing.Infrastructure.Mappings;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.EventSourcing.Infrastructure
{
    /// <summary>
    /// Eventsourcing context.
    /// </summary>
    [DbConfigurationType(typeof(DbCommonConfiguration))]
    [ExcludeFromCodeCoverage]
    public class EventSourcingContext : DbContext
    {
        /// <summary>
        /// Base constructor.
        /// </summary>
        public EventSourcingContext()
        {
            EventSourceEntities = base.Set<EventData>();
        }
        #region ·   DbSets  ·
        /// <summary>
        /// Table Eventsource.
        /// </summary>
        public IDbSet<EventData> EventSourceEntities
        {
            get;
            private set;
        }
        #endregion
        /// <summary>
        /// Configurations for EventSourcingContext context.
        /// </summary>
        /// <param name="modelBuilder">Builder to create context.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "It is used by EF Infrastructure")]
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingEntitySetNameConvention>();
            modelBuilder.Configurations.Add(new EventSourcedMap());
        }
    }
}

