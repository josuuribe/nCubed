using RaraAvis.nCubed.Core.Configurations.Common.Types;
using RaraAvis.nCubed.Core.Configurations.CQRS.Types;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Configurations.CQRS
{
    /// <summary>
    /// Speccific CQRS configuration.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "CQRS")]
    public class CQRSConfiguration : ConfigurationElement//, ICommonConfiguration
    {
        /// <summary>
        /// All types inside CQRS.
        /// </summary>
        public IEnumerable<TypesElement> AllTypes
        {
            get
            {
                return new List<TypesElement> { SerializeTypes.Commands, CommandHandlers, Bus };
            }
        }

        /// <summary>
        /// Types to be serialized.
        /// </summary>
        [ConfigurationProperty("serializeTypes", IsRequired = true)]
        public SerializeCommandElement SerializeTypes
        {
            get
            {
                return (SerializeCommandElement)this["serializeTypes"];
            }
            set
            {
                this["serializeTypes"] = value;
            }
        }

        /// <summary>
        /// Command handler elements.
        /// </summary>
        [ConfigurationProperty("commandHandlers", IsRequired = true)]
        public CommandHandlerElement CommandHandlers
        {
            get
            {
                return (CommandHandlerElement)this["commandHandlers"];
            }
            set
            {
                this["commandHandlers"] = value;
            }
        }

        /// <summary>
        /// Command bus.
        /// </summary>
        [ConfigurationProperty("bus", IsRequired = true)]
        public CommandBusElement Bus
        {
            get
            {
                return (CommandBusElement)this["bus"];
            }
            set
            {
                this["bus"] = value;
            }
        }
    }
}
