using RaraAvis.nCubed.Core.Configurations.Common.Sections;
using System;
using System.Configuration;

namespace RaraAvis.nCubed.Core.Configurations.Common
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
        /// <summary>
        /// Common types configuration.
        /// </summary>
        [ConfigurationProperty("typesConfiguration", IsRequired = false)]
        public SystemConfiguration TypesConfiguration
        {
            get
            {
                return (SystemConfiguration)this["typesConfiguration"];
            }
            set
            {
                this["typesConfiguration"] = value;
            }
        }
    }
}
