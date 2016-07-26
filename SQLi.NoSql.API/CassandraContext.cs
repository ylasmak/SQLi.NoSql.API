using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net;

using SQLi.NoSql.API.Collection;
using SQLi.NoSql.API.Core;

using Cassandra;

namespace SQLi.NoSql.API.Cassandra
{
    public class CassandraContext : IDataBaseContext
    {
        const int defualtProt = 9042;
        public object CreateDataBase()
        {
            string connexionString = string.Empty;
            var configuration = ConfigurationManager.GetSection("CassandraConnextion") as DBConnexionConfig;

            if (!configuration.Port.HasValue)
            {
                configuration.Port = defualtProt;
            }
            var endpoint = new IPEndPoint(IPAddress.Parse(configuration.Host), configuration.Port.Value);           

            Cluster cluster = Cluster.Builder().AddContactPoint(endpoint).Build();


            ISession session = cluster.Connect(configuration.DataBase);

            return session;
        }
    }
}
