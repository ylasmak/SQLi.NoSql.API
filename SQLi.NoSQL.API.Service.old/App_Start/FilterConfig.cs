using System.Web;
using System.Web.Mvc;

namespace SQLi.NoSQL.API.Service
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
