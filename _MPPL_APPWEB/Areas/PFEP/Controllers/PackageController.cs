using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using _MPPL_WEB_START.Migrations;
using MDL_PFEP.Repo;
//using MDL_PFEP.Models.PFEP;
using MDL_PFEP.Models.DEF;
using MDLX_BASE.Repo;
using MDL_PFEP.Interface;
using MDL_BASE.Models.IDENTITY;
using MDL_PFEP.Repo.ELDISY_PFEP;
using XLIB_COMMON.Model;
using XLIB_COMMON.Interface;
using System.Collections.Generic;
using MDL_PFEP.Repo.DEF;
using MDL_iLOGIS.ComponentConfig.Entities;

namespace _MPPL_WEB_START.Areas.PFEP.Controllers
{
    public class PackageController : Controller
    {
        private IDbContextPFEP_Eldisy db;
        private GenericRepository<Package> repository;
        private PackageRepo packageRepo;

        public PackageController(IDbContextPFEP_Eldisy db)
        {
            this.db = db;
            repository = new GenericRepository<Package>(db);
            packageRepo = new PackageRepo(db);
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetArray(Package filterObject)
        {
            var items = packageRepo.GetListWithFilters(filterObject);
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdate(Package newObject)
        {
            if (User.IsInRole(DefRoles.PFEP_PACKINGINSTR_CONFIRMER) || User.IsInRole(DefRoles.ADMIN))
            {
                int k = packageRepo.AddOrUpdate(newObject);
                newObject.Id = k;
                AlertManager.Instance.AddAlert(AlertMessageType.success, "Zaktualizowano opakowanie", User.Identity.Name);
            }
            else
            {
                AlertManager.Instance.AddAlert(AlertMessageType.danger, "Nie masz uprawnień do zmian", User.Identity.Name);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(Package deleteObject)
        {
            if (User.IsInRole(DefRoles.ADMIN))
            {
                Package delObj = packageRepo.GetById(deleteObject.Id);
                packageRepo.MakeDeleted(delObj);
            }
            else
            {
                AlertManager.Instance.AddAlert(AlertMessageType.danger, "Nie masz uprawnień do usunięcia", User.Identity.Name);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AutocompleteByCode(string Prefix)
        {
            List<Package> AutocompleteByCode = packageRepo.GetPackageAutocompleteList(Prefix);
            return Json(AutocompleteByCode, JsonRequestBehavior.AllowGet);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
