using RaraAvis.nCubed.Core.Adapter;
using System;
using System.ComponentModel.Composition;
using System.Runtime.Serialization;

namespace RaraAvis.nCubed.CQRS.Core.Services
{
    /// <summary>
    /// Base class for DTO commands.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dto")]
    [InheritedExport]
    [DataContract(IsReference = true)]
    public abstract class CommandDto
    {
        /// <summary>
        /// Base constructor.
        /// </summary>
        protected CommandDto() { }
    }
}

