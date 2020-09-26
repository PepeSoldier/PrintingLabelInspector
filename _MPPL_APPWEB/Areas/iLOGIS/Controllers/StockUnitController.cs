using _MPPL_WEB_START.Areas._APPWEB.ViewModels;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentCore.Enums;
using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_iLOGIS.ComponentWMS.Mappers;
using MDL_iLOGIS.ComponentWMS.Models;
using MDL_iLOGIS.ComponentWMS.ViewModels;
using MDL_ONEPROD.ComponentWMS.Models;
using MDL_WMS.ComponentConfig.UnitOfWorks;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using XLIB_COMMON.Model;
using Microsoft.AspNet.Identity;
using MDL_iLOGIS.ComponentWMS.Enums;
using System;
using MDL_BASE.Models.IDENTITY;
using MDL_iLOGIS.ComponentCore.Models;
using System.Diagnostics;
using System.Text.RegularExpressions;
using XLIB_COMMON.Enums;
using MDLX_MASTERDATA.Models;
using MDLX_CORE.Model;

namespace _MPPL_WEB_START.Areas.iLOGIS.Controllers
{
    [Authorize]
    public class StockUnitController : Controller
    {
        private readonly IDbContextiLOGIS db;
        private UnitOfWork_iLogis uow;
        private WarehouseLocationModel whlModel;
        private iLogisMovementManager movementManager;

        public StockUnitController(IDbContextiLOGIS db)
        {
            this.db = db;
            uow = new UnitOfWork_iLogis(db);
            whlModel = new WarehouseLocationModel(uow);
            movementManager = new iLogisMovementManager(db);
            ViewBag.Skin = "nasaSkin";
        }

        //------------------------------------------------------------------------------------------
        //-------------------------PODSTAWOWE OPERACJE----------------------------------------------
        [HttpPost]
        public JsonResult Get(int id)
        {
            StockUnit pi = uow.StockUnitRepo.GetById(id);
            return Json(pi);
        }
        [HttpPost]
        public JsonResult GeWhLocById(int id)
        {
            WarehouseLocation warehouseLocation = uow.WarehouseLocationRepo.GetById(id);
            return Json(warehouseLocation);
        }
        [HttpPost]
        public JsonResult GetByBarcode(string barcode, string template)
        {
            StockUnitViewModel vm = new StockUnitViewModel();
            BarcodeManager parser = new BarcodeManager();
            StockUnit stockUnit;
            parser.Parse(barcode, template);

            //EXCEPTION FOR STEEL. TEMPORARY
            //9000001984310468500
            //9SSSSSSSSSS1QQQQQDD
            if (template == "9SSSSSSSSSS1QQQQQDD")
            {
                stockUnit = uow.StockUnitRepo.GetPLVCoilBySerialNumber(parser.SerialNumber);
            }
            else
            {
                stockUnit = uow.StockUnitRepo.GetBySerialNumberAndItemCode(parser.SerialNumber, parser.ItemCode);
            }

            if (stockUnit != null)
            {
                vm = StockUnitMapper.Map(stockUnit);
            }
            else
            {
                vm.ItemCode = parser.ItemCode;
                vm.WarehouseLocationName = parser.Location;
                vm.CurrentQtyinPackage = parser.Qty;
                vm.SerialNumber = parser.SerialNumber;
                vm.Id = parser.StockUnitId;
            }

            return Json(vm);
        }
        [HttpPost]
        public JsonResult GetByIdOrSerialNumber(int id, string serialNumber)
        {
            StockUnitViewModel vm = new StockUnitViewModel();
            StockUnit stockUnit = uow.StockUnitRepo.GetById(id);

            if (stockUnit == null && serialNumber.Length > 0)
            {
                stockUnit = uow.StockUnitRepo.GetBySerialNumber(serialNumber);
            }

            if (stockUnit != null)
            {
                vm = StockUnitMapper.Map(stockUnit);
            }

            return Json(vm);
        }
        [HttpPost]
        public JsonResult GetByCodeFromIncoming(string itemCode)
        {
            StockUnitViewModel vm = new StockUnitViewModel();

            ItemWMS itemWMS = uow.ItemWMSRepo.Get(null, null, itemCode);
            StockUnit stockUnit = uow.StockUnitRepo.GetFromIncomingAreaByItemWMSId(itemWMS.Id);

            if (stockUnit != null)
            {
                vm = StockUnitMapper.Map(stockUnit);
            }

            return Json(vm);
        }
        [HttpPost]
        public JsonResult GetByCodeAndLocation(string itemCode, string locationName)
        {
            StockUnitViewModel vm = new StockUnitViewModel();
            //ItemWMS itemWMS = uow.ItemWMSRepo.Get(null, null, itemCode);

            StockUnit stockUnit = uow.StockUnitRepo.GetByCodeAndLocationAndQty(itemCode, locationName);

            if (stockUnit != null)
            {
                vm = StockUnitMapper.Map(stockUnit);
            }

            return Json(vm);
        }

