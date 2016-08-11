using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Configurations.Common.Sections
{
    /// <summary>
    /// Database section.
    /// </summary>
    public class DatabaseSection : ActivitySection
    {
        /// <summary>
        /// Activity Id.
        /// </summary>
        [ConfigurationProperty("logSql", IsRequired = false, DefaultValue = false)]
        public bool LogSql
        {
            get
            {
                return (bool)this["logSql"];
            }
            set
            {
                this["logSql"] = value;
            }
        }

        /// <summary>
        /// Activity Id.
        /// </summary>
        [ConfigurationProperty("sqlCommandTimeOut", IsRequired = false, DefaultValue = 30)]
        public int SqlCommandTimeout
        {
            get
            {
                return (int)this["sqlCommandTimeOut"];
            }
            set
            {
                this["sqlCommandTimeOut"] = value;
            }
        }
    }
}
