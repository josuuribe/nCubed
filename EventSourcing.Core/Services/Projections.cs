using RaraAvis.nCubed.Core.Adapter;
using RaraAvis.nCubed.EventSourcing.Core.Events;
using RaraAvis.nCubed.EventSourcing.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.EventSourcing.Core.Services
{
    /// <summary>
    /// Extension class that projects <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Events.IEvent"/> into <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Services.EventDto"/> and viceversa.
    /// </summary>
    public static class Projections
    {
        /// <summary>
        /// Projects a <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Services.EventDto"/> element.
        /// </summary>
        /// <typeparam name="TSource">The source <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Services.EventDto"/> object.</typeparam>
        /// <typeparam name="TTarget">The target <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Events.IEvent"/> object.</typeparam>
        /// <param name="item">The <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Services.EventDto"/> object to be mapped.</param>
        /// <returns>A <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Events.IEvent"/> object.</returns>
        public static TTarget ProjectAsEvent<TSource, TTarget>(this TSource item)
            where TSource : EventDto
            where TTarget : class, IEvent, new()
        {
            return TypeAdapter.Instance.Project<TSource, TTarget>((TSource)item);
        }
        /// <summary>
        /// Projects a <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Services.EventDto"/> collection.
        /// </summary>
        /// <typeparam name="TSource">The source <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Services.EventDto"/> object.</typeparam>
        /// <typeparam name="TTarget">The target <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Events.IEvent"/> object.</typeparam>
        /// <param name="items">The <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Services.EventDto"/> collection to be mapped.</param>
        /// <returns>A <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Events.IEvent"/> collection.</returns>
        public static IEnumerable<TTarget> ProjectAsEvent<TSource, TTarget>(this IEnumerable<TSource> items)
            where TSource : EventDto
            where TTarget : class, IEvent, new()
        {
            return TypeAdapter.Instance.Project<TSource, TTarget>(items.Cast<TSource>());
        }
        /// <summary>
        /// Projects a <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Services.EventDto"/> item using a custom function.
        /// </summary>
        /// <typeparam name="TSource">The source <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Services.EventDto"/> object.</typeparam>
        /// <typeparam name="TTarget">The target <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Events.IEvent"/> object.</typeparam>
        /// <param name="item">A <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Services.EventDto"/>.</param>
        /// <param name="customMapping">Custom funcion to use.</param>
        /// <returns>The source <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Services.EventDto"/> object.</returns>
        public static TTarget ProjectAsEvent<TSource, TTarget>(this TSource item, Func<TTarget, object, TTarget> customMapping)
            where TSource : EventDto
            where TTarget : class, IEvent, new()
        {
            return TypeAdapter.Instance.Project<TSource, TTarget>((TSource)item, customMapping);
        }
        /// <summary>
        /// Projects a <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Services.EventDto"/> collection using a custom function.
        /// </summary>
        /// <typeparam name="TSource">The source <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Services.EventDto"/> object.</typeparam>
        /// <typeparam name="TTarget">The target <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Events.IEvent"/> object.</typeparam>
        /// <param name="items">An <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Services.EventDto"/> collection.</param>
        /// <param name="customMapping">Custom funcion to use.</param>
        /// <returns>>A <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Events.IEvent"/> collection.</returns>
        public static IEnumerable<TTarget> ProjectAsEvent<TSource, TTarget>(this IEnumerable<TSource> items, Func<TTarget, object, TTarget> customMapping)
            where TSource : EventDto
            where TTarget : class, IEvent, new()
        {
            return TypeAdapter.Instance.Project<TSource, TTarget>(items.Cast<TSource>(), customMapping);
        }
        /// <summary>
        /// Projects a <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Events.IEvent"/> element.
        /// </summary>
        /// <typeparam name="TSource">The target <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Events.IEvent"/> object.</typeparam>
        /// <typeparam name="TTarget">The source <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Services.EventDto"/> object.</typeparam>
        /// <param name="item">The <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Events.IEvent"/> object to be mapped.</param>
        /// <returns>An <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Events.IEvent"/> object.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dto")]
        public static TTarget ProjectAsDto<TSource, TTarget>(this TSource item)
            where TSource : class, IEvent
            where TTarget : EventDto, new()
        {
            return TypeAdapter.Instance.Project<TSource, TTarget>((TSource)item);
        }
        /// <summary>
        /// Projects a <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Events.IEvent"/> collection.
        /// </summary>
        /// <typeparam name="TSource">The target <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Events.IEvent"/> object.</typeparam>
        /// <typeparam name="TTarget">The source <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Services.EventDto"/> object.</typeparam>
        /// <param name="items">The <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Events.IEvent"/> collection to be mapped.</param>
        /// <returns>A <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Services.EventDto"/> collection.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dto")]
        public static IEnumerable<TTarget> ProjectAsDto<TSource, TTarget>(this IEnumerable<TSource> items)
            where TSource : class, IEvent, new()
            where TTarget : EventDto, new()
        {
            return TypeAdapter.Instance.Project<TSource, TTarget>(items.Cast<TSource>());
        }
        /// <summary>
        /// Projects a <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Events.IEvent"/> item using a custom function.
        /// </summary>
        /// <typeparam name="TSource">The source <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Events.IEvent"/> object.</typeparam>
        /// <typeparam name="TTarget">The target <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Services.EventDto"/> object.</typeparam>
        /// <param name="item">A DTO.</param>
        /// <param name="customMapping">Custom funcion to use.</param>
        /// <returns>The source <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Services.EventDto"/> object.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dto")]
        public static TTarget ProjectAsDto<TSource, TTarget>(this TSource item, Func<TTarget, object, TTarget> customMapping)
            where TSource : class, IEvent
            where TTarget : EventDto, new()
        {
            return TypeAdapter.Instance.Project<TSource, TTarget>((TSource)item, customMapping);
        }
        /// <summary>
        /// Projects a <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Services.EventDto"/> collection using a custom function.
        /// </summary>
        /// <typeparam name="TSource">The source <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Events.IEvent"/> object.</typeparam>
        /// <typeparam name="TTarget">The target <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Services.EventDto"/> object.</typeparam>
        /// <param name="items">An <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Events.IEvent"/> collection.</param>
        /// <param name="customMapping">Custom funcion to use.</param>
        /// <returns>A <see cref="T:RaraAvis.nCubed.EventSourcing.Core.Services.EventDto"/> collection.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dto")]
        public static IEnumerable<TTarget> ProjectAsDto<TSource, TTarget>(this IEnumerable<TSource> items, Func<TTarget, object, TTarget> customMapping)
            where TSource : class, IEvent
            where TTarget : EventDto, new()
        {
            return TypeAdapter.Instance.Project<TSource, TTarget>(items.Cast<TSource>(), customMapping);
        }
    }
}
