using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;



using SQLi.NoSql.API.Collection;
using SQLi.NoSql.API.Core;

namespace SQLi.NoSql.API.MongoDB
{
    public class Class1
    {
        
  

        public object TestConnexion()
        {
            var client = new MongoClient("mongodb://192.168.157.142:27017");
            var database = client.GetDatabase("scratch");
            var collection = database.GetCollection<BsonDocument>("zips");

            var filter = new BsonDocument();
            var count = 0;

           

            foreach (var item in collection.Find(filter).ToList())
            {
                var x = item;
            }

            return null;

        }

        public  void Add()
        {
          var document = new BsonDocument
                {
                    { "address" , new BsonDocument
                        {
                            { "street", "2 Avenue" },
                            { "zipcode", "11175" },
                            { "building", "1480" },
                            { "coord", new BsonArray { 73.9557413, 40.7720266 ,78.98876} }
                        }
                    },
                    { "borough", "Manhattan" },
                    { "cuisine", "Italian" },
                    { "grades", new BsonArray
                        {
                            new BsonDocument
                            {
                                { "date", new DateTime(2014, 10, 1, 0, 0, 0, DateTimeKind.Utc) },
                                { "grade", "A" },
                                { "score", 11 }
                            },
                            new BsonDocument
                            {
                                { "date", new DateTime(2014, 1, 6, 0, 0, 0, DateTimeKind.Utc) },
                                { "grade", "B" },
                                { "score", 17 }
                            }
                        }
                    },
                    { "name", "Vella" },
                    { "restaurant_id", "41704620" }
                };

            var client = new MongoClient("mongodb://192.168.157.142:27017");
            var database = client.GetDatabase("TransactionLogger");
            var collection = database.GetCollection<BsonDocument>("FloussyTransactionLogger");
           
        }
        
        public void Query()
        {
            var client = new MongoClient("mongodb://192.168.157.147:27017");
            var database = client.GetDatabase("scratch");
            var collection = database.GetCollection<SensorData>("SensorData12");                        

            var x = new SensorData();

            x.TTL = 33;
            x.Type = "type";
            x.Value = "XX";
            x._id = "4507-1610-2135-9625";               

            collection.InsertOne(x);        

        }


    }
      
}
