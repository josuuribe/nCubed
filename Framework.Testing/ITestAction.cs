using RaraAvis.nCubed.Core.Testing.Infrastructure.TestDbSet;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Testing
{
    /// <summary>
    /// Interface for testing actions.
    /// </summary>
    public interface ITestAction
    {
        /// <summary>
        /// Fakes <see cref="T:System.Data.Entity.Infrastructure.DbEntityEntry`1"/>.
        /// </summary>
        /// <typeparam name="T">Type entry to fake.</typeparam>
        /// <returns>The action object.</returns>
        ITestAction UseEntry<T>() where T : class;
        /// <summary>
        /// Use specified find to search entity.
        /// </summary>
        /// <typeparam name="T">Type entry to fake.</typeparam>
        /// <param name="func">The func object.</param>
        /// <returns>The action object.</returns>
        ITestAction UseFind<T>(Func<IEnumerable<dynamic>, object[], object> func) where T : class;
        /// <summary>
        /// Mark repository as preserved so these are not cleared.
        /// </summary>
        /// <param name="testSet">Test set to preserve.</param>
        /// <returns>The action object.</returns>
        ITestAction MarkPreserve(ITestDb testSet);
        /// <summary>
        /// Marks a <see cref="T:System.Data.Entity.DbSet`1"/> to raise an exception.
        /// </summary>
        /// <typeparam name="T">Exception type to raise.</typeparam>
        /// <param name="retryCount">Number of <see cref="M:System.Data.Entity.DbContext.SaveChanges"/> before raise exception.</param>
        /// <param name="retryError">If error must be rethrown.</param>
        /// <returns>The action object.</returns>
        ITestAction MarkExceptionSavingDbSet<T>(int retryCount, bool retryError) where T : Exception, new();
        /// <summary>
        /// Registers a set of data.
        /// </summary>
        /// <typeparam name="T">The entity type to register set.</typeparam>
        /// <returns>The action object.</returns>
        ITestAction UseRegisterTestDbSet<T>() where T : class,new();
        /// <summary>
        /// Fake <see cref="T:System.Data.Entity.Infrastructure.DbChangeTracker" />.
        /// </summary>
        /// <typeparam name="T">A <see cref="T:System.Data.Entity.Infrastructure.DbChangeTracker" /> related entity.</typeparam>
        /// <returns>The action object.</returns>
        ITestAction UseChangeTracker<T>() where T : class;
        /// <summary>
        /// Use the context given, not the fake one.
        /// </summary>
        /// <typeparam name="T">A <see cref="T:System.Data.Entity.DbContext"/> to use.</typeparam>
        /// <returns>The action object.</returns>
        ITestAction UseRealContext<T>() where T : DbContext, new();
    }
}
