using _MPPL_WEB_START.Areas._APPWEB.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.AP.Controllers
{
    [Authorize]
    public class StartController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Unavailable()
        {
            ViewBag.Messsage = "Funkcjonalność niedostępna";
            return View();
        }
    }
}