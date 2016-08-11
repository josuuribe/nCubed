using RaraAvis.nCubed.DDD.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.DDD.Test.FakeObjects.Infrastructure
{
    public class FakePersonRepository : EntityRepository<Person>
    {
        public FakePersonRepository()
            : base(new FakeUoW())
        {

        }

        public FakePersonRepository(IQueryableUnitOfWork uow)
            : base(uow)
        {

        }

        public int CountPerson()
        {
            return QueryableUnitOfWork.CreateSet<Person>().Count();
        }
    }
}
