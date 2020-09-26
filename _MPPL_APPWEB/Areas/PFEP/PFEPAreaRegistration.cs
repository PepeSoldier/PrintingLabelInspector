using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.PFEP
{
    public class PFEPAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "PFEP";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "PFEP_default",
                "PFEP/{controller}/{action}/{id}",
                new { controller = "home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "_MPPL_WEB_START.Areas.PFEP.Controllers" }
            );
        }
    }
}