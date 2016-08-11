using Optimissa.nCubed.CQRS.Core.ProcessManager.Fakes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Fakes;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimissa.nCubed.CQRS.Test.FakeObjects.Infrastructure
{
    class FakeApplicationProcessManagerContext : DbContext
    {
        IDbSet<StubIProcessManager> fakeProcessManagers = new FakeDbSet<StubIProcessManager>();

        public IDbSet<StubIProcessManager> FakeProcessManagers
        {
            get
            {
                return fakeProcessManagers;
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //throw new UnintentionalCodeFirstException();
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}
