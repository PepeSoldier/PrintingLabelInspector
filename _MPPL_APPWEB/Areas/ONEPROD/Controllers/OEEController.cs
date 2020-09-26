using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Repo;
using System.Collections.Generic;
using System.Web.Mvc;
using MDL_BASE.ComponentBase.Repos;
using System.Linq;
using MDL_ONEPROD.Model.Scheduling;
using _MPPL_WEB_START.Areas._APPWEB.Models;
using _MPPL_WEB_START.Areas._APPWEB.ViewModels;
using System;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.ComponentOEE.Models;
using _MPPL_WEB_START.Areas.ONEPROD.OEE.ViewModels;
using _MPPL_WEB_START.Models;
using MDL_BASE.ComponentBase.Entities;
using _MPPL_WEB_START.Areas.ONEPROD.ViewModels.OEE;
using MDL_ONEPROD.Model.Scheduling.Interface;
using MDL_ONEPROD.ComponentOEE.ViewModels;

namespace _MPPL_WEB_START.Areas.ONEPROD.Controllers
{
    public class OEEController : BaseController
    {
        UnitOfWorkOneProdOEE uowOEE;
        UnitOfWorkOneprod uowONEPROD;
        RepoLabourBrigade repoLabourBrigade;

        public OEEController(IDbContextOneProdOEE db)
        {
            uowOEE = new UnitOfWorkOneProdOEE(db);
            repoLabourBrigade = new RepoLabourBrigade(db);
            uowONEPROD = new UnitOfWorkOneprod(db);
        }

        public ActionResult Index()
        {
            FilterOeeViewModel fovm = GetFiltersForOee(uowOEE);
            return View(fovm);
        }

        public ActionResult CompareResults()
        {
            FilterOeeViewModel fovm = GetFiltersForOee(uowOEE);
            return View(fovm);
        }

        public static FilterOeeViewModel GetFiltersForOee(UnitOfWorkOneProdOEE uowOEE)
        {
            FilterOeeViewModel fovm = new FilterOeeViewModel();

            List<LabourBrigade> labBrigades = new List<LabourBrigade>();
            labBrigades.Add(new LabourBrigade { Id = -1, Name = "" });
            labBrigades.AddRange(uowOEE.LabourBrigadeRepo.GetList().ToList());

            fovm.MachineList = uowOEE.ResourceRepo.GetListForDropDown();
            fovm.LabourBrigades = new SelectList(labBrigades, "Id", "Name");
            return fovm;
        }

