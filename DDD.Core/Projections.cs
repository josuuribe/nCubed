using RaraAvis.nCubed.DDD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaraAvis.nCubed.DDD.Core;
using RaraAvis.nCubed.Core.Adapter;
using RaraAvis.nCubed.DDD.Core.Services;

namespace RaraAvis.nCubed.DDD.Core
{
    /// <summary>
    /// Extension class that projects <see cref="T:RaraAvis.nCubed.DDD.Core.Entity"/> into <see cref="T:RaraAvis.nCubed.DDD.Core.Services.EntityDto"/> and viceversa.
    /// </summary>
    public static class Projections
    {        
        /// <summary>
        /// Projects a <see cref="T:RaraAvis.nCubed.DDD.Core.Services.EntityDto"/> element.
        /// </summary>
        /// <typeparam name="TSource">The source <see cref="T:RaraAvis.nCubed.DDD.Core.Services.EntityDto"/> object.</typeparam>
        /// <typeparam name="TTarget">The target <see cref="T:RaraAvis.nCubed.DDD.Core.Entity"/> object.</typeparam>
        /// <param name="item">The <see cref="T:RaraAvis.nCubed.DDD.Core.Services.EntityDto"/> object to be mapped.</param>
        /// <returns>The target <see cref="T:RaraAvis.nCubed.DDD.Core.Entity"/> object.</returns>
        public static TTarget ProjectAsEntity<TSource, TTarget>(this TSource item)
            where TSource : EntityDto, new()
            where TTarget : Entity, new()
        {
            return TypeAdapter.Instance.Project<TSource, TTarget>((TSource)item);
        }
        /// <summary>
        /// Projects a list of <see cref="T:RaraAvis.nCubed.DDD.Core.Services.EntityDto"/>.
        /// </summary>
        /// <typeparam name="TSource">The source <see cref="T:RaraAvis.nCubed.DDD.Core.Services.EntityDto"/> object.</typeparam>
        /// <typeparam name="TTarget">The target <see cref="T:RaraAvis.nCubed.DDD.Core.Entity"/> object.</typeparam>
        /// <param name="items">The list of <see cref="T:RaraAvis.nCubed.DDD.Core.Services.EntityDto"/> objects mapped.</param>
        /// <returns>The list of <see cref="T:RaraAvis.nCubed.DDD.Core.Entity"/> objects.</returns>
        public static IEnumerable<TTarget> ProjectAsEntity<TSource, TTarget>(this IEnumerable<TSource> items)
            where TSource : EntityDto, new()
            where TTarget : Entity, new()
        {
            return TypeAdapter.Instance.Project<TSource, TTarget>(items.Cast<TSource>());
        }
        /// <summary>
        /// Projects a list of <see cref="T:RaraAvis.nCubed.DDD.Core.Services.EntityDto"/> using a custom converter.
        /// </summary>
        /// <typeparam name="TSource">The source <see cref="T:RaraAvis.nCubed.DDD.Core.Services.EntityDto"/> object.</typeparam>
        /// <typeparam name="TTarget">The target <see cref="T:RaraAvis.nCubed.DDD.Core.Entity"/> object.</typeparam>
        /// <param name="item">The list of <see cref="T:RaraAvis.nCubed.DDD.Core.Services.EntityDto"/> object to be mapped.</param>
        /// <param name="customMapping">A expression that processes the mapping.</param>
        /// <returns>A list of <see cref="T:RaraAvis.nCubed.DDD.Core.Entity"/> objects.</returns>
        public static TTarget ProjectAsEntity<TSource, TTarget>(this TSource item, Func<TTarget, object, TTarget> customMapping)
            where TSource : EntityDto, new()
            where TTarget : Entity, new()
        {
            return TypeAdapter.Instance.Project<TSource, TTarget>((TSource)item, customMapping);
        }
        /// <summary>
        /// Projects a <see cref="T:RaraAvis.nCubed.DDD.Core.Entity"/> element.
        /// </summary>
        /// <typeparam name="TSource">The source <see cref="T:RaraAvis.nCubed.DDD.Core.Entity"/> object.</typeparam>
        /// <typeparam name="TTarget">The target <see cref="T:RaraAvis.nCubed.DDD.Core.Services.EntityDto"/> object.</typeparam>
        /// <param name="item">The <see cref="T:RaraAvis.nCubed.DDD.Core.Entity"/> object to be mapped.</param>
        /// <returns>Mapped <see cref="T:RaraAvis.nCubed.DDD.Core.Services.EntityDto"/> object.</returns>      
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dto")]
        public static TTarget ProjectAsDto<TSource, TTarget>(this TSource item)
            where TSource : Entity
            where TTarget : EntityDto, new()
        {
            return TypeAdapter.Instance.Project<TSource, TTarget>((TSource)item);
        }
        /// <summary>
        /// Projects a <see cref="T:RaraAvis.nCubed.DDD.Core.Entity"/> using a custom converter.
        /// </summary>
        /// <typeparam name="TSource">The source <see cref="T:RaraAvis.nCubed.DDD.Core.Entity"/> object.</typeparam>
        /// <typeparam name="TTarget">The target <see cref="T:RaraAvis.nCubed.DDD.Core.Services.EntityDto"/> object.</typeparam>
        /// <param name="items">The <see cref="T:RaraAvis.nCubed.DDD.Core.Entity"/> object to be mapped.</param>
        /// <returns>A <see cref="T:RaraAvis.nCubed.DDD.Core.Services.EntityDto"/> object.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dto")]
        public static IEnumerable<TTarget> ProjectAsDto<TSource, TTarget>(this IEnumerable<TSource> items)
            where TSource : Entity
            where TTarget : EntityDto, new()
        {
            return TypeAdapter.Instance.Project<TSource, TTarget>(items.Cast<TSource>());
        }
        /// <summary>
        /// Projects a list of <see cref="T:RaraAvis.nCubed.DDD.Core.Entity"/> using a custom converter.
        /// </summary>
        /// <typeparam name="TSource">The source <see cref="T:RaraAvis.nCubed.DDD.Core.Entity"/> object.</typeparam>
        /// <typeparam name="TTarget">The target <see cref="T:RaraAvis.nCubed.DDD.Core.Services.EntityDto"/> object.</typeparam>
        /// <param name="item">The list of <see cref="T:RaraAvis.nCubed.DDD.Core.Entity"/> object to be mapped.</param>
        /// <param name="customMapping">A expression that processes the mapping.</param>
        /// <returns>A list of <see cref="T:RaraAvis.nCubed.DDD.Core.Services.EntityDto"/> objects.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dto")]
        public static TTarget ProjectAsDto<TSource, TTarget>(this TSource item, Func<TTarget, object, TTarget> customMapping)
            where TSource : Entity
            where TTarget : EntityDto, new()
        {
            return TypeAdapter.Instance.Project<TSource, TTarget>((TSource)item, customMapping);
        }
        /// <summary>
        /// Projects a list of <see cref="T:RaraAvis.nCubed.DDD.Core.Entity"/>.
        /// </summary>
        /// <typeparam name="TSource">The source <see cref="T:RaraAvis.nCubed.DDD.Core.Entity"/> object.</typeparam>
        /// <typeparam name="TTarget">The target <see cref="T:RaraAvis.nCubed.DDD.Core.Services.EntityDto"/> object.</typeparam>
        /// <param name="items">The list of <see cref="T:RaraAvis.nCubed.DDD.Core.Entity"/> objects to be mapped.</param>
        /// <param name="customMapping">A expression that processes the mapping.</param>
        /// <returns>The list of <see cref="T:RaraAvis.nCubed.DDD.Core.Services.EntityDto"/> objects.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dto")]
        public static IEnumerable<TTarget> ProjectAsDto<TSource, TTarget>(this IEnumerable<TSource> items, Func<TTarget, object, TTarget> customMapping)
            where TSource : Entity
            where TTarget : EntityDto, new()
        {
            return TypeAdapter.Instance.Project<TSource, TTarget>(items.Cast<TSource>(), customMapping);
        }
        /// <summary>
        /// Projects a list of <see cref="T:RaraAvis.nCubed.DDD.Core.Services.EntityDto"/> using a custom converter.
        /// </summary>
        /// <typeparam name="TSource">The source <see cref="T:RaraAvis.nCubed.DDD.Core.Services.EntityDto"/> object.</typeparam>
        /// <typeparam name="TTarget">The target <see cref="T:RaraAvis.nCubed.DDD.Core.Entity"/> object.</typeparam>
        /// <param name="items">The list of <see cref="T:RaraAvis.nCubed.DDD.Core.Services.EntityDto"/> object to be mapped.</param>
        /// <param name="customMapping">A expression that processes the mapping.</param>
        /// <returns>A list of <see cref="T:RaraAvis.nCubed.DDD.Core.Entity"/> objects.</returns>
        public static IEnumerable<TTarget> ProjectAsEntity<TSource, TTarget>(this IEnumerable<TSource> items, Func<TTarget, object, TTarget> customMapping)
            where TSource : EntityDto, new()
            where TTarget : Entity, new()
        {
            return TypeAdapter.Instance.Project<TSource, TTarget>(items.Cast<TSource>(), customMapping);
        }
    }
}
