using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Configurations.Common.Sections.Retries
{
    /// <summary>
    /// Retry section.
    /// </summary>
    public class CommonRetrySection : ConfigurationElement
    {
        /// <summary>
        /// Retry name.
        /// </summary>
        [ConfigurationProperty("retryName", IsRequired = false, DefaultValue = "DefaultRetry")]
        public string RetryName
        {
            get
            {
                return (string)this["retryName"];
            }
            set
            {
                this["retryName"] = value;
            }
        }
        /// <summary>
        /// Retry count.
        /// </summary>
        [ConfigurationProperty("retryCount", IsRequired = false, DefaultValue = 3)]
        public int RetryCount
        {
            get
            {
                return (int)this["retryCount"];
            }
            set
            {
                this["retryCount"] = value;
            }
        }
        /// <summary>
        /// First fast retry.
        /// </summary>
        [ConfigurationProperty("firstFastRetry", IsRequired = false, DefaultValue = true)]
        public bool FirstFastRetry
        {
            get
            {
                return (bool)this["firstFastRetry"];
            }
            set
            {
                this["firstFastRetry"] = value;
            }
        }
    }
}
