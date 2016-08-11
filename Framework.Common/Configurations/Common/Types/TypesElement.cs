using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Configurations.Common.Types
{
    /// <summary>
    /// The collection of types.
    /// </summary>
    public class TypesElement : ConfigurationElement
    {
        /// <summary>
        /// The collection types.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "It is used by Configuration Infrastructure"), ConfigurationProperty("types", IsRequired = true)]
        public TypeCollection Types
        {
            get
            {
                return (TypeCollection)this["types"];
            }
            set
            {
                this["types"] = value;
            }
        }
    }
}
