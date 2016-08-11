using Optimissa.nCubed.CQRS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimissa.nCubed.CQRS.Test.Infrastructure
{
    class FakeInitializer : IDatabaseInitializer<FakeApplicationProcessManagerContext>, IDatabaseInitializer<CQRSContext>
    {
        public void InitializeDatabase(FakeApplicationProcessManagerContext context)
        {
            int result1 = context.Database.ExecuteSqlCommand("DROP TABLE IF EXISTS FakeProcessManagers");

            int result2 = context.Database.ExecuteSqlCommand("CREATE TABLE IF NOT EXISTS FakeProcessManagers (" +
                                                             "Id	BLOB, " +
                                                             "Completed	NUMERIC," +
                                                             "PRIMARY KEY(Id));");
        }

        public void InitializeDatabase(CQRSContext context)
        {
            int result1 = context.Database.ExecuteSqlCommand("DROP TABLE IF EXISTS N3_UndispatchedMessages");

            int result2 = context.Database.ExecuteSqlCommand("CREATE TABLE IF NOT EXISTS N3_UndispatchedMessages (" +
                                                             "Id	TEXT, " +
                                                             "CreatedTime	NUMERIC," +
                                                             "Commands TEXT," +
                                                             "PRIMARY KEY(Id));");
        }
    }
}
