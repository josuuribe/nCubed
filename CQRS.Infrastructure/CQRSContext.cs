using RaraAvis.nCubed.CQRS.Core.Entities;
using RaraAvis.nCubed.CQRS.Infrastructure.Mappings;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaraAvis.nCubed.Core.Infrastructure;
using System.Diagnostics.CodeAnalysis;
using RaraAvis.nCubed.Core.Infrastructure.Sql;
using System.ComponentModel.Composition;


namespace RaraAvis.nCubed.CQRS.Infrastructure
{
    /// <summary>
    /// CQRS context.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "CQRS"), Export(typeof(DbContext))]
    [DbConfigurationType(typeof(DbCommonConfiguration))]
    [ExcludeFromCodeCoverage]
    public class CQRSContext : DbContext
    {
        /// <summary>
        /// CQRS context constructor.
        /// </summary>
        public CQRSContext()
        {            
            UndispatchedMessages = base.Set<UndispatchedMessage>();
        }
        #region ·   DbSets  ·
        /// <summary>
        /// Table UndispatchedMessage. 
        /// </summary>        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Undispatched")]
        public IDbSet<UndispatchedMessage> UndispatchedMessages
        {
            get;
            private set;
        }
        #endregion
        /// <summary>
        /// Configurations for CQRS context.
        /// </summary>
        /// <param name="modelBuilder">Builder to create context.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "It is used by EF Infrastructure")]
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingEntitySetNameConvention>();
            modelBuilder.Configurations.Add(new UndispatchedMessageMap());
        }

    }
}
