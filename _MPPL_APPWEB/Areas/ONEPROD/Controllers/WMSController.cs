using _MPPL_WEB_START.Areas.ONEPROD.APS.ViewModels;
using MDL_BASE.Models.IDENTITY;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_ONEPROD.ComponentWMS._Interfaces;
using MDL_ONEPROD.ComponentWMS.UnitOfWorks;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Manager;
using MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.ONEPROD.Controllers
{
    //[Authorize(Roles = DefRoles.TechnologyOperator)]
    public partial class WMSController : BaseController
    {
        public WMSController(IDbContextOneprod db, IDbContextOneprodWMS dbWMS = null)
        {
            this.db = db;
            this.dbWMS = dbWMS;
            uow = new UnitOfWorkOneprod(db);
            if (dbWMS != null)
            {
                uowWMS = new UnitOfWorkOneprodWMS(dbWMS);
            }
            ViewBag.Skin = "nasaSkin";
        }

        readonly IDbContextOneprod db;
        readonly IDbContextOneprodWMS dbWMS;
        UnitOfWorkOneprod uow;
        UnitOfWorkOneprodWMS uowWMS;
        
        [HttpPost]
        public JsonResult GetQtyPerBox(int workorderId)
        {
            List<int> TrolleysCapacity = new List<int>();
            Workorder task = uow.WorkorderRepo.GetById(workorderId);

            if (uowWMS != null && task != null && task.Qty_Remain > 0)
            {
                TrolleysCapacity = uowWMS.WarehouseItemRepo.GetListByItemGroup(Convert.ToInt32(task.Item.ItemGroupId)).Select(x => x.QtyPerLocation).ToList();
            }

            return Json(TrolleysCapacity);
        }

        public ActionResult StockMonitor()
        {
            ViewBag.Skin = "nasaSkin";
            StockViewModel vm = new StockViewModel();
            vm.Stock = new TaskStockMonitorManager(uow).GetCurrentStock();

            List<ItemInventory> stockTemp = new TaskStockMonitorManager(uow).GetCurrentStock();
            stockTemp.RemoveAll(x => x.Stock < 1);

            //dodaj lisę wszystkich boxów
            List<Warehouse> boxes = uowWMS.WarehouseRepo.GetList().ToList();
            Warehouse boxTmp;

            foreach (ItemInventory partStock in stockTemp)
            {
                //sprawdz do jakiego boxa można zapakowac komponent
                List<WarehouseItem> list = uowWMS.WarehouseItemRepo.GetListByItemGroup(itemGroupId: partStock.Item.ItemGroupOP.Id).ToList();

                WarehouseItem bpc = list.FirstOrDefault();

                if (bpc != null)
                {
                    boxTmp = boxes.FirstOrDefault(x => x.Id == bpc.WarehouseId);
                    boxTmp.CurrentUsage += (double)partStock.Stock / (double)bpc.QtyPerLocation;
                    boxTmp.Color = partStock.Item.ItemGroupOP.Color;
                    boxTmp.BoxesCapacitySum += bpc.QtyPerLocation;
                    boxTmp.BoxesCount++;
                    boxTmp.BoxAvgQty = boxTmp.BoxesCapacitySum / boxTmp.BoxesCount;
                }
                //zarezerwuj boxy i odejmij ilość ze stocku
                //Jezeli ilość wciąż wieksza od zera to zarezerwuj alternatywny box
                //jezeli alternatywny nie istnieje to minusuj dalej pokazując przeciężenie bufora
            }
            vm.Boxes = boxes;
            //var itemGroups = vm.Stock.Select(x=>x.Part.ItemGroup).Distinct()

            return View(vm);
        }
        [HttpPost]
        public JsonResult StockMonitorAncCoverage(int partId)
        {
            Workorder task = uow.WorkorderRepo.GetFirstNotCoveredTask(partId);
            DateTime coverageDate = DateTime.Now.AddDays(7);

            if (task.ClientOrder != null)
            {
                //if(task.Order.Qty_Remain < task.Order.Qty_Total)
                //{
                //    decimal shouldBeProduced = ((DateTime.Now - task.Order.StartDate).TotalMinutes / (task.Order.EndDate - task.Order.StartDate).TotalMinutes) * task.Order.Qty_Total;
                //}

                if (task.Qty_Produced == 0)
                {
                    coverageDate = task.ClientOrder.StartDate;
                }
                else
                {
                    int percent = task.Qty_Total > 0 ? 100 * task.Qty_Produced / task.Qty_Total : 0;
                    int addMinutes = (int)(percent * (task.ClientOrder.EndDate - task.ClientOrder.StartDate).TotalMinutes) / 100;
                    coverageDate = task.ClientOrder.StartDate.AddMinutes(addMinutes);
                }
            }

            return Json(new { coverageDate = coverageDate.ToString("yyyy-MM-dd HH:mm"), coverageMinutes = (int)(coverageDate - DateTime.Now).TotalMinutes });
        }


        //--------------------------------------STOCK
        [Authorize(Roles = DefRoles.ONEPROD_VIEWER)]
        public ActionResult Stock()
        {
            StockViewModel vm = new StockViewModel();
            vm.Stock = null;
            vm.SelectedDate = DateTime.Now.Date.AddHours(6);
            vm.StockDates = uow.ItemInventoryRepo.GetStockDates();
            return View(vm);
        }
        [HttpPost]
        public ActionResult LoadStock(StockViewModel vm)
        {
            vm.Stock = uow.ItemInventoryRepo.GetStock(vm.SelectedDate);

            if (vm.Stock == null)
            {
                DateTime stockDate = DateTime.Now.Date;
                stockDate = stockDate.AddHours(DateTime.Now.Hour);
                stockDate = stockDate.AddMinutes(DateTime.Now.Minute);

                List<ItemInventory> stockTemp = new TaskStockMonitorManager(uow).GetCurrentStock();
                vm.Stock = uow.ItemInventoryRepo.CreateStock(stockDate, stockTemp);
                vm.SelectedDate = stockDate;
            }
            vm.StockDates = uow.ItemInventoryRepo.GetStockDates();
            return View("Stock", vm);
        }
        [HttpPost]
        public ActionResult CreateStock(StockViewModel vm)
        {
            DateTime stockDate = DateTime.Now.Date;
            stockDate = stockDate.AddHours(DateTime.Now.Hour);
            stockDate = stockDate.AddMinutes(DateTime.Now.Minute);

            vm.Stock = uow.ItemInventoryRepo.GetStock(stockDate);

            if (vm.Stock == null)
            {
                List<ItemInventory> stockTemp = new TaskStockMonitorManager(uow).GetCurrentStock();
                vm.Stock = uow.ItemInventoryRepo.CreateStock(stockDate, stockTemp);
                vm.SelectedDate = stockDate;
            }

            vm.StockDates = uow.ItemInventoryRepo.GetStockDates();
            return View("Stock", vm);
        }

        [HttpPost, Authorize(Roles = DefRoles.ONEPROD_VIEWER)]
        public JsonResult StockDelete(DateTime date)
        {
            uow.ItemInventoryRepo.DeleteStock(date);
            return Json(0);
        }
        [HttpPost, Authorize(Roles = DefRoles.ONEPROD_VIEWER)]
        public JsonResult StockUpdate(StockViewModel vm)
        {
            int r = uow.ItemInventoryRepo.UpdateStock(vm.StockId, vm.StockQty);
            return Json(r);
        }
        [HttpPost, Authorize(Roles = DefRoles.ONEPROD_VIEWER)]
        public JsonResult ScrapkUpdate(StockViewModel vm)
        {
            int r = uow.ItemInventoryRepo.UpdateScrap(vm.StockId, vm.StockQty);
            return Json(r);
        }
        [HttpPost, Authorize(Roles = DefRoles.ONEPROD_VIEWER)]
        public JsonResult ConsiderScrap(StockViewModel vm)
        {
            //uowWMS.ItemInventoryRepo.ConsiderScrap(vm.SelectedDate);

            int scrapQty = 0;
            int declaredQty = 0;
            List<ItemInventory> listStock = uow.ItemInventoryRepo.GetListByDate(vm.SelectedDate);
            Workorder task;
            foreach (ItemInventory partStock in listStock)
            {
                scrapQty = partStock.ScrapQty;
                while (scrapQty > 0 && !partStock.isScrapApplied)
                {
                    task = db.Workorders.Where(x => x.ItemId == partStock.ItemId && x.Qty_Produced > 0).OrderByDescending(x => x.StartTime).Take(1).FirstOrDefault();

                    if (task == null) break;

                    declaredQty = task.Qty_Produced;
                    task.Qty_Produced = Math.Max(0, task.Qty_Produced - scrapQty);
                    scrapQty -= Math.Min(scrapQty, declaredQty);
                    partStock.isScrapApplied = true;

                    uow.WorkorderRepo.Update(task);
                    uow.ItemInventoryRepo.Update(partStock);
                }
            }
            return Json(0);
        }
        [HttpPost, Authorize(Roles = DefRoles.ONEPROD_VIEWER)]
        public JsonResult ConsiderStock(StockViewModel vm)
        {
            if (vm.SelectedDate != null && vm.SelectedDate.Year > 2017)
            {
                TaskManager tm = new TaskManager(uow);
                TaskStockManager tsm = new TaskStockManager(tm, "", uow);

                var workorders = uow.WorkorderRepo.GetWorkorderOfResourceGroup(vm.AreaId, new DateTime(2000, 1, 1), vm.SelectedDate.AddDays(60)).ToList();
                tsm.ConsiderInventory(workorders, vm.SelectedDate);
            }
            return Json(0);
        }
    }
}