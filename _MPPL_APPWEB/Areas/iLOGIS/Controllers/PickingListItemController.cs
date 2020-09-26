using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_iLOGIS.ComponentWMS.Enums;
using MDL_iLOGIS.ComponentWMS.Models;
using MDLX_CORE.ComponentCore.Entities;
using MDL_WMS.ComponentConfig.UnitOfWorks;
using MDLX_MASTERDATA.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using MDL_iLOGIS.ComponentWMS.Repos;
using _MPPL_WEB_START.Areas.iLOGIS.ViewModels.PickingList;
using MDL_iLOGIS.ComponentWMS._Interfaces;
using MDL_ONEPROD.Model.Scheduling;
using MDL_iLOGIS.ComponentWMS.ViewModels;
using MDL_BASE.Models.MasterData;
using WebGrease.Css.Ast.Selectors;
using _MPPL_WEB_START.Areas._APPWEB.ViewModels;
using MDL_iLOGIS.ComponentCore.Enums;
using MDLX_CORE.Model;

namespace _MPPL_WEB_START.Areas.iLOGIS.Controllers
{
    [Authorize]
    public class PickingListItemController : Controller
    {
        IDbContextiLOGIS db;
        ILocataionManager locataionManager;
        UnitOfWork_iLogis uow;
        public PickingListItemController(IDbContextiLOGIS db)
        {
            ViewBag.Skin = "nasaSkin";
            this.db = db;
            this.locataionManager = AutofacDependencyResolver.Current.ApplicationContainer.Resolve<ILocataionManager>();
            uow = new UnitOfWork_iLogis(db, locataionManager);
        }

