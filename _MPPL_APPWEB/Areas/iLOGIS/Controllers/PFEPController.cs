using _MPPL_WEB_START.Areas._APPWEB.Controllers;
using _MPPL_WEB_START.Areas.iLOGIS.ViewModels;
using MDL_AP.Interfaces;
using MDL_AP.Repo;
using MDL_AP.Repo.ActionPlan;
using MDL_BASE.Models.Base;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentPFEP.Models;
using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_iLOGIS.ComponentWMS.Enums;
using MDL_ONEPROD.ComponentWMS.Repos;
using MDL_WMS.ComponentConfig.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.iLOGIS.Controllers
{
    public class PFEPController : BaseController
    {
        readonly IDbContextiLOGIS db;
        //IDbContextAP dbContext;
        //UnitOfWorkActionPlan uowAP;
        UnitOfWork_iLogis uow;
        
        public PFEPController(IDbContextiLOGIS db)
        {
            this.db = db;
            //uowAP = new UnitOfWorkActionPlan(dbContext);
            uow = new UnitOfWork_iLogis(db);
            ViewBag.Skin = "nasaSkin";
        }
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Data()
        {
            return View();
        }
        [HttpPost]
        public JsonResult DataGetList(PFEPData filter,int pageIndex, int pageSize)
        {
            PFEPDataRepo repo = new PFEPDataRepo(db);
            IQueryable<PFEPData> query = repo.GetList(filter);

            int startIndex = (pageIndex - 1) * pageSize;
            int itemsCount = query.Count();
            List<PFEPData> data = query.OrderByDescending(x=>x.ItemCreatedDate)
                .Skip(startIndex).Take(pageSize).ToList();
            
            return Json(new { data, itemsCount });
        }

        [HttpPost]
        public JsonResult DataUpdate(PFEPData item)
        {
            string Url = new RepoAttachment(db).GetByPFEPData(item);
            if(Url == "")
            {
                item.PackingCardUrl = "";
            }

            return Json(item);
        }

        public ActionResult ChangeLog(int? objectId = null, int? parentObjectId = null, string objectDescription = "", string parentObjectDescription = "", string mode = "")
        {
            ViewBag.ObjectId = objectId;
            ViewBag.ObjectDescription = objectDescription;
            ViewBag.ParentObjectId = parentObjectId;
            ViewBag.ParentObjectDescription = parentObjectDescription;
            ViewBag.Mode = mode;
            return View();
        }

        [HttpPost]
        public JsonResult ChangeLogGetList(ChangeLog filter, string mode, int pageIndex, int pageSize)
        {
            //var query = uow.ChangeLogRepo.GetListByObjectIdAndName(filter.ObjectId, filter.ObjectName);
            //TODO: przefiltrować przez listę obiektów
            //var query = uow.ChangeLogRepo.GetListByParentId(filter.ParentObjectId);
            string[] tablesToConsider;
            IQueryable<ChangeLog> query;

            if (mode == "Item" || mode == "ItemWMS")
            {
                tablesToConsider = new string[4] { "Item", "ItemWMS", "PackageItem", "WorkstationItem" };
                query = uow.ChangeLogRepo.GetList(filter.ObjectId, filter.ObjectDescription, filter.ParentObjectId, filter.ParentObjectDescription, tablesToConsider);
            }
            else
            {
                tablesToConsider = new string[1] { mode };
                query = uow.ChangeLogRepo.GetList(filter.ObjectId, filter.ObjectDescription, filter.ParentObjectId, filter.ParentObjectDescription, tablesToConsider);
            }
            
            int startIndex = (pageIndex - 1) * pageSize;
            int itemsCount = query.Count();

            List<ChangeLog> items = query.OrderByDescending(x=>x.Date).Skip(startIndex).Take(pageSize).ToList();

            return Json(new { data = items, itemsCount });
        }


        public ActionResult TrainDashboard()
        {
            var trains = uow.TransporterRepo.GetList().Where(x => x.Type == EnumTransporterType.Train).ToList();
            ViewBag.Trains = trains;
            return View();
        }
        

        public ActionResult TrainRoute(int trainId)
        {
            var workstations = uow.DeliveryListItemRepo.GetList().Where(x => x.TransporterId == trainId).Select(x => x.Workstation).Distinct().OrderBy(x=>x.SortOrderTrain).ToList();
            ViewBag.Workstations = workstations;
            ViewBag.TrainId = trainId;

            return View();
        }

        [HttpPost]
        public JsonResult TrainGetCurrentData(int trainId)
        {
            TransporterLog tlFilter = new TransporterLog();
            tlFilter.TransporterId = trainId;
            tlFilter.EntryType = EnumTransporterLogEntryType.Delivery;

            TransporterLog tlLastEntry = uow.TransporterLogRepo.GetList(tlFilter).OrderByDescending(x => x.Id).Take(1).FirstOrDefault();

            string currentWorkstation = string.Empty;

            if(tlLastEntry != null)
            {
                currentWorkstation = tlLastEntry.Location;
            }

            return Json(currentWorkstation);
        }

    }
}