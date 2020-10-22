using _LABELINSP_APPWEB.Areas._APPWEB.Controllers;
using MDL_LABELINSP.Entities;
using MDL_LABELINSP.Interfaces;
using MDL_LABELINSP.Models;
using MDL_LABELINSP.UnitOfWorks;
using MDL_LABELINSP.ViewModel;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using XLIB_COMMON.Model;

namespace _LABELINSP_APPWEB.Areas.LABELINSP.Controllers
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

        [HttpGet]
        public ActionResult LabelInspector(int port)
        {
            ViewBag.Port = port;
            return View();
        }
        [HttpGet]
        public ActionResult LabelInspectorTest()
        {
            ViewBag.Port = 0;
            return View();
        }
        [HttpPost]
        public JsonResult InspectLabelTest(string fileName)
        {
            LabelInspectionManager lim = new LabelInspectionManager(db, true);
            LabelinspViewModel packingLabelViewModel = lim.InspectLabelTest(fileName);
            return Json(packingLabelViewModel);
        }

        [HttpGet]
        public JsonResult TCPBarcodeReceived(string barcode, string workstationName)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<LabelInspectorHub>();
            //context.Clients.All.broadcastMessage(barcode);
            //context.Clients.All.broadcastMessage(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            context.Clients.Group(workstationName).broadcastMessage(barcode);
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult WorkorderLabelGetData(string serialNumber)
        {
            //uow.DisableProxyCreation();
            LabelinspViewModel packingLabelViewModel = new LabelinspViewModel();
            packingLabelViewModel.WorkorderLabel = uow.WorkorderLabelRepo.GetBySerialNumber(serialNumber);
            packingLabelViewModel.WorkorderLabelInspections = uow.WorkorderLabelInspectionRepo.GetByPackingLabelId(packingLabelViewModel.WorkorderLabel?.Id ?? 0);

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

        [HttpGet]
        public JsonResult SaveToDBTest(string serialNumber, string elc)
        {
            Logger2FileSingleton.Instance.SaveLog("TestLog");
            try
            {
                WorkorderLabel workorderLabel = new WorkorderLabel() {SerialNumber = serialNumber, TimeStamp = DateTime.Now };
                uow.WorkorderLabelRepo.AddOrUpdate(workorderLabel);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            

            return Json(0, JsonRequestBehavior.AllowGet);
        }

    }
}