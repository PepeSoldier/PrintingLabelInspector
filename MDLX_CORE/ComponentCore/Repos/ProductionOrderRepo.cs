using MDL_BASE.Models.MasterData;
using XLIB_COMMON.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using MDLX_MASTERDATA.Entities;
using MDLX_CORE.ComponentCore.Entities;
using MDL_BASE.Interfaces;

namespace MDLX_CORE.ComponentCore.Repos
{
    public class ProductionOrderRepo : RepoGenericAbstract<ProductionOrder>
    {
        protected new IDbContextCore db;
        
        public ProductionOrderRepo(IDbContextCore db)
            : base(db)
        {
            this.db = db;
        }

        public override ProductionOrder GetById(int id)
        {
            return db.ProductionOrders.FirstOrDefault(d => d.Id == id);
        }
        public override IQueryable<ProductionOrder> GetList()
        {
            return db.ProductionOrders.Where(x=>x.Deleted == false).OrderByDescending(x => x.Id);
        }
        public ProductionOrder GetBySerialNo(int serialNo)
        {
            var po = db.Database.SqlQuery<ProductionOrder>(
                    "Select * FROM PRD.ProdOrder po " +
                    " LEFT JOIN _MPPL.MASTERDATA_Item Pnc ON po.PncId = Pnc.Id " +
                    "WHERE (SerialNoFrom IS NOT NULL AND SerialNoTo IS NOT NULL) AND " +
                    "(CAST(SerialNoFrom as INT) <= " + serialNo + " AND " + serialNo + " <= CAST(SerialNoTo as INT))")
                .ToList().FirstOrDefault();
            
            if(po != null)
                po.Pnc = db.Items.FirstOrDefault(x => x.Id == po.PncId);

            return po;
            //return db.ProductionOrders.FirstOrDefault(d => 
            //    (d.SerialNoFrom != null && d.SerialNoTo != null) &&
            //    int.Parse(d.SerialNoFrom) <= serialNo && serialNo <= int.Parse(d.SerialNoTo));
        }

        public ProductionOrder GetByOrderNumber( string orderNumber)
        {
            return db.ProductionOrders.Where(x => x.OrderNumber == orderNumber).FirstOrDefault();
        }
        public IQueryable<ProductionOrder> GetListSortedBySequence()
        {
            return db.ProductionOrders.Where(x => x.Deleted == false).OrderBy(x => x.Sequence);
        }
        public IQueryable<ProductionOrder> GetListNew(ProductionOrderFilter sorter)
        {
            DateTime StartDate = Convert.ToDateTime(sorter.Date);
            DateTime DateLimit = StartDate.AddHours(24);
            int PncId = Convert.ToInt32(sorter.Pnc);
            
            IQueryable<ProductionOrder> pO = db.ProductionOrders.AsNoTracking()
                 .Where(x => 
                     (x.Deleted == false) &&
                     ((x.StartDate >= StartDate && x.StartDate <= DateLimit)|| StartDate.Year == 1) &&
                     (x.Line.Name == sorter.Line || sorter.Line == null) &&
                     (x.OrderNumber == sorter.OrderNumber || sorter.OrderNumber == null)  &&
                     (x.PncId == PncId || PncId == 0))
                 .OrderBy(o => o.StartDate);
            return pO;
        }
        public IQueryable<ProductionOrder> GetListFiltered(DateTime dateFrom, DateTime dateTo, List<int> lineIds, string orderNo, string pnc)
        {
            return db.ProductionOrders
                .Where(x =>
                    (x.EndDate >= dateFrom && x.StartDate < dateTo) &&
                    (lineIds.Count == 0 || lineIds.Contains(x.LineId)) &&
                    (orderNo == null || orderNo == x.OrderNumber) &&
                    (pnc == null || pnc == x.Pnc.Code)
            );
        }
        public IQueryable<ProductionOrder> GetByTimeRangeAndLine(DateTime dateFrom, DateTime dateTo, string[] lineNames)
        {
            IQueryable<ProductionOrder> query = db.ProductionOrders.Where(
                    x => x.Deleted == false &&
                    x.EndDate >= dateFrom &&
                    x.StartDate < dateTo &&
                    //x.CounterProductsOut < (x.QtyPlanned - 5) &&
                    lineNames.Contains(x.Line.Name))
                .OrderBy(x => x.StartDate)
                .ThenBy(x => x.LineId);

            return query;
        }

