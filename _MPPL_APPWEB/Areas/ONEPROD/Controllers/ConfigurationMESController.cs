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
using _MPPL_WEB_START.Areas._APPWEB.Models;

namespace _MPPL_WEB_START.Areas.ONEPROD.Controllers
{
    [Authorize(Roles = DefRoles.ONEPROD_ADMIN)]
    public class ConfigurationMESController : Controller
    {
        IDbContextOneprodMes db;
        RepoPreprodConf repoConf; //= new RepoPreprodConf();
        UnitOfWorkOneprod uow;  //= new UnitOfWorkONEPROD();
        UnitOfWorkOneProdMes uowMES;

        public ConfigurationMESController(IDbContextOneprodMes db)
        {
            this.db = db;
            uow = new UnitOfWorkOneprod(db);
            uowMES = new UnitOfWorkOneProdMes(db);
            repoConf = new RepoPreprodConf(db);
            ViewBag.Skin = "nasaSkin";
        }

        //Workplaces
        public ActionResult Workplace()
        {
            ViewBag.TraceabilityEnabled = false;
            ViewBag.ReportOnlineEnabled = false;

            if (AppClient.appClient != null && AppClient.appClient.SettingsONEPROD != null)
            {
                ViewBag.TraceabilityEnabled = AppClient.appClient.SettingsONEPROD.TraceabilityEnabled;
                ViewBag.ReportOnlineEnabled = AppClient.appClient.SettingsONEPROD.ReportOnlineAllowedMachinesIDs.Length > 0;
            }

            return View();
        }
        [HttpPost]
        public ActionResult WorkplaceDelete(Workplace Workplace)
        {
            uowMES.WorkplaceRepo.Delete(uowMES.WorkplaceRepo.GetById(Workplace.Id));
            return Json(0);
        }
        [HttpPost]
        public JsonResult WorkplaceUpdate(Workplace Workplace)
        {
            Workplace.Machine = null;
            if (Workplace.SelectedTask != null)
            {
                Workplace.SelectedTask.Resource = null;
            }
            uowMES.WorkplaceRepo.AddOrUpdate(Workplace);
            //PrintLabelModelZEBRA pz = new PrintLabelModelZEBRA();
            //var item = new ZebraDataLabelOne()
            //{
            //    BarcodeNumber = "1234567",
            //    ClientName = "ETAILERS",
            //    PrintDate = "19.02.2019",
            //    OrderNo = "DW-DE-M2M-K6QZTQYS484",
            //    PrinterIP = "192.168.0.10",
            //    SerialNo = "OC-16-001502-1000"
            //};
            //pz.PrepareLabelOne(item);
            //pz.SendLabelToPrinter();
            return Json(Workplace);
        }
        [HttpPost]
        public JsonResult WorkplaceGetList()
        {
            var list = uowMES.WorkplaceRepo.GetList().ToList();
            list.ForEach(x=> {
                if (x.SelectedTask != null && x.SelectedTask.Item != null)
                {
                    x.SelectedTask.Item.ItemGroupId = 0; x.SelectedTask.Item.ItemGroupOP = null;
                }
            }) ;
            return Json(list);
        }

    }
}