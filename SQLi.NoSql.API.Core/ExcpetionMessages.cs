using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLi.NoSql.API.Core
{
   public class ExcpetionMessages
    {
        public const string DataBaseConfigurationError = "Data Base configuration Error";
        public const string MongoDBNotSuppportSaveChanges= "MongoDB does not support Save changes";
        public const string MongoDBNotSuppportIncludeEntities = "MongoDB does not support include entities";
        public const string MongoDBNotSuppportUpdateUseReplaceOneMethod = "MongoDB Not Suppport Update Method Use UpdateOne Method";

        public const string CassandraNotSuppportIncludeEntities = "Cassandra does not support include entities";
        public const string CassandraNotSuppportSaveChanges = "Cassandra does not support Save changes";
    }
}
