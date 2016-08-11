using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Configurations.Common.Types
{
    /// <summary>
    /// Element to be excluded for MEF.
    /// </summary>
    public class ExcludeElement : ConfigurationElement
    {
        /// <summary>
        /// Name for excluded element.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods"), ConfigurationProperty("name", IsRequired = true)]
        public string Type
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }
    }
}
