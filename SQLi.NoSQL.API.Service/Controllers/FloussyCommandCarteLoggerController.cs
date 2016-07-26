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
    public class FloussyCommandCarteLoggerController : ApiController
    {
        public void Post(HttpRequestMessage value)
        {

            var json = value.Content.ReadAsStringAsync().Result;
            var collectionName = Helper.GetAppSettingValue(this.GetType().Name);

            WafaCashLoggerModdel.AddLogToCollection(json, collectionName);


        }
    }
}
