using SQLi.NoSql.API.MongoR.Lib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SQLi.NoSql.API.MongoDB;

using MongoDB.Bson;
using System.Data;
using Newtonsoft.Json;

namespace SQLi.NoSql.API.MongoR.Lib
{
   public class AggregationQueryProcessing
    {
        public static  List<LamdbdaExpression> LambdaFilterProcessing(  XElement LamdaExpression)
        {

            var expressionResult = new List<LamdbdaExpression>();
            var filters = LamdaExpression.Elements().ToList();

            foreach (var filter in filters)
            {
                expressionResult.Add(ProcessNaode(filter));
            }               


            return expressionResult;

        }

        public static LamdbdaExpression ProcessNaode(XElement node)
        {
            var expressionResult = new LamdbdaExpression();
            var lambdaOperator = node.Name.ToString();

            expressionResult.Operator = GetOperatorByString(lambdaOperator);
         
                var attributes = node.Attributes().ToList();
                expressionResult.Field = attributes.First(p => p.Name == "field").Value;
                expressionResult.Values = attributes.First(p => p.Name == "values").Value;
                expressionResult.DataType = attributes.First(p => p.Name == "type").Value;
                expressionResult.AllowNull = attributes.First(p => p.Name == "alownull").Value == "true" ? true :false;
         


            return expressionResult;
        }

        private static LambdaOperator GetOperatorByString(string lambdaOperator)
        {
          
          switch(lambdaOperator.ToLower())
            {
                
                case "eq":
                    return LambdaOperator.Eq;
                case "lt":
                    return LambdaOperator.Lt;
                case "gt":
                    return LambdaOperator.Gt;
                default :
                    throw new Exception(string.Format("Operator: {0} non pris en compte",lambdaOperator));

            }

        }     

        public static string BuildAggregationPiplineFrameWorkQuery(Report  report)
        {

            //BuildBson Query            

            BuildMatchQuery(report);

            return string.Empty;
        }

        public static Report BuildMatchQuery(Report report)
        {

         

            var bsonArray = new BsonArray();
            DateTime outDate = DateTime.MinValue;
            int filterCount = 0;

            foreach (var query in report.LambdaQuery)
            {

                if (report.FilterDataValue[query.Values] == null ||
                        string.IsNullOrEmpty(report.FilterDataValue[query.Values].ToString()))
                {
                    if ( query.AllowNull)
                    {
                        continue;
                    }
                    else
                    {

                        throw new Exception("champs obligatoire non renségné ");
                    }
                }

                BsonDocument element = null;
                if (query.Operator == LambdaOperator.Eq)
                {
                    if (DateTime.TryParse(report.FilterDataValue[query.Values].ToString(), out outDate))
                    {
                        element = new BsonDocument(query.Field, BsonDateTime.Create(outDate));
                        
                    }

                    else
                    {
                        element = new BsonDocument(query.Field, BsonString.Create(report.FilterDataValue[query.Values]));
                    }
                    filterCount++;
                    bsonArray.Add(element);
                }

                if (query.Operator == LambdaOperator.Gt)
                {
                 

                    if (DateTime.TryParse(report.FilterDataValue[query.Values].ToString(), out outDate))
                    {

                        element = new BsonDocument("$gt", BsonDateTime.Create(outDate));
                    }

                    else
                    {
                        element = new BsonDocument("$gt", BsonString.Create(report.FilterDataValue[query.Values]));
                    }

                    var surElement = new BsonDocument(query.Field, element);
                    bsonArray.Add(surElement);
                    filterCount++;
                }

              
                if (query.Operator == LambdaOperator.Lt)
                {
                    if ( DateTime.TryParse( report.FilterDataValue[query.Values].ToString(),out  outDate))
                    {
                        element = new BsonDocument("$lt", BsonDateTime.Create(outDate));
                    }

                    else
                    {
                        element = new BsonDocument("$lt", BsonString.Create(report.FilterDataValue[query.Values]));
                    }
                    var surElement = new BsonDocument(query.Field, element);
                    bsonArray.Add(surElement);
                    filterCount++;

                }

            }
            
            if(filterCount == 0)
            {
                throw new Exception("Au moins un filtre doit être renségné");
            }

            var bsonAnd = new BsonDocument("$and", bsonArray);
            var bsonMatch = new BsonDocument("$match", bsonAnd);
          

            for(int i=0;i< report.Query.Count; i++)
            {
                if(report.Query[i].Contains("$$lambdaFilter$$"))
                {
                    report.Query[i] = bsonMatch.ToString();
                }
            }

            var aggregationFrameWork  = new AggregationFrameWorkPipline(report.ServerUri,
                                                                            report.ServerPort,
                                                                            report.DataBase,
                                                                            report.CollectionName);


            var result =  aggregationFrameWork.Execute(report.Query,report.Grid.MaxInPage,
                                                                report.CurrentPage,
                                                                report.ResultCount
                                                                );

            report.ResultCount = result.Item2;
            long rem = 0;

            report.PageCount = Math.DivRem(report.ResultCount, report.Grid.MaxInPage, out rem);

            if(rem >0)
            {
                report.PageCount = report.PageCount + 1;
            }

           BuildGridResult(report, result.Item1);

            return report;
        }

