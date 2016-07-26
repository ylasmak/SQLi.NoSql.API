using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQLi.NoSql.API.MongoDB;
using SQLi.NoSql.API.Collection;
using SQLi.NoSql.API.Core;

using SQLi.NoSql.API.Cassandra;
using SQLi.NoSql.API.Collection.CarteFloussy;





namespace SQLi.NoSql.API.Console
{
    class Program
    {
        static void Main(string[] args)
        {

            var mongoRepo = new MongoDBRepository<MongoDBContext>();
            var x = mongoRepo.GetListEntities<FloussyTransaction>(p => p.TransactionList.Any(s => s.Error == ErrorType.Default)).Select(p => p.TransactionList.Where(s => s.Error == ErrorType.Default)).ToList();
            var y = x.ToList();

         //mongoRepo.MongoContext.GetCollection<FloussyTransaction>("FloussyTransaction").Aggregate(pipeline)



        }
    }
}
