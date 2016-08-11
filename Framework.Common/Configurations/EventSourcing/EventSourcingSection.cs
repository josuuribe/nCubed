using RaraAvis.nCubed.Core.Configurations.Common.Sections;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Configurations.EventSourcing
{
    /// <summary>
    /// Event sourcing section.
    /// </summary>
    public class EventSourcingSection : ActivitySection
    {
        /// <summary>
        /// EventSourcing types configuration.
        /// </summary>
        [ConfigurationProperty("typesConfiguration", IsRequired = true)]
        public EventSourcingConfiguration TypesConfiguration
        {
            get
            {
                return (EventSourcingConfiguration)this["typesConfiguration"];
            }
            set
            {
                this["typesConfiguration"] = value;
            }
        }
    }
}
