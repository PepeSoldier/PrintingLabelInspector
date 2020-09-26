using _MPPL_WEB_START.Areas._APPWEB.Models;
using _MPPL_WEB_START.Areas._APPWEB.ViewModels;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Model.Scheduling.Interface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.ONEPROD.Models
{
    public class ChartOfReasonTypesDataPreparer : ChartByEntryTypetDataPreparer
    {
        public ChartOfReasonTypesDataPreparer(DateTime dateFrom, DateTime dateTo, int intervalInHours, List<DateTime> openShifts, int shiftTarget) 
            : base(dateFrom, dateTo, intervalInHours, openShifts, shiftTarget)
        {
            this.openShifts = openShifts;
            this.ShiftTarget = shiftTarget / 60;
        }

        public override ChartViewModel PrepareChartData(List<OEEReportProductionDataAbstract> prodDataList, string title, int languageId = 1)
        {
            this.prodDataList = prodDataList;
            this.languageId = languageId;
            List<int?> reasonTypeIds = prodDataList
                .Where(x => x.ReasonType.EntryType >= EnumEntryType.StopPlanned)
                .Select(x => x.ReasonTypeId).Distinct().ToList();

            ChartViewModel vm = new ChartViewModel();
            vm.labels = PrepareLabels();
            vm.title = title;
            vm.datasets = new List<ChartDataSetViewModel>();
            vm.datasets.Add(PrepareTotalDataSet());
            vm.datasets.Add(PrepareTargetDataSet(null));
            //vm.datasets.Add(PrepareCommentsDataSet());

            foreach (int? reasonTypeId in reasonTypeIds)
            {
                List<OEEReportProductionDataAbstract> dataListOfReasons = prodDataList.Where(x => x.ReasonTypeId == reasonTypeId).ToList();
                ChartDataSetViewModel dataSet = PrepareReasonDataSet(dataListOfReasons);
                vm.datasets.Add(dataSet);
            }
            return vm;
        }
        public override ChartDataSetViewModel PrepareReasonDataSet(List<OEEReportProductionDataAbstract> dataListOfReasons)
        {
            ReasonType reasonFirst = dataListOfReasons.FirstOrDefault().ReasonType;
            Color colorSolid = ColorHelper.MapColorFromString(reasonFirst != null? reasonFirst.Color : null); 
            Color colorTransparent = ColorHelper.ColorChangeTransparency(colorSolid, 0.6);

            List<decimal> dataOfReason = new List<decimal>();
            List<string> backgroundColorOfReason = new List<string>();
            List<string> borderColorOfReason = new List<string>() { ColorTranslator.ToHtml(colorSolid) };
            List<string> commentOfReason = new List<string>();

            int previousValue = 0;
            for (DateTime date = dateFrom; date < dateTo; date = date.AddHours(intervalInHours))
            {
                int sumInSec = CalculateReasonSumOfInterval(dataListOfReasons, date, previousValue);
                previousValue = sumInSec;
                dataOfReason.Add(sumInSec / 60);
                backgroundColorOfReason.Add(ColorHelper.ConvertToHtmlRGBA(colorTransparent));
                commentOfReason.Add(JoinCommentSumOfInterval(dataListOfReasons, date, intervalInHours));
                //borderColorOfReason.Add(ColorTranslator.ToHtml(colorSolid));
            }
            ChartDataSetViewModel dataSet = new ChartDataSetViewModel();
            dataSet.label = reasonFirst != null? reasonFirst.GetName(languageId) ?? "?" : "?";
            dataSet.backgroundColor = backgroundColorOfReason;
            dataSet.borderColor = borderColorOfReason;
            dataSet.datalabels = "LabelsBar";
            dataSet.data = dataOfReason;
            dataSet.borderWidth = 1;
            dataSet.stack = "stack 0";
            dataSet.comments = commentOfReason;

            return dataSet;
        }
        
    }
}