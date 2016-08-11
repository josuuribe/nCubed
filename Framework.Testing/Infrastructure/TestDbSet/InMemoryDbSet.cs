using RaraAvis.nCubed.Core.Testing.Infrastructure.TestDbSet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Testing.Infrastructure.TestDbSet
{
    /// <summary>
    /// An in memory <see cref="T:System.Data.Entity.DbSet`1"/>.
    /// </summary>
    /// <typeparam name="TEntity">Entity type to store.</typeparam>
    public sealed class InMemoryDbSet<TEntity> : DbSet<TEntity>, IQueryable, IEnumerable<TEntity>, IDbAsyncEnumerable<TEntity>, ITestDb
            where TEntity : class, new()
    {
        private Func<IEnumerable<dynamic>, object[], object> find;
        private ObservableCollection<TEntity> data;
        private IQueryable query;

        #region ·   DbSet members   ·
        /// <summary>
        /// Base constructor.
        /// </summary>
        public InMemoryDbSet()
        {
            data = new ObservableCollection<TEntity>();
            query = data.AsQueryable();
        }
        /// <summary>
        /// Adds an entity.
        /// </summary>
        /// <param name="item">Entity to add.</param>
        /// <returns>Entity added.</returns>
        public override TEntity Add(TEntity item)
        {
            data.Add(item);
            return item;
        }
        /// <summary>
        /// Entity to remove.
        /// </summary>
        /// <param name="item">Entity to remove.</param>
        /// <returns>Entity removed.</returns>
        public override TEntity Remove(TEntity item)
        {
            data.Remove(item);
            return item;
        }
        /// <summary>
        /// Entity to attach.
        /// </summary>
        /// <param name="item">Entity to attach.</param>
        /// <returns>Entity attached.</returns>
        public override TEntity Attach(TEntity item)
        {
            data.Add(item);
            return item;
        }
        /// <summary>
        /// Entity to create.
        /// </summary>
        /// <returns>Entity created.</returns>
        public override TEntity Create()
        {
            return new TEntity();
        }
        /// <summary>
        /// Crate a derived entity.
        /// </summary>
        /// <typeparam name="TDerivedEntity">Derived type.</typeparam>
        /// <returns>Derived entity created.</returns>
        public override TDerivedEntity Create<TDerivedEntity>()
        {
            return Activator.CreateInstance<TDerivedEntity>();
        }
        /// <summary>
        /// Local data.
        /// </summary>
        public override ObservableCollection<TEntity> Local
        {
            get { return data; }
        }
        /// <summary>
        /// Invokes the specified func that performs the find.
        /// </summary>
        /// <param name="keyValues">Keyvalues </param>
        /// <returns>A TEntity object.</returns>
        public override TEntity Find(params object[] keyValues)
        {
            return find.Invoke(data, keyValues) as TEntity;
        }
        Type IQueryable.ElementType
        {
            get { return query.ElementType; }
        }
        Expression IQueryable.Expression
        {
            get { return query.Expression; }
        }
        IQueryProvider IQueryable.Provider
        {
            get { return new TestDbAsyncQueryProvider<TEntity>(query.Provider); }
        }
        System.Collections.Generic.IEnumerator<TEntity> System.Collections.Generic.IEnumerable<TEntity>.GetEnumerator()
        {
            return data.GetEnumerator();
        }
        IDbAsyncEnumerator<TEntity> IDbAsyncEnumerable<TEntity>.GetAsyncEnumerator()
        {
            return new TestDbAsyncEnumerator<TEntity>(data.GetEnumerator());
        }
        #endregion

        #region ·   ITestDb   ·
        /// <summary>
        /// Registers a find func to perform the find action.
        /// </summary>
        /// <param name="func">Func that finds.</param>
        public void RegisterFind(Func<IEnumerable<dynamic>, object[], object> func)
        {
            this.find = func;
        }
        /// <summary>
        /// Clears all data.
        /// </summary>
        public void Clear()
        {
            data.Clear();
        }
        #endregion
    }
}
