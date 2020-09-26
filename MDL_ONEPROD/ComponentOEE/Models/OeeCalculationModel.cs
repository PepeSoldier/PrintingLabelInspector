using MDL_ONEPROD.ComponentOEE.Repos;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Model.Scheduling.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Enums;

namespace MDL_ONEPROD.ComponentOEE.Models
{
    public class OeeCalculationModel
    {
        public OeeCalculationModel()
        {
            prodDataAnalyzer = new ProductionAnalyzeModel();
            productionData = new List<OEEReportProductionDataAbstract>();
            totalAvailavleSeconds = 24 * 60 * 60;
        }
        public OeeCalculationModel(List<OEEReportProductionDataAbstract> productionData)
        {
            prodDataAnalyzer = new ProductionAnalyzeModel();
            this.productionData = productionData;
            
            totalAvailavleSeconds = CalcTotalAvailableSeconds(productionData);
            totalAvailavleSeconds -= CalcTotalClosedSeconds(productionData);
        }
        
        public ProductionAnalyzeModel ProductionAnalyzer { get { return prodDataAnalyzer; } }
        protected ProductionAnalyzeModel prodDataAnalyzer;
        protected List<OEEReportProductionDataAbstract> productionData;
        protected int totalAvailavleSeconds = 0;

        #region PublicProperties
        public decimal ShiftTime { get; set; }
        public decimal GoodProductionTime { get; set; }
        //public decimal ScrapMaterialTime { get; set; }
        public decimal ScrapProcessTime { get; set; }
        //public decimal TotalUsedTime { get; set; }

        public decimal TotalCount { get; set; }
        public decimal GoodCount { get; set; }
        public decimal ScrapCount { get { return TotalCount - GoodCount; } }
        public decimal ScrapMaterialCount { get { return prodDataAnalyzer.ScrapMaterialQtyCount(productionData); } }
        public decimal ScrapProcessCount { get { return prodDataAnalyzer.ScrapProcessQtyCount(productionData); } }

        public decimal StopPlannedTime { get; set; }
        //public decimal StopPlannedChangeOverTime { get; set; }
        public decimal StopUnplannedTime { get; set; }
        //public decimal StopUnplannedBreakdownsTime { get; set; }
        public decimal StopPerformanceTime { get; set; } 
        //public decimal StopUnplannedPerformanceTime { get; set; }
        //public decimal StopUnplannedChangeOverTime { get; set; }
        public decimal StopQualityTime { get; set; } 
        public decimal MicrostopsAuto { get { return CalculateMikrostopsAuto(); } }
        

        public decimal Availability { get; set; }
        public decimal Performance { get; set; }
        public decimal Quality { get; set; }
        public decimal Result { get; private set; }
        public int NRFT { get { return CalculateNRFT(); }  }
        #endregion

        public void CalculateOEE()
        {
            ShiftTime = totalAvailavleSeconds;
            GoodProductionTime = prodDataAnalyzer.ProductionQtyToTime(productionData, EnumEntryType.Production);
            //ScrapMaterialTime = prodDataAnalyzer.ProductionQtyToTime(productionData, EnumEntryType.ScrapMaterial);
            ScrapProcessTime = prodDataAnalyzer.ProductionQtyToTime(productionData, EnumEntryType.ScrapProcess);
            //TotalUsedTime = GoodProductionTime + ScrapProcessCount;

            GoodCount = prodDataAnalyzer.GoodQtyCount(productionData);
            TotalCount = prodDataAnalyzer.TotalQtyCount(productionData);

            StopPlannedTime = prodDataAnalyzer.ProductionQtyToTime(productionData, EnumEntryType.StopPlanned);
            StopUnplannedTime = prodDataAnalyzer.ProductionQtyToTime(productionData, EnumEntryType.StopUnplanned);
            StopPerformanceTime = prodDataAnalyzer.ProductionQtyToTime(productionData, EnumEntryType.StopPerformance);
            StopQualityTime = prodDataAnalyzer.ProductionQtyToTime(productionData, EnumEntryType.StopQuality);
            //StopPlannedChangeOverTime = prodDataAnalyzer.ProductionQtyToTime(productionData, EnumEntryType.StopPlannedChangeOver);
            //StopUnplannedBreakdownsTime = prodDataAnalyzer.ProductionQtyToTime(productionData, EnumEntryType.StopUnplannedBreakdown);
            //StopUnplannedChangeOverTime = prodDataAnalyzer.ProductionQtyToTime(productionData, EnumEntryType.StopUnplannedChangeOver);

            CalculateAvailability();
            CalculatePerformance();
            CalculateQuality();

            CalculateResult();
        }
        public void CalculateResult()
        {
            if(Availability > 0 && Performance > 0 && Quality > 0)
            {
                Result = Availability * Performance * Quality;
            }
        }
        public void CalculateAvailability()
        {
            //Availability = Run Time / Planned Production Time
            decimal plannedProductionTime = CalcPlannedProductionTime();
            decimal runTime = CalcRunTime();

            Availability = plannedProductionTime > 0 ? (decimal)runTime / (decimal)plannedProductionTime : 0;
        }
        public void CalculatePerformance()
        {
            decimal idealTotalProductionTime = CalcIdealTotalProductionTime();
            decimal runTime = CalcRunTime() - StopQualityTime;
            //Performance = (Ideal Cycle Time × Total Count) / Run Time
            Performance = runTime > 0 ? (decimal)idealTotalProductionTime / (decimal)runTime : 0;
        }
        public void CalculateQuality()
        {

            ////Poprzednia formuła wyliczała wskaźnik Quality nasępująco:
            ////this.Quality = GoodCount / TotalCount;

            ////gdzie TotalCount = GoodCount + ScrapCount. Wszystko uwzględnione w sztukach.

            ////Nowa formuła wylicza wszystko w oparciu o czas:

            ////czas poświęcony na wyprodukowanie komponentów zgodnych jakościowo
            //decimal fullyProductiveTime = GoodProductionTime;
            ////Czas dostępny pomniejszony o mikropostoje, postoje planowane i nieplanowane
            //decimal netRunTime = ShiftTime - StopPerformanceTime - StopPlannedTime - StopUnplannedTime;

            ////Różnicę pomiędzy netRunTime a fullyProductiveTime stanowią postoje jakościowe i czas zmarnowany na produkowanie wadliwych elementów
            ////netRunTime - fullyProductiveTime = StopQualityTime + ScrapProcessTime

            //this.Quality = netRunTime != 0? fullyProductiveTime / netRunTime : 0;

            decimal totalQualityStopTime = StopQualityTime + ScrapProcessTime;
            decimal netRunTime = ShiftTime - StopPerformanceTime - StopPlannedTime - StopUnplannedTime;

            Quality = Math.Max(0, netRunTime > 0 ? 1 - totalQualityStopTime / netRunTime : 0);

            //decimal qualityInTime = netRunTime > 0 ? GoodProductionTime / netRunTime : 0;
            //this.Quality = qualityInTime;
            //this.Quality = TotalCount > 0 ? (decimal)GoodCount / (decimal)TotalCount : 0;
        }
        public int CalculateNRFT()
        {
            return TotalCount != 0? Convert.ToInt32(1000000 * ScrapCount / TotalCount) : 0;
        }

