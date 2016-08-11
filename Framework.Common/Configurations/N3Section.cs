using RaraAvis.nCubed.Core.Configurations.CQRS;
using RaraAvis.nCubed.Core.Configurations.DDD;
using RaraAvis.nCubed.Core.Configurations.EventSourcing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Configurations
{
    /// <summary>
    /// N3 section.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class N3Section : ConfigurationSection
    {
        /// <summary>
        /// XML root element name configuration .
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "NAME", Justification = "It is a constant.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores",Justification="It is a constant.")]
        public const string N3_NAME = "nCubed";
        /// <summary>
        /// Root section.
        /// </summary>
        public static N3Section Section
        {
            get
            {
                return (ConfigurationManager.GetSection(N3Section.N3_NAME) as N3Section);
            }
        }
        /// <summary>
        /// DDD Section
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "DDD"), ConfigurationProperty("DDD", IsRequired = false)]
        public DDDSection DDD
        {
            get
            {
                return (DDDSection)this["DDD"];
            }
            set
            {
                this["DDD"] = value;
            }
        }

        /// <summary>
        /// CQRS Section
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "CQRS"), ConfigurationProperty("CQRS", IsRequired = false)]
        public CQRSSection CQRS
        {
            get
            {
                return (CQRSSection)this["CQRS"];
            }
            set
            {
                this["CQRS"] = value;
            }
        }
        /// <summary>
        /// EventSourcing section.
        /// </summary>
        [ConfigurationProperty("ES", IsRequired = false)]
        public EventSourcingSection ES
        {
            get
            {
                return (EventSourcingSection)this["ES"];
            }
            set
            {
                this["ES"] = value;
            }
        }
        /// <summary>
        /// System section.
        /// </summary>
        [ConfigurationProperty("System", IsRequired = false)]
        public SystemSection System
        {
            get
            {
                return (SystemSection)this["System"];
            }
            set
            {
                this["System"] = value;
            }
        }
    }
}
