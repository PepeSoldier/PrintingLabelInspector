using _MPPL_WEB_START.Areas.iLOGIS.ViewModels.PickingList;
using MDL_BASE.Models.MasterData;
using MDLX_CORE.ComponentCore.Entities;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_iLOGIS.ComponentWMS.Enums;
using MDL_WMS.ComponentConfig.UnitOfWorks;
//using MDLX_CORE.ComponentCore.Entities;
using MDLX_MASTERDATA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _MPPL_WEB_START.Areas.iLOGIS.ViewModels;
using Microsoft.AspNet.Identity;
using MDL_iLOGIS.ComponentWMS.ViewModels;

namespace _MPPL_WEB_START.Areas.iLOGIS.Controllers
{
    //[Authorize]
    public class DeliveryListController : Controller
    {
        int version = 24;
        readonly IDbContextiLOGIS db;
        readonly UnitOfWork_iLogis uow;
        readonly int woHoursFromLimit = -192;
        readonly int woHoursToLimit = 240;


        public DeliveryListController(IDbContextiLOGIS db)
        {
            ViewBag.Skin = "nasaSkin";
            this.db = db;
            uow = new UnitOfWork_iLogis(db);
        }

        public ActionResult Index(int trainId = 0)
        {
            if (trainId <= 0)
            {
                return Redirect("DeliveryList/TransporterSelector");
            }

            Transporter train = uow.TransporterRepo.GetById_AsNoTracking(trainId);
            ViewBag.TrainName = train.Name;
            ViewBag.TrainId = train.Id;
            ViewBag.LoopQty = train.LoopQty;
            return View("DeliveryList");
        }
        public ActionResult DeliveryList(int trainId = 0)
        {
            if(trainId <= 0)
            {
                return Redirect("TransporterSelector");
            }

            Transporter train = uow.TransporterRepo.GetById(trainId);
            ViewBag.TrainName = train.Name;
            ViewBag.TrainId = train.Id;
            ViewBag.LoopQty = train.LoopQty;
            return View();
        }
        public ActionResult DeliveryListMultiLines(int trainId)
        {
            Transporter train = uow.TransporterRepo.GetById_AsNoTracking(trainId);
            ViewBag.TrainName = train.Name;
            ViewBag.TrainId = train.Id;
            ViewBag.LoopQty = train.LoopQty;
            return View();
        }
        public ActionResult TransporterSelector()
        {
            List<Transporter> trains = uow.TransporterRepo.GetList().Where(x => x.Type == EnumTransporterType.Train).ToList();
            ViewBag.Trains = trains;
            return View();
        }