        public List<ProductionOrder> GetListByIds(int[] ProdOrders)
        {
            DateTime now = DateTime.Now;

            return db.ProductionOrders.AsNoTracking()
                        .Where(x => ProdOrders.Contains(x.Id)
                                && (x.QtyRemain > 0 || (x.QtyRemain <= 0 && x.StartDate < now)))
                        .OrderBy(o => o.StartDate).ToList();
        }
        public List<ProductionOrder> GetByTimeRange(DateTime dateFrom, DateTime dateTo)
        {
            return db.ProductionOrders.Where(
                    x => x.Deleted == false && 
                    x.EndDate >= dateFrom &&
                    x.CounterProductsOut < (x.QtyPlanned - 5) &&
                    x.StartDate < dateTo)
                .OrderBy(x => x.StartDate)
                .ToList();
        }

        //public List<ProductionOrder> GetByPickingListGuid(string pickingListGuid)
        //{
            

        //}

        //public IQueryable<ProductionOrder> GetByTimeRangeAndLine(DateTime dateFrom, DateTime dateTo, string[] lineNames)
        //{
        //    IQueryable<ProductionOrder> query = db.ProductionOrders.Where(
        //            x => x.Deleted == false &&
        //            x.EndDate >= dateFrom &&
        //            x.StartDate < dateTo &&
        //            //x.CounterProductsOut < (x.QtyPlanned - 5) &&
        //            lineNames.Contains(x.Line.Name))
        //        .OrderBy(x => x.StartDate)
        //        .ThenBy(x => x.LineId);

        //    return query;
        //}

        public List<ProductionOrder> GetByOrderNumbers(List<string> orderNumbers)
        {
            return db.ProductionOrders.Where(x => orderNumbers.Contains(x.OrderNumber)).ToList();
        }
        public List<ProductionOrder> GetPreviousOrdersOfLine(DateTime dateFrom, string lineName, int number)
        {
            List<ProductionOrder> list = db.ProductionOrders.Where(
                    x => x.Deleted == false &&
                    x.FirstProductIn < dateFrom &&
                    x.Line.Name == lineName)
                .OrderByDescending(x => x.FirstProductIn)
                .Take(number)
                .ToList();

            return list.OrderBy(x => x.FirstProductIn).ToList();
        }
        public List<ProductionOrder> GetScheduleRange(string lineName, int startAtSequence, int endAtSequence)
        {
            List<ProductionOrder> poList = GetListSortedBySequence()
                                    .Where(x => x.Line.Name == lineName && x.Sequence >= startAtSequence && x.Sequence <= endAtSequence)
                                    .OrderBy(o => o.Sequence)
                                    .ToList();

            ProductionOrder firstPO = GetListSortedBySequence().Where(x => x.Sequence < startAtSequence).OrderByDescending(o => o.Sequence).FirstOrDefault();
            ProductionOrder lastPO = GetListSortedBySequence().Where(x => x.Sequence > endAtSequence).OrderBy(o => o.Sequence).FirstOrDefault();

            poList.Insert(0, firstPO);
            poList.Add(lastPO);

            return poList;
        }

        public List<Item> GetPNCsByTimeRange(DateTime dateFrom, DateTime dateTo)
        {
            return db.ProductionOrders.Where(x => x.Deleted == false && x.StartDate >= dateFrom && x.StartDate < dateTo).Select(x => x.Pnc).ToList();
        }
        public List<string> GetPncIdList(string prefix)
        {
            return db.ProductionOrders.Where(x => x.PncId.ToString().StartsWith(prefix)).Select(x => x.PncId.ToString()).Distinct().Take(5).ToList();
        }
        public List<string> GetProductionOrderList(string prefix)
        {
            return db.ProductionOrders.Where(x => x.OrderNumber.StartsWith(prefix)).Select(x => x.OrderNumber).Distinct().Take(5).ToList();
        }

        public void SetDeletedNotUpdatedWorkorders()
        {
            db.Database.ExecuteSqlCommand(
                "UPDATE [PRD].[ProdOrder] SET Deleted = 1 WHERE LastUpdate < '" + 
                DateTime.Now.AddMinutes(-5).ToString("yyyy-MM-dd HH:mm:ss") + "'"
            );
        }
    }
}