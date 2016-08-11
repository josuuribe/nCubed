using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Expressions
{
    /// <summary>
    /// Combines expressions. Based on the original post by Colin Meek:
    /// http://blogs.msdn.com/b/meek/archive/2008/05/02/linq-to-entities-combining-predicates.aspx
    /// </summary>
    public static class CombinerExpression
    {
        /// <summary>
        /// And operator that joins two clauses.
        /// </summary>
        /// <typeparam name="T">Expression result.</typeparam>
        /// <param name="first">First expression to be joined.</param>
        /// <param name="second">Second expression to be joined.</param>
        /// <returns>A Expression combined.</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return Compose(first, second, Expression.AndAlso);
        }
        /// <summary>
        /// Or operator that joins two clauses.
        /// </summary>
        /// <typeparam name="T">Expression result.</typeparam>
        /// <param name="first">First expression to be joined.</param>
        /// <param name="second">Second expression to be joined.</param>
        /// <returns>A Expression combined.</returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return Compose(first, second, Expression.OrElse);
        }
        /// <summary>
        /// Compose two expressions.
        /// </summary>
        /// <typeparam name="T">Expression type to compose.</typeparam>
        /// <param name="first">First expression to compose.</param>
        /// <param name="second">Second expression to compose.</param>
        /// <param name="merge">Merge expression.</param>
        /// <returns>Composed expression.</returns>
        private static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // build parameter map (from parameters of second to parameters of first)
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with parameters from the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // apply composition of lambda expression bodies to parameters from the first expression 
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }
        /// <summary>
        /// Helper class for parameters.
        /// </summary>
        partial class ParameterRebinder : ExpressionVisitor
        {
            /// <summary>
            /// Class that maps a Dictionary.
            /// </summary>
            private readonly Dictionary<ParameterExpression, ParameterExpression> map;
            /// <summary>
            /// Base constructor.
            /// </summary>
            /// <param name="map">Dictionary for mapping.</param>
            public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
            {
                this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
            }
            /// <summary>
            /// Replaces parameters.
            /// </summary>
            /// <param name="map">Dictionary that maps elements.</param>
            /// <param name="exp">Visit parameter.</param>
            /// <returns>Expression replaced.</returns>
            public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
            {
                return new ParameterRebinder(map).Visit(exp);
            }
            /// <summary>
            /// Visit parameter.
            /// </summary>
            /// <param name="p">Parameter to be get.</param>
            /// <returns>Expression replaced.</returns>
            protected override Expression VisitParameter(ParameterExpression p)
            {
                ParameterExpression replacement;
                if (map.TryGetValue(p, out replacement))
                {
                    p = replacement;
                }
                return base.VisitParameter(p);
            }
        }
    }

}
