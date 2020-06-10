using RaraAvis.nCubed.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.DDD.Core
{
    /// <summary>
    /// A base clase for entity objects.
    /// </summary>
    [Serializable]
    [InheritedExport]
    public abstract class Entity
    {
        #region ·   Members ·

        int? requestedHashCode;

        #endregion

        #region ·   Properties  ·

        /// <summary>
        /// Get the persisten object identifier
        /// </summary>
        public virtual Guid EntityId
        {
            get; protected set;
        }
        /// <summary>
        /// The entity root this entity belongs to.
        /// </summary>
        public IAggregateRoot AggregateRoot
        {
            get; protected set;
        }
        #endregion

        #region ·   Public Methods  ·

        /// <summary>
        /// Check if this entity is transient, ie, without identity at this moment
        /// </summary>
        /// <returns>True if entity is transient, else false</returns>
        public bool IsTransient
        {
            get
            {
                return this.EntityId == Guid.Empty;
            }
        }

        /// <summary>
        /// Generate identity for this entity
        /// </summary>
        public void GenerateNewIdentity()
        {
            if (IsTransient)
                this.EntityId = IdentityGenerator.NewSequentialGuid();
        }

        /// <summary>
        /// Change current identity for a new non transient identity
        /// </summary>
        /// <param name="identity">the new identity</param>
        public void ChangeCurrentIdentity(Guid identity)
        {
            if (identity != Guid.Empty)
                this.EntityId = identity;
        }

        #endregion

        #region ·   Overrides Methods   ·

        /// <summary>
        /// <see cref="M:System.Object.Equals(System.Object)"/>
        /// </summary>
        /// <param name="obj"> <see cref="M:System.Object.Equals(System.Object)"/>.</param>
        /// <returns> <see cref="M:System.Object.Equals(System.Object)"/>.</returns>
        public override bool Equals(object obj)
        {
            Entity item = (Entity)obj;

            if (obj == null)
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            if (item.IsTransient || this.IsTransient)
                return false;
            else
                return item.EntityId == this.EntityId;
        }

        /// <summary>
        /// <see cref="M:System.Object.GetHashCode"/>
        /// </summary>
        /// <returns> <see cref="M:System.Object.GetHashCode"/>.</returns>
        public override int GetHashCode()
        {
            if (!IsTransient)
            {
                if (!requestedHashCode.HasValue)
                    requestedHashCode = this.EntityId.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

                return requestedHashCode.Value;
            }
            else
                return base.GetHashCode();

        }
        /// <summary>
        /// Equals operator.
        /// </summary>
        /// <param name="left">Left operator.</param>
        /// <param name="right">Right operator.</param>
        /// <returns>True is satisfies, false otherwise.</returns>
        public static bool operator ==(Entity left, Entity right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }
        /// <summary>
        /// Not equals operator.
        /// </summary>
        /// <param name="left">Left operator.</param>
        /// <param name="right">Right operator.</param>
        /// <returns>True is satisfies, false otherwise.</returns>
        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }

        #endregion

        #region ·   Constructors    ·
        /// <summary>
        /// Set entity Id.
        /// </summary>
        /// <param name="id">Entity Id.</param>
        protected Entity(Guid id)
        {
            this.EntityId = id;
        }
        /// <summary>
        /// Base contructor.
        /// </summary>
        /// <remarks>Generates a Guid.Empty Id to remark the fact that this entity is transient.</remarks>
        protected Entity()
        {

        }
        #endregion
    }
}
