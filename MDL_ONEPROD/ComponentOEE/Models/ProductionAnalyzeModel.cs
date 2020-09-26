using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Model.Scheduling.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.ComponentOEE.Models
{
    public class ProductionAnalyzeModel
    {
        //public decimal ProductionQtyToTime(List<OEEReportProductionDataAbstract> productionData)
        //{
        //    decimal numberOfSeconds = 0;
        //    productionData.ForEach(x => { numberOfSeconds+= x.ProdQty * x.CycleTime; });

        //    return numberOfSeconds;
        //}
        public decimal ProductionQtyToTime(List<OEEReportProductionDataAbstract> productionData, ReasonType reasonType)
        {
            decimal totalSeconds = 0;

            foreach (OEEReportProductionDataAbstract x in productionData)
            {
                if (x.ReasonType != null && (x.ReasonTypeId == reasonType.Id))
                {
                    if (reasonType.EntryType == EnumEntryType.Production || reasonType.EntryType == EnumEntryType.ScrapProcess)
                    {
                        totalSeconds += x.ProdQty * x.CycleTime;
                    }
                    else
                    {
                        totalSeconds += x.UsedTime;
                    }
                }
            }
            return totalSeconds;
        }
        public decimal ProductionTotalTime(List<OEEReportProductionDataAbstract> productionData)
        {
            decimal totalSeconds = 0;

            foreach (OEEReportProductionDataAbstract x in productionData)
            {
                if (x.ReasonType != null && 
                    x.ReasonType.EntryType != EnumEntryType.TimeAvailable &&
                    x.ReasonType.EntryType != EnumEntryType.TimeClosed
                    )
                {
                    totalSeconds += x.UsedTime;
                }
            }

            return totalSeconds;
        }
        public decimal ProductionQtyToTime(List<OEEReportProductionDataAbstract> productionData, EnumEntryType entryType)
        {
            decimal totalSeconds = 0;

            foreach (OEEReportProductionDataAbstract x in productionData)
            {
                if (x.ReasonType != null && 
                    (x.ReasonType.EntryType == entryType || 
                     (entryType == EnumEntryType.ScrapProcess && x.ReasonType.EntryType == EnumEntryType.ScrapProcess)
                    )
                   ) //&& isEntryTypeOfScrapProcessType(x.ReasonType.EntryType)) )
                {
                    //if (entryType == EnumEntryType.Production || entryType == EnumEntryType.ScrapMaterial || (isEntryTypeOfScrapProcessType(entryType)) )
                    if (entryType == EnumEntryType.Production || entryType == EnumEntryType.ScrapProcess) // || (isEntryTypeOfScrapProcessType(entryType)) )
                    {
                        totalSeconds += x.ProdQty * x.CycleTime;
                    }
                    else
                    {
                        totalSeconds += x.UsedTime;
                    }
                }
            }

            return totalSeconds;
        }
        public decimal GoodQtyCount(List<OEEReportProductionDataAbstract> productionData)
        {
            return productionData.Where(x => x.ReasonType != null && x.ReasonType.EntryType == EnumEntryType.Production).Select(x => x.ProdQty).Sum();
        }
        public decimal ScrapProcessQtyCount(List<OEEReportProductionDataAbstract> productionData)
        {
            return productionData.Where(x =>
                    x.ReasonType != null && 
                    x.ReasonType.EntryType == EnumEntryType.ScrapProcess)
                .DefaultIfEmpty().Sum(x => x != null? x.ProdQty : 0);
        }
        public decimal ScrapMaterialQtyCount(List<OEEReportProductionDataAbstract> productionData)
        {
            return productionData.Where(x => 
                    x.ReasonType != null &&
                    x.ReasonType.EntryType == EnumEntryType.ScrapMaterial)
                .DefaultIfEmpty().Sum(x => x != null? x.ProdQty : 0);
        }
        public decimal TotalQtyCount(List<OEEReportProductionDataAbstract> productionData)
        {
            return productionData.Select(x => x.ProdQty).Sum();
        }

        //private bool isEntryTypeOfScrapProcessType(EnumEntryType entryType)
        //{
        //    if( entryType == EnumEntryType.ScrapProcess //||
        //        //entryType == EnumEntryType.ScrapProcessCrack ||
        //        //entryType == EnumEntryType.ScrapProcessDent ||
        //        //entryType == EnumEntryType.ScrapProcessFold ||
        //        //entryType == EnumEntryType.ScrapProcessScratch 
        //        )
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
    }
}