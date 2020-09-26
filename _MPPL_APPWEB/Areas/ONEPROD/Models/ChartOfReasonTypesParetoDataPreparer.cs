using _MPPL_WEB_START.Areas._APPWEB.Models;
using _MPPL_WEB_START.Areas._APPWEB.ViewModels;
using MDL_ONEPROD.ComponentOEE.Models;
using MDL_ONEPROD.ComponentOEE.Repos;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Model.Scheduling.Interface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.ONEPROD.Models
{
    public class ChartOfReasonTypesParetoDataPreparer : ChartDataPreparer
    {
        public List<ParetoModel> paretoList;
        public OeeCalculationModel oeeCalc;
        private decimal lastResult = 0m;
        //private EnumEntryType entryType;
        private int limit = 1000;

        public ChartOfReasonTypesParetoDataPreparer(List<ParetoModel> paretoModelList, OeeCalculationModel oeeCalc) //, EnumEntryType entryType)
        {
            this.paretoList = paretoModelList;
            this.oeeCalc = oeeCalc;
            this.lastResult = oeeCalc.Result;
            //this.entryType = entryType;
        }

        public virtual ChartViewModel PrepareChartData(string title, int limit = 1000, int languageId = 1)
        {
            this.limit = limit;
            ChartViewModel vm = new ChartViewModel();
            vm.labels = paretoList.Select(x => x.GetName(languageId)).Take(limit).ToList();
            vm.title = title;
            vm.datasets.Add(PrepareParetoShareLineDataSet());
            vm.datasets.Add(PrepareOeeImpactDataSet());
            vm.datasets.Add(PrepareParetoReasonTypeBarDataSet());
            return vm;
        }
        private ChartDataSetViewModel PrepareParetoReasonTypeBarDataSet()
        {
            List<string> backgroundColors = PrepareColorsTransparent(0.6);
            List<string> borderColors = paretoList.Select(x => x.Color).ToList();
            for(int i = 0; i < borderColors.Count(); i++)
            {
                if(borderColors[i] == null)
                {
                    borderColors[i] = "black";
                }
            }

            ChartDataSetViewModel dataSet = new ChartDataSetViewModel()
            {
                label = "Czas",
                displayUnit = " [min]",
                backgroundColor = backgroundColors.Take(limit).ToList(),
                borderColor = borderColors.Take(limit).ToList(),
                data = paretoList.Select(x => (decimal)x.Value / 60).Take(limit).ToList(),
                borderWidth = 1,
                yAxisID = "y-axis-1",
                datalabels = "LabelsNone",
            };
            return dataSet;
        }
        private ChartDataSetViewModel PrepareParetoShareLineDataSet()
        {
            List<string> backgroundColors = new List<string>();
            List<string> borderColors = new List<string>() { "red" };
            List<decimal> dataList = new List<decimal>();
            decimal value = 0m;
            decimal sum = paretoList.Sum(x => x.Value);

            if (sum != 0)
            {
                foreach (var val in paretoList)
                {
                    value += ((val.Value / sum) * 100);
                    dataList.Add((decimal)value);
                }
            }

            ChartDataSetViewModel dataset = new ChartDataSetViewModel()
            {
                label = "Udział",
                displayUnit = "%",
                type = "line",
                backgroundColor = backgroundColors.Take(limit).ToList(),
                borderColor = borderColors.Take(limit).ToList(),
                data = dataList.Take(limit).ToList(),
                borderWidth = 1,
                lineTension = 0,
                pointBackgroundColor = "red",
                yAxisID = "y-axis-2",
                datalabels = "LabelsLineRound",
            };
            return dataset;
        }

        private ChartDataSetViewModel PrepareOeeImpactDataSet()
        {
            List<string> backgroundColors = new List<string>();
            List<string> borderColors = new List<string>() { "blue" };
            List<decimal> dataList = new List<decimal>();
            decimal value = 0m;

            foreach (var val in paretoList)
            {
                value = EstimateOee(val.Value, (EnumEntryType)val.EntryType);
                dataList.Add((decimal)value);
            }

            ChartDataSetViewModel dataset = new ChartDataSetViewModel()
            {
                label = "Wpływ",
                displayUnit = "%",
                type = "line",
                backgroundColor = backgroundColors.Take(limit).ToList(),
                borderColor = borderColors.Take(limit).ToList(),
                data = dataList.Take(limit).ToList(),
                borderWidth = 1,
                lineTension = 0,
                fontSize = 12,
                pointBackgroundColor = "blue",
                yAxisID = "y-axis-2",
                datalabels = "LabelsLine",
            };
            return dataset;
        }

        private decimal EstimateOee(decimal stoppageTime, EnumEntryType entryType)
        {
            if (entryType == EnumEntryType.StopUnplanned)
            //if (entryType == EnumEntryType.StopUnplannedBreakdown || entryType == EnumEntryType.StopUnplanned || entryType == EnumEntryType.StopUnplannedChangeOver)
            {
                oeeCalc.StopUnplannedTime -= stoppageTime; //oeeCalc.StopUnplannedBreakdownsTime -= stoppageTime;
                oeeCalc.GoodProductionTime += stoppageTime;
                oeeCalc.CalculateAvailability();
                oeeCalc.CalculatePerformance();
                oeeCalc.CalculateQuality();
                oeeCalc.CalculateResult();
                oeeCalc.StopUnplannedTime += stoppageTime; //oeeCalc.StopUnplannedBreakdownsTime += stoppageTime;
                oeeCalc.GoodProductionTime -= stoppageTime;
            }
            else if (entryType == EnumEntryType.StopPlanned)
            //else if(entryType == EnumEntryType.StopPlannedChangeOver || entryType == EnumEntryType.StopPlanned)
            {
                oeeCalc.StopPlannedTime -= stoppageTime; //oeeCalc.StopPlannedChangeOverTime -= stoppageTime;
                oeeCalc.GoodProductionTime += stoppageTime;
                oeeCalc.CalculateAvailability();
                oeeCalc.CalculatePerformance();
                oeeCalc.CalculateQuality();
                oeeCalc.CalculateResult();
                oeeCalc.StopPlannedTime += stoppageTime; //oeeCalc.StopPlannedChangeOverTime += stoppageTime;
                oeeCalc.GoodProductionTime -= stoppageTime;
            }
            //else if(entryType == EnumEntryType.StopUnplannedPreformance)
            else if(entryType == EnumEntryType.StopPerformance)
            {
                oeeCalc.GoodProductionTime += stoppageTime;
                oeeCalc.CalculateAvailability();
                oeeCalc.CalculatePerformance();
                oeeCalc.CalculateQuality();
                oeeCalc.CalculateResult();
                oeeCalc.GoodProductionTime -= stoppageTime;
            }
            else
            {
                oeeCalc.CalculateAvailability();
                oeeCalc.CalculatePerformance();
                oeeCalc.CalculateQuality();
                oeeCalc.CalculateResult();
            }
            
            return (oeeCalc.Result - lastResult) * 100;
        }
        
        private List<string> PrepareColorsTransparent(double transparency)
        {
            List<string> colors = paretoList.Select(x => x.Color).ToList();
            for(int i = 0; i < colors.Count; i++)
            {
                colors[i] = ColorHelper.ColorChangeTransparency(colors[i], transparency);
            }
            return colors;
        }
    }
}