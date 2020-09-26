using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Repo;
using System.Collections.Generic;
using System.Web.Mvc;
using MDL_BASE.ComponentBase.Repos;
using _MPPL_WEB_START.Areas._APPWEB.ViewModels;
using _MPPL_WEB_START.Areas.ONEPROD.OEE.ViewModels;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.ComponentOEE.Models;
using System;
using MDL_ONEPROD.Model.Scheduling;
using System.Linq;
using MDL_ONEPROD.Model.Scheduling.Interface;
using System.Data.Entity.SqlServer;
using _MPPL_WEB_START.Areas.ONEPROD.Models;
using MDL_BASE.ComponentBase.Entities;
using MDL_ONEPROD.ComponentENERGY.Entities;
using MDL_ONEPROD.ComponentENERGY;
using _MPPL_WEB_START.Areas._APPWEB.Models;
using MDL_ONEPROD.ComponentENERGY.Models;

namespace _MPPL_WEB_START.Areas.ONEPROD.Controllers
{
    public class OEEDashboardController : Controller
    {
        UnitOfWorkOneProdOEE uowOEE;
        UnitOfWorkOneProdENERGY uowEnergy;
        //UnitOfWorkOneprod uowONEPROD;
        RepoLabourBrigade repoLabourBrigade;

        public OEEDashboardController(IDbContextOneProdOEE db, IDbContextOneProdENERGY db2 = null)
        {
            uowOEE = new UnitOfWorkOneProdOEE(db);
            uowEnergy = new UnitOfWorkOneProdENERGY(db2);
            repoLabourBrigade = new RepoLabourBrigade(db);
            //uowONEPROD = new UnitOfWorkOneprod(db);
        }

        public ActionResult Index()
        {
            List<DashBoardViewModel> AreaMachineList = new List<DashBoardViewModel>();
            List<ResourceOP> AreaList = uowOEE.ResourceGroupRepo.GetList().ToList();

            foreach(ResourceOP area in AreaList)
            {
                if (area.IsOEE)
                {
                    DashBoardViewModel dashBoardViewModel = new DashBoardViewModel();
                    dashBoardViewModel.Area = area;
                    dashBoardViewModel.MachineList = uowOEE.ResourceRepo.GetListByGroup(area.Id).ToList();
                    AreaMachineList.Add(dashBoardViewModel);

                }
            }
            //uowOEE.OEEReportProductionDataRepo.GetDataForPareto();

            return View(AreaMachineList);
        }

        [HttpGet]
        public ActionResult IndexMachine(DateTime dateFrom, DateTime dateTo, int machineId, int labourBrigadeId = -1)
        {
            DashBoardMachineViewModel vm = new DashBoardMachineViewModel();
            ResourceOP machine = uowOEE.ResourceRepo.GetById(machineId);

            if (machine != null)
            {
                List<OEEReportProductionDataAbstract> productionData = uowOEE.OEEReportProductionDataRepo.GetDataByTimeRangeAndMachineId(dateFrom, dateTo, machineId, labourBrigadeId).ToList<OEEReportProductionDataAbstract>();
                OeeCalculationModel oeeCalc = new OeeCalculationModel(productionData);
                oeeCalc.CalculateOEE();

                vm.MachineId = machine.Id;
                vm.MachineName = machine.Name;
                vm.MachineExpectedOEE = machine.TargetOee;
                vm.Targets = uowOEE.ResourceRepo.GetTargetsForMachine(machineId);
                vm.AvailabilityResult = oeeCalc.Availability;
                vm.PerformanceResult = oeeCalc.Performance;
                vm.QualityResult = oeeCalc.Quality;
                vm.OeeResult = oeeCalc.Result;
                vm.NRFT = oeeCalc.NRFT;
                vm.UndefunedStopsCount = productionData.Count(x => x.ReasonTypeId == 5);
            }
            return View(vm);
        }


        [HttpGet]
        public ActionResult MachineDetails()
        {
            DashBoardViewModel vm = new DashBoardViewModel();
            vm.ReasonTypes = new List<ReasonType>();
            vm.ReasonTypes.Add(uowOEE.ReasonTypeRepo.GetById(2));
            vm.ReasonTypes.AddRange(uowOEE.ReasonTypeRepo.GetListStoppages().ToList());
            //vm.MachineList = uowONEPROD.MachineRepo.GetListForDropDown();
            ViewBag.MediaEnabled = AppClient.appClient.SettingsONEPROD.MediaEnabled;

            return View(vm);
        }
        [HttpGet]
        public ActionResult ReasonDetails()
        {
            DashBoardViewModel vm = new DashBoardViewModel();

            return View(vm);
        }

