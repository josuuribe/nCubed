using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Expressions
{
    /// <summary>
    /// Extension class for querying.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Queryable sentence.
        /// </summary>
        /// <typeparam name="TSource">Type source.</typeparam>
        /// <param name="source">Source object.</param>
        /// <returns>A projection expression object.</returns>
        public static ProjectionExpression<TSource> Bind<TSource>(this IQueryable<TSource> source)
        {
            return new ProjectionExpression<TSource>(source);
        }
    }
}
