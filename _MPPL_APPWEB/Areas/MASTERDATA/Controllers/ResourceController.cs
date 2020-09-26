using MDL_BASE.Models.MasterData;
using MDL_ONEPROD.Repo;
using MDLX_MASTERDATA._Interfaces;
using MDLX_MASTERDATA.Entities;
using MDLX_MASTERDATA.Enums;
using MDLX_MASTERDATA.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.MASTERDATA.Controllers
{
    public class ResourceController : Controller
    {
        private readonly IDbContextMasterData db;
        UnitOfWorkMasterData uow;

        public ResourceController(IDbContextMasterData db)
        {
            this.db = db;
            uow = new UnitOfWorkMasterData(db);
            ViewBag.Skin = "nasaSkin";
        }

        // GET: MASTERDATA/Resource
        public ActionResult Resource()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResourceDelete(Resource2 resources)
        {
            uow.ResourceRepo.Delete(uow.ResourceRepo.GetById(resources.Id));
            return RedirectToAction("Resource");
        }
        [HttpPost]
        public JsonResult ResourceUpdate(Resource2 resources)
        {
            resources.ResourceGroupId = resources.ResourceGroupId < 1 ? null : resources.ResourceGroupId;
            uow.ResourceRepo.AddOrUpdate(resources);
            return Json(resources);
        }
        [HttpPost]
        public JsonResult ResourceGetList(Resource2 filter)
        {
            List<Resource2> resources = uow.ResourceRepo.GetList(filter).ToList();
            return Json(resources);
        }
        [HttpPost]
        public JsonResult ResourceDropDownGetList(Resource2 filter)
        {
            List<Resource2> resources = uow.ResourceRepo.GetList(filter)
                .Where(x => x.Type == ResourceTypeEnum.Resource) //&& x.AreaId == filter.AreaId)
                .OrderBy(x => x.ResourceGroup != null? x.ResourceGroup.Name : x.Name)
                .ThenBy(x => x.Name)
                .ToList();
            return Json(resources);
        }

        //[HttpGet]
        //public JsonResult ResourceUpdate(Resource2 machine)
        //{
        //    machine.ResourceGroup = null;
        //    machine.ResourceGroupId = machine.ResourceGroupId < 1 ? null : machine.ResourceGroupId;
        //    uow.ResourceRepo.AddOrUpdate(machine);
        //    return Json(machine);
        //}

        [HttpPost]
        public JsonResult ResourceGroupGetList(Resource2 filter)
        {
            List<Resource2> areas = uow.ResourceRepo.GetList(filter).Where(x=>x.Type == ResourceTypeEnum.Group).ToList();
            return Json(areas);
        }
    }
}