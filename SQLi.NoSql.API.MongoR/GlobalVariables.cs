using SQLi.NoSql.API.MongoR.Lib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQLi.NoSql.API.MongoR
{
    public static class GlobalVariables
    {
       public static Report CurrentFilter {

            get
            {
                return HttpContext.Current.Application["CurrentFilter"] as Report;
            }
            set
            {
                HttpContext.Current.Application["CurrentFilter"] = value;
            }
        }

        public static Report CurrentForm
        {

            get
            {
                return HttpContext.Current.Application["CurrentForm"] as Report;
            }
            set
            {
                HttpContext.Current.Application["CurrentForm"] = value;
            }
        }
        // read-write variable
        public static string path
        {
            get
            {
                return HttpContext.Current.Application["path"] as string;
            }
            set
            {
                HttpContext.Current.Application["path"] = value;
            }
        }
    }

}