using RaraAvis.nCubed.Core.Configurations.Common.Sections;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Configurations.CQRS
{
    /// <summary>
    /// CQRS section.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "CQRS")]
    public class CQRSSection : ActivitySection
    {

        /// <summary>
        /// Types configuration for CQRS element.
        /// </summary>
        [ConfigurationProperty("typesConfiguration", IsRequired = true)]
        public CQRSConfiguration TypesConfiguration
        {
            get
            {
                return (CQRSConfiguration)this["typesConfiguration"];
            }
            set
            {
                this["typesConfiguration"] = value;
            }
        }
    }
}
