using _MPPL_WEB_START.Areas._APPWEB.ViewModels;
using MDL_ONEPROD.ComponentENERGY.Entities;
using MDL_ONEPROD.ComponentENERGY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.ONEPROD.Models
{
    public class ChartEnergyOEEReport : ChartDataPreparer
    {

        protected EnergyAnalyzeModel EnergyAnModel;

        public ChartEnergyOEEReport()
        {
        }

        public ChartEnergyOEEReport(EnergyAnalyzeModel energyAnModel,DateTime dateFrom, DateTime dateTo, int intervalInHours) : base(dateFrom, dateTo, intervalInHours)
        {
            this.EnergyAnModel = energyAnModel;
        }

        public ChartViewModel PrepareChartPricePerUnit(string title)
        {
            ChartViewModel vm = new ChartViewModel();
            vm.labels = PrepareLabels();
            vm.title = title;
            vm.datasets = new List<ChartDataSetViewModel>();
            vm.datasets.Add(PreparePricePerUnitDataSet());

            return vm;
        }

        public ChartViewModel PrepareChartTotalCost(string title)
        {
            ChartViewModel vm = new ChartViewModel();
            vm.labels = PrepareLabelsForPieChart();
            vm.title = title;
            vm.datasets = new List<ChartDataSetViewModel>();
            vm.datasets.Add(PrepareTotalCostDataSet());

            return vm;
        }

        public ChartViewModel PrepareChartUseEnergyPerUnit(string title)
        {
            ChartViewModel vm = new ChartViewModel();
            vm.labels = PrepareLabelsForPieChart();
            vm.title = title;
            vm.datasets = new List<ChartDataSetViewModel>();
            vm.datasets.Add(PrepareUseEnergyPerUnitDataSet());

            return vm;
        }

        public ChartViewModel PrepareChartEnergyConsumption(string title)
        {
            ChartViewModel vm = new ChartViewModel();
            vm.labels = PrepareLabels();
            vm.title = title;
            vm.datasets = new List<ChartDataSetViewModel>();
            vm.datasets.Add(PrepareProductionDataSet());
            vm.datasets.Add(PrepareGasDataSet());
            vm.datasets.Add(PrepareElectricityDataSet());
            vm.datasets.Add(PrepareWaterDataSet());

            return vm;
        }

        public List<string> PrepareLabelsForPieChart()
        {
            List<string> labels = new List<string>();
            string label = string.Empty;

            foreach (EnumEnergyType enumEnergyType in (EnumEnergyType[])Enum.GetValues(typeof(EnumEnergyType)))
            {
                if (enumEnergyType != EnumEnergyType.ALL && enumEnergyType != EnumEnergyType.Undefined)
                {
                    label = enumEnergyType.ToString();
                    labels.Add(label);
                }
            }

            return labels;
        }

        private ChartDataSetViewModel PreparePricePerUnitDataSet()
        {
            ChartDataSetViewModel total = new ChartDataSetViewModel()
            {
                label = "PPUnit_label",
                type = "bar",
                backgroundColor = new List<string>(),
                borderColor = new List<string>() { "rgba(0, 243, 255,0.8)" },
                data = new List<decimal>(),
                borderWidth = 1,
                lineTension = 0,
                fontSize = 11,
                fontColor = "black",
                pointBackgroundColor = "rgba(0, 243, 255,0.8)",
                datalabels = "LabelsEnergyLine",
            };
            decimal? PricePerUnit = 0;
            for (DateTime date = dateFrom; date < dateTo; date = date.AddHours(intervalInHours))
            {
                PricePerUnit = EnergyAnModel.PricePerProductionUnit(date, date.AddHours(intervalInHours));
                total.data.Add((decimal)PricePerUnit);
                total.backgroundColor.Add("rgba(0, 243, 255,0.8)");
            }

            return total;
        }

        private ChartDataSetViewModel PrepareTotalCostDataSet()
        {
            ChartDataSetViewModel total = new ChartDataSetViewModel()
            {
                label = "TTCOST_Label",
                type = "pie",
                backgroundColor = new List<string>() { "rgba(0,255,0,0.8)", "rgba(255,255,0,0.9)", "rgba(0, 243, 255,0.8)", "rgba(255,30,30,0.2)" },
                borderColor = new List<string>() { "rgba(255,0,0,0.2)", "rgba(255,10,10,0.2)", "rgba(255,20,20,0.2)", "rgba(255,30,30,0.2)" },
                data = new List<decimal>(),
                borderWidth = 1,
                lineTension = 0,
                fontSize = 11,
                fontColor = "black",
                pointBackgroundColor = "rgba(255,0,0,0.2)",
                datalabels = "LabelsEnergyLine",
            };

            decimal TotalCostByType = 0;
            int colorForChart = 0;
            foreach (EnumEnergyType enumEnergyType in (EnumEnergyType[])Enum.GetValues(typeof(EnumEnergyType)))
            {
                if(enumEnergyType != EnumEnergyType.ALL && enumEnergyType != EnumEnergyType.Undefined)
                {
                    TotalCostByType = EnergyAnModel.TotalCostByEnergyType(enumEnergyType);
                    total.data.Add(TotalCostByType);
                    //total.backgroundColor.Add("rgba(" + (255 - colorForChart) + ", " + (colorForChart + 30) + "," + colorForChart + ",0.2)");
                    colorForChart += 30;
                }
            }
            return total;
        }

        private ChartDataSetViewModel PrepareUseEnergyPerUnitDataSet()
        {
            ChartDataSetViewModel total = new ChartDataSetViewModel()
            {
                label = "PUEPU_Label",
                type = "pie",
                backgroundColor = new List<string>() { "rgba(0,255,0,0.8)", "rgba(255,255,0,0.9)", "rgba(0, 243, 255,0.8)", "rgba(255,30,30,0.2)" },
                borderColor = new List<string>() { "rgba(255,0,0,0.2)" },
                data = new List<decimal>(),
                borderWidth = 1,
                lineTension = 0,
                fontSize = 11,
                fontColor = "black",
                pointBackgroundColor = "rgba(255,0,0,0.2)",
                datalabels = "LabelsEnergyLine",
            };

            
            decimal UsePerUnit = 0;
            foreach (EnumEnergyType enumEnergyType in (EnumEnergyType[])Enum.GetValues(typeof(EnumEnergyType)))
            {
                if (enumEnergyType != EnumEnergyType.ALL && enumEnergyType != EnumEnergyType.Undefined)
                {
                    UsePerUnit = EnergyAnModel.UsePerUnitByEnergyType(enumEnergyType);
                    total.data.Add(UsePerUnit);
                }
            }
            return total;
        }

        private ChartDataSetViewModel PrepareProductionDataSet()
        {
            ChartDataSetViewModel total = new ChartDataSetViewModel()
            {
                label = "Produkcja",
                type = "line",
                backgroundColor = new List<string>(),
                borderColor = new List<string>() { "red" },
                data = new List<decimal>(),
                borderWidth = 1,
                lineTension = 0,
                pointBackgroundColor = "red",
                yAxisID = "y-axis-2",
                datalabels = "LabelsLineRound",
            };
            decimal? ProductionQty = 0;
            for (DateTime date = dateFrom; date < dateTo; date = date.AddHours(intervalInHours))
            {
                ProductionQty = EnergyAnModel.GetProductionQty(date, date.AddHours(intervalInHours));
                total.data.Add((decimal)ProductionQty);
                total.backgroundColor.Add("rgba(255,0,0,0.4)");
            }

            return total;
        }

        private ChartDataSetViewModel PrepareGasDataSet()
        {
            ChartDataSetViewModel total = new ChartDataSetViewModel()
            {
                label = "Gaz ",
                displayUnit = " [PLN]",
                type = "bar",
                backgroundColor = new List<string>(),
                borderColor = new List<string>() { "rgba(105,145,195,0.5)" },
                data = new List<decimal>(),
                borderWidth = 1,
                lineTension = 0,
                fontSize = 11,
                fontColor = "black",
                yAxisID = "y-axis-1",
                pointBackgroundColor = "rgba(105,145,195,0.5)",
                datalabels = "LabelsLineRound",
            };
            decimal? EnergyCost = 0;
            for (DateTime date = dateFrom; date < dateTo; date = date.AddHours(intervalInHours))
            {
                EnergyCost = EnergyAnModel.TotalCostByEnergyType(EnumEnergyType.Gas, date, date.AddHours(intervalInHours));
                total.data.Add((decimal)EnergyCost);
                total.backgroundColor.Add("rgba(105,145,195,0.5)");
            }
            return total;
        }

        private ChartDataSetViewModel PrepareElectricityDataSet()
        {
            ChartDataSetViewModel total = new ChartDataSetViewModel()
            {
                label = "En.Elektr. ",
                displayUnit = " [PLN]",
                type = "bar",
                backgroundColor = new List<string>(),
                borderColor = new List<string>() { "rgba(105,145,195,0.5)" },
                data = new List<decimal>(),
                borderWidth = 1,
                lineTension = 0,
                fontSize = 11,
                fontColor = "black",
                yAxisID = "y-axis-1",
                pointBackgroundColor = "rgba(105,145,195,0.5)",
                datalabels = "LabelsLineRound",
            };
            decimal? EnergyCost = 0;
            for (DateTime date = dateFrom; date < dateTo; date = date.AddHours(intervalInHours))
            {
                EnergyCost = EnergyAnModel.TotalCostByEnergyType(EnumEnergyType.Electricity, date, date.AddHours(intervalInHours));
                total.data.Add((decimal)EnergyCost);
                total.backgroundColor.Add("rgba(15,145,195,0.5)");
            }
            return total;
        }

        private ChartDataSetViewModel PrepareWaterDataSet()
        {
            ChartDataSetViewModel total = new ChartDataSetViewModel()
            {
                label = "Air ",
                displayUnit = " [PLN]",
                type = "bar",
                backgroundColor = new List<string>(),
                borderColor = new List<string>() { "rgba(105,145,195,0.5)" },
                data = new List<decimal>(),
                borderWidth = 1,
                lineTension = 0,
                fontSize = 11,
                fontColor = "black",
                yAxisID = "y-axis-1",
                pointBackgroundColor = "rgba(105,145,195,0.5)",
                datalabels = "LabelsLineRound",
            };
            decimal? EnergyCost = 0;
            for (DateTime date = dateFrom; date < dateTo; date = date.AddHours(intervalInHours))
            {
                EnergyCost = EnergyAnModel.TotalCostByEnergyType(EnumEnergyType.Air, date, date.AddHours(intervalInHours));
                total.data.Add((decimal)EnergyCost);
                total.backgroundColor.Add("rgba(65,145,195,0.5)");
            }
            return total;
        }
    }
}