using _MPPL_WEB_START.Areas._APPWEB.ViewModels;
using MDL_ONEPROD.ComponentENERGY.Entities;
using MDL_ONEPROD.ComponentENERGY.Repos;
using MDL_ONEPROD.ComponentOEE.Repos;
using MDL_ONEPROD.Model.OEEProd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.ONEPROD.Models
{
    public class ChartEnergyConsumptionPreparer : ChartDataPreparer
    {
        protected OEEReportProductionDataRepo prodDataList;
        protected EnergyConsumptionDataRepo energyDataList;
        protected int machineId;

        public ChartEnergyConsumptionPreparer(EnergyConsumptionDataRepo energyDataList, DateTime dateFrom, DateTime dateTo, int intervalInHours, int machineId) : base(dateFrom, dateTo, intervalInHours)
        {
            this.machineId = machineId;
            this.energyDataList = energyDataList;
        }
        public virtual ChartViewModel PrepareChartData(OEEReportProductionDataRepo prodDataList, string title)
        {
            this.prodDataList = prodDataList;

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

        public ChartDataSetViewModel PrepareProductionDataSet()
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
                ProductionQty = (decimal?)prodDataList.GetDataByTimeRangeAndMachineId(date, date.AddHours(intervalInHours), machineId).Sum(x => (decimal?)x.ProdQty);
                if(ProductionQty == null)
                {
                    ProductionQty = 0;
                }
                total.data.Add((decimal)ProductionQty);
                total.backgroundColor.Add("rgba(255,0,0,0.4)");
            }

            return total;
        }

        public ChartDataSetViewModel PrepareGasDataSet()
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
                EnergyCost = (decimal?)energyDataList.GetDataByTimeRangeAndMachineIdAndEnergyType(date, date.AddHours(intervalInHours), machineId, EnumEnergyType.Gas).Sum(x => (decimal?)x.TotalValue);
                if (EnergyCost == null)
                {
                    EnergyCost = 0;
                }
                total.data.Add((decimal)EnergyCost);
                total.backgroundColor.Add("rgba(105,145,195,0.5)");
            }

            return total;
        }

        public ChartDataSetViewModel PrepareElectricityDataSet()
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
                EnergyCost = (decimal?)energyDataList.GetDataByTimeRangeAndMachineIdAndEnergyType(date, date.AddHours(intervalInHours), machineId, EnumEnergyType.Electricity).Sum(x => (decimal?)x.TotalValue);
                if (EnergyCost == null)
                {
                    EnergyCost = 0;
                }
                total.data.Add((decimal)EnergyCost);
                total.backgroundColor.Add("rgba(15,145,195,0.5)");
            }

            return total;
        }

        public ChartDataSetViewModel PrepareWaterDataSet()
        {
            ChartDataSetViewModel total = new ChartDataSetViewModel()
            {
                label = "Woda ",
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
                EnergyCost = (decimal?)energyDataList.GetDataByTimeRangeAndMachineIdAndEnergyType(date, date.AddHours(intervalInHours), machineId, EnumEnergyType.Air).Sum(x => (decimal?)x.TotalValue);
                if (EnergyCost == null)
                {
                    EnergyCost = 0;
                }
                total.data.Add((decimal)EnergyCost);
                total.backgroundColor.Add("rgba(65,145,195,0.5)");
            }

            return total;
        }
    }
}