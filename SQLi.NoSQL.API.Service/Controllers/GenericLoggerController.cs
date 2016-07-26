using SQLi.NoSql.API.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SQLi.NoSQL.API.Service.Controllers
{
    public class GenericLoggerController : ApiController
    {
        public void Post(HttpRequestMessage value)
        {

            var json = value.Content.ReadAsStringAsync().Result;
          

            WafaCashLoggerModdel.AddGenricLogToCollection(json);


        }
    }
}
