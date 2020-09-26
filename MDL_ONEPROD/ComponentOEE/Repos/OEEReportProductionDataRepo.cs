using XLIB_COMMON.Repo;
using MDL_ONEPROD.ComponentOEE.Models;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Model.Scheduling.Interface;
using MDL_ONEPROD.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using XLIB_COMMON.Enums;
using MDL_ONEPROD.ComponentMes.Models;
using MDL_BASE.Interfaces;

namespace MDL_ONEPROD.ComponentOEE.Repos
{
    public class OEEReportProductionDataRepo : RepoGenericAbstract<OEEReportProductionData>
    {
        protected new IDbContextOneProdOEE db;
        UnitOfWorkOneProdOEE unitOfWork;

        public OEEReportProductionDataRepo(IDbContextOneProdOEE db, XLIB_COMMON.Interface.IAlertManager alertManager, UnitOfWorkOneProdOEE unitOfWork = null)
           : base(db, alertManager)
        {
            this.db = db;
            this.unitOfWork = unitOfWork;
        }

        public override OEEReportProductionData GetById(int id)
        {
            return db.OEEReportProductionData.FirstOrDefault(x => x.Id == id);
        }
        public override IQueryable<OEEReportProductionData> GetList()
        {
            return db.OEEReportProductionData.Where(x => (x.Deleted == false && x.Report.IsDraft == false)).OrderByDescending(x => x.Id);
        }

        public OEEReportProductionData GetAvailableTimeByReportId(int reportId)
        {
            return db.OEEReportProductionData
                .FirstOrDefault(x =>
                    (x.Deleted == false) &&
                    (x.ReportId == reportId) &&
                    (x.ReasonType.EntryType == EnumEntryType.TimeAvailable));
        }

        public List<OEEReportProductionData> GetShiftData(DateTime date, Shift shift, int machineId)
        {
            return db.OEEReportProductionData
                .Where(x => (x.Deleted == false && x.Report.IsDraft == false) &&
                            x.Report.ReportDate == date && 
                            x.Report.Shift == shift &&
                            x.Report.MachineId == machineId)
                .OrderByDescending(x => x.Id)
                .ToList();
        }
        public IQueryable<OEEReportProductionData> GetDataByTimeRangeAndMachineId(DateTime dateFrom, DateTime dateTo, int machineId = -1, int labourBrigadeId = -1, int? reasonTypeId = null) //EnumEntryType entryType = EnumEntryType.Undefined)
        {
            return db.OEEReportProductionData
                .Where(x => (x.Deleted == false && x.Report.IsDraft == false) &&
                            x.ProductionDate >= dateFrom &&
                            x.ProductionDate < dateTo &&
                            (machineId == -1 || x.Report.MachineId == machineId) &&
                            (labourBrigadeId == -1 || x.Report.LabourBrigadeId == labourBrigadeId) &&
                            //(x.EntryType == entryType || entryType == EnumEntryType.Undefined))
                            (reasonTypeId == null || x.ReasonTypeId == reasonTypeId))
                .OrderByDescending(x => x.Id);
        }
        public List<OEEReportProductionData> GetDataByTimeRangeAndMachineIdAndReasonId(DateTime dateFrom, DateTime dateTo, int machineId, int reasonId, int labourBrigadeId = -1)
        {
            return db.OEEReportProductionData
                .Where(x => (x.Deleted == false && x.Report.IsDraft == false) &&
                            x.ProductionDate >= dateFrom &&
                            x.ProductionDate < dateTo &&
                            x.Report.MachineId == machineId &&
                            (labourBrigadeId == -1 || x.Report.LabourBrigadeId == labourBrigadeId) &&
                            x.ReasonId == reasonId)
                .OrderByDescending(x => x.Id)
                .ToList();
        }
        public List<OEEReportProductionData> GetShiftDataTest()
        {
            return db.OEEReportProductionData
                    .Where(x => (x.Deleted == false && x.Report.IsDraft == false))
                    .OrderByDescending(x => x.Id)
                    .ToList();
        }

