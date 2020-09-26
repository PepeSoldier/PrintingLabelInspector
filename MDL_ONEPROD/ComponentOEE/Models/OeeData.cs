using MDL_ONEPROD.ComponentOEE.Repos;
using MDL_ONEPROD.Model.OEEProd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.ComponentOEE.Models
{
    public class OeeDataOfDateRange
    {
        public OeeDataOfDateRange() { }
        public OeeDataOfDateRange(OEEReportProductionDataRepo repo, DateTime dateFrom, DateTime dateTo, int machineId, int labourBrigadeId, int intervalInHours)
        {
            OeeDataList = new List<OeeDataOfDate>();

            for (DateTime date = dateFrom; date < dateTo; date = date.AddHours(intervalInHours))
            {
                List<OEEReportProductionDataAbstract> productionData = repo
                    .GetDataByTimeRangeAndMachineId(date, date.AddHours(intervalInHours), machineId, labourBrigadeId)
                    .ToList<OEEReportProductionDataAbstract>();

                if (productionData.Count > 0)
                {
                    OeeCalculationModel oeeCalcModel = new OeeCalculationModel(productionData);
                    oeeCalcModel.CalculateOEE();

                    OeeDataList.Add(new OeeDataOfDate
                    {
                        Date = date,
                        MachineId = machineId,
                        OeeCalculatedData = oeeCalcModel
                    });
                }
            }

            CalcResults();
        }
        public void CalcResults()
        {
            decimal count = 0;

            ShiftTimeTotal = 0;
            GoodProductionTimeTotal = 0;
            ScrapMaterialTimeTotal = 0;
            ScrapProcessTimeTotal = 0;
            TotalCountTotal = 0;
            GoodCountTotal = 0;
            //StopUnplannedBreakdownsTimeTotal = 0;
            //StopPlannedChangeOverTimeTotal = 0;
            StopUnplannedTimeTotal = 0;
            StopPlannedTimeTotal = 0;
            StopPerformanceTotal = 0;
            AvailabilityAverage = 0;
            PerformanceAverage = 0;
            QualityAverage = 0;
            ResultAverage = 0;

            foreach (OeeDataOfDate oeeData in OeeDataList)
            {
                ShiftTimeTotal += oeeData.OeeCalculatedData.ShiftTime;
                GoodProductionTimeTotal += oeeData.OeeCalculatedData.GoodProductionTime;
                //ScrapMaterialTimeTotal += oeeData.OeeCalculatedData.ScrapMaterialTime;
                ScrapProcessTimeTotal += oeeData.OeeCalculatedData.ScrapProcessTime;
                TotalCountTotal += oeeData.OeeCalculatedData.TotalCount;
                GoodCountTotal += oeeData.OeeCalculatedData.GoodCount;
                StopPlannedTimeTotal += oeeData.OeeCalculatedData.StopPlannedTime;
                //StopPlannedChangeOverTimeTotal += oeeData.OeeCalculatedData.StopPlannedChangeOverTime;
                //StopUnplannedBreakdownsTimeTotal += oeeData.OeeCalculatedData.StopUnplannedBreakdownsTime;
                StopUnplannedTimeTotal += oeeData.OeeCalculatedData.StopUnplannedTime;
                //StopUnplannedChangeOverTimeTotal += oeeData.OeeCalculatedData.StopUnplannedChangeOverTime;
                StopPerformanceTotal += oeeData.OeeCalculatedData.StopPerformanceTime;
                AvailabilityAverage += oeeData.OeeCalculatedData.Availability;
                PerformanceAverage += oeeData.OeeCalculatedData.Performance;
                QualityAverage += oeeData.OeeCalculatedData.Quality;
                ResultAverage += oeeData.OeeCalculatedData.Result;
                count++;
            }
            if (count != 0)
            {
                AvailabilityAverage = AvailabilityAverage / count;
                PerformanceAverage = PerformanceAverage / count;
                QualityAverage = QualityAverage / count;
                ResultAverage = ResultAverage / count;
            }
        }

        public List<OeeDataOfDate> OeeDataList { get; set; }
        
        public decimal ShiftTimeTotal { get; set; }
        //public decimal OperationalTimeTotal { get { return ShiftTimeTotal - StopPlannedTimeTotal - StopPlannedChangeOverTimeTotal; } }
        public decimal OperationalTimeTotal { get { return ShiftTimeTotal - StopPlannedTimeTotal; } }
        public decimal GoodProductionTimeTotal { get; set; }
        public decimal ScrapMaterialTimeTotal { get; set; }
        public decimal ScrapProcessTimeTotal { get; set; }

        public decimal TotalCountTotal { get; set; }
        public decimal GoodCountTotal { get; set; }
        public decimal ScrapCountTotal { get { return TotalCountTotal - GoodCountTotal; } }

        public decimal StopPlannedTimeTotal { get; set; }
        //public decimal StopPlannedChangeOverTimeTotal { get; set; }

        public decimal StopUnplannedTimeTotal { get; set; }
        //public decimal StopUnplannedBreakdownsTimeTotal { get; set; }
        public decimal StopPerformanceTotal { get; set; }       
        //public decimal StopUnplannedChangeOverTimeTotal { get; set; }

        public decimal AvailabilityAverage { get; set; }
        public decimal PerformanceAverage { get; set; }
        public decimal QualityAverage { get; set; }
        public decimal ResultAverage { get; set; }
    }

    public class OeeDataOfDate
    {
        public DateTime Date { get; set; }
        public string DateString { get { return Date.ToString(); } }
        public int MachineId { get; set; }
        public OeeCalculationModel OeeCalculatedData { get; set; }
    }
}