        //------------------VIEW-PICKING-LIST-ITEMS-------------------------------
        [HttpGet]
        public ActionResult PickingListItem(int pickingListId, string pickingListGuid,int workOrderId,int pickerId)
        {
            ViewBag.PickingListId = pickingListId;
            ViewBag.WorkorderId = workOrderId;
            ViewBag.PickerId = pickerId;
            ViewBag.PickingListGuid = pickingListGuid;

            return View();
        }
        [HttpPost]
        public JsonResult GetList(int workOrderId, int pickerId, int parameterH = -2,string pickingListGuid="")
        {
            IQueryable<PickingListItemViewModel_2> pickignListItemVMQ;
            pickignListItemVMQ = uow.PickingListItemRepo.GetByWorkOrderAndPicker(workOrderId, pickerId, pickingListGuid);
            
            
            ProductionOrder wo = uow.ProductionOrderRepo.GetById(workOrderId);
            
            if (pickignListItemVMQ.Count() <= 0) // Rozpatrzanie przypadku, gdy nie znaleziono żadnych Itemów do PickingListy.
            {
                List<PickingList> pickingList = new List<PickingList>();
                if (pickingListGuid != "")
                {
                    pickingList = uow.PickingListRepo.GetByGuid(pickingListGuid);
                }
                else
                {
                    pickingList = uow.PickingListRepo.GetByWorkOrderAndPicker(workOrderId, pickerId);
                }
                                
                if (pickingList.Count() == 0) // Tutaj prawdopodobnie nigdy nie wejdzie.
                {
                    PickingListController.CreateNewPickingList(uow, workOrderId, pickerId);
                    pickignListItemVMQ = uow.PickingListItemRepo.GetByWorkOrderAndPicker(workOrderId, pickerId);
                }

                foreach(var pickList in pickingList)
                {
                    if (pickList.Id > 0 && pickList.Status == EnumPickingListStatus.CreatedEmpty)// Przypadek, gdy w picking liscie jest zero itemow - jest to ponowne stworzenie itemow.
                    {
                        List<PickingListItem> itemListForPicker = PickingListController.GetItemsForPicker(uow, wo, pickerId, pickList);
                        if (itemListForPicker.Count > 0)
                        {
                            PickingListController.AddItemsToPickingList(uow, itemListForPicker);
                            pickignListItemVMQ = uow.PickingListItemRepo.GetByWorkOrderAndPicker(workOrderId, pickerId);
                        }
                    }
                }
                
            }

            List<PickingListItemViewModel_2> pickignListItemVM = new List<PickingListItemViewModel_2>();
            pickignListItemVM = pickignListItemVMQ.ToList().Where(x=> (parameterH < 0 || x.H == parameterH))
                .OrderByDescending(x => x.Status == EnumPickingListItemStatus.Pending)
                .ThenByDescending(x => x.Status == EnumPickingListItemStatus.Problem)
                .ThenByDescending(x=> x.Status == EnumPickingListItemStatus.CompletedWithProblem)
                .ThenByDescending(x => x.Status == EnumPickingListItemStatus.Completed)
                .ToList();

            PickingListViewModel_2 vm = new PickingListViewModel_2();
            if (pickignListItemVM.Count() != 0)
            {
                vm.PickingListItems = pickignListItemVM;
                vm.IsDataNull = false;
                vm.WorkOrderNo = pickignListItemVM[0].WorkOrderNo;
                vm.PncCode = wo.Pnc.Code;
                vm.PncQtyRemaining = wo.QtyRemain;
                vm.ResourceName = wo.Line.Name;
                vm.PickingListStatus = (int)GetPickingListStatusByItems(pickignListItemVM);
                vm.NumberToPick = pickignListItemVMQ.Count(x => x.Status == EnumPickingListItemStatus.Pending);
                vm.PlatformList = uow.PickingListItemRepo.GePlatformsDistinct(workOrderId, pickerId, wo.LineId, parameterH).ToList();
                vm.PickingListGuid  = pickingListGuid;
                if(pickingListGuid != "")
                {
                    
                    List<PickingList> pickingListByGuid = uow.PickingListRepo.GetByGuid(pickingListGuid);
                    List<string> orderNumbers = pickingListByGuid.Select(x => x.WorkOrder.OrderNumber).ToList();
                    vm.ProductionOrderList = uow.ProductionOrderRepo.GetByOrderNumbers(orderNumbers);
                    vm.PncQtyRemaining = vm.ProductionOrderList.Sum(x => x.QtyRemain);
                    foreach (var pickingList in pickingListByGuid)
                    {
                        pickingList.Status = (EnumPickingListStatus)vm.PickingListStatus;
                        uow.PickingListRepo.AddOrUpdate(pickingList);
                    }
                }
                else
                {
                    PickingList pickingList = uow.PickingListRepo.GetById(pickignListItemVM[0].PickingListId);
                    pickingList.Status = (EnumPickingListStatus)vm.PickingListStatus;
                    uow.PickingListRepo.AddOrUpdate(pickingList);
                }
               
            }
            return Json(vm);
        }

