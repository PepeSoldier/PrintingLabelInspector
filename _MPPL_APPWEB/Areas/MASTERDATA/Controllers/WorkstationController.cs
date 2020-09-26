using _MPPL_WEB_START.Areas._APPWEB.Models;
using MDL_BASE.Interfaces;
using MDL_BASE.Models.IDENTITY;
using MDL_BASE.Models.MasterData;
using MDL_ONEPROD.Repo;
using MDLX_MASTERDATA._Interfaces;
using MDLX_MASTERDATA.Repos;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XLIB_COMMON.Model;
using XLIB_COMMON.Repo.Base;

namespace _MPPL_WEB_START.Areas.MASTERDATA.Controllers
{
    public class WorkstationController : Controller
    {
        private readonly IDbContextMasterData db;
        private ChangeLogRepo changeLogRepo;

        public WorkstationController(IDbContextMasterData db, IDbContextCore dbCore)
        {
            this.db = db;
            changeLogRepo = new ChangeLogRepo(dbCore);
            ViewBag.Skin = "nasaSkin";
        }

        public ActionResult Workstation()
        {
            return View();
        }

        [HttpPost, Authorize(Roles = DefRoles.ILOGIS_ADMIN)]
        public ActionResult WorkstationDelete(Workstation workstation)
        {
            RepoWorkstation workstationRepo = new RepoWorkstation(db);
            workstationRepo.Delete(workstationRepo.GetById(workstation.Id));
            return RedirectToAction("Workstation");
        }
        [HttpPost, Authorize(Roles = DefRoles.ILOGIS_ADMIN)]
        public JsonResult WorkstationUpdate(Workstation workstation)
        {
            RepoWorkstation workstationRepo = new RepoWorkstation(db);

            Workstation oldPart = workstationRepo.GetByIdAsNoTracking(workstation.Id);
            ObjectsComparer oc = new ObjectsComparer();
            oc.DetectChanges(oldPart, workstation, workstation.Id, workstation.Id, workstation.Name, workstation.Name);
            changeLogRepo.AddChangeLogs(oc.ObjectDataChanges, User.Identity.GetUserId());

            workstationRepo.AddOrUpdate(workstation);
            return Json(workstation);
        }
        [HttpPost]
        public JsonResult WorkstationGetList(Workstation filter)
        {
            RepoWorkstation workstationRepo = new RepoWorkstation(db);
            List<Workstation> workstations = workstationRepo.GetList(filter).OrderBy(x=>x.SortOrder).ToList();
            return Json(workstations);
        }
        [HttpPost]
        public JsonResult Autocomplete(string prefix, string lineName = "")
        {
            UnitOfWorkMasterData uow = new UnitOfWorkMasterData(db);
            List<AutocompleteViewModel> acData = uow.WorkstationRepo.GetList()
                .Where(x => 
                        x.Deleted == false && 
                        x.Name.StartsWith(prefix) &&
                        (lineName.Length <= 0 || x.Line.Name.Contains(lineName)) )
                .OrderBy(x => x.Name)
                .Take(10)
                .Select(x => new AutocompleteViewModel {
                    TextField = x.Name + " [" + x.Line.Name + "]",
                    ValueField = x.Id.ToString(), Data1 = x.Name, Data2 = x.Line.Name
                })
                .ToList();

            return Json(acData);
        }
    }
}