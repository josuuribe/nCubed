using RaraAvis.nCubed.Core.Adapter;
using RaraAvis.nCubed.CQRS.Core.Commands;
using RaraAvis.nCubed.CQRS.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.CQRS.Core.Services
{
    /// <summary>
    /// Extension class that projects <see cref="T:RaraAvis.nCubed.CQRS.Core.Commands.ICommand"/> into <see cref="T:RaraAvis.nCubed.CQRS.Core.Services.CommandDto"/> and viceversa.
    /// </summary>
    public static class Projections
    {
        /// <summary>
        /// Projects a <see cref="T:RaraAvis.nCubed.CQRS.Core.Services.CommandDto"/> element.
        /// </summary>
        /// <typeparam name="TSource">The source <see cref="T:RaraAvis.nCubed.CQRS.Core.Services.CommandDto"/> object.</typeparam>
        /// <typeparam name="TTarget">The target <see cref="T:RaraAvis.nCubed.CQRS.Core.Commands.ICommand"/> object.</typeparam>
        /// <param name="item">The <see cref="T:RaraAvis.nCubed.CQRS.Core.Services.CommandDto"/> object to be mapped.</param>
        /// <returns>The target <see cref="T:RaraAvis.nCubed.CQRS.Core.Commands.ICommand"/> object.</returns>
        public static TTarget ProjectAsCommand<TSource, TTarget>(this TSource item)
            where TSource : CommandDto
            where TTarget : class, ICommand, new()
        {
            return TypeAdapter.Instance.Project<TSource, TTarget>((TSource)item);
        }
        /// <summary>
        /// Projects a list of <see cref="T:RaraAvis.nCubed.CQRS.Core.Services.CommandDto"/>.
        /// </summary>
        /// <typeparam name="TSource">The source <see cref="T:RaraAvis.nCubed.CQRS.Core.Services.CommandDto"/> object.</typeparam>
        /// <typeparam name="TTarget">The target <see cref="T:RaraAvis.nCubed.CQRS.Core.Commands.ICommand"/> object.</typeparam>
        /// <param name="items">The list of <see cref="T:RaraAvis.nCubed.CQRS.Core.Services.CommandDto"/> objects mapped.</param>
        /// <returns>The list of <see cref="T:RaraAvis.nCubed.CQRS.Core.Commands.ICommand"/> objects.</returns>
        public static IEnumerable<TTarget> ProjectAsCommand<TSource, TTarget>(this IEnumerable<TSource> items)
            where TSource : CommandDto
            where TTarget : class, ICommand, new()
        {
            return TypeAdapter.Instance.Project<TSource, TTarget>(items.Cast<TSource>());
        }
        /// <summary>
        /// Projects a <see cref="T:RaraAvis.nCubed.CQRS.Core.Services.CommandDto"/> using a custom converter.
        /// </summary>
        /// <typeparam name="TSource">The source <see cref="T:RaraAvis.nCubed.CQRS.Core.Services.CommandDto"/>	object.</typeparam>
        /// <typeparam name="TTarget">The target <see cref="T:RaraAvis.nCubed.CQRS.Core.Commands.ICommand"/> object.</typeparam>
        /// <param name="item">The <see cref="T:RaraAvis.nCubed.CQRS.Core.Services.CommandDto"/> object to be mapped.</param>
        /// <param name="customMapping">A expression that processes the mapping.</param>
        /// <returns>A <see cref="T:RaraAvis.nCubed.CQRS.Core.Commands.ICommand"/> object.</returns>
        public static TTarget ProjectAsCommand<TSource, TTarget>(this TSource item, Func<TTarget, object, TTarget> customMapping)
            where TSource : CommandDto
            where TTarget : class, ICommand, new()
        {
            return TypeAdapter.Instance.Project<TSource, TTarget>((TSource)item, customMapping);
        }
        /// <summary>
        /// Projects a list of <see cref="T:RaraAvis.nCubed.CQRS.Core.Services.CommandDto"/> using a custom converter.
        /// </summary>
        /// <typeparam name="TSource">The source <see cref="T:RaraAvis.nCubed.CQRS.Core.Services.CommandDto"/> object.</typeparam>
        /// <typeparam name="TTarget">The target <see cref="T:RaraAvis.nCubed.CQRS.Core.Commands.ICommand"/>	object.</typeparam>
        /// <param name="items">The list of <see cref="T:RaraAvis.nCubed.CQRS.Core.Services.CommandDto"/> object to be mapped.</param>
        /// <param name="customMapping">A expression that processes the mapping.</param>
        /// <returns>A list of <see cref="T:RaraAvis.nCubed.CQRS.Core.Commands.ICommand"/> objects.</returns>
        public static IEnumerable<TTarget> ProjectAsCommand<TSource, TTarget>(this IEnumerable<TSource> items, Func<TTarget, object, TTarget> customMapping)
            where TSource : CommandDto
            where TTarget : class, ICommand, new()
        {
            return TypeAdapter.Instance.Project<TSource, TTarget>(items.Cast<TSource>(), customMapping);
        }
        /// <summary>
        /// Projects a <see cref="T:RaraAvis.nCubed.CQRS.Core.Commands.ICommand"/> element.
        /// </summary>
        /// <typeparam name="TSource">The source <see cref="T:RaraAvis.nCubed.CQRS.Core.Commands.ICommand"/>	object.</typeparam>
        /// <typeparam name="TTarget">The target <see cref="T:RaraAvis.nCubed.CQRS.Core.Services.CommandDto"/> object.</typeparam>
        /// <param name="item">The <see cref="T:RaraAvis.nCubed.CQRS.Core.Commands.ICommand"/> object to be mapped.</param>
        /// <returns>Mapped <see cref="T:RaraAvis.nCubed.CQRS.Core.Services.CommandDto"/> object.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dto")]
        public static TTarget ProjectAsDto<TSource, TTarget>(this TSource item)
            where TSource : class, ICommand
            where TTarget : CommandDto, new()
        {
            return TypeAdapter.Instance.Project<TSource, TTarget>((TSource)item);
        }
        /// <summary>
        /// Projects a list of <see cref="T:RaraAvis.nCubed.CQRS.Core.Commands.ICommand"/>.
        /// </summary>
        /// <typeparam name="TSource">The source <see cref="T:RaraAvis.nCubed.CQRS.Core.Commands.ICommand"/> object.</typeparam>
        /// <typeparam name="TTarget">The target <see cref="T:RaraAvis.nCubed.CQRS.Core.Services.CommandDto"/> object.</typeparam>
        /// <param name="items">The list of <see cref="T:RaraAvis.nCubed.CQRS.Core.Commands.ICommand"/> objects mapped.</param>
        /// <returns>The list of <see cref="T:RaraAvis.nCubed.CQRS.Core.Services.CommandDto"/> objects.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dto")]
        public static IEnumerable<TTarget> ProjectAsDto<TSource, TTarget>(this IEnumerable<TSource> items)
            where TSource : class, ICommand
            where TTarget : CommandDto, new()
        {
            return TypeAdapter.Instance.Project<TSource, TTarget>(items.Cast<TSource>());
        }
        /// <summary>
        /// Projects a <see cref="T:RaraAvis.nCubed.CQRS.Core.Commands.ICommand"/> using a custom converter.
        /// </summary>
        /// <typeparam name="TSource">The source <see cref="T:RaraAvis.nCubed.CQRS.Core.Commands.ICommand"/> object.</typeparam>
        /// <typeparam name="TTarget">The target <see cref="T:RaraAvis.nCubed.CQRS.Core.Services.CommandDto"/> object.</typeparam>
        /// <param name="item">The <see cref="T:RaraAvis.nCubed.CQRS.Core.Commands.ICommand"/> object to be mapped.</param>
        /// <param name="customMapping">A expression that processes the mapping.</param>
        /// <returns>A <see cref="T:RaraAvis.nCubed.CQRS.Core.Services.CommandDto"/> object.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dto")]
        public static TTarget ProjectAsDto<TSource, TTarget>(this TSource item, Func<TTarget, object, TTarget> customMapping)
            where TSource : class, ICommand
            where TTarget : CommandDto, new()
        {
            return TypeAdapter.Instance.Project<TSource, TTarget>((TSource)item, customMapping);
        }
        /// <summary>
        /// Projects a list of <see cref="T:RaraAvis.nCubed.CQRS.Core.Commands.ICommand"/> using a custom converter.
        /// </summary>
        /// <typeparam name="TSource">The source <see cref="T:RaraAvis.nCubed.CQRS.Core.Commands.ICommand"/> object.</typeparam>
        /// <typeparam name="TTarget">The target <see cref="T:RaraAvis.nCubed.CQRS.Core.Services.CommandDto"/> object.</typeparam>
        /// <param name="items">The list of <see cref="T:RaraAvis.nCubed.CQRS.Core.Commands.ICommand"/> object to be mapped.</param>
        /// <param name="customMapping">A expression that processes the mapping.</param>
        /// <returns>A list of <see cref="T:RaraAvis.nCubed.CQRS.Core.Services.CommandDto"/> objects.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dto")]
        public static IEnumerable<TTarget> ProjectAsDto<TSource, TTarget>(this IEnumerable<TSource> items, Func<TTarget, object, TTarget> customMapping)
            where TSource : class, ICommand
            where TTarget : CommandDto, new()
        {
            return TypeAdapter.Instance.Project<TSource, TTarget>(items.Cast<TSource>(), customMapping);
        }
    }
}
