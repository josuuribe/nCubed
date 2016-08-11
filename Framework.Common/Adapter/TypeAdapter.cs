using EmitMapper;
using EmitMapper.MappingConfiguration;
using EmitMapper.MappingConfiguration.MappingOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Adapter
{
    /// <summary>
    /// A type adapter class.
    /// </summary>
    public class TypeAdapter : ITypeAdapter
    {
        /// <summary>
        /// A <see cref="TypeAdapter"/>	instance.
        /// </summary>
        private static TypeAdapter instance = null;
        /// <summary>
        /// Returns a singleton <see cref="TypeAdapter"/> instance.
        /// </summary>
        public static TypeAdapter Instance
        {
            get
            {
                return instance = instance ?? new TypeAdapter();
            }
        }
        /// <summary>
        /// Maps an object.
        /// </summary>
        /// <typeparam name="TSource">Type mapped.</typeparam>
        /// <typeparam name="TTarget">Type to map.</typeparam>
        /// <param name="item">Object to map.</param>
        /// <returns>Mapped object.</returns>
        public TTarget Project<TSource, TTarget>(TSource item)
            where TSource : class
            where TTarget : class, new()
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<TSource, TTarget>().Map(item);
        }
        /// <summary>
        /// Map a list of objects.
        /// </summary>
        /// <typeparam name="TSource">Type mapped.</typeparam>
        /// <typeparam name="TTarget">Type to map.</typeparam>
        /// <param name="items">List of items to map.</param>
        /// <returns>Mapped list.</returns>
        public IEnumerable<TTarget> Project<TSource, TTarget>(IEnumerable<TSource> items)
            where TSource : class
            where TTarget : class, new()
        {
            var mapper = ObjectMapperManager.DefaultInstance.GetMapper<TSource, TTarget>();
            return items.Select(mapper.Map);
        }
        /// <summary>
        /// Maps an object using a custom function.
        /// </summary>
        /// <typeparam name="TSource">Type mapped.</typeparam>
        /// <typeparam name="TTarget">Type to map.</typeparam>
        /// <param name="item">Object to map.</param>
        /// <param name="customMapping">Custom mapping function.</param>
        /// <returns>A TTarget object.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Error is intentional")]
        public TTarget Project<TSource, TTarget>(TSource item, Func<TTarget, object, TTarget> customMapping)
            where TSource : class
            where TTarget : class, new()
        {
            ValuesPostProcessor<TTarget> vpp = new ValuesPostProcessor<TTarget>(customMapping);
            return ObjectMapperManager.DefaultInstance.GetMapper<TSource, TTarget>(DefaultMapConfig.Instance.PostProcess<TTarget>(vpp).SetConfigName(customMapping.Method.ToString())).Map(item);
        }
        /// <summary>
        /// Maps a list of objects using a custom function.
        /// </summary>
        /// <typeparam name="TSource">Type mapped.</typeparam>
        /// <typeparam name="TTarget">Type to map.</typeparam>
        /// <param name="items">List of items to map.</param>
        /// <param name="customMapping">Custom mapping function.</param>
        /// <returns>Mapped list.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Error is intentional")]
        public IEnumerable<TTarget> Project<TSource, TTarget>(IEnumerable<TSource> items, Func<TTarget, object, TTarget> customMapping)
            where TSource : class
            where TTarget : class, new()
        {
            var mapper = ObjectMapperManager.DefaultInstance.GetMapper<TSource, TTarget>(DefaultMapConfig.Instance.PostProcess<TTarget>(new ValuesPostProcessor<TTarget>(customMapping)).SetConfigName(customMapping.Method.ToString()));
            return items.Select(mapper.Map);
        }
    }
}
