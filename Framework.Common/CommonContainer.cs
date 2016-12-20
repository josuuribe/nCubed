using RaraAvis.nCubed.Core.Configurations;
using RaraAvis.nCubed.Core.Configurations.Common.Types;
using RaraAvis.nCubed.Core.Containers;
using RaraAvis.nCubed.Core.Containers.DI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core
{
    /// <summary>
    /// Class that processes MEF container for Common objects.
    /// </summary>
    public sealed class CommonContainer : SystemContainer
    {
        /// <summary>
        /// Base constructor.
        /// </summary>
        public CommonContainer()
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
        /// Types for Common given.
        /// </summary>
        protected override IEnumerable<TypesElement> AllTypes
        {
            get { return N3Section.Section.System.TypesConfiguration.AllTypes; }
        }
    }
}
