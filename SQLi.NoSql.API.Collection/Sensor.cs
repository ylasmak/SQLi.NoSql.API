
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

using SQLi.NoSql.API.Core;

namespace SQLi.NoSql.API.Collection
{
    
        [DataContract]
        public class SensorData : MongoDBCollectionBase
    {
            [DataMember]
            [BsonElement]
            [BsonRepresentation(BsonType.String)]
            public string Type { get; set; }

            [DataMember]
            [BsonElement]
            [BsonRepresentation(BsonType.String)]
            public string Desc { get; set; }

            
            [DataMember]
            [BsonElement]
            [BsonRepresentation(BsonType.String)]
            public string Value { get; set; }

            [DataMember]
            [BsonElement]
            [BsonRepresentation(BsonType.String)]
            public string ValueUnits { get; set; }

            [DataMember]
            [BsonElement]
            [BsonRepresentation(BsonType.Boolean)]
            public bool Event { get; set; }

            [DataMember]
            [BsonElement]
            [BsonRepresentation(BsonType.Int32)]
            public int TTL { get; set; }
        }  
}
