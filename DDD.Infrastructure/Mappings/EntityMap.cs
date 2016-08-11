using RaraAvis.nCubed.DDD.Core;
using RaraAvis.nCubed.DDD.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.DDD.Infrastructure.Mappings
{
    /// <summary>
    /// EntityMap.
    /// </summary>
    /// <typeparam name="T">Maps DDD entities.</typeparam>
    [ExcludeFromCodeCoverage]
    public class EntityMap<T> : EntityTypeConfiguration<T> where T : Entity
    {
        /// <summary>
        /// Base constructor.
        /// </summary>
        public EntityMap()
        {
            this.Ignore(t => t.EntityId);
            this.Ignore(t => t.IsTransient);
        }
    }
}
