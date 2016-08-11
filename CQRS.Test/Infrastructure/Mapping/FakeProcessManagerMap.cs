using Optimissa.nCubed.CQRS.Test.FakeObjects;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimissa.nCubed.CQRS.Test.Infrastructure.Mapping
{
    public class FakeProcessManagerMap : EntityTypeConfiguration<FakeProcessManager>
    {
        /// <summary>
        /// Base constructor.
        /// </summary>
        public FakeProcessManagerMap()
        {
            this.HasKey(p => p.Id);
            this.ToTable("FakeProcessManagers");

            this.Ignore(p => p.Commands);
        }
    }
}
