using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Expressions
{
    /// <summary>
    /// Prevents double enumeration (and potential roundtrip to the data source) when checking 
    /// for the presence of items in an enumeration.
    /// </summary>
    public static class CacheAnyEnumerableExtensions
    {
        /// <summary>
        /// Makes sure that calls to <see cref="M:RaraAvis.nCubed.Core.Expressions.IAnyEnumerableCollection`1.Any"/> are 
        /// cached, and reuses the resulting enumerator.
        /// </summary>
        /// <typeparam name="T">Source type list.</typeparam>
        /// <param name="source">Source list.</param>
        /// <returns>A <see cref="T:RaraAvis.nCubed.Core.Expressions.IAnyEnumerableCollection`1"/>.</returns>
        public static IAnyEnumerableCollection<T> AsCachedAnyEnumerable<T>(this IEnumerable<T> source)
        {
            return new AnyEnumerable<T>(source);
        }
        /// <summary>
        /// Lazily computes whether the inner enumerable has 
        /// any values, and caches the result.
        /// </summary>
        /// <typeparam name="T">Type for collection.</typeparam>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
        private class AnyEnumerable<T> : IAnyEnumerableCollection<T>
        {
            private readonly IEnumerable<T> enumerable;
            private IEnumerator<T> enumerator;
            private bool hasAny;
            public AnyEnumerable(IEnumerable<T> enumerable)
            {
                this.enumerable = enumerable;
            }

            public bool Any()
            {
                this.InitializeEnumerator();

                return this.hasAny;
            }

            public IEnumerator<T> GetEnumerator()
            {
                this.InitializeEnumerator();

                return this.enumerator;
            }

            private void InitializeEnumerator()
            {
                if (this.enumerator == null)
                {
                    var inner = this.enumerable.GetEnumerator();
                    this.hasAny = inner.MoveNext();
                    this.enumerator = new SkipFirstEnumerator(inner, this.hasAny);
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            private class SkipFirstEnumerator : IEnumerator<T>
            {
                private readonly IEnumerator<T> inner;
                private readonly bool hasNext;
                private bool isFirst = true;

                public SkipFirstEnumerator(IEnumerator<T> inner, bool hasNext)
                {
                    this.inner = inner;
                    this.hasNext = hasNext;
                }

                public T Current { get { return this.inner.Current; } }

                public void Dispose()
                {
                    this.inner.Dispose();
                }

                object IEnumerator.Current { get { return this.Current; } }

                public bool MoveNext()
                {
                    if (this.isFirst)
                    {
                        this.isFirst = false;
                        return this.hasNext;
                    }

                    return this.inner.MoveNext();
                }

                public void Reset()
                {
                    this.inner.Reset();
                }
            }
        }
    }

}
