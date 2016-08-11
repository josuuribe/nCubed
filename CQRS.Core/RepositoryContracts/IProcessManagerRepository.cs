using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using RaraAvis.nCubed.CQRS.Core.ProcessManager;
using System.ComponentModel.Composition;


namespace RaraAvis.nCubed.CQRS.Core.RepositoryContracts
{
    /// <summary>
    /// This for reading and writing process managers (also known as Sagas in the CQRS community).
    /// </summary>
    /// <typeparam name="T">A IDisposable object.</typeparam>
    public interface IProcessManagerRepository<T> : IDisposable
        where T : class, IProcessManager
    {
        /// <summary>
        /// Find the specified <see cref="T:RaraAvis.nCubed.CQRS.Core.ProcessManager.IProcessManager"/>.
        /// </summary>
        /// <param name="id">A id identifier.</param>
        /// <returns>A <see cref="T:RaraAvis.nCubed.CQRS.Core.ProcessManager.IProcessManager"/> object.</returns>
        T Find(Guid id);
        /// <summary>
        /// Saves into store the specified <see cref="T:RaraAvis.nCubed.CQRS.Core.ProcessManager.IProcessManager"/>.
        /// </summary>
        /// <param name="processManager">A <see cref="T:RaraAvis.nCubed.CQRS.Core.ProcessManager.IProcessManager"/> object.</param>
        void Save(T processManager);
        /// <summary>
        /// Updates into store the specified <see cref="T:RaraAvis.nCubed.CQRS.Core.ProcessManager.IProcessManager"/>.
        /// </summary>
        /// <param name="processManager">A <see cref="T:RaraAvis.nCubed.CQRS.Core.ProcessManager.IProcessManager"/> object.</param>
        void Update(T processManager);
        /// <summary>
        /// Find base a <see cref="T:System.Linq.Expressions.Expression`1"/> given.
        /// </summary>
        /// <param name="predicate">Predicate that processes objects.</param>
        /// <param name="includeCompleted">Include completed process managers or not.</param>
        /// <returns>A <see cref="T:RaraAvis.nCubed.CQRS.Core.ProcessManager.IProcessManager"/> object.</returns>
        T Find(Expression<Func<T, bool>> predicate, bool includeCompleted);
        /// <summary>
        /// Find base a <see cref="T:System.Linq.Expressions.Expression`1"/> given.
        /// </summary>
        /// <param name="predicate">Predicate that processes objects.</param>
        /// <returns>A <see cref="T:RaraAvis.nCubed.CQRS.Core.ProcessManager.IProcessManager"/> object.</returns>
        T Find(Expression<Func<T, bool>> predicate);
    }
}
