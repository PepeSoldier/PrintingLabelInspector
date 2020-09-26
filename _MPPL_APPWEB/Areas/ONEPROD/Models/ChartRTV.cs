using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _MPPL_WEB_START.Areas._APPWEB.ViewModels;
using MDL_ONEPROD.ComponentRTV.Repos;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.Model.Scheduling.Interface;

namespace _MPPL_WEB_START.Areas.ONEPROD.Models
{
    public class ChartRTV
    {
        private DateTime dateFrom;
        private DateTime dateTo;
        private int machineId;
        private int intervalInMinutes;
        private ResourceOP resource;
        private List<RTVOEEProductionData> prodDataList;

        public ChartRTV(DateTime dateFrom, DateTime dateTo, int intervalInMinutes, List<RTVOEEProductionData> prodData, ResourceOP resource)
        {
            this.prodDataList = prodData;
            this.dateTo = dateTo;
            this.dateFrom = dateFrom;
            this.machineId = resource.Id;
            this.resource = resource;
            this.intervalInMinutes = intervalInMinutes;
        }

        public List<string> PrepareLabels()
        {
            List<string> labels = new List<string>();
            for (DateTime date = dateFrom; date <= dateTo; date = date.AddMinutes(intervalInMinutes))
            {
                labels.Add(date.Hour + ":" + date.Minute.ToString("00"));
            }
            return labels;
        }

        public ChartViewModel PrepareChartData(RTVOEEProductionDataRepo rTVOEEProductionDataRepo, string title)
        {
            ChartViewModel vm = new ChartViewModel();
            vm.labels = PrepareLabels();
            vm.title = title;
            
            vm.datasets.Add(PrepareDataSet());
            vm.datasets.Add(PrepareDataSetSimulated());

            return vm;
        }

        public ChartDataSetViewModel PrepareDataSetSimulated()
        {
            string breaks = "8:00-8:05;9:50-10:10;12:00-12:05;16:00-16:05;17:50-18:10;20:00-20:05;0:00-0:05;1:50-2:10;4:00-4:05";
            breaks = resource.Breaks != null && resource.Breaks.Length > 3 ? resource.Breaks : breaks;

            BreakManager bm = new BreakManager(breaks);

            RTVOEEProductionData prodData = null;
            List<string> backgroundColors = new List<string>() { "#2abcac" };//PrepareColorsTransparent(0.6);
            List<string> borderColors = new List<string>() { "#2abcac" };//paretoList.Select(x => x.Color).ToList();
            List<decimal> data = new List<decimal>(); //{ 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };
            DateTime dateToNow = dateTo < DateTime.Now ? dateTo : DateTime.Now;
            DateTime lastChange = dateFrom;
            DateTime nextChange = dateFrom;
            decimal qtyTotal = 0;
            decimal cycleTime = 0;
            decimal estimatedQtyPerMinute = 0;

            //Petla od początku do teraz
            for (DateTime currentMinute = dateFrom; currentMinute <= dateToNow; currentMinute = currentMinute.AddMinutes(intervalInMinutes))
            {
                if (bm.IsMinuteInBreakTime(currentMinute)) //simulate break
                {
                    qtyTotal += 0;
                }
                else //find cycle time basing on last entry
                {
                    if (nextChange <= currentMinute)
                    {
                        prodData = prodDataList
                            .Where(x => x.ProductionDate <= currentMinute && cycleTime != x.CycleTime && x.ReasonType.EntryType != EnumEntryType.Undefined)
                            .OrderByDescending(x => x.Id).Take(1).FirstOrDefault();

                        if (prodData != null && cycleTime != prodData.CycleTime)
                        {
                            estimatedQtyPerMinute = EstimateQtyPerMinute(prodData.CycleTime);
                            cycleTime = prodData.CycleTime;
                            lastChange = currentMinute;
                        }

                        //find next change
                        prodData = prodDataList
                            .Where(x => currentMinute <= x.ProductionDate && cycleTime != x.CycleTime && x.ReasonType.EntryType != EnumEntryType.Undefined)
                            .OrderBy(x => x.Id).Take(1).FirstOrDefault();

                        if (prodData != null && cycleTime != prodData.CycleTime)
                            nextChange = prodData.ProductionDate;
                        else
                            nextChange = dateToNow;
                    }
                    qtyTotal += estimatedQtyPerMinute;
                }
                data.Add(qtyTotal);
            }

            //Petla od teraz do końca
            prodData = prodDataList.OrderByDescending(x => x.Id).Take(1).FirstOrDefault();
            for (DateTime currentMinute = DateTime.Now; currentMinute <= dateTo; currentMinute = currentMinute.AddMinutes(intervalInMinutes))
            {
                if (bm.IsMinuteInBreakTime(currentMinute))
                {
                    estimatedQtyPerMinute = 0;
                }
                else
                {
                    if (prodData != null)
                    {
                        //estimatedQtyPerMinute = EstimateQtyPerMinute(prodData, DateTime.Now, dateTo);
                        estimatedQtyPerMinute = EstimateQtyPerMinute(prodData.CycleTime);
                    }
                }
                qtyTotal += estimatedQtyPerMinute;
                data.Add(qtyTotal);
            }

            ChartDataSetViewModel dataSet = new ChartDataSetViewModel()
            {
                label = "Czas",
                displayUnit = " [h]",
                backgroundColor = backgroundColors,
                borderColor = borderColors,
                data = data,
                borderWidth = 2,
                pointBackgroundColor = "rgba(0,0,0,0)",
                pointBorderWidth = 0,
                pointRadius = 0,
                datalabels = "LabelsNone",
            };
            return dataSet;
        }

