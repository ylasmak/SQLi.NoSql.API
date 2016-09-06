using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Bson;



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

        public Tuple<List<BsonDocument>, int, Stack<string>> Execute(List<string> jsonDocument,
                                          int maxInPageCount, int pageNumber,
                                         int resultCount)
        {

            var log = new Stack<string>();
            var list = new List<BsonDocument>();

            foreach (var document in jsonDocument)
            {
              
                var documentBson = BsonDocument.Parse(document);
                list.Add(documentBson);
            } 

            if (pageNumber == 0 && pageNumber != -1)
            {

                var tmp =  BuildCountQuery(list);
                resultCount = tmp.Item1;
                log.Push(tmp.Item2);
            }

            if (maxInPageCount != -1)
            {
                // Tacke the currentPage
                long skip = maxInPageCount * pageNumber;

                list = AddSkipAndTackeStages(list, skip, maxInPageCount);
            }

           var option = new AggregateOptions() { AllowDiskUse = true };

            var statTime = DateTime.Now.Ticks;

            var result =  _collection.Aggregate<BsonDocument>(list, option).ToList();


            var endDate = DateTime.Now.Ticks;
            var duration = endDate - statTime;
            var resultDuration = TimeSpan.FromTicks(duration).TotalSeconds;

            log.Push(string.Format("{0} - {1} : Duration (s) {2} ",  TimeSpan.FromTicks( statTime).ToString(), BuildStringQuery(list.Select(p => p.ToString()).ToArray()), resultDuration));

           return new Tuple<List<BsonDocument>, int, Stack<string>>(result, resultCount,log);

        }

        private List<BsonDocument> AddSkipAndTackeStages(List<BsonDocument> list,long skip, int maxInPageCount)
        {
           list.Add( new BsonDocument("$skip", skip));
           list.Add( new BsonDocument("$limit", maxInPageCount));
           return list;
        }

        private Tuple< int,string> BuildCountQuery(List<BsonDocument> query)
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

            var statTime = DateTime.Now.Ticks;

            var resultTmpQuery = _collection.Aggregate<BsonDocument>(tmpQuery.ToList()).ToList();

            var endDate = DateTime.Now.Ticks;
            var duration = endDate - statTime;
            var resultDuration = TimeSpan.FromTicks(duration).TotalSeconds;

            var log =string.Format("{0} - {1} : Duration (s) {2} ", TimeSpan.FromTicks(statTime).ToString(), BuildStringQuery(tmpQuery.Select(p => p.ToString()).ToArray()), resultDuration);

            var res = resultTmpQuery.Select(x => x.ToDynamic()).ToList();
            if(res.Count >0 )
            {
                toalQuery = int.Parse(res[0].Count.Value.ToString());
            }
            else
            {
                toalQuery = 0;
            }

            return new Tuple<int, string>( toalQuery, log);

        }


        private string BuildStringQuery(string[] stages)
        {
            if (stages.Count() == 1) return stages[0];
            var st = new System.Text.StringBuilder();
            
            foreach(var stage in stages)
            {
                st.AppendLine(stage);
                st.AppendLine(",");
            }

           return st.ToString(0, st.Length - 2);

        }

    }
}
