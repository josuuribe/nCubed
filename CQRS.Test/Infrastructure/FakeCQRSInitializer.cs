using Optimissa.nCubed.CQRS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimissa.nCubed.CQRS.Test.Infrastructure
{
    class FakeCQRSInitializer : IDatabaseInitializer<CQRSContext>
    {
        public void InitializeDatabase(CQRSContext context)
        {
            int result3 = context.Database.ExecuteSqlCommand("DROP TABLE IF EXISTS N3_UndispatchedMessages");

            int result4 = context.Database.ExecuteSqlCommand("CREATE TABLE IF NOT EXISTS N3_UndispatchedMessages (" +
                                                             "Id	TEXT, " +
                                                             "CreatedTime	NUMERIC," +
                                                             "Commands TEXT," +
                                                             "PRIMARY KEY(Id));");
        }
    }
}