        public JsonResult CalculateDemandMultiLines(int trainId)
        {
            Transporter train = uow.TransporterRepo.GetById_AsNoTracking(trainId);
            ReplaceResourcesMain(train);
            string[] dedicatedResourcesArray = train.DedicatedResourcesArray;

            //zadeklaruj listę itemów przechowujących zapotrzebowanie
            List<DeliveryListWorkorderViewModel> woList = _DeleteUnneededWosAndGetListOfWorkorders(train, 32);
            List<DeliveryListItemViewModel> dlItemList = new List<DeliveryListItemViewModel>();
            //List<DeliveryListItemViewModel> dlItemsToBeDeliveredList = new List<DeliveryListItemViewModel>();

            DateTime loopDateFrom = DateTime.Now;
            DateTime loopDateTo = DateTime.Now;

            int fakeId = 1;

            foreach (string dedicatedResourceName in dedicatedResourcesArray)
            {
                //pobierz itemy dla zleceń na obecnej linii
                List<DeliveryListWorkorderViewModel> woOfResourceList = woList.Where(x => x.LineName == dedicatedResourceName).OrderBy(x=>x.StartTime).ToList();
                int[] woIds = woOfResourceList.Select(x => x.Id).ToArray();
                DeliveryListWorkorderItemsListViewModel itemsVM = _GetItemsForWorkorders(trainId, woIds);
                
                List<DeliveryListWorkorderViewModel> woForCalculationList = _GetLimitedToLoopQtyWorkorders(train.LoopQty, woOfResourceList);

                foreach (DeliveryListWorkorderItemsViewModel woItems in itemsVM.WorkorderItems)
                {
                    DeliveryListWorkorderViewModel woConsidered = woForCalculationList.FirstOrDefault(x => x.Id == woItems.WoId);

                    if (woConsidered != null)
                    {
                        foreach (DeliveryListItemViewModel dlItem in woItems.Items)
                        {
                            //policz zapotrzebowanie dla każdego itema
                            //--przelicz pozycję limitera
                            //--wez sumę potrzenych sztuk aż do limitera
                            //złącz wyliczenia do listy itemów
                            //--wyszukaj w itemList obecnego itema i dodaj wyliczone wartości

                            DeliveryListItemViewModel dlItemTemp = dlItemList.FirstOrDefault(x => x.Code == dlItem.Code & x.WorkstationId == dlItem.WorkstationId);
                            if (dlItemTemp == null)
                            {

                                dlItemList.Add(new DeliveryListItemViewModel()
                                {
                                    Id = dlItem.Id,
                                    ItemWMSId = dlItem.ItemWMSId,
                                    Code = dlItem.Code,
                                    Name = dlItem.Name,
                                    Workstation = dlItem.Workstation,
                                    WorkstationName = dlItem.WorkstationName,
                                    WorkstationId = dlItem.WorkstationId,
                                    WorkstationProductsFromIn = dlItem.WorkstationProductsFromIn,
                                    WorkstationProductsFromOut = dlItem.WorkstationProductsFromOut,
                                    QtyRequested = (int)(woConsidered.Qty * dlItem.BomQty),
                                    QtyDelivered = dlItem.QtyDelivered - (int)(woConsidered.QtyIn * dlItem.BomQty),
                                    QtyUsed = Math.Max(dlItem.QtyUsed - (int)(woConsidered.QtyIn * dlItem.BomQty), 0),
                                    QtyPerPackage = dlItem.QtyPerPackage,
                                    BomQty = dlItem.BomQty,
                                    CoveredSeconds = (int) (woConsidered.ProcessingTime * dlItem.QtyDelivered / woConsidered.QtyTotal),
                                    MaxCoveredTime = dlItem.QtyDelivered > 0? woConsidered.StartTime.AddSeconds(woConsidered.ProcessingTime * (double)dlItem.QtyDelivered / (double)woConsidered.QtyTotal) : DateTime.Now,
                                    IsSubstituteData = dlItem.IsSubstituteData
                                }); ;
                                fakeId++;
                            }
                            else
                            {
                                dlItemTemp.QtyRequested += (int)(woConsidered.Qty * dlItem.BomQty);
                                dlItemTemp.QtyDelivered += dlItem.QtyDelivered - (int)(woConsidered.QtyIn * dlItem.BomQty);
                                dlItemTemp.QtyUsed += Math.Max(dlItem.QtyUsed - (int)(woConsidered.QtyIn * dlItem.BomQty),0);
                                dlItemTemp.CoveredSeconds += (int)(woConsidered.ProcessingTime * dlItem.QtyDelivered / woConsidered.QtyTotal);
                                dlItemTemp.MaxCoveredTime = dlItem.QtyDelivered > 0? woConsidered.StartTime.AddSeconds(woConsidered.ProcessingTime * (double)dlItem.QtyDelivered / (double)woConsidered.QtyTotal) : dlItemTemp.MaxCoveredTime;
                            }

                            DateTime dateTemp = woConsidered.StartTime.AddSeconds(woConsidered.ProcessingTime * woConsidered.Qty / woConsidered.QtyTotal);
                            loopDateTo = loopDateTo < dateTemp ? dateTemp : loopDateTo;
                        }
                    }
                }
            }

            return Json(new { list = dlItemList, loopDateFrom, loopDateTo });
        }

        private List<DeliveryListWorkorderViewModel> _GetLimitedToLoopQtyWorkorders(int loopQty, List<DeliveryListWorkorderViewModel> woOfResourceList)
        {
            List<DeliveryListWorkorderViewModel> woForCalculationList = new List<DeliveryListWorkorderViewModel>();
            
            int i = 0;
            int remainingQty = loopQty;
            while (remainingQty > 0 && i < woOfResourceList.Count)
            {

                int woRemainQty = woOfResourceList[i].Qty - woOfResourceList[i].QtyIn;
                DeliveryListWorkorderViewModel woTemp = woOfResourceList[i];

                if (woRemainQty < remainingQty)
                {
                    //uwzględnij zlecenie i szukaj dalej
                    woForCalculationList.Add(woTemp);
                    woTemp.Qty = woRemainQty;
                    remainingQty -= woTemp.Qty;
                }
                else
                {
                    //koniec szykania, wez czesc zlecenia
                    woForCalculationList.Add(woTemp);
                    woTemp.Qty = remainingQty;
                    remainingQty -= woTemp.Qty;
                }
                i++;
            }

            return woForCalculationList;
        }

