using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Testing.Infrastructure.TestDbSet
{
    /// <summary>
    /// Interface to test Database.
    /// </summary>
    public interface ITestDb
    {
        /// <summary>
        /// A func that processes a find.
        /// </summary>
        /// <param name="func">Find function.</param>
        void RegisterFind(Func<IEnumerable<dynamic>, object[], object> func);
        /// <summary>
        /// Clears all data.
        /// </summary>
        void Clear();
    }
}
