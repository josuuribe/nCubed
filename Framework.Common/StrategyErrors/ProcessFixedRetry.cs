using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using RaraAvis.nCubed.Core.Configurations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.StrategyErrors
{
    /// <summary>
    /// Class for Sql incremental retry strategy.
    /// </summary>
    public class ProcessFixedRetry : RetryPolicy<AppProcessErrorStrategy>
    {
        /// <summary>
        /// Base constructor.
        /// </summary>
        public ProcessFixedRetry()
            : base(new FixedInterval(
                (ConfigurationManager.GetSection(N3Section.N3_NAME) as N3Section).System.RetryConfiguration.ProcessConfiguration.RetryName,
            (ConfigurationManager.GetSection(N3Section.N3_NAME) as N3Section).System.RetryConfiguration.ProcessConfiguration.RetryCount,
            TimeSpan.FromMilliseconds((ConfigurationManager.GetSection(N3Section.N3_NAME) as N3Section).System.RetryConfiguration.ProcessConfiguration.RetryInterval),
            (ConfigurationManager.GetSection(N3Section.N3_NAME) as N3Section).System.RetryConfiguration.ProcessConfiguration.FirstFastRetry))
        {

        }
    }
}
