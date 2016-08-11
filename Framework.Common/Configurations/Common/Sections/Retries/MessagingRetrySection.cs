using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Configurations.Common.Sections.Retries
{
    /// <summary>
    /// Messaging Section.
    /// </summary>
    public class MessagingRetrySection : CommonRetrySection
    {
        /// <summary>
        /// MinBackOff.
        /// </summary>
        [ConfigurationProperty("minBackOff", IsRequired = false, DefaultValue = 1000.0)]
        public double MinBackOff
        {
            get
            {
                return (double)this["minBackOff"];
            }
            set
            {
                this["minBackOff"] = value;
            }
        }
        /// <summary>
        /// MaxBackOff.
        /// </summary>
        [ConfigurationProperty("maxBackOff", IsRequired = false, DefaultValue = 3000.0)]
        public double MaxBackOff
        {
            get
            {
                return (double)this["maxBackOff"];
            }
            set
            {
                this["maxBackOff"] = value;
            }
        }
        /// <summary>
        /// DeltaBackOff.
        /// </summary>
        [ConfigurationProperty("deltaBackOff", IsRequired = false, DefaultValue = 500.0)]
        public double DeltaBackOff
        {
            get
            {
                return (double)this["deltaBackOff"];
            }
            set
            {
                this["deltaBackOff"] = value;
            }
        }
    }
}
