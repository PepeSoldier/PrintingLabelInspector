using _MPPL_WEB_START.Models;
using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.iLOGIS
{
    public class iLOGISAreaRegistration : AreaRegistration //AreaRegistrationMPPL
    {
        public override string AreaName 
        {
            get 
            {
                return "iLOGIS";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            //string[] registercomponents = new string[] { };
            //base.registerarea(context, registercomponents);

            context.MapRoute(
                "iLOGIS_default",
                "iLOGIS/{controller}/{action}/{id}",
                new { controller = "home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}