        [HttpGet]
        public ActionResult OeeResultsHorizontalBar(DateTime dateFrom, DateTime dateTo, int machineId, int labourBrigadeId, int intervalInHours)
        {
            OeeDataOfDateRange oeeDataOfDateRange = new OeeDataOfDateRange(uowOEE.OEEReportProductionDataRepo, dateFrom, dateTo, machineId, labourBrigadeId, intervalInHours);
            
            oeeDataOfDateRange.AvailabilityAverage *= 100;
            oeeDataOfDateRange.PerformanceAverage *= 100;
            oeeDataOfDateRange.QualityAverage *= 100;
            oeeDataOfDateRange.ResultAverage *= 100;

            return View(oeeDataOfDateRange);
        }

        [HttpGet]
        public JsonResult GetOeeResultOfDateRange(DateTime dateFrom, DateTime dateTo, int machineId, int labourBrigadeId, int intervalInHours)
        {
            OeeDataOfDateRange oeeDataOfDateRange = new OeeDataOfDateRange(uowOEE.OEEReportProductionDataRepo, dateFrom, dateTo, machineId, labourBrigadeId, intervalInHours);
            
            return Json(oeeDataOfDateRange, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetOeeOnlineData(DateTime dateFrom, DateTime dateTo, int machineId, int labourBrigadeId)
        {
            var data = uowOEE.OEEReportProductionDataRepo
                .GetDataByTimeRangeAndMachineId(dateFrom, dateTo, machineId, labourBrigadeId)
                .Select(x=>x.Report)
                .Distinct()
                .ToList();

            var plcProdData = data.Sum(x => x.TotalQtyCountedOnline);
            var plcStopData = data.Sum(x => x.TotalStoppageTimeCountedOnline) / 60;
            var operatorProdData = data.Sum(x => x.TotalQtyDeclaredByOperator);
            var operatorStopData = data.Sum(x => x.TotalStoppageTimeDeclaredByOperator) / 60;
            var RTVScreenShotNameList = GenerateRTVScreenShotNameList(dateFrom, dateTo, machineId);
            var RTVScreenShotFilePath = RTVController.GenerateRTVScreenShotFilePath();

            return Json(new { plcProdData, plcStopData, operatorProdData, operatorStopData, RTVScreenShotFilePath, RTVScreenShotNameList }, JsonRequestBehavior.AllowGet);
        }

        private List<string> GenerateRTVScreenShotNameList(DateTime dateFrom, DateTime dateTo, int machineId)
        {
            DateTime dateOfFixed3rdShift = new DateTime(2019, 12, 20).Date;
            List<string> rtvScreenShotNameList = new List<string>();
            dateFrom = dateTo.AddDays(-7) > dateFrom? dateTo.AddDays(-7) : dateFrom;

            for(DateTime dt = dateFrom; dt < dateTo; dt = dt.AddHours(8))
            {
                int hour = dt.AddHours(2).Hour;
                int shift = hour >= 16? 2 : hour >= 8? 1 : 3; 

                if(dt < dateOfFixed3rdShift && shift == 3)
                {
                    rtvScreenShotNameList.Add(RTVController.GenerateRTVScreenShotFileName(machineId, dt.AddDays(1), shift));
                }
                else
                {
                    rtvScreenShotNameList.Add(RTVController.GenerateRTVScreenShotFileName(machineId, dt, shift));
                }
            }

            return rtvScreenShotNameList;
        }

        [HttpPost]
        public JsonResult GetChartOEEDataResults(DateTime dateFrom, DateTime dateTo, int machineId, int labourBrigadeId, int intervalInHours = 24)
        {
            MachineTargets machineTargets = uowOEE.ResourceRepo.GetTargetsForMachine(machineId);
            ChartOEEDataPreparer chdp = new ChartOEEDataPreparer(dateFrom, dateTo, labourBrigadeId, intervalInHours, machineId, machineTargets);
            ChartViewModel vm = chdp.PrepareChartData(uowOEE.OEEReportProductionDataRepo, "OEE");

            return Json(vm);
        }
        [HttpPost]
        public JsonResult GetChartDataOfReasonTypes(DateTime dateFrom, DateTime dateTo, int machineId, int labourBrigadeId = -1, int intervalInHours = 24, int languageId = 1)
        {
            List<OEEReportProductionDataAbstract> chartDataByEntryTypeList = uowOEE.OEEReportProductionDataRepo
                .GetListBy_DateRange_Machine_Type(dateFrom, dateTo, machineId, null, labourBrigadeId)
                .Where(x => x.ReasonType != null && x.ReasonType.EntryType >= EnumEntryType.StopPlanned)
                .ToList<OEEReportProductionDataAbstract>();

            List<DateTime> openShifts = uowOEE.OEEReportProductionDataRepo.GetListOfOpenShifts(dateFrom, dateTo, machineId, labourBrigadeId);
            ResourceOP machine = uowOEE.ResourceRepo.GetById(machineId);
            int shiftTargetinSEC = 0;

            ChartOfReasonTypesDataPreparer chdp = new ChartOfReasonTypesDataPreparer(dateFrom, dateTo, intervalInHours, openShifts, shiftTargetinSEC);
            ChartViewModel vm = chdp.PrepareChartData(chartDataByEntryTypeList, "Kategorie Postojów", languageId);
            return Json(vm);
        }
        [HttpPost]
        public JsonResult GetChartDataCumulatedOfReasonTypes(DateTime dateFrom, DateTime dateTo, int machineId, int labourBrigadeId = -1, int intervalInHours = 24, int languageId = 1)
        {
            List<OEEReportProductionDataAbstract> chartDataByEntryTypeList = uowOEE.OEEReportProductionDataRepo
                .GetListBy_DateRange_Machine_Type(dateFrom, dateTo, machineId, null, labourBrigadeId)
                .Where(x => x.ReasonType != null && x.ReasonType.EntryType >= EnumEntryType.StopPlanned)
                .ToList<OEEReportProductionDataAbstract>();
            List<DateTime> openShifts = uowOEE.OEEReportProductionDataRepo.GetListOfOpenShifts(dateFrom, dateTo, machineId, labourBrigadeId);
            ResourceOP machine = uowOEE.ResourceRepo.GetById(machineId);
            //int shiftTarget = GetShiftTargetBy_EntryType_Machine(entryType, machine);
            int shiftTargetinSEC = 0;

            ChartOfReasonTypesCumulatedDataPreparer chdp = new ChartOfReasonTypesCumulatedDataPreparer(dateFrom, dateTo, intervalInHours, openShifts, shiftTargetinSEC);
            ChartViewModel vm = chdp.PrepareChartData(chartDataByEntryTypeList, "Kategorie Postojów", languageId);
            return Json(vm);
        }
        [HttpPost]
        public JsonResult GetChartParetoDataOfReasonTypes(DateTime dateFrom, DateTime dateTo, int machineId, int labourBrigadeId = 1, int intervalInHours = 24, int limit = 1000, int languageId = 1)
        {
            //ReasonType reasonType = uowOEE.ReasonTypeRepo.GetById(reasonTypeId);
            List<OEEReportProductionDataAbstract> productionData = uowOEE.OEEReportProductionDataRepo
                .GetDataByTimeRangeAndMachineId(dateFrom, dateTo, machineId, labourBrigadeId)
                .ToList<OEEReportProductionDataAbstract>();
            OeeCalculationModel oeeCalc = new OeeCalculationModel(productionData);
            oeeCalc.CalculateOEE();

            List<ParetoModel> paretoModelList = uowOEE.OEEReportProductionDataRepo
                .GetParetoBy_ReasonType_UsedTime(dateFrom, dateTo, machineId, labourBrigadeId)
                .Where(x => x.EntryType >= (int)EnumEntryType.StopPlanned)
                .ToList();

            ChartOfReasonTypesParetoDataPreparer chdp = new ChartOfReasonTypesParetoDataPreparer(paretoModelList, oeeCalc);
            ChartViewModel vm = chdp.PrepareChartData("Pareto", limit, languageId);

            return Json(vm);
        }

        [HttpPost]
        public JsonResult GetChartDataByEntryType(DateTime dateFrom, DateTime dateTo, int machineId, int labourBrigadeId, int reasonTypeId, int intervalInHours = 24, int languageId = 1)
        {
            ReasonType reasonType = uowOEE.ReasonTypeRepo.GetById(reasonTypeId);
            List<OEEReportProductionDataAbstract> chartDataByEntryTypeList = uowOEE.OEEReportProductionDataRepo.GetListBy_DateRange_Machine_Type(dateFrom, dateTo, machineId, reasonTypeId, labourBrigadeId).ToList<OEEReportProductionDataAbstract>();
            List<DateTime> openShifts = uowOEE.OEEReportProductionDataRepo.GetListOfOpenShifts(dateFrom, dateTo, machineId, labourBrigadeId);
            ResourceOP machine = uowOEE.ResourceRepo.GetById(machineId);
            //int shiftTarget = GetShiftTargetBy_EntryType_Machine(reasonTypeId, machine);
            int shiftTargetinSEC = (int)uowOEE.MachineTargetRepo.GetTargetByResourceAndReasonType(machineId, reasonTypeId);

            ChartByEntryTypetDataPreparer chdp = new ChartByEntryTypetDataPreparer(dateFrom, dateTo, intervalInHours, openShifts, shiftTargetinSEC);
            ChartViewModel vm = chdp.PrepareChartData(chartDataByEntryTypeList, reasonType.Name, languageId);
            return Json(vm);
        }

        private int GetShiftTargetBy_EntryType_Machine(int? reasonTypeId)
        {
            //switch (enumEntryType)
            //{
            //    case EnumEntryType.StopPlanned: return machine.TargetInSec_StopPlanned;
            //    case EnumEntryType.StopPlannedChangeOver: return machine.TargetInSec_StopPlannedChangeOver;
            //    case EnumEntryType.StopUnplanned: return machine.TargetInSec_StopUnplanned;
            //    case EnumEntryType.StopUnplannedBreakdown: return machine.TargetInSec_StopUnplannedBreakdown;
            //    case EnumEntryType.StopUnplannedChangeOver: return machine.TargetInSec_StopUnplannedChangeOver;
            //    case EnumEntryType.StopUnplannedPreformance: return machine.TargetInSec_StopUnplannedPreformance;
            //    default: return 0;
            //}
            //var r = uowOEE.MachineTargetRepo.GetTargetByResourceAndReasonType()
            return 0;
        }
        [HttpPost]
        public JsonResult GetChartDataCumulatedByEntryType(DateTime dateFrom, DateTime dateTo, int machineId, int labourBrigadeId, int reasonTypeId, int intervalInHours = 24, int languageId = 1)
        {
            ReasonType reasonType = uowOEE.ReasonTypeRepo.GetById(reasonTypeId);
            List<OEEReportProductionDataAbstract> chartDataByEntryTypeList = uowOEE.OEEReportProductionDataRepo.GetListBy_DateRange_Machine_Type(dateFrom, dateTo, machineId, reasonTypeId, labourBrigadeId).ToList<OEEReportProductionDataAbstract>();
            List<DateTime> openShifts = uowOEE.OEEReportProductionDataRepo.GetListOfOpenShifts(dateFrom, dateTo, machineId, labourBrigadeId);
            ResourceOP machine = uowOEE.ResourceRepo.GetById(machineId);
            //int shiftTarget = GetShiftTargetBy_EntryType_Machine(entryType, machine);
            int shiftTargetinSEC = (int)uowOEE.MachineTargetRepo.GetTargetByResourceAndReasonType(machineId, reasonTypeId);

            ChartByEntryTypeCumulatedDataPreparer chdp = new ChartByEntryTypeCumulatedDataPreparer(dateFrom, dateTo, intervalInHours, openShifts, shiftTargetinSEC);
            ChartViewModel vm = chdp.PrepareChartData(chartDataByEntryTypeList, reasonType.Name, languageId);
            return Json(vm);
        }
        [HttpPost]
        public JsonResult GetChartParetoDataResults(DateTime dateFrom, DateTime dateTo, int machineId, int labourBrigadeId, int reasonTypeId, int intervalInHours = 24, int limit = 1000, int languageId = 1)
        {
            ReasonType reasonType = uowOEE.ReasonTypeRepo.GetById(reasonTypeId);
            List<OEEReportProductionDataAbstract> productionData = uowOEE.OEEReportProductionDataRepo
                .GetDataByTimeRangeAndMachineId(dateFrom, dateTo, machineId, labourBrigadeId)
                .ToList<OEEReportProductionDataAbstract>();
            OeeCalculationModel oeeCalc = new OeeCalculationModel(productionData);
            oeeCalc.CalculateOEE();

            List<ParetoModel> paretoModelList = uowOEE.OEEReportProductionDataRepo.GetParetoBy_Reason_UsedTime(dateFrom, dateTo, reasonTypeId, machineId, labourBrigadeId);

            ChartByEntryTypeParetoDataPreparer chdp = new ChartByEntryTypeParetoDataPreparer(paretoModelList, oeeCalc, reasonType.EntryType);
            ChartViewModel vm = chdp.PrepareChartData("Pareto", limit, languageId);

            return Json(vm);
        }
        [HttpPost]
        public JsonResult GetChartParetoScrapReasonType(DateTime dateFrom, DateTime dateTo, int machineId, int labourBrigadeId, int languageId = 1)
        {
            List<OEEReportProductionDataAbstract> productionData = uowOEE.OEEReportProductionDataRepo
                .GetDataByTimeRangeAndMachineId(dateFrom, dateTo, machineId, labourBrigadeId)
                .Where(x => x.ReasonType != null && (EnumEntryType.ScrapMaterial <= x.ReasonType.EntryType && x.ReasonType.EntryType < EnumEntryType.ScrapLabel))
                .ToList<OEEReportProductionDataAbstract>();
            OeeCalculationModel oeeCalc = new OeeCalculationModel(productionData);
            oeeCalc.CalculateOEE();

            List<ParetoModel> paretoModelList = uowOEE.OEEReportProductionDataRepo.GetParetoBy_Scrap_ReasonType(dateFrom, dateTo, machineId, labourBrigadeId);

            ChartParetoScrapProcessDataPreparer chdp = new ChartParetoScrapProcessDataPreparer(paretoModelList, oeeCalc);
            ChartViewModel vm = chdp.PrepareChartData("Pareto Typów", 1000, languageId);

            return Json(vm);
        }
        [HttpPost]
        public JsonResult GetChartParetoScrapReason(DateTime dateFrom, DateTime dateTo, int machineId, int labourBrigadeId, int languageId = 1, int limit = 1000)
        {
            List<OEEReportProductionDataAbstract> productionData = uowOEE.OEEReportProductionDataRepo
                .GetDataByTimeRangeAndMachineId(dateFrom, dateTo, machineId, labourBrigadeId)
                .Where(x => x.ReasonType != null && (EnumEntryType.ScrapMaterial <= x.ReasonType.EntryType && x.ReasonType.EntryType < EnumEntryType.ScrapLabel))
                .ToList<OEEReportProductionDataAbstract>();
            OeeCalculationModel oeeCalc = new OeeCalculationModel(productionData);
            oeeCalc.CalculateOEE();

            List<ParetoModel> paretoModelList = uowOEE.OEEReportProductionDataRepo.GetParetoBy_Scrap_Reason(dateFrom, dateTo, machineId, labourBrigadeId);

            ChartParetoScrapProcessDataPreparer chdp = new ChartParetoScrapProcessDataPreparer(paretoModelList, oeeCalc);
            ChartViewModel vm = chdp.PrepareChartData("Pareto Powodów", limit, languageId);

            return Json(vm);
        }
        [HttpPost]
        public JsonResult GetChartParetoScrapAnc(DateTime dateFrom, DateTime dateTo, int machineId, int labourBrigadeId, int languageId = 1, int limit = 1000)
        {
            List<OEEReportProductionDataAbstract> productionData = uowOEE.OEEReportProductionDataRepo
                .GetDataByTimeRangeAndMachineId(dateFrom, dateTo, machineId, labourBrigadeId)
                .Where(x => x.ReasonType != null && (EnumEntryType.ScrapProcess <= x.ReasonType.EntryType && x.ReasonType.EntryType < EnumEntryType.ScrapLabel))
                .ToList<OEEReportProductionDataAbstract>();
            OeeCalculationModel oeeCalc = new OeeCalculationModel(productionData);
            oeeCalc.CalculateOEE();

            List<ParetoModel> paretoModelList = uowOEE.OEEReportProductionDataRepo.GetParetoBy_Scrap_ANC(dateFrom, dateTo, machineId, labourBrigadeId);

            ChartParetoScrapAncDataPreparer chdp = new ChartParetoScrapAncDataPreparer(paretoModelList, oeeCalc);
            ChartViewModel vm = chdp.PrepareChartData("Pareto", limit, languageId);

            return Json(vm);
        }
        [HttpPost]
        public JsonResult GetChartReasonAnalysis(DateTime dateFrom, DateTime dateTo, int machineId, int reasonId, int labourBrigadeId, int intervalInHours = 24, int languageId = 1)
        {
            if (reasonId > 0)
            {
                List<OEEReportProductionDataAbstract> productionData = uowOEE.OEEReportProductionDataRepo
                    .GetDataByTimeRangeAndMachineIdAndReasonId(dateFrom, dateTo, machineId, reasonId, labourBrigadeId)
                    .ToList<OEEReportProductionDataAbstract>();

                List<DateTime> openShifts = uowOEE.OEEReportProductionDataRepo.GetListOfOpenShifts(dateFrom, dateTo, machineId, labourBrigadeId);

                ResourceOP machine = uowOEE.ResourceRepo.GetById(machineId);
                Reason reason = uowOEE.ReasonRepo.GetById(reasonId);

                //int shiftTarget = GetShiftTargetBy_EntryType_Machine(reason.EntryType, machine);
                int shiftTargetinSEC = (int)uowOEE.MachineTargetRepo.GetTargetByResourceAndReasonType(machineId, reason.ReasonTypeId);

                ChartByEntryTypetDataPreparer chdp = new ChartByEntryTypetDataPreparer(dateFrom, dateTo, intervalInHours, openShifts, shiftTargetinSEC);
                ChartViewModel vm = chdp.PrepareChartData(productionData, reasonId.ToString(), languageId);
                return Json(vm);
            }
            else
            {
                return Json(0);
            }
            
        }


        // Akcje dotyczące nowego widoku COMPARED RESULTS
        public ActionResult ComparedResults(DateTime dateFrom, DateTime dateTo, List<int> machineIds, int reasonId, List<int> labourBrigadeIds, int intervalInHours = 24, int languageId = 1)
        {
            ChartMultiViewModel chartMultiVM = new ChartMultiViewModel();
            chartMultiVM.ChartViewModels = new List<ChartViewModel>();
            intervalInHours = (int)(dateTo - dateFrom).TotalHours;
            List<ReasonType> reasonTypes = uowOEE.ReasonTypeRepo.GetListStoppages().ToList();
            //List<Machine> 
            List<ResourceOP> resources = uowOEE.ResourceRepo.GetList().ToList();

            foreach (int resourceId in machineIds)
            {
                ResourceOP resource = resources.FirstOrDefault(x => x.Id == resourceId);
                MachineTargets machineTargets = uowOEE.ResourceRepo.GetTargetsForMachine(resource);
                ChartRadarOEEDataPreparer chdp = new ChartRadarOEEDataPreparer(dateFrom, dateTo, 0, intervalInHours, resourceId, machineTargets, reasonTypes);
                ChartViewModel chartVM = chdp.PrepareChartData(uowOEE.OEEReportProductionDataRepo, resource.Name);
                chartVM.id = resourceId;
                List<LabourBrigade> brigades = uowOEE.LabourBrigadeRepo.GetList().ToList();

                foreach (int labourBrigadeId in labourBrigadeIds)
                {
                    LabourBrigade brigade = brigades.FirstOrDefault(x => x.Id == labourBrigadeId);
                    if (brigade != null)
                    {
                        ChartDataSetViewModel ds = chdp.PrepareBrigadeDataSet(brigade);
                        chartVM.datasets.Add(ds);
                    }
                }
                chartVM.targets.Add(chdp.PrepareTargetDataSet());
                chartMultiVM.ChartViewModels.Add(chartVM);
            }
            NormalizeResults(chartMultiVM);

            return View(chartMultiVM);
        }

        private static void NormalizeResults(ChartMultiViewModel result)
        {
            int numberOfElements = result.ChartViewModels.FirstOrDefault().datasets.FirstOrDefault().data.Count;
            decimal[] maxValArr = new decimal[numberOfElements];

            for (int i = 0; i < maxValArr.Length; i++)
            {
                maxValArr[i] = result.ChartViewModels.Max(x => x.datasets.Max(z => z.data[i]));
            }

            foreach (ChartViewModel vm in result.ChartViewModels)
            {
                for (int k = 0; k < vm.datasets.Count; k++)
                {
                    for (int i = 4; i < maxValArr.Length; i++)
                    {
                        vm.datasets[k].data[i] = maxValArr[i] != 0 ? 100 * vm.datasets[k].data[i] / maxValArr[i] : 0; // skalowanie danych.
                    }
                }
            }
        }
    }
}