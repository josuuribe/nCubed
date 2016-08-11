using RaraAvis.nCubed.Core.Exceptions;
using RaraAvis.nCubed.Core.Infrastructure;
using RaraAvis.nCubed.Core.Infrastructure.Sql;
using RaraAvis.nCubed.DDD.Core;
using RaraAvis.nCubed.DDD.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.DDD.Infrastructure
{
    /// <summary>
    /// Unit of Work pattern.
    /// </summary>
    [DbConfigurationType(typeof(DbCommonConfiguration))]
    [ExcludeFromCodeCoverage]
    public abstract class UnitOfWork : DbContext, IQueryableUnitOfWork
    {
        #region ·   IUnitOfWork ·
        /// <summary>
        /// Crate set.
        /// </summary>
        /// <typeparam name="TEntity">Entoty type set to create.</typeparam>
        /// <returns>A DbSet.</returns>
        public virtual DbSet<TEntity> CreateSet<TEntity>() where TEntity : Entity
        {
            DbSet<TEntity> set = default(DbSet<TEntity>);
            DDDExceptionProcessor.ProcessDataAccess(
                () => set = base.Set<TEntity>()
            );
            return set;
        }
        /// <summary>
        /// Attachs an entity to DbSet.
        /// </summary>
        /// <typeparam name="TEntity">Entity type to attach.</typeparam>
        /// <param name="item">Entity to attach.</param>
        public virtual void Attach<TEntity>(TEntity item) where TEntity : Entity
        {
            //attach and set as unchanged
            DDDExceptionProcessor.ProcessDataAccess(
                () =>
                {
                    base.Entry<TEntity>(item).State = System.Data.Entity.EntityState.Unchanged;
                }
            );
        }
        /// <summary>
        /// Set entity as modified.
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <param name="item">Entity to establish.</param>
        public virtual void SetModified<TEntity>(TEntity item) where TEntity : Entity
        {
            //this operation also attach item in object state manager
            DDDExceptionProcessor.ProcessDataAccess(
            () => base.Entry<TEntity>(item).State = System.Data.Entity.EntityState.Modified
            );
        }
        /// <summary>
        /// Apply values to entity.
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <param name="original">Original values.</param>
        /// <param name="current">Current values.</param>
        public virtual void ApplyCurrentValues<TEntity>(TEntity original, TEntity current) where TEntity : Entity
        {
            //if it is not attached, attach original and set current values
            DDDExceptionProcessor.ProcessDataAccess(
            () => base.Entry<TEntity>(original).CurrentValues.SetValues(current)
            );
        }
        /// <summary>
        /// Commits object.
        /// </summary>
        public virtual void Commit()
        {
            DDDExceptionProcessor.ProcessDataAccess(() =>
            {
                base.SaveChanges();
            });
        }
        /// <summary>
        /// Commits object and refresh changes.
        /// </summary>
        public virtual void CommitAndRefreshChanges()
        {
            bool saveFailed = false;

            do
            {
                try
                {
                    DDDExceptionProcessor.ProcessDataAccess(() =>
                        {
                            base.SaveChanges();

                            saveFailed = false;
                        });
                }
                catch (SemanticException sex)
                {
                    DbUpdateConcurrencyException ex = sex.InnerException as DbUpdateConcurrencyException;

                    saveFailed = true;

                    ex.Entries.ToList()
                              .ForEach(entry =>
                              {
                                  entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                              });
                }
            } while (saveFailed);

        }
        /// <summary>
        /// Roll back changes.
        /// </summary>
        public virtual void RollbackChanges()
        {
            // set all entities in change tracker 
            // as 'unchanged state'
            DDDExceptionProcessor.ProcessDataAccess(
                () =>
            base.ChangeTracker.Entries()
                              .ToList()
                              .ForEach(entry => entry.State = System.Data.Entity.EntityState.Unchanged)
                              );
        }
        /// <summary>
        /// Execute query.
        /// </summary>
        /// <typeparam name="TEntity">Entity returned.</typeparam>
        /// <param name="sqlQuery">Query to execute.</param>
        /// <param name="parameters">Query parameters.</param>
        /// <returns>A collection of entity objects.</returns>
        public virtual IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters)
        {
            IEnumerable<TEntity> entities = default(IEnumerable<TEntity>);
            DDDExceptionProcessor.ProcessDataAccess(
                () => entities = base.Database.SqlQuery<TEntity>(sqlQuery, parameters)
            );
            return entities;
        }
        /// <summary>
        /// Execute command.
        /// </summary>
        /// <param name="sqlCommand">Sql command.</param>
        /// <param name="parameters">Aql parameters.</param>
        /// <returns>Sql value returned.</returns>
        public virtual int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            int res = default(int);
            DDDExceptionProcessor.ProcessDataAccess(
                () => res = base.Database.ExecuteSqlCommand(sqlCommand, parameters)
                );
            return res;
        }
        /// <summary>
        /// <see cref="M:System.IDisposable.Dispose"/>
        /// </summary>
        /// <param name="disposing">False if unmanaged resources must be disposed, false otherwise.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        #endregion
    }
}
