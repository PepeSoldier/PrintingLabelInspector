using System.Web.Mvc;

namespace _LABELINSP_APPWEB.Areas.LABELINSP
{
    public class LABELINSPAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "LABELINSP";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "LABELINSP_default",
                "LABELINSP/{controller}/{action}/{id}",
                new { controller = "home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}