using _MPPL_WEB_START.Areas.iLOGIS.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.iLOGIS.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        const int iLogisVersion = 2;

        public HomeController()
        {
            ViewBag.Skin = "nasaSkin";
        }
        // GET: iLOGIS/Home
        public ActionResult Index(int id = 0)
        {
            HomeViewModel hvm = new HomeViewModel();
            hvm.Id = 1;
            return View(hvm);
        }

        [HttpPost]
        public JsonResult GetCurrentVersion(string moduleName)
        {
            int version = 10;
            return Json(version);
        }
    }
}