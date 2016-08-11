using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Configurations.Common.Types
{
    /// <summary>
    /// A element type.
    /// </summary>
    public class TypeElement : ConfigurationElement
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
        [ConfigurationProperty("file", IsRequired = false, DefaultValue = "*")]
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
        /// Namespace for element.
        /// </summary>
        [ConfigurationProperty("namespace", IsRequired = false, IsKey = true, DefaultValue = "*")]
        public string Namespace
        {
            get
            {
                return (string)this["namespace"];
            }
            set
            {
                this["namespace"] = value;
            }
        }
    }

}
