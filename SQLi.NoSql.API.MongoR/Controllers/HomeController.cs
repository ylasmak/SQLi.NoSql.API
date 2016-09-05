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

            string filename = string.Empty;
            if (string.IsNullOrEmpty(path))
            {

                 filename = ConfigurationManager.AppSettings["ConsfigFolderSource"];
            }
            else {
                filename = path;
            }
            string[] array0 = Directory.GetDirectories(filename);
            string[] array1 = Directory.GetFiles(filename);
            ViewBag.Directories = array0;
            ViewBag.Files = array1;
            return View();
        }

         
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            ConfigurationModel.LoadReportConfiguration(null);

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            var result = AggregationQueryProcessing.BuildAggregationPiplineFrameWorkQuery(ConfigurationModel.GetReportConfiguration("TransactionDetails"));

            return View();
        }

        public ActionResult ReportDetails()

        {

            ViewBag.Message = "Your contact page.";
            ConfigurationModel.LoadReportConfiguration(null);
            
            var model = new ReportConfigurationModel();
            model.CurrentReport = ConfigurationModel.GetReportConfiguration("TransactionDetails", true);

            return View(model);
        }

        public ActionResult Filter(FormCollection form)
        {
            var model = new ReportConfigurationModel();
            ConfigurationModel.LoadReportConfiguration(null);
            model.CurrentReport = ConfigurationModel.GetReportConfiguration("TransactionDetails");

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
            return RedirectToAction("ReportDetails");
        }

        public ActionResult Pagination(string pageNumbre)
        {
            var model = new ReportConfigurationModel();
            ConfigurationModel.LoadReportConfiguration(null);
            model.CurrentReport = ConfigurationModel.GetReportConfiguration("TransactionDetails", true);
            model.CurrentReport.CurrentPage = int.Parse( pageNumbre) - 1;

            AggregationQueryProcessing.BuildAggregationPiplineFrameWorkQuery(model.CurrentReport);

            return RedirectToAction("ReportDetails");
        }

        public ActionResult ExcelExport(string file)
        {
            var model = new ReportConfigurationModel();
            ConfigurationModel.LoadReportConfiguration(file);
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


        public ActionResult DrawChart()
        {
            var model = new ReportConfigurationModel();
            ConfigurationModel.LoadReportConfiguration(null);
            model.CurrentReport = ConfigurationModel.GetReportConfiguration("TransactionDetails", true);
            AggregationQueryProcessing.BuildGraphicalCharts(model.CurrentReport);

            return View("dynamicCharts", model);
        }


        public ActionResult ListReport(string path)
        {

            var form = Request.Form;
            if (form != null && form.AllKeys != null && form["path"] !=null ) {
                path = form["path"].ToString();
            }
            if (string.IsNullOrEmpty(path))
            {
                path=GlobalVariables.path;
            }
                if (!string.IsNullOrEmpty(path))
            {
                    GlobalVariables.path = path;
                    var themeFolder = "~/Views/Theme/";
                    var model = new ReportModelTheme();
                    ConfigurationModel.LoadReportConfiguration(path);
               

                    var config = ConfigurationModel.GetReportConfiguration("TransactionDetails", true);
                    model.ReporResultFiltre = config;
                    model.ReportChart = config;
                    model.ReportForm = config;
                    model.ReporLog = config;

                /*  result report */

                int pageNum = 1;

                    if (Request != null && Request.QueryString != null && Request.QueryString["page"] != null && int.TryParse(Request.QueryString["page"], out pageNum))
                    {
                        int.TryParse(Request.QueryString["page"], out pageNum);
                    }

                    if (Request != null && Request.Form != null && Request.Form["pageNumbre"] != null && int.TryParse(Request.Form["pageNumbre"], out pageNum))
                    {
                        int.TryParse(Request.Form["pageNumbre"], out pageNum);
                        pageNum = pageNum - 1;
                        model.ReporResultFiltre= GlobalVariables.CurrentFilter ;
                    model.ReportForm = GlobalVariables.CurrentForm;
                }
                
                    model.ReporResultFiltre.CurrentPage = pageNum;
                    //model.ReporResultFiltre.PageCount = 0;
                    //model.ReporResultFiltre.ResultCount = 0;
                    model.ReporResultFiltre.Grid.ResultSet = null;
                    model.ReporResultFiltre.Grid.ExcelResultSet = null;
                    model.ReporResultFiltre.path = path;

                if (form != null && form.AllKeys != null && form["path"] != null)
                {
                    foreach (var filter in form.AllKeys)
                    {
                        model.ReporResultFiltre.FilterDataValue[filter] = form[filter];
                    }

                   //
                }
                bool hasFilterValue = false;
                if(model.ReporResultFiltre !=null 
                    && model.ReporResultFiltre.FilterDataValue !=null 
                    && model.ReporResultFiltre.FilterDataValue.Count > 0)
                { 
                    foreach (var filter in model.ReporResultFiltre.FilterDataValue)
                    {
                        if(filter.Value !=null)
                        {
                            hasFilterValue = true;
                        }
                    }
                }

                /* end result */
                if (hasFilterValue)
                {
                    AggregationQueryProcessing.BuildAggregationPiplineFrameWorkQuery(model.ReporResultFiltre);
                }
                /* start chart  */

                AggregationQueryProcessing.BuildGraphicalCharts(model.ReportChart);

                /* End chart  */

                /* start form   */

                // model.ReportForm = new Report();

                /* end  form  */
                GlobalVariables.CurrentFilter = model.ReporResultFiltre;
                GlobalVariables.CurrentForm= model.ReportForm ;
                model.ReportForm.path = path;
                return View(themeFolder + config.ReporTheme + ".cshtml", model);
               
            }
            return View();
        }   
        }
}