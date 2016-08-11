using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using RaraAvis.nCubed.Core.Configurations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Infrastructure.StrategyErrors
{
    /// <summary>
    /// Class for Sql incremental retry strategy.
    /// </summary>
    public class SqlIncrementalRetry : RetryPolicy<SqlDbExecutionStrategy>
    {
        /// <summary>
        /// Base constructor.
        /// </summary>
        public SqlIncrementalRetry()
            : base(new Incremental(
                (ConfigurationManager.GetSection(N3Section.N3_NAME) as N3Section).System.RetryConfiguration.DatabaseConfiguration.RetryName,
            (ConfigurationManager.GetSection(N3Section.N3_NAME) as N3Section).System.RetryConfiguration.DatabaseConfiguration.RetryCount,
            TimeSpan.FromMilliseconds((ConfigurationManager.GetSection(N3Section.N3_NAME) as N3Section).System.RetryConfiguration.DatabaseConfiguration.InitialRetry),
            TimeSpan.FromMilliseconds((ConfigurationManager.GetSection(N3Section.N3_NAME) as N3Section).System.RetryConfiguration.DatabaseConfiguration.IncrementalRetry),
            (ConfigurationManager.GetSection(N3Section.N3_NAME) as N3Section).System.RetryConfiguration.DatabaseConfiguration.FirstFastRetry))
        {

        }
    }
}