        private decimal CalcPlannedProductionTime()
        {
            return ShiftTime - CalcPlannedStopTime();
        }
        private decimal CalcPlannedStopTime()
        {
            return StopPlannedTime; // + StopPlannedChangeOverTime;
        }
        private decimal CalcUnplannedStopTime()
        {
            return StopUnplannedTime; //+ StopUnplannedBreakdownsTime + StopUnplannedChangeOverTime;
        }
        private decimal CalcIdealTotalProductionTime()
        {
            //return GoodProductionTime + ScrapMaterialTime + ScrapProcessTime;
            return GoodProductionTime + ScrapProcessTime;
        }
        private decimal CalcRunTime()
        {
            decimal plannedProdTime = CalcPlannedProductionTime();
            decimal unplannedStopTime = CalcUnplannedStopTime();
            
            return plannedProdTime - unplannedStopTime;
        }
        private decimal CalculateMikrostopsAuto()
        {
            var mst = ShiftTime - CalcIdealTotalProductionTime();
            mst = mst - CalcPlannedStopTime();
            mst = mst - CalcUnplannedStopTime();
            mst = mst - StopPerformanceTime;
            mst = mst - StopQualityTime;

            return mst;
        }

        protected virtual int CalcTotalAvailableSeconds(List<OEEReportProductionDataAbstract> productionData)
        {
            int totalSeconds = (int)productionData
                .Where(x => 
                    x.ReasonType != null && 
                    x.ReasonType.EntryType == EnumEntryType.TimeAvailable)
                .Sum(x => x.UsedTime);

            //Poniższy kod odejmie ilość do końca zmiany dla wszystkich maszyn.
            DateTime dateTime_minus470 = DateTime.Now.AddMinutes(-470); //spacjalnie nie 480 - na 10 ostatnich minut zmiany ma nie odejmować
            var currentShiftTimesAvailable = productionData
                .Where(x => 
                    x.ReasonType != null && 
                    x.ReasonType.EntryType == EnumEntryType.TimeAvailable && 
                    x.ProductionDate > dateTime_minus470)
                .ToList();

            if(currentShiftTimesAvailable.Count > 0)
            {
                DateTime now = DateTime.Now;
                int endShiftHour = now.Hour < 6 ? 6 : now.Hour < 14 ? 14 : now.Hour > 22 ? 30 : 22;
                DateTime endShift = DateTime.Now.Date.AddHours(endShiftHour);
                int secondsToShiftEnd = (int)(endShift - now).TotalSeconds;

                for (int i=0; i < currentShiftTimesAvailable.Count; i++)
                {
                    totalSeconds -= secondsToShiftEnd;
                }
            }

            return totalSeconds;
        }
        protected virtual int CalcTotalClosedSeconds(List<OEEReportProductionDataAbstract> productionData)
        {
            return (int)productionData.Where(x => x.ReasonType != null && x.ReasonType.EntryType == EnumEntryType.TimeClosed).DefaultIfEmpty().Sum(x => x != null ? x.UsedTime : 0);
        }
    }
}