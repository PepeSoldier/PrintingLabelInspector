using MDL_ONEPROD.ComponentOEE.Models;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Model.Scheduling.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.ComponentRTV.Models
{
    public class RTVOeeCalculationModel : OeeCalculationModel
    {
        public RTVOeeCalculationModel(List<OEEReportProductionDataAbstract> productionData, DateTime dateFrom, DateTime dateTo)
        {
            prodDataAnalyzer = new ProductionAnalyzeModel();
            this.productionData = productionData;

            totalAvailavleSeconds = CalcTotalAvailableSeconds(dateFrom, dateTo);
            totalAvailavleSeconds -= CalcTotalClosedSeconds(productionData);
        }

        protected int CalcTotalAvailableSeconds(DateTime dateFrom, DateTime dateTo)
        {
            return (int)(dateTo - dateFrom).TotalSeconds;
        }

    }
}