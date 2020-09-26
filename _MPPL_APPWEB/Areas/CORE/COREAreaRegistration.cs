using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.CORE
{
    public class COREAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CORE";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "CORE_default",
                "CORE/{controller}/{action}/{id}",
                new { controller = "home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}