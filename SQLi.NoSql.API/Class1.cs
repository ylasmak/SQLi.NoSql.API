using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLi.NoSql.API.Cassandra
{
   public class CassandraTest
    {

        public void test()
        {
            var x = new CassandraContext();
            x.CreateDataBase();
        }
    }
}
