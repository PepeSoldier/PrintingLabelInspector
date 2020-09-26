using XLIB_COMMON.Repo;
using MDL_ONEPROD.ComponentRTV._Interfaces;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Model.Scheduling.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lextm.SharpSnmpLib.Security;
using MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.ComponentRTV.Models;
using MDL_ONEPROD.ComponentMes.Models;

namespace MDL_ONEPROD.ComponentRTV.Repos
{
    public class RTVOEEProductionDataRepo : RepoGenericAbstract<RTVOEEProductionData>
    {
        protected new IDbContextOneProdRTV db;
        //UnitOfWorkOneProdOEE unitOfWork;

        public RTVOEEProductionDataRepo(IDbContextOneProdRTV db) : base(db)
        {
            this.db = db;
            //this.unitOfWork = unitOfWork;
        }

        public override RTVOEEProductionData GetById(int id)
        {
            return db.RTVOEEProductionData.FirstOrDefault(x => x.Id == id);
        }
        public override IQueryable<RTVOEEProductionData> GetList()
        {
            return db.RTVOEEProductionData.Where(x => x.Deleted == false).OrderByDescending(x => x.Id);
        }

        public List<RTVOEEProductionData> GetDataOfMinutesMachineId(DateTime dateFrom, DateTime dateTo, int machineId) //, ReasonType reasonType = null)//EnumEntryType entryType = EnumEntryType.Undefined)
        {
            dateTo = dateTo.AddMinutes(1);
            List<RTVOEEProductionData> list = db.RTVOEEProductionData
                .Where(x => x.Deleted == false &&
                            x.MachineId == machineId &&
                            (dateFrom <= x.TimeStamp && x.TimeStamp < dateTo)) //&&
                            //(reasonType == null || x.ReasonTypeId == reasonType.Id))
                            //(x.EntryType == entryType || entryType == EnumEntryType.Undefined))
                .OrderBy(x => x.Id).ToList();

            if (list == null || list.Count < 1)
            {
                list = db.RTVOEEProductionData
                .Where(x => x.Deleted == false &&
                            x.MachineId == machineId &&
                            (x.ProductionDate <= dateFrom && dateTo < x.TimeStamp)) //&&
                            //(x.EntryType == entryType || entryType == EnumEntryType.Undefined))
                            //(reasonType == null || x.ReasonTypeId == reasonType.Id))
                .OrderBy(x => x.Id).ToList();
            }

            if (list == null || list.Count < 1)
            {
                list = new List<RTVOEEProductionData>();
                list.Add(new RTVOEEProductionData() { ReasonType = new ReasonType { EntryType = EnumEntryType.Undefined }, CycleTime = 0, ProdQty = 0, UsedTime = 0 });
            }

            return list;
        }
        public List<RTVOEEProductionData> GetDataOfMinuteMachineId(DateTime dateFrom, int machineId, ReasonType reasonType = null)//EnumEntryType entryType = EnumEntryType.Undefined)
        {
            DateTime dateTo = dateFrom.AddMinutes(1);
            List<RTVOEEProductionData> list = db.RTVOEEProductionData
                .Where(x => x.Deleted == false &&
                            x.MachineId == machineId &&
                            (dateFrom <= x.TimeStamp && x.TimeStamp < dateTo) &&
                            (reasonType == null || x.ReasonTypeId == reasonType.Id))
                            //(x.EntryType == entryType || entryType == EnumEntryType.Undefined))
                .OrderBy(x => x.Id).ToList();

            if (list == null || list.Count < 1)
            {
                list = db.RTVOEEProductionData
                .Where(x => x.Deleted == false &&
                            x.MachineId == machineId &&
                            (x.ProductionDate <= dateFrom && dateTo < x.TimeStamp) &&
                            //(x.EntryType == entryType || entryType == EnumEntryType.Undefined))
                            (reasonType == null || x.ReasonTypeId == reasonType.Id))
                .OrderBy(x => x.Id).ToList();
            }

            if (list == null || list.Count < 1)
            {
                list = new List<RTVOEEProductionData>();
                list.Add(new RTVOEEProductionData() { ReasonType = new ReasonType { EntryType = EnumEntryType.Undefined }, CycleTime = 0, ProdQty = 0, UsedTime = 0 });
                //list = db.RTVOEEProductionData
                //.Where(x => x.Deleted == false &&
                //            x.MachineId == machineId &&
                //            dateFrom <= x.TimeStamp &&
                //            (x.EntryType == entryType || entryType == EnumEntryType.Undefined))
                //.OrderBy(x => x.Id).Take(1).ToList();
            }

            return list;
        }
        public List<RTVOEEProductionData> GetDataByTimeRangeAndMachineId(DateTime dateFrom, DateTime dateTo, int machineId)//, ReasonType reasonType = null)//EnumEntryType entryType = EnumEntryType.Undefined)
        {
            List<RTVOEEProductionData> list = db.RTVOEEProductionData
                .Where(x => x.Deleted == false &&
                            x.MachineId == machineId &&
                            (dateFrom <= x.ProductionDate && x.ProductionDate < dateTo) &&
                            //((reasonType == null && x.ReasonType != null && x.ReasonType.EntryType < EnumEntryType.StopPlanned) || (reasonType != null && x.ReasonTypeId == reasonType.Id)))
                            //(x.EntryType == entryType || (entryType == EnumEntryType.Undefined && x.EntryType >= 0)))
                            (x.ReasonType != null && x.ReasonType.EntryType >= EnumEntryType.TimeAvailable))
                //.OrderByDescending(x => x.Id)
                .OrderByDescending(x => x.TimeStamp)
                .ToList();

            if (list == null || list.Count < 1)
            {
                //if stoppage started before shift start (may occure when shifts are closed)
                dateTo = dateTo > DateTime.Now ? DateTime.Now.AddMinutes(-2) : dateTo;
                list = db.RTVOEEProductionData
                .Where(x => x.Deleted == false &&
                            x.MachineId == machineId &&
                            (x.ProductionDate <= dateFrom && dateTo < x.TimeStamp) &&
                            //(entryType == EnumEntryType.Undefined && x.EntryType >= 0))
                            //(reasonType == null && x.ReasonType.EntryType >= EnumEntryType.TimeAvailable))
                            (x.ReasonType != null && x.ReasonType.EntryType >= EnumEntryType.TimeAvailable))
                .OrderBy(x => x.Id).ToList();
            }

            return list;
        }

