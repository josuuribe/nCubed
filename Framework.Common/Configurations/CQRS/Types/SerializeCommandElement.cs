using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Configurations.CQRS.Types
{
    /// <summary>
    /// Serialize command element.
    /// </summary>
    public class SerializeCommandElement : ConfigurationElement
    {
        /// <summary>
        /// Commands to be processed.
        /// </summary>
        [ConfigurationProperty("commands", IsRequired = true)]
        public CommandElement Commands
        {
            get
            {
                return (CommandElement)this["commands"];
            }
            set
            {
                this["commands"] = value;
            }
        }
    }
}