        public static EnumPickingListStatus GetPickingListStatusByItems(List<PickingListItemViewModel_2> pickingListItems)
        {
            EnumPickingListStatus status = EnumPickingListStatus.Unassigned;
            int totalItems = pickingListItems.Count();
            int totalItemCompleted = pickingListItems.Where(x => x.Status == EnumPickingListItemStatus.Completed).Count();
            int totalItemsWithError = pickingListItems.Where(x => x.Status == EnumPickingListItemStatus.CompletedWithProblem).Count();
            int totalItemsProblem = pickingListItems.Where(x => x.Status == EnumPickingListItemStatus.Problem).Count();
            int totalItemsPending = pickingListItems.Where(x => x.Status == EnumPickingListItemStatus.Pending).Count();
            int totalItemsFeeding = pickingListItems.Where(x => x.Status == EnumPickingListItemStatus.Feeding).Count();
            int totalItemsClosed = pickingListItems.Where(x => x.Status == EnumPickingListItemStatus.Closed).Count();

            if (pickingListItems != null && pickingListItems.Count > 0 && pickingListItems.LastOrDefault().PickingListId > 0)
            {
                status = EnumPickingListStatus.Created;
            }

            if(totalItems == 0)
            {
                status = EnumPickingListStatus.CreatedEmpty;
            }

            if (totalItemsPending > 0)
            {
                status = EnumPickingListStatus.Pending;
            }

            if (totalItemCompleted > 0)
            {
                status = EnumPickingListStatus.Processing;
            }

            if (((totalItemCompleted + totalItemsClosed) == totalItems) && totalItems > 0)
            {
                status = EnumPickingListStatus.Completed;

                if (totalItemsFeeding > 0)
                {
                    status = EnumPickingListStatus.Feeding;
                }
            }

            if (totalItemsProblem >= 1)
            {
                status = EnumPickingListStatus.Problem;
            }

            if (totalItemsWithError > 0 && (totalItemsPending == 0 || totalItemsProblem + totalItemCompleted == totalItems))
            {
                status = EnumPickingListStatus.CompletedWithProblem;

                if (totalItemsFeeding > 0)
                {
                    status = EnumPickingListStatus.Feeding;
                }
            }   
            
            if(totalItemsFeeding == totalItems && totalItems > 0)
            {
                status = EnumPickingListStatus.Closed;
            }
            return status;
        }

        [HttpPost]
        public JsonResult PickingListPlatformLocationUpdate(int platformId, string platformLocationName)
        {
            WarehouseLocation wL = uow.WarehouseLocationRepo.GetById(platformId);
            if (wL != null)
            {
                WarehouseLocation location = uow.WarehouseLocationRepo.GetByName(platformLocationName);
                if (location == null)
                {
                    Warehouse wh = uow.WarehouseRepo.GetOrCreate("Strefa Przygotowań", "9998");

                    location = new WarehouseLocation()
                    {
                        Name = platformLocationName,
                        WarehouseId = wh.Id
                    };
                    uow.WarehouseLocationRepo.Add(location);
                    wL.ParentWarehouseLocation = location;
                    wL.ParentWarehouseLocationId = location.Id;
                }
                else
                {
                    wL.ParentWarehouseLocation = location;
                    wL.ParentWarehouseLocationId = location.Id;
                }
                uow.WarehouseLocationRepo.AddOrUpdate(wL);
                return Json(platformLocationName);
            }
            return Json(platformLocationName);
        }

        //------------------VIEW-PICKING-LIST-ITEM-MANAGE-------------------------
        [HttpGet]
        public ActionResult Manage()
        {
            return View();
        }
        [HttpPost]
        public JsonResult ManageGetData(int pickingListItemId)
        {
            PickingListItemManageViewModel vm = new PickingListItemManageViewModel();
            int commentListId = 0;
            PickingListItem pickingListItem = uow.PickingListItemRepo.GetById(pickingListItemId);
            var p = pickingListItem.Platform;

            if (p == null)
            {
                p = uow.PickingListItemRepo.GetPlatformIdByWorkOrderIdAndPickerId(pickingListItem.PickingList);
            }
            vm.PickingListItemId = pickingListItemId;
            vm.PickingListItemCode = pickingListItem.ItemWMS.Item.Code;
            vm.PickingListItemName = pickingListItem.ItemWMS.Item.Name;
            vm.PickingListItemLocationName = pickingListItem.WarehouseLocation.NameFormatted;
            vm.PickingListItemPlatformId = p != null? p.Id : 0;
            vm.PickingListItemPlatformName = p != null? p.Name : "";
            vm.MaximumItemNumbersToPackage = pickingListItem.ItemWMS.Weight != 0 ? GetMaximumNumbersItem(pickingListItem.ItemWMS.Weight) : 0;
            vm.QtyPicked = pickingListItem.QtyPicked;
            vm.QtyRequested = pickingListItem.QtyRequested;
            vm.UnitOfMeasure = pickingListItem.UnitOfMeasure;
            vm.PickingListId = pickingListItem.PickingListId;
            vm.PickerId = pickingListItem.PickingList.TransporterId;
            vm.WorkOrderId = pickingListItem.PickingList.WorkOrderId;
            vm.PickingListItemStatus = EnumTransportingStatus.Completed;//pickingListItem.Status;
            vm.StockUnitSerialNumber = pickingListItem.StockUnit != null ? pickingListItem.StockUnit.SerialNumber : "";
            vm.StockUnitCurrentQtyInPackage = pickingListItem.StockUnit != null ? pickingListItem.StockUnit.CurrentQtyinPackage : 0;
            vm.BarcodeTemplate = uow.SystemVariableRepo.GetValueString("BarcodeTemplate_WH");

            IEnumerable<SelectListItem> coomentSelectList = new List<SelectListItem>
            {
                new SelectListItem {Text = "----", Value = 0.ToString()},
                new SelectListItem {Text = "Brak komp.", Value = 1.ToString()},
                new SelectListItem {Text = "Zła lokalizacja", Value = 2.ToString()},
                new SelectListItem {Text = "Materiał na linii", Value = 3.ToString()}
            };

            if (pickingListItem.Comment != null && pickingListItem.Comment != "")
            {
                try
                {
                    commentListId = Int32.Parse(coomentSelectList.Where(x => x.Text == pickingListItem.Comment).FirstOrDefault().Value);
                }
                catch { }
            }
            else
            {
                commentListId = 0;
            }

            vm.CommentList =new SelectList (coomentSelectList, "Value","Text", commentListId);
            vm.CommentListId = commentListId;
            return Json(vm);
        }

