using _MPPL_WEB_START.Areas.IDENTITY;
using System.Web;
using System.Web.Mvc;

namespace _MPPL_WEB_START
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
