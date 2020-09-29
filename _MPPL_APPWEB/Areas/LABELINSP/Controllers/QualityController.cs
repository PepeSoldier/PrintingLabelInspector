using _MPPL_WEB_START.Areas._APPWEB.Controllers;
using _MPPL_WEB_START.Areas.ONEPROD.Models;
using MDL_BASE.Interfaces;
using MDL_CORE.ComponentCore.ViewModel;
using MDLX_CORE.ComponentCore.UnitOfWorks;
using Microsoft.AspNet.SignalR;
using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.LABELINSP.Controllers
{
    //[System.Web.Mvc.Authorize(Roles = DefRoles.ONEPROD_MES_OPERATOR)]
    public partial class QualityController : BaseController
    {
        private readonly UnitOfWorkCore uow;
        private readonly IDbContextCore db;

        public QualityController(IDbContextCore db)
        {
            this.db = db;
            ViewBag.Skin = "nasaSkin";
            uow = new UnitOfWorkCore(db);
        }

        //PrintingLabelInspector Methods
        public ActionResult PrintingLabelInspector()
        {
            return View();
        }

        [HttpGet]
        public JsonResult TCPBarcodeReceived(string barcode)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<JobLabelCheckHub>();
            context.Clients.All.broadcastMessage(barcode);
            //context.Clients.All.broadcastMessage(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            //context.Clients.Group(workstationName).broadcastMessage(barcode);
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PackingLabelGetData(string serialNumber)
        {
            PackingLabelViewModel packingLabelViewModel = new PackingLabelViewModel();
            packingLabelViewModel.PackingLabel = uow.PackingLabelRepo.GetBySerialNumber(serialNumber);
            packingLabelViewModel.PackingLabelTests = uow.PackingLabelTestRepo.GetByPackingLabelId(packingLabelViewModel.PackingLabel.Id);

            return Json(packingLabelViewModel);
        }
    }
}