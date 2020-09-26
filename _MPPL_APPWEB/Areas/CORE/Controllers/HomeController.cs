using _MPPL_WEB_START.Areas.iLOGIS.ViewModels;
using MDL_BASE.Models.IDENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.CORE.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
            ViewBag.Skin = "nasaSkin";
        }

        //TODO: dlaczego index kontrolera Home to konfiguracja? 
        [Authorize(Roles = DefRoles.ADMIN)]
        public ActionResult Index(int id = 0)
        {
            HomeViewModel hvm = new HomeViewModel();
            hvm.Id = 1;
            return View(hvm);
        }
    }
}