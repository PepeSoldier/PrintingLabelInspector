using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _LABELINSP_APPWEB.Models
{
    public abstract class AreaRegistrationMPPL : AreaRegistration
    {
        public void RegisterArea(AreaRegistrationContext context, string[] components)
        {
            string[] namespaces = new string[components.Length + 1];
            string[] viewLocatios = new string[components.Length];

            namespaces[0] = "_LABELINSP_APPWEB.Areas." + AreaName + ".Controllers";

            for (int i = 0; i < components.Length; i++)
            {
                namespaces[i + 1] = "_LABELINSP_APPWEB.Areas." + AreaName + "." + components[i] + ".Controllers";
                viewLocatios[i] = "~/Areas/" + AreaName + "/" + components[i] + "/Views/{1}/{0}.cshtml";
            }

            var razorEngine = ViewEngines.Engines.OfType<MyRazorViewEngine>().FirstOrDefault();
            razorEngine.AreaViewLocationFormats = razorEngine.AreaViewLocationFormats.Concat(viewLocatios).ToArray();
            razorEngine.AreaPartialViewLocationFormats = razorEngine.AreaPartialViewLocationFormats.Concat(viewLocatios).ToArray();

            context.MapRoute(
                AreaName + "_default",
                AreaName + "/{controller}/{action}/{id}",
                new { controller = "home", action = "Index", id = UrlParameter.Optional },
                namespaces: namespaces
            );
        }
    }
}