        private int GetMaximumNumbersItem(decimal weight)
        {
            decimal maxAvailableWeightToPick = uow.SystemVariableRepo.GetMaxAvailableWeightToPick();
            decimal maxWeightForPackage = uow.SystemVariableRepo.GetMaxWeightForPackage();
            decimal MaxWeight = maxAvailableWeightToPick - maxWeightForPackage;
            return (int)(MaxWeight / weight);
        }

        [HttpPost]
        public JsonResult ManageSave(PickingListItemManageViewModel pickingListItemManage)
        {
            PickingListItem pli = uow.PickingListItemRepo.GetById(pickingListItemManage.PickingListItemId);
            if (pli == null) return Json(EnumPickingListItemStatus.Problem);

            EnumPickingListItemStatus status;
            JsonModel jsonModel = new JsonModel();
            StockUnit stockUnit = new StockUnit();
            decimal qtyPickedNow = pickingListItemManage.QtyPicked;
            bool isItemAlreadyOnLine = IsItemAlreadyOnLine(pickingListItemManage.CommentListId);
            
            pli.Comment = pickingListItemManage.CommentItemString;
            AssignPlatform(pli, pickingListItemManage.PickingListItemPlatformName);

            if (pickingListItemManage.isDifferentSerialNumber)
            {
                stockUnit = ReplaceStockUnit(pli, jsonModel, qtyPickedNow, pickingListItemManage.SerialNumberScanned, pickingListItemManage.StockUnitSerialNumber, isItemAlreadyOnLine);
                if(jsonModel.Status > 0) { return Json(jsonModel); }
            }
            else
            {
                stockUnit = GetStokUnitOfPickingListItem(pli, pickingListItemManage.StockUnitSerialNumber);
            }

            if(qtyPickedNow > stockUnit.CurrentQtyinPackage)
            {
                qtyPickedNow = stockUnit.CurrentQtyinPackage;
            }

            pli.QtyPicked += qtyPickedNow;
            decimal qtyNotFound = 0.00M;
            decimal qtyRemainToPick = pli.QtyRequested - pli.QtyPicked;

            if (qtyRemainToPick > 0)
            {
                if (isItemAlreadyOnLine)
                {
                    uow.StockUnitRepo.UnreserveQtyFromStockUnit(stockUnit, qtyRemainToPick);
                    qtyRemainToPick = 0;
                }
                else
                {
                    SetStockUnitStatusToProblem(pli);
                    uow.StockUnitRepo.UnreserveQtyFromStockUnit(stockUnit, qtyRemainToPick);
                    qtyNotFound = PickingListController.FindLocationsAndSaveToDB(uow, qtyRemainToPick, pli);
                }
            }


            if(pli.WarehouseLocation != null && pli.WarehouseLocation.Name != "BRAK") 
            {
                MoveQtyFromStockUnit(stockUnit, qtyPickedNow, pli.Platform.Id, pli.Platform.WarehouseId, pli.PickingList.WorkOrder);
                uow.StockUnitRepo.UnreserveQtyFromStockUnit(stockUnit, qtyPickedNow);
                status = SetPickingListItemStatus(pli, (int)qtyRemainToPick, (int)qtyNotFound);
                AddTransporterLog(pli, status, pli.PickingList.WorkOrderId, pli.PickingList.TransporterId);
            }
            else
            {
                MoveQtyFromWarehouse(pli, qtyPickedNow); // Gry ktoś z lokacji BRAK znalazł odpowiednią ilość item'ów.

                //jeżeli był BRAK ale znalazł całą nową ilość to usuwa
                if (qtyNotFound <= 0)
                {
                    uow.PickingListItemRepo.Delete(pli);
                }
                //jeżeli był BRAK ale znalazł część ilości to pomniejsza QtyRequested
                if (qtyNotFound > 0 && qtyNotFound < pli.QtyRequested)
                {
                    status = EnumPickingListItemStatus.CompletedWithProblem;
                    //pLi.QtyRequested = qtyNotFound;
                    //uow.PickingListItemRepo.AddOrUpdate(pLi);
                }
                //jeżeli był BRAK i nie znalazł nowych ilości to nie usuwa, BRAK ZOSTAJE
                else
                {
                    status = SetPickingListItemStatus(pli, (int)qtyRemainToPick, (int)qtyNotFound);
                }
                ////jednak chyba zawsze trzeba usunąć BRAK bo generują się nowe wpisy na BRAKI
                //uow.PickingListItemRepo.Delete(pLi);
                AddTransporterLog(pli, status, pli.PickingList.WorkOrderId, pli.PickingList.TransporterId);
            }

            jsonModel.Data = status;
            return Json(jsonModel);
        }

