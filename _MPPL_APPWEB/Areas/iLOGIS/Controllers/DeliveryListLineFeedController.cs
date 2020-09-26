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
using _MPPL_WEB_START.Areas.iLOGIS.ViewModels;
using MDL_BASE.Models.IDENTITY;
using MDL_iLOGIS.ComponentCore.Enums;
using _MPPL_WEB_START.Areas._APPWEB.ViewModels;

namespace _MPPL_WEB_START.Areas.iLOGIS.Controllers
{
    [Authorize]
    public class DeliveryListLineFeedController : Controller
    {
        IDbContextiLOGIS db;
        ILocataionManager locataionManager;
        UnitOfWork_iLogis uow;

        public DeliveryListLineFeedController(IDbContextiLOGIS db)
        {
            ViewBag.Skin = "nasaSkin";
            this.db = db;
            this.locataionManager = AutofacDependencyResolver.Current.ApplicationContainer.Resolve<ILocataionManager>();
            uow = new UnitOfWork_iLogis(db, locataionManager);
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult DeliveryListCreate(int transporterId, int workorderId)
        {
            Transporter transporter = uow.TransporterRepo.GetById(transporterId);
            ProductionOrder wo = uow.ProductionOrderRepo.GetById(workorderId);

            if (transporter != null && wo != null)
            {
                DeliveryList deliveryList = uow.DeliveryListRepo.GetList(transporterId, workorderId).FirstOrDefault();

                if (deliveryList == null)
                {
                    deliveryList = new DeliveryList();
                    deliveryList.TransporterId = transporterId;
                    deliveryList.WorkOrderId = workorderId;
                    deliveryList.Status = EnumDeliveryListStatus.Unassigned;
                    uow.DeliveryListRepo.AddOrUpdate(deliveryList);
                }
                    var pickingListItems = uow.PickingListItemRepo.GetByWorkOrder(workorderId);
                    var deliveryListItems = new List<DeliveryListItem>();

                    foreach (var pli in pickingListItems)
                    {
                        deliveryListItems.Add(new DeliveryListItem()
                        {
                            ItemWMSId = pli.ItemWMSId,
                            DeliveryListId = deliveryList.Id,
                            StockUnitId = pli.StockUnitId,
                            TransporterId = transporterId,
                            WarehouseLocationId = pli.PlatformId,
                            WorkOrderId = workorderId,
                            WorkstationId = null,
                            PickingListItemId = pli.Id,
                            BomQty = pli.BomQty,
                            QtyPerPackage = pli.QtyPicked,
                            QtyRequested = pli.QtyRequested,
                            QtyDelivered = 0,
                            QtyUsed = 0,
                            UnitOfMeasure = pli.UnitOfMeasure,
                            Status = EnumDeliveryListItemStatus.Pending
                        });
                    }

                    uow.DeliveryListItemRepo.AddOrUpdateRange(deliveryListItems);
                
            }

            return Json(0);
        }

        [HttpGet]
        public ActionResult DeliveryListLF(int transporterId, int? lastDeliveryListId = null)
        {
            ViewBag.TransporterId = transporterId;
            ViewBag.LastDeliveryListId = lastDeliveryListId;
            ViewBag.HasUserAdminRights = User.IsInRole(DefRoles.ILOGIS_ADMIN);
            return View();
        }
        [HttpPost]
        public JsonResult DeliveryListLFGetList(int transporterId)
        {
            var transporter = uow.TransporterRepo.GetById(transporterId);
            
            DateTime dateFrom = DateTime.Now.AddHours(-240);
            DateTime dateTo = DateTime.Now.AddHours(148);

            List<int> linesIds = new List<int>();
            foreach (string lineName in transporter.DedicatedResourcesArray)
            {
                linesIds.AddRange(uow.ResourceRepo.GetByName(lineName).Select(x => x.Id).ToList());
            }

            List<DeliveryListViewModel> vm = uow.DeliveryListRepo.GetListFiltered(dateFrom, dateTo, transporterId, linesIds, null, null);
            
            return Json(vm);
        }
        
        private EnumDeliveryListStatus DeliveryListLFUpdateStatus(List<DeliveryListItemViewModel> deliveryListItems)
        {
            if (deliveryListItems.Count <= 0) return EnumDeliveryListStatus.Unknow;

            int totalItems = deliveryListItems.Count;
            int totalItemsPending = deliveryListItems.Count(x => x.Status <= EnumDeliveryListItemStatus.Pending);
            int totalItemsCompleted = deliveryListItems.Count(x => x.Status >= EnumDeliveryListItemStatus.Completed);
            int deliveryListId = deliveryListItems[0].DeliveryListId;

            return _UpdateStatus(totalItems, totalItemsCompleted, deliveryListId);
        }
        private EnumDeliveryListStatus DeliveryListLFUpdateStatus(List<DeliveryListItem> deliveryListItems)
        {
            if (deliveryListItems.Count <= 0) return EnumDeliveryListStatus.Unknow;

            int totalItems = deliveryListItems.Count;
            int totalItemsPending = deliveryListItems.Count(x => x.Status <= EnumDeliveryListItemStatus.Pending);
            int totalItemsCompleted = deliveryListItems.Count(x => x.Status >= EnumDeliveryListItemStatus.Completed);
            int deliveryListId = deliveryListItems[0].DeliveryListId??0;

            return _UpdateStatus(totalItems, totalItemsCompleted, deliveryListId);
        }
        private EnumDeliveryListStatus _UpdateStatus(int totalItems, int totalItemsCompleted, int deliveryListId)
        {
            DeliveryList dl = uow.DeliveryListRepo.GetById(deliveryListId);
            if (totalItemsCompleted == totalItems)
            {
                dl.Status = EnumDeliveryListStatus.Completed;

                List<PickingList> pickingLists = uow.PickingListRepo.GetByWorkOrder(dl.WorkOrderId);
                foreach(var pickingList in pickingLists)
                {
                    pickingList.Status = EnumPickingListStatus.Closed;
                    uow.PickingListRepo.Update(pickingList);
                }
            }
            else if (totalItemsCompleted > 0)
            {
                dl.Status = EnumDeliveryListStatus.Processing;
            }
            else
            {
                dl.Status = EnumDeliveryListStatus.Pending;
            }

            uow.DeliveryListRepo.AddOrUpdate(dl);
            return dl.Status;
        }

        [HttpGet]
        public ActionResult DeliveryListItemsLF(int deliveryListId, int workOrderId, int transporterId)
        {
            ViewBag.WorkOrderId = workOrderId;
            ViewBag.TransporterId = transporterId;
            ViewBag.DeliveryListId = deliveryListId;
            return View();
        }
        [HttpPost]
        public JsonResult DeliveryListItemsLFGetList(int workOrderId, int transporterId, int parameterH = -1)
        {
            List<DeliveryListItemViewModel> vm = null;
            ProductionOrder wo = uow.ProductionOrderRepo.GetById(workOrderId);

            if (wo != null)
            {
                vm = uow.DeliveryListItemRepo.GetDeliveryListItemsLF(workOrderId, transporterId, wo.LineId, parameterH);

                if (vm.Count <= 0)
                {
                    var resp = DeliveryListCreate(transporterId, workOrderId);
                    vm = uow.DeliveryListItemRepo.GetDeliveryListItemsLF(workOrderId, transporterId, wo.LineId, parameterH);
                }

                DeliveryListLFUpdateStatus(vm);
            }
            return Json(vm);
        }
        [HttpPost]
        public JsonResult DeliveryListLFConfirmDelivery(int deliveryListItemId, string workstationName = "")
        {
            DeliveryListItem dli = uow.DeliveryListItemRepo.GetById(deliveryListItemId);
            EnumDeliveryListItemStatus status = ConfirmDelivery(dli);
            AddTransporterLog(dli, dli.Status, workstationName);
            return Json(status);
        }
        [HttpPost, AuthorizeRoles(DefRoles.ILOGIS_ADMIN)]
        public JsonResult DeliveryListLFConfirmAllAndClose(int deliveryListId, int transporterId, int workorderId)
        {
            EnumDeliveryListStatus status = EnumDeliveryListStatus.Unknow;
            DeliveryList dl = uow.DeliveryListRepo.GetById(deliveryListId);

            if(dl == null)
            {
                DeliveryListCreate(transporterId, workorderId);
                dl = uow.DeliveryListRepo.GetList().Where(x => x.TransporterId == transporterId && x.WorkOrderId == workorderId).FirstOrDefault();
            }

            if (dl != null)
            {
                List<DeliveryListItem> dliList = uow.DeliveryListItemRepo.GetList(dl.Id).ToList();
                foreach(var dli in dliList)
                {
                    ConfirmDelivery(dli);
                }
                status = DeliveryListLFUpdateStatus(dliList);
            }
            return Json(status);
        }

        private EnumDeliveryListItemStatus ConfirmDelivery(DeliveryListItem dli)
        {
            EnumDeliveryListItemStatus status = EnumDeliveryListItemStatus.Pending;

            if (dli != null && dli.Status < EnumDeliveryListItemStatus.Completed)
            {
                string freeText = "PL (" + dli.PickingListItem.PickingList.Transporter.Code + "): " + dli.DeliveryList.WorkOrder.OrderNumber;
                string itemCode = dli.ItemWMS != null && dli.ItemWMS.Item != null ? dli.ItemWMS.Item.Code : "";
                string whLocName = dli.WarehouseLocation?.Name;
                StockUnit su = uow.StockUnitRepo.GetByCodeAndLocationAndQty(itemCode, whLocName, dli.QtyRequested);
                WarehouseLocation whl = uow.WarehouseLocationRepo.GetByName(dli.WorkOrder.Line.Name);

                if (dli.QtyRequested <= 0)
                {
                    status = EnumDeliveryListItemStatus.Completed;
                    dli.Status = EnumDeliveryListItemStatus.Completed;
                    uow.DeliveryListItemRepo.AddOrUpdate(dli);
                }
                else
                {
                    if (su == null)
                    {
                        Warehouse wh_preparationArea = uow.WarehouseRepo.GetPreparationAreaWarehouse();
                        //ItemWMS itemWMS = uow.ItemWMSRepo.Get(null, null, itemCode);
                        //su = uow.StockUnitRepo.GetFromWarehouse_CreateIfNotExists(itemWMS, wh_preparationArea);
                        //uow.StockUnitRepo.AddOrUpdate(su);
                        //su = uow.StockUnitRepo.GetById(su.Id);

                        var suc = new StockUnitController(db);
                        suc.ControllerContext = this.ControllerContext;
                        var json = suc.MoveManual(itemCode, dli.QtyRequested, dli.UnitOfMeasure, EnumMovementType.CODE_311, wh_preparationArea.Code, whl.Warehouse.Code, freeText1: freeText);

                        if ((iLogisStatus)((JsonModel)json.Data).Status == iLogisStatus.NoError)
                        {
                            status = EnumDeliveryListItemStatus.Completed;
                            dli.Status = EnumDeliveryListItemStatus.Completed;
                            uow.DeliveryListItemRepo.AddOrUpdate(dli);
                        }
                    }
                    else
                    {
                        if (su != null && whl != null)
                        {

                            var suc = new StockUnitController(db);
                            suc.ControllerContext = this.ControllerContext;
                            //TODO: Jednostki miary w DeliveryListach!
                            var json = suc.Move(su.Id, su.SerialNumber, dli.QtyRequested, dli.StockUnit.UnitOfMeasure, EnumMovementType.CODE_311, destinationlocationId: whl.Id, freeText1: freeText, force: true, print: false);

                            if ((iLogisStatus)((JsonModel)json.Data).Status == iLogisStatus.NoError ||
                                (iLogisStatus)((JsonModel)json.Data).Status == iLogisStatus.MovementDoneButLabelsWereNotPrinted)
                            {
                                status = EnumDeliveryListItemStatus.Completed;
                                dli.Status = EnumDeliveryListItemStatus.Completed;
                                uow.DeliveryListItemRepo.AddOrUpdate(dli);
                            }
                        }
                    }
                }
            }

            return status;
        }
        private void AddTransporterLog(DeliveryListItem pLi, EnumDeliveryListItemStatus status, string workstationName)
        {
            TransporterLog tl = uow.TransporterLogRepo.GetTransporterLogByRelatedObjectId(pLi.Id, EnumTransporterLogEntryType.Delivery);
            if (tl == null)
            {
                tl = new TransporterLog();
                tl.TimeStamp = DateTime.Now;
                tl.ItemWMS = pLi.ItemWMS;
                tl.ItemWMSId = pLi.ItemWMSId;
                //tl.User = pLi.User;
                tl.UserId = User.Identity.GetUserId();
                tl.RelatedObjectId = pLi.Id;
                tl.WorkorderNumber = pLi.WorkOrder.OrderNumber;
                tl.ProductItemCode = pLi.WorkOrder.Pnc.Code;
                tl.TransporterId = pLi.TransporterId;
                tl.Status = (EnumTransportingStatus)pLi.Status;
                tl.EntryType = EnumTransporterLogEntryType.Delivery;
                tl.Location = workstationName;
                tl.Comment = pLi.WarehouseLocation?.Name;
                tl.ItemQty = pLi.QtyRequested;
            }
            else
            {
                tl.ItemQty = pLi.QtyRequested;
                tl.Comment = pLi.WarehouseLocation?.Name;
            }
            uow.TransporterLogRepo.AddOrUpdate(tl);
        }

        //------------------VIEW-TRANSPORTER-LIST-------------------------------
        [HttpGet]
        public ActionResult TransporterList()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetLineFeeders()
        {
            List<Transporter> TransporterList = uow.TransporterRepo.GetLineFeeders();
            return Json(TransporterList);
        }
    }
}