using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.DDD.Core
{
    /// <summary>
    /// Base class for value objects in domain.
    /// Value
    /// </summary>
    /// <typeparam name="TValueObject">The type of this value object</typeparam>
    [DataContract]
    public class ValueObject<TValueObject> : IEquatable<TValueObject>
        where TValueObject : ValueObject<TValueObject>
    {

        #region IEquatable and Override Equals operators

        /// <summary>
        /// <see cref="T:System.IEquatable`1"/>
        /// </summary>
        /// <param name="other"> <see cref="M:System.Object.Equals(System.Object)"/>.</param>
        /// <returns>True if equals, false otherwise."/>.</returns>
        public bool Equals(TValueObject other)
        {
            if ((object)other == null)
                return false;

            if (Object.ReferenceEquals(this, other))
                return true;

            //compare all public properties
            PropertyInfo[] publicProperties = this.GetType().GetProperties();

            return (publicProperties.All(p =>
                {
                    var left = p.GetValue(this, null);
                    var right = p.GetValue(other, null);

                    if (typeof(TValueObject).IsAssignableFrom(left.GetType()))
                    {
                        //check not self-references...
                        return Object.ReferenceEquals(left, right);
                    }
                    else
                    {
                        return left.Equals(right);
                    }
                }));
        }
        /// <summary>
        /// <see cref="M:System.Object.Equals(System.Object)"/>
        /// </summary>
        /// <param name="obj">Another object.</param>
        /// <returns> <see cref="M:System.Object.Equals(System.Object)"/>.</returns>
        public override bool Equals(object obj)
        {
            return Equals((TValueObject)obj);
        }
        /// <summary>
        /// <see cref="M:System.Object.GetHashCode"/>
        /// </summary>
        /// <returns> <see cref="M:System.Object.GetHashCode"/>.</returns>
        public override int GetHashCode()
        {
            int hashCode = 31;
            bool changeMultiplier = false;
            int index = 1;

            //compare all public properties
            PropertyInfo[] publicProperties = this.GetType().GetProperties();


            if (publicProperties != null)
            {
                foreach (var item in publicProperties)
                {
                    object value = item.GetValue(this, null);

                    if (value != null)
                    {

                        hashCode = hashCode * ((changeMultiplier) ? 59 : 114) + value.GetHashCode();

                        changeMultiplier = !changeMultiplier;
                    }
                    else
                    {
                        hashCode = hashCode ^ (index * 13);//only for support {"a",null,null,"a"} <> {null,"a","a",null}
                    }
                }
            }

            return hashCode;
        }
        /// <summary>
        /// Equals operator.
        /// </summary>
        /// <param name="left">Left operator.</param>
        /// <param name="right">Right operator.</param>
        /// <returns>True is satisfies, false otherwise.</returns>
        public static bool operator ==(ValueObject<TValueObject> left, ValueObject<TValueObject> right)
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
        public static bool operator !=(ValueObject<TValueObject> left, ValueObject<TValueObject> right)
        {
            return !(left == right);
        }

        #endregion
    }

}
