using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Optimissa.nCubed.CQRS.Test.FakeObjects.Infrastructure
{
    public class FakeDbSet<T> : DbSet<T>, IDbSet<T> where T : class
    {
        protected readonly HashSet<T> _data;
        protected readonly IQueryable _query;

        public FakeDbSet()
        {
            _data = new HashSet<T>();
            _query = _data.AsQueryable();
        }

        public override T Add(T entity)
        {
            _data.Add(entity);
            return entity;
        }

        public override T Attach(T entity)
        {
            _data.Add(entity);
            return entity;
        }

        public override TDerivedEntity Create<TDerivedEntity>()
        {
            throw new NotImplementedException();
        }

        public override T Create()
        {
            return Activator.CreateInstance<T>();
        }

        /*public override T Find(params object[] keyValues)
        {
            throw new NotImplementedException(
               "Derive from FakeDbSet and override Find");
        }*/

        public System.Collections.ObjectModel.ObservableCollection<T> Local
        {
            get
            {
                return new
                  System.Collections.ObjectModel.ObservableCollection<T>(_data);
            }
        }

        public override T Remove(T entity)
        {
            _data.Remove(entity);
            return entity;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        public Type ElementType
        {
            get { return _query.ElementType; }
        }

        public Expression Expression
        {
            get { return _query.Expression; }
        }

        public IQueryProvider Provider
        {
            get { return _query.Provider; }
        }
    }
}
