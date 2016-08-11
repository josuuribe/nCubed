using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaraAvis.nCubed.DDD.Infrastructure;
using System.Data.Entity;
using RaraAvis.nCubed.DDD.Core;
using System.Data.Entity.Infrastructure;
using RaraAvis.nCubed.Core.Logging;
using RaraAvis.nCubed.DDD.Infrastructure.Properties;
using System.Diagnostics.CodeAnalysis;
using RaraAvis.nCubed.DDD.Core.Exceptions;
using RaraAvis.nCubed.DDD.Core.Specification;
using System.ComponentModel.Composition;

namespace RaraAvis.nCubed.DDD.Infrastructure
{

    /// <summary>
    /// Repository base class
    /// </summary>
    /// <typeparam name="TEntity">The type of underlying entity in this repository</typeparam>
    [InheritedExport(typeof(IEntityRepository<>))]
    public class EntityRepository<TEntity> : IEntityRepository<TEntity>
        where TEntity : Entity
    {
        #region ·   Members ·
        /// <summary>
        /// Unit Of Work pattern.
        /// </summary>
        protected IQueryableUnitOfWork QueryableUnitOfWork
        {
            get;
            set;
        }

        #endregion

        #region ·   Constructor ·

        /// <summary>
        /// Create a new instance of repository
        /// </summary>
        /// <param name="unitOfWork">Associated Unit Of Work</param>
        public EntityRepository(IQueryableUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException("unitOfWork");
            this.QueryableUnitOfWork = unitOfWork;
        }
        #endregion

        #region ·   Private Methods ·

        IDbSet<TEntity> GetSet()
        {
            return QueryableUnitOfWork.CreateSet<TEntity>();
        }
        #endregion

        #region ·   IRepository Members ·
        /// <summary>
        /// <see cref="T:RaraAvis.nCubed.DDD.Infrastructure.IQueryableUnitOfWork"/>
        /// </summary>
        public IUnitOfWork UnitOfWork
        {
            get
            {
                return QueryableUnitOfWork;
            }
        }
        /// <summary>
        /// <see cref="T:RaraAvis.nCubed.DDD.Core.IEntityRepository`1"/>
        /// </summary>
        /// <param name="item">A <see cref="T:RaraAvis.nCubed.DDD.Core.IEntityRepository`1"/>.</param>
        public virtual void Add(TEntity item)
        {
            DDDExceptionProcessor.ProcessDataAccess(() =>
                {
                    if (item == (TEntity)null)
                        throw new ArgumentNullException(String.Format(System.Globalization.CultureInfo.InvariantCulture, Resources.InfoCannotAddNullEntity, typeof(TEntity).ToString()));
                    else
                    {
                        GetSet().Add(item); // add new item in this set
                    }
                });
        }
        /// <summary>
        /// <see cref="T:RaraAvis.nCubed.DDD.Core.IEntityRepository`1"/>
        /// </summary>
        /// <param name="item">A <see cref="T:RaraAvis.nCubed.DDD.Core.IEntityRepository`1"/>.</param>
        public virtual void Remove(TEntity item)
        {
            DDDExceptionProcessor.ProcessDataAccess(() =>
                {
                    if (item == (TEntity)null)
                    {
                        throw new ArgumentNullException(String.Format(System.Globalization.CultureInfo.InvariantCulture, Resources.InfoCannotRemoveNullEntity, typeof(TEntity).ToString()));
                    }
                    else
                    {
                        //attach item if not exist
                        QueryableUnitOfWork.Attach(item);

                        //set as "removed"
                        GetSet().Remove(item);
                    }
                });
        }

        /// <summary>
        /// <see cref="T:RaraAvis.nCubed.DDD.Core.IEntityRepository`1"/>
        /// </summary>
        /// <param name="item">A <see cref="T:RaraAvis.nCubed.DDD.Core.IEntityRepository`1"/>.</param>
        public virtual void TrackItem(TEntity item)
        {
            DDDExceptionProcessor.ProcessDataAccess(() =>
                {
                    if (item == (TEntity)null)
                        throw new ArgumentNullException(String.Format(System.Globalization.CultureInfo.InvariantCulture, Resources.InfoCannotTrackNullEntity, typeof(TEntity).ToString()));
                    else
                    {
                        QueryableUnitOfWork.Attach<TEntity>(item);
                    }
                });
        }

        /// <summary>
        /// <see cref="T:RaraAvis.nCubed.DDD.Core.IEntityRepository`1"/>
        /// </summary>
        /// <param name="item">A <see cref="T:RaraAvis.nCubed.DDD.Core.IEntityRepository`1"/>.</param>
        public virtual void Modify(TEntity item)
        {
            DDDExceptionProcessor.ProcessDataAccess(() =>
                {
                    if (item == (TEntity)null)
                        throw new ArgumentNullException(String.Format(System.Globalization.CultureInfo.InvariantCulture, Resources.InfoCannotTrackNullEntity, typeof(TEntity).ToString()));
                    else
                    {
                        QueryableUnitOfWork.SetModified(item);
                    }
                });
        }
        /// <summary>
        /// <see cref="T:RaraAvis.nCubed.DDD.Core.IEntityRepository`1"/>
        /// </summary>
        /// <param name="id">A <see cref="T:RaraAvis.nCubed.DDD.Core.IEntityRepository`1"/>.</param>
        /// <returns>A <see cref="T:RaraAvis.nCubed.DDD.Core.IEntityRepository`1"/>.</returns>
        public virtual TEntity GetById(string id)
        {
            TEntity entity = default(TEntity);
            DDDExceptionProcessor.ProcessDataAccess(() =>
                {
                    entity = GetSet().Find(id);
                });
            return entity;
        }
        /// <summary>
        /// <see cref="T:RaraAvis.nCubed.DDD.Core.IEntityRepository`1"/>
        /// </summary>
        /// <param name="id">A <see cref="T:RaraAvis.nCubed.DDD.Core.IEntityRepository`1"/>.</param>
        /// <returns>A <see cref="T:RaraAvis.nCubed.DDD.Core.IEntityRepository`1"/>.</returns>
        public virtual TEntity GetById(long id)
        {
            TEntity entity = default(TEntity);
            DDDExceptionProcessor.ProcessDataAccess(() =>
                {
                    entity = GetSet().Find(id);
                });
            return entity;
        }
        /// <summary>
        /// <see cref="T:RaraAvis.nCubed.DDD.Core.IEntityRepository`1"/>
        /// </summary>
        /// <param name="id">A <see cref="T:RaraAvis.nCubed.DDD.Core.IEntityRepository`1"/>.</param>
        /// <returns>A <see cref="T:RaraAvis.nCubed.DDD.Core.IEntityRepository`1"/>.</returns>
        public virtual TEntity GetById(int id)
        {
            TEntity entity = default(TEntity);
            DDDExceptionProcessor.ProcessDataAccess(() =>
                {
                    entity = GetSet().Find(id);
                });
            return entity;
        }
        /// <summary>
        /// <see cref="T:RaraAvis.nCubed.DDD.Core.IEntityRepository`1"/>
        /// </summary>
        /// <param name="id">A <see cref="T:RaraAvis.nCubed.DDD.Core.IEntityRepository`1"/>.</param>
        /// <returns>A <see cref="T:RaraAvis.nCubed.DDD.Core.IEntityRepository`1"/>.</returns>
        public virtual TEntity GetById(Guid id)
        {
            TEntity entity = default(TEntity);
            DDDExceptionProcessor.ProcessDataAccess(() =>
                {
                    entity = id != Guid.Empty ? GetSet().Find(id) : null;
                });
            return entity;
        }
        /// <summary>
        /// <see cref="T:RaraAvis.nCubed.DDD.Core.IEntityRepository`1"/>
        /// </summary>
        /// <returns>A <see cref="T:RaraAvis.nCubed.DDD.Core.IEntityRepository`1"/>.</returns>
        public virtual IEnumerable<TEntity> All()
        {
            IEnumerable<TEntity> entities = default(IEnumerable<TEntity>);
            DDDExceptionProcessor.ProcessDataAccess(() =>
                {
                    entities = GetSet();
                }
            );
            return entities;
        }
        /// <summary>
        /// <see cref="T:RaraAvis.nCubed.DDD.Core.IEntityRepository`1"/>
        /// </summary>
        /// <param name="specification">A <see cref="T:RaraAvis.nCubed.DDD.Core.IEntityRepository`1"/>.</param>
        /// <returns>A <see cref="T:RaraAvis.nCubed.DDD.Core.IEntityRepository`1"/>.</returns>
        public virtual IEnumerable<TEntity> AllMatching(ISpecification<TEntity> specification)
        {
            return DDDExceptionProcessor.ProcessDomain<IEnumerable<TEntity>>(() =>
                {
                    return GetSet().Where(specification.SatisfiedBy());
                });
        }
        /// <summary>
        /// <see cref="T:RaraAvis.nCubed.DDD.Core.IEntityRepository`1"/>
        /// </summary>
        /// <typeparam name="T">A <see cref="T:RaraAvis.nCubed.DDD.Core.IEntityRepository`1"/>.</typeparam>
        /// <param name="pageIndex">A <see cref="T:RaraAvis.nCubed.DDD.Core.IEntityRepository`1"/>.</param>
        /// <param name="pageCount">A <see cref="T:RaraAvis.nCubed.DDD.Core.IEntityRepository`1"/>.</param>
        /// <param name="orderByExpression">A <see cref="T:RaraAvis.nCubed.DDD.Core.IEntityRepository`1"/>.</param>
        /// <param name="ascending">A <see cref="T:RaraAvis.nCubed.DDD.Core.IEntityRepository`1"/>.</param>
        /// <returns>A <see cref="T:RaraAvis.nCubed.DDD.Core.IEntityRepository`1"/>.</returns>
        public virtual IEnumerable<TEntity> GetPaged<T>(int pageIndex, int pageCount, System.Linq.Expressions.Expression<Func<TEntity, T>> orderByExpression, bool ascending)
        {
            var set = GetSet();

            return DDDExceptionProcessor.ProcessDomain<IEnumerable<TEntity>>(() =>
                {
                    if (ascending)
                    {
                        return set.OrderBy(orderByExpression)
                                  .Skip(pageCount * pageIndex)
                                  .Take(pageCount);
                    }
                    else
                    {
                        return set.OrderByDescending(orderByExpression)
                                  .Skip(pageCount * pageIndex)
                                  .Take(pageCount);
                    }
                });
        }
        /// <summary>
        /// <see cref="T:RaraAvis.nCubed.DDD.Core.IEntityRepository`1"/>
        /// </summary>
        /// <param name="filter">A <see cref="T:RaraAvis.nCubed.DDD.Core.IEntityRepository`1"/>.</param>
        /// <returns>A <see cref="T:RaraAvis.nCubed.DDD.Core.IEntityRepository`1"/>.</returns>
        public virtual IEnumerable<TEntity> GetFiltered(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter)
        {
            return DDDExceptionProcessor.ProcessDomain<IEnumerable<TEntity>>(() =>
                {
                    return GetSet().Where(filter);
                });
        }

        /// <summary>
        /// <see cref="T:RaraAvis.nCubed.DDD.Core.IEntityRepository`1"/>
        /// </summary>
        /// <param name="persisted">A <see cref="T:RaraAvis.nCubed.DDD.Core.IEntityRepository`1"/>.</param>
        /// <param name="current">A <see cref="T:RaraAvis.nCubed.DDD.Core.IEntityRepository`1"/>.</param>
        public virtual void Merge(TEntity persisted, TEntity current)
        {
            DDDExceptionProcessor.ProcessDataAccess(() =>
                {
                    QueryableUnitOfWork.ApplyCurrentValues(persisted, current);
                });
        }

        #endregion

        #region ·   IDisposable Members ·
        bool disposed = false;
        /// <summary>
        /// <see cref="M:System.IDisposable.Dispose"/>
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// <see cref="M:System.IDisposable.Dispose"/>
        /// </summary>
        /// <param name="disposing">False if unmanaged resources must be disposed, false otherwise.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                QueryableUnitOfWork.Dispose();
            }
            disposed = true;
        }
        #endregion
    }
}
