using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.OTHER
{
    public class OTHERAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "OTHER";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "OTHER_default",
                "OTHER/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}