        [HttpPost, Authorize]
        public JsonResult CreateNew(
            int? itemId = null, int? itemWMSId = null, string itemCode = null, decimal qty = 0, string serialNumber = null, EnumMovementType type = EnumMovementType.Unassigned,
            int maxQtyPerPackage = 0, int warehouseId = 0, int locationTypeId = 0, int locationId = 0, int packageId = 0, int numberOfPackages = 1, bool print = true)
        {
            iLogisStatus status = iLogisStatus.NoError;
            StockUnitViewModel vm = null;

            ItemWMS itemWMS = uow.ItemWMSRepo.Get(itemWMSId, itemId, itemCode);

            if (itemWMS != null)
            {
                PackageItem packageItem = uow.PackageItemRepo.Get(itemWMS, maxQtyPerPackage, warehouseId, locationTypeId, packageId, locationId);
                Warehouse warehouse = uow.WarehouseRepo.GetById(warehouseId);

                if (packageItem != null)
                {
                    StockUnitModel stockUnitModel = new StockUnitModel(db);
                    iLogisPrintLabelManager printLabelManager = new iLogisPrintLabelManager(db, User.Identity.Name, print);

                    for (int i = 0; i < numberOfPackages; i++)
                    {
                        using (var transaction = db.BeginTransaction())
                        {
                            try
                            {
                                StockUnit stockUnit = stockUnitModel.CreateStockUnit_OnVirtual(itemWMS, warehouse, packageItem, qty, maxQtyPerPackage, serialNumber);
                                stockUnitModel.Save();

                                StockUnit stockUnitSource = new StockUnit();
                                ReflectionHelper.CopyProperties(stockUnit, stockUnitSource);

                                status = whlModel.LocateStockUnit(stockUnit, locationTypeId: locationTypeId, packageId: packageId, warehouseId: warehouseId, locationId: locationId);
                                if (status == iLogisStatus.NoError)
                                {
                                    movementManager.CreateMovementLog(stockUnitSource, stockUnit, User.Identity.GetUserId(), qty, stockUnit.UnitOfMeasure);
                                    movementManager.Save();

                                    vm = stockUnit.FirstOrDefault<StockUnitViewModel>();
                                    printLabelManager.PrintLabelForStockUnit(stockUnit);
                                    
                                    transaction.Commit();
                                }
                                else
                                {
                                    transaction.Rollback();
                                }
                            }
                            catch(Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                                transaction.Rollback();
                            }
                        }
                    }
                }
                else
                {
                    status = iLogisStatus.PackageItemNotFound;
                }
            }
            else
            {
                status = iLogisStatus.ItemWMSNotFound;
            }

            return Json(new { data = vm, status });
        }
        [HttpPost, Authorize]
        public JsonResult CreateNewFromDelivery(int itemWMSId, decimal qty, int deliveryId, int deliveryItemId,
           int maxQtyPerPackage = 0, int warehouseId = 0, int packageId = 0, int numberOfPackages = 1)
        {
            iLogisStatus status = iLogisStatus.NoError;
            StockUnitViewModel vm = null;
            ItemWMS itemWMS = uow.ItemWMSRepo.Get(itemWMSId, null, null);

            if (itemWMS != null)
            {
                StockUnitModel stockUnitModel = new StockUnitModel(db);
                PackageItem packageItem = uow.PackageItemRepo.Get(itemWMS, maxQtyPerPackage, warehouseId, 0, packageId, 0);
                Warehouse warehouse = uow.WarehouseRepo.GetById(warehouseId);
                StockUnit sourceStockUnit = uow.StockUnitRepo.GetFromIncomingAreaByItemWMSId(itemWMS.Id);
                iLogisPrintLabelManager printLabelManager = new iLogisPrintLabelManager(db, User.Identity.Name, true);
                decimal qtyTemp = 0;

                if (packageItem != null && sourceStockUnit != null)
                {
                    for (int i = 0; i < numberOfPackages; i++)
                    {
                        qtyTemp = Math.Min(qty, sourceStockUnit.CurrentQtyinPackage);

                        if (qtyTemp > 0)
                        {
                            using (var transaction = db.Database.BeginTransaction())
                            {
                                try { 
                                    sourceStockUnit = stockUnitModel.TakeQtyFromPackage(sourceStockUnit, qtyTemp);

                                    StockUnit stockUnit = stockUnitModel.CreateStockUnit_OnIncoming(itemWMS, warehouse, packageItem, qtyTemp, maxQtyPerPackage, null);
                                    stockUnit.ReferenceDeliveryItemId = deliveryItemId;
                                    stockUnitModel.Save();

                                    status = whlModel.LocateStockUnit(stockUnit, packageId: packageId, warehouseId: warehouseId);
                                    if (status == iLogisStatus.NoError)
                                    {
                                        movementManager.CreateMovementLog(sourceStockUnit, stockUnit, User.Identity.GetUserId(), qty, stockUnit.UnitOfMeasure);
                                        movementManager.Save();
                                    }

                                    printLabelManager.PrintLabelForStockUnit(stockUnit);
                                    vm = stockUnit.FirstOrDefault<StockUnitViewModel>();
                                    transaction.Commit();
                                }
                                catch
                                {
                                    status = iLogisStatus.PrintingProblem;
                                    transaction.Rollback();
                                }
                            }
                        }
                    }
                }
                else
                {
                    status = iLogisStatus.PackageItemNotFound;

                    if(sourceStockUnit == null)
                    {
                        status = iLogisStatus.StockUnitQtyInPackageIsLess;
                    }
                }
            }
            else
            {
                status = iLogisStatus.ItemWMSNotFound;
            }

            return Json(new { data = vm, status });
        }
        [HttpPost]
        public JsonResult CreateNewFromDeliveryManual(List<DeliveryItemViewModel> deliveryItems, bool oneLabel = false)
        {
            iLogisStatus status = iLogisStatus.NoError;
            StockUnitViewModel vm = null;
            StockUnitModel stockUnitModel = new StockUnitModel(db);
            WarehouseLocationModel wlm = new WarehouseLocationModel(uow);
            iLogisPrintLabelManager printLabelManager = new iLogisPrintLabelManager(db, User.Identity.Name, true);

            if (deliveryItems.Count > 0)
            {
                ItemWMS itemWMS = uow.ItemWMSRepo.Get(deliveryItems[0].ItemWMSId, null, null);
                StockUnit sourceStockUnit = uow.StockUnitRepo.GetFromIncomingAreaByItemWMSId(itemWMS != null ? itemWMS.Id : 0);
                WarehouseLocation wl = uow.WarehouseLocationRepo.GetByName(deliveryItems[0].DestinationLocationName);
                PackageItem pi = uow.PackageItemRepo.GetById(deliveryItems[0].PackageItemId);
                List<KeyValuePair<int, decimal>> deliveryItemsIds = new List<KeyValuePair<int, decimal>>();

                if (itemWMS != null && sourceStockUnit != null && wl != null)
                {
                    if (pi == null)
                    {
                        pi = uow.PackageItemRepo.Get(itemWMS, deliveryItems[0].QtyInPackage, wl.WarehouseId, wl.TypeId ?? 0, wl.Id);
                    }

                    if (oneLabel)
                    {
                        deliveryItemsIds = deliveryItems.Select(x => new KeyValuePair<int, decimal>(x.Id, x.QtyInPackage)).ToList();
                        decimal sum = deliveryItems.Sum(x => x.QtyInPackage);
                        deliveryItems = deliveryItems.Take(1).ToList();
                        deliveryItems[0].QtyInPackage = sum;
                    }

                    foreach (var di in deliveryItems)
                    {
                        using (var transaction = db.Database.BeginTransaction())
                        {
                            try
                            {
                                sourceStockUnit = stockUnitModel.TakeQtyFromPackage(sourceStockUnit, di.QtyInPackage);
                                StockUnit stockUnit = stockUnitModel.CreateStockUnit_OnIncoming(itemWMS, wl.Warehouse, pi, di.QtyInPackage, di.QtyInPackage, null);
                                stockUnit.ReferenceDeliveryItemId = di.Id;
                                stockUnitModel.Save();

                                wlm.SetLocationOfStockUnit(stockUnit, wl, pi, pi != null ? -1 : (1m / deliveryItems.Count), force: true);
                                movementManager.SetFreeText("Delivery: " + di.DeliveryId.ToString(), null);
                                movementManager.CreateMovementLog(sourceStockUnit, stockUnit, User.Identity.GetUserId(), di.QtyInPackage, stockUnit.UnitOfMeasure);
                                movementManager.Save();

                                if (!oneLabel)
                                {
                                    var dict = new List<KeyValuePair<int, decimal>>();
                                    dict.Add(new KeyValuePair<int, decimal>(di.Id, di.QtyInPackage));
                                    _SetLocationAssignedStatus(dict, di.QtyInPackage);
                                }
                                else
                                {
                                    _SetLocationAssignedStatus(deliveryItemsIds, di.QtyInPackage);
                                }

                                vm = stockUnit.FirstOrDefault<StockUnitViewModel>();
                                transaction.Commit();
                                PrintLabel(stockUnit, status);
                            }
                            catch
                            {
                                status = iLogisStatus.PrintingProblem;
                                transaction.Rollback();
                            }
                        }
                    }
                }
                else
                {
                    if (itemWMS == null) //&& pi == null && wl == null) 
                        status = iLogisStatus.ItemWMSNotFound;
                    else if(sourceStockUnit == null)
                        status = iLogisStatus.StockUnitNotFound;
                    else if(wl == null)
                        status = iLogisStatus.LocationNotFound;
                    else if (pi == null)
                        status = iLogisStatus.PackageItemNotFound;
                }
            }

            return Json(new { data = vm, status });
        }

