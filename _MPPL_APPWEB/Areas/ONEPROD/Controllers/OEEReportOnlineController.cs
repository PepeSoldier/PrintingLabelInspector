using _MPPL_WEB_START.Areas.ONEPROD.OEE.ViewModels;
using MDL_ONEPROD.ComponentOEE.Models;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Repo;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using XLIB_COMMON.Enums;
using MDL_BASE.ComponentBase.Repos;
using System.Linq;
using MDL_ONEPROD.Model.Scheduling;
using _MPPL_WEB_START.Areas._APPWEB.Models;
using MDL_ONEPROD.Model.Scheduling.Interface;
using _MPPL_WEB_START.Areas._APPWEB.ViewModels;
using MDL_BASE.ComponentBase.Entities;
using Microsoft.AspNet.Identity;
using MDL_BASE.Models.IDENTITY;
using MDL_ONEPROD.ComponentRTV._Interfaces;
using MDL_ONEPROD.ComponentMes.Models;
using MDL_ONEPROD.ComponentMes.ViewModels;
using System.Threading;
using XLIB_COMMON.Model;

namespace _MPPL_WEB_START.Areas.ONEPROD.Controllers
{
    [Authorize(Roles = DefRoles.ONEPROD_ADMIN + "," + DefRoles.ONEPROD_MES_OPERATOR + "," + DefRoles.ONEPROD_MES_SUPEROPERATOR)]
    public class OEEReportOnlineController : Controller
    {
        UnitOfWorkOneProdRTV uowRTV;
        UnitOfWorkOneProdOEE uowOEE;
        UnitOfWorkOneProdMes uowMes;
        IDbContextOneprodMes dbMes;
        IDbContextOneProdOEE dbOee;

        public OEEReportOnlineController(IDbContextOneProdOEE dbOEE, IDbContextOneProdRTV dbRTV, IDbContextOneprodMes dbMes)
        {
            uowOEE = new UnitOfWorkOneProdOEE(dbOEE);
            uowRTV = new UnitOfWorkOneProdRTV(dbRTV);    
            uowMes = new UnitOfWorkOneProdMes(dbMes);
            this.dbMes = dbMes;
            this.dbOee = dbOEE;
            ViewBag.Skin = "nasaSkin";
        }

        public ActionResult Index(int resourceId, int workplaceId)
        {
            int[] reportOnlineAllowedMachinesIDs = new int[] { 0 };
            
            //If dodany, bo czasami wywalało null reference exception
            if(AppClient.appClient != null && AppClient.appClient.SettingsONEPROD != null)
            {
                reportOnlineAllowedMachinesIDs = AppClient.appClient.SettingsONEPROD.ReportOnlineAllowedMachinesIDs;
                ViewBag.ReportOnlinePillHeigh = AppClient.appClient.SettingsONEPROD.ReportOnlinePillHeigh;
                ViewBag.ReportOnlinePillFontSize = AppClient.appClient.SettingsONEPROD.ReportOnlinePillFontSize;
                ViewBag.MesWorkplaceVerifyIP = AppClient.appClient.SettingsONEPROD.MesWorkplaceVerifyIP;
            }
            else
            {
                ViewBag.ReportOnlinePillHeigh = 29;
                ViewBag.ReportOnlinePillFontSize = 18;
                ViewBag.MesWorkplaceVerifyIP = false;
            }

            ViewBag.IsReportOnlineEnabled = reportOnlineAllowedMachinesIDs.Contains(0) || reportOnlineAllowedMachinesIDs.Contains(resourceId);
            ViewBag.ResourceId = resourceId;
            ViewBag.WorkplaceId = workplaceId;
            ViewBag.ReasonTypes = uowOEE.ReasonTypeRepo.GetListStoppages().ToList();

            return View();
        }

        [HttpGet]
        public ActionResult ReportOnline(DateTime dateFrom, DateTime dateTo, int machineId, int reportId = 0)
        {
            ReportOnlineViewModel vm = new ReportOnlineViewModel();
            vm.Report = GetReport(dateFrom, dateTo, machineId, reportId);
            vm.RegisteredProduction = uowRTV.RTVOEEProductionDataRepo.GetDataForReport_Prod(dateFrom, dateTo, machineId);
            vm.DeclaredProduction = uowOEE.OEEReportProductionDataRepo.GetListByReportId(vm.Report.Id).ToList();

            return View(vm);
        }
        public OEEReport GetReport(DateTime dateFrom, DateTime dateTo, int machineId, int reportId = 0)
        {
            OEEReport oeeReport = uowOEE.OEEReportRepo.GetById(reportId);

            if(oeeReport == null)
            {
                oeeReport = FindOrCreateReport(dateFrom, machineId);
            }
            return oeeReport;
        }

