using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Configurations.Common.Sections.Retries
{
    /// <summary>
    /// Database section.
    /// </summary>
    public class DatabaseRetrySection : CommonRetrySection
    {
        /// <summary>
        /// Initial Retry.
        /// </summary>
        [ConfigurationProperty("initialRetry", IsRequired = false, DefaultValue = 2.0)]
        public double InitialRetry
        {
            get
            {
                return (double)this["initialRetry"];
            }
            set
            {
                this["initialRetry"] = value;
            }
        }
        /// <summary>
        /// Incremental Retry.
        /// </summary>
        [ConfigurationProperty("incrementalRetry", IsRequired = false, DefaultValue = 1.0)]
        public double IncrementalRetry
        {
            get
            {
                return (double)this["incrementalRetry"];
            }
            set
            {
                this["incrementalRetry"] = value;
            }
        }
    }
}
