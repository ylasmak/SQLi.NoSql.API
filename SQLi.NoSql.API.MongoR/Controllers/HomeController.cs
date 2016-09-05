using System.Web.Mvc;
using SQLi.NoSql.API.MongoR.Lib;
using SQLi.NoSql.API.MongoR.Lib.Model;
using SQLi.NoSql.API.MongoR.Models;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace SQLi.NoSql.API.MongoR.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string path)
        {


            var result = ConfigurationModel.GetCurrentConfiguration(path);
            ViewBag.Directories = result.Item2;
            ViewBag.Files = result.Item1;
            return View();
        }
        
        public ActionResult ReportDetails(string path)
        {

            ViewBag.Message = "Your contact page.";
          
            
            var model = new ReportConfigurationModel();
            model.CurrentReport = ConfigurationModel.GetReportConfiguration(path, true);

            return View(model);
        }

        public ActionResult Filter(FormCollection form)
        {
            var model = new ReportConfigurationModel();

            var path = form["path"].Replace(",","");

            model.CurrentReport = ConfigurationModel.GetReportConfiguration(path);

            model.CurrentReport.CurrentPage = 0;
            model.CurrentReport.PageCount = 0;
            model.CurrentReport.ResultCount = 0;
            model.CurrentReport.Grid.ResultSet = null;
            model.CurrentReport.Grid.ExcelResultSet = null;

            foreach (var filter in form.AllKeys)
            {
                model.CurrentReport.FilterDataValue[filter] = form[filter];
            }

            AggregationQueryProcessing.BuildAggregationPiplineFrameWorkQuery(model.CurrentReport);
            return RedirectToAction("ListReport", new { path= path });
        }

        public ActionResult Pagination(string pageNumbre)
        {
            var model = new ReportConfigurationModel();
          
            model.CurrentReport = ConfigurationModel.GetReportConfiguration("TransactionDetails", true);
            model.CurrentReport.CurrentPage = int.Parse( pageNumbre) - 1;

            AggregationQueryProcessing.BuildAggregationPiplineFrameWorkQuery(model.CurrentReport);

            return RedirectToAction("ReportDetails");
        }

        public ActionResult ExcelExport(string file)
        {
            var model = new ReportConfigurationModel();
          
            model.CurrentReport = ConfigurationModel.GetReportConfiguration("TransactionDetails", true);

            AggregationQueryProcessing.ExcelExport(model.CurrentReport);


            // instantiate the GridView control from System.Web.UI.WebControls namespace
            // set the data source
            GridView gridview = new GridView();
            gridview.DataSource = model.CurrentReport.Grid.ExcelResultSet;
            gridview.DataBind();

            // Clear all the content from the current response
            Response.ClearContent();
            Response.Buffer = true;
            // set the header
            Response.AddHeader("content-disposition", string.Format( "attachment;filename = {0}",model.CurrentReport.ExcelExportFileName));
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            // create HtmlTextWriter object with StringWriter
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    // render the GridView to the HtmlTextWriter
                    gridview.RenderControl(htw);
                    // Output the GridView content saved into StringWriter
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }
            }

            return RedirectToAction("ReportDetails");
        }


        public ActionResult DrawChart(string path)
        {
            var model = new ReportConfigurationModel();
          
            model.CurrentReport = ConfigurationModel.GetReportConfiguration("TransactionDetails", true);
            AggregationQueryProcessing.BuildGraphicalCharts(model.CurrentReport);

            return View("dynamicCharts", model);
        }


        public ActionResult ListReport(string path)
        {

          

            if (!string.IsNullOrEmpty(path))
            {
                   
                var themeFolder = "~/Views/Theme/";
                var model = new ReportModelTheme();                               

                var config = ConfigurationModel.GetReportConfiguration(path, true);
                model.CurrentReport = config;
                model.Path = path;


                return View(themeFolder + config.ReporTheme + ".cshtml", model);
               
            }
            return View();
        }   
        }
}