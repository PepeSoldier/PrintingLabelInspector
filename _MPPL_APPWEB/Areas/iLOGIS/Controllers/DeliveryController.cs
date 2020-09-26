using _MPPL_WEB_START.Areas._APPWEB.ViewModels;
using MDL_BASE.Models.IDENTITY;
using MDL_BASE.Models.MasterData;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentConfig.Mappers;
using MDL_iLOGIS.ComponentConfig.ViewModels;
using MDL_iLOGIS.ComponentCore.Enums;
using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_iLOGIS.ComponentWMS.EntityInterfaces;
using MDL_iLOGIS.ComponentWMS.Enums;
using MDL_iLOGIS.ComponentWMS.Mappers;
using MDL_iLOGIS.ComponentWMS.Models;
using MDL_iLOGIS.ComponentWMS.ViewModels;
using MDL_ONEPROD.ComponentWMS.Models;
using MDL_ONEPROD.Repo;
using MDL_WMS.ComponentConfig.UnitOfWorks;
using MDLX_MASTERDATA._Interfaces;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XLIB_COMMON.Enums;
using XLIB_COMMON.Model;
using XLIB_COMMON.Repo.IDENTITY;

namespace _MPPL_WEB_START.Areas.iLOGIS.Controllers
{
    [Authorize]
    public class DeliveryController : Controller
    {
        UnitOfWork_iLogis uow;
        private readonly IDbContextiLOGIS db;

        public DeliveryController(IDbContextiLOGIS db)
        {
            this.db = db;
            uow = new UnitOfWork_iLogis(db);
            ViewBag.Skin = "nasaSkin";
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Browse()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            DeliveryViewModel vm = new DeliveryViewModel();
            if (id != 0)
            {
                Delivery d = uow.DeliveryRepo.GetById(id);
                vm = d.CastToViewModel<DeliveryViewModel>();
            }
            else
            {
                vm.DocumentDate = DateTime.Now;
            }
            return View(vm);
        }
        //---------------Delivery------------------------------------------------
        [HttpPost]
        public JsonResult Edit(DeliveryViewModel vm)
        {
            Delivery d = new Delivery();
            ReflectionHelper.CopyProperties(vm, d);
            d.StampTime = DateTime.Now;
            uow.DeliveryRepo.AddOrUpdate(d);

            vm = d.FirstOrDefault<DeliveryViewModel>();

            return Json(vm);
        }
        [HttpPost]
        public JsonResult DeliveryGetList(DeliveryViewModel filter, int pageIndex, int pageSize)
        {
            IQueryable<int> deliveryListId;
            if(filter.Guid == "undefined" || filter.Guid == null)
            {
                filter.Guid = "";
            }
            deliveryListId = uow.DeliveryItemRepo.GetUniqueDeliveryId(filter);
            if (filter.Guid != "")
            {
                pageSize = deliveryListId.Count();
            }
            var delList = deliveryListId.ToList();


            IQueryable<Delivery> query = uow.DeliveryRepo.GetFilterList(filter)
                .Include(c => c.DeliveryItems)
                .Where(x => filter.ItemCode == null || deliveryListId.Contains(x.Id))
                .OrderByDescending(x => x.Id);

            int startIndex = (pageIndex - 1) * pageSize;
            int itemsCount = query.Count();

            List<DeliveryViewModel> deliveries = query
                .Skip(startIndex).Take(pageSize)
                .ToList<DeliveryViewModel>();


            return Json(new { data = deliveries, itemsCount });
        }
        public JsonResult DeliveryGet(int id)
        {
            Delivery d = uow.DeliveryRepo.GetById(id);
            return Json(d);
        }
        [HttpPost]
        public JsonResult DeliveryUpdate(Delivery item)
        {
            uow.DeliveryRepo.AddOrUpdate(item);
            return Json(item);
        }
        [HttpPost]
        public JsonResult DeliveryDelete(Delivery item)
        {
            uow.DeliveryRepo.Delete(item);
            return Json(null);
        }
        [HttpPost]
        public JsonResult DeliveryImport(Delivery delivery, List<DeliveryItem> deliveryItems)
        {
            if (delivery.Supplier != null && delivery.Supplier.Name != null && delivery.Supplier.Code != null)
            {
                Contractor contractor = uow.RepoContractor.GetOrCreate(delivery.Supplier.Code, delivery.Supplier.Name);
                Delivery d = CheckIfDeliveryExistsAndCreate(delivery, contractor);
                string userId = User.Identity.GetUserId();
                
                var diListGroupped = deliveryItems.GroupBy(x => x.ItemWMS).Select(x => new
                {
                    ItemWMSId = x.Key.Id,
                    ItemCode = x.Key.Item.Code,
                    TotalQty = x.Sum(s => s.TotalQty)
                });

                List<DeliveryItem> diList = uow.DeliveryItemRepo.GetByDeliveryId(d.Id).ToList();

                foreach (var y in diListGroupped)
                {
                    DeliveryItem di = diList.FirstOrDefault(x => x.ItemWMSId == y.ItemWMSId);
                    if (di == null)
                    {
                        di = new DeliveryItem()
                        {
                            DeliveryId = d.Id,
                            AdminEntry = true,
                            OperatorEntry = false,
                            ItemWMSId = y.ItemWMSId,
                            NumberOfPackages = 1,
                            QtyInPackage = y.TotalQty,
                            TotalQty = y.TotalQty,
                            UserId = userId,
                            WasPrinted = false,
                            Deleted = false,
                        };
                    }
                    else
                    {
                        di.TotalQty = y.TotalQty;
                        di.QtyInPackage = y.TotalQty;
                    }

                    uow.DeliveryItemRepo.AddOrUpdate(di);
                }
            }

            return Json(null);
        }

