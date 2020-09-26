using MDL_BASE.Interfaces;
using MDL_BASE.Models.MasterData;
using MDL_ONEPROD.Repo;
using MDLX_MASTERDATA.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.MASTERDATA.Controllers
{
    public class ContractorController : Controller
    {
        private readonly IDbContextCore db;
        UnitOfWorkMasterData uow;

        public ContractorController(IDbContextCore db)
        {
            this.db = db;
            ViewBag.Skin = "nasaSkin";
            uow = new UnitOfWorkMasterData(db);
        }

        [HttpGet]
        public ActionResult Contractor()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ContractorDelete(Contractor Contractors)
        {
            RepoContractor ContractorRepo = new RepoContractor(db);
            ContractorRepo.Delete(ContractorRepo.GetById(Contractors.Id));
            return RedirectToAction("Contractor");
        }
        [HttpPost]
        public JsonResult ContractorUpdate(Contractor Contractors)
        {
            RepoContractor ContractorRepo = new RepoContractor(db);
            ContractorRepo.AddOrUpdate(Contractors);
            return Json(Contractors);
        }
        [HttpPost]
        public JsonResult ContractorGetList(Contractor filter)
        {
            RepoContractor ContractorRepo = new RepoContractor(db);
            List<Contractor> Contractors = ContractorRepo.GetList(filter).ToList();
            return Json(Contractors);
        }

        [HttpPost]
        public JsonResult Autocomplete(string prefix)
        {
            List<Contractor> autocomplete = uow.RepoContractor.GetContractorAutocompleteList(prefix);
            return Json(autocomplete, JsonRequestBehavior.AllowGet);
        }
    }
}