        private StockUnit ReplaceStockUnit(PickingListItem pli, JsonModel jsonModel, decimal qtyPickedNow, string serialNumberScanned, string stockUnitSerialNumber, bool isItemAlreadyOnLine)
        {
            jsonModel.Status = (int)iLogisStatus.NoError;

            StockUnit stockUnit = uow.StockUnitRepo.GetBySerialNumber(serialNumberScanned);

            if (stockUnit != null)
            {
                decimal qtyAvailable = stockUnit.CurrentQtyinPackage - stockUnit.ReservedQty;
                if (qtyAvailable >= qtyPickedNow)
                {
                    decimal remainToPick = pli.QtyRequested - pli.QtyPicked - qtyPickedNow;

                    StockUnit stockUnitOld = GetStokUnitOfPickingListItem(pli, stockUnitSerialNumber);
                    uow.StockUnitRepo.UnreserveQtyFromStockUnit(stockUnitOld, pli.QtyRequested);
                    uow.StockUnitRepo.ReserveStockUnit(stockUnit, qtyPickedNow);
                    pli.StockUnitId = stockUnit.Id;
                    pli.QtyRequested = qtyPickedNow;
                    uow.PickingListItemRepo.AddOrUpdate(pli);

                    if (remainToPick > 0 && !isItemAlreadyOnLine)
                    {
                        PickingListController.FindLocationsAndSaveToDB(uow, remainToPick, pli);
                    }
                }
                else
                {
                    jsonModel.Data = "Dostępna ilośc: " + qtyAvailable.ToString("0.####");
                    jsonModel.Status = (int)iLogisStatus.StockUnitRequestedQtyNotAvailable;
                    //return Json(jsonModel);
                }
            }
            else
            {
                jsonModel.Data = "Zgłoś administratorowi";
                jsonModel.Status = (int)iLogisStatus.StockUnitNotFound;
                //return Json(jsonModel);
            }

            return stockUnit;
        }
        private StockUnit GetStokUnitOfPickingListItem(PickingListItem pLi, string serialNumber)
        {
            StockUnit stockUnit = pLi.StockUnit;
            decimal qtyToPick = pLi.QtyRequested - pLi.QtyPicked;

            if (stockUnit == null)
            {
                stockUnit = uow.StockUnitRepo.GetBySerialNumber(serialNumber);
            }

            if (stockUnit == null && qtyToPick > 0)
            {
                stockUnit = uow.StockUnitRepo.GetReservedStockUnit(pLi.PickingListId, pLi.ItemWMSId, pLi.WarehouseLocationId, 0);
            }
            else if (stockUnit == null && qtyToPick <= 0) // Przypadek gdy ktoś raz pobrał ilość np. 80 i potem pobiera 70.
            {
                stockUnit = uow.StockUnitRepo.GetStockUnit(pLi.PickingListId, pLi.ItemWMSId, pLi.WarehouseLocationId, 0);
            }

            return stockUnit;
        }
        private void SetStockUnitStatusToProblem(PickingListItem pLi)
        {
            if (pLi.StockUnit != null && pLi.StockUnit.WarehouseLocation != null && pLi.StockUnit.WarehouseLocation.Name != "BRAK")
            {
                pLi.StockUnit.Status = StatusEnum.PickerProblem; // -1 dajemy wtedy gdy użytkownik pobrał mniejszą ilość czyli w lokacji brakuje sztuk
                uow.StockUnitRepo.AddOrUpdate(pLi.StockUnit);
            }
        }
        private bool IsItemAlreadyOnLine(int commentListId)
        {
           return  commentListId == 3 ? true : false;
        }
        private void AddTransporterLog(PickingListItem pLi, EnumPickingListItemStatus status, int workOrderId = 0, int transporterId = 0)
        {
            TransporterLog tl = uow.TransporterLogRepo.GetTransporterLogByRelatedObjectId(pLi.Id,EnumTransporterLogEntryType.Picking);
            if(tl == null)
            {
                tl = new TransporterLog();
                tl.TimeStamp = DateTime.Now;
                tl.ItemWMS = pLi.ItemWMS;
                tl.ItemWMSId = pLi.ItemWMSId;
                //tl.User = pLi.User;
                tl.UserId = User.Identity.GetUserId();
                tl.RelatedObjectId = pLi.Id;
                tl.WorkorderNumber = pLi.PickingList.WorkOrder.OrderNumber;
                tl.ProductItemCode = pLi.PickingList.WorkOrder.Pnc.Code;
                tl.TransporterId = transporterId;
                tl.Status = (EnumTransportingStatus)status;
                tl.EntryType = EnumTransporterLogEntryType.Picking;
                tl.Location = pLi.WarehouseLocation.Name;
                tl.Comment = pLi.Comment;
                tl.ItemQty = pLi.QtyPicked;
            }
            else
            {
                tl.ItemQty = pLi.QtyPicked;
                tl.Comment = pLi.Comment;
            }
            uow.TransporterLogRepo.AddOrUpdate(tl);
        }
        private void MoveQtyFromStockUnit(StockUnit stockUnit, decimal qtyPicked, int platformId, int warehouseId, ProductionOrder wo)
        {
            if (stockUnit != null && qtyPicked > 0)
            {
                LabelExtraData data = new LabelExtraData();
                data.ExtraLabel1 = "PNC:";
                data.ExtraLabel2 = "Nr Zam:";
                data.ExtraLabel3 = "Ilosc:";
                data.ExtraData1 = wo.Pnc.Code;
                data.ExtraData2 = wo.OrderNumber;
                data.ExtraData3 = wo.QtyRemain.ToString();

                var c = new StockUnitController(db);
                c.ControllerContext = this.ControllerContext;
                c.MoveToLocation(stockUnit.Id, "", qtyPicked, stockUnit.UnitOfMeasure, platformId, warehouseId, labelExtraData: data);
                //uow.StockUnitRepo.TakeQtyFromStockUnit(pLi.PickingListId, stockUnit.Id, qtyToPick, pLi.QtyRequested);
            }
        }
        private void MoveQtyFromWarehouse(PickingListItem pLi, decimal qtyPicked)
        {
            if (qtyPicked > 0)
            {
                string wh_mainWarehouseCode = uow.WarehouseRepo.GetMainWarehouseCode();
                if (wh_mainWarehouseCode != "")
                {
                    var suc = new StockUnitController(db);
                    suc.ControllerContext = this.ControllerContext;
                    var json = suc.MoveManualToLocation(
                        pLi.ItemWMS.Item.Code,
                        pLi.QtyPicked,
                        pLi.UnitOfMeasure,
                        EnumMovementType.CODE_311,
                        wh_mainWarehouseCode,
                        pLi.Platform.Name,
                        "Utworzenie z Lokacji BRAK podczas pikowania"
                    );
                    Warehouse sourceWh = uow.WarehouseRepo.GetByCode(wh_mainWarehouseCode);
                    pLi.WarehouseLocation = uow.WarehouseLocationRepo.GetVirtualForWarehouse(sourceWh.Id);
                }
            }
        }
        private void AssignPlatform(PickingListItem pLi, string platformName) 
        { 
            pLi.Platform = uow.WarehouseLocationRepo.GetByName(platformName);

            if ((pLi.Platform == null && platformName != "") || (pLi.Platform !=null && pLi.Platform.Name != platformName))
            {
                Warehouse wh = uow.WarehouseRepo.GetPreparationAreaWarehouse();//uow.WarehouseRepo.GetOrCreate("Platformy", "9999");
                if(wh == null)
                {
                    wh = uow.WarehouseRepo.GetOrCreate("Platformy", "9999");
                }
                WarehouseLocationType whlt = uow.WarehouseLocationTypeRepo.GetOrCreate("Platforma");

                WarehouseLocation NewPlatform = new WarehouseLocation();
                NewPlatform.Name = platformName;
                NewPlatform.WarehouseId = wh.Id;
                NewPlatform.TypeId = whlt.Id;
                uow.WarehouseLocationRepo.Add(NewPlatform);
                pLi.Platform = NewPlatform;
            }
        }
        private EnumPickingListItemStatus SetPickingListItemStatus(PickingListItem pLi,int qtyReaminToPick = 0, int qtyNotFound = 0)
        {
            EnumPickingListItemStatus statusToJSON = EnumPickingListItemStatus.Completed;

            if(qtyReaminToPick == 0)
            {
                pLi.Status = EnumPickingListItemStatus.Completed;
                statusToJSON = EnumPickingListItemStatus.Completed;
            }
            //Niekompletny picking i nieznalezione nowe ilości w innych lokacjach
            else if (pLi.QtyPicked < pLi.QtyRequested && qtyNotFound == qtyReaminToPick)
            {
                pLi.Status = EnumPickingListItemStatus.CompletedWithProblem; // Pobrał jakąś ilość z lokacji dlatego ustawiamy mu, że zakończono z problemem.
                statusToJSON = EnumPickingListItemStatus.Problem; // Nie znalazł nowych lokacji, więc informacja będzie, że nie znaleziono nowych lokacji. - Sorry, życie.
            }
            //Niekompletny picking ale znalezione wymagane ilości w innych lokacjach
            else if (pLi.QtyPicked < pLi.QtyRequested && qtyNotFound == 0)
            {
                pLi.Status = EnumPickingListItemStatus.Completed; //musi być CREATED zwrocone do JS
                statusToJSON = EnumPickingListItemStatus.CompletedWithProblem; //musi być CREATED zwrocone do JS
            }
            //Niekompletny picking ale znaleziona część wymaganych ilości w innych lokacjach
            else if (pLi.QtyPicked < pLi.QtyRequested && qtyNotFound < qtyReaminToPick)
            {
                pLi.Status = EnumPickingListItemStatus.CompletedWithProblem;
                statusToJSON = EnumPickingListItemStatus.CompletedWithProblem;
            }
            //kompletny picking lub materiał jest na linii.
            else if (pLi.QtyPicked >= pLi.QtyRequested || (pLi.QtyPicked == 0 && pLi.Comment == "Materiał na linii"))
            {
                pLi.Status = EnumPickingListItemStatus.Completed;
                statusToJSON = EnumPickingListItemStatus.Completed;
            }

            uow.PickingListItemRepo.AddOrUpdate(pLi);

            return statusToJSON;
        }

