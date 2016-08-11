using RaraAvis.nCubed.Core.Infrastructure.Sql;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Testing.Infrastructure
{
    /// <summary>
    /// Empty context to help faking <see cref="T:System.Data.Entity.DbContext" />.
    /// </summary>
    [DbConfigurationType(typeof(DbCommonConfiguration))]
    public class TestEmptyDbContext : DbContext
    {
        /// <summary>
        /// Base constructor.
        /// </summary>
        public TestEmptyDbContext()
        {
            
        }
    }
}