        private void _SetLocationAssignedStatus(List<KeyValuePair<int, decimal>> deliveryItemsIdWithQty, decimal qtyToBeLocated)
        {
            decimal qty = 0;
            List<DeliveryItem> deliveryItems = new List<DeliveryItem>();
            deliveryItems = uow.DeliveryItemRepo.GetByIds(deliveryItemsIdWithQty.Select(x=>x.Key).ToList()).ToList();

            foreach(var keyValuePair in deliveryItemsIdWithQty)
            {
                int deliveryItemId = keyValuePair.Key;
                decimal qtyInPackage = keyValuePair.Value;
                DeliveryItem di = deliveryItems.FirstOrDefault(x => x.Id == deliveryItemId);

                if(di != null)
                {
                    qty = Math.Min(Math.Min(qtyToBeLocated, di.TotalQty - di.TotalLocatedQty), qtyInPackage);
                    qtyToBeLocated -= qty;
                    di.TotalLocatedQty += qty;
                    di.IsLocationAssigned = true;
                }
            }

            uow.DeliveryItemRepo.AddOrUpdateRange(deliveryItems);
        }

        [HttpPost, Authorize]
        public JsonResult DeleteStockUnit(string serialNumber)
        {
            iLogisStatus status = iLogisStatus.NoError;
            StockUnitViewModel vm = new StockUnitViewModel();

            using (var transaction = db.BeginTransaction())
            {
                try
                {
                    StockUnit stockUnit = uow.StockUnitRepo.GetBySerialNumber(serialNumber);
                    whlModel.DeleteFromLocation(stockUnit);
                    vm.WarehouseLocationUtilization = stockUnit.WarehouseLocation.Utilization;
                    //movementManager.SetFreeText(, null);
                    movementManager.CreateMovementLogByType(stockUnit, User.Identity.GetUserId(), stockUnit.CurrentQtyinPackage, stockUnit.UnitOfMeasure, EnumMovementType.CODE_103);
                    movementManager.Save();
                    uow.StockUnitRepo.Delete(stockUnit);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                }
            }
            
            return Json(new { data = vm, status });
        }

        //------------------------------------------------------------------------------------------
        //-------------------------ETYKIETA ZBIORCZA (GRUPY)----------------------------------------

        [HttpPost, Authorize]
        public JsonResult AddPackageToGroup(int stockUnitId, int stockUnitGroupId)
        {
            StockUnit stockUnit = uow.StockUnitRepo.GetById(stockUnitId);
            StockUnit stockUnitGroup = uow.StockUnitRepo.GetById(stockUnitGroupId);
            JsonModel jsonModel = new JsonModel();

            if (stockUnit != null && !stockUnit.IsGroup && stockUnitGroup != null && stockUnitGroup.IsGroup)
            {
                StockUnit stockUnitSource = new StockUnit();
                ReflectionHelper.CopyProperties(stockUnit, stockUnitSource);

                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        stockUnit.GroupId = stockUnitGroup.Id;
                        stockUnit.WarehouseLocationId = stockUnitGroup.WarehouseLocationId;
                        uow.StockUnitRepo.AddOrUpdate(stockUnit);
                        movementManager.CreateMovementLog(stockUnitSource, stockUnit, User.Identity.GetUserId(), stockUnit.CurrentQtyinPackage, stockUnit.UnitOfMeasure);
                        movementManager.Save();

                        new iLogisPrintLabelManager(db, User.Identity.Name, true).PrintLabelForStockUnit(stockUnitGroup);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                }
            }
            else
            {
                jsonModel.MessageType = JsonMessageType.danger;
                jsonModel.Message = "W opakowaniu znajduje się inny kod artykułu";
            }

            return Json(jsonModel);
        }
        [HttpPost, Authorize]
        public JsonResult GreateGroup(int[] stockUnitIds)
        {
            JsonModel jsonModel = new JsonModel();
            List<StockUnit> stockUnits = new List<StockUnit>();

            foreach(int stockUnitId in stockUnitIds)
            {
                StockUnit su = uow.StockUnitRepo.GetById(stockUnitId);
                if(su != null)
                {
                    stockUnits.Add(su);
                }
            }

            if(stockUnits.Count > 1)
            {
                decimal totalQty = stockUnits.Sum(x => x.CurrentQtyinPackage);
                StockUnitModel stockUnitModel = new StockUnitModel(db);
                iLogisPrintLabelManager printLabelManager = new iLogisPrintLabelManager(db, User.Identity.Name, true);

                StockUnit stockUnitGroup = stockUnitModel.CreateStockUnitGroup_OnVirtual(stockUnits[0].ItemWMS, stockUnits[0].WarehouseLocation.Warehouse, totalQty);
                stockUnitModel.Save();

                foreach (StockUnit su in stockUnits)
                {
                    su.GroupId = stockUnitGroup.Id;
                }
                uow.StockUnitRepo.AddOrUpdateRange(stockUnits);

                stockUnitModel.Save();

            }
            else
            {
                jsonModel.MessageType = JsonMessageType.danger;
                jsonModel.Message = "Wymagane są conajmniej 2 opakowania by utowrzyć grupę";
            }


            return Json(jsonModel);
        }

        //------------------------------------------------------------------------------------------
        //-------------------------PRZESUNIECIA-----------------------------------------------------

        //TODO: AddToPackage dodać możliwość użycia w interfejsie użytkownika
        [HttpPost, Authorize]
        public JsonResult MoveToStockUnit(int stockUnitId, int destStockUnitId, decimal qty)
        {
            StockUnit sourceStockUnit = uow.StockUnitRepo.GetById(stockUnitId);
            StockUnit destStockUnit = uow.StockUnitRepo.GetById(destStockUnitId);
            JsonModel jsonModel = new JsonModel();

            if (destStockUnit != null && sourceStockUnit != null)
            {
                if(destStockUnit.ItemWMSId != sourceStockUnit.ItemWMSId)
                {
                    jsonModel.Status = (int)iLogisStatus.ItemWMSNotTheSame;
                    jsonModel.MessageType = JsonMessageType.danger;
                    return Json(jsonModel);
                }

                decimal qtyAvailable = sourceStockUnit.CurrentQtyinPackage - sourceStockUnit.ReservedQty;
                if (qty > qtyAvailable)
                {
                    jsonModel.Status = (int)iLogisStatus.StockUnitRequestedQtyNotAvailable;
                    jsonModel.MessageType = JsonMessageType.danger;
                    jsonModel.Data = "Dostępna ilość: " + qtyAvailable.ToString("0.#####");
                    return Json(jsonModel);
                }

                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        StockUnitModel stockUnitModel = new StockUnitModel(db);
                        stockUnitModel.TakeQtyFromPackage(sourceStockUnit, qty);
                        stockUnitModel.AddQtyToPackage(destStockUnit, qty);
                        jsonModel.Data = destStockUnit.CurrentQtyinPackage;
                        movementManager.CreateMovementLog(destStockUnit, destStockUnit, User.Identity.GetUserId(), qty, destStockUnit.UnitOfMeasure);
                        movementManager.Save();

                        new iLogisPrintLabelManager(db, User.Identity.Name, true).PrintLabelForStockUnit(destStockUnit);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                }
            }
            else
            {
                jsonModel.MessageType = JsonMessageType.danger;
                jsonModel.Message = "W opakowaniu znajduje się inny kod artykułu";
            }