        //Confirm -> ItemManage
        public ActionResult ItemManage(int trainId, int itemWMSId, int workstationId)
        {
            ViewBag.ItemWMSId = itemWMSId;
            ViewBag.WorkstationId = workstationId;
            ViewBag.TrainId = trainId;
            return View(itemWMSId);
        }
        //ConfirmPartially -> ItemManageManualQty
        public ActionResult ItemManageManualQty(int trainId, int itemWMSId, int workstationId, int workstationQty)
        {
            ViewBag.ItemWMSId = itemWMSId;
            ViewBag.WorkstationId = workstationId;
            ViewBag.WorkstationQty = workstationQty;
            return View();
        }
        //WorkstationInventory -> ItemManageInventory
        public ActionResult ItemManageInventory(int trainId, int itemWMSId, int workstationId, int workstationQty)
        {
            ViewBag.ItemWMSId = itemWMSId;
            ViewBag.WorkstationId = workstationId;
            ViewBag.WorkstationQty = workstationQty;
            return View();
        }
        [HttpPost]
        public JsonResult GetItemDetails(int trainId, int itemWMSId, int workstationId)
        {
            List<DeliveryListItem> dliList = uow.DeliveryListItemRepo.GetList()
                .Where(x => x.ItemWMSId == itemWMSId && (workstationId == 0 || x.WorkstationId == workstationId))
                .ToList();

            decimal totalQtyUsed = 0;
            if (dliList.Count > 0)
            {
                DeliveryListItem dl = dliList.FirstOrDefault();
                decimal totalQtyRequested = dliList.Sum(x => x.QtyRequested);

                if (dl.Workstation != null)
                {
                    int prodIn = dliList.Sum(x => x.WorkOrder.CounterProductsIn);
                    int prodOut = dliList.Sum(x => x.WorkOrder.CounterProductsOut);
                    totalQtyUsed = Math.Max(prodIn - dl.Workstation.ProductsFromOut, 0);
                    totalQtyUsed = Math.Max(prodOut + dl.Workstation.ProductsFromOut, totalQtyRequested);
                }
                else {
                    totalQtyUsed = dliList.Sum(x => x.WorkOrder.CounterProductsIn);
                }

                var obj = new
                {
                    ItemWMSId = itemWMSId,
                    ItemCode = dl.ItemWMS.Item.Code,
                    ItemName = dl.ItemWMS.Item.Name,
                    WorkstationId = workstationId,
                    WorkstationName = dl.Workstation != null ? dl.Workstation.Name : "-",
                    TotalQtyRequested = totalQtyRequested,
                    TotalQtyDelivered = dliList.Sum(x => Math.Min(x.QtyDelivered,(x.WorkOrder.StartDate > DateTime.Now)? x.WorkOrder.QtyRemain : x.QtyDelivered)),
                    TotalQtyUsed = totalQtyUsed,
                    //TotalQtyUsed = dliList.Sum(x => x.WorkOrder.CounterProductsIn),
                    TotalQtyOnWorkstation = 0,
                    QtyPerPackage = dl.QtyPerPackage,
                };

                return Json(obj);
            }
            else
            {
                return Json(null);
            }
        }
        [HttpPost]
        public JsonResult ChangeItemQty(int trainId, int itemWMSId, int workstationId, int qty)
        {
            List<DeliveryListItem> dliList = uow.DeliveryListItemRepo.GetListByItemIdAndWorkstationId(itemWMSId, workstationId, trainId).ToList();

            if (qty >= 0)
            {
                IncreaseQtyDelivered(qty, dliList);
            }
            else
            {
                DecreaseQtyDelivered(qty, dliList);
            }


            if (dliList.Count > 0)
            {
                return PrepareItemData(dliList, itemWMSId, workstationId);
            }
            else
            {
                return Json(null);
            }
        }
        [HttpPost]
        public JsonResult ChangeItemQtyMultiLines(int trainId, int itemId, int workstationId, int qty, int[] linesIds)
        {
            //List<DeliveryListItem> dliList = new List<DeliveryListItem>();
            List<DeliveryListItem> dliList = uow.DeliveryListItemRepo.GetListByItemIdAndWorkstationId(itemId, workstationId, trainId).ToList();
            List<DeliveryListItem> dliListTemp = new List<DeliveryListItem>();
            int dliListCount = dliList.Count;
            int remainQty = Math.Abs(qty);
            int qtyToChange = 0;

            int[] noWorkordersCount = new int[linesIds.Length];
            DateTime[] CoverageTimes = new DateTime[linesIds.Length];
            
            while(remainQty > 0 && noWorkordersCount.Sum(x=>x) < linesIds.Length)
            {
                CalculateCoverageTimesForLines(linesIds, dliList, CoverageTimes);

                qtyToChange = Math.Min(20, remainQty);

                if (qty >= 0)
                {
                    int minValIndex = GetMinCoverageTimeIndex(CoverageTimes);
                    dliListTemp = dliList.Where(x => x.WorkOrder.LineId == linesIds[minValIndex]).ToList();
                    if (dliListTemp.Count > 0)
                    {
                        IncreaseQtyDelivered(qtyToChange, dliListTemp);
                    }
                    else
                    {
                        noWorkordersCount[minValIndex] = 1;
                    }
                }
                else
                {
                    int maxValIndex = GetMaxCoverageTimeIndex(CoverageTimes);
                    dliListTemp = dliList.Where(x => x.WorkOrder.LineId == linesIds[maxValIndex]).ToList();
                    if (dliListTemp.Count > 0)
                    {
                        DecreaseQtyDelivered(qtyToChange * -1, dliListTemp);
                    }
                    else{
                        noWorkordersCount[maxValIndex] = 1;
                    }
                }

                remainQty -= qtyToChange;
            }


            if (dliListCount > 0)
            {
                return PrepareItemData(dliList, itemId, workstationId);
            }
            else
            {
                return Json(null);
            }
        }

