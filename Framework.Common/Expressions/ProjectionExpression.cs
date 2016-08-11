using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Expressions
{
    /// <summary>
    /// Class that projects an expression.
    /// </summary>
    /// <typeparam name="TSource">Source type to be projected.</typeparam>
    public class ProjectionExpression<TSource>
    {
        /// <summary>
        /// Dictionary that stores a string and an expression.
        /// </summary>
        private static readonly Dictionary<string, Expression> ExpressionCache = new Dictionary<string, Expression>();
        /// <summary>
        /// IQueryable source.
        /// </summary>
        private readonly IQueryable<TSource> source;
        /// <summary>
        /// Base constructor.
        /// </summary>
        /// <param name="source">IQueryable source.</param>
        public ProjectionExpression(IQueryable<TSource> source)
        {
            this.source = source;
        }
        /// <summary>
        /// Projects to method.
        /// </summary>
        /// <typeparam name="TTarget">Destination type.</typeparam>
        /// <returns>A IQueryable source</returns>
        public IQueryable<TTarget> To<TTarget>()
        {
            var queryExpression = GetCachedExpression<TTarget>() ?? BuildExpression<TTarget>();

            return source.Select(queryExpression);
        }
        /// <summary>
        /// Gets a cached expression.
        /// </summary>
        /// <typeparam name="TDest">Destination type.</typeparam>
        /// <returns>A IQueryable source</returns>
        private static Expression<Func<TSource, TDest>> GetCachedExpression<TDest>()
        {
            var key = GetCacheKey<TDest>();

            return ExpressionCache.ContainsKey(key) ? ExpressionCache[key] as Expression<Func<TSource, TDest>> : null;
        }
        /// <summary>
        /// Builds an expression.
        /// </summary>
        /// <typeparam name="TDest">Destination type.</typeparam>
        /// <returns>A IQueryable source</returns>
        private static Expression<Func<TSource, TDest>> BuildExpression<TDest>()
        {
            var sourceProperties = typeof(TSource).GetProperties();
            var destinationProperties = typeof(TDest).GetProperties().Where(dest => dest.CanWrite);
            var parameterExpression = Expression.Parameter(typeof(TSource), "src");

            var bindings = destinationProperties
                                .Select(destinationProperty => BuildBinding(parameterExpression, destinationProperty, sourceProperties))
                                .Where(binding => binding != null);

            var expression = Expression.Lambda<Func<TSource, TDest>>(Expression.MemberInit(Expression.New(typeof(TDest)), bindings), parameterExpression);

            var key = GetCacheKey<TDest>();

            ExpressionCache.Add(key, expression);

            return expression;
        }
        /// <summary>
        /// Builds a binding.
        /// </summary>
        /// <param name="parameterExpression">Parameter expression.</param>
        /// <param name="destinationProperty">Destination property.</param>
        /// <param name="sourceProperties">Source properties.</param>
        /// <returns>A member assignment.</returns>
        private static MemberAssignment BuildBinding(Expression parameterExpression, MemberInfo destinationProperty, IEnumerable<PropertyInfo> sourceProperties)
        {
            var sourceProperty = sourceProperties.FirstOrDefault(src => src.Name == destinationProperty.Name);

            if (sourceProperty != null)
            {
                return Expression.Bind(destinationProperty, Expression.Property(parameterExpression, sourceProperty));
            }

            var propertyNames = SplitCamelCase(destinationProperty.Name);

            if (propertyNames.Length == 2)
            {
                sourceProperty = sourceProperties.FirstOrDefault(src => src.Name == propertyNames[0]);

                if (sourceProperty != null)
                {
                    var sourceChildProperty = sourceProperty.PropertyType.GetProperties().FirstOrDefault(src => src.Name == propertyNames[1]);

                    if (sourceChildProperty != null)
                    {
                        return Expression.Bind(destinationProperty, Expression.Property(Expression.Property(parameterExpression, sourceProperty), sourceChildProperty));
                    }
                }
            }

            return null;
        }
        /// <summary>
        /// Gets a cached key.
        /// </summary>
        /// <typeparam name="TDest">Destination type.</typeparam>
        /// <returns>A string with cached key.</returns>
        private static string GetCacheKey<TDest>()
        {
            return string.Concat(typeof(TSource).FullName, typeof(TDest).FullName);
        }
        /// <summary>
        /// Split camel case.
        /// </summary>
        /// <param name="input">Input string to split.</param>
        /// <returns>A string matrix.</returns>
        private static string[] SplitCamelCase(string input)
        {
            return Regex.Replace(input, "([A-Z])", " $1", RegexOptions.Compiled).Trim().Split(' ');
        }
    }

}