        private Delivery CheckIfDeliveryExistsAndCreate(Delivery delivery, Contractor contractor)
        {
            Delivery deliveryInDB = uow.DeliveryRepo.GetList().Where(x =>
                    x.Supplier.Name == delivery.Supplier.Name
                    && x.DocumentNumber == delivery.DocumentNumber
                    && x.DocumentDate.Year == delivery.DocumentDate.Year)
                .FirstOrDefault();

            if (deliveryInDB == null)
            {
                deliveryInDB = new Delivery()
                {
                    DocumentDate = delivery.DocumentDate,
                    DocumentNumber = delivery.DocumentNumber,
                    StampTime = DateTime.Now,
                    SupplierId = contractor.Id,
                    UserId = User.Identity.GetUserId(),
                    EnumDeliveryStatus = MDL_iLOGIS.ComponentWMS.Enums.EnumDeliveryStatus.Init,
                };
                uow.DeliveryRepo.AddOrUpdate(deliveryInDB);
            }
            return deliveryInDB;
        }

        //---------------Delivery-Item-------------------------------------------
        [HttpPost]
        public JsonResult DeliveryItemGetList(DeliveryItemViewModel filter, int pageIndex, int pageSize)
        {
            IQueryable<DeliveryItem> query = uow.DeliveryItemRepo.GetByDeliveryId(filter.DeliveryId);

            int startIndex = (pageIndex - 1) * pageSize;
            int itemsCount = query.Count();

            List<DeliveryItemViewModel> diList = query
                .Skip(startIndex).Take(pageSize)
                .ToList<DeliveryItemViewModel>();

            return Json(new { data = diList, itemsCount });
        }
        [HttpPost]
        public JsonResult DeliveryItemUpdate(DeliveryItemViewModel item)
        {
            DeliveryItem di = uow.DeliveryItemRepo.GetById(item.Id);

            if (di == null)
            {
                di = new DeliveryItem();
            }

            di.ItemWMSId = item.ItemWMSId;
            di.DeliveryId = item.DeliveryId;
            di.QtyInPackage = item.QtyInPackage;
            di.UnitOfMeasure = item.UnitOfMeasure;
            uow.DeliveryItemRepo.AddOrUpdate(di);
            return Json(item);
        }
        [HttpPost]
        public JsonResult DeliveryItemDelete(DeliveryItemViewModel item)
        {
            DeliveryItem di = new DeliveryItem() { Id = item.Id };
            uow.DeliveryItemRepo.Delete(di);
            return Json(1);
        }
        [HttpPost]
        public JsonResult SaveDeliveryItemGrid(List<DeliveryItemViewModel> deliveryItems)
        {
            int i = 1;

            foreach (DeliveryItemViewModel delItm in deliveryItems)
            {
                DeliveryItem di = SaveDeliveryItemFromGrid(i, delItm);
                i = di != null ? i + 1 : i;
            }

            return Json(0);
        }
        [HttpPost]
        public JsonResult SaveDeliveryItem(DeliveryItemViewModel deliveryItem, int rowNumber)
        {
            DeliveryItem x = SaveDeliveryItemFromGrid(rowNumber, deliveryItem);

            DeliveryItemViewModel vm = new DeliveryItemViewModel();

            if (x != null)
            {
                vm = new DeliveryItemViewModel()
                {
                    ItemWMSId = x.ItemWMSId,
                    Id = x.DeliveryId,
                    SupplierId = deliveryItem.SupplierId,
                    QtyInPackage = x.QtyInPackage,
                    UnitOfMeasure = x.UnitOfMeasure,
                };
            }
            else
            {
                vm = new DeliveryItemViewModel()
                {
                    Id = 0,
                    ItemWMSId = 0,
                    DeliveryId = 0,
                    SupplierId = 0,
                    ItemCode = "CODE NOT FOUND",
                    ItemName = "",
                    QtyInPackage = deliveryItem.QtyInPackage,
                    UnitOfMeasure = deliveryItem.UnitOfMeasure
                };
            }

            return Json(vm);
        }
        private DeliveryItem SaveDeliveryItemFromGrid(int LineNumber, DeliveryItemViewModel delItm)
        {
            DeliveryItem deliveryItem = null;

            if (delItm.Id > 0)
            {
                deliveryItem = uow.DeliveryItemRepo.GetById(delItm.Id);
            }
            else
            {
                var itemWMS = uow.ItemWMSRepo.GetList().Where(x => x.Item.Code == delItm.ItemCode).FirstOrDefault();

                if (itemWMS != null)
                {
                    deliveryItem = new DeliveryItem();
                    deliveryItem.ItemWMS = itemWMS;
                    deliveryItem.ItemWMSId = itemWMS.Id;
                    deliveryItem.DeliveryId = delItm.Id;
                }
            }

            if (deliveryItem != null)
            {
                uow.DeliveryItemRepo.AddOrUpdate(deliveryItem);
            }

            return deliveryItem;
        }
        [HttpPost]
        public JsonResult DeliveryInspectionCreateGroup(List<int> deliveryIds)
        {
            Guid obj = Guid.NewGuid();
            List<Delivery> delivList = new List<Delivery>();
            foreach (int deliveryId in deliveryIds)
            {
                Delivery delivery = uow.DeliveryRepo.GetById(deliveryId);
                delivery.Guid = obj.ToString();
                uow.DeliveryRepo.AddOrUpdate(delivery);
                delivList.Add(delivery);
            }
            return Json(delivList);
        }

