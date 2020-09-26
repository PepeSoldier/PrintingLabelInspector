using XLIB_COMMON.Repo;
using MDL_ONEPROD.Common;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Interface;

namespace MDL_ONEPROD.Repo.Scheduling
{
    public class ItemInventoryRepo : RepoGenericAbstract<ItemInventory>
    {
        protected new IDbContextOneprod db;
        private UnitOfWorkOneprod unitOfWork;

        public ItemInventoryRepo(IDbContextOneprod db, IAlertManager alertManager, UnitOfWorkOneprod unitOfWork = null) : base(db)
        {
            this.db = db;
            this.unitOfWork = unitOfWork;
        }

        public override ItemInventory GetById(int id)
        {
            return db.ItemInventories.FirstOrDefault(x => x.Id == id);
        }
        public override IQueryable<ItemInventory> GetList()
        {
            return db.ItemInventories.OrderBy(x => x.Id);
        }
       
        public ItemInventory GetByPartId(int partId)
        {
            return db.ItemInventories.LastOrDefault(s => s.ItemId == partId);
        }
        public ItemInventory GetByPartIdAndDate(int partId, DateTime date)
        {
            return db.ItemInventories.First(s => s.ItemId == partId && s.ReportDate == date);
        }
        public List<ItemInventory> GetListByDate(DateTime date)
        {
            return db.ItemInventories
                .Where(s => s.ReportDate == date)
                .OrderBy(s => s.Item.ItemGroupOP.ResourceGroupId)
                .ThenBy(s => s.Item.ItemGroup.Process.Name)
                .ThenBy(s => s.Item.ItemGroup.Name).ToList();
        }
        public List<ItemInventory> GetStock(DateTime date)
        {
            List<ItemInventory> list = GetListByDate(date);

            if (!(list.Count > 0))
            {
                return null;
                //return CreateStock(date, new List<PartStock>());
            }
            return list;
        }
        public List<ItemInventory> CreateStock(DateTime date, List<ItemInventory> calculatedStocks)
        {
            List<ItemOP> parts = unitOfWork.ItemOPRepo.GetListOnlyActive().ToList();
            ItemInventory calculatedStock;

            foreach (ItemOP p in parts)
            {
                calculatedStock = calculatedStocks.FirstOrDefault(x => x.ItemId == p.Id);

                ItemInventory ps = new ItemInventory();
                ps.ItemId = p.Id;
                ps.ReportDate = date;
                ps.StockCalculated = (calculatedStock != null) ? calculatedStock.Stock : 0;
                ps.Stock = 0;
                ps.UserId = 1;
                ps.TimeStamp = date;

                AddOrUpdate(ps);
            }
            return GetStock(date);
        }
        public List<DateTime> GetStockDates()
        {
            return db.Database.SqlQuery<DateTime>("SELECT DISTINCT ReportDate FROM ONEPROD.WMS_ItemInventory ORDER BY ReportDate DESC").ToList();
        }

        public int GetStock(int partId)
        {
            ItemInventory ps2 = GetByPartId(partId);

            if (ps2 != null)
                return ps2.Stock;
            else
                return -1;
        }
        public int GetStock(int partId, DateTime date)
        {
            ItemInventory ps2 = GetByPartIdAndDate(partId, date);

            if (ps2 != null)
                return ps2.Stock;
            else
                return -1;
        }
        public int UpdateStock(int id, int qty)
        {
            ItemInventory pStock = db.ItemInventories.FirstOrDefault(p => p.Id == id);

            if (pStock != null)
            {
                pStock.Stock = qty;
                return AddOrUpdate(pStock);
            }
            return -1;
        }
        public int UpdateScrap(int id, int qty)
        {
            ItemInventory pStock = db.ItemInventories.FirstOrDefault(p => p.Id == id);

            if (pStock != null)
            {
                pStock.ScrapQty = qty;
                return AddOrUpdate(pStock);
            }
            return -1;
        }
        public void DeleteStock(DateTime date)
        {
            db.ItemInventories.RemoveRange(db.ItemInventories.Where(s => s.ReportDate == date));
            db.SaveChanges();
        }

        public void ConsiderScrap(DateTime stockDate)
        {
            
        }

        //public void ConsiderInventory(DateTime stockDate)
        //{
        //    int stockQty = 0;
        //    int declaredQty = 0;
        //    List<PartStock> listStock = GetListByDate(stockDate);
        //    Task task;
        //    foreach (PartStock partStock in listStock)
        //    {
        //        stockQty = partStock.ScrapQty;
        //        while (stockQty > 0 && !partStock.isApplied)
        //        {
        //            task = db.Tasks.Where(x => x.PartId == partStock.PartId && (x.Qty_Total - x.Qty_Produced) > 0).OrderBy(x => x.DueDate).Take(1).FirstOrDefault();

        //            if (task == null) break;

        //            declaredQty = task.Qty_Produced;
        //            task.Qty_Produced = Math.Max(0, task.Qty_Produced - stockQty);
        //            stockQty -= Math.Min(stockQty, declaredQty);
        //            partStock.isApplied = true;

        //            Update(task);
        //            Update(partStock);
        //        }
        //    }
        //}


        //PartStock
        //public List<PartStock> GetStock(DateTime date)
        //{
        //    return GetListByDate(date);
        //}
    }
}