        private void CalculateCoverageTimesForLines(int[] linesIds, List<DeliveryListItem> dliList, DateTime[] CoverageTimes)
        {
            int i = 0;
            foreach (int lineId in linesIds)
            {
                var x1 = dliList.Where(x => x.QtyDelivered < x.QtyRequested && x.WorkOrder.LineId == lineId).OrderBy(x => x.WorkOrder.StartDate).FirstOrDefault();
                CoverageTimes[i] = x1 != null ? x1.WorkOrder.StartDate
                    .AddSeconds((x1.WorkOrder.EndDate - x1.WorkOrder.StartDate).TotalSeconds * (double)x1.QtyDelivered / (double)x1.QtyRequested) : DateTime.Now.AddYears(1);
                i++;
            }
        }
        private int GetMinCoverageTimeIndex(DateTime[] CoverageTimes)
        {
            int minValIndex = 0;
            for (int i = 1; i < CoverageTimes.Length; i++)
            {
                if (CoverageTimes[i] < CoverageTimes[i - 1])
                {
                    minValIndex = i;
                }
            }

            return minValIndex;
        }
        private int GetMaxCoverageTimeIndex(DateTime[] CoverageTimes)
        {
            int maxValIndex = 0;
            for (int i = 1; i < CoverageTimes.Length; i++)
            {
                if (CoverageTimes[i] > CoverageTimes[i - 1])
                {
                    maxValIndex = i;
                }
            }

            return maxValIndex;
        }

        private void IncreaseQtyDelivered(decimal qty, List<DeliveryListItem> dliList)
        {
            decimal qtyToAdd = 0;
            decimal woRemainingQtyToDeliver = 0;
            int i = 0;
            while (qty > 0 && i < dliList.Count)
            {
                woRemainingQtyToDeliver = dliList[i].QtyRequested - dliList[i].QtyDelivered;

                if (woRemainingQtyToDeliver > 0)
                {
                    qtyToAdd = Math.Min(qty, woRemainingQtyToDeliver);
                    qty = qty - qtyToAdd;
                    dliList[i].QtyDelivered += qtyToAdd;
                    uow.DeliveryListItemRepo.AddOrUpdate(dliList[i]);
                }
                i++;
            }
        }
        private void DecreaseQtyDelivered(decimal qty, List<DeliveryListItem> dliList)
        {
            decimal qtyToRemove = 0;
            decimal woQtyDelivered = 0;
            int i = dliList.Count-1;
            qty = Math.Abs(qty);
            while (qty > 0 && i >= 0)
            {
                woQtyDelivered = dliList[i].QtyDelivered;

                if (woQtyDelivered > 0)
                {
                    qtyToRemove = Math.Min(qty, woQtyDelivered);
                    qty = qty - qtyToRemove;
                    dliList[i].QtyDelivered -= qtyToRemove;
                    uow.DeliveryListItemRepo.AddOrUpdate(dliList[i]);
                }
                i--;
            }
        }

