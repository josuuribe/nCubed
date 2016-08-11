using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Containers.DI
{
    /// <summary>
    /// Class for post processing objects.
    /// </summary>
    public class RegisteredEventArgs:EventArgs
    {
        /// <summary>
        /// Container that stores simple objects.
        /// </summary>
        public CompositionContainer ContainerSimple { get; private set; }
        /// <summary>
        /// Container that stores complex objects.
        /// </summary>
        public CompositionContainer ContainerFactory { get; private set; }
        /// <summary>
        /// Base constructor.
        /// </summary>
        /// <param name="containerSimple">A composition container for create simple objects.</param>
        /// <param name="containerFactory">A composition container for create complex objects.</param>
        public RegisteredEventArgs(CompositionContainer containerSimple, CompositionContainer containerFactory)
        {
            this.ContainerSimple = containerSimple;
            this.ContainerFactory = containerFactory;
        }
    }
}
