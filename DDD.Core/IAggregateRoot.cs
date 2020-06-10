using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.DDD.Core
{
    /// <summary>
    /// Interface that marks an aggregate.
    /// </summary>
    [InheritedExport]
    public interface IAggregateRoot
    {
        /// <summary>
        /// Get the persisten object identifier
        /// </summary>
        Guid AggregateId
        {
            get;
        }
    }
}
