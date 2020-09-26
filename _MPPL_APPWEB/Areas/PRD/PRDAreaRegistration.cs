using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.PRD
{
    public class PRDAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "PRD";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "PRD_default",
                "PRD/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}