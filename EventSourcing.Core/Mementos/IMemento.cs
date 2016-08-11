using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.EventSourcing.Core.Mementos
{
    /// <summary>
    /// An opaque object that contains the state of another object (a snapshot) and can be used to restore its state.
    /// </summary>
    public interface IMemento
    {
        /// <summary>
        /// The version of the <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Events.IEventSourced"/> instance.
        /// </summary>
        int Version { get; }
    }
}
