using RaraAvis.nCubed.Core.Configurations.Common.Types;
using RaraAvis.nCubed.Core.Configurations.DDD.Types;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Configurations.DDD
{
    /// <summary>
    /// Speccific DDD configuration.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "DDD")]
    public class DDDConfiguration : ConfigurationElement//, ICommonConfiguration
    {
        /// <summary>
        /// All types inside DDD.
        /// </summary>
        public IEnumerable<TypesElement> AllTypes
        {
            get
            {
                return new List<TypesElement> { DomainServices, ApplicationServices, Repositories };
            }
        }
        /// <summary>
        /// Domain configuration.
        /// </summary>
        [ConfigurationProperty("domainServices", IsRequired = true)]
        public DomainElement DomainServices
        {
            get
            {
                return (DomainElement)this["domainServices"];
            }
            set
            {
                this["domainServices"] = value;
            }
        }
        /// <summary>
        /// Application service.
        /// </summary>
        [ConfigurationProperty("applicationServices", IsRequired = true)]
        public ApplicationElement ApplicationServices
        {
            get
            {
                return (ApplicationElement)this["applicationServices"];
            }
            set
            {
                this["applicationServices"] = value;
            }
        }
        /// <summary>
        /// Repository.
        /// </summary>
        [ConfigurationProperty("repositories", IsRequired = true)]
        public RepositoryElement Repositories
        {
            get
            {
                return (RepositoryElement)this["repositories"];
            }
            set
            {
                this["repositories"] = value;
            }
        }
    }

}
