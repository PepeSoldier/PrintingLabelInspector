using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Repo;
using System.Collections.Generic;
using System.Web.Mvc;
using MDL_BASE.ComponentBase.Repos;
using System.Linq;
using _MPPL_WEB_START.Areas.ONEPROD.OEE.ViewModels;
using System;
using XLIB_COMMON.Enums;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.ComponentOEE.Models;
using _MPPL_WEB_START.Areas._APPWEB.ViewModels;
using MDL_ONEPROD.Model.Scheduling.Interface;
using MDL_BASE.Models.IDENTITY;
using _MPPL_WEB_START.Areas.ONEPROD.Models;
using _MPPL_WEB_START.Areas.ONEPROD.ViewModels.OEE;
using MDL_BASE.ComponentBase.Entities;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Threading;
using System.Globalization;
using MDL_ONEPROD.ComponentOEE.ViewModels;

namespace _MPPL_WEB_START.Areas.ONEPROD.Controllers
{
    //[Authorize(Roles = DefRoles.TechnologyUser)]
    public class OEEBrowseController : Controller
    {
        UnitOfWorkOneProdOEE uowOEE;
        UnitOfWorkOneprod uowONEPROD;
        RepoLabourBrigade repoLabourBrigade;

        public OEEBrowseController(IDbContextOneProdOEE db)
        {
            uowOEE = new UnitOfWorkOneProdOEE(db);
            repoLabourBrigade = new RepoLabourBrigade(db);
            uowONEPROD = new UnitOfWorkOneprod(db);
        }

        public ActionResult Index()
        {
            BrowseReportsViewModel vm = new BrowseReportsViewModel();
            vm.SelectedDate = DateTime.Now.Date;
            vm.SelectedShift = Shift.I_zmiana;
            vm.OeePartData = new List<OeePartData>();
            vm.Machines = uowONEPROD.ResourceRepo.GetListForDropDown();

            return View(vm);
        }

