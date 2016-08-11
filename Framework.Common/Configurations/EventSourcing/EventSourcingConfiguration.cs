using RaraAvis.nCubed.Core.Configurations.Common.Types;
using RaraAvis.nCubed.Core.Configurations.EventSourcing.Types;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Configurations.EventSourcing
{
    /// <summary>
    /// Speccific Event sourcing configuration.
    /// </summary>
    public class EventSourcingConfiguration : ConfigurationElement//, ICommonConfiguration
    {
        /// <summary>
        /// All types inside DDD.
        /// </summary>
        public IEnumerable<TypesElement> AllTypes
        {
            get
            {
                return new List<TypesElement> { SerializeTypes.Events, EventHandlers, Bus };
            }
        }
        /// <summary>
        /// Retry name.
        /// </summary>
        [ConfigurationProperty("cacheExpires", IsRequired = false, DefaultValue = 30.0)]
        public double CacheExpires
        {
            get
            {
                return (double)this["cacheExpires"];
            }
            set
            {
                this["cacheExpires"] = value;
            }
        }
        /// <summary>
        /// Types to be serialized.
        /// </summary>
        [ConfigurationProperty("serializeTypes", IsRequired = true)]
        public SerializeEventElement SerializeTypes
        {
            get
            {
                return (SerializeEventElement)this["serializeTypes"];
            }
            set
            {
                this["serializeTypes"] = value;
            }
        }

        /// <summary>
        /// Event handler elements.
        /// </summary>
        [ConfigurationProperty("eventHandlers", IsRequired = true)]
        public EventHandlerElement EventHandlers
        {
            get
            {
                return (EventHandlerElement)this["eventHandlers"];
            }
            set
            {
                this["eventHandlers"] = value;
            }
        }

        /// <summary>
        /// Event bus.
        /// </summary>
        [ConfigurationProperty("bus", IsRequired = true)]
        public EventBusElement Bus
        {
            get
            {
                return (EventBusElement)this["bus"];
            }
            set
            {
                this["bus"] = value;
            }
        }
    }
}