        private JsonResult PrepareItemData(List<DeliveryListItem> dliList, int itemWMSId, int workstationId)
        {
            DeliveryListItem dl = dliList.FirstOrDefault();
            var obj = new
            {
                ItemWMSId = itemWMSId,
                ItemCode = dl.ItemWMS.Item.Code,
                ItemName = dl.ItemWMS.Item.Name,
                WorkstationId = workstationId,
                WorkstationName = dl.Workstation != null ? dl.Workstation.Name : "-",
                TotalQtyRequested = dliList.Sum(x => x.QtyRequested),
                TotalQtyDelivered = dliList.Sum(x => x.QtyDelivered),
                TotalQtyUsed = dliList.Sum(x => x.QtyUsed),
                TotalQtyOnWorkstation = 0,
                QtyPerPackage = dl.QtyPerPackage,
                Workorders = dliList.Select(x => new { x.WorkOrderId, x.QtyDelivered }),
            };

            return Json(obj);
        }

        private void ReplaceResourcesMain(Transporter train)
        {
            List<string> dedicatedResources = new List<string>();
            foreach (string s in train.DedicatedResourcesArray) { dedicatedResources.Add(ReplaceResources(s)); }
            train.DedicatedResources = string.Join(",", dedicatedResources.Distinct());
        }
        private string ReplaceResources(string resourceName)
        {
            switch (resourceName)
            {
                case "PKG": return "101,103,104";
                case "PKD": return "101,103,104";
                case "PKF": return "101,103,104";
                default: return resourceName;
            }
        }

        [HttpPost]
        public JsonResult GetWorkorders(int trainId)
        {
            Transporter train = uow.TransporterRepo.GetById_AsNoTracking(trainId);
            ReplaceResourcesMain(train);
            List<DeliveryListWorkorderViewModel> woList = _DeleteUnneededWosAndGetListOfWorkorders(train, 16);

            return Json(new { woList, version });
        }

        private List<DeliveryListWorkorderViewModel> _DeleteUnneededWosAndGetListOfWorkorders(Transporter train, int woCount)
        {
            DateTime from = DateTime.Now.AddHours(woHoursFromLimit);
            DateTime to = DateTime.Now.AddHours(woHoursToLimit);

            int resourcesCount = train.DedicatedResourcesArray.Count();

            DeleteFinishedWorkordersFromDeliveryLists(train.Id);
            DeleteDeletedWorkordersFromDeliveryLists(train.Id);

            List<DeliveryListWorkorderViewModel> woList = uow.ProductionOrderRepo.GetByTimeRangeAndLine(from, to, train.DedicatedResourcesArray)
                .Where(x => (resourcesCount < 3 && x.CounterProductsOut <= x.QtyPlanned - 3) || (resourcesCount >= 3 && x.CounterProductsIn < x.QtyPlanned))
                //.Where(x => x.CounterProductsOut <= x.QtyPlanned-3)
                .Take(woCount)
                .ToList()
                .Select(x => new DeliveryListWorkorderViewModel()
                {
                    Id = x.Id,
                    ItemCode = x.Pnc.Code,
                    ItemName = x.Pnc.Name,
                    Number = x.OrderNumber,
                    Qty = (x.QtyPlanned - x.QtyProducedInPast), //x.StartDate > DateTime.Now ? x.QtyRemain.ToString() : x.QtyPlanned.ToString(),
                    QtyTotal = (x.QtyPlanned), //x.StartDate > DateTime.Now ? x.QtyRemain.ToString() : x.QtyPlanned.ToString(),
                    StartTime = x.StartDate,
                    StartTimeStr = x.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    EndTime = x.EndDate,
                    EndTimeStr = x.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    ProcessingTime = (int)(x.EndDate - x.StartDate).TotalSeconds,
                    QtyIn = x.CounterProductsIn - x.QtyProducedInPast,
                    QtyOut = x.CounterProductsOut - x.QtyProducedInPast,
                    LastScanDate = x.LastProductIn,
                    LineId = x.LineId,
                    LineName = x.Line.Name,
                    Notice = x.Notice
                }).OrderBy(x=>x.StartTime).ToList();

            DeleteOutOfRangeWorkordersFromDeliveryList(train.Id, woList.Select(x => x.Id).ToList());
            return woList;
        }

        [HttpPost]
        public JsonResult GetItemsForWorkorders(int trainId, int[] woIds)
        {
            DeliveryListWorkorderItemsListViewModel vm = _GetItemsForWorkorders(trainId, woIds);
            return Json(vm);
        }