        [HttpPost]
        public JsonResult DeliveryDeleteGroup(string deliveryGroupGuid = "")
        {
            List<Delivery> deliveryListByGroup = uow.DeliveryRepo.GetGroupByGuid(deliveryGroupGuid);
            foreach(var deliv in deliveryListByGroup)
            {
                deliv.Guid = "";
                uow.DeliveryRepo.AddOrUpdate(deliv);
            }
            return Json(deliveryListByGroup);
        }

        //---------------Delivery-Insp.------------------------------------------
        [HttpGet]
        public ActionResult DeliveryInspection(bool chooseSupplier = false, string supplierCode = "", string supplierName = "", string itemCodeDeliv="")
        {
            ViewBag.ChooseSuppliers = chooseSupplier;
            ViewBag.SupplierCode = supplierCode;
            ViewBag.SupplierName = supplierName;
            ViewBag.ItemCodeDeliv = itemCodeDeliv;
            return View();
        }
        [HttpPost] //DeliveryInspectionChooseSupplier
        public JsonResult DeliveryInspectionGetDeliveries(DeliveryViewModel filter) 
        {
            if(filter.Guid == "undefined" || filter.Guid == null)
            {
                filter.Guid = "";
            }

            List<Delivery> deliveryList = uow.DeliveryRepo.GetFilterListForInspection(filter).Include(x=>x.DeliveryItems)
                .OrderByDescending(x => x.Id).Skip(0).Take(50).ToList();

            List<DeliveryViewModel> deliveryVMList = new List<DeliveryViewModel>();

            foreach (var delivery in deliveryList)
            {
                decimal totaQty = delivery.DeliveryItems.Where(x => x.OperatorEntry).Sum(x => x.TotalQty);
                decimal totaLocatedQty = delivery.DeliveryItems.Where(x => x.OperatorEntry).Sum(x => x.TotalLocatedQty);

                DeliveryViewModel deliveryVM = delivery.CastToViewModel<DeliveryViewModel>();
                deliveryVM.LocateAssignedProgress = (int)(totaQty != 0 ? 100 * totaLocatedQty / totaQty : 0);
                deliveryVM.LocateProgress = 0; //TODO: progress lokowania

                deliveryVMList.Add(deliveryVM);
            }


            //List<DeliveryViewModel> deliveryVMList = uow.DeliveryRepo.GetFilterListForInspection(filter)
            //    .OrderByDescending(x => x.Id).Skip(0).Take(50).ToList<DeliveryViewModel>();

            //foreach (var deliveryVM in deliveryVMList)
            //{
            //    List<DeliveryItem> referenceDeliveryItems = uow.DeliveryItemRepo.GetReferenceDeliveryItemsIdsForOperatorEntries(deliveryVM.Id, deliveryVM.Guid);
            //    List<int> referenceDeliveryItemIds = referenceDeliveryItems.Select(x => x.Id).ToList();
            //    List<StockUnit> stockUnits = uow.StockUnitRepo.GetList()
            //        .Where(x => referenceDeliveryItemIds.Contains(x.ReferenceDeliveryItemId) && x.IsLocated == true).ToList();

            //    foreach (var stockUnit in stockUnits)
            //    {
            //        DeliveryItem delivItm = referenceDeliveryItems.Find(item => item.Id == stockUnit.ReferenceDeliveryItemId && item.IsLocated == false);
            //        if (delivItm != null)
            //        {
            //            delivItm.IsLocated = true;
            //            uow.DeliveryItemRepo.AddOrUpdate(delivItm);
            //            referenceDeliveryItems[referenceDeliveryItems.FindIndex(itm => itm.Id == delivItm.Id)].IsLocated = true;
            //        }

            //    }

            //    if (referenceDeliveryItemIds.Count != 0)
            //    {
            //        deliveryVM.LocateProgress = (int)((decimal)referenceDeliveryItems.Where(x => x.IsLocated == true).Count() / (decimal)referenceDeliveryItems.Count() * 100);
            //        deliveryVM.LocateAssignedProgress = (int)((decimal)referenceDeliveryItems.Where(x => x.IsLocationAssigned == true).Count() / (decimal)referenceDeliveryItems.Count() * 100);
            //    }

            //    if (deliveryVM.LocateAssignedProgress == 100 && deliveryVM.Status != EnumDeliveryStatus.Finished)
            //    {
            //        Delivery dd = uow.DeliveryRepo.GetById(deliveryVM.Id);
            //        dd.EnumDeliveryStatus = EnumDeliveryStatus.Finished;
            //        deliveryVM.Status = EnumDeliveryStatus.Finished; ;
            //        uow.DeliveryRepo.AddOrUpdate(dd);
            //    }
            //    // Do usunięcia przy następnym update od 26.08.2020
            //    //if(stockUnits.Count() != 0)
            //    //{
            //    //    delivViewModel.LocateProgress = (int)(stockUnits.Sum(x => x.CurrentQtyinPackage) / referenceDeliveryItems.Sum(x => x.TotalQty) * 100);
            //    //}
            //    //else
            //    //{
            //    //    delivViewModel.LocateProgress = 0;
            //    //}   
            //}

            return Json(deliveryVMList);
        }
        [HttpGet]
        public ActionResult DeliveryAdministratorInspection(int id)
        {
            DeliveryViewModel vm = new DeliveryViewModel();
            Delivery d = id > 0 ? uow.DeliveryRepo.GetById(id) : null;

            if (d != null)
            {
                vm = d.CastToViewModel<DeliveryViewModel>();
            }
            else
            {
                vm.DocumentDate = DateTime.Now;
            }
            return View(vm);
        }
        [HttpPost]
        public JsonResult DeliveryAdministratorInspectionData(int deliveryId)
        {
            int incomingWarehouseLocationId = uow.SystemVariableRepo.GetValueInt("IncomingWarehouseLocationId");

            var diList = uow.DeliveryItemRepo.GetByDeliveryId(deliveryId)
            .GroupJoin(db.StockUnits.Where(x => x.WarehouseLocationId == incomingWarehouseLocationId).DefaultIfEmpty(),
                    di => di.ItemWMSId,
                    su => su.ItemWMSId,
                    (di, su) => new { SU = su, DI = di })
            .SelectMany(
                x => x.SU.DefaultIfEmpty(),
                (di, su) => new DeliveryItemViewModel {
                    Id = di.DI.Id,
                    ItemWMSId = di.DI.ItemWMS.Id,
                    ItemCode = di.DI.ItemWMS.Item.Code,
                    ItemName = di.DI.ItemWMS.Item.Name,
                    QtyInPackage = di.DI.QtyInPackage,
                    UnitOfMeasure = di.DI.UnitOfMeasure,
                    WasPrinted = di.DI.WasPrinted,
                    NumberOfPackages = di.DI.NumberOfPackages,
                    OperatorEntry = di.DI.OperatorEntry,
                    AdminEntry = di.DI.AdminEntry,
                    TotalQty = di.DI.TotalQty,
                    RemainingQty = su != null? su.CurrentQtyinPackage : 0,
                    DeliveryId = deliveryId
                }
                //.Select(su => new { DI_ = di, SU_ = su })
                //x => x.SU.DefaultIfEmpty(),
                //(di1, su1) => new { DI_ = di1, SU_ = su1 }
            )
            .GroupBy(c => new
            {
                c.ItemWMSId,
                c.ItemCode,
                c.ItemName,
                c.QtyInPackage,
                c.UnitOfMeasure,
                c.WasPrinted,
                c.OperatorEntry,
                c.AdminEntry
                //c.DI_.QtyInPackage,
                //c.DI_.WasPrinted,
                //c.DI_.OperatorEntry
            }).Select(
                x => new DeliveryItemViewModel()
                {
                    Id = x.Max(itm => itm.Id),
                    ItemWMSId = x.Key.ItemWMSId,
                    ItemCode = x.Key.ItemCode,
                    ItemName = x.Key.ItemName,
                    QtyInPackage = x.Key.QtyInPackage,
                    UnitOfMeasure = x.Key.UnitOfMeasure,
                    WasPrinted = x.Key.WasPrinted,
                    NumberOfPackages = x.Sum(f => f.NumberOfPackages),
                    OperatorEntry = x.Key.OperatorEntry,
                    AdminEntry = !x.Key.OperatorEntry,
                    TotalQty = x.Sum(f => f.TotalQty),
                    RemainingQty = x.Sum(f => f != null? f.RemainingQty : 0),
                    DeliveryId = deliveryId
            }).OrderBy(x => x.ItemCode).ThenByDescending(x=>x.AdminEntry).ThenByDescending(x=>x.TotalQty).ToList();

            return Json(diList);
        }
        [HttpGet]
        public ActionResult DeliveryInspectionBlindCheck(int supplierId, int deliveryId = 0, string deliveryItemListGroupGuid="")
        {
            ViewBag.SupplierId = supplierId;
            ViewBag.DeliveryId = deliveryId;
            ViewBag.DeliveryItemListGroupGuid = deliveryItemListGroupGuid;

            return View();
        }
        [HttpPost]
        public JsonResult DeliveryInspectionBlindCheckAddItem(int deliveryId, string deliveryItemListGroupGuid, int supplierId, string itemCode, decimal numberOfPackages, decimal qtyInPackage)
        {
            Delivery d = new Delivery();
                
            if (deliveryId == 0)
            {
                d = uow.DeliveryRepo.GetGroupByGuid(deliveryItemListGroupGuid).FirstOrDefault();
            }
            else
            {
                d = uow.DeliveryRepo.GetById(deliveryId);
            }
            
            ItemWMS itemWMS = uow.ItemWMSRepo.GetByCode(itemCode);
            JsonModel jsonModel = new JsonModel();

            if (d != null && itemWMS != null && d.SupplierId == supplierId)
            {
                DeliveryItem deliveryItem = new DeliveryItem();
                deliveryItem.OperatorEntry = true;
                deliveryItem.ItemWMSId = itemWMS.Id;
                deliveryItem.DeliveryId = d.Id;
                deliveryItem.NumberOfPackages = (int)numberOfPackages;
                deliveryItem.QtyInPackage = qtyInPackage;
                deliveryItem.TotalQty = (int)numberOfPackages * qtyInPackage;
                uow.DeliveryItemRepo.Add(deliveryItem);

                if(d.EnumDeliveryStatus < EnumDeliveryStatus.Init)
                {
                    d.EnumDeliveryStatus = EnumDeliveryStatus.Init;
                    uow.DeliveryRepo.AddOrUpdate(d);
                }

                jsonModel.MessageType = JsonMessageType.success;
                jsonModel.Message = iLogisStatus.NoError.ToString();
            }
            else
            {
                jsonModel.MessageType = JsonMessageType.danger;
                
                if(itemWMS == null)
                    jsonModel.Message = iLogisStatus.ItemWMSNotFound.ToString();
                if(d == null)
                    jsonModel.Message = iLogisStatus.DeliveryNotFound.ToString();
                if(d != null && d.SupplierId != supplierId)
                    jsonModel.Message = iLogisStatus.DeliverySupplierNotCorrect.ToString();
            }
            
            return Json(jsonModel);
        }
        [HttpPost]
        public JsonResult DeliveryInspectionBlindCheckDeleteItem(int deliveryItemId)
        {
            DeliveryItem di = uow.DeliveryItemRepo.GetById(deliveryItemId);
            
            if (di != null)
            {
                uow.DeliveryItemRepo.Delete(di);
            }

            return Json(0);
        }
        [HttpGet]// Store keeper Scanning Itemes - DeliveryInspectionStoreKeeper
        public ActionResult DeliveryInspectionStoreKeeper()
        {
            return View();
        }
        [HttpGet]// Summary Scanning Items - DeliveryInspectionSummary
        public ActionResult DeliveryInspectionSummary(int supplierId, int deliveryId = 0, string deliveryItemListGroupGuid = "")
        {
            ViewBag.SupplierId = supplierId;
            ViewBag.DeliveryId = deliveryId;
            ViewBag.DeliveryItemListGroupGuid = deliveryItemListGroupGuid;
            return View();
        }
        [HttpPost]
        public JsonResult DeliveryInspectionSummaryData(int supplierId, int deliveryId = 0, string deliveryItemListGroupGuid = "")
        {
            DeliveryInspectionSummaryViewModel vm = new DeliveryInspectionSummaryViewModel();
            List<DeliveryItemViewModel> operatorEntries = new List<DeliveryItemViewModel>();
            List<DeliveryItemViewModel> adminEntries = new List<DeliveryItemViewModel>();
            Contractor supplier = uow.RepoContractor.GetById(supplierId);
            vm.SupplierId = supplierId;
            vm.SupplierName = supplier.Name;

            //TODO: tu jest powielony kod.

            if (deliveryId != 0)
            {
                Delivery d = uow.DeliveryRepo.GetById(deliveryId);
                vm.DeliveryId = d.Id;
                vm.DeliveryDocument = d.DocumentNumber;
                operatorEntries = uow.DeliveryItemRepo.GetGrouppedByItemCde(deliveryId, true).ToList<DeliveryItemViewModel>();
                adminEntries = uow.DeliveryItemRepo.GetGrouppedByItemCde(deliveryId, false).ToList<DeliveryItemViewModel>();
                d.EnumDeliveryStatus = EnumDeliveryStatus.Init;
                uow.DeliveryRepo.AddOrUpdate(d);
            }
            else
            {
                List<Delivery> deliveriegByGuidGroup = uow.DeliveryRepo.GetGroupByGuid(deliveryItemListGroupGuid);
                foreach(var delivery in deliveriegByGuidGroup)
                {
                    delivery.EnumDeliveryStatus = EnumDeliveryStatus.Init;
                    uow.DeliveryRepo.AddOrUpdate(delivery);
                }
                operatorEntries = uow.DeliveryItemRepo.GetGrouppedByItemCde(deliveryItemListGroupGuid, true).ToList<DeliveryItemViewModel>();
                adminEntries = uow.DeliveryItemRepo.GetGrouppedByItemCde(deliveryItemListGroupGuid, false).ToList<DeliveryItemViewModel>();
            }
            
            foreach (var ae in operatorEntries)
            {
                ae.TotalQtyDocument = adminEntries.Where(x => x.ItemWMSId == ae.ItemWMSId).Sum(x => x.TotalQtyDocument);
            }

            foreach (var ae in adminEntries)
            {
                if (!(operatorEntries.Select(x => x.ItemWMSId).Contains(ae.ItemWMSId)))
                {
                    operatorEntries.AddRange(adminEntries.Where(x => x.ItemWMSId == ae.ItemWMSId));
                }
            }


            if (operatorEntries.Count > 0)
            {
                foreach (var oe in operatorEntries)
                {
                    //List<StockUnit> stockUnits = new List<StockUnit>();
                    //List<int> deliveryItemIds = uow.DeliveryItemRepo.GetReferenceDeliveryItemsIdsForOperatorEntriesAndItemId(oe.ItemWMSId,oe.Id, oe.DeliveryGroupGuid).Select(x => x.Id).ToList();

                    //stockUnits = uow.StockUnitRepo.GetList().Where(x => deliveryItemIds.Contains(x.ReferenceDeliveryItemId)).ToList();
                    
                    //oe.TotalLocatedQty = stockUnits.Sum(x => x.InitialQty);
                    oe.LocateAssignedProgress = (int)((decimal)oe.TotalLocatedQty / (decimal)oe.TotalQty * 100);
                }
            }

            vm.DeliveryItems = operatorEntries;
           
            return Json(vm);
        }
        [HttpGet]//Localization items - DeliveryInspectionLocalization
        public ActionResult DeliveryInspectionLocalization()
        {
            return View();
        }
        [HttpGet]// Summary Scanning Items - DeliveryInspectionSummary
        public ActionResult DeliveryInspectionSummaryItem(string itemCode, int deliveryId = 0, string deliveryItemListGroupGuid = "")
        {
            ViewBag.ItemCode = itemCode;
            ViewBag.DeliveryId = deliveryId;
            ViewBag.DeliveryItemListGroupGuid = deliveryItemListGroupGuid;

            return View();
        }
        [HttpPost]
        public JsonResult DeliveryInspectionSummaryDataItem(string itemCode, int deliveryId = 0, string deliveryItemListGroupGuid = "")
        {
            DeliveryInspectionSummaryViewModel vm = new DeliveryInspectionSummaryViewModel();
            ItemWMS itemWMS = uow.ItemWMSRepo.GetByCode(itemCode);
            Delivery delivery = new Delivery();
            List<DeliveryItemViewModel> operatorEntries = new List<DeliveryItemViewModel>();
            decimal adminEntriesSum = 0.0M;
            if (deliveryId != 0)
            {
                delivery = uow.DeliveryRepo.GetById(deliveryId);
                operatorEntries = uow.DeliveryItemRepo.GetList()
                .Where(x => x.DeliveryId == deliveryId && x.ItemWMS.Item.Code == itemCode && x.OperatorEntry == true && x.Deleted == false)
                .ToList<DeliveryItemViewModel>();

                adminEntriesSum = uow.DeliveryItemRepo.GetList()
                    .Where(x => x.DeliveryId == deliveryId && x.ItemWMS.Item.Code == itemCode && x.AdminEntry == true && x.Deleted == false)
                    .Select(x => x.TotalQty)
                    .DefaultIfEmpty(0)
                    .Sum();
            }
            else
            {
                List<Delivery> deliveryList = uow.DeliveryRepo.GetGroupByGuid(deliveryItemListGroupGuid);
                operatorEntries = uow.DeliveryItemRepo.GetList()
                .Where(x => x.Delivery.Guid == deliveryItemListGroupGuid && x.ItemWMS.Item.Code == itemCode && x.OperatorEntry == true && x.Deleted == false)
                .ToList<DeliveryItemViewModel>();

                adminEntriesSum = uow.DeliveryItemRepo.GetList()
                    .Where(x => x.Delivery.Guid == deliveryItemListGroupGuid && x.ItemWMS.Item.Code == itemCode && x.AdminEntry == true && x.Deleted == false)
                    .Select(x => x.TotalQty)
                    .DefaultIfEmpty(0)
                    .Sum();
                delivery = deliveryList[0];
            }
            
            

            

            //decimal? adminEntriesSum = query != null ? (decimal)query : 0;

            vm.SupplierId = delivery.SupplierId;
            vm.SupplierName = delivery.Supplier.Name;
            vm.DeliveryId = delivery.Id;
            vm.DeliveryDocument = delivery.DocumentNumber;
            vm.SelectedItemDocumentQty = (decimal)adminEntriesSum;
            vm.SelectedItemCode = itemWMS.Item.Code;
            vm.SelectedItemName = itemWMS.Item.Name;

            //if (operatorEntries.Count > 0) {
            //}

            vm.DeliveryItems = operatorEntries;

            return Json(vm);
        }


