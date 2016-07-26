using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLi.NoSql.API.Core.AppiLogger;

namespace SQLi.NoSql.API.MongoR.Lib.Model
{
   public class ConfigurationModel
    {

        private static Dictionary<string, Report> _configDictionnary;
        private static string configurationFolder = @"E:\RMongo";

        public static void LoadReportConfiguration()
        {
            if (_configDictionnary == null)
            {
                _configDictionnary = new Dictionary<string, Report>();

            }
            else
            {
                return;
            }

           var files = Directory.GetFiles(configurationFolder);

            if(files != null && files.Length > 0)
            {

                foreach(var file in files)
                {

                  
                    ProccessFileConfiguration(file);
                }
            }


        }

        private static void ProccessFileConfiguration(string config)
        {
           try
            {
                var report = new Report();
               var xConfig  =  XDocument.Load(config);

                var reportNode = xConfig.Descendants("report");
                //Load Report attribute
                var node = reportNode.Attributes().ToList();
                report.ReportName = node.First(p => p.Name.ToString() == "name").Value.ToString();
                report.CollectionName = node.First(p => p.Name.ToString() == "collection").Value.ToString();
                report.ServerPort =  node.First(p => p.Name.ToString() == "port").Value.ToString();
                report.ServerUri = node.First(p => p.Name.ToString() == "server").Value.ToString();
                report.DataBase = node.First(p => p.Name.ToString() == "database").Value.ToString();
                report.ExcelExportFileName = node.FirstOrDefault(p => p.Name.ToString() == "ExcelFileName") == null ? (report.ReportName +".xls") : node.FirstOrDefault(p => p.Name.ToString() == "ExcelFileName").Value.ToString();

                //Load Filter List

                var filterNode = reportNode.Descendants("filter").Descendants("item").ToList();
                if(filterNode != null && filterNode.Count > 0)
                {
                    report.FilterList = new List<Filter>();

                    foreach(var filter in filterNode)
                    {
                        var oneFilter = filter.Attributes().ToList();
                        var item = new Filter();
                        item.Name = oneFilter.First(p => p.Name == "name").Value.ToString();
                        item.DisplayName = oneFilter.First(p => p.Name == "displayName").Value.ToString();
                        item.Type = oneFilter.First(p => p.Name == "type").Value.ToString();
                        item.Source = oneFilter.FirstOrDefault(p => p.Name == "source") != null ? oneFilter.FirstOrDefault(p => p.Name == "source").Value.ToString() : null;

                        report.FilterList.Add(item);
                    }
                }

                //Load Query
                var test = reportNode.Descendants("query").ToList()[0].Value;

                //Load LambdaFilter

                report.Query = new List<string>();


                var piplineList = reportNode.Descendants("query").Descendants("pipline").ToList();
                foreach(var pipline in piplineList)
                {
                    report.Query.Add(pipline.Value);

                }


                report.OriginQuery = new List<string>();
                foreach(var item in report.Query)
                {
                    report.OriginQuery.Add(item);
                }
                                       

                report.LambdaQuery = AggregationQueryProcessing.LambdaFilterProcessing(reportNode.Descendants("lambdaFilter").ToList()[0]);

                //Load Grid Definition
                report.Grid = new GridDefinittion();
            
                var gridNode = reportNode.Descendants("grid");

                report.Grid.MaxInPage = int.Parse( gridNode.Attributes().ToList().First(p => p.Name == "maxInPage").Value.ToString());

                report.Grid.Column = new List<ColumnDefinition>();
                var columnsNode = gridNode.Descendants("column").ToList();

                if(columnsNode != null && columnsNode.Count()>0)
                {

                    foreach(var column in columnsNode)
                    {
                        var OneClomun = new ColumnDefinition();
                        var columnAttribute = column.Attributes().ToArray();

                        OneClomun.Header = columnAttribute.First(p => p.Name == "header").Value.ToString();
                        OneClomun.Name = columnAttribute.First(p => p.Name == "name").Value.ToString();

                        OneClomun.Binding = columnAttribute.First(p => p.Name == "binding").Value.ToString();

                        report.Grid.Column.Add(OneClomun);

                    }
                }

                //Process filter dictionnary

                report.FilterDataValue = new Dictionary<string, object>();

                if(report.FilterList != null)
                {
                    foreach(var filter in report.FilterList)
                    {
                        report.FilterDataValue.Add(filter.Name, null);
                    }
                }
                
                   
                _configDictionnary.Add(report.ReportName, report);

            }
            catch(Exception ex)
            {
                APILogger.Write(ex.Message, LogLevel.Error);
                throw ex;
            }
        }


        public static Report GetReportConfiguration(string reportName)
        {
            return GetReportConfiguration(reportName, false);
        }
        public static Report GetReportConfiguration(string reportName,bool initializedQuery)
        {
            var tmp = _configDictionnary[reportName];

            if (!initializedQuery)
            {
                tmp.Query = new List<string>();
                foreach (var item in tmp.OriginQuery)
                {
                    tmp.Query.Add(item);
                }
            }
            return tmp;

        }
    }
}
