using RaraAvis.nCubed.DDD.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.DDD.Infrastructure
{
    /// <summary>
    /// The UnitOfWork contract for EF implementation
    /// <remarks>
    /// This contract extend IUnitOfWork for use with EF code
    /// </remarks>
    /// </summary>
    public interface IQueryableUnitOfWork
        : IUnitOfWork, ISql
    {
        /// <summary>
        /// Returns a IDbSet instance for access to entities of the given type in the context, 
        /// the ObjectStateManager, and the underlying store. 
        /// </summary>
        /// <typeparam name="TEntity">Set type.</typeparam>
        /// <returns>A <see cref="T:System.Data.Entity.DbSet`1"/>.</returns>
        DbSet<TEntity> CreateSet<TEntity>() where TEntity : Entity;

        /// <summary>
        /// Attach this item into "ObjectStateManager".
        /// </summary>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <param name="item">The item to attach.</param>
        void Attach<TEntity>(TEntity item) where TEntity : Entity;

        /// <summary>
        /// Set object as modified
        /// </summary>
        /// <typeparam name="TEntity">The type of entity</typeparam>
        /// <param name="item">The entity item to set as modifed</param>
        void SetModified<TEntity>(TEntity item) where TEntity : Entity;

        /// <summary>
        /// Apply current values in <paramref name="original"/>
        /// </summary>
        /// <typeparam name="TEntity">The type of entity</typeparam>
        /// <param name="original">The original entity</param>
        /// <param name="current">The current entity</param>
        void ApplyCurrentValues<TEntity>(TEntity original, TEntity current) where TEntity : Entity;


    }
}