        [HttpPost]
        public JsonResult DeliveryInspectionSummarySetStatus(int deliveryId = 0, string deliveryItemListGroupGuid = "")
        {
            List<DeliveryItemViewModel> operatorEntries = new List<DeliveryItemViewModel>();
            List<DeliveryItemViewModel> adminEntries = new List<DeliveryItemViewModel>();
            if (deliveryId != 0)
            {
                operatorEntries = uow.DeliveryItemRepo.GetGrouppedByItemCde(deliveryId, true).ToList<DeliveryItemViewModel>();
                adminEntries = uow.DeliveryItemRepo.GetGrouppedByItemCde(deliveryId, false).ToList<DeliveryItemViewModel>();
            }
            else
            {
                operatorEntries = uow.DeliveryItemRepo.GetGrouppedByItemCde(deliveryItemListGroupGuid, true).ToList<DeliveryItemViewModel>();
                adminEntries = uow.DeliveryItemRepo.GetGrouppedByItemCde(deliveryItemListGroupGuid, false).ToList<DeliveryItemViewModel>();
            }
            
            int errorCount = 0;
            foreach (var ae in operatorEntries)
            {
                ae.TotalQtyDocument = adminEntries.Where(x => x.ItemWMSId == ae.ItemWMSId).Sum(x => x.TotalQtyDocument);
                errorCount += ae.TotalQtyDocument != ae.TotalQtyFound ? 1 : 0;
            }
            EnumDeliveryStatus status = GetDeliveryStatus(errorCount, operatorEntries.Count());

            if(deliveryId != 0)
            {
                Delivery delivery = uow.DeliveryRepo.GetById(deliveryId);
                delivery.EnumDeliveryStatus = status;
                uow.DeliveryRepo.AddOrUpdate(delivery);
            }
            else
            {
                List<Delivery> deliveryLsit = uow.DeliveryRepo.GetGroupByGuid(deliveryItemListGroupGuid);
                foreach(var delivery in deliveryLsit)
                {
                    delivery.EnumDeliveryStatus = status;
                    uow.DeliveryRepo.AddOrUpdate(delivery);
                }
            }
            return Json(status);
        }

