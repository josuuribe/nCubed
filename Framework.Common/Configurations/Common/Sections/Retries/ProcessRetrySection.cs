using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Configurations.Common.Sections.Retries
{
    /// <summary>
    /// Process retries section.
    /// </summary>
    public class ProcessRetrySection : CommonRetrySection
    {
        /// <summary>
        /// Initial Retry.
        /// </summary>
        [ConfigurationProperty("retryInterval", IsRequired = false, DefaultValue = 2.0)]
        public double RetryInterval
        {
            get
            {
                return (double)this["retryInterval"];
            }
            set
            {
                this["retryInterval"] = value;
            }
        }
    }
}
