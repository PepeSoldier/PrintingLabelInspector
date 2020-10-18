using _LABELINSP_APPWEB.Areas.IDENTITY;
using System.Web;
using System.Web.Mvc;

namespace _LABELINSP_APPWEB
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new ExcludeFilterAttribute());
           // filters.Add(new PasswordExpiredAttribute());
        }
    }
}
