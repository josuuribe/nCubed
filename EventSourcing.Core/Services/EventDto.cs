using RaraAvis.nCubed.Core.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.EventSourcing.Core.Services
{
    /// <summary>
    /// EventDto base class.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dto")]
    [InheritedExport]
    [DataContract(IsReference = true)]
    public abstract class EventDto
    {
        /// <summary>
        /// Base constructor.
        /// </summary>
        protected EventDto() { }
    }
}
