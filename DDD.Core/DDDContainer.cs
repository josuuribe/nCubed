using RaraAvis.nCubed.Core.Configurations;
using RaraAvis.nCubed.Core.Configurations.Common.Types;
using RaraAvis.nCubed.Core.Containers;
using RaraAvis.nCubed.Core.Containers.DI;
using RaraAvis.nCubed.Core.Exceptions;
using RaraAvis.nCubed.Core.Serialization;
using RaraAvis.nCubed.DDD.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.DDD.Core
{
    /// <summary>
    /// Class that processses MEF container for DDD.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "DDD")]
    public sealed class DDDContainer : SystemContainer
    {
        /// <summary>
        /// Base constructor.
        /// </summary>
        public DDDContainer()
            : base()
        {
            base.Register(AllTypes);
        }
        /// <summary>
        /// A <see cref="T:RaraAvis.nCubed.Core.Containers.DI.DIManager"> object that manages dependencies.</see>
        /// </summary>
        public DIManager Constructor
        {
            get { return base.DIManager; }
        }
        /// <summary>
        /// Types for DDD given.
        /// </summary>
        protected override IEnumerable<TypesElement> AllTypes
        {
            get { return N3Section.Section.DDD.TypesConfiguration.AllTypes; }
        }
    }
}