        [HttpGet]
        public JsonResult AutocompleteByName(string Prefix)
        {
            List<ResourceOP> MachineAutocomplete = uowONEPROD.ResourceRepo.GetAutocomplete(Prefix).ToList();
            List<AutocompleteViewModel> adm = MachineAutocomplete.Select(x =>
                new AutocompleteViewModel
                {
                    TextField = x.Name,
                    ValueField = x.Id.ToString()
                }).ToList();

            return Json(adm, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult AncAutocomplete(string Prefix, int MachineId = 7)
        {
            List<MCycleTime> cycleTimes = uowONEPROD.CycleTimeRepo.GetCycleTimesByMachine(MachineId).ToList();
            List<int> partCategoryIds = cycleTimes.Select(x => x.ItemGroupId).ToList();
            List<ItemOP> ancAC = uowONEPROD.ItemOPRepo.GetList().Where(x => x.Deleted == false && partCategoryIds.Contains((int)x.ItemGroupId)).ToList();

            if (Prefix.Length > -1)
            {
                ancAC = ancAC.Where(x => x.Code.StartsWith(Prefix.ToUpper())).ToList();
            }

            List<AutocompleteViewModel> adm = ancAC.Select(x =>
                new AutocompleteViewModel
                {
                    TextField = x.GetName,
                    ValueField = x.Code,
                    Data1 = GetCycleTime(cycleTimes, x.ItemGroupId),
                }
            ).ToList();

            return Json(adm, JsonRequestBehavior.AllowGet);
        }
        private string GetCycleTime(List<MCycleTime> cycleTimes, int? partCategoryId)
        {
            if(partCategoryId != null)
            {
                MCycleTime ct = cycleTimes.FirstOrDefault(c => c.ItemGroupId == partCategoryId);
                if (ct != null)
                {
                    return ct.CycleTime.ToString("0.###").Replace(",",".");
                }
            }
            
            return "0";
        }

        [HttpGet]
        public JsonResult EmployeeAutocomplete(string Prefix, string shiftCode)
        {
            List<string> results;

            if (Prefix.Length > -1)
            {
                results = repoLabourBrigade.LabourEmployeeAutocomplete(Prefix, shiftCode);
            }
            else
            {
                results = new List<string>();
            }

            List<AutocompleteViewModel> acResults = results.Select(x =>
                new AutocompleteViewModel
                {
                    TextField = x,
                    ValueField = x
                }
            ).ToList();

            return Json(acResults, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ExportToExcel(DateTime dateFrom, DateTime dateTo, int machineId = -1, int labourBrigadeId = -1)
        {

            List<OEEReportProductionData> productionData = uowOEE.OEEReportProductionDataRepo.GetDataByTimeRangeAndMachineId(dateFrom, dateTo, machineId, labourBrigadeId).ToList();

            var exceldata = productionData.Select(x => new {
                x.Id,
                Machine = x.Report == null ? "?" : x.Report.Machine.Name,
                x.ProductionDate,
                EntryType = x.ReasonType.EntryType,
                ReasonTypeName = x.ReasonType.Name,
                Name = (int)x.ReasonType.EntryType < 20? x.Item == null? "?" : x.Item.Name : x.Reason == null? "?" : x.Reason.Name,
                ItemCode = x.Item == null ? "?" : x.Item.Code,
                x.CycleTime,
                ProductionCycleTime = x.Detail == null ? 0 : x.Detail.ProductionCycleTime,
                UsedTime = x.UsedTime,
                User = x.User == null? "?" : x.User.UserName,
                x.TimeStamp
            }).ToList();

            ExcelExportModel.ExportData(exceldata, Response, "OEEData_" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            return View();
        }

        [HttpPost]
        public JsonResult OEEGetData(DateTime dateFrom, DateTime dateTo, int machineId, int labourBrigadeId = -1)
        {
            DashBoardMachineViewModel vm = new DashBoardMachineViewModel();
            List<OEEReportProductionDataAbstract> productionData = uowOEE.OEEReportProductionDataRepo.GetDataByTimeRangeAndMachineId(dateFrom, dateTo, machineId, labourBrigadeId).ToList<OEEReportProductionDataAbstract>();
            OeeCalculationModel oeeCalc = new OeeCalculationModel(productionData);
            oeeCalc.CalculateOEE();
            
            vm.NumberOfRecords = productionData.Count();
            vm.AvailabilityResult = oeeCalc.Availability;
            vm.PerformanceResult = oeeCalc.Performance;
            vm.QualityResult = oeeCalc.Quality;
            vm.OeeResult = oeeCalc.Result;
            vm.Targets = uowONEPROD.ResourceRepo.GetTargetsForMachine(machineId);

            vm.ProductionTimeInMin = oeeCalc.GoodProductionTime / 60;
            vm.StopsPlannedInMin = oeeCalc.StopPlannedTime /60;
            vm.StopsPerformanceInMin = oeeCalc.StopPerformanceTime / 60;
            vm.StopsUnplannedInMin = oeeCalc.StopUnplannedTime / 60;
            //vm.StopsUnplannedChangeoverInMin = oeeCalc.StopUnplannedChangeOverTime / 60;
            //vm.StopsBreakdownInMin = oeeCalc.StopUnplannedBreakdownsTime / 60;
            vm.ScrapProcessQty = oeeCalc.ScrapProcessCount;
            vm.ScrapMaterialQty = oeeCalc.ScrapMaterialCount;
            vm.ProducedGoodQty = oeeCalc.GoodCount;

            return Json(vm);

            //return Json(new
            //{
            //    Availability = oeeCalc.Availability,
            //    Performance = oeeCalc.Performance,
            //    Quality = oeeCalc.Quality,
            //    OeeResult = oeeCalc.Result
            //});
        }

        public ActionResult ChartTest()
        {
            ChartViewModel vm = new ChartViewModel();
            vm.title = "Oee Wynik KW35 - Prasa 1";
            vm.labels = new List<string> { "Pn", "Wt", "Śr","Czw", "Pt" };
            vm.datasets.Add(
                new ChartDataSetViewModel()
                {
                    label = "OeeResult",
                    data = new List<decimal> { 85, 67, 54, 32, 12 },
                });
            vm.datasets.Add(
                new ChartDataSetViewModel()
                {
                    label = "OeeTarget",
                    data = new List<decimal> { 30, 40, 75, 80, 99 },
                });


            return View(vm);
        }


        public ActionResult SendEmailWithReport(DateTime dateFrom, DateTime dateTo)
        {
            return View(PrepareEmailData(dateFrom, dateTo));
        }
        private List<DashBoardViewModel> PrepareEmailData(DateTime dateFrom, DateTime dateTo)
        {
            List<DashBoardViewModel> AreaMachineList = new List<DashBoardViewModel>();
            List<ResourceOP> AreaList = uowONEPROD.ResourceGroupRepo.GetList().ToList();

            foreach (ResourceOP area in AreaList)
            {
                if (area.Id >= 16 && area.Id <= 18)
                {
                    DashBoardViewModel dashBoardViewModel = new DashBoardViewModel();
                    dashBoardViewModel.Area = area;
                    dashBoardViewModel.MachineList = uowONEPROD.ResourceRepo.GetListByGroup(area.Id).ToList();
                    dashBoardViewModel.DateFrom = dateFrom;
                    dashBoardViewModel.DateTo = dateTo;
                    AreaMachineList.Add(dashBoardViewModel);
                }
            }

            foreach (DashBoardViewModel area in AreaMachineList)
            {
                foreach (ResourceOP machine in area.MachineList)
                {
                    OeeDataOfDateRange oeeDataOfDateRange = new OeeDataOfDateRange(uowOEE.OEEReportProductionDataRepo, dateFrom, dateTo, machine.Id, -1, 24);
                    machine.OeeResult = oeeDataOfDateRange.ResultAverage;
                }
            }
            
            return AreaMachineList;
        }

        [HttpGet]
        public JsonResult SendEmail(DateTime dateFrom, DateTime dateTo)
        {
            string emailHtml = RenderViewToString(this.ControllerContext, "SendEmailWithReport", PrepareEmailData(dateFrom, dateTo));
            MailerONEPROD mailer = new MailerONEPROD(Mailer.Create());
            mailer.OeeReport(emailHtml, "kamil.krzyzanowski@electrolux.com", dateFrom);
            return Json("");
        }
        
        [HttpPost]
        public JsonResult ReasonTypesProductionGetList()
        {
            List<ReasonType> reasonTypes =  uowOEE.ReasonTypeRepo.GetListProduction().ToList();
            return Json(reasonTypes);
        }
        [HttpPost]
        public JsonResult ReasonProductionGetList()
        {
            List<Reason> reasons = new List<Reason>();

            reasons.Add(new Reason() { Id = -1, Name = "", NameEnglish = "", ReasonTypeId = 0, Deleted = false });
            reasons.AddRange(
                uowOEE.ReasonRepo.GetList().Where(x => 
                x.ReasonType.EntryType >= EnumEntryType.Production && 
                x.ReasonType.EntryType <= EnumEntryType.ScrapLabel)
                .ToList());

            return Json(reasons);
        }
        [HttpPost]
        public JsonResult ReasonTypesStoppagesGetList(int reasonTypeId = 0)
        {
            List<ReasonType> reasonTypes = uowOEE.ReasonTypeRepo.GetListStoppages().ToList();
            return Json(reasonTypes);
        }
    }
}