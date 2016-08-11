using RaraAvis.nCubed.Core.Configurations.Common.Sections;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Configurations.DDD
{
    /// <summary>
    /// CQRS section.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "DDD")]
    public class DDDSection : ActivitySection
    {
        /// <summary>
        /// Container that stores serializing types.
        /// </summary>
        [ConfigurationProperty("typesConfiguration", IsRequired = true)]
        public DDDConfiguration TypesConfiguration
        {
            get
            {
                return (DDDConfiguration)this["typesConfiguration"];
            }
            set
            {
                this["typesConfiguration"] = value;
            }
        }
    }
}
