using Optimissa.nCubed.Core.Infrastructure.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimissa.nCubed.EventSourcing.Infrastructure
{
    public class EventSourcingConfiguration : DbCommonConfiguration<EventSourcingContext>
    {
        public EventSourcingConfiguration()
            : base()
        {
            SetDatabaseInitializer(new DbInitializer<EventSourcingContext>());
        }
    }
}
