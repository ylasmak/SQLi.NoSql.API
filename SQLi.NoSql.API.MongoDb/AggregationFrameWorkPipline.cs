using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;

using MongoDB.Bson;
using SQLi.NoSql.API.Collection.MongoR;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace SQLi.NoSql.API.MongoDB
{
    public class AggregationFrameWorkPipline
    {

        private IMongoCollection<BsonDocument> _collection;
        public AggregationFrameWorkPipline(string server, string port, string dataBase,string collection)
        {
            var client = new MongoClient(string.Format("mongodb://{0}:{1}",server,port));
            var database = client.GetDatabase(dataBase);
             _collection = database.GetCollection<BsonDocument>(collection);
        }

        private static bool IsValidJson(string strInput)
        {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public Tuple<List<BsonDocument>, int> Execute(List<string> jsonDocument,
                                          int maxInPageCount, int pageNumber,
                                         int resultCount)
        {

            var list = new List<BsonDocument>();

            foreach (var document in jsonDocument)
            {
              
                var documentBson = BsonDocument.Parse(document);
                list.Add(documentBson);
            } 

            if (pageNumber == 0 && pageNumber != -1)
            {

                resultCount = BuildCountQuery(list);
            }

            if (maxInPageCount != -1)
            {
                // Tacke the currentPage
                long skip = maxInPageCount * pageNumber;

                list = AddSkipAndTackeStages(list, skip, maxInPageCount);
            }

            var option = new AggregateOptions() { AllowDiskUse = true };
           var result =  _collection.Aggregate<BsonDocument>(list.ToList(), option).ToList();

           return new Tuple<List<BsonDocument>, int>(result, resultCount);

        }

        private List<BsonDocument> AddSkipAndTackeStages(List<BsonDocument> list,long skip, int maxInPageCount)
        {
           list.Add( new BsonDocument("$skip", skip));
           list.Add( new BsonDocument("$limit", maxInPageCount));
           return list;
        }

        private int BuildCountQuery(List<BsonDocument> query)
        {

            int toalQuery = 0;

            var tmpQuery = new List<BsonDocument>();
            foreach (var step in query)
            {
                if (step.ElementAt(0).Name != "$project" && step.ElementAt(0).Name != "$sort")
                {
                    tmpQuery.Add(step);
                }
            }

            var group = new BsonDocument
                {
                    { "$group",
                        new BsonDocument
                            {
                                { "_id", "null"
                                },
                                {
                                    "Count", new BsonDocument
                                                 {
                                                     {
                                                         "$sum", 1
                                                     }
                                                 }
                                }
                            }
                  }
                };

            tmpQuery.Add(group);

            var resultTmpQuery = _collection.Aggregate<BsonDocument>(tmpQuery.ToList()).ToList();

            var res = resultTmpQuery.Select(x => x.ToDynamic()).ToList();
            if(res.Count >0 )
            {
                toalQuery = int.Parse(res[0].Count.Value.ToString());
            }
            else
            {
                toalQuery = 0;
            }

            return toalQuery;

        }

    }
}
