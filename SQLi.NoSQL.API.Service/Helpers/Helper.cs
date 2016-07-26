using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace SQLi.NoSQL.API.Service.Helpers
{
    public class Helper
    {
        public static string GetAppSettingValue(string key)
        {

            return key.Substring(0, key.IndexOf("Controller"));


        }
    }
}