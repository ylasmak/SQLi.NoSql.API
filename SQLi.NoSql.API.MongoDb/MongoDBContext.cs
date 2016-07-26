using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using MongoDB.Driver;

using SQLi.NoSql.API.Core;
using SQLi.NoSql.API.Core.Exception;

namespace SQLi.NoSql.API.MongoDB
{
    public class MongoDBContext : IDataBaseContext
    {
        private const string connectionStringWithowtAuthentification= "mongodb://{0}:{1}";
        private const string connectionStringWithAuthentification = "mongodb://{0}:{1} {2}:{3}";
        public object CreateDataBase()
        {

            APIContext.Instance.DbContext = this.GetType().Name;

            string connexionString = string.Empty;
            var configuration = ConfigurationManager.GetSection("MongoDBConnextion") as DBConnexionConfig;

            if (configuration != null)
            {
                if (string.IsNullOrEmpty(configuration.UserName) || string.IsNullOrEmpty(configuration.Password))
                {
                    connexionString = string.Format(connectionStringWithowtAuthentification,
                                                    configuration.Host,
                                                    configuration.Port);
                }
                else
                {
                    connexionString = string.Format(connectionStringWithAuthentification,
                                                    configuration.UserName,
                                                    configuration.Password,
                                                    configuration.Host,
                                                    configuration.Port);
                }


                var client = new MongoClient(connexionString);
                
                APIContext.Instance.ConnexionString = connexionString;
                var database = client.GetDatabase(configuration.DataBase);
                
                return database;
            }
            else
            {
                throw new NoSQLDataBaseConnexionExcpetion(ExcpetionMessages.DataBaseConfigurationError);
            }



        }
    }
}
