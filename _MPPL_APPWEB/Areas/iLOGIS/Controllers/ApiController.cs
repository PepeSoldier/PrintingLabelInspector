using MDL_BASE.Interfaces;
using MDL_BASE.Models.IDENTITY;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.DataExchange;
using PDFtoPrinter;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XLIB_COMMON.Model;
using XLIB_COMMON.Repo.IDENTITY;
using XLIB_RawPrint;

namespace _MPPL_WEB_START.Areas.iLOGIS.Controllers
{
    public class ApiController : Controller
    {
        IDbContextiLOGIS db;
        public ApiController(IDbContextiLOGIS db)
        {
            this.db = db;

            DbContext context = (DbContext)db;
            UserRepo _userManager1 = new UserRepo(new ApplicationUserStore<User>(context), db);
            RoleRepo _roleManager1 = new RoleRepo(new ApplicationRoleStore(context), db);
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult ImportWorkorders(string clientName)
        {
            ImportWorkordersAbstract importWorkorders = null;

            switch (clientName)
            {
                case "ElectroluxPLB": importWorkorders = new ImportWorkordersElectroluxPLB(db); break;
            }

            importWorkorders.ImportData();

            return Json(new { Action = "ImportWorkorders", importWorkorders.Errors }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ImportDeliveries(string clientName)
        {
            ImportDeliveriesAbstract importDeliveries = null;

            switch (clientName)
            {
                case "ElectroluxPLB": importDeliveries = new ImportDeliveries_ElectroluxPLB(db); break;
            }

            importDeliveries.ImportData();

            return Json(new { Action = "ImportDeliveries", importDeliveries.Errors }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ImportStocks(string clientName)
        {
            ImportStocksAbstract importStocks = null;

            switch (clientName)
            {
                case "ElectroluxPLB": importStocks = new ImportStocksElectroluxPLB(db); break;
            }

            importStocks.ImportData();

            return Json(new { Action = "ImportStocks", importStocks.Errors }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ImportMovements(string clientName)
        {
            ImportMovementsAbstract importMovements = null;

            switch (clientName)
            {
                case "ElectroluxPLB": importMovements = new ImportMovementsElectroluxPLB(db); break;
            }

            importMovements.ImportData();

            return Json(new { Action = "ImportMovements", importMovements.Errors }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ExportMovements(string clientName)
        {
            ExportMovementsAbstract exportMovements = null;

            switch (clientName)
            {
                case "ElectroluxPLB": exportMovements = new ExportMovementsElectroluxPLB(db); break;
            }

            exportMovements.ExportData();

            return Json(new { Action = "ExportMovements", exportMovements.Errors }, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public JsonResult PrintPDF()
        {
            //Printer printer = new Printer();
            //FileStream fs = System.IO.File.Open(@"c:\inetpub\label.pdf", FileMode.Open);
            //printer.PrintPDF("Brother DCP-1610W series", fs, "label.pdf", false);

            var printer = new PDFtoPrinterPrinter();
            //printer.Print(new PrintingOptions(@"\\PLWS3805\PLB_K24", @"c:\inetpub\label.pdf")).Wait();
            //PLWP1023, 10.26.33.172
            printer.Print(new PrintingOptions(@"\\plws1060\plwp1023", @"C:\inetpub\label.pdf")).Wait();

            return Json(0, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Test(string param1)
        {
            Logger2FileSingleton.Instance.SaveLog("ApiController.Test");
            return Json(param1 + ". " + DateTime.Now.ToLongTimeString(), JsonRequestBehavior.AllowGet);
        }
    }
}