using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.MASTERDATA
{
    public class MASTERDATAAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "MASTERDATA";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "MASTERDATA_default",
                "MASTERDATA/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}