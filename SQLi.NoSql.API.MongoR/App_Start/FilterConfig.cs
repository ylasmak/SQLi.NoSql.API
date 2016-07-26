using System.Web;
using System.Web.Mvc;

namespace SQLi.NoSql.API.MongoR
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
