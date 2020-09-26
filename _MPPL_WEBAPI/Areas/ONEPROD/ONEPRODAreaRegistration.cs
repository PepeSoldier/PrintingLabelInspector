using System.Web.Mvc;

namespace _MPPL_WEBAPI.Areas.ONEPROD
{
    public class ONEPRODAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ONEPROD";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ONEPROD_default",
                "ONEPROD/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}