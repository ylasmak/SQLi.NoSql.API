using SQLi.NoSql.API.MongoR.Lib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQLi.NoSql.API.MongoR.Models
{
    public class ReportModelTheme
    {
        public string currentConfig { get; set; }
        public Report ReportForm { get; set; }
        public Report ReportChart { get; set; }
        public Report ReporLog { get; set; }
        public Report ReporResultFiltre { get; set; }
    }

    public class constReport
    {
        public static string constFistreTheme = "";
    }
}