        private EnumDeliveryStatus GetDeliveryStatus(int errors,int operatorEntriesCount)
        {
            EnumDeliveryStatus status = EnumDeliveryStatus.Created;

            if (operatorEntriesCount > 0)
            {
                status = EnumDeliveryStatus.Init;

                if (errors == 0)
                {
                    status = EnumDeliveryStatus.AfterInspection;
                }
                else if (errors > 0)
                {
                    status = EnumDeliveryStatus.AfterInspectionWithProlem;
                }
            }
            return status;
        }


        private EnumDeliveryStatus GetDeliveryStatus(List<DeliveryItemViewModel> operatorEntries)
        {
            EnumDeliveryStatus status;
            int totalOperatorEntries = operatorEntries.Count();
            int inspectedWithProblem = operatorEntries.Where(x => (x.TotalQtyDocument - x.TotalQtyFound) > 0).Count();
            int inspectedWithoutProblem = operatorEntries.Where(x => (x.TotalQtyDocument - x.TotalQtyFound) == 0).Count();
            if (totalOperatorEntries == inspectedWithoutProblem)
            {
                status = EnumDeliveryStatus.AfterInspection;
            }
            else if (inspectedWithProblem >= 1)
            {
                status = EnumDeliveryStatus.AfterInspectionWithProlem;
            }
            else
            {
                status = EnumDeliveryStatus.Init;
            }
            return status;
        }

