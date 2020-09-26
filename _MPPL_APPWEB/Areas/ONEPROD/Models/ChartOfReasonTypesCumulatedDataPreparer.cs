using _MPPL_WEB_START.Areas._APPWEB.ViewModels;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Model.Scheduling.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.ONEPROD.Models
{
    public class ChartOfReasonTypesCumulatedDataPreparer : ChartOfReasonTypesDataPreparer
    {
        public ChartOfReasonTypesCumulatedDataPreparer(DateTime dateFrom, DateTime dateTo, int intervalInHours, List<DateTime> openShifts, int shiftTarget)
            : base(dateFrom, dateTo, intervalInHours, openShifts, shiftTarget)
        {
        }

        public override ChartViewModel PrepareChartData(List<OEEReportProductionDataAbstract> prodDataList, string title, int languageId = 1)
        {
            this.prodDataList = prodDataList;
            this.languageId = languageId;

            List<int?> reasonTypeIds = prodDataList
                .Where(x=>x.ReasonType.EntryType >= EnumEntryType.StopPlanned)
                .Select(x => x.ReasonTypeId).Distinct().ToList();

            ChartViewModel vm = new ChartViewModel();
            vm.labels = PrepareLabels();
            vm.title = title;
            vm.datasets = new List<ChartDataSetViewModel>();
            vm.datasets.Add(PrepareTotalDataSet());
            vm.datasets.Add(PrepareTargetDataSet());

            foreach (int? reasonTypeId in reasonTypeIds)
            {
                List<OEEReportProductionDataAbstract> dataListOfReasonTypes = prodDataList.Where(x => x.ReasonTypeId == reasonTypeId).ToList();
                ChartDataSetViewModel dataSet = PrepareReasonDataSet(dataListOfReasonTypes);
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