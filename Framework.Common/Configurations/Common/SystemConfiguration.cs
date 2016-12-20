using RaraAvis.nCubed.Core.Configurations.Common.Sections;
using RaraAvis.nCubed.Core.Configurations.Common.Types;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Configurations.Common
{
    /// <summary>
    /// System sourcing section.
    /// </summary>
    public class SystemConfiguration : ConfigurationElement
    {
        /// <summary>
        /// All types inside DDD.
        /// </summary>
        public IEnumerable<TypesElement> AllTypes
        {
            get
            {
                return new List<TypesElement> { CommonElements };
            }
        }
        /// <summary>
        /// CommonConfiguration types configuration.
        /// </summary>
        [ConfigurationProperty("commonTypes", IsRequired = true)]
        public TypesElement CommonElements
        {
            get
            {
                return (TypesElement)this["commonTypes"];
            }
            set
            {
                this["commonTypes"] = value;
            }
        }
    }
}
