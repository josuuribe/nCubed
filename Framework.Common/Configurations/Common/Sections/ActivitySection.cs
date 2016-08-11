using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Configurations.Common.Sections
{
    /// <summary>
    /// Section used for establish Ids.
    /// </summary>
    public class ActivitySection : ConfigurationElement
    {
        /// <summary>
        /// Activity Id.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "value", Justification = "It is used by Configuration Infrastructure"), ConfigurationProperty("activityId", IsRequired = true, DefaultValue = "00000000-0000-0000-0000-000000000000")]
        public Guid ActivityId
        {
            get
            {
                return (Guid)this["activityId"];
            }
            set
            {
                this["activityId"] = ActivityId;
            }
        }
    }
}