        //------------------VIEW-PICKING-LIST-ITEM-SUMMARY-------------------------
        [HttpGet]
        public ActionResult Summary(int pickingListId, int workOrderId, int pickerId, string pickingListGuid="")
        {
            ViewBag.WorkOrderId = workOrderId;
            ViewBag.PickerId = pickerId;
            ViewBag.PickingListId = pickingListId;
            ViewBag.PickingListGuid = pickingListGuid;
            return View();
        }
        [HttpPost]
        public JsonResult SummaryGetList(int workOrderId, int pickerId, string pickingListGuid="")
        {
            //TODO: skonczyc funkcje w repo
            List<PickingListItemViewModel_2> summaryList = new List<PickingListItemViewModel_2>();
            if (pickingListGuid == "")
            {
                summaryList = uow.PickingListItemRepo.GetSummary(workOrderId, pickerId);
            }
            else
            {
                summaryList = uow.PickingListItemRepo.GetByWorkOrderAndPicker(workOrderId, pickerId, pickingListGuid).ToList();
            }

            return Json(summaryList);
        }

        //------------------VIEW-PICKING-LINE-FEED---------------------------------
        [HttpGet]
        public ActionResult LineFeed(int pickingListId, int workOrderId, int pickerId)
        {
            ViewBag.WorkOrderId = workOrderId;
            ViewBag.PickerId = pickerId;
            ViewBag.PickingListId = pickingListId;
            return View();
        }
        [HttpPost]
        public JsonResult LineFeedGetList(int workOrderId, int pickerId, int parameterH = -1)
        {
            ProductionOrder wo = uow.ProductionOrderRepo.GetById(workOrderId);
            List<PickingListItemViewModel_2> pList = uow.PickingListItemRepo.GetByWorkOrderAndPickerWithWorkstation(workOrderId, pickerId, wo.LineId, parameterH);

            PickingListViewModel_2 vm = new PickingListViewModel_2();
            if (pList.Count() != 0)
            {
                vm.PickingListItems = pList;
                vm.IsDataNull = false;
                vm.WorkOrderNo = pList[0].WorkOrderNo;
                vm.PickingListStatus = (int)uow.PickingListRepo.GetById(pList[0].PickingListId).Status;
                vm.NumberToPick = pList.Count(x => x.Status == EnumPickingListItemStatus.Pending);
                vm.PlatformList = uow.PickingListItemRepo.GePlatformsDistinct(workOrderId,pickerId, wo.LineId,parameterH).ToList();
            }
            return Json(vm);
        }
        [HttpPost]
        public JsonResult LineFeedConfirmDelivery(int pickingListItemId)
        {
            PickingListItem pli = uow.PickingListItemRepo.GetById(pickingListItemId);
            pli.Status = EnumPickingListItemStatus.Closed;
            uow.PickingListItemRepo.AddOrUpdate(pli);

            return Json(0);
        }
    }
}