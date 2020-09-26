using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.ONEPROD.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
            ViewBag.Skin = "nasaSkin";
        }

        public ActionResult Index(int? id)
        {

            return View();
        }
    }
}