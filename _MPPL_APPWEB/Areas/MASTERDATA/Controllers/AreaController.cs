using MDL_BASE.Interfaces;
using MDL_BASE.Models.MasterData;
using MDLX_MASTERDATA.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.MASTERDATA.Controllers
{
    public class AreaController : Controller
    {
        private readonly IDbContextCore db;

        public AreaController(IDbContextCore db)
        {
            this.db = db;
            ViewBag.Skin = "nasaSkin";
        }

        //Area
        public ActionResult Area()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AreaDelete(Area areas)
        {
            RepoArea areaRepo = new RepoArea(db);
            areaRepo.Delete(areaRepo.GetById(areas.Id));
            return RedirectToAction("Area");
        }
        [HttpPost]
        public JsonResult AreaUpdate(Area areas)
        {
            RepoArea areaRepo = new RepoArea(db);
            areaRepo.AddOrUpdate(areas);
            return Json(areas);
        }
        [HttpPost]
        public JsonResult AreaGetList(Area filter)
        {
            RepoArea areaRepo = new RepoArea(db);
            List<Area> areas = areaRepo.GetList(filter).ToList();
            return Json(areas);
        }
    }
}