        //private decimal EstimateQtyPerMinute(RTVOEEProductionData prodData, DateTime from, DateTime to)
        //{
        //    decimal estimatedQtyPerMinute;
        //    //int remainingSec = (int)(to - from).TotalSeconds;
        //    //if (remainingSec > 0 && prodData != null)
        //    //{
        //    //    int estimatedQty = (int)((prodData.CycleTime > 0) ? (remainingSec / prodData.CycleTime) : prodData.ProdQtyTotal);
        //    //    estimatedQtyPerMinute = estimatedQty / (decimal)((decimal)remainingSec / (decimal)60);
        //    //}
        //    if(prodData != null && prodData.CycleTime > 0)
        //    {
        //        estimatedQtyPerMinute = 60 / prodData.CycleTime;
        //    }
        //    else
        //    {
        //        estimatedQtyPerMinute = 1;
        //    }
        //    return estimatedQtyPerMinute;
        //}
        private decimal EstimateQtyPerMinute(decimal CycleTime)
        {
            decimal estimatedQtyPerMinute = (CycleTime > 0)? 60 / CycleTime : 1;
            return estimatedQtyPerMinute;
        }

        public ChartDataSetViewModel PrepareDataSet()
        {
            List<string> backgroundColors = new List<string>() { "#f08604" };//PrepareColorsTransparent(0.6);
            List<string> borderColors = new List<string>() { "#f08604" };//paretoList.Select(x => x.Color).ToList();
            List<decimal> data = new List<decimal>(); //{ 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };

            int qtyTotal = 0;
            RTVOEEProductionData prodData;

            DateTime dateTo1 = dateTo < DateTime.Now ? dateTo : DateTime.Now;
            for (DateTime date = dateFrom; date <= dateTo1; date = date.AddMinutes(intervalInMinutes))
            {
                prodData = prodDataList.Where(x => date <= x.ProductionDate && x.ProductionDate < date.AddMinutes(intervalInMinutes) && x.ProdQtyTotal >= 0)
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefault();
                
                qtyTotal = (prodData != null)? prodData.ProdQtyShift : qtyTotal;
                data.Add(qtyTotal);
            }

            ChartDataSetViewModel dataSet = new ChartDataSetViewModel()
            {
                label = "Czas",
                displayUnit = " [h]",
                backgroundColor = backgroundColors,
                borderColor = borderColors,
                data = data,
                borderWidth = 3,
                pointBackgroundColor = "rgba(0,0,0,0)",
                pointBorderWidth = 0,
                pointRadius = 0,
                datalabels = "LabelsNone", 
            };
            return dataSet;
        }
    }


    public class Break
    {
        public TimeSpan Length { get { return Stop - Start; } }

        public DateTime Start { get; set; }
        public DateTime Stop { get; set; }
    }
    public class BreakManager
    {
        List<Break> breaks;

        public BreakManager(string breaksStr)
        {
            breaks = PrepareBreakList(breaksStr);
        }

        private List<Break> PrepareBreakList(string breaksStr)
        {
            List<Break> breaksTemp = new List<Break>();
            string[] breaksArray = breaksStr.Split(';');

            foreach (string br in breaksArray)
            {
                string start = br.Split('-')[0];
                string stop = br.Split('-')[1];

                breaksTemp.Add(new Break()
                {
                    Start = Convert.ToDateTime(start),
                    Stop = Convert.ToDateTime(stop)
                });
            }

            return breaksTemp.OrderBy(x=>x.Start).ToList();
        }

        public DateTime FindNextBreak(DateTime date)
        {
            if(breaks == null || breaks.Count < 1)
            {
                return date;
            }

            TimeSpan timeOfDay = date.TimeOfDay;
            Break b = breaks.FirstOrDefault(x => x.Start.TimeOfDay >= timeOfDay);

            if(b != null)
            {
                return date.Date.AddSeconds(b.Start.TimeOfDay.TotalSeconds);
            }
            else
            {
                date = date.Date.AddDays(1);
                timeOfDay = date.TimeOfDay;
                b = breaks.FirstOrDefault(x => x.Start.TimeOfDay >= timeOfDay);

                if (b != null)
                {
                    return date.Date.AddSeconds(b.Start.TimeOfDay.TotalSeconds);
                }
                return date;
            }
        }
        public bool IsMinuteInBreakTime(DateTime date)
        {
            var timeOfDay = date.TimeOfDay;
            foreach (Break br in breaks)
            {
                if (br.Start.TimeOfDay <= timeOfDay && timeOfDay <= br.Stop.TimeOfDay)
                {
                    return true;   
                }
            }

            return false;
        }
    }
}