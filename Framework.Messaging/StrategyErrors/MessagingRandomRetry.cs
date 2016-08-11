using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using RaraAvis.nCubed.Core.Configurations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Messaging.StrategyErrors
{
    /// <summary>
    /// Class for retrying messages.
    /// </summary>
    public class MessagingRandomRetry : RetryPolicy<MessagingErrorDetectionStrategy>
    {
        /// <summary>
        /// Base constructor.
        /// </summary>
        public MessagingRandomRetry()
            : base(new ExponentialBackoff(
                (ConfigurationManager.GetSection(N3Section.N3_NAME) as N3Section).System.RetryConfiguration.MessagingConfiguration.RetryName,
            (ConfigurationManager.GetSection(N3Section.N3_NAME) as N3Section).System.RetryConfiguration.MessagingConfiguration.RetryCount,
            TimeSpan.FromMilliseconds((ConfigurationManager.GetSection(N3Section.N3_NAME) as N3Section).System.RetryConfiguration.MessagingConfiguration.MinBackOff),
            TimeSpan.FromMilliseconds((ConfigurationManager.GetSection(N3Section.N3_NAME) as N3Section).System.RetryConfiguration.MessagingConfiguration.MaxBackOff),
            TimeSpan.FromMilliseconds((ConfigurationManager.GetSection(N3Section.N3_NAME) as N3Section).System.RetryConfiguration.MessagingConfiguration.DeltaBackOff),
            (ConfigurationManager.GetSection(N3Section.N3_NAME) as N3Section).System.RetryConfiguration.MessagingConfiguration.FirstFastRetry))
        {

        }
    }
}
