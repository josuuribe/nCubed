using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Expressions
{
    /// <summary>
    /// Exposes a cached <see cref="M:RaraAvis.nCubed.Core.Expressions.IAnyEnumerableCollection`1.Any"/> operator.
    /// </summary>
    /// <typeparam name="T">Type for collection.</typeparam>
    public interface IAnyEnumerableCollection<out T> : IEnumerable<T>
    {
        /// <summary>
        /// Any.
        /// </summary>
        /// <returns>True or false.</returns>
        bool Any();
    }
}
