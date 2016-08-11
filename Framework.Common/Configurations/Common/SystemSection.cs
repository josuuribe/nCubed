using RaraAvis.nCubed.Core.Configurations.Common;
using RaraAvis.nCubed.Core.Configurations.Common.Sections;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Configurations
{
    /// <summary>
    /// Base class for common section.
    /// </summary>
    public class SystemSection : ActivitySection
    {
        /// <summary>
        /// Activity Id.
        /// </summary>
        [ConfigurationProperty("logId", IsRequired = false, DefaultValue = "00000000-0000-0000-0000-000000000000")]
        public Guid LogId
        {
            get
            {
                return (Guid)this["logId"];
            }
            set
            {
                this["logId"] = value;
            }
        }
        /// <summary>
        /// Common configuration for database.
        /// </summary>
        [ConfigurationProperty("Database", IsRequired = false)]
        public DatabaseSection DatabaseConfiguration
        {
            get
            {
                return (DatabaseSection)this["Database"];
            }
            set
            {
                this["Database"] = value;
            }
        }
        /// <summary>
        /// Common configuration for database.
        /// </summary>
        [ConfigurationProperty("Retry", IsRequired = false)]
        public RetrySection RetryConfiguration
        {
            get
            {
                return (RetrySection)this["Retry"];
            }
            set
            {
                this["Retry"] = value;
            }
        }
    }
}