            return Json(jsonModel);
        }

        [HttpPost, Authorize]
        public JsonResult MoveToLocation(
            int stockUnitId, string serialNumber, decimal qtyToMove, UnitOfMeasure unitOfMeasure,
            int? destinationlocationId = null, int? destinationWarehouseId = null, bool force = false, bool discartSerialNumber = false, 
            LabelExtraData labelExtraData = null)
        {
            serialNumber = serialNumber == "0" ? "-undefined-" : serialNumber;
            StockUnit stockUnit = uow.StockUnitRepo.GetById(stockUnitId);
            //iLogisStatus status = iLogisStatus.NoError;
            JsonModel jsonModel = new JsonModel();
            jsonModel.Status = (int)iLogisStatus.NoError;
            decimal qtyMoved = 0;

            if (stockUnit == null)
            {
                stockUnit = uow.StockUnitRepo.GetBySerialNumber(serialNumber);
            }

            if (stockUnit != null)
            {
                StockUnit stockUnitSource = new StockUnit();
                ReflectionHelper.CopyProperties(stockUnit, stockUnitSource);

                if (unitOfMeasure == UnitOfMeasure.Undefined)
                {
                    unitOfMeasure = stockUnit.UnitOfMeasure;

                    if (unitOfMeasure == UnitOfMeasure.Undefined)
                    {
                        unitOfMeasure = stockUnit.ItemWMS.Item.UnitOfMeasure;
                        stockUnit.UnitOfMeasure = unitOfMeasure;
                    }
                    if (unitOfMeasure == UnitOfMeasure.Undefined)
                    {
                        return Json(new JsonModel() { Status = (int)iLogisStatus.StockUnitUoMIncorrect });
                    }
                }

                if (unitOfMeasure != stockUnit.UnitOfMeasure)
                {
                    qtyToMove = ConverterUoM.Convert(qtyToMove, unitOfMeasure, stockUnit.UnitOfMeasure, stockUnit.ItemWMS.Item.UnitOfMeasures.ToList());
                }

                if (qtyToMove <= 0) jsonModel.Status = (int)iLogisStatus.StockUnitUoMConversionProblem;
                if (jsonModel.Status > (int)iLogisStatus.NoError) return Json(jsonModel);

                if (qtyToMove > stockUnit.CurrentQtyinPackage && stockUnit.WarehouseLocation.Type.TypeEnum != WarehouseLocationTypeEnum.Virtual)
                {
                    //błąd ilości!
                    jsonModel.Status = (int)iLogisStatus.StockUnitQtyInPackageIsLess;
                    qtyMoved = 0;
                }
                else if (qtyToMove <= 0)
                {
                    jsonModel.Status = (int)iLogisStatus.StockUnitPutQtyGreeaterThanZero;
                }
                else if (stockUnit.CurrentQtyinPackage - qtyToMove != 0 || discartSerialNumber || stockUnit.SerialNumber == "0") //Nie pobrał całego opakowania
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            StockUnitModel stockUnitModel = new StockUnitModel(db);
                            stockUnit = stockUnitModel.TakeQtyFromPackage(stockUnit, qtyToMove);

                            Warehouse warehouse = uow.WarehouseRepo.GetById(destinationWarehouseId ?? 0);
                            StockUnit stockUnitNew;

                            if (discartSerialNumber)
                            {
                                if(warehouse == null)
                                {
                                    WarehouseLocation whl = uow.WarehouseLocationRepo.GetById(destinationlocationId ?? 0);
                                    warehouse = whl?.Warehouse;
                                }
                                stockUnitNew = uow.StockUnitRepo.GetFromWarehouse_CreateIfNotExists(stockUnit.ItemWMS, warehouse);
                            }
                            else
                            {
                                stockUnitNew = stockUnitModel.CreateStockUnitFromExisting(stockUnit, warehouse, qtyToMove);
                            }

                            jsonModel.Status = (int)whlModel.LocateStockUnit(stockUnitNew, warehouseId: destinationWarehouseId ?? 0, locationId: destinationlocationId ?? 0, force: force);
                            if (jsonModel.Status == (int)iLogisStatus.NoError)
                            {
                                movementManager.CreateMovementLog(stockUnitSource, stockUnitNew, User.Identity.GetUserId(), qtyToMove, stockUnit.UnitOfMeasure);
                                movementManager.Save();
                                whlModel.DeleteIfWarehouseIsOutOfScope(stockUnit);
                                transaction.Commit();
                                qtyMoved = qtyToMove;
                            }
                            else
                            {
                                transaction.Rollback(); 
                                qtyMoved = 0;
                            }
                            
                            if(qtyMoved > 0)
                                jsonModel.Status = (int)PrintLabels(stockUnit, stockUnitNew, (iLogisStatus)jsonModel.Status, true, labelExtraData);
                            
                        }
                        catch
                        {
                            transaction.Rollback();
                            jsonModel.Status = (int)iLogisStatus.GeneralError;
                            qtyMoved = 0;
                        }
                    }
                }
                else
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        //nowa etykieta
                        try
                        {
                            jsonModel.Status = (int)whlModel.LocateStockUnit(stockUnit, warehouseId: destinationWarehouseId ?? 0, locationId: destinationlocationId ?? 0, force: force);
                            if (jsonModel.Status == (int)iLogisStatus.NoError)
                            {
                                movementManager.CreateMovementLog(stockUnitSource, stockUnit, User.Identity.GetUserId(), qtyToMove, stockUnit.UnitOfMeasure);
                                movementManager.Save();
                                whlModel.DeleteIfWarehouseIsOutOfScope(stockUnit);
                                transaction.Commit();
                                jsonModel.Status = (int)PrintLabel(stockUnit, (iLogisStatus)jsonModel.Status);
                                qtyMoved = qtyToMove;
                            }
                            else
                            {
                                transaction.Rollback();
                                qtyMoved = 0;
                            }
                        }
                        catch
                        {
                            transaction.Rollback();
                            qtyMoved = 0;
                        }
                    }
                }
            }
            else
            {
                jsonModel.Status = (int)iLogisStatus.StockUnitNotFound;
            }

            //  DO UZUPEŁNIENIA
            //ItemWMS item = uow.ItemWMSRepo.GetById(itemId);
            //WarehouseLocation wl = new BufferLocationModel(uow).FindEmptyLocationForItem(item);
            //jsonModel.Status = (int)status;
            jsonModel.Data = qtyMoved;
            return Json(jsonModel);
        }

        [HttpPost, Authorize]
        public JsonResult Move(
            int stockUnitId, string serialNumber, decimal qtyToMove, UnitOfMeasure unitOfMeasure, EnumMovementType type,
            int? destinationlocationId = null, int? destinationWarehouseId = null, string freeText1 = null, 
            bool force = false, bool discartSerialNumber = false, bool print = true)
        {
            serialNumber = serialNumber == "0" ? "-undefined-" : serialNumber;
            StockUnit stockUnit = uow.StockUnitRepo.GetById(stockUnitId);
            iLogisStatus status = iLogisStatus.NoError;
            decimal qtyMoved = 0;
            if (stockUnit == null)
            {
                stockUnit = uow.StockUnitRepo.GetBySerialNumber(serialNumber);
            }

            if (stockUnit != null)
            {
                StockUnit stockUnitSource = new StockUnit();
                ReflectionHelper.CopyProperties(stockUnit, stockUnitSource);

                if(unitOfMeasure == UnitOfMeasure.Undefined)
                {
                    unitOfMeasure = stockUnit.UnitOfMeasure;

                    if(unitOfMeasure == UnitOfMeasure.Undefined)
                    {
                        unitOfMeasure = stockUnit.ItemWMS.Item.UnitOfMeasure;
                        stockUnit.UnitOfMeasure = unitOfMeasure;
                    }
                    if(unitOfMeasure == UnitOfMeasure.Undefined)
                    {
                        status = iLogisStatus.StockUnitUoMIncorrect;
                        return Json(new JsonModel() { Status = (int)status });
                    }
                }

                if (unitOfMeasure != stockUnit.UnitOfMeasure)
                {
                    qtyToMove = ConverterUoM.Convert(qtyToMove, unitOfMeasure, stockUnit.UnitOfMeasure, stockUnit.ItemWMS.Item.UnitOfMeasures.ToList());
                }

                if (qtyToMove <= 0) status = iLogisStatus.StockUnitUoMConversionProblem;
                if (status > (int)iLogisStatus.NoError) return Json(new JsonModel() { Status = (int)status });

                if (qtyToMove > stockUnit.CurrentQtyinPackage && stockUnit.WarehouseLocation.Type.TypeEnum != WarehouseLocationTypeEnum.Virtual)
                {
                    //błąd ilości!
                    status = iLogisStatus.StockUnitQtyInPackageIsLess;
                }
                else if (qtyToMove <= 0)
                {
                    status = iLogisStatus.StockUnitPutQtyGreeaterThanZero;
                }
                else if (stockUnit.CurrentQtyinPackage - qtyToMove != 0 || discartSerialNumber || stockUnit.SerialNumber == "0") //Nie pobrał całego opakowania
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            StockUnitModel stockUnitModel = new StockUnitModel(db);
                            stockUnit = stockUnitModel.TakeQtyFromPackage(stockUnit, qtyToMove);

                            Warehouse warehouse = uow.WarehouseRepo.GetById(destinationWarehouseId ?? 0);

                            StockUnit stockUnitNew;

                            if (discartSerialNumber)
                            {
                                if (warehouse == null)
                                {
                                    WarehouseLocation whl = uow.WarehouseLocationRepo.GetById(destinationlocationId ?? 0);
                                    warehouse = whl?.Warehouse;
                                }
                                stockUnitNew = uow.StockUnitRepo.GetFromWarehouse_CreateIfNotExists(stockUnit.ItemWMS, warehouse);
                                stockUnitNew.CurrentQtyinPackage += qtyToMove;
                                uow.StockUnitRepo.AddOrUpdate(stockUnitNew);
                            }
                            else
                            {
                                stockUnitNew = stockUnitModel.CreateStockUnitFromExisting(stockUnit, warehouse, qtyToMove);
                            }

                            //StockUnit stockUnitNew = stockUnitModel.CreateStockUnitFromExisting(stockUnit, warehouse, qtyToMove);

                            status = whlModel.LocateStockUnit(stockUnitNew, warehouseId: destinationWarehouseId ?? 0, locationId: destinationlocationId ?? 0, force: force);
                            if (status == iLogisStatus.NoError)
                            {
                                if (destinationlocationId != null)
                                {
                                    WarehouseLocation destinationLocation = uow.WarehouseLocationRepo.GetById((int)destinationlocationId);
                                    movementManager.SetFreeText(freeText1, null);
                                    movementManager.CreateMovementLogByType(stockUnitSource, destinationLocation, User.Identity.GetUserId(), qtyToMove, stockUnitSource.UnitOfMeasure, type);
                                }
                                else
                                {
                                    movementManager.SetFreeText(freeText1, null);
                                    movementManager.CreateMovementLogByType(stockUnitSource, User.Identity.GetUserId(), qtyToMove, stockUnitSource.UnitOfMeasure, type);
                                }
                                movementManager.Save();
                                whlModel.DeleteIfWarehouseIsOutOfScope(stockUnit);
                                transaction.Commit();
                                qtyMoved = qtyToMove;
                                status = PrintLabels(stockUnit, stockUnitNew, status, print);
                            }
                            else
                            {
                                qtyMoved = qtyToMove;
                                transaction.Rollback();
                            }

                        }
                        catch
                        {
                            transaction.Rollback();
                            status = iLogisStatus.GeneralError;
                            qtyMoved = qtyToMove;
                        }
                    }
                }
                else
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        //nowa etykieta
                        try
                        {
                            status = whlModel.LocateStockUnit(stockUnit, warehouseId: destinationWarehouseId ?? 0, locationId: destinationlocationId ?? 0, force: force);
                            string userId = User.Identity.GetUserId();
                            if (status == iLogisStatus.NoError)
                            {
                                if (destinationlocationId != null)
                                {
                                    WarehouseLocation destinationLocation = uow.WarehouseLocationRepo.GetById((int)destinationlocationId);
                                    movementManager.SetFreeText(freeText1, null);
                                    movementManager.CreateMovementLogByType(stockUnitSource, destinationLocation, userId, qtyToMove, stockUnitSource.UnitOfMeasure, type);
                                }
                                else
                                {
                                    movementManager.SetFreeText(freeText1, null);
                                    movementManager.CreateMovementLogByType(stockUnitSource, userId, qtyToMove, stockUnitSource.UnitOfMeasure, type);
                                }
                                movementManager.Save();
                                whlModel.DeleteIfWarehouseIsOutOfScope(stockUnit);
                                transaction.Commit();
                                qtyMoved = qtyToMove;
                                status = PrintLabel(stockUnit, status, print);
                            }
                            else
                            {
                                qtyMoved = 0;
                                transaction.Rollback();
                            }

                        }
                        catch(Exception e)
                        {
                            qtyMoved = 0;
                            transaction.Rollback();
                        }
                    }
                }
            }
            else
            {
                status = iLogisStatus.StockUnitNotFound;
            }

            //  DO UZUPEŁNIENIA
            //ItemWMS item = uow.ItemWMSRepo.GetById(itemId);
            //WarehouseLocation wl = new BufferLocationModel(uow).FindEmptyLocationForItem(item);
            JsonModel jsonModel = new JsonModel();
            jsonModel.Status = (int)status;
            jsonModel.Data = qtyMoved;
            return Json(jsonModel);
        }

        [HttpPost, Authorize]
        public JsonResult MoveManual(
            string itemCode, decimal qtyToMove, UnitOfMeasure unitOfMeasure, EnumMovementType type,
            string sourceWarehouseCode = "", string destinationWarehouseCode ="", string freeText1 = null)
        {
            //iLogisStatus status = iLogisStatus.NoError;
            JsonModel jsonModel = new JsonModel();
            jsonModel.Status = (int)iLogisStatus.NoError;

            decimal qtyMoved = 0;
            ItemWMS itemWMS = uow.ItemWMSRepo.Get(null, null, itemCode);
            Warehouse sourceWh = uow.WarehouseRepo.GetByCode(sourceWarehouseCode);
            Warehouse destWh = uow.WarehouseRepo.GetByCode(destinationWarehouseCode);

            if (itemWMS == null) jsonModel.Status = (int)iLogisStatus.ItemWMSNotFound;
            if (sourceWh == null) jsonModel.Status = (int)iLogisStatus.WarehouseSourceNotFoud;
            if (destWh == null) jsonModel.Status = (int)iLogisStatus.WarehouseDestNotFoud;
            if (qtyToMove <= 0) jsonModel.Status = (int)iLogisStatus.StockUnitPutQtyGreeaterThanZero;
            if (type != EnumMovementType.CODE_311) jsonModel.Status = (int)iLogisStatus.GeneralError; //TODO: obsłużyć inne ruchy
            if (isUnitOfMeasureCorrect(unitOfMeasure, itemWMS)) { jsonModel.Status = (int)iLogisStatus.StockUnitUoMIncorrect; }
            if (jsonModel.Status > (int)iLogisStatus.NoError) return Json(jsonModel);

            WarehouseLocation sourceWhl = uow.WarehouseLocationRepo.GetVirtualForWarehouse(sourceWh.Id);
            WarehouseLocation destWhl = uow.WarehouseLocationRepo.GetVirtualForWarehouse(destWh.Id);

            if (sourceWhl == null) jsonModel.Status = (int)iLogisStatus.LocationNotFound;
            if (destWhl == null) jsonModel.Status = (int)iLogisStatus.LocationNotFound;
            if (jsonModel.Status > (int)iLogisStatus.NoError) return Json(jsonModel);

            //StockUnitModel stockUnitModel = new StockUnitModel(db);
            StockUnit sourceStockUnit = uow.StockUnitRepo.GetFromWarehouseLoc_CreateIfNotExists(itemWMS, sourceWhl);
            StockUnit destStockUnit = uow.StockUnitRepo.GetFromWarehouseLoc_CreateIfNotExists(itemWMS, destWhl);

            if (unitOfMeasure != sourceStockUnit.UnitOfMeasure)
            {
                qtyToMove = ConverterUoM.Convert(qtyToMove, unitOfMeasure, sourceStockUnit.UnitOfMeasure, sourceStockUnit.ItemWMS.Item.UnitOfMeasures.ToList());
            }

            if (qtyToMove <= 0) jsonModel.Status = (int)iLogisStatus.StockUnitUoMConversionProblem;
            if (jsonModel.Status > (int)iLogisStatus.NoError) return Json(jsonModel);

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    sourceStockUnit.CurrentQtyinPackage -= qtyToMove;
                    destStockUnit.CurrentQtyinPackage += qtyToMove;
                    uow.StockUnitRepo.AddOrUpdate(sourceStockUnit);
                    uow.StockUnitRepo.AddOrUpdate(destStockUnit);
                    movementManager.SetFreeText(freeText1, null);
                    movementManager.CreateMovementLog(sourceStockUnit, destStockUnit, User.Identity.GetUserId(), qtyToMove, sourceStockUnit.UnitOfMeasure);
                    movementManager.Save();
                    whlModel.DeleteIfWarehouseIsOutOfScope(destStockUnit);
                    transaction.Commit();
                    qtyMoved = qtyToMove;
                }
                catch
                {
                    jsonModel.Status = (int)iLogisStatus.GeneralError;
                    transaction.Rollback();
                    qtyMoved = 0;
                }
            }

            jsonModel.Data = qtyMoved;
            return Json(jsonModel);
        }

        private static bool isUnitOfMeasureCorrect(UnitOfMeasure unitOfMeasure, ItemWMS itemWMS)
        {
            return itemWMS.Item.UnitOfMeasure != unitOfMeasure && !(itemWMS.Item.UnitOfMeasures.Select(x => x.AlternativeUnitOfMeasure).Contains(unitOfMeasure));
        }

        [HttpPost, Authorize]
        public JsonResult MoveManualToLocation(
            string itemCode, decimal qtyToMove, UnitOfMeasure unitOfMeasure, EnumMovementType type,
            string sourceWarehouseCode = "", string destinationLocationName = "", string freeText1 = null)
        {
            iLogisStatus status = iLogisStatus.NoError;
            decimal qtyMoved = 0;
            ItemWMS itemWMS = uow.ItemWMSRepo.Get(null, null, itemCode);
            Warehouse sourceWh = uow.WarehouseRepo.GetByCode(sourceWarehouseCode);
            WarehouseLocation destWhLocation = uow.WarehouseLocationRepo.GetByName(destinationLocationName);

            if (itemWMS == null) return Json(iLogisStatus.ItemWMSNotFound);
            if (sourceWh == null) return Json(iLogisStatus.WarehouseSourceNotFoud);
            if (qtyToMove <= 0) return Json(iLogisStatus.StockUnitPutQtyGreeaterThanZero);
            if (type != EnumMovementType.CODE_311) return Json(iLogisStatus.GeneralError); //TODO: obsłużyć inne ruchy

            WarehouseLocation sourceWhl = uow.WarehouseLocationRepo.GetVirtualForWarehouse(sourceWh.Id);
            WarehouseLocation destWhl = destWhLocation;

            if (sourceWhl == null) return Json(iLogisStatus.LocationNotFound);
            if (destWhl == null) return Json(iLogisStatus.LocationNotFound);

            StockUnitModel stockUnitModel = new StockUnitModel(db);
            StockUnit sourceStockUnit = uow.StockUnitRepo.GetFromWarehouseLoc_CreateIfNotExists(itemWMS, sourceWhl);
            StockUnit destStockUnit = uow.StockUnitRepo.GetFromWarehouseLoc_CreateIfNotExists(itemWMS, destWhl);

            if (unitOfMeasure != sourceStockUnit.UnitOfMeasure)
            {
                qtyToMove = ConverterUoM.Convert(qtyToMove, unitOfMeasure, sourceStockUnit.UnitOfMeasure, sourceStockUnit.ItemWMS.Item.UnitOfMeasures.ToList());
            }

            if (qtyToMove <= 0) status = iLogisStatus.StockUnitUoMConversionProblem;
            if (status > (int)iLogisStatus.NoError) return Json(new JsonModel() { Status = (int)status });

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    sourceStockUnit.CurrentQtyinPackage -= qtyToMove;
                    destStockUnit.CurrentQtyinPackage += qtyToMove;
                    uow.StockUnitRepo.AddOrUpdate(sourceStockUnit);
                    uow.StockUnitRepo.AddOrUpdate(destStockUnit);
                    movementManager.SetFreeText(freeText1, null);
                    movementManager.CreateMovementLog(sourceStockUnit, destStockUnit, User.Identity.GetUserId(), qtyToMove, sourceStockUnit.UnitOfMeasure);
                    movementManager.Save();
                    whlModel.DeleteIfWarehouseIsOutOfScope(destStockUnit);
                    transaction.Commit();
                    PrintLabels(sourceStockUnit, destStockUnit, status);
                    qtyMoved = qtyToMove;
                }
                catch
                {
                    status = iLogisStatus.GeneralError;
                    transaction.Rollback();
                    qtyMoved = 0;
                }
            }

            JsonModel jsonModel = new JsonModel();
            jsonModel.Status = (int)status;
            jsonModel.Data = qtyMoved;
            return Json(jsonModel);
        }

        [HttpPost, Authorize]
        public JsonResult ConfirmMovement(int stockUnitId, string destinationlocationName)
        {
            StockUnit stockUnit = uow.StockUnitRepo.GetById(stockUnitId);
            iLogisStatus status = iLogisStatus.NoError;

            Regex rgx = new Regex("[^a-zA-Z0-9]");
            destinationlocationName = rgx.Replace(destinationlocationName, "");

            if (stockUnit.WarehouseLocation != null && stockUnit.WarehouseLocation.Name.Contains(destinationlocationName))
            {
                stockUnit.IsLocated = true;
                uow.StockUnitRepo.AddOrUpdate(stockUnit);
                return Json(status);
            }
            else
            {
                status = iLogisStatus.LocationNotTheSame;
                return Json(status);
            }
        }

        //------------------------------------------------------------------------------------------
        //-----------------------------WYDRUKI------------------------------------------------------

        [HttpPost, Authorize]
        public JsonResult ReprintLabel(int stockUnitId, string serialNumber)
        {
            StockUnit stockUnit = uow.StockUnitRepo.Get(stockUnitId, serialNumber);

            if (stockUnit.IsLocated == false || 1==1)
            {
                new iLogisPrintLabelManager(db, User.Identity.Name, true).PrintLabelForStockUnit(stockUnit);
                stockUnit.IsLocated = true;
                uow.StockUnitRepo.AddOrUpdate(stockUnit);
                return Json(iLogisStatus.LabelPrinted);
            }
            return Json(iLogisStatus.LabelCantBePrinted);
        }

        private iLogisStatus PrintLabels(StockUnit stockUnit, StockUnit stockUnitNew, iLogisStatus status, bool print = true, LabelExtraData extraData = null)
        {
            try
            {
                bool printLabelForOldPackage = !stockUnit.Deleted;
                bool printLabelWithDetails = !stockUnit.Deleted;
                iLogisPrintLabelManager printLabelManager = new iLogisPrintLabelManager(db, User.Identity.Name, print);
                printLabelManager.PrintSmallLabelForStockUnit(stockUnit, false, User.Identity.GetUserId(), printLabelForOldPackage);

                if (stockUnitNew.WarehouseLocation != null && 
                    (stockUnitNew.WarehouseLocation.Type.TypeEnum == WarehouseLocationTypeEnum.Shelf ||
                    stockUnitNew.WarehouseLocation.Type.TypeEnum == WarehouseLocationTypeEnum.OnFloor)
                    )
                {
                    printLabelManager.PrintLabelForStockUnit(stockUnitNew);
                }
                else
                {
                    printLabelManager.PrintSmallLabelForStockUnit(stockUnitNew, printLabelWithDetails, User.Identity.GetUserId(), true, extraData);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Printing failed", ex.Message);
                status = iLogisStatus.MovementDoneButLabelsWereNotPrinted;
            }

            return status;
        }
        private iLogisStatus PrintLabel(StockUnit stockUnit, iLogisStatus status, bool print = true)
        {
            var pm = new iLogisPrintLabelManager(db, User.Identity.Name, print);

            try
            {
                if (stockUnit.WarehouseLocation.Type.TypeEnum == WarehouseLocationTypeEnum.Shelf ||
                    stockUnit.WarehouseLocation.Type.TypeEnum == WarehouseLocationTypeEnum.OnFloor
                    )
                {
                    pm.PrintLabelForStockUnit(stockUnit);
                }
                else
                {
                    pm.PrintSmallLabelForStockUnit(stockUnit, true, User.Identity.GetUserId(), true);
                }
            }
            catch
            {
                Debug.WriteLine("Printing failed");
                status = iLogisStatus.MovementDoneButLabelsWereNotPrinted;
            }

            return status;
        }

        //------------------------------------------------------------------------------------------
        //-------------------------IMPORT DANYCH----------------------------------------------------
        [HttpGet]
        public ActionResult ImportFromExcelView()
        {
            return View();
        }
        [HttpGet, AllowAnonymous]
        public JsonResult ImportFromExcel(
            string dLocationName, string itemCode, decimal qty, 
            int numberOfPackages, int maxQtyPerPackage, int maxPackagePerPallet, EnumMovementType type)
        {
            int virtualLocation_9103 = 3217;
            Regex rgx = new Regex("[^a-zA-Z0-9]");
            dLocationName = rgx.Replace(dLocationName, "");


            StockUnitModel stockUnitModel = new StockUnitModel(db);
            WarehouseLocationModel wlm = new WarehouseLocationModel(uow);
            StockUnit stockUnit = uow.StockUnitRepo.GetList()
                .Where(x => x.ItemWMS.Item.Code == itemCode && x.SerialNumber == "0" && x.WarehouseLocation.Id == virtualLocation_9103)
                .FirstOrDefault();

            WarehouseLocation wl = uow.WarehouseLocationRepo.GetByName(dLocationName);
            Warehouse wh = uow.WarehouseRepo.GetList().FirstOrDefault(x => x.Code == "9103");
            //Warehouse warehouse = uow.WarehouseRepo.GetById(wl.WarehouseId);

            iLogisStatus status = iLogisStatus.NoError;

            if(wl == null)
            {
                return Json(iLogisStatus.LocationNotFound, JsonRequestBehavior.AllowGet);
            }

            if (stockUnit != null)
            {
                //StockUnit stockUnitSource = new StockUnit();
                //ReflectionHelper.CopyProperties(stockUnit, stockUnitSource);
                PackageItem pi = uow.PackageItemRepo.Get(stockUnit.ItemWMS, qtyPerPackage: maxQtyPerPackage, warehouseId: 0);

                if(pi == null)
                {
                    pi = new PackageItem()
                    {
                        ItemWMS = stockUnit.ItemWMS,
                        ItemWMSId = stockUnit.ItemWMSId,
                        PackageId = 1,
                        PackagesPerPallet = maxPackagePerPallet,
                        QtyPerPackage = maxQtyPerPackage,
                        PickingStrategy = PickingStrategyEnum.FullPackage,
                        WarehouseId = wh.Id,
                        WarehouseLocationTypeId = 1,
                    };

                    uow.PackageItemRepo.Add(pi);
                }

                for (int i = 0; i< numberOfPackages; i++)
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            stockUnit = stockUnitModel.TakeQtyFromPackage(stockUnit, qty);
                            StockUnit stockUnitNew = stockUnitModel.CreateStockUnit_OnVirtual(stockUnit.ItemWMS, wl.Warehouse, pi, qty, maxQtyPerPackage, null);
                            stockUnitModel.Save();
                            decimal requiredUtylization = (pi != null && pi.PackagesPerPallet > 0) ? -1 : (1m / maxPackagePerPallet);
                            status = wlm.SetLocationOfStockUnit(stockUnitNew, wl, pi, requiredUtylization);

                            if(status == iLogisStatus.NoError)
                                transaction.Commit();
                            else
                                transaction.Rollback();
                        }
                        catch(Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                            transaction.Rollback();
                            return Json(iLogisStatus.GeneralError);
                        }
                    }
                }
            }
            else
            {
                status = iLogisStatus.StockUnitNotFound;
            }

            //  DO UZUPEŁNIENIA
            //ItemWMS item = uow.ItemWMSRepo.GetById(itemId);
            //WarehouseLocation wl = new BufferLocationModel(uow).FindEmptyLocationForItem(item);
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        //-------------------------------------------------------------------------------------------
        //-------------------------PRZEGLADANIE ZAPASOW----------------------------------------------
        [HttpGet]
        public ActionResult Stock()
        {

            List<WarehouseLocationType> wlt = uow.WarehouseLocationTypeRepo.GetList().ToList();
            wlt.Add(new WarehouseLocationType() { Id = 0, Name = " " });
            ViewBag.LocationTypes = wlt;
            return View();
        }
        [HttpGet]
        public ActionResult StockMobile()
        {
            ViewBag.BarcodeTemplate = uow.SystemVariableRepo.GetValueString("BarcodeTemplate_WH");
            return View();
        }
        [HttpPost]
        public JsonResult StockGetList(string warehouseName, string warehouseLocationName, string itemCode, string serialNo, int? locationTypeId = 0)
        {
            List<StockUnitViewModel> stockUnits = new List<StockUnitViewModel>();
            StockUnitViewModel bw = new StockUnitViewModel();
            //browseList = GetBrowseWarehouseList(warehouseName, warehouseLocationName, itemCode);

            Regex rgx = new Regex("[^a-zA-Z0-9]");
            warehouseLocationName = rgx.Replace(warehouseLocationName, "");

            bool showQtyDetails = (itemCode != null && itemCode.Length > 0) || (serialNo != null && serialNo.Length > 0);

            stockUnits = uow.StockUnitRepo.GetList(warehouseName, warehouseLocationName, itemCode, serialNo, locationTypeId)
                .GroupBy(x => new { x.WarehouseLocation.Warehouse, x.UnitOfMeasure })
                .Select(y => new StockUnitViewModel()
                {
                    ItemCode = itemCode,
                    CurrentQtyinPackage = showQtyDetails ? y.Sum(x => x.CurrentQtyinPackage) : 0,
                    //ReservedQty = showQtyDetails ? y.Sum(x => x.ReservedQty) : 0,
                    UnitOfMeasure = y.Key.UnitOfMeasure,
                    SerialNumber = null,
                    WarehouseLocationUtilization = -1,
                    WarehouseLocationName = null,
                    WarehouseLocationTypeName = null,
                    WarehouseCode = y.Key.Warehouse.Code,
                    WarehouseName = y.Key.Warehouse.Name,
                    WarehouseIsMRP = y.Key.Warehouse.isMRP,
                    WarehouseIsOutOfScope = y.Key.Warehouse.isOutOfScore,
                    AccountingWarehouseCode = y.Key.Warehouse.AccountingWarehouse != null? y.Key.Warehouse.AccountingWarehouse.Code : y.Key.Warehouse.Code,
                    AccountingWarehouseName = y.Key.Warehouse.AccountingWarehouse != null? y.Key.Warehouse.AccountingWarehouse.Name : y.Key.Warehouse.Name
                })
                .OrderBy(x => x.WarehouseCode)
                .ToList();

            return Json(stockUnits);
        }
        [HttpPost]
        public JsonResult StockWarehouseDetailsGetList(string warehouseName, string warehouseLocationName, string itemCode, string serialNo, int? locationTypeId = 0, int pageIndex = 1, int pageSize = 1000)
        {
            List<StockUnitViewModel> stockUnits = new List<StockUnitViewModel>();
            StockUnitViewModel bw = new StockUnitViewModel();
            //browseList = GetBrowseWarehouseList(warehouseName, warehouseLocationName, itemCode);
            //bool showQtyDetails = (itemCode != null && itemCode.Length > 0) || (serialNo != null && serialNo.Length > 0);
            IQueryable<StockUnitViewModel> query = uow.StockUnitRepo.GetList(warehouseName, warehouseLocationName, itemCode, serialNo, locationTypeId)
                .GroupBy(x => new { x.WarehouseLocation, x.ItemWMS, x.UnitOfMeasure })
                .Select(y => new StockUnitViewModel()
                {
                    ItemCode = y.Key.ItemWMS.Item.Code,
                    ItemName = y.Key.ItemWMS.Item.Name,
                    ItemUoM = y.Key.ItemWMS.Item.UnitOfMeasure,
                    UnitOfMeasure = y.Key.UnitOfMeasure,
                    CurrentQtyinPackage = y.Sum(x => x.CurrentQtyinPackage),
                    ReservedQty = y.Sum(x => x.ReservedQty),
                    SerialNumber = null,
                    WarehouseLocationUtilization = -1,
                    WarehouseLocationName = y.Key.WarehouseLocation.Name,
                    WarehouseLocationTypeName = y.Key.WarehouseLocation.Type.Name,
                    WarehouseName = y.Key.WarehouseLocation.Warehouse.Name,
                    WarehouseCode = y.Key.WarehouseLocation.Warehouse.Code,
                    AccountingWarehouseCode = y.Key.WarehouseLocation.Warehouse.AccountingWarehouse != null? y.Key.WarehouseLocation.Warehouse.AccountingWarehouse.Code : y.Key.WarehouseLocation.Warehouse.Code,
                })
                .OrderByDescending(x => x.WarehouseName);

            int startIndex = (pageIndex - 1) * pageSize;
            int itemsCount = query.Count();

            stockUnits = query.Skip(startIndex).Take(pageSize).ToList();

            return Json(new { data = stockUnits, itemsCount });
        }
        [HttpPost]
        public JsonResult StockLocationDetailsGetList(string warehouseCode, string warehouseLocationName, string itemCode, string serialNo, int? locationTypeId = 0, int pageIndex = 1, int pageSize = 1000)
        {
            IQueryable<StockUnit> query = uow.StockUnitRepo.GetList(warehouseCode, warehouseLocationName, itemCode, serialNo, locationTypeId);
            int startIndex = (pageIndex - 1) * pageSize;
            int itemsCount = query.Count();

            List<StockUnitViewModel> stockUnits = new List<StockUnitViewModel>();
            StockUnitViewModel bw = new StockUnitViewModel();

            stockUnits = query.Skip(startIndex).Take(pageSize).Select(x => new StockUnitViewModel()
            {
                Id = x.Id,
                ItemCode = x.ItemWMS.Item.Code,
                ItemUoM = x.ItemWMS.Item.UnitOfMeasure,
                CurrentQtyinPackage = x.CurrentQtyinPackage,
                ReservedQty = x.ReservedQty,
                UnitOfMeasure = x.UnitOfMeasure,
                Status = x.Status,
                SerialNumber = x.SerialNumber,
                WarehouseLocationUtilization = x.WarehouseLocation != null ? x.WarehouseLocation.Utilization : 0m,
                WarehouseLocationName = x.WarehouseLocation != null ? x.WarehouseLocation.Name : "",
                WarehouseLocationTypeName = x.WarehouseLocation != null && x.WarehouseLocation.Type != null ? x.WarehouseLocation.Type.Name : "",
                WarehouseName = x.WarehouseLocation != null && x.WarehouseLocation.Warehouse != null ? x.WarehouseLocation.Warehouse.Name : "",
                WarehouseCode = x.WarehouseLocation != null && x.WarehouseLocation.Warehouse != null ? x.WarehouseLocation.Warehouse.Code : ""
            }).ToList();

            return Json(new { data = stockUnits, itemsCount });
        }

    }
}