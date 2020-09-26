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
using MDL_iLOGIS.ComponentWMS._Interfaces;
using MDL_BASE.Models.MasterData;
using MDLX_MASTERDATA.Enums;
using XLIB_COMMON.Repo.IDENTITY;
using Lextm.SharpSnmpLib.Security;
using MDL_BASE.Models.IDENTITY;
using _MPPL_WEB_START.Areas._APPWEB.ViewModels;
using System.Web.UI.WebControls;

namespace _MPPL_WEB_START.Areas.iLOGIS.Controllers
{
    [Authorize]
    public class PickingListController : Controller
    {
        int version = 25;
        IDbContextiLOGIS db;
        ILocataionManager locataionManager;
        UnitOfWork_iLogis uow;
        UserRepo UserManager;
        public PickingListController(IDbContextiLOGIS db, IUserStore<MDL_BASE.Models.IDENTITY.User, string> userStore)
        {
            ViewBag.Skin = "nasaSkin";
            this.db = db;
            UserManager = new UserRepo(userStore, db);
            this.locataionManager = AutofacDependencyResolver.Current.ApplicationContainer.Resolve<ILocataionManager>();
            uow = new UnitOfWork_iLogis(db, locataionManager);
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetVersion()
        {
            return Json(version);
        }

        //------------------VIEW-PICKING-LIST------------------------------------
        [HttpGet]
        public ActionResult PickingList(int pickerId, int? lastPickingListId = null)
        {
            ViewBag.PickerId = pickerId;
            ViewBag.LastPickingListId = lastPickingListId;
            ViewBag.IsAdminMode = UserManager.IsInRole(User.Identity.GetUserId(), DefRoles.ILOGIS_ADMIN);
            return View();
        }

        [HttpPost]
        public JsonResult PickingListCreateGroup(List<int> pickingListIds)
        {
            List<PickingList> pickingList = new List<PickingList>();
            foreach (int pickingListId in pickingListIds)
            {
                PickingList pickList = uow.PickingListRepo.GetById(pickingListId);
                pickingList.Add(pickList);
            }
            List<int> workOrderIds = pickingList.Select(x => x.WorkOrderId).ToList();
            List<PickingList> pList = uow.PickingListRepo.GetByWorkOrdersAndPicker(workOrderIds, 0);
            List<int> transrpotersIds = uow.TransporterRepo.GetPickers().Select(x => x.Id).ToList();

            List<PickingList> pickingListWithSameGuid = new List<PickingList>();
            foreach (int transporterId in transrpotersIds)
            {
                Guid guid = Guid.NewGuid();
                DateTime guidDateTime = DateTime.Now;
                pickingListWithSameGuid = pList.Where(x => x.TransporterId == transporterId).ToList();
                foreach (var pickList in pickingListWithSameGuid)
                {
                    pickList.Guid = guid.ToString();
                    pickList.GuidCreationDate = guidDateTime;
                    uow.PickingListRepo.AddOrUpdate(pickList);
                }
            }
            return Json(pList);
        }

        [HttpPost]
        public JsonResult PickingListDeleteGroup(string pickingListGroupGuid)
        {
            List<PickingList> pickingListByGroup = uow.PickingListRepo.GetByGuid(pickingListGroupGuid);
            List<int> workOrderIds = pickingListByGroup.Select(x => x.WorkOrderId).ToList();
            List<PickingList> pList = uow.PickingListRepo.GetByWorkOrdersAndPicker(workOrderIds, 0);

            foreach (var pickingList in pList)
            {
                pickingList.Guid = "";
                pickingList.GuidCreationDate = new DateTime(1900, 1, 1);
                uow.PickingListRepo.AddOrUpdate(pickingList);
            }
            return Json(pList);
        }


        [HttpPost]
        public JsonResult GetList(int pickerId)
        {
            PLWOviewModel vm = new PLWOviewModel();
            vm.transporter = uow.TransporterRepo.GetById(pickerId);

            DateTime dateFrom = DateTime.Now.AddDays(-240);
            DateTime dateTo = DateTime.Now.AddHours(148);

            //if (_MPPL_WEB_START.Properties.Settings.Default.Client == "ElectroluxPLB")
            //{
            //    dateFrom = new DateTime(2020, 7, 01, 22, 0, 0);
            //    dateTo = new DateTime(2020, 8, 03, 6, 0, 0);
            //}

            List<int> linesIds = new List<int>();
            foreach (string lineName in vm.transporter.DedicatedResourcesArray)
            {
                linesIds.AddRange(uow.ResourceRepo.GetByName(lineName).Select(x => x.Id).ToList());
            }

            //vm.list = uow.PickingListRepo.GetListFiltered(dateFrom, dateTo, linesIds, null, null);
            vm.list = uow.PickingListRepo.GetListFiltered(dateFrom, dateTo, pickerId, linesIds, null, null);
            vm.list = vm.list.OrderByDescending(x => x.GuidCreationDateTime).ThenBy(x => x.WorkOrderNumber).ToList();
            return Json(vm);
        }
        [HttpPost]
        public JsonResult Create(int workOrderId, int pickerId, int[] workOrderIds)
        {
            IQueryable<PickingListItemViewModel_2> pList = uow.PickingListItemRepo.GetByWorkOrderAndPicker(workOrderId, pickerId);
            int pickingListId;

            if (pList.Count() <= 0)
            {
                PickingList pickingListCreated = uow.PickingListRepo.GetByWorkOrderAndPicker(workOrderId, pickerId).FirstOrDefault();
                 
                if (pickingListCreated == null || pickingListCreated.Id <= 0)
                {
                    ProductionOrder wo = uow.ProductionOrderRepo.GetById(workOrderId);
                    PickingList pickingList = CreateNewPickingList(uow, workOrderId, pickerId);
                    List<PickingListItem> itemListForPicker = GetItemsForPicker(uow, wo, pickerId, pickingList);

                    if (itemListForPicker.Count > 0)
                    {
                        if (isConnectionWithFSDS(itemListForPicker[0]))
                        {
                            AddItemsToPickingList(uow, itemListForPicker);
                            pickingListId = pickingList.Id;
                        }
                        else
                        {
                            uow.PickingListRepo.Delete(pickingList);
                            return Json("No connection with FSDS");
                        }
                    }
                    pickingListId = pickingList.Id;
                }
                else
                {
                    return Json("Picking List is Created");
                }
            }
            else
            {
                pickingListId = pList.FirstOrDefault().PickingListId;
            }
            return Json(pickingListId);
        }

        [HttpPost]
        public JsonResult CreateMany(List<int> workOrderIds)
        {
            int problems = 0;
            int numberOfCreatedPickingLists = 0;
            JsonModel jsonModel = new JsonModel();
            List<PickingList> pickListToReturn = new List<PickingList>();
            foreach (int workOrderId in workOrderIds)
            {
                ProductionOrder po = uow.ProductionOrderRepo.GetById(workOrderId);
                List<Transporter> transporters = uow.TransporterRepo.GetPickers();
                foreach (var transporter in transporters)
                {
                    PickingList pickingListCreated = uow.PickingListRepo.GetByWorkOrderAndPicker(workOrderId, transporter.Id).FirstOrDefault();
                    if (pickingListCreated == null || pickingListCreated.Id <= 0)
                    {
                        pickingListCreated = CreateNewPickingList(uow, workOrderId, transporter.Id);
                        List<PickingListItem> itemListForPicker = GetItemsForPicker(uow, po, transporter.Id, pickingListCreated);
                        pickingListCreated.Status = EnumPickingListStatus.Pending;
                        uow.PickingListRepo.AddOrUpdate(pickingListCreated);
                        if (itemListForPicker.Count > 0)
                        {
                            if (isConnectionWithFSDS(itemListForPicker[0]))
                            {
                                AddItemsToPickingList(uow, itemListForPicker);
                                numberOfCreatedPickingLists++;
                            }
                            else
                            {
                                uow.PickingListRepo.Delete(pickingListCreated);
                                problems++;
                            }
                        }
                        pickListToReturn.Add(pickingListCreated);
                    }
                    else
                    {
                        _VerifyPickingList(po, transporter, pickingListCreated);
                    }
                }
            }
            if (problems > 0 && numberOfCreatedPickingLists > 0)
            {
                jsonModel.MessageType = JsonMessageType.warning;
                jsonModel.Message = "Liczba nieutworzonych PL: " + problems;
            }
            else if (problems > 0)
            {
                jsonModel.MessageType = JsonMessageType.danger;
                jsonModel.Message = "Nie utworzonow żadnych Picking List";
            }
            else
            {
                jsonModel.MessageType = JsonMessageType.success;
                jsonModel.Message = "Liczba utworzonych PL: " + numberOfCreatedPickingLists;
            }
            jsonModel.Data = pickListToReturn;
            return Json(jsonModel);
        }

        [HttpPost]
        public JsonResult GetPickingListsForAllPickers(List<int> pickingListsIds)
        {
            List<PickingList> pList = new List<PickingList>();
            if (pickingListsIds != null)
            {
                List<PickingList> pickingList = new List<PickingList>();
                foreach (int pickingListId in pickingListsIds)
                {
                    PickingList pickList = uow.PickingListRepo.GetById(pickingListId);
                    pickingList.Add(pickList);
                }
                List<int> workOrderIds = pickingList.Select(x => x.WorkOrderId).ToList();
                pList = uow.PickingListRepo.GetByWorkOrdersAndPicker(workOrderIds, 0);
            }
            return Json(pList);
        }

        [HttpPost]
        public JsonResult VerifyPickingList(int pickerId)
        {
            Transporter train = uow.TransporterRepo.GetById_AsNoTracking(pickerId);
            List<int> transporters = train.ConnectedTransportersArray; transporters.Add(Convert.ToInt32(train.Code));
            var pickingLists = uow.PickingListRepo.GetList().Where(x => x.TransporterId == pickerId).ToList();

            foreach (var pl in pickingLists)
            {
                if (pl.Status >= EnumPickingListStatus.Created && pl.Status < EnumPickingListStatus.Completed)
                {
                    _VerifyPickingList(pl.WorkOrder, train, pl);
                }
            }

            return Json(0);
        }
        private void _VerifyPickingList(ProductionOrder po, Transporter picker, PickingList pi)
        {
            if (po != null)
            {
                List<PickingListItem> existingDL = uow.PickingListItemRepo.GetByWorkOrderPicker(po.Id, picker.Id).ToList();
                List<PickingListItem> simulatedDL = GetItemsForPicker(uow, po, picker.Id, pi);
                //uow.PickingListItemRepo..Simulate(transporters, (po.QtyPlanned - po.QtyProducedInPast), picker.Id, po.Id, po.OrderNumber, po.Pnc.Code, picker.DedicatedResourcesArray);

                DeleteUnusedItems(existingDL, simulatedDL);
                AddNewItems(existingDL, simulatedDL);
            }
        }
        private void DeleteUnusedItems(List<PickingListItem> existingDL, List<PickingListItem> simulatedDL)
        {
            int i = 0;
            while (i < existingDL.Count)
            {
                var existingItem = existingDL[i];
                int counter = simulatedDL.Count(x => x.ItemWMSId == existingItem.ItemWMSId);
                if (counter <= 0)
                {
                    uow.PickingListItemRepo.Delete(existingItem);
                    existingDL.Remove(existingItem);
                }
                else
                {
                    i++;
                }
            }
        }
        private void AddNewItems(List<PickingListItem> existingDL, List<PickingListItem> simulatedDL)
        {
            foreach (var simulatedItem in simulatedDL)
            {
                int counter = existingDL.Count(x => x.ItemWMSId == simulatedItem.ItemWMSId);
                if (counter <= 0)
                {
                    //uow.PickingListItemRepo.AddOrUpdate(simulatedItem);
                    FindLocationsAndSaveToDB(uow, simulatedItem.PickingList.WorkOrder.QtyRemain, simulatedItem);
                }
            }
        }

        [HttpPost]
        public JsonResult SetStatus(int pickingListId, int pickerId = 0, int workOrderId = 0)
        {
            if (pickingListId > 0 && pickerId > 0 && workOrderId > 0)
            {
                IQueryable<PickingListItemViewModel_2> pList = uow.PickingListItemRepo.GetByWorkOrderAndPicker(workOrderId, pickerId);
                PickingList pickingList = uow.PickingListRepo.GetById(pickingListId);
                pickingList.Status = PickingListItemController.GetPickingListStatusByItems(pList.ToList());
                uow.PickingListRepo.AddOrUpdate(pickingList);
            }
            return Json(0);
        }

        public static PickingList CreateNewPickingList(UnitOfWork_iLogis uow, int workOrderId, int pickerId)
        {
            PickingList pickingList = new PickingList()
            {
                WorkOrderId = workOrderId,
                TransporterId = pickerId,
                Status = EnumPickingListStatus.Unassigned,
            };
            uow.PickingListRepo.AddOrUpdate(pickingList);
            return pickingList;
        }
        public static List<PickingListItem> GetItemsForPicker(UnitOfWork_iLogis uow, ProductionOrder wo, int pickerId, PickingList pickingList)
        {
            Transporter tr = uow.TransporterRepo.GetById(pickerId);
            List<int> pickersNumbers = tr.ConnectedTransportersArray;
            pickersNumbers.Add(Int32.Parse(tr.Code)); // dodanie pickera standardowego

            List<int> pickerItemIds = uow.ItemWMSRepo.GetList().Where(x => pickersNumbers.Contains(x.PickerNo)).Select(x => x.Item.Id).ToList();
            List<BomWorkorder> bomWorkorderItems = uow.BomWorkorderRepo.GetItemsOfWorkOrderFilteredByIds(wo, pickerItemIds);

            if (bomWorkorderItems == null || bomWorkorderItems.Count <= 0)
            {
                //Pobierz dane ze zwykłego bomu jezeli zamówienie nie zostało znalezione w bomWorkorders
                bomWorkorderItems = uow.BomRepo.GetItemsForPNCAndTransporter(wo.Pnc.Code, pickerItemIds).Select(x =>
                    new BomWorkorder()
                    {
                        Parent = x.Pnc,
                        ParentId = (int)x.PncId,
                        Child = x.Anc,
                        ChildId = (int)x.AncId,
                        QtyUsed = x.PCS
                    }).ToList();
            }

            List<int> bomItemIds = bomWorkorderItems.Select(y => y.ChildId).ToList();
            List<ItemWMS> ItemWMSs = uow.ItemWMSRepo.GetList().Where(x => bomItemIds.Contains(x.ItemId)).ToList();
            List<PickingListItem> pickingListItems = new List<PickingListItem>();

            foreach (BomWorkorder bw in bomWorkorderItems)
            {
                PickingListItem pli = new PickingListItem();

                pli.ItemWMS = ItemWMSs.FirstOrDefault(x => x.ItemId == bw.ChildId);
                pli.ItemWMSId = pli.ItemWMS != null ? pli.ItemWMS.Id : 0;
                pli.BomQty = bw.QtyUsed;
                pli.QtyPicked = 0;
                pli.QtyRequested = (decimal)((decimal)wo.QtyRemain * bw.QtyUsed);
                pli.UnitOfMeasure = bw.UnitOfMeasure;
                pli.PickingListId = pickingList.Id;
                pli.PickingList = pickingList;

                if (pli.ItemWMS != null)
                {
                    pickingListItems.Add(pli);
                }
            }

            pickingListItems = GroupPickingListItemsByItemWMS(pickingListItems);

            return pickingListItems;
        }
        public static List<PickingListItem> GroupPickingListItemsByItemWMS(List<PickingListItem> pickingListItems)
        {
            List<PickingListItem> pickingListItemsGroupped = pickingListItems
                .GroupBy(x => x.ItemWMS).Select(x => new PickingListItem()
                {
                    ItemWMS = x.Key,
                    ItemWMSId = x.Key.Id,
                    BomQty = x.FirstOrDefault().BomQty,
                    QtyPicked = 0,
                    QtyRequested = x.Sum(qr => qr.QtyRequested),
                    UnitOfMeasure = x.FirstOrDefault().UnitOfMeasure,
                    PickingListId = x.FirstOrDefault().PickingListId,
                    PickingList = x.FirstOrDefault().PickingList
                }).ToList();

            return pickingListItemsGroupped;
        }
        public static void AddItemsToPickingList(UnitOfWork_iLogis uow, List<PickingListItem> itemListForPicker)
        {
            foreach (PickingListItem pickingListItem in itemListForPicker)
            {
                FindLocationsAndSaveToDB(uow, pickingListItem.QtyRequested, pickingListItem);
            }
        }
        private bool isConnectionWithFSDS(PickingListItem pLi)
        {
            var testApi = locataionManager.GetLocationsOfItem(pLi.ItemWMS.Item.Code, 10, 0);
            return testApi == null ? false : true;
        }
        public static decimal FindLocationsAndSaveToDB(UnitOfWork_iLogis uow, decimal qtyRemain, PickingListItem pickingListItem)
        {
            PickingListItem pListItem;
            List<StockUnit> stockUnitList = uow.StockUnitRepo.GetLocationsOfItemByQty(pickingListItem.ItemWMS.Item.Code, qtyRemain);

            //pickingListItem.BomQty = pickingListItem.BomQty == 0? 1 : pickingListItem.BomQty;
            //decimal remainToPick = qtyRemain * pickingListItem.BomQty;
            decimal qtyRemainToPick = qtyRemain;
            stockUnitList = stockUnitList.Where(x => x.WarehouseLocation.Type.TypeEnum != WarehouseLocationTypeEnum.Trolley).ToList();
            foreach (StockUnit stockUnit in stockUnitList)
            {
                decimal qtyAvailable = stockUnit.CurrentQtyinPackage - stockUnit.ReservedQty;
                decimal qtyToPick = Math.Min(qtyRemainToPick, qtyAvailable); //remainToPick >= QtyAvailable ? QtyAvailable : remainToPick;

                if (qtyToPick > 0)
                {
                    pListItem = new PickingListItem()
                    {
                        ItemWMSId = pickingListItem.ItemWMSId,
                        QtyRequested = qtyToPick,
                        UnitOfMeasure = pickingListItem.UnitOfMeasure,
                        Status = EnumPickingListItemStatus.Pending,
                        PickingListId = pickingListItem.PickingListId,
                        WarehouseLocationId = stockUnit.WarehouseLocation.Id,
                        StockUnitId = stockUnit.Id,
                        BomQty = pickingListItem.BomQty
                    };
                    uow.PickingListItemRepo.AddOrUpdate(pListItem);
                    uow.StockUnitRepo.ReserveStockUnit(stockUnit, qtyToPick);
                    qtyRemainToPick -= qtyToPick;
                }
            }

            if (qtyRemainToPick > 0)
            {
                int wlMissingId;
                WarehouseLocation wLMissing = uow.WarehouseLocationRepo.GetByName("BRAK");
                wlMissingId = wLMissing.Id;

                AddOrUpdateMissingItemEntry(uow, pickingListItem, qtyRemainToPick, wlMissingId);
            }

            return qtyRemainToPick;
        }
        private static void AddOrUpdateMissingItemEntry(UnitOfWork_iLogis uow, PickingListItem pickingListItem, decimal remainToPick, int wlMissingId)
        {
            PickingListItem pListItemMissing = uow.PickingListItemRepo.GetList()
                .FirstOrDefault(x => 
                    x.PickingListId == pickingListItem.PickingListId &&
                    x.ItemWMSId == pickingListItem.ItemWMSId && 
                    x.WarehouseLocationId == wlMissingId
                );

            if (pListItemMissing == null)
            {
                pListItemMissing = new PickingListItem()
                {
                    ItemWMSId = pickingListItem.ItemWMSId,
                    QtyRequested = remainToPick,
                    UnitOfMeasure = pickingListItem.UnitOfMeasure,
                    Status = EnumPickingListItemStatus.Problem,
                    PickingListId = pickingListItem.PickingListId,
                    WarehouseLocationId = wlMissingId,
                    StockUnitId = null,
                };
                uow.PickingListItemRepo.AddOrUpdate(pListItemMissing);
            }
            else
            {
                if (pListItemMissing.Id != pickingListItem.Id)
                {
                    pListItemMissing.QtyRequested += remainToPick;
                }
                uow.PickingListItemRepo.AddOrUpdate(pListItemMissing);
            }
        }

        //------------------VIEW-TRANSPORTER-LIST-------------------------------
        [HttpGet]
        public ActionResult TransporterList()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetPickers()
        {
            string userId = User.Identity.GetUserId();
            List<Transporter> TransporterList = uow.TransporterRepo.GetPickers();

            return Json(TransporterList);
        }

        //------------------VIEW-TRANSPORTER-LIST-------------------------------
        [HttpGet]
        public ActionResult PickingStatus(int resourceId = 0)
        {
            if (resourceId <= 0)
            {
                return Redirect("PickingStatusResourceSelector");
            }

            Resource2 r = uow.ResourceRepo.GetById(resourceId);
            string resourceName = r != null ? r.Name : "";
            return RedirectToAction("VIndex", "ScheduleMonitor", new { area = "PRD", line = resourceName });
        }
        [HttpGet]
        public ActionResult PickingStatusResourceSelector()
        {
            ViewBag.Resources = uow.ResourceRepo.GetList().Where(x => x.Type == ResourceTypeEnum.Resource).ToList();
            return View();
        }
    }
}