using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SQLi.NoSql.API.Core;

namespace SQLi.NoSql.API.Collection.CarteFloussy
{
    public class FloussyTransaction : MongoDBCollectionBase
    {
        [BsonElement]
        public string NumeroDeCarte { get; set; }

        [BsonElement]
        public List<Transaction> TransactionList { get; set; }
    }

    public class Transaction
    {

        [BsonElement]
        public ServiceInformation ServiceInformation { get; set; }

        [BsonElement]
        public Dictionary<string, object> Parameters { get; set; }


        [BsonElement]
        public Object Result { get; set; }

        [BsonElement]
        public DateTime Date { get; set; }

        [BsonElement]
        public PlateForme PalteForme { get; set; }

        [BsonElement]
        public ErrorType Error { get; set; }

        [BsonElement]
        public TrasactionAction Action { get; set; }

        [BsonElement]
        public Identifier ConnectedUser { get; set; }
    }


    public enum TrasactionAction
    {
        Ventre,
        Recharge,
        Resiliation
    }

    public enum ErrorType
    {
        Default,
        TimeOut,
        TechnicalError
    }

    public enum PlateForme
    {
        Integra,
        Faoury
    }
    public class ServiceInformation
    {

        [BsonElement]
        public string Url { get; set; }

        [BsonElement]
        public string Name { get; set; }

        [BsonElement]
        public string Method { get; set; }
    }

    public class Identifier
    {
        [BsonElement]
        public string UserId { get; set; }

        [BsonElement]
        public string AgenceCode { get; set; }

    }


}
