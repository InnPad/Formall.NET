using System.Web;
using System.Web.Mvc;

namespace Formall.Web.Mvc.ExtJs
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}