        public static Report BuildGraphicalCharts(Report report)
        {
            var aggregationFrameWork = new AggregationFrameWorkPipline(report.ServerUri,
                                                                           report.ServerPort,
                                                                           report.DataBase,
                                                                           report.CollectionName);

            var result = aggregationFrameWork.Execute(report.Query, -1, -1, -1);

            BuildExcelResult(report, result.Item1);
            BuildChartsData(report);

            return report;
        }

        public static Report ExcelExport(Report report)
        {

            var aggregationFrameWork = new AggregationFrameWorkPipline(report.ServerUri,
                                                                            report.ServerPort,
                                                                            report.DataBase,
                                                                            report.CollectionName);

            var result = aggregationFrameWork.Execute(report.Query,-1,-1,-1);

            BuildExcelResult(report, result.Item1);

            return report;
        }

        private static void BuildChartsData(Report report)
        {
           
            

            //report.Grid.ExcelResultSet = resultGrid;

        }

        private static void BuildExcelResult(Report report, List<BsonDocument> resultSet)
        {
            var resultGrid = new DataSet();
            resultGrid.Tables.Add(new DataTable("ReseultSet"));
            //Adding columns

            foreach (var column in report.Grid.Column)
            {
                resultGrid.Tables[0].Columns.Add(new DataColumn(column.Name));
            }


            foreach (var record in resultSet)
            {
                var row = resultGrid.Tables[0].NewRow();

                foreach (var column in report.Grid.Column)
                {
                    var bindingPath = column.Binding.Split('.').ToList();
                    var value = GetValueForBinding(record, bindingPath);

                    row[column.Name] = value;

                }

                resultGrid.Tables[0].Rows.Add(row);
            }

            report.Grid.ExcelResultSet = resultGrid;

        }

        private static void BuildGridResult(Report report,List<BsonDocument> resultSet)
        {
            var resultGrid = new DataSet();
            resultGrid.Tables.Add(new DataTable("ReseultSet"));
            //Adding columns

            foreach(var column in report.Grid.Column)
            {
                resultGrid.Tables[0].Columns.Add(new DataColumn(column.Name));
            }

         
            foreach (var record in resultSet)
            {
                var row = resultGrid.Tables[0].NewRow();

                foreach (var column in report.Grid.Column)
                {
                    var bindingPath = column.Binding.Split('.').ToList();
                    var value = GetValueForBinding(record, bindingPath);

                    row[column.Name] = value;

                }

                resultGrid.Tables[0].Rows.Add(row);
            }

            report.Grid.ResultSet = resultGrid;

        }

        private static object GetValueForBinding(dynamic record, List<string> binding)
        {
            try
            {
                if (binding.Count == 1) return record[binding.First()].Value;
                else
                {
                    var tmpRecord = record[binding.First()];
                    binding.RemoveAt(0);

                    return GetValueForBinding(tmpRecord, binding);
                }
            }
            catch
            {
                return string.Empty;
            }
            
        }
    }
}