        public IQueryable<OEEReportProductionData> GetListByReportId(int reportId)
        {
            return db.OEEReportProductionData.Include(x => x.ReasonType)
                .Where(x => (x.Deleted == false) &&
                            x.ReportId == reportId)
                .OrderBy(x => x.Id);
        }
        public IQueryable<OEEReportProductionData> GetListByReportId(int reportId, int? reasonTypeId)
        {
            return db.OEEReportProductionData
                .Where(x => (x.Deleted == false) &&
                            x.ReportId == reportId &&
                            //x.EntryType == entryType)
                            (x.ReasonTypeId == reasonTypeId))
                .OrderBy(x => x.Id);
        }
        public IQueryable<OEEReportProductionData> GetListOfProductionByReportId(int reportId)
        {
            return db.OEEReportProductionData
                .Where(x => (x.Deleted == false) &&
                            (x.ReportId == reportId) &&
                            (EnumEntryType.Production <= x.ReasonType.EntryType && 
                            x.ReasonType.EntryType < EnumEntryType.StopPlanned))
                .OrderBy(x => x.Id);
        }
        public IQueryable<OEEReportProductionData> GetListOfStoppagesByReportId(int reportId)
        {
            return db.OEEReportProductionData.Where(x => 
                    (x.Deleted == false) && 
                    (x.ReportId == reportId) &&
                    (x.ReasonType.EntryType >= EnumEntryType.StopPlanned ||
                    x.ReasonType.EntryType == EnumEntryType.TimeClosed))
                .OrderBy(x => x.Id);
        }
        public IQueryable<OEEReportProductionData> GetListBy_DateRange_Machine_Type(DateTime dateFrom, DateTime dateTo, int machineId, int? reasonTypeId = null, int labourBrigadeId = -1)
        {
            return db.OEEReportProductionData
                .Where(x => (x.Deleted == false && x.Report.IsDraft == false) &&
                            x.Report.MachineId == machineId &&
                            (x.ProductionDate >= dateFrom && x.ProductionDate < dateTo) &&
                            (labourBrigadeId == -1 || x.Report.LabourBrigadeId == labourBrigadeId) &&
                            (reasonTypeId == null || x.ReasonTypeId == reasonTypeId)) //&&
                            //x.EntryType == entryType)
                .OrderBy(x => x.Id);
        }
        
