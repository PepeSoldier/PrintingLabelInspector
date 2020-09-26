using _MPPL_WEB_START.Areas._APPWEB.ViewModels;
using MDL_BASE.Models.Base;
using MDL_BASE.Models.IDENTITY;
using MDL_CORE.ComponentCore.Entities;
using MDL_CORE.ComponentCore.Enums;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentCore.Enums;
using MDL_iLOGIS.ComponentCore.Models;
using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_iLOGIS.ComponentWMS.Mappers;
using MDL_iLOGIS.ComponentWMS.ViewModels;
using MDL_ONEPROD.ComponentMes.Models;
using MDL_WMS.ComponentConfig.UnitOfWorks;
using MDLX_CORE.Model;
using MDLX_CORE.Model.PrintModels;
using MDLX_MASTERDATA.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;   
using System.Linq;
using System.Web.Mvc;
using XLIB_COMMON.Model;
using XLIB_COMMON.Repo.Base;
using XLIB_COMMON.Repo.IDENTITY;

namespace _MPPL_WEB_START.Areas.iLOGIS.Controllers
{
    [Authorize]
    public partial class WMSController : Controller
    {
        readonly IDbContextiLOGIS db;
        
        readonly RoleRepo RoleManager;
        private UserRepo userRepo;
        UnitOfWork_iLogis uow;

        public WMSController(IDbContextiLOGIS db, IRoleStore<ApplicationRole, string> roleStore)
        {
            this.db = db;
            uow = new UnitOfWork_iLogis(db);
            ViewBag.Skin = "nasaSkin";
            RoleManager = new RoleRepo(roleStore, db);
        }
        
        [HttpGet]
        public ActionResult Index(int id = 0)
        {
            return View();
        }
        [HttpGet, Authorize]
        public ActionResult MobileWMS()
        {
            return View();
        }
        [HttpGet]
        public ActionResult MobileMenu()
        {
            return View();
        }
        [HttpGet]
        public ActionResult MobileSettings()
        {
            SystemVariableRepo svr = new SystemVariableRepo(db);
            int mobilePrinterId = uow.SystemVariableRepo.GetValueInt("MobilePrinterId", User.Identity.GetUserId());
            Printer p = uow.PrinterRepo.GetList().Where(x => x.Id == mobilePrinterId).FirstOrDefault();
            ViewBag.MobilePrinterIp = p != null? p.IpAdress : "";

            return View();
        }
        [HttpPost]
        public JsonResult SaveMobilePrinter(string printerIP)
        {
            SystemVariableRepo svr = new SystemVariableRepo(db);

            try
            {
                Printer p = uow.PrinterRepo.GetList().Where(x => x.IpAdress == printerIP).FirstOrDefault();
                //int mobilePrinterId =  svr.GetValueInt("MobilePrinterId", User.Identity.GetUserId());
                svr.UpdateValue("MobilePrinterId", p.Id.ToString(), EnumVariableType.Int, User.Identity.GetUserId());
                return Json(0);
            }
            catch
            {
                return Json(-1);
            }
        }
        [HttpPost]
        public JsonResult DetachMobilePrinter()
        {
            SystemVariableRepo svr = new SystemVariableRepo(db);

            try
            {
                svr.UpdateValue("MobilePrinterId", "0", EnumVariableType.Int, User.Identity.GetUserId());
                return Json(0);
            }
            catch
            {
                return Json(-1);
            }
        }
        [HttpPost]
        public JsonResult TestMobilePrinter()
        {
            iLogisStatus status = iLogisStatus.PrintingProblem;

            try
            {
                SystemVariableRepo svr = new SystemVariableRepo(db);

                int mobilePrinterId = uow.SystemVariableRepo.GetValueInt("MobilePrinterId", User.Identity.GetUserId());
                Printer p = uow.PrinterRepo.GetList().Where(x => x.Id == mobilePrinterId).FirstOrDefault();


                string labelDefinition = "^XA^FWN^FO70,055^FS^CF0,60^FO85,100^FDDzien dobry!^FS^XZ";
                PrintLabelModelZEBRA printModel = new PrintLabelModelZEBRA(p.IpAdress);

                printModel.PrepareLabel(labelDefinition, new LabelData() { Code = "", Barcode = "" });
                printModel.Print();
                status = iLogisStatus.LabelPrinted;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            return Json(status);
        }


        [HttpPost]
        public JsonResult DashboardWarehouseOccupation()
        {
            return Json("");
        }
        [HttpPost]
        public JsonResult DashboardLastDaliveries()
        {
            return Json("");
        }
        [HttpPost]
        public JsonResult DashboardLinePicking()
        {
            return Json("");
        }
        [HttpPost]
        public JsonResult DashboardMissingPicking()
        {
            return Json("");
        }
        
        public void PrintLabel(string printerId)
        {
            PrintLabelModelZEBRA plZebra = new PrintLabelModelZEBRA(printerId);
        }

        //IncomingHistory
        public ActionResult IncomingHistory()
        {
            return View();
        }

        //TransporterLog
        public ActionResult TransporterLog()
        {
            //TransporterLog vm = new TransporterLog();
            return View();
        }

        [HttpPost, Authorize(Roles = DefRoles.ILOGIS_ADMIN)]
        public ActionResult TransporterLogDelete(int id)
        {
            TransporterLog tI = uow.TransporterLogRepo.GetById(id);
            uow.TransporterLogRepo.Delete(tI);
            return Json("");
        }
        [HttpPost, Authorize(Roles = DefRoles.ILOGIS_ADMIN)]
        public JsonResult TransporterLogUpdate(TransporterLog item)
        {
            uow.TransporterLogRepo.AddOrUpdate(item);            
            return Json(item);
        }
        [HttpPost]
        public JsonResult TransporterLogGetList(TransporterLog filter, int pageIndex = 1, int pageSize = 100)
        {
            IQueryable<TransporterLog> query = uow.TransporterLogRepo.GetList(filter);
            int startIndex = (pageIndex - 1) * pageSize;
            int itemsCount = query.Select(x=>x.Id).Count();
            List<TransporterLog> items = query.Skip(startIndex).Take(pageSize).ToList();

            return Json(new { data = items, itemsCount });
        }
        [HttpPost, Authorize(Roles = DefRoles.ILOGIS_ADMIN)]
        public JsonResult TransporterLogGroupUpdate(TransporterLog item)
        {
            uow.TransporterLogRepo.AddOrUpdate(item);
            return Json(item);
        }
        [HttpPost]
        public JsonResult GetQtyPerPackage(int itemId)
        {
            List<decimal> qtyPerPackageList = new List<decimal>();
            ItemWMS itemWMS = uow.ItemWMSRepo.Get(null, itemId, null);

            if (itemWMS != null)
            {
                qtyPerPackageList = uow.PackageItemRepo.GetList().Where(x =>
                    (x.ItemWMSId == itemWMS.Id || (itemWMS.Item.ItemGroupId != null && itemWMS.Item.ItemGroupId == x.ItemWMS.Item.Id))
                ).Select(x => x.QtyPerPackage).ToList();
            }

            return Json(qtyPerPackageList);
        }
    }
}