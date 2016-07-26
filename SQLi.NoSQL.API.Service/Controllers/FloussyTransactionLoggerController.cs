using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SQLi.NoSql.API.MongoDB;


using SQLi.NoSQL.API.Service.Helpers;



namespace SQLi.NoSQL.API.Service.Controllers
{
    public class FloussyTransactionLoggerController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

               // POST api/values
        public void Post(HttpRequestMessage value)
        {

           var json =  value.Content.ReadAsStringAsync().Result;
           var collectionName = Helper.GetAppSettingValue(this.GetType().Name);

            WafaCashLoggerModdel.AddLogToCollection(json, collectionName);
          

        }


        

        




    }
}