        public void UpdateProductionDateForReportEntries(int reportId, int newMachineId, DateTime newDate, Shift newShift)
        {
            int hour = (((int)newShift - 1) * 8) + 6;
            DateTime date = newDate.Date.AddHours(hour);

            List<OEEReportProductionData> data = GetListByReportId(reportId).ToList();

            foreach(OEEReportProductionData d in data)
            {
                d.Report.MachineId = newMachineId;
                d.ProductionDate = date;
                AddOrUpdate(d);
            }
        }
        public List<DateTime> GetListOfOpenShifts(DateTime dateFrom, DateTime dateTo, int machineId, int labourBrigadeId)
        {
            return db.OEEReportProductionData
                .Where(x => (x.Deleted == false && x.Report.IsDraft == false) &&
                            x.Report.MachineId == machineId &&
                            (labourBrigadeId == -1 || x.Report.LabourBrigadeId == labourBrigadeId) &&
                            (x.ProductionDate >= dateFrom && x.ProductionDate < dateTo) &&
                            x.ReasonType.EntryType == 0)
                            .Select(x => x.ProductionDate).ToList();
        }
        public List<ParetoModel> GetParetoBy_Reason_UsedTime(DateTime dateFrom, DateTime dateTo, int reasonTypeId, int machineId, int labourBrigadeId)
        {
            var query = db.OEEReportProductionData
                .Where( x => (x.Deleted == false && x.Report.IsDraft == false) &&
                        //x.ReasonType.EntryType == entryType &&
                        x.ReasonTypeId == reasonTypeId &&
                        x.Report.Machine.Id == machineId &&
                        (labourBrigadeId == -1 || x.Report.LabourBrigadeId == labourBrigadeId) &&
                        x.ProductionDate >= dateFrom && x.ProductionDate < dateTo
                )
                .GroupBy(x => x.Reason)
                .Select(y => new ParetoModel()
                {
                    Name = y.Key.Name ?? "?",
                    NameEnglish = y.Key.NameEnglish?? "?",
                    Color = y.Key.Color,
                    Value = y.Sum(x => x.UsedTime)
                }).OrderByDescending(x => x.Value);

            return query.ToList();
        }
        public List<ParetoModel> GetParetoBy_ReasonType_UsedTime(DateTime dateFrom, DateTime dateTo, int machineId, int labourBrigadeId)
        {
            var query = db.OEEReportProductionData
                .Where(x => (x.Deleted == false && x.Report.IsDraft == false) &&
                       //x.ReasonTypeId == reasonTypeId &&
                       x.Report.Machine.Id == machineId &&
                       (labourBrigadeId == -1 || x.Report.LabourBrigadeId == labourBrigadeId) &&
                       x.ProductionDate >= dateFrom && x.ProductionDate < dateTo
                )
                .GroupBy(x => x.ReasonType)
                .Select(y => new ParetoModel()
                {
                    Name = y.Key.Name ?? "?",
                    NameEnglish = y.Key.NameEnglish ?? "?",
                    Color = y.Key.Color,
                    Value = y.Sum(x => x.UsedTime),
                    EntryType = (int)y.Key.EntryType
                }).OrderByDescending(x => x.Value);

            return query.ToList();
        }
        public List<ParetoModel> GetParetoBy_Scrap_Reason(DateTime dateFrom, DateTime dateTo, int machineId, int labourBrigadeId)
        {
            var query = db.OEEReportProductionData
                .Where(x => (x.Deleted == false && x.Report.IsDraft == false) &&
                       (x.ReasonId != null && EnumEntryType.ScrapMaterial <= x.ReasonType.EntryType && x.ReasonType.EntryType < EnumEntryType.ScrapLabel) &&
                       x.Report.Machine.Id == machineId &&
                       (labourBrigadeId == -1 || x.Report.LabourBrigadeId == labourBrigadeId) &&
                       (x.ProductionDate >= dateFrom && x.ProductionDate < dateTo)
                )
                .GroupBy(x => x.Reason)
                .Select(y => new ParetoModel()
                {
                    Name = (y.Key.Name).ToString(),
                    NameEnglish = (y.Key.NameEnglish).ToString(),
                    Color = (y.Key.Color).ToString(),
                    Value = y.Sum(x => x.ProdQty)
                }).OrderByDescending(x => x.Value);

            return query.ToList();
        }
        public List<ParetoModel> GetParetoBy_Scrap_ReasonType(DateTime dateFrom, DateTime dateTo, int machineId, int labourBrigadeId)
        {
            var query = db.OEEReportProductionData
                .Where(x => (x.Deleted == false && x.Report.IsDraft == false) &&
                       (EnumEntryType.ScrapMaterial <= x.ReasonType.EntryType && x.ReasonType.EntryType < EnumEntryType.ScrapLabel) &&
                       x.Report.Machine.Id == machineId &&
                       (labourBrigadeId == -1 || x.Report.LabourBrigadeId == labourBrigadeId) &&
                       (x.ProductionDate >= dateFrom && x.ProductionDate < dateTo)
                )
                .GroupBy(x => x.ReasonType)
                .Select(y => new ParetoModel()
                {
                    Name = (y.Key.Name).ToString(),
                    NameEnglish = (y.Key.Name).ToString(),
                    Color = (y.Key.Color).ToString(),
                    Value = y.Sum(x => x.ProdQty)
                }).OrderByDescending(x => x.Value);

            return query.ToList();
        }

        public List<ParetoModel> GetParetoBy_Scrap_ANC(DateTime dateFrom, DateTime dateTo, int machineId, int labourBrigadeId)
        {
            var query = db.OEEReportProductionData
                .Where(x => (x.Deleted == false && x.Report.IsDraft == false) &&
                       (EnumEntryType.ScrapProcess <= x.ReasonType.EntryType && x.ReasonType.EntryType < EnumEntryType.ScrapLabel) &&
                       x.Report.Machine.Id == machineId &&
                       (labourBrigadeId == -1 || x.Report.LabourBrigadeId == labourBrigadeId) &&
                       (x.ProductionDate >= dateFrom && x.ProductionDate < dateTo)
                )
                .GroupBy(x => x.Item.Code).ToList()
            .Select(y => new ParetoModel()
             {
                 Name = y.Key,
                 NameEnglish = y.Key,
                 Color = "blue",
                 Value = y.Sum(x => x.ProdQty)
             }).OrderByDescending(x => x.Value);
            
            return query.ToList();
        }