        [HttpGet]
        public JsonResult GetReportId(DateTime dateFrom, DateTime dateTo, int machineId)
        {
            OEEReport oeeReport = FindOrCreateReport(dateFrom, machineId);

            return Json(oeeReport.Id, JsonRequestBehavior.AllowGet);
        }
        private OEEReport FindOrCreateReport(DateTime dateFrom, int machineId)
        {
            Shift shift = CalculateShiftByStartDate(dateFrom);
            OEEReport oeeReport = uowOEE.OEEReportRepo.GetByMachineIdDateAndShift(machineId, dateFrom.Date, shift);
            oeeReport = oeeReport != null ? oeeReport : CreateReport(machineId, dateFrom, shift);

            return oeeReport;
        }
        
        private OEEReport FindReport(DateTime dateFrom, int machineId)
        {
            Shift shift = CalculateShiftByStartDate(dateFrom);
            OEEReport oeeReport = uowOEE.OEEReportRepo.GetByMachineIdDateAndShift(machineId, dateFrom.Date, shift);
            return oeeReport;
        }
        private OEEReport CreateReport(int resourceId, DateTime dateFrom, Shift shift)
        {
            OEEReport draft = new OEEReport();
            draft.Shift = shift;
            draft.ReportDate = dateFrom.Date;
            draft.TimeStamp = DateTime.Now;
            draft.UserId = User.Identity.GetUserId();
            draft.IsDraft = false;
            draft.MachineId = resourceId;
            uowOEE.OEEReportRepo.AddOrUpdate(draft);

            return draft;
        }

        public JsonResult SaveReport(int resourceId, int workplaceId,int brigadeId, DateTime dateFrom, DateTime dateTo)
        {
            //2020.03.17 Zabezpieczenie przez zapisaniem pustych raportów

            if (User.Identity.IsAuthenticated)
            {
                string userId = User.Identity.GetUserId();
                int declaredQtyByOperator = SaveReport_(resourceId, workplaceId, brigadeId, dateFrom, dateTo, userId);
                return Json(declaredQtyByOperator);
            }
            else
            {
                return Json("Błąd lgowania - zaloguj się");
            }
        }