        private DeliveryListWorkorderItemsListViewModel _GetItemsForWorkorders(int trainId, int[] woIds)
        {
            DeliveryListWorkorderItemsListViewModel vm = new DeliveryListWorkorderItemsListViewModel();
            vm.WorkorderItems = new List<DeliveryListWorkorderItemsViewModel>();
            List<DeliveryListItemViewModel> items;
            if (woIds != null)
            {

                foreach (int woId in woIds)
                {
                    items = GetOrCreateDeliveryListItems(trainId, woId);

                    int totalItems = items.Count;
                    int substituteData = items.Count(x => x.IsSubstituteData);

                    vm.WorkorderItems.Add(new DeliveryListWorkorderItemsViewModel
                    {
                        WoId = woId,
                        Items = items,
                        DataStatus = substituteData == 0 ? DeliveryListDataStatus.DataOK : substituteData >= totalItems ? DeliveryListDataStatus.DataSubstitute : DeliveryListDataStatus.DataFixed
                    });
                }
            }

            return vm;
        }

        private List<DeliveryListItemViewModel> GetOrCreateDeliveryListItems(int trainId, int woId)
        {
            List<DeliveryListItemViewModel> items = GetDeliveryListItems(trainId, woId);
            if (items.Count == 0)
            {
                CreateNewDeliveryList(trainId, woId);
                items = GetDeliveryListItems(trainId, woId);
            }

            return items;
        }
        private List<DeliveryListItemViewModel> GetDeliveryListItems(int trainId, int woId)
        {
            return uow.DeliveryListItemRepo
                    .GetList(trainId, woId)
                    .OrderBy(x=>x.Workstation.SortOrderTrain)
                    .ThenBy(x=>x.ItemWMS.Item.Code)
                    .Select(x => new DeliveryListItemViewModel()
                    {
                        //Id = x.Id,
                        ItemWMSId = x.ItemWMSId,
                        Code = x.ItemWMS.Item.Code,
                        Name = x.ItemWMS.Item.Name,
                        Workstation = x.Workstation != null? x.Workstation.Name : "-",
                        WorkstationName = x.Workstation != null? x.Workstation.Name : "-",
                        WorkstationId = x.WorkstationId != null? x.WorkstationId : 0,
                        WorkstationOrder = x.Workstation != null? x.Workstation.SortOrderTrain : 0,
                        WorkstationProductsFromIn = x.Workstation != null? x.Workstation.ProductsFromIn : 0,
                        WorkstationProductsFromOut = x.Workstation != null? x.Workstation.ProductsFromOut : 0,
                        QtyDelivered = x.QtyDelivered,
                        QtyRequested = x.QtyRequested,
                        QtyUsed =x.QtyUsed,
                        QtyPerPackage = x.QtyPerPackage,
                        BomQty = x.BomQty,
                        IsSubstituteData = x.IsSubstituteData
                    })
                    .ToList();
        }

