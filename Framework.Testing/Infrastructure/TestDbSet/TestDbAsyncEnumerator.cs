using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Testing.Infrastructure.TestDbSet
{
    internal class TestDbAsyncEnumerator<T> : IDbAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;
        public TestDbAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }
        public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_inner.MoveNext());
        }
        public T Current
        {
            get { return _inner.Current; }
        }
        object IDbAsyncEnumerator.Current
        {
            get { return Current; }
        }
        public void Dispose()
        {
            _inner.Dispose();
        }
    } 
}
