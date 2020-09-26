using _MPPL_WEB_START.Areas._APPWEB.Models;
using _MPPL_WEB_START.Areas._APPWEB.ViewModels;
using MDL_BASE.ComponentBase.Entities;
using MDL_ONEPROD.ComponentOEE.Models;
using MDL_ONEPROD.ComponentOEE.Repos;
using MDL_ONEPROD.Model.OEEProd;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.ONEPROD.Models
{
    public class ChartRadarOEEDataPreparer: ChartOEEDataPreparer
    {
        private List<ReasonType> reasonTypes;

        public ChartRadarOEEDataPreparer(DateTime dateFrom, DateTime dateTo, int labourBrigade, int intervalInHours, int machineId, MachineTargets machineTargets, List<ReasonType> reasonTypes) : 
            base(dateFrom, dateTo, labourBrigade, intervalInHours, machineId, machineTargets)
        {
            this.reasonTypes = reasonTypes;
        }

        public override ChartViewModel PrepareChartData(OEEReportProductionDataRepo repo, string title)
        {
            this.repo = repo;

            ChartViewModel vm = new ChartViewModel();
            vm.labels = PrepareLabels();
            vm.title = title;
            vm.datasets = new List<ChartDataSetViewModel>();
            //vm.datasets.AddRange(PreparDataSets_OEE_A_P_Q());

            return vm;
        }
        public override List<string> PrepareLabels()
        {
            List<string> labels = new List<string>() { "OEE", "A", "P", "Q" }; //, "Pl.", "Przezbr.", "Awarie", "NiePl.", "Mik_post." };
            foreach(ReasonType rt in reasonTypes)
            {
                labels.Add(rt.Name);
            }
            return labels;
        }
        public ChartDataSetViewModel PrepareBrigadeDataSet(LabourBrigade labourBrigade)
        {
            string[] colors = {"gray", "#90ee90", "#f08484", "#F268FA", "#ffe641", "#41ffd5", "#90ee90", "#f08484", "#1e90ff", "#ffe641" };

            this.labourBrigadeId = labourBrigade.Id;

            List<OEEReportProductionDataAbstract> prodDataList = repo
                    .GetDataByTimeRangeAndMachineId(dateFrom, dateTo, machineId, labourBrigadeId)
                    .ToList<OEEReportProductionDataAbstract>();
            OeeCalculationModel oeeCalc = new OeeCalculationModel(prodDataList);
            oeeCalc.CalculateOEE();

            var dataOEE = (decimal)(oeeCalc.Result * 100);
            var dataA = (decimal)(oeeCalc.Availability * 100);
            var dataP = (decimal)(oeeCalc.Performance * 100);
            var dataQ = (decimal)(oeeCalc.Quality * 100);
            
            //var stopPlannedTime = (decimal)(oeeCalc.StopPlannedTime / 60);
            //ar stopUnplannedBreakdownsTime = 0; //(decimal)(oeeCalc.StopUnplannedBreakdownsTime / 60);
            //var stopUnplannedChangeOverTime = 0; //(decimal)(oeeCalc.StopUnplannedChangeOverTime / 60);
            //var stopUnplannedPerformanceTime = 0; //(decimal)(oeeCalc.StopUnplannedPerformanceTime / 60);
            //var stopUnplannedTime = (decimal)(oeeCalc.StopUnplannedTime / 60);

            List<decimal> data_ = new List<decimal>() { dataOEE, dataA, dataP, dataQ };
            foreach (ReasonType rt in reasonTypes)
            {
                data_.Add(oeeCalc.ProductionAnalyzer.ProductionQtyToTime(prodDataList, rt));
            }

            var vm = new ChartDataSetViewModel()
            {
                data = data_, 
                    //new List<decimal>() { dataOEE, dataA, dataP, dataQ }, 
                    //stopPlannedTime, 
                    //stopUnplannedChangeOverTime, 
                    //stopUnplannedBreakdownsTime, 
                    //stopUnplannedTime, 
                    //stopUnplannedPerformanceTime },
                label = "BRYG. " + labourBrigade.Name,
                backgroundColor = new List<string> { ColorHelper.ConvertToHtmlRGBA(ColorHelper.ColorChangeTransparency(ColorHelper.MapColorFromString(colors[labourBrigadeId]),0.15)) },
                borderColor = new List<string> { colors[labourBrigadeId] },
                fill = true,
                borderWidth = 3,
                lineTension = 0,
            };

            return vm;
        }
        public ChartDataSetViewModel PrepareTargetDataSet()
        {
            ChartDataSetViewModel target = new ChartDataSetViewModel()
            {
                label = "Cel",
                type = "line",
                backgroundColor = new List<string>(),
                borderColor = new List<string>() { "red" },
                data = new List<decimal>(),
                borderWidth = 1,
                lineTension = 0,
                pointBackgroundColor = "red",
                datalabels = "LabelsNone",
            };

            List<decimal> targetsData = new List<decimal> {
                    machineTargets.OeeTarget,
                    machineTargets.AvailabilityTarget,
                    machineTargets.PerformanceTarget,
                    machineTargets.QualityTarget 
            };

            foreach (ReasonType rt in reasonTypes)
            {
                targetsData.Add(1);
            }

            foreach (var targetItem in targetsData)
            {
                target.data.Add(targetItem);
                target.backgroundColor.Add("rgba(0,0,0,0)");
            }

            return target;
        }

        public override List<ChartDataSetViewModel> PreparDataSets_OEE_A_P_Q()
        {
            return new List<ChartDataSetViewModel>();
        }
    }
    public class ChartOEEDataPreparer : ChartDataPreparer
    {
        protected OEEReportProductionDataRepo repo;
        protected MachineTargets machineTargets;
        protected int machineId;
        protected int labourBrigadeId;

        public ChartOEEDataPreparer(DateTime dateFrom, DateTime dateTo, int labourBrigadeId, int intervalInHours, int machineId, MachineTargets machineTargets) : base(dateFrom, dateTo, intervalInHours)
        {
            this.machineId = machineId;
            this.machineTargets = machineTargets;
            this.labourBrigadeId = labourBrigadeId;
        }

        public virtual ChartViewModel PrepareChartData(OEEReportProductionDataRepo repo, string title)
        {
            this.repo = repo;

            ChartViewModel vm = new ChartViewModel();
            vm.labels = PrepareLabels();
            vm.title = title;
            vm.datasets = new List<ChartDataSetViewModel>();
            vm.datasets.AddRange(PreparDataSets_OEE_A_P_Q());

            return vm;
        }
        public virtual List<ChartDataSetViewModel> PreparDataSets_OEE_A_P_Q()
        {
            List<decimal> dataOEE = new List<decimal>();
            List<decimal> dataA = new List<decimal>();
            List<decimal> dataP = new List<decimal>();
            List<decimal> dataQ = new List<decimal>();

            for (DateTime date = dateFrom; date < dateTo; date = date.AddHours(intervalInHours))
            {
                List<OEEReportProductionDataAbstract> prodDataList = repo
                    .GetDataByTimeRangeAndMachineId(date, date.AddHours(intervalInHours), machineId, labourBrigadeId)
                    .ToList<OEEReportProductionDataAbstract>();
                OeeCalculationModel oeeCalc = new OeeCalculationModel(prodDataList);
                oeeCalc.CalculateOEE();

                dataOEE.Add((decimal)(oeeCalc.Result * 100));
                dataA.Add((decimal)(oeeCalc.Availability * 100));
                dataP.Add((decimal)(oeeCalc.Performance * 100));
                dataQ.Add((decimal)(oeeCalc.Quality * 100));
            }

            List<ChartDataSetViewModel> dataSets = new List<ChartDataSetViewModel>();
            dataSets.Add(PrepareTargetDataSet(dataOEE, machineTargets.OeeTarget * 100));
            dataSets.Add(PrepareDataSet(dataOEE, machineTargets.OeeTarget * 100, "OEE"));

            dataSets.Add(PrepareTargetDataSet(dataA, machineTargets.AvailabilityTarget * 100));
            dataSets.Add(PrepareDataSet(dataA, machineTargets.AvailabilityTarget * 100, "A"));

            dataSets.Add(PrepareTargetDataSet(dataP, machineTargets.PerformanceTarget * 100));
            dataSets.Add(PrepareDataSet(dataP, machineTargets.PerformanceTarget * 100, "P"));

            dataSets.Add(PrepareTargetDataSet(dataQ, machineTargets.QualityTarget * 100));
            dataSets.Add(PrepareDataSet(dataQ, machineTargets.QualityTarget * 100, "Q"));

            return dataSets;
        }
        public ChartDataSetViewModel PrepareDataSet(List<decimal> data, decimal targetValue, string title)
        {
            Color colorTransp = new Color();
            Color colorSolid = new Color();
            List<string> backgroundColors = new List<string>();
            List<string> borderColors = new List<string>();
            
            foreach (int d in data)
            {
                colorTransp = OeeResultToColor.AssignColor(targetValue, d);
                colorSolid = ColorHelper.ColorChangeTransparency(colorTransp, 1);
                backgroundColors.Add(ColorHelper.ConvertToHtmlRGBA(colorTransp));
                borderColors.Add(ColorTranslator.ToHtml(colorSolid));
            }

            return new ChartDataSetViewModel()
            {
                label = title,
                backgroundColor = backgroundColors,
                borderColor = borderColors,
                data = data,
                borderWidth = 1,
                datalabels = "LabelsBar",
            };
        }
        public virtual ChartDataSetViewModel PrepareTargetDataSet(List<decimal> data, decimal targetValue)
        {
            ChartDataSetViewModel target = new ChartDataSetViewModel()
            {
                label = "Cel",
                type = "line",
                backgroundColor = new List<string>(),
                borderColor = new List<string>() { "red" },
                data = new List<decimal>(),
                borderWidth = 1,
                lineTension = 0,
                pointBackgroundColor = "red",
                datalabels = "LabelsNone",
            };

            foreach(decimal targetItem in data)
            {
                if(targetItem == 0)
                {
                    target.data.Add(0);
                }
                else
                {
                    target.data.Add(targetValue);
                }
                target.backgroundColor.Add("rgba(0,0,0,0)");
            }
            
            return target;
        }
    }
}