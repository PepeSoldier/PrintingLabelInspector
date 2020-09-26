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
    public class ChartParetoScrapProcessDataPreparer : ChartByEntryTypeParetoDataPreparer
    {
        //public List<ParetoModel> paretoList;
        //public OeeCalculationModel oeeCalc;
        private decimal lastResult = 0m;
        private int limit = 1000;
        Dictionary<int, string> translatePL = new Dictionary<int, string>();
        Dictionary<int, string> translateEn = new Dictionary<int, string>();

        public ChartParetoScrapProcessDataPreparer(List<ParetoModel> paretoModelList, OeeCalculationModel oeeCalc) : base (paretoModelList, oeeCalc, 0)
        {
            this.paretoList = paretoModelList;
            this.oeeCalc = oeeCalc;
            this.lastResult = oeeCalc.Result;
            //CreateDictionaries();
            //Translate();
        }

        public override ChartViewModel PrepareChartData(string title, int limit = 1000, int languageId = 1)
        {
            this.limit = limit;
            ChartViewModel vm = new ChartViewModel();
            vm.labels = paretoList.Select(x => x.GetName(languageId)).Take(limit).ToList();
            vm.title = title;
            vm.datasets.Add(PrepareParetoShareLineDataSet());

            //vm.datasets.Add(PrepareOeeImpactDataSet());
            

            vm.datasets.Add(PrepareParetoReasonBarDataSet());
            return vm;
        }
        private ChartDataSetViewModel PrepareParetoReasonBarDataSet()
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
                label = "Ilość",
                displayUnit = " [szt]",
                backgroundColor = backgroundColors.Take(limit).ToList(),
                borderColor = borderColors.Take(limit).ToList(),
                data = paretoList.Select(x => (decimal)x.Value).Take(limit).ToList(),
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
        private List<string> PrepareColorsTransparent(double transparency)
        {
            List<string> colors = paretoList.Select(x => x.Color).ToList();
            for(int i = 0; i < colors.Count; i++)
            {
                colors[i] = ColorHelper.ColorChangeTransparency(colors[i], transparency);
            }
            return colors;
        }

        private void CreateDictionaries()
        {
            translatePL.Add(12, "Scrap Procesowy");
            translatePL.Add(13, "Scrap Procesowy - rysy");
            translatePL.Add(14, "Scrap Procesowy - wgnioty");
            translatePL.Add(15, "Scrap Procesowy - pekniecia");
            translatePL.Add(16, "Scrap Procesowy - marszczenia");

            translateEn.Add(12, "Scrap Process");
            translateEn.Add(13, "Scrap Process - Scratch ");
            translateEn.Add(14, "Scrap Process - Dent");
            translateEn.Add(15, "Scrap Process - Crack");
            translateEn.Add(16, "Scrap Process - Fold");
        }
        private void Translate()
        {
            KeyValuePair<int, string> v;

            foreach (var val in paretoList)
            {
                v = translatePL.FirstOrDefault(x => x.Key == Convert.ToInt32(val.Name));
                val.Name = v.Value ?? "?";
                v = translateEn.FirstOrDefault(x => x.Key == Convert.ToInt32(val.NameEnglish));
                val.NameEnglish = v.Value ?? "?";
            }
        }
    }
}