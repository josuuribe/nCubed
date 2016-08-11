using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Adapter
{
    /// <summary>
    /// Base contract for map dto to aggregate or aggregate to dto.
    /// <remarks>
    /// This is a  contract for work with "auto" mappers ( automapper,emitmapper,valueinjecter...)
    /// or adhoc mappers
    /// </remarks>
    /// </summary>
    public interface ITypeAdapter
    {
        /// <summary>
        /// Adapt a source object to an instance of type <typeparamref name="TTarget"/>
        /// </summary>
        /// <typeparam name="TSource">Type of source item</typeparam>
        /// <typeparam name="TTarget">Type of target item</typeparam>
        /// <param name="item">Instance to adapt</param>
        /// <returns>A <paramref name="item"/> mapped to <typeparamref name="TTarget"/>.</returns>
        TTarget Project<TSource, TTarget>(TSource item)
            where TTarget : class,new()
            where TSource : class;

        /// <summary>
        /// Adapt a source object to an instance of type <typeparamref name="TTarget"/>
        /// </summary>
        /// <typeparam name="TSource">Type of source item</typeparam>
        /// <typeparam name="TTarget">Type of target item</typeparam>
        /// <param name="item">Element to be mapped.</param>
        /// <param name="customMapping">Function that performs custom mapping.</param>
        /// <returns>The mapped object.</returns>
        TTarget Project<TSource, TTarget>(TSource item, Func<TTarget, object, TTarget> customMapping)
            where TTarget : class,new()
            where TSource : class;

        /// <summary>
        /// Adapt a collection of source object to an instance of type <typeparamref name="TTarget"/>
        /// </summary>
        /// <typeparam name="TSource">Type of source item</typeparam>
        /// <typeparam name="TTarget">Type of target item</typeparam>
        /// <param name="items">A collection of elements to be mapped.</param>
        /// <returns>The mapped object.</returns>
        IEnumerable<TTarget> Project<TSource, TTarget>(IEnumerable<TSource> items)
            where TTarget : class,new()
            where TSource : class;

        /// <summary>
        /// Adapt a collection of source object to an instance of type <typeparamref name="TTarget"/>
        /// </summary>
        /// <typeparam name="TSource">Type of source item</typeparam>
        /// <typeparam name="TTarget">Type of target item</typeparam>
        /// <param name="items">A collection of elements to be mapped.</param>
        /// <param name="customMapping">Function that performs custom mapping.</param>
        /// <returns>The mapped object.</returns>
        IEnumerable<TTarget> Project<TSource, TTarget>(IEnumerable<TSource> items, Func<TTarget, object, TTarget> customMapping)
            where TSource : class
            where TTarget : class, new();
    }
}
