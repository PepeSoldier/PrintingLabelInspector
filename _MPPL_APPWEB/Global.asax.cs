using _LABELINSP_APPWEB.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using XLIB_COMMON.Model;

namespace _LABELINSP_APPWEB
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ModelBinders.Binders.DefaultBinder = new CustomModelBinder();
            ViewEngines.Engines.Add(new MyRazorViewEngine());
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);       
            HostingEnvironment.RegisterObject(new HostingEnvironmentRegisteredObject());
        }
    }


    public class HostingEnvironmentRegisteredObject : IRegisteredObject
    {
        // this is called both when shutting down starts and when it ends
        public void Stop(bool immediate)
        {
            if (immediate)
                return;

            Logger2FileSingleton.Instance.SaveLog("HostingEnvironmentRegisteredObject.Stop");
            JobScheduler.Instance.Stop();
            // shutting down code here
            // there will about Shutting down time limit seconds to do the work
        }
    }
}