        public List<RTVOEEProductionData> GetDataForReport_Stops(DateTime dateFrom, DateTime dateTo, int machineId, EnumEntryType entryType = EnumEntryType.Undefined)
        {
            List<RTVOEEProductionData> list = db.RTVOEEProductionData
                .Where(x => x.Deleted == false &&
                            x.MachineId == machineId &&
                            (dateFrom <= x.ProductionDate && x.ProductionDate < dateTo) &&
                            (x.ReasonType.EntryType >= EnumEntryType.StopPlanned))
                .OrderByDescending(x => x.Id)
                .ToList();

            return list?? new List<RTVOEEProductionData>();
            
        }
        public List<RTVOEEProductionItem> GetDataForReport_Prod(DateTime dateFrom, DateTime dateTo, int machineId)
        {
            var query = from d in db.RTVOEEProductionData
                        from dd in db.RTVOEEProductionDataDetails.Where(x => x.RTVOEEProductionData.Id == d.Id).Take(1)
                        where d.MachineId == machineId && d.Deleted == false && d.ReasonType.EntryType < EnumEntryType.StopPlanned &&
                                (d.ProductionDate >= dateFrom && d.ProductionDate < dateTo)
                        group d by new { d.Item, d.ItemId, d.MachineId, dd.ProgramNo, dd.ProgramName, d.Deleted, d.ReasonType } into g
                        select new RTVOEEProductionItem
                        {
                            Item = g.Key.Item,
                            ItemId = g.Key.ItemId,
                            ProgramName = g.Key.ProgramName,
                            ProgramNo = g.Key.ProgramNo,
                            //entryType = g.Key.EntryType,
                            ReasonType = g.Key.ReasonType,
                            ProdQty = g.Sum(x => x.ProdQty),
                            TimeStamp = g.Max(x => x.TimeStamp)
                        };

            var list = query.ToList();
            return list;
        }
        public int GetProducedQty(DateTime dateFrom, DateTime dateTo, int machineId) //, ReasonType reasonType = null)//EnumEntryType entryType = EnumEntryType.Undefined)
        {
            decimal qty = db.RTVOEEProductionData
                .Where(x => x.Deleted == false &&
                            x.MachineId == machineId &&
                            (dateFrom <= x.ProductionDate && x.ProductionDate < dateTo))
                .DefaultIfEmpty()
                //.Max(x => x != null? x.ProdQtyShift : 0);
                .Sum(x => x != null ? x.ProdQty * (x.PiecesPerPallet <= 0? 1 : x.PiecesPerPallet) : 0);

            return (int)qty;
        }
        public List<ReportOnlineModel> GetStoppageDataForReportOnline(DateTime dateFrom, DateTime dateTo, int machineId) //, ReasonType reasonType = null)//EnumEntryType entryType = EnumEntryType.Undefined)
        {
            IQueryable<ReportOnlineModel> query = db.RTVOEEProductionData
                .Where(x => x.Deleted == false &&
                            x.MachineId == machineId &&
                            (dateFrom <= x.ProductionDate && x.ProductionDate < dateTo) &&
                            //((reasonType == null && x.ReasonType != null & x.ReasonType.EntryType >= EnumEntryType.StopPlanned) || (reasonType != null && x.ReasonTypeId == reasonType.Id)))
                            //(x.EntryType == entryType || (entryType == EnumEntryType.Undefined && x.EntryType >= EnumEntryType.StopPlanned)))
                            (x.ReasonType != null && (x.ReasonType.EntryType >= EnumEntryType.StopPlanned || x.ReasonType.EntryType == EnumEntryType.TimeClosed)))
                            .GroupBy(x => new { x.Reason, x.ReasonType })
                .Select(st => new ReportOnlineModel()
                {
                    ReasonId = st.Key.Reason != null ? (int?)st.Key.Reason.Id : null,
                    ReasonName = st.Key.Reason != null ? st.Key.Reason.Name : st.Key.ReasonType != null ? st.Key.ReasonType.Name :string.Empty,
                    ReasonTypeName = st.Key.ReasonType != null? st.Key.ReasonType.Name : string.Empty,
                    ReasonTypeId = st.Key.ReasonType != null? (int?)st.Key.ReasonType.Id : null,
                    ReasonTypeEntryType = st.Key.ReasonType != null? st.Key.ReasonType.EntryType : EnumEntryType.Undefined,
                    //EntryType = st.Key.EntryType,
                    UsedTime = st.Sum(x => x.UsedTime),
                    ProductionDate = dateFrom,
                    ProdQty = st.Sum(x => x.ProdQty)
                });

            return query.ToList();
        }
        public List<ReportOnlineModel> GetProductionDataForReportOnline(DateTime dateFrom, DateTime dateTo, int machineId) //, ReasonType reasonType = null)//EnumEntryType entryType = EnumEntryType.Undefined)
        {
            IQueryable<ReportOnlineModel> query = db.RTVOEEProductionData
                .Where(x => x.Deleted == false &&
                            x.MachineId == machineId &&
                            (dateFrom <= x.ProductionDate && x.ProductionDate < dateTo)) //&&
                            //((reasonType ==  null && x.ReasonType.EntryType < EnumEntryType.StopPlanned) || x.ReasonTypeId == reasonType.Id)) 
                            //(x.EntryType == entryType || (entryType == EnumEntryType.Undefined && x.EntryType < EnumEntryType.StopPlanned)))
                            .GroupBy(x => new { x.Reason, x.ReasonType})
                .Select(st => new ReportOnlineModel()
                {
                    ReasonId = st.Key.Reason != null ? (int?)st.Key.Reason.Id : null,
                    ReasonName = st.Key.Reason != null ? st.Key.Reason.Name : string.Empty,
                    //EntryType = st.Key.EntryType,
                    ReasonTypeName = st.Key.ReasonType != null ? st.Key.ReasonType.Name : string.Empty,
                    ReasonTypeId = st.Key.ReasonType != null ? (int?)st.Key.ReasonType.Id : null,
                    ReasonTypeEntryType = st.Key.ReasonType != null ? st.Key.ReasonType.EntryType : EnumEntryType.Undefined,
                    UsedTime = st.Sum(x => x.UsedTime),
                    ProductionDate = dateFrom,
                    ProdQty = st.Sum(x => x.ProdQty)
                });

            return query.ToList();
        }

    }
}