        public int SaveReport_(int resourceId, int workplaceId, int brigadeId, DateTime dateFrom, DateTime dateTo, string userId)
        {
            decimal totalQtyCountedOnline = uowRTV.RTVOEEProductionDataRepo.GetProducedQty(dateFrom, dateTo, resourceId);
            decimal totalDeclaredByOperator = uowMes.ProductionLogRepo.GetProductionSumForReportOnline(dateFrom, dateTo, workplaceId);

            if ((totalQtyCountedOnline > 0 || totalDeclaredByOperator > 0))
            {
                OEEReport report = FindOrCreateReport(dateFrom, resourceId);
                uowOEE.OEEReportProductionDataRepo.AddAvailableTimeEntryIfNotExists(report, userId);

                if (report != null && report.Id > 0)
                {
                    using (var transaction = uowOEE.BeginTransaction())
                    {
                        try
                        {
                            List<OEEReportProductionData> reportProdDataProductionList = uowOEE.OEEReportProductionDataRepo.GetListOfProductionByReportId(report.Id).ToList();
                            SaveProduction(workplaceId, dateFrom, dateTo, report, reportProdDataProductionList);
                            DeleteUnusedRecordsOfReportOnline(reportProdDataProductionList);

                            List<OEEReportProductionData> reportProdDataStoppagesList = uowOEE.OEEReportProductionDataRepo.GetListOfStoppagesByReportId(report.Id).ToList();
                            List<ReportOnlineModel> stoppagesReportOnlineList = uowRTV.RTVOEEProductionDataRepo.GetStoppageDataForReportOnline(dateFrom, dateTo, resourceId);
                            SaveStoppages(resourceId, dateFrom, dateTo, report, reportProdDataStoppagesList, stoppagesReportOnlineList);
                            DeleteUnusedRecordsOfReportOnline(reportProdDataStoppagesList);

                            report.TotalQtyCountedOnline = (int)uowRTV.RTVOEEProductionDataRepo.GetProducedQty(dateFrom, dateTo, resourceId);
                            report.TotalQtyDeclaredByOperator = (int)reportProdDataProductionList.Sum(x => x.ProdQty);
                            report.TotalStoppageTimeCountedOnline = (int)stoppagesReportOnlineList.Where(x => x.ReasonTypeEntryType >= EnumEntryType.StopPlanned).Sum(x => x.UsedTime);
                            report.TotalStoppageTimeDeclaredByOperator = (int)reportProdDataStoppagesList.Where(x => x.ReasonTypeEntryType >= EnumEntryType.StopPlanned).Sum(x => x.UsedTime);
                            report.LabourBrigadeId = brigadeId;
                            uowOEE.OEEReportProductionDataRepo.AddOrUpdate(report);

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            Logger2FileSingleton.Instance.SaveLog("OEEReportOnlineController.SaveReport_ " + ex.Message);
                            transaction.Rollback();
                        }
                    }

                    //Thread.Sleep(2500);
                    //var c = new OEECreateReportController(dbOee);
                    //c.ControllerContext = this.ControllerContext;
                    //c.VerifyReport(report.Id);
                    OeeHelper._VerifyReport(uowOEE, report.Id, User.Identity.GetUserId());

                    return report.TotalQtyDeclaredByOperator;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return -1;
            }
        }
        private void DeleteUnusedRecordsOfReportOnline(List<OEEReportProductionData> reportProdDataList)
        {
            List<OEEReportProductionData> reportProdDataUnusedList = reportProdDataList
                                .Where(x => x.EntryStatus == OEEProdDataEntryStatus.Deleted && 
                                x.ReasonType.EntryType != EnumEntryType.TimeAvailable).ToList();

            foreach (OEEReportProductionData unusedData in reportProdDataUnusedList)
            {
                unusedData.Deleted = true;
                uowOEE.OEEReportProductionDataRepo.AddOrUpdate(unusedData);
                //uowOEE.OEEReportProductionDataRepo.Delete(unusedData);
                reportProdDataList.RemoveAll(x=>x.Id == unusedData.Id);
            }
        }
        private void SaveProduction(int workplaceId, DateTime dateFrom, DateTime dateTo, OEEReport report, List<OEEReportProductionData> reportProdDataList)
        {
            List<ReportOnlineModel> productionLogReportOnlineList = uowMes.ProductionLogRepo
                                .GetProductionDataForReportOnline(dateFrom, dateTo, workplaceId);

            foreach (ReportOnlineModel rom in productionLogReportOnlineList)
            {
                rom.ReportId = report.Id;
                OEEReportProductionData reportProdData = reportProdDataList
                    .FirstOrDefault(x =>
                        x.ItemId == rom.ItemId &&
                        x.ReasonId == rom.ReasonId &&
                        //x.ReasonType.EntryType == rom.ReasonTypeEntryType &&
                        x.ReasonTypeId == rom.ReasonTypeId);


                if (reportProdData == null)
                {
                    OEEReportProductionData pd = uowOEE.OEEReportProductionDataRepo.AddOrUpdateProductionData(rom, User.Identity.GetUserId());
                    pd.EntryStatus = OEEProdDataEntryStatus.New;
                    reportProdDataList.Add(pd);
                }
                else
                {
                    reportProdData.EntryStatus = OEEProdDataEntryStatus.Existing;
                    rom.Id = reportProdData.Id;
                    OEEReportProductionData pd = uowOEE.OEEReportProductionDataRepo.AddOrUpdateProductionData(rom, User.Identity.GetUserId());
                }
            }
        }
        private void SaveStoppages(int resourceId, DateTime dateFrom, DateTime dateTo, OEEReport report, List<OEEReportProductionData> reportProdDataList, List<ReportOnlineModel> stoppagesReportOnlineList)
        {
            foreach (ReportOnlineModel rom in stoppagesReportOnlineList)
            {
                rom.ReportId = report.Id;
                OEEReportProductionData reportProdData = reportProdDataList
                    .FirstOrDefault(x =>
                        x.ReasonId == rom.ReasonId &&
                        //x.ReasonType.EntryType == rom.ReasonTypeEntryType &&
                        x.ReasonTypeId == rom.ReasonTypeId);

                if (reportProdData == null)
                {
                    OEEReportProductionData pd = uowOEE.OEEReportProductionDataRepo.AddOrUpdateStoppageData(rom, User.Identity.GetUserId());
                    pd.EntryStatus = OEEProdDataEntryStatus.New;
                    reportProdDataList.Add(pd);
                }
                else
                {
                    reportProdData.EntryStatus = OEEProdDataEntryStatus.Existing;
                    rom.Id = reportProdData.Id;
                    OEEReportProductionData pd = uowOEE.OEEReportProductionDataRepo.AddOrUpdateStoppageData(rom, User.Identity.GetUserId());
                }
            }
        }

        [HttpPost]
        public JsonResult GetStoppages(DateTime dateFrom, DateTime dateTo, int resourceId, int stoppageId = 0)
        {
            var list = uowRTV.RTVOEEProductionDataRepo
                .GetDataByTimeRangeAndMachineId(dateFrom, dateTo, resourceId)
                .Where(x => 
                    (stoppageId == 0 || x.Id == stoppageId) &&
                    (x.ReasonType.EntryType == EnumEntryType.TimeClosed || x.ReasonType.EntryType >= EnumEntryType.StopPlanned))
                .Select(st => new {
                    st.Id,
                    ReasonTypeId = st.ReasonType.Id,
                    ReasonTypeName = st.ReasonType.Name,
                    st.ReasonType.EntryType,
                    st.ProductionDate,
                    st.TimeStamp,
                    st.UsedTime,
                    ReasonId = st.Reason != null? st.Reason.Id : 0,
                    ResonName = st.Reason != null? st.Reason.Name : st.ReasonType != null? st.ReasonType.Name : "?"
                })
                .ToList();

            return Json(list);
        }
        [HttpPost]
        public JsonResult SetStoppageReason(int id, int? reasonId, int reasonTypeId)
        {
            //ReasonType reasonType = uowOEE.ReasonTypeRepo.GetById(reasonTypeId);
            //Reason reason = uowOEE.ReasonRepo.GetById(reasonId);

            //if (reason != null)
            //{
                RTVOEEProductionData st = uowRTV.RTVOEEProductionDataRepo.GetById(id);
                st.ReasonId = reasonId;
                st.ReasonTypeId = reasonTypeId;
                st.UserId = User.Identity.GetUserId();
                //st.EntryType = entrytype;
                uowRTV.RTVOEEProductionDataRepo.AddOrUpdate(st);

                return Json(0);
            //}
            //else
            //{
            //    return Json(-1);
            //}
             
        }
        [HttpPost]
        public JsonResult GetReportOnline(DateTime dateFrom, DateTime dateTo, int resourceId, int workplaceId)
        {
            List<ReportOnlineModel> productionLogList = uowMes.ProductionLogRepo
                .GetProductionDataForReportOnline(dateFrom, dateTo, workplaceId)
                .ToList();
            
            List<ReportOnlineModel> stoppagesSummaryList = uowRTV.RTVOEEProductionDataRepo
                .GetStoppageDataForReportOnline(dateFrom, dateTo, resourceId);

            //List<ReportOnlineModel> productionReportOnlineList = uowRTV.RTVOEEProductionDataRepo.GetProductionDataForReportOnline(dateFrom, dateTo, resourceId);
            //OEEReport report = FindReport(dateFrom, resourceId);            
            List<OEEReportProductionData> OEEprodData = uowOEE.OEEReportProductionDataRepo.GetShiftData(dateFrom.Date, CalculateShiftByStartDate(dateFrom), resourceId);
            
            foreach(ReportOnlineModel rom in productionLogList)
            {
                rom.ReportedProdQty = OEEprodData
                    .Where(x => (x.Item != null && x.Item.Code == rom.ItemCode) && 
                            //(x.ReasonType.EntryType == rom.ReasonTypeEntryType) &&
                            (x.ReasonTypeId == rom.ReasonTypeId))
                    .Sum(x => x.ProdQty);
            }

            //if(report != null)
            //{
            //    //List<OEEReportProductionData> reportProdDataProductionList = uowOEE.OEEReportProductionDataRepo.GetListOfProductionByReportId(report.Id).ToList();
            //    //productionLogList.Where(x => x.EntryType == EnumEntryType.Production).FirstOrDefault().ReportedProdQty = report
            //    productionLogList.Where(x => x.EntryType == EnumEntryType.Production).FirstOrDefault().ReportedProdQty = report.TotalQtyDeclaredByOperator;
            //}

            decimal countedByPLC = uowRTV.RTVOEEProductionDataRepo.GetProducedQty(dateFrom, dateTo, resourceId);
            stoppagesSummaryList.RemoveAll(x => x.ReasonTypeEntryType < EnumEntryType.StopPlanned);

            return Json(new { stoppageSummary = stoppagesSummaryList, productionLogs = productionLogList, countedByPLC});
        }


        public ActionResult ReportOnlineReason()
        {
            return View("ReportOnline.Reason");
        }
        public JsonResult GetReasons(int resourceId)
        {
            List<ReasonSelectorItem> reasons;
            reasons = new List<ReasonSelectorItem>();
            //r.Add(new ReasonSelectorItem() { Id = -1 * (int)EnumEntryType.Production, Name = "Produkcja OK", ParentId = 0 });
            ////r.Add(new ReasonSelectorItem() { Id = -1 * (int)EnumEntryType.ScrapMaterial, Name = "Scrap Materiałowy", ParentId = 0 });
            //r.Add(new ReasonSelectorItem() { Id = -1 * (int)EnumEntryType.ScrapProcess, Name = "Scrap Procesowy", ParentId = 0 });
            //r.Add(new ReasonSelectorItem() { Id = -1 * (int)EnumEntryType.ScrapLabel, Name = "Scrap Etykiety", ParentId = 0 });
            //r.Add(new ReasonSelectorItem() { Id = -1 * (int)EnumEntryType.StopPlanned, Name = "Postoje Planowane", ParentId = 1 });
            ////r.Add(new ReasonSelectorItem() { Id -1 * = (int)EnumEntryType.StopPlannedChangeOver, Name = "Przezbrojenia Planowane", ParentId = 1 });
            //r.Add(new ReasonSelectorItem() { Id = -1 * (int)EnumEntryType.StopUnplanned, Name = "Postoje Nieplanowane", ParentId = 1 });
            ////r.Add(new ReasonSelectorItem() { Id = -1 * (int)EnumEntryType.StopUnplannedBreakdown, Name = "Awarie", ParentId = 1 });
            ////r.Add(new ReasonSelectorItem() { Id = -1 * (int)EnumEntryType.StopUnplannedChangeOver, Name = "Przezbrojenia", ParentId = 1 });
            ////r.Add(new ReasonSelectorItem() { Id = -1 * (int)EnumEntryType.StopUnplannedPreformance, Name = "Mikroprzestoje", ParentId = 1 });

            reasons.AddRange(uowOEE.ReasonTypeRepo.GetList()
                .Where(x=>x.EntryType > EnumEntryType.TimeAvailable)
                .Select(x => new ReasonSelectorItem()
                {
                    Id = -1 * x.Id,
                    Name = x.Name,
                    ParentId = x.EntryType >= EnumEntryType.StopPlanned || x.EntryType == EnumEntryType.TimeClosed? 1 : 0,
                    ColorGroup = x.Color,
                    ReasonTypeId = x.Id,
                    ReasonTypeName = x.Name
                })
                .ToList()
            );

            reasons.AddRange(uowOEE.MachineReasonRepo.GetByMachineId(resourceId)
                .Select(x => new ReasonSelectorItem() {
                    Id = x.Reason.Id,
                    Name = x.Reason.Name,
                    ParentId = x.Reason.GroupId != null ? (int)x.Reason.GroupId : (int)x.Reason.ReasonTypeId * -1,
                    Color = x.Reason.Color,
                    ColorGroup = x.Reason.ColorGroup,
                    GroupId = x.Reason.GroupId != null? (int)x.Reason.GroupId : 0,
                    GroupName = x.Reason.GroupId != null? x.Reason.Group.Name : "",
                    ReasonTypeId = x.Reason.ReasonTypeId,
                    ReasonTypeName = x.Reason.ReasonType.Name
                })
                .OrderBy(x => x.GroupName)
                .ThenBy(x => x.Name)
                .ToList()
            );

            foreach(ReasonSelectorItem rsi in reasons)
            {
                rsi.SubreasonsCount = reasons.Count(x => x.ParentId == rsi.Id);

                if(rsi.ParentId > 1 && reasons.Count(x => x.Id == rsi.ParentId) == 0)
                {
                    rsi.ParentId = rsi.ReasonTypeId * -1;
                }
            }
            
            return Json(reasons, JsonRequestBehavior.AllowGet);
        }

        public ActionResult StopSplit(int stoppageId)
        {
            RTVOEEProductionData stoppage = uowRTV.RTVOEEProductionDataRepo.GetById(stoppageId);

            return View(stoppage);
        }
        [HttpPost]
        public JsonResult StopSplit(int stoppageId, decimal secondsLeft, decimal secondsRight)
        {
            //decimal totalStoppageTimeSeconds = stoppage.UsedTime;

            RTVOEEProductionData stoppage = uowRTV.RTVOEEProductionDataRepo.GetById(stoppageId);

            if(stoppage == null)
                return Json(new JsonModel() { MessageType = JsonMessageType.danger, Message = "Nie udało się wykonać operacji" });

            if((DateTime.Now - stoppage.TimeStamp).TotalSeconds < 60)
                return Json(new JsonModel() { MessageType = JsonMessageType.danger, Message = "Postój można podzielić po zakończeniu naliczania czasu" });

            stoppage.TimeStamp = stoppage.ProductionDate.AddSeconds((double)secondsLeft);
            stoppage.UsedTime = secondsLeft;
            
            uowRTV.RTVOEEProductionDataRepo.AddOrUpdate(stoppage);
            
            RTVOEEProductionData stoppageNew = new RTVOEEProductionData();
            stoppageNew.MachineId = stoppage.MachineId;
            stoppageNew.ProdQty = 0;
            stoppageNew.ProdQtyTotal = -1;
            stoppageNew.ProdQtyShift = -1;
            stoppageNew.Deleted = false;
            stoppageNew.CycleTime = 0;
            stoppageNew.UsedTime = secondsRight;
            stoppageNew.ProductionDate = stoppage.TimeStamp;
            stoppageNew.TimeStamp = stoppage.TimeStamp.AddSeconds((double)secondsRight);
            stoppageNew.UserId = User.Identity.GetUserId();
            stoppageNew.ReasonTypeId = (int)EnumEntryType.ReasonNotSelected;
            stoppageNew.PiecesPerPallet = stoppage.PiecesPerPallet;
            
            uowRTV.RTVOEEProductionDataRepo.AddOrUpdate(stoppageNew);

            return Json(new JsonModel() { MessageType = JsonMessageType.success, Message = "Postój został podzielony" });
        }

        public ActionResult ChangeDeclarationDate(int productionLogId)
        {
            ProductionLog prodLog = uowMes.ProductionLogRepo.GetById(productionLogId);
            ViewBag.NewDate = CalculateNewDate(prodLog.TimeStamp);
            return View(prodLog);
        }
        [HttpPost]
        public JsonResult ChangeDeclarationDateExecute(int productionLogId)
        {
            ProductionLog prodLog = uowMes.ProductionLogRepo.GetById(productionLogId);
            prodLog.TimeStamp = CalculateNewDate(prodLog.TimeStamp);
            prodLog.UserName = User.Identity.Name;
            uowMes.ProductionLogRepo.AddOrUpdate(prodLog);

            return Json(0);
        }

        private DateTime CalculateNewDate(DateTime timeStamp)
        {
            DateTime ts_temp = timeStamp;
            DateTime ts_date_temp = timeStamp.Date;

            if (ts_temp.Hour > 22)
            {
                timeStamp = ts_date_temp.AddHours(21).AddMinutes(59).AddSeconds(59);
            }
            else if (ts_temp.Hour < 6)
            {
                timeStamp = ts_date_temp.AddDays(-1).AddHours(21).AddMinutes(59).AddSeconds(59);
            }
            else if (ts_temp.Hour > 14)
            {
                timeStamp = ts_date_temp.AddHours(13).AddMinutes(59).AddSeconds(59);
            }
            else
            {
                timeStamp = ts_date_temp.AddHours(5).AddMinutes(59).AddSeconds(59);
            }

            return timeStamp;
        }

        private static Shift CalculateShiftByStartDate(DateTime dateFrom)
        {
            return dateFrom.Hour >= 22 ? Shift.III_zmiana : dateFrom.Hour >= 14 ? Shift.II_zmiana : Shift.I_zmiana;
        }
    }
}