        [HttpPost]
        public JsonResult OeeReportGetList(DateTime dateFrom, DateTime dateTo, int machineId, int labourBrigadeId, int? reasonTypeId = null, int intervalInHours = 24)
        {
            List<BrowseOeeGridViewModel> browseOeeData = uowOEE.OEEReportProductionDataRepo
              .GetDataByTimeRangeAndMachineId(dateFrom, dateTo, machineId, labourBrigadeId, reasonTypeId)
              .OrderBy(x => x.ReasonType != null? x.ReasonType.EntryType : EnumEntryType.Undefined)
              .ToList()
              .Select(x => new BrowseOeeGridViewModel
              {
                  Id = x.Id,
                  ReportId = x.ReportId,
                  ItemCode = (x.Item != null ? x.Item.Code : ""),
                  ItemName = (x.Item != null ? x.Item.Name : ""),
                  ReasonName = (x.Reason != null ? x.Reason.Name : ""),
                  //EntryType = x.EntryType,
                  ReasonTypeName = x.ReasonType != null? x.ReasonType.Name : string.Empty,
                  ReasonTypeId = x.ReasonTypeId,
                  ReasonTypeEntryType = x.ReasonType != null ? x.ReasonType.EntryType : EnumEntryType.Undefined,
                  ProdQty = x.ProdQty,
                  EntryTimeStamp = x.TimeStamp.ToShortDateString(),
                  UsedTime = (decimal)x.UsedTime / (decimal)60,
                  CycleTime = x.CycleTime,
                  ProductionCycleTime = (x.Detail != null) ? x.Detail.ProductionCycleTime : 0,
                  Shift = x.Report.ReportDate.ToString("yyyy-MM-dd") + " #" + Convert.ToString((int)x.Report.Shift),
                  ProdDateFormatted = ChartDataPreparer.PrepareDataLabel(x.ProductionDate, intervalInHours, dateFrom, dateTo),
                  UserName = (x.User != null) ? x.User.UserName : "-",
              })
              .ToList();

            return Json(browseOeeData, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult IndexDetails(DateTime dateFrom, DateTime dateTo, int MachineId, int labourBrigadeId = -1)
        {
            BrowseReportsViewModel vm = new BrowseReportsViewModel();
            List<OEEReportProductionDataAbstract> productionData = uowOEE.OEEReportProductionDataRepo.GetDataByTimeRangeAndMachineId(dateFrom, dateTo, MachineId, labourBrigadeId).ToList<OEEReportProductionDataAbstract>();
            List<ReasonType> reasonTypes = uowOEE.ReasonTypeRepo.GetList().ToList();

            OeeCalculationModel oeeCalc = new OeeCalculationModel(productionData);
            oeeCalc.CalculateOEE();

            //vm.ProductionData = productionData;
            vm.AvailabilityResult = oeeCalc.Availability;
            vm.PerformanceResult = oeeCalc.Performance;
            vm.QualityResult = oeeCalc.Quality;
            vm.OeeResult = oeeCalc.Result;

            vm.OeePartData = new List<OeePartData>();

            if (productionData.Count > 0)
            {
                //vm.OeePartData.Add(new OeePartData
                //{
                //    Name = "Czas Dostępny",
                //    Qty = 0,
                //    TimeInMin = oeeCalc.ShiftTime / 60,
                //    CssClass = "EtReasonTypeId_1"

                //});

                //vm.OeePartData.Add(new OeePartData
                //{
                //    Name = "Produkcja",
                //    Qty = oeeCalc.GoodCount,
                //    TimeInMin = oeeCalc.GoodProductionTime / 60,
                //    CssClass = "EtReasonTypeId_10"
                //});
                //vm.OeePartData.Add(new OeePartData
                //{
                //    Name = "Scrap",
                //    Qty = oeeCalc.TotalCount - oeeCalc.GoodCount,
                //    TimeInMin = (oeeCalc.ScrapProcessTime) / 60, //TimeInMin = (oeeCalc.ScrapMaterialTime + oeeCalc.ScrapProcessTime) / 60,
                //    CssClass = "EtReasonTypeId_12"
                //});

                foreach(ReasonType rt in reasonTypes)
                {
                    vm.OeePartData.Add(new OeePartData
                    {
                        Name = rt.Name,
                        Qty = 0,
                        TimeInMin = oeeCalc.ProductionAnalyzer.ProductionQtyToTime(productionData, rt) / 60,
                        CssClass = "EtReasonTypeId_" + rt.Id,
                        Color = rt.Color
                    });
                }

                /*
                vm.OeePartData.Add(new OeePartData
                {
                    Name = "Postoje Nieplanowane",
                    Qty = 0,
                    TimeInMin = oeeCalc.StopUnplannedTime / 60,
                    CssClass = "EtStopUnplanned"
                });
                //vm.OeePartData.Add(new OeePartData
                //{
                //    Name = "Awarie",
                //    Qty = 0,
                //    TimeInMin = oeeCalc.StopUnplannedBreakdownsTime / 60,
                //    CssClass = "EtStopBreakdown"
                //});
                vm.OeePartData.Add(new OeePartData
                {
                    Name = "Postoje Planowane",
                    Qty = 0,
                    TimeInMin = oeeCalc.StopPlannedTime / 60,
                    CssClass = "EtStopPlanned"
                });
                //vm.OeePartData.Add(new OeePartData
                //{
                //    Name = "Przezbrojenia",
                //    Qty = 0,
                //    TimeInMin = oeeCalc.StopPlannedChangeOverTime / 60,
                //    CssClass = "EtStopChangeOver"
                //});
                //vm.OeePartData.Add(new OeePartData
                //{
                //    Name = "Mikro Przestoje",
                //    Qty = 0,
                //    TimeInMin = oeeCalc.StopUnplannedPerformanceTime / 60,
                //    CssClass = "EtStopPerformance"
                //});
                */
            }
            return View(vm);
        }

        public ActionResult Reports()
        {
            ViewBag.Skin = "nasaSkin";
            return View();
        }

        public ActionResult Report(int id)
        {
            ViewBag.Skin = "nasaSkin";
            return View();
        }

        public ActionResult OEEData()
        {
            FilterOeeViewModel fovm = OEEController.GetFiltersForOee(uowOEE);
            return View(fovm);
        }

        //public FilterOeeViewModel GetFiltersForOee()
        //{
        //    FilterOeeViewModel fovm = new FilterOeeViewModel();

        //    List<LabourBrigade> labBrigades = new List<LabourBrigade>();
        //    labBrigades.Add(new LabourBrigade { Id = -1, Name = "" });
        //    labBrigades.AddRange(uowOEE.LabourBrigadeRepo.GetList().ToList());

        //    fovm.MachineList = uowOEE.ResourceRepo.GetListForDropDown();
        //    fovm.LabourBrigades = new SelectList(labBrigades, "Id", "Name");
        //    return fovm;
        //}

        public JsonResult OEEDataGetList(OEEDataViewModel filter, int pageIndex, int pageSize)
        {
            DateTime dateDefault = new DateTime();
            DateTime dateFromConverted = filter.dateFrom == null ? new DateTime() : DateTime.Parse(filter.dateFrom);
            DateTime dateToConverted = filter.dateTo == null ? new DateTime() : DateTime.Parse(filter.dateTo);
            var query = uowOEE.OEEReportProductionDataRepo.GetList()
                .Where(x =>
                    (dateFromConverted == dateDefault || x.ProductionDate >= dateFromConverted) &&
                    (dateToConverted == dateDefault || x.ProductionDate < dateToConverted) &&
                    (filter.machineIds.Count() == 0 || filter.machineIds.Contains(x.Report.Machine.Id)) &&
                    (filter.labourBrigadeIds.Count() == 0 || filter.labourBrigadeIds.Contains(x.Report.LabourBrigade.Id)) &&
                    (filter.ItemCode == null || x.Item.Code == filter.ItemCode) &&
                    (filter.ItemName == null || x.Item.Name.Contains(filter.ItemName)) &&
                    (filter.ProductionDate == dateDefault || x.ProductionDate == filter.ProductionDate) &&
                    (filter.UserName == null || x.User.UserName == filter.UserName) &&
                    (filter.ReasonTypeEntryType == EnumEntryType.TimeAvailable || x.ReasonType.EntryType == filter.ReasonTypeEntryType) &&
                    (
                        filter.Type == -1 ||
                        (filter.Type == 0 && (x.ReasonType.EntryType > EnumEntryType.TimeAvailable && x.ReasonType.EntryType < EnumEntryType.StopPlanned)) ||
                        (filter.Type == 1 && (x.ReasonType.EntryType >= EnumEntryType.StopPlanned))
                    ) &&
                    (filter.ReasonId <= 0 || x.ReasonId == filter.ReasonId) &&
                    (filter.ReasonTypeId <= 0 || x.ReasonTypeId == filter.ReasonTypeId))
                 .OrderByDescending(x=>x.ProductionDate)
                 .ThenBy(x=>x.Report.Machine.Name);

            int startIndex = (pageIndex - 1) * pageSize;
            int itemsCount = query.Count();
            var list = query.Skip(startIndex).Take(pageSize)
                .Select(x => new OEEDataViewModel()
                {
                    CycleTime = x.CycleTime,
                    EntryTimeStamp = x.TimeStamp,
                    ReasonTypeEntryType = x.ReasonType.EntryType,
                    ReasonTypeName = x.ReasonType.Name,
                    ReasonTypeId = x.ReasonTypeId,
                    ReasonId = x.ReasonId,
                    ItemCode = x.Item != null ? x.Item.Code : "", //x.Reason != null? x.ReasonType.Name : "",
                    ItemName = x.Item != null? x.Item.Name : "", //x.Reason != null? x.Reason.Name : "",
                    ProdQty = x.ProdQty,
                    ProductionDate = x.ProductionDate,
                    ReasonName = x.Reason.Name,
                    ReportId = x.ReportId,
                    ResourceName = x.Report.Machine.Name,
                    UsedTime = x.UsedTime/60,
                    Shift = x.Report.Shift.ToString(),
                    LabourBrigadeName = x.Report.LabourBrigade.Name,
                    UserName = x.User.UserName
                }).ToList();

            list.ForEach(x=> { x.ProductionDateFormatted = x.ProductionDate.ToString("yyyy-MM-dd HH:mm:ss"); } );
            list.ForEach(x=> { x.YearWeek = Thread.CurrentThread.CurrentCulture.Calendar.GetWeekOfYear(x.ProductionDate, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday).ToString(); } );

            return Json(new { data = list, itemsCount });
        }
    }
}