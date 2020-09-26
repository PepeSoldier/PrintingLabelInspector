using _MPPL_WEB_START.Areas._APPWEB.ViewModels;
using _MPPL_WEB_START.Areas.iLOGIS.ViewModels;
using Autofac;
using Autofac.Integration.Mvc;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentConfig.Mappers;
using MDL_iLOGIS.ComponentConfig.ViewModels;
using MDL_iLOGIS.ComponentWMS._Interfaces;
using MDL_ONEPROD.ComponentWMS.Models;
using MDL_WMS.ComponentConfig.UnitOfWorks;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.iLOGIS.Controllers
{
    [Authorize]
    public class WarehouseLocationController : Controller
    {
        private IDbContextiLOGIS db;
        private ILocataionManager locataionManager;
        private UnitOfWork_iLogis uow;

        public WarehouseLocationController(IDbContextiLOGIS db)
        {
            ViewBag.Skin = "nasaSkin";
            this.db = db;
            this.locataionManager = AutofacDependencyResolver.Current.ApplicationContainer.Resolve<ILocataionManager>();
            uow = new UnitOfWork_iLogis(db, locataionManager);
        }

        [HttpPost]
        public JsonResult FindEmptyLocation(string itemCode, int qtyPerPackage, int warehouseId = 0)
        {
            ItemWMS item = uow.ItemWMSRepo.GetByCode(itemCode);
            WarehouseLocation wl = new WarehouseLocationModel(uow).FindPlaceForItem(item, qtyPerPackage, warehouseId);
            return Json(wl);
        }
        [HttpPost]
        public JsonResult WorkplaceBufferParseBarcode(string barcode, string locationMode)
        {
            JsonModel jsonModel = new JsonModel();
            //TODO: qty nie moze byc pobierane po prostu z bacode, poniewaz po odstawieniu na bufor nikt nie zmienia etykiety na nowa
            //trzeba wykorzystac mechanizm lokalizacji
            //barcodeParser.Parse(barcode);
            //jsonModel.Data = new { barcodeParser };
            return Json(jsonModel);
        }
        [HttpPost]
        public JsonResult SetParentLocation(string warehouseLocationName, string parentWarehouseLocationName)
        {
            JsonModel jsonModel = new JsonModel();
            WarehouseLocation whLoc = uow.WarehouseLocationRepo.GetByName(warehouseLocationName);
            WarehouseLocation parentWhLoc = uow.WarehouseLocationRepo.GetByName(parentWarehouseLocationName);

            new WarehouseLocationModel(uow).SetParentLocation(whLoc, parentWhLoc);

            return Json(1);
        }
        [HttpPost]
        public JsonResult SetTrolleyLocation(string warehouseLocationName, string parentWarehouseLocationName)
        {
            JsonModel jsonModel = new JsonModel();
            WarehouseLocation whLoc = uow.WarehouseLocationRepo.GetByName(warehouseLocationName);
            WarehouseLocation parentWhLoc = uow.WarehouseLocationRepo.GetByName(parentWarehouseLocationName);

            if (whLoc == null)
            {
                jsonModel.SetMessage("Wózek '" + warehouseLocationName + "' nie został znaleziony", JsonMessageType.danger);
            }
            if (whLoc.Type.TypeEnum != WarehouseLocationTypeEnum.Trolley)
            {
                jsonModel.SetMessage("'" + warehouseLocationName + "' nie jest wózkiem", JsonMessageType.danger);
            }
            else if (parentWhLoc == null)
            {
                jsonModel.SetMessage("Lokacja '" + parentWarehouseLocationName + "' nie została znaleziona", JsonMessageType.danger);
            }
            else
            {
                new WarehouseLocationModel(uow).SetParentLocation(whLoc, parentWhLoc);
                jsonModel.SetMessage("Zalokalizowano wózek", JsonMessageType.success);
            }

            return Json(jsonModel);
        }
        [HttpPost]
        public JsonResult GetByNameAndType(string name, WarehouseLocationTypeEnum type = WarehouseLocationTypeEnum.Shelf)
        {
            WarehouseLocation loc = uow.WarehouseLocationRepo.GetByNameAndType(name, type);
            return Json(new { loc.Id, loc.Name, loc.Utilization });
        }
        [HttpPost]
        public JsonResult GetByNameAndTypeForItemId(string name, int itemId, WarehouseLocationTypeEnum type = WarehouseLocationTypeEnum.Shelf)
        {
            ItemWMS itemWMS = uow.ItemWMSRepo.Get(null, itemId, null);
            WarehouseLocation loc = uow.WarehouseLocationRepo.GetByNameAndType(name, type);
            PackageItem pi = uow.PackageItemRepo.Get(itemWMS, locationTypeId: loc != null? loc.Type.Id : 0); 

            if (pi != null)
            {
                return Json(new { loc.Id, loc.Name, loc.Utilization, maxQtyInPackage = pi.QtyPerPackage, maxPackagesPerPallet = pi.PackagesPerPallet, pickingStrategy = pi.PickingStrategy });
            }
            else
            {
                return Json(new { loc.Id, loc.Name, loc.Utilization, maxQtyInPackage = 0, maxPackagesPerPallet = 1, pickingStrategy = 0 });
            }
        }
        [HttpPost]
        public JsonResult GetLocation(string nameFormatted, int warehouseId = 0)
        {
            Warehouse wh = uow.WarehouseRepo.GetById(warehouseId);
            List<string> formattedLocations = new List<string>();

            Regex rgx = new Regex("[^a-zA-Z0-9]");
            string nameDeformatted = rgx.Replace(nameFormatted, "");

            formattedLocations.Add(nameFormatted);
            formattedLocations.Add(nameDeformatted);

            List<WarehouseLocationViewModel> vm = uow.WarehouseLocationRepo.GetList()
                .Where(x => 
                        formattedLocations.Contains(x.Name) && 
                        (x.WarehouseId == warehouseId || x.Warehouse.ParentWarehouseId == warehouseId || x.WarehouseId <= 0) &&
                        x.Warehouse.WarehouseType != MDL_iLOGIS.ComponentWMS.Enums.WarehouseTypeEnum.ExternalWarehouse)
                .ToList<WarehouseLocationViewModel>();

            if(vm == null || vm.Count <= 0)
            {
                vm = uow.WarehouseLocationRepo.GetList().Where(x => formattedLocations.Contains(x.Name)).ToList<WarehouseLocationViewModel>();
            }

            return Json(vm);
        }
    }
}