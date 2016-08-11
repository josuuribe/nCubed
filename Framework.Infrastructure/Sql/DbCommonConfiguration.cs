using RaraAvis.nCubed.Core.Configurations;
using RaraAvis.nCubed.Core.Containers;
using RaraAvis.nCubed.Core.Containers.DI;
using RaraAvis.nCubed.Core.Infrastructure.Logging;
using RaraAvis.nCubed.Core.Infrastructure.StrategyErrors;
using RaraAvis.nCubed.Core.Serialization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Infrastructure.Sql
{
    /// <summary>
    /// Custom configuration for database.
    /// </summary>
    public class DbCommonConfiguration : DbConfiguration
    {
        /// <summary>
        /// Base cosntructor.
        /// </summary>
        public DbCommonConfiguration()
        {
            N3Section nCubedConfiguration = (ConfigurationManager.GetSection(N3Section.N3_NAME) as N3Section);

            //TODO: ¿Cómo afecta a la opción por código?
            SetExecutionStrategy(SqlProviderServices.ProviderInvariantName, () => new SqlDbExecutionStrategy());

            SetDefaultConnectionFactory(new ServiceConfigurationSettingConnectionFactory());

            if (nCubedConfiguration.System.DatabaseConfiguration.LogSql)
            {
                DbInterception.Add(new InterceptorLogging());
            }
        }
    }
}