        [HttpPost]
        public JsonResult CreateDeliveryList(int trainId, int[] woIds)
        {
            CreateNewDeliveryList(trainId, woIds);
            return Json(0);
        }
        private void CreateNewDeliveryList(int trainId, int[] woIds)
        {
            Transporter train = uow.TransporterRepo.GetById_AsNoTracking(trainId);
            List<int> selectedPlatforms= train.ConnectedTransportersArray;
            //List<int> trainItemIds = uow.ItemWMSRepo.GetList().Where(x => connectedTransporters.Contains(x.PickerNo)).Select(x => x.Id).ToList();
            List<DeliveryListItem> list;
            ProductionOrder po;

            foreach (int woId in woIds)
            {
                po = uow.ProductionOrderRepo.GetById(woId);
                list = uow.DeliveryListItemRepo.Simulate(selectedPlatforms, (po.QtyPlanned - po.QtyProducedInPast), trainId, woId, po.OrderNumber, po.Pnc.Code, train.DedicatedResourcesArray).ToList();

                foreach (DeliveryListItem dlItem in list)
                {
                    uow.DeliveryListItemRepo.AddOrUpdate(dlItem);
                }
            }
        }
        private void CreateNewDeliveryList(int trainId, int woId)
        {
            Transporter train = uow.TransporterRepo.GetById_AsNoTracking(trainId);
            List<int> selectedPlatforms= train.ConnectedTransportersArray;
            selectedPlatforms.Add(Convert.ToInt32(train.Code));
            //List<int> trainItemIds = uow.ItemWMSRepo.GetList().Where(x => connectedTransporters.Contains(x.PickerNo)).Select(x => x.Id).ToList();
            List<DeliveryListItem> list;
            ProductionOrder po;

            po = uow.ProductionOrderRepo.GetById(woId);
            list = uow.DeliveryListItemRepo.Simulate(selectedPlatforms, (po.QtyPlanned - po.QtyProducedInPast), trainId, woId, po.OrderNumber, po.Pnc.Code, train.DedicatedResourcesArray);
            
            //if(list.Count <= 0)
            //{
            //    list = uow.DeliveryListItemRepo.SimulateFromBOM(selectedPlatforms, (po.QtyPlanned - po.QtyProducedInPast), trainId, woId, po.Pnc.Code, train.DedicatedResourcesArray);
            //}

            foreach (DeliveryListItem dlItem in list)
            {
                try
                {
                    //if(list.FirstOrDefault(x=>x.ItemId==dlItem.ItemId&&x.TransporterId==dlItem.TransporterId&&x.WorkstationId==dlItem.WorkstationId&&x.WorkOrderId==dlItem.WorkOrderId) == null)
                    uow.DeliveryListItemRepo.AddOrUpdate(dlItem);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }            
        }
        private void DeleteFinishedWorkordersFromDeliveryLists(int trainId)
        {
            DateTime fromLimiter = DateTime.Now.AddHours(woHoursFromLimit);

            List<int> list = uow.DeliveryListItemRepo.GetList()
                .Where(x =>
                    (x.TransporterId == trainId) &&
                    (x.WorkOrder.CounterProductsOut > x.WorkOrder.QtyPlanned - 3 || x.WorkOrder.EndDate < fromLimiter)
                )
                .Select(x => x.WorkOrderId)
                .Distinct()
                .ToList();

            foreach (int _woId in list)
            {
                uow.DeliveryListItemRepo.DeleteByWorkorderId(_woId);
            }
        }
        private void DeleteDeletedWorkordersFromDeliveryLists(int trainId)
        {
            DateTime fromLimiter = DateTime.Now.AddHours(woHoursFromLimit);

            List<DeliveryListItem> list = uow.DeliveryListItemRepo.GetList()
                .Where(x =>
                    (x.TransporterId == trainId) &&
                    (x.WorkOrder.Deleted == true))
                .Distinct()
                .ToList();
            
            foreach (DeliveryListItem dli in list)
            {
                DeleteDeliveryListItem_And_MoveQtyToAnother(dli);
            }
        }
        private void DeleteOutOfRangeWorkordersFromDeliveryList(int trainId, List<int> inRangeWoIds)
        {
            List<DeliveryListItem> outOfRangeWoDeliveryList = uow.DeliveryListItemRepo.GetList()
                .Where(x => x.TransporterId == trainId && !inRangeWoIds.Contains(x.WorkOrderId))
                .ToList();

            foreach (DeliveryListItem dli in outOfRangeWoDeliveryList)
            {
                DeleteDeliveryListItem_And_MoveQtyToAnother(dli);
            }
        }
        private void DeleteDeliveryListItem_And_MoveQtyToAnother(DeliveryListItem dli)
        {
            decimal qtyDelivered_ToBeUsedInAnotherWO = 0;
            qtyDelivered_ToBeUsedInAnotherWO = dli.QtyDelivered - (dli.WorkOrder != null ? dli.WorkOrder.CounterProductsIn : dli.QtyDelivered);
            uow.DeliveryListItemRepo.Delete(dli);

            if (qtyDelivered_ToBeUsedInAnotherWO > 0)
            {
                List<DeliveryListItem> dliList = uow.DeliveryListItemRepo.GetListByItemIdAndWorkstationId(dli.ItemWMSId, dli.WorkstationId, dli.TransporterId).ToList();
                IncreaseQtyDelivered(qtyDelivered_ToBeUsedInAnotherWO, dliList);
            }
        }

        [HttpPost]
        public JsonResult VerifyDeliveryList(int trainId)
        {
            Transporter train = uow.TransporterRepo.GetById_AsNoTracking(trainId);
            List<int> transporters = train.ConnectedTransportersArray; transporters.Add(Convert.ToInt32(train.Code));
            var pos = uow.DeliveryListItemRepo.GetList().Where(x => x.TransporterId == trainId).Select(x => x.WorkOrder).Distinct().ToList();

            foreach(var po in pos)
            {
                //if (po.OrderNumber == "1607059314")
                _VerifyDeliveryList(po, train, transporters);
            }

            return Json(0);
        }

        [HttpPost]
        public JsonResult VerifyDeliveryListByWO(int trainId, int woId)
        {
            Transporter train = uow.TransporterRepo.GetById_AsNoTracking(trainId);
            List<int> transporters = train.ConnectedTransportersArray; transporters.Add(Convert.ToInt32(train.Code));
            ProductionOrder po = uow.ProductionOrderRepo.GetById(woId);

            _VerifyDeliveryList(po, train, transporters);

            return Json(0);
        }

        private void _VerifyDeliveryList(ProductionOrder po, Transporter train, List<int> transporters)
        {
            if (po != null)
            {
                List<DeliveryListItem> existingDL = uow.DeliveryListItemRepo.GetList(train.Id, po.Id).ToList();
                List<DeliveryListItem> simulatedDL = uow.DeliveryListItemRepo.Simulate(transporters, (po.QtyPlanned - po.QtyProducedInPast), train.Id, po.Id, po.OrderNumber, po.Pnc.Code, train.DedicatedResourcesArray);

                DeleteUnusedItems(existingDL, simulatedDL);
                AddNewItems(existingDL, simulatedDL);
                UpdateQtyPerPackage(existingDL, simulatedDL);
            }
        }

        private void DeleteUnusedItems(List<DeliveryListItem> existingDL, List<DeliveryListItem> simulatedDL)
        {
            int i = 0;
            while(i < existingDL.Count)
            {
                var eitm = existingDL[i];
                int counter = simulatedDL.Count(x => x.ItemWMSId == eitm.ItemWMSId && x.WorkstationId == eitm.WorkstationId);
                if (counter <= 0)
                {
                    uow.DeliveryListItemRepo.Delete(eitm);
                    existingDL.Remove(eitm);
                }
                else
                {
                    i++;
                }
            }
        }
        private void AddNewItems(List<DeliveryListItem> existingDL, List<DeliveryListItem> simulatedDL)
        {
            foreach (var sitm in simulatedDL)
            {
                int counter = existingDL.Count(x => x.ItemWMSId == sitm.ItemWMSId && x.WorkstationId == sitm.WorkstationId);
                if (counter <= 0)
                {
                    uow.DeliveryListItemRepo.AddOrUpdate(sitm);
                }
            }
        }
        private void UpdateQtyPerPackage(List<DeliveryListItem> existingDL, List<DeliveryListItem> simulatedDL)
        {
            Dictionary<int, decimal> itemQtyPerPackage = simulatedDL
                            .Select(x => new { x.ItemWMSId, x.QtyPerPackage })
                            .Distinct()
                            .ToDictionary(y => y.ItemWMSId, z => z.QtyPerPackage);

            foreach (var sitm in itemQtyPerPackage)
            {
                List<DeliveryListItem> eitmList = existingDL.Where(x => x.ItemWMSId == sitm.Key).ToList();

                foreach (var ei in eitmList)
                {
                    if (ei.QtyPerPackage != sitm.Value)
                    {
                        ei.QtyPerPackage = sitm.Value;
                        uow.DeliveryListItemRepo.AddOrUpdate(ei);
                    }
                }
            }
        }

        public JsonResult AddTransporterLog(int trainId, int itemWMSId, int workstationId, int qty)
        {
            DeliveryListItem dlItem = uow.DeliveryListItemRepo.GetListByItemIdAndWorkstationId(itemWMSId, workstationId, trainId).FirstOrDefault();

            TransporterLog transpLog = new TransporterLog();
            transpLog.TimeStamp = DateTime.Now;
            transpLog.ItemWMSId = dlItem.ItemWMSId;
            //transpLog.UserId = User.Identity.GetUserId();
            transpLog.RelatedObjectId = dlItem.Id;
            transpLog.WorkorderNumber = dlItem.WorkOrder.OrderNumber;
            transpLog.ProductItemCode = dlItem.WorkOrder.Pnc.Code;
            transpLog.TransporterId = dlItem.TransporterId;
            transpLog.Status = EnumTransportingStatus.Completed;
            transpLog.EntryType = EnumTransporterLogEntryType.Delivery;
            transpLog.Location = dlItem.Workstation.Line.Name + "-" + dlItem.Workstation.Name;
            transpLog.ItemQty = qty;
            
            uow.TransporterLogRepo.AddOrUpdate(transpLog);

            return Json(0);
        }
        public JsonResult AddTransporterLog_Loading(int transporterId)
        {
            //DeliveryListItem dlItem = uow.DeliveryListItemRepo.GetById(deliveryListItemId);

            TransporterLog transpLog = new TransporterLog();
            transpLog.TimeStamp = DateTime.Now;
            transpLog.ItemWMSId = null;
            //transpLog.UserId = User.Identity.GetUserId();
            transpLog.RelatedObjectId = 0;
            transpLog.WorkorderNumber = "";
            transpLog.ProductItemCode = "";
            transpLog.TransporterId = transporterId;
            transpLog.Status = EnumTransportingStatus.Created;
            transpLog.EntryType = EnumTransporterLogEntryType.Delivery;
            transpLog.Location = "Załadunek";
            transpLog.ItemQty = 0;

            uow.TransporterLogRepo.AddOrUpdate(transpLog);

            return Json(0);
        }

    }
}