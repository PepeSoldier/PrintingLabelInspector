using System.Web.Mvc;
using System.Linq;
using _MPPL_WEB_START.Models;

namespace _MPPL_WEB_START.Areas.PFEP
{
    public class ONEPRODAreaRegistration : AreaRegistrationMPPL
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
            string[] registerComponents = new string[] { }; //new string[] { "Base", "OEE", "Scheduling" };

            base.RegisterArea(context, registerComponents);
        }
    }
}