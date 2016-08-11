using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Configurations.EventSourcing.Types
{
    /// <summary>
    /// Serialize element.
    /// </summary>
    public class SerializeEventElement : ConfigurationElement
    {
        /// <summary>
        /// Events element.
        /// </summary>
        [ConfigurationProperty("events", IsRequired = true)]
        public EventElement Events
        {
            get
            {
                return (EventElement)this["events"];
            }
            set
            {
                this["events"] = value;
            }
        }
    }
}
