using MDL_ONEPROD.ComponentWMS.UnitOfWorks;
using MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.Repo;
using MDLX_MASTERDATA.Entities;
using MDLX_MASTERDATA.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.Manager
{
    public class ItemInventoryModel
    {
        public int Id { get; set; }
        public virtual Model.Scheduling.ItemOP Item { get; set; }
        public int ItemId { get; set; }
        public int StockCalculated { get; set; }
        public int Stock { get; set; }
        public int ScrapQty { get; set; }
        public DateTime ReportDate { get; set; }
    }
    
    public class TaskStockMonitorManager
    {

        UnitOfWorkOneprod uow;

        public TaskStockMonitorManager(UnitOfWorkOneprod uow)
        {
            this.uow = uow;
        }

        public List<ItemInventory> GetCurrentStock(int resourceGroupId = 0)
        {
            //ItemInventory tmpInventory;
            DateTime reportDate = DateTime.Now;
            //List<Workorder> workorders = uow.WorkorderRepo.GetList()
            //            .Where(t => 
            //                t.Qty_Produced > 0 && 
            //                (areaId == 0 || t.Item.ItemGroup.ResourceGroupId == areaId) &&
            //                t.ItemId != null)
            //            .OrderBy(t => t.Item.ItemGroup.Process.Name)
            //            .ThenBy(t => t.Item.ItemGroup.ResourceGroup.StageNo)
            //            .ThenBy(t => t.Item.ItemGroup.Name)
            //            .ThenBy(t => t.Item.Name)
            //            .ToList();
            List<ItemInventory> inventoryList = new List<ItemInventory>();

            List<ItemInventoryModel> inventoryList_TEMP = uow.WorkorderRepo.GetList()
                .Where(t =>
                    t.Item.Type != ItemTypeEnum.ItemGroup &&
                    t.Qty_Produced > 0 &&
                    (resourceGroupId == 0 || t.Item.ItemGroupOP.ResourceGroupId == resourceGroupId) &&
                    t.ItemId != null)
                .GroupBy(x => x.Item)
                .Select(y => new ItemInventoryModel(){
                    Item = y.Key,
                    ItemId = y.Key != null? y.Key.Id : 0,
                    ReportDate = reportDate,
                    Stock = y.Sum(wo => wo != null? wo.Qty_Produced - wo.Qty_Used : 0)})
                .OrderBy(t => t.Item.ItemGroup.Process.Name)
                .ThenBy(t => t.Item.ItemGroupOP.ResourceGroupOP.StageNo)
                .ThenBy(t => t.Item.ItemGroup.Name)
                .ThenBy(t => t.Item.Name)
                .ToList();

            inventoryList = inventoryList_TEMP.Select(x =>
                new ItemInventory()
                {
                    Item = x.Item,
                    ItemId = x.ItemId,
                    ReportDate = x.ReportDate,
                    Stock = x.Stock
                }).ToList();

            //foreach (Workorder wo in workorders)
            //{
            //    tmpInventory = inventoryList.FirstOrDefault(t => t.ItemId == wo.ItemId);

            //    if(tmpInventory == null)
            //    {
            //        tmpInventory = new ItemInventory
            //        {
            //            ItemId = (int)wo.ItemId,
            //            Item = uow.ItemRepo.GetById((int)wo.ItemId),
            //            ReportDate = reportDate
            //        };
            //        inventoryList.Add(tmpInventory);
            //    }
            //    tmpInventory.Stock += (wo.Qty_Produced - wo.Qty_Used);
            //}

            return inventoryList;
        }
    }
}