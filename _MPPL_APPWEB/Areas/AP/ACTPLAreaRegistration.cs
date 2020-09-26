using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.AP
{
    public class APAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AP";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AP_default",
                "AP/{controller}/{action}/{id}",
                new { controller = "Start", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "_MPPL_WEB_START.Areas.AP.Controllers" }
            );
        }
    }
}