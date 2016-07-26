using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SQLi.NoSql.API.Core
{
  public  class MongoDBCollectionBase
    {
        [DataMember]
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string _id { get; set; }
    }
}
