using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLi.NoSql.API.Collection.MongoR
{
    public class ResultSet
    {
        public object Result { get; set; }
        public long ResultCount { get; set; }
    }
}
