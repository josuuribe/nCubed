using RaraAvis.nCubed.Core.Configurations.Common.Sections.Retries;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Configurations.Common.Sections
{
    /// <summary>
    /// Retries section.
    /// </summary>
    public class RetrySection : ConfigurationElement
    {
        /// <summary>
        /// Messaging configuration for database.
        /// </summary>
        [ConfigurationProperty("Database", IsRequired = false)]
        public DatabaseRetrySection DatabaseConfiguration
        {
            get
            {
                return (DatabaseRetrySection)this["Database"];
            }
            set
            {
                this["Database"] = value;
            }
        }
        /// <summary>
        /// Messaging configuration for messaging.
        /// </summary>
        [ConfigurationProperty("Messaging", IsRequired = false)]
        public MessagingRetrySection MessagingConfiguration
        {
            get
            {
                return (MessagingRetrySection)this["Messaging"];
            }
            set
            {
                this["Messaging"] = value;
            }
        }
        /// <summary>
        /// Messaging configuration for process.
        /// </summary>
        [ConfigurationProperty("Process", IsRequired = false)]
        public ProcessRetrySection ProcessConfiguration
        {
            get
            {
                return (ProcessRetrySection)this["Process"];
            }
            set
            {
                this["Process"] = value;
            }
        }
    }
}
