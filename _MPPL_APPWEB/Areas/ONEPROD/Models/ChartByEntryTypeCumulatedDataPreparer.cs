using _MPPL_WEB_START.Areas._APPWEB.ViewModels;
using MDL_ONEPROD.Model.OEEProd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.ONEPROD.Models
{
    public class ChartByEntryTypeCumulatedDataPreparer : ChartByEntryTypetDataPreparer
    {
        public ChartByEntryTypeCumulatedDataPreparer(DateTime dateFrom, DateTime dateTo, int intervalInHours, List<DateTime> openShifts, int shiftTarget)
            : base(dateFrom, dateTo, intervalInHours, openShifts, shiftTarget)
        {
        }

        public override ChartViewModel PrepareChartData(List<OEEReportProductionDataAbstract> prodDataList, string title, int languageId = 1)
        {
            this.prodDataList = prodDataList;
            this.languageId = languageId;

            List<int?> reasonIds = prodDataList.Select(x => x.ReasonId).Distinct().ToList();

            ChartViewModel vm = new ChartViewModel();
            vm.labels = PrepareLabels();
            vm.title = title;
            vm.datasets = new List<ChartDataSetViewModel>();
            vm.datasets.Add(PrepareTotalDataSet());
            vm.datasets.Add(PrepareTargetDataSet());

            foreach (int? reasonId in reasonIds)
            {
                List<OEEReportProductionDataAbstract> dataListOfReasons = prodDataList.Where(x => x.ReasonId == reasonId).ToList();
                ChartDataSetViewModel dataSet = PrepareReasonDataSet(dataListOfReasons);
                vm.datasets.Add(dataSet);
            }
            return vm;
        }

        public override int CalculateReasonSumOfInterval(List<OEEReportProductionDataAbstract> reasonList, DateTime date, int previousValue)
        {
            return previousValue + (int)reasonList.Where(x => x.ProductionDate >= date && x.ProductionDate < date.AddHours(intervalInHours)).Sum(x => x.UsedTime);
        }
        public override int CalculateTargetInSecOfInterval(int previousValue, DateTime date)
        {
            return CalculateTargetInSecOfInterval(date) + previousValue;
        }
    }
}