using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLi.NoSql.API.MongoR.Lib.Model
{
    public class Report
    {
        public string ReportName { get; set; }

        public string ExcelExportFileName { get; set; }

        public string ServerUri { get; set; }

        public string ServerPort { get; set; }

        public string DataBase { get; set; }

        public string CollectionName { get; set; }
        public List<Filter> FilterList { get; set; }

        public List< string> Query { get; set; }

        public List<string> OriginQuery { get; set; }

        public List<LamdbdaExpression> LambdaQuery { get;set;}

        public GridDefinittion Grid { get; set; }

        public Dictionary<string,object> FilterDataValue { get; set; }

        public int ResultCount { get; set; }

        public long PageCount { get; set; }

        public int CurrentPage { get; set; }

        public bool DisplayGraphs { get; set; } 

        public List<Graph> GraphList { get; set; }

    

      
    }

    public class Graph
    {
        public string width { get; set; }

        public string Height { get; set; }

        public string Title { get; set; }

        public string Xfield { get; set; }

        public string FieldType { get; set; }

        public string ApplyFunction { get; set; }
    }



    public class Filter
    {
        public string Name { get; set; }
        public string Type { get; set; }

        public string Source { get; set; }

        public string DisplayName { get; set; }


    }

    public class GridDefinittion
    {
        public int MaxInPage { get; set; }

        public List<ColumnDefinition> Column { get; set; }
        public DataSet ResultSet { get; set; }

        public DataSet ExcelResultSet { get; set; }
    }

    public class ColumnDefinition
    {
        public string Header { get; set; }
        public string Binding { get; set; }

        public string Name { get; set; }

        public string sort { get; set; }


    }

    public class LamdbdaExpression
    {

        public LambdaOperator Operator { get; set; }
       // public List< LamdbdaExpression> Expression { get; set; }

        public string Field { get; set; }

        public string Values { get; set; }

        public bool AllowNull { get; set; }

        public string DataType { get; set; }

    }
    public enum LambdaOperator
    {   
        Eq,
        Lt,
        Gt
    }



}
