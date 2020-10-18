using System.Web.Mvc;

namespace MDL_BASE.Areas.IDENTITY
{
    public class IDAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "IDENTITY";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "IDENTITY_default",
                "IDENTITY/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "_LABELINSP_APPWEB.Areas.IDENTITY.Controllers" }
            );
        }
    }
}