        public void UpdateAvailableTimeEntry(OEEReport report, string userId, int minutes)
        {
            if (report != null && report.Id > 0)
            {
                OEEReportProductionData pd = GetAvailableTimeByReportId(report.Id);

                if (pd == null)
                {
                    pd = new OEEReportProductionData();
                    int hour = (((int)report.Shift - 1) * 8) + 6;
                    pd.ReportId = report.Id;
                    pd.ReasonTypeId = unitOfWork.ReasonTypeRepo.GetIdByEntryType(EnumEntryType.TimeAvailable);
                    pd.UserId = userId;
                    pd.ProductionDate = report.ReportDate.Date.AddHours(hour);
                }
                else
                {
                    pd.TimeStamp = DateTime.Now;
                    pd.UsedTime = minutes;
                }

                AddOrUpdate(pd);
            }
        }
        public void AddAvailableTimeEntryIfNotExists(OEEReport report, string userId)
        {
            if (report != null && report.Id > 0)
            {
                OEEReportProductionData pd = GetAvailableTimeByReportId(report.Id);

                if (pd == null)
                {
                    pd = new OEEReportProductionData();
                    int hour = (((int)report.Shift - 1) * 8) + 6;

                    pd.ReportId = report.Id;
                    //pd.EntryType = EnumEntryType.TimeAvailable;
                    pd.ReasonTypeId = unitOfWork.ReasonTypeRepo.GetIdByEntryType(EnumEntryType.TimeAvailable);
                    pd.UsedTime = 8 * 60 * 60;
                    pd.TimeStamp = DateTime.Now;
                    pd.UserId = userId;
                    pd.ProductionDate = report.ReportDate.Date.AddHours(hour);
                    AddOrUpdate(pd);
                }
            }
        }
        public OEEReportProductionData AddOrUpdateProductionData(IOeeProductionData vm, string userId)
        {
            OEEReportProductionData pd = GetById(vm.Id);
            pd = (pd != null) ? pd : new OEEReportProductionData();

            pd.Id = vm.Id;
            pd.ReportId = vm.ReportId;
            //pd.Item = item;
            pd.ReasonId = vm.ReasonId > 0? vm.ReasonId : null;
            pd.CycleTime = vm.CycleTime;
            pd.ItemId = vm.ItemId;
            pd.ProdQty = (int) vm.ProdQty;
            //pd.EntryType = vm.EntryType;
            pd.ReasonType = null;
            pd.ReasonTypeId = vm.ReasonTypeId;
            pd.ProductionDate = vm.ProductionDate;
            pd.UsedTime = pd.ProdQty* pd.CycleTime;
            pd.TimeStamp = DateTime.Now;
            pd.UserId = userId;

            AddOrUpdate(pd);
            return pd;
        }
        public OEEReportProductionData AddOrUpdateStoppageData(IOeeProductionData vm, string userId)
        {
            
            OEEReportProductionData pd = GetById(vm.Id);
            pd = (pd != null) ? pd : new OEEReportProductionData();
            
            pd.ItemId = vm.ItemId;
            pd.ReportId = vm.ReportId;
            pd.ReasonId = vm.ReasonId;
            pd.UsedTime = vm.UsedTime;
            pd.ProductionDate = vm.ProductionDate;
            //pd.EntryType = vm.EntryType;
            pd.ReasonType = null;
            pd.ReasonTypeId = vm.ReasonTypeId;
            pd.TimeStamp = DateTime.Now;
            pd.UserId = userId;

            AddOrUpdate(pd);

            vm.Id = pd.Id;
            
            return pd;
        }

        public override void Delete(IModelEntity entity)
        {
            base.Delete(entity);
        }
    }
   
}