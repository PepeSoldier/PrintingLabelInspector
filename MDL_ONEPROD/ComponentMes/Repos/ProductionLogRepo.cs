using XLIB_COMMON.Repo;
using MDL_ONEPROD.Common;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Interface;
using MDL_ONEPROD.ComponentMes.Models;
using MDL_ONEPROD.ComponentMes._Interfaces;
using MDL_ONEPROD.Model.Scheduling.Interface;
using MDL_CORE.ComponentCore.Models;
using MDL_ONEPROD.ComponentMes.ViewModels;

namespace MDL_ONEPROD.Repo.Scheduling
{
    public class ProductionLogRepo: RepoGenericAbstract<ProductionLog> //, ISerialNumberGenerator
    {
        protected new IDbContextOneprodMes db;
        
        public ProductionLogRepo(IDbContextOneprodMes db, IAlertManager alertManager) : base(db)
        {
            this.db = db;
        }

        public override ProductionLog GetById(int id)
        {
            return db.ProductionLogs.FirstOrDefault(x => x.Id == id);
        }
        public override IQueryable<ProductionLog> GetList()
        {
            return db.ProductionLogs.Where(x => x.Deleted == false).OrderBy(x => x.Id);
        }

        public IQueryable<ProductionLog> GetBySerialNumber(string serialNo)
        {
            return db.ProductionLogs.Where(x => x.SerialNo == serialNo);
        }
        public IQueryable<ProductionLog> GetBySerialNumberAndItemCode(string serialNo, string itemCode)
        {
            return db.ProductionLogs.Where(x => x.SerialNo == serialNo && x.Item.Code == itemCode);
        }
        public IQueryable<ProductionLog> GetByTimeRangeAndWorkplaceId(DateTime dateFrom, DateTime dateTo, int workplaceId)
        {
            IQueryable<ProductionLog> query = db.ProductionLogs
                .Join(db.CycleTimes,
                    plog => new { ItemGroupId = (int)plog.Item.ItemGroupId, MachineId = plog.Workplace.MachineId },
                    ct => new { ItemGroupId = ct.ItemGroupId, MachineId = ct.MachineId },
                    (plog, ct) => new { CT = ct, PLOG = plog })
                .Where(x => x.PLOG.WorkplaceId == workplaceId &&
                    x.PLOG.TimeStamp >= dateFrom &&
                    x.PLOG.TimeStamp < dateTo)
                .Select(x => new ProductionLog()
                {
                    ClientWorkOrderNumber = x.PLOG.ClientWorkOrderNumber,
                    CostCenter = x.PLOG.CostCenter,
                    CycleTime = x.CT.CycleTime,
                    DeclaredQty = x.PLOG.DeclaredQty,
                    Deleted = x.PLOG.Deleted,
                    //EntryType = x.PLOG.EntryType,
                    ReasonType = x.PLOG.ReasonType,
                    ReasonTypeId = x.PLOG.ReasonTypeId,
                    Id = x.PLOG.Id,
                    InternalWorkOrderNumber = x.PLOG.InternalWorkOrderNumber,
                    Item = x.PLOG.Item,
                    ItemId = x.PLOG.ItemId,
                    OEEReportProductionData = x.PLOG.OEEReportProductionData,
                    OEEReportProductionDataId = x.PLOG.OEEReportProductionDataId,
                    Reason = x.PLOG.Reason,
                    ReasonId = x.PLOG.ReasonId,
                    SerialNo = x.PLOG.SerialNo,
                    TimeStamp = x.PLOG.TimeStamp,
                    TransferNumber = x.PLOG.TransferNumber,
                    UserName = x.PLOG.UserName,
                    WorkorderTotalQty = x.PLOG.WorkorderTotalQty,
                    Workplace = x.PLOG.Workplace,
                    WorkplaceId = x.PLOG.WorkplaceId
                });

            return query;
        }
        public IQueryable<ProductionLog> GetList(FilterProductionLogViewModel filter)
        {
            bool isListEmpty = filter.MachineIds == null || filter.MachineIds.Count <= 0 || 
                (filter.MachineIds.Count == 1 && filter.MachineIds[0] == 0);

            IQueryable<ProductionLog> query = db.ProductionLogs.Where(x =>
                (x.Deleted == false) &&
                (filter.DateFrom <= x.TimeStamp && x.TimeStamp <= filter.DateTo) &&
                (filter.MachineId <= 0 || x.Workplace.MachineId == filter.MachineId) &&
                (isListEmpty || filter.MachineIds.Contains(x.Workplace.MachineId)) &&
                (filter.SerialNumber == null || x.SerialNo == filter.SerialNumber) &&
                (filter.ItemCode == null || x.Item.Code == filter.ItemCode) &&
                (filter.WorkOrder == null || x.InternalWorkOrderNumber == filter.WorkOrder))
            .OrderByDescending(x => x.Id);

            return query;
        }
        public IQueryable<ProductionLog> GetList(ProductionLog filter)
        {
            return db.ProductionLogs
                .Where(x => x.Deleted == false &&
                    (filter.SerialNo == null || x.SerialNo == filter.SerialNo) &&
                    (filter.CostCenter == null || x.CostCenter == filter.CostCenter) &&
                    (filter.DeclaredQty == 0 || x.DeclaredQty == filter.DeclaredQty) &&
                    (filter.InternalWorkOrderNumber == null || x.InternalWorkOrderNumber == filter.InternalWorkOrderNumber) &&
                    (filter.ItemCode == null || x.Item.Code == filter.ItemCode) &&
                    (filter.UserName == null || x.UserName == filter.UserName) &&
                    (filter.ClientWorkOrderNumber == null || x.ClientWorkOrderNumber == filter.ClientWorkOrderNumber) &&
                    (filter.WorkplaceId == 0 || x.WorkplaceId == filter.WorkplaceId))
                .OrderBy(x => x.Id);
        }
        public IQueryable<ProductionLog> GetListByWorkorderNumber(string workorderNumber)
        {
            return db.ProductionLogs.AsNoTracking().Where(m => m.ClientWorkOrderNumber == workorderNumber).OrderBy(m => m.Id);
        }
        public IQueryable<ProductionLog> GetListByInternalWorkOrderNumber(string serialNumber)
        {
            return db.ProductionLogs.AsNoTracking().Where(m => m.InternalWorkOrderNumber == serialNumber).OrderBy(m => m.Id);
        }

