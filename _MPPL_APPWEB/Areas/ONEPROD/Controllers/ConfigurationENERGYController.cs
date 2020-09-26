using MDL_ONEPROD;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Manager;
using MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.Repo;
using _MPPL_WEB_START.Areas.ONEPROD.Base.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Model.Scheduling.Interface;
using MDL_BASE.Models.IDENTITY;
using MDL_ONEPROD.ComponetMes.Models;
using MDL_ONEPROD.ComponentWMS.UnitOfWorks;
using MDL_ONEPROD.ComponentWMS._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_ONEPROD.ComponentENERGY;
using MDL_ONEPROD.ComponentENERGY.Entities;
using MDL_CORE.ComponentCore.Entities;
using _MPPL_WEB_START.Areas.ONEPROD.ViewModels.OEE;
using _MPPL_WEB_START.Areas.ONEPROD.Models;
using _MPPL_WEB_START.Areas._APPWEB.ViewModels;
using MDL_ONEPROD.ComponentOEE.ViewModels;

namespace _MPPL_WEB_START.Areas.ONEPROD.Controllers
{
    [Authorize(Roles = DefRoles.ONEPROD_ADMIN)]
    public class ConfigurationENERGYController : Controller
    {
        IDbContextOneProdENERGY db;
        //RepoPreprodConf repoConf; //= new RepoPreprodConf();
        UnitOfWorkOneProdENERGY uow;
        UnitOfWorkOneprod uowONEPROD;
        UnitOfWorkOneProdOEE uowOEE;

        public ConfigurationENERGYController(IDbContextOneProdENERGY db3, IDbContextOneProdOEE db2)
        {
            this.db = db3;
            uow = new UnitOfWorkOneProdENERGY(db3);
            uowONEPROD = new UnitOfWorkOneprod(db3);
            uowOEE = new UnitOfWorkOneProdOEE(db2);
            ViewBag.Skin = "defaultSkin";
        }
        public ActionResult Index()
        {
            return View();
        }

        //EnergyMeter
        public ActionResult EnergyMeter()
        {
            return View();
        }
        [HttpPost]
        public ActionResult EnergyMeterDelete(int id)
        {
            EnergyMeter mE = uow.EnergyMeterRepo.GetById(id);
            uow.EnergyMeterRepo.Delete(mE);
            return Json("");
        }

        [HttpPost]
        public JsonResult EnergyMeterUpdate(EnergyMeter item)
        {
            //item.Resource = uow.ResourceRepo.GetById((int)item.ResourceId);
            item.Resource = null;
            uow.EnergyMeterRepo.AddOrUpdate(item);
            return Json(item);
        }

        [HttpPost]
        public JsonResult EnergyMeterGetList(EnergyMeter filter, int pageIndex, int pageSize)
        {
            var query = uow.EnergyMeterRepo.GetList(filter);
            int startIndex = (pageIndex - 1) * pageSize;
            int itemsCount = query.Count();

            return Json(new { data = query.ToList(), itemsCount });
        }
        [HttpPost]
        public JsonResult EnergyMeterGroupUpdate(EnergyMeter item)
        {
            uow.EnergyMeterRepo.AddOrUpdate(item);
            return Json(item);
        }

        //Energy Cost
        
        public ActionResult EnergyCost()
        {
            return View();
        }
        [HttpPost]
        public ActionResult EnergyCostDelete(int id)
        {
            EnergyCost mE = uow.EnergyCostRepo.GetById(id);
            uow.EnergyCostRepo.Delete(mE);
            return Json("");
        }

        [HttpPost]
        public JsonResult EnergyCostUpdate(EnergyCost item)
        {
            JsonUpdateGridModel jUgridModel = new JsonUpdateGridModel();
            if (isValidItem(item))
            {
                jUgridModel.Item = item;
                return Json(jUgridModel);
            }
            else
            {
                jUgridModel.Item = uow.EnergyCostRepo.GetById(item.Id);
                jUgridModel.Error = true;
                return Json(jUgridModel);
            }
        }

        private bool isValidItem(EnergyCost item)
        {
            return uow.EnergyCostRepo.AddOrUpdate(item) > -1 ? true : false;
        }

        [HttpPost]
        public JsonResult EnergyCostGetList(EnergyCost filter, int pageIndex, int pageSize)
        {
            List<EnergyCost> items = new List<EnergyCost>();
            var items2 = uow.EnergyCostRepo.GetList(filter);
            int itemsCount = 0;
            return Json(new { data = items2, itemsCount });
        }
        [HttpPost]
        public JsonResult EnergyCostGroupUpdate(EnergyCost item)
        {
            uow.EnergyCostRepo.AddOrUpdate(item);
            return Json(item);
        }

        // Energy Consumption
        public ActionResult EnergyConsumption()
        {
            FilterOeeViewModel fovm = GetFiltersForOee();
            return View(fovm);
        }

        public FilterOeeViewModel GetFiltersForOee()
        {
            FilterOeeViewModel fovm = new FilterOeeViewModel();
            fovm.MachineList = uowONEPROD.ResourceRepo.GetListForDropDown();
            
            return fovm;
        }
        [HttpPost]
        public JsonResult GetChartEnergyConsumption(DateTime dateFrom, DateTime dateTo, int machineId, int intervalInHours = 24)
        {
            ChartEnergyConsumptionPreparer chdp = new ChartEnergyConsumptionPreparer(uow.EnergyConsumptionDataRepo,dateFrom, dateTo, intervalInHours, machineId);
            ChartViewModel vm = chdp.PrepareChartData(uowOEE.OEEReportProductionDataRepo, "Production");

            return Json(vm);
        }

        public ActionResult EnergyDetails()
        {
            return View();
        }
    }
}