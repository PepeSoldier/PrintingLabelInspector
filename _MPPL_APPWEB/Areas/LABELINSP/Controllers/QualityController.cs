﻿using _MPPL_WEB_START.Areas._APPWEB.Controllers;
using _MPPL_WEB_START.Areas.LABELINSP.Interfaces;
using _MPPL_WEB_START.Areas.LABELINSP.Models;
using _MPPL_WEB_START.Areas.LABELINSP.UnitOfWorks;
using _MPPL_WEB_START.Areas.LABELINSP.ViewModel;
using MDL_LABELINSP.Models;
using Microsoft.AspNet.SignalR;
using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.LABELINSP.Controllers
{
    //[System.Web.Mvc.Authorize(Roles = DefRoles.ONEPROD_MES_OPERATOR)]
    public partial class QualityController : BaseController
    {
        private readonly UnitOfWorkLabelInsp uow;
        private readonly IDbContextLabelInsp db;

        public QualityController(IDbContextLabelInsp db)
        {
            this.db = db;
            ViewBag.Skin = "nasaSkin";
            uow = new UnitOfWorkLabelInsp(db);
        }

        //PrintingLabelInspector Methods
        public ActionResult PrintingLabelInspector(int port)
        {
            ViewBag.Port = port;
            return View();
        }

        [HttpGet]
        public JsonResult TCPBarcodeReceived(string barcode, string workstationName)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<JobLabelCheckHub>();
            //context.Clients.All.broadcastMessage(barcode);
            //context.Clients.All.broadcastMessage(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            context.Clients.Group(workstationName).broadcastMessage(barcode);
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PackingLabelGetData(string serialNumber)
        {
            PackingLabelViewModel packingLabelViewModel = new PackingLabelViewModel();
            packingLabelViewModel.PackingLabel = uow.PackingLabelRepo.GetBySerialNumber(serialNumber);
            packingLabelViewModel.PackingLabelTests = uow.PackingLabelTestRepo.GetByPackingLabelId(packingLabelViewModel.PackingLabel?.Id ?? 0);

            return Json(packingLabelViewModel);
        }

        [HttpGet]
        public JsonResult InspectLabel(string serialNumber)
        {
            ImageProcessing ip = new ImageProcessing();

            ip.SetImage(@"C:\inetpub\wwwroot\LABELINSP\RawLables\" + serialNumber + ".png");

            string expectedB = "2409110790362103412345";
            string expectedS = "30385789215529";
            string expectedN = "HJÄLPSAM";
            string expectedP = "30385789";

            ip.RotateImage(180);
            ip.BarcodeDetectReadAddFrame_Big(expectedB);
            ip.BarcodeDetectReadAddFrame_Small(expectedS);
            ip.ReadModelName(expectedN);
            ip.ReadIKEAProductCode(expectedP);

            //ip.SaveFinalPreviewImage(@"C:\inetpub\wwwroot\LABELINSP\InspectedLabels\" + serialNumber + ".png");
            ip.SaveAllImages(@"C:\inetpub\wwwroot\LABELINSP\InspectedLabels\", serialNumber);

            return Json(0, JsonRequestBehavior.AllowGet);
        }
    }
}