        public int DeleteProductionLog(int id)
        {
            ProductionLog part = db.ProductionLogs.FirstOrDefault(p => p.Id == id);
            if (part != null)
            {
                part.Deleted = true;
                return AddOrUpdate(part);
            }
            return 0;
        }
        public decimal GetProductionSumForReportOnline(DateTime dateFrom, DateTime dateTo, int workplaceId)
        {
            decimal qty = db.ProductionLogs
                .Where(x =>
                    x.ReasonType.EntryType != EnumEntryType.ScrapLabel &&
                    x.WorkplaceId == workplaceId &&
                    x.TimeStamp >= dateFrom && x.TimeStamp < dateTo)
                .DefaultIfEmpty()
                .Sum(x => x != null? x.DeclaredQty : 0);

            return qty;
        }
        public List<ReportOnlineModel> GetProductionDataForReportOnline(DateTime dateFrom, DateTime dateTo, int workplaceId)
        {
            IQueryable<ReportOnlineModel> query = db.ProductionLogs
                .Join(db.CycleTimes,
                    plog => new { ItemGroupId = (int)plog.Item.ItemGroupId, MachineId = plog.Workplace.MachineId },
                    ct => new { ItemGroupId = ct.ItemGroupId, MachineId = ct.MachineId },
                    (plog, ct) => new { CT = ct, PLOG = plog })
                .Where(x => 
                    x.PLOG.ReasonType.EntryType != EnumEntryType.ScrapLabel &&
                    x.PLOG.WorkplaceId == workplaceId &&
                    x.PLOG.TimeStamp >= dateFrom &&
                    x.PLOG.TimeStamp < dateTo)
                .GroupBy(x => new { x.PLOG.Reason, x.PLOG.ReasonType, x.PLOG.Item })
                .Select(st => new ReportOnlineModel() {
                    ReasonId = st.Key.Reason != null ? (int?)st.Key.Reason.Id : null,
                    ReasonName = st.Key.Reason != null ? st.Key.Reason.Name : string.Empty,
                    //EntryType = st.Key.EntryType,
                    //ReasonType = st.Key.ReasonType,
                    ReasonTypeName = st.Key.ReasonType.Name,
                    ReasonTypeId = st.Key.ReasonType.Id,
                    ReasonTypeEntryType = st.Key.ReasonType.EntryType,
                    ItemId = st.Key.Item.Id,
                    ItemCode = st.Key.Item.Code,
                    ProdQty = st.Sum(x => x.PLOG.DeclaredQty),
                    CycleTime = st.Min(x=>x.CT.CycleTime),
                    UsedTime = st.Sum(x => x.PLOG.DeclaredQty * x.CT.CycleTime),
                    ProductionDate = dateFrom
                });

            return query.ToList();
        }
        //public List<TreantJsNode> GetTreantParentsByPrLogId(int productionLogId)
        //{
        //    return db.ProductionLogs.Where(x => x.Id == productionLogId).Select(x => new TreantJsNode()
        //    {
        //        text = new TreantJsNodeText()
        //        {
        //            name = x.Item.Code,
        //            desc = x.ItemId.ToString(),
        //            title = x.SerialNo
        //        }
        //    }).ToList();
        //}
        //public int CreateEntry(ProductionLog prodLog)
        //{
        //    prodLog.ItemCode = "111333222";
        //    prodLog.DeclaredQty = 1;
        //    prodLog.SerialNo = "tekst"; //????
        //    int serialNumber = 0;

        //    return serialNumber;
        //}

        //public int GenerateSerialNumber(int resourceId)
        //{
        //    return new SerialNumberManager(db).GenerateSerialNumberForONEPROD(resourceId);
        //    //string query = SerialNumberManager.GenerateSerialNumberForONEPROD(resourceId);

        //    //int? serialNumber = null;

        //    //try
        //    //{
        //    //    serialNumber = db.Database.SqlQuery<int>(query).FirstOrDefault();
        //    //}
        //    //catch(Exception ex)
        //    //{
        //    //    serialNumber = -1;
        //    //    Console.WriteLine(ex.Message);
        //    //}
            
        //    //return serialNumber ?? 0;
        //}
    }
}