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
    public class PathElement : ConfigurationElement
    {
        /// <summary>
        /// Key for element.
        /// </summary>
        [ConfigurationProperty("key", IsRequired = true, IsKey = true)]
        public string Key
        {
            get
            {
                return (string)this["key"];
            }
            set
            {
                this["key"] = value;
            }
        }

        /// <summary>
        /// Library path for assembly.
        /// </summary>
        [ConfigurationProperty("path", IsRequired = false)]
        public string Path
        {
            get
            {
                return (string)this["path"];
            }
            set
            {
                this["path"] = value;
            }
        }

        /// <summary>
        /// Assembly filename.
        /// </summary>
        [ConfigurationProperty("file", IsRequired = false, DefaultValue = "*.dll")]
        public string File
        {
            get
            {
                return (string)this["file"];
            }
            set
            {
                this["file"] = value;
            }
        }

        /// <summary>
        /// A collection of elements to be excluded.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "It is used by Configuration Infrastructure"), ConfigurationProperty("excludedTypes", IsRequired = false)]
        public ExcludeCollection ExcludeElements
        {
            get
            {
                return (ExcludeCollection)this["excludedTypes"];
            }
            set
            {
                this["excludedTypes"] = value;
            }
        }
    }

}
