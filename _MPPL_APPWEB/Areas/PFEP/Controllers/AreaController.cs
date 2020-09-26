using _MPPL_WEB_START.Areas.PFEP.Models;
using _MPPL_WEB_START.Migrations;
using MDL_BASE.Enums;
using MDL_BASE.Models.IDENTITY;
using MDL_BASE.Models.MasterData;

using MDL_PFEP.Interface;
using MDL_PFEP.Models.DEF;
using MDL_PFEP.Repo.ELDISY_PFEP;
using MDLX_MASTERDATA.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XLIB_COMMON.Interface;
using XLIB_COMMON.Model;

namespace _MPPL_WEB_START.Areas.PFEP.Controllers
{
    public class AreaController : Controller
    {
        private IDbContextPFEP_Eldisy db;
        private RepoArea repoArea;
        

        public AreaController(IDbContextPFEP_Eldisy db)
        {
            this.db = db; //new DbContextAPP_Eldisy();
            repoArea = new RepoArea(db);           
        }

        // GET: PFEP/Area
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAreas(Area filterObject)
        {
            var items = repoArea.GetList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddArea (Area newObject)
        {
            repoArea.AddOrUpdate(newObject);
            return Json(newObject, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteArea(Area deleteObject)
        {
            deleteObject.Deleted = true;
            repoArea.AddOrUpdate(deleteObject);
            return Json(deleteObject, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AutocompleteByName(string Prefix)
        {
            List<Area> AreaAutocomplete = repoArea.GetAreaOrderList(Prefix);
            return Json(AreaAutocomplete, JsonRequestBehavior.AllowGet);
        }
    }
}