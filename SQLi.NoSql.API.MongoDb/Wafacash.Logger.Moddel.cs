using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SQLi.NoSql.API.Core.AppiLogger;

namespace SQLi.NoSql.API.MongoDB
{
   public class WafaCashLoggerModdel
    {
        public static void AddLogToCollection(string jsonDocument,string collectionName)
        {
            try
            {
                var document = BsonDocument.Parse(jsonDocument);

                document = CorrectDateField(document);

                var keyValue = document.First().Value.ToString();

                var result = new BsonDocument();
                result.Add("_id", keyValue);

                var mongoRepo = new MongoDBRepository<MongoDBContext>();
                var fResult = mongoRepo.FinObjectByKey(collectionName, keyValue).ToList();

                if (fResult != null && fResult.Count == 1)
                {
                    var value = fResult.First().Last().Value as BsonArray;
                    if (value != null)
                    {
                        var doc = fResult.First().Last().Value as BsonArray;
                        doc.Add(document.Last().Value);
                        CopyMidelElmentInResult(document, result);
                        result.Add(document.Last().Name, doc);
                        mongoRepo.ReplaceOne(keyValue, collectionName, result);
                    }
                    else
                    {
                        var doc = fResult.First().Last().Value as BsonDocument;
                        var list = new BsonArray();
                        list.Add(doc);
                        list.Add(document.Last().Value);
                        CopyMidelElmentInResult(document, result);
                        result.Add(doc.Last().Name, list);
                        mongoRepo.ReplaceOne(keyValue, collectionName, result);
                    }
                }
                else
                {

                    CopyMidelElmentInResult(document, result);
                    var list = new BsonArray();
                    list.Add(document.Last().Value);
                    result.Add(document.Last().Name, list);
                    mongoRepo.AddOne(collectionName, result);
                }

            }
            catch(Exception ex)
            {
                APILogger.Write(ex.Message, LogLevel.Error);
            }
           
        }

        public static void AddGenricLogToCollection(string jsonDocument)
        {
            try
            {
                var document = BsonDocument.Parse(jsonDocument);
                var collectionName = document.GetElement("CollectionName").Value.ToString();
                var keyValue = document.GetElement("ValueKey").Value.ToString();
                var idKey = document.GetElement("IdKey").Value.ToString();

                var result = new BsonDocument();
                result.Add("_id", keyValue);
                result.Add(idKey, keyValue);          

                var mongoRepo = new MongoDBRepository<MongoDBContext>();
                var fResult = mongoRepo.FinObjectByKey(collectionName, keyValue).ToList();

                if (fResult != null && fResult.Count == 1)
                {
                    var value = fResult.First().Last().Value as BsonArray;
                    if (value != null)
                    {
                        var doc = fResult.First().Last().Value as BsonArray;
                        doc.Add(document.Last().Value);                       
                        result.Add(document.Last().Name, doc);
                        mongoRepo.ReplaceOne(keyValue, collectionName, result);
                    }
                    else
                    {
                        var doc = fResult.First().Last().Value as BsonDocument;
                        var list = new BsonArray();
                        list.Add(doc);
                        list.Add(document.Last().Value);                       
                        result.Add(doc.Last().Name, list);
                        mongoRepo.ReplaceOne(keyValue, collectionName, result);

                    }
                }
                else
                {                  
                    var list = new BsonArray();
                    list.Add(document.Last().Value);
                    result.Add(document.Last().Name, list);
                    mongoRepo.AddOne(collectionName, result);
                }
            }

            catch (Exception ex)
            {
                APILogger.Write(ex.Message, LogLevel.Error);
            }
        }

        private static void CopyMidelElmentInResult(BsonDocument document, BsonDocument result)
        {
            int i = 0;
            int count = document.Elements.Count()-1;
            while (i < count)
            {
                result.Add(document.ElementAt(i));
                i++;

            }
        }


        private static BsonDocument CorrectDateField(BsonDocument bsonDocument)
        {
            BsonDocument result = new BsonDocument();

            int cpt = bsonDocument.Count();
            for (int i = 0; i < cpt; i++)
            {
              
                var element = bsonDocument.ElementAt(i);
                var value = element.Value;
                var name = element.Name;

                if(value is BsonDocument)
                {
                    value =CorrectDateField(value as BsonDocument);
                    element = new BsonElement(name, value);
                }
                else
                {
                    DateTime Out;
                    if (DateTime.TryParse(value.ToString(), out Out))
                    {
                        value = new BsonDateTime(Out);
                        element = new BsonElement(name, value);
                    }
                }                


                result.Add(element);

            }
            return result; 
        }
    }
}