        private EnumDeliveryStatus GetDeliveryStatus(List<DeliveryItemViewModel> operatorEntries, List<DeliveryItemViewModel> adminEntries)
        {
            EnumDeliveryStatus status;

            int totalOperatorEntries = operatorEntries.Count();
            int totalAdminEntries = adminEntries.Count();
            int inspectedWithProblem = operatorEntries.Where(x => (x.TotalQtyDocument - x.TotalQtyFound) > 0).Count();
            int inspectedWithoutProblem = operatorEntries.Where(x => (x.TotalQtyDocument - x.TotalQtyFound) == 0).Count();
            if (totalOperatorEntries == inspectedWithoutProblem)
            {
                status = EnumDeliveryStatus.AfterInspection;
            }
            else if (inspectedWithProblem >= 1)
            {
                status = EnumDeliveryStatus.AfterInspectionWithProlem;
            }
            else
            {
                status = EnumDeliveryStatus.Init;
            }
            return status;
        }

        [HttpGet]
        public ActionResult DeliveryInspectionSummaryItemLocate(string itemCode, int deliveryId, string deliveryItemListGroupGuid = "")
        {
            ViewBag.ItemCode = itemCode;
            ViewBag.DeliveryId = deliveryId;
            ViewBag.DeliveryItemListGroupGuid = deliveryItemListGroupGuid;
            return View();
        }
        [HttpPost]
        public JsonResult DeliveryInspectionSummaryItemLocateGetData(string itemCode, int deliveryId, string deliveryItemListGroupGuid = "")
        {
            DeliveryInspectionSummaryViewModel vm = new DeliveryInspectionSummaryViewModel();

            ItemWMS itemWMS = uow.ItemWMSRepo.GetByCode(itemCode);
            List<Delivery> deliveryList = new List<Delivery>();
            Delivery delivery = new Delivery();
            List<DeliveryItemViewModel> operatorEntries = new List<DeliveryItemViewModel>();
            decimal adminEntriesSum = 0.0M;

            if (deliveryId != 0)
            {
                delivery = uow.DeliveryRepo.GetById(deliveryId);
                
                operatorEntries = uow.DeliveryItemRepo.GetList()
                .Where(x => x.DeliveryId == deliveryId && x.ItemWMS.Item.Code == itemCode && x.OperatorEntry == true && x.Deleted == false)
                .ToList<DeliveryItemViewModel>();

                adminEntriesSum = uow.DeliveryItemRepo.GetList()
                    .Where(x => x.DeliveryId == deliveryId && x.ItemWMS.Item.Code == itemCode && x.AdminEntry == true && x.Deleted == false)
                    .Select(x => x.TotalQty)
                    .DefaultIfEmpty(0)
                    .Sum();
            }
            else
            {
                deliveryList = uow.DeliveryRepo.GetGroupByGuid(deliveryItemListGroupGuid);
                delivery = deliveryList[0];

                operatorEntries = uow.DeliveryItemRepo.GetList()
                .Where(x => x.Delivery.Guid == deliveryItemListGroupGuid && x.ItemWMS.Item.Code == itemCode && x.OperatorEntry == true && x.Deleted == false)
                .ToList<DeliveryItemViewModel>();

                adminEntriesSum = uow.DeliveryItemRepo.GetList()
                    .Where(x => x.Delivery.Guid == deliveryItemListGroupGuid && x.ItemWMS.Item.Code == itemCode && x.AdminEntry == true && x.Deleted == false)
                    .Select(x => x.TotalQty)
                    .DefaultIfEmpty(0)
                    .Sum();
            }



            //List<int> referenceDeliveryItemsIds = operatorEntries.Select(x => x.Id).ToList();
            //List<StockUnit> stockUnits = uow.StockUnitRepo.GetList().Where(x => referenceDeliveryItemsIds.Contains(x.ReferenceDeliveryItemId)).ToList();
            
            vm.SupplierId = delivery.SupplierId;
            vm.SupplierName = delivery.Supplier.Name;
            vm.DeliveryId = delivery.Id;
            vm.DeliveryDocument = delivery.DocumentNumber;
            vm.DeliveriesByGroup = deliveryList;
            vm.SelectedItemDocumentQty = adminEntriesSum;
            vm.SelectedItemCode = itemWMS.Item.Code;
            vm.SelectedItemName = itemWMS.Item.Name;

            if (operatorEntries.Count > 0)
            {
                foreach(var oe in operatorEntries)
                {
                    decimal totalLocatedQty = oe.TotalLocatedQty;

                    for (int i = 0; i < oe.NumberOfPackages; i++)
                    {
                        //StockUnit su = stockUnits.FirstOrDefault(x => x.InitialQty == oe.QtyInPackage);

                        vm.DeliveryItems.Add(new DeliveryItemViewModel()
                        {
                            Id = oe.Id,
                            DeliveryId = oe.DeliveryId,
                            AdminEntry = false,
                            OperatorEntry = true,
                            Deleted = oe.Deleted,
                            ItemCode = oe.ItemCode,
                            ItemName = oe.ItemName,
                            ItemWMSId = oe.ItemWMSId,
                            NumberOfPackages = 1,
                            QtyInPackage = oe.QtyInPackage,
                            UnitOfMeasure = oe.UnitOfMeasure,
                            RemainingQty = oe.QtyInPackage,
                            SupplierId = oe.SupplierId,
                            TotalQty = oe.QtyInPackage,
                            TotalQtyDocument = oe.TotalQtyDocument,
                            TotalQtyFound = oe.QtyInPackage,
                            WasPrinted = oe.WasPrinted,
                            //TotalQuantityLocated = su != null ? su.InitialQty : 0,
                            TotalLocatedQty = Math.Min(totalLocatedQty, oe.QtyInPackage),
                            IsLocationAssigned = oe.IsLocationAssigned,
                            IsLocated = oe.IsLocated
                        });

                        //stockUnits.Remove(su);
                        totalLocatedQty -= oe.QtyInPackage;
                    }

                }
            }

            return Json(vm);
        }
        
        [HttpPost]
        public JsonResult GetPackageItems(string itemCode, decimal maxQtyPerPackage, int packageId)
        {
            ItemWMS itemWMS = uow.ItemWMSRepo.Get(null, null, itemCode);
            int itemId = itemWMS != null ? itemWMS.Id : 0;
            string itemName = itemWMS != null ? itemWMS.Item.Name : "";
            UnitOfMeasure uom = itemWMS != null ? itemWMS.Item.UnitOfMeasure : UnitOfMeasure.Undefined;

            IQueryable <PackageItem> piList = uow.PackageItemRepo
                .GetList(itemWMS, maxQtyPerPackage, 0, 0, packageId, 0)
                .OrderByDescending(x => x.Id);
            
            List<PackageItemViewModel> pivmList = piList.ToList<PackageItemViewModel>();

            return Json(new { PackageItemViewModelList = pivmList, itemWMS = new { Id = itemId, Name = itemName, UnitOfMeasure = uom } });
        }
    }
}