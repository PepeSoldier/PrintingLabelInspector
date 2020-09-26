using _MPPL_WEB_START.Areas._APPWEB.Models;
using _MPPL_WEB_START.Areas._APPWEB.ViewModels;
using MDL_ONEPROD.Model.OEEProd;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.ONEPROD.Models
{
    public class ChartByEntryTypetDataPreparer : ChartDataPreparer
    {
        protected List<OEEReportProductionDataAbstract> prodDataList;
        protected List<DateTime> openShifts;
        protected int languageId { get; set; }
        protected int ShiftTarget { get; set; }

        public ChartByEntryTypetDataPreparer(DateTime dateFrom, DateTime dateTo, int intervalInHours, List<DateTime> openShifts, int shiftTarget) : base(dateFrom, dateTo, intervalInHours)
        {
            this.openShifts = openShifts;
            this.ShiftTarget = shiftTarget / 60;
        }

        public virtual ChartViewModel PrepareChartData(List<OEEReportProductionDataAbstract> prodDataList, string title, int languageId = 1)
        {
            this.prodDataList = prodDataList;
            this.languageId = languageId;
            List<int?> reasonIds = prodDataList.Select(x => x.ReasonId).Distinct().ToList();

            ChartViewModel vm = new ChartViewModel();
            vm.labels = PrepareLabels();
            vm.title = title;
            vm.datasets = new List<ChartDataSetViewModel>();
            vm.datasets.Add(PrepareTotalDataSet());
            vm.datasets.Add(PrepareTargetDataSet(null));
            vm.datasets.Add(PrepareCommentsDataSet());

            foreach (int? reasonId in reasonIds)
            {
                List<OEEReportProductionDataAbstract> dataListOfReasons = prodDataList.Where(x => x.ReasonId == reasonId).ToList();
                ChartDataSetViewModel dataSet = PrepareReasonDataSet(dataListOfReasons);
                vm.datasets.Add(dataSet);
            }
            return vm;
        }
        public virtual ChartDataSetViewModel PrepareReasonDataSet(List<OEEReportProductionDataAbstract> dataListOfReasons)
        {
            Reason reasonFirst = dataListOfReasons.FirstOrDefault().Reason;
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
        public ChartDataSetViewModel PrepareTargetDataSet(string yAxisId = null)
        {
            ChartDataSetViewModel target = new ChartDataSetViewModel()
            {
                label = "Cel",
                type = "line",
                backgroundColor = new List<string>(),
                borderColor = new List<string>() { "red" },
                data = new List<decimal>(),
                borderWidth = 1,
                lineTension = 0,
                yAxisID = yAxisId,
                pointBackgroundColor = "red",
                datalabels = "LabelsNone",
            };
            int previousValue = 0;
            for (DateTime date = dateFrom; date < dateTo; date = date.AddHours(intervalInHours))
            {
                int sumInSec = CalculateTargetInSecOfInterval(previousValue, date);
                previousValue = sumInSec;
                target.data.Add(sumInSec);
                target.backgroundColor.Add("rgba(0,0,0,0)");
            }

            return target;
        }
        public ChartDataSetViewModel PrepareTotalDataSet()
        {
            ChartDataSetViewModel total = new ChartDataSetViewModel()
            {
                label = "Total",
                type = "line",
                backgroundColor = new List<string>(),
                borderColor = new List<string>() { "rgba(0,0,0,0)" },
                data = new List<decimal>(),
                borderWidth = 1,
                lineTension = 0,
                fontSize = 11,
                fontColor = "black",
                //yAxisID = "lineAxes",
                pointBackgroundColor = "rgba(205,205,205,0.5)",
                datalabels = "LabelsLineRound",
            };

            int previousValue = 0;
            for (DateTime date = dateFrom; date < dateTo; date = date.AddHours(intervalInHours))
            {
                int sumInSec = CalculateReasonSumOfInterval(prodDataList, date, previousValue);
                previousValue = sumInSec;
                total.data.Add(sumInSec/60);
                total.backgroundColor.Add("rgba(205,205,205,0.5)");
            }

            return total;
        }
        public ChartDataSetViewModel PrepareCommentsDataSet()
        {
            ChartDataSetViewModel total = new ChartDataSetViewModel()
            {
                label = "Komentarze",
                type = "line",
                backgroundColor = new List<string>(),
                borderColor = new List<string>() { "rgba(0,0,0,0)" },
                data = new List<decimal>(),
                comments = new List<string>(),
                borderWidth = 1,
                lineTension = 0,
                //yAxisID = "lineAxes",
                pointBackgroundColor = "blue",
                datalabels = "LabelsComments",

            };

            List<Reason> reasons = prodDataList.Select(x => x.Reason).Distinct().ToList();
            string commentOfDate = string.Empty;
            string commentReasonColor = string.Empty;

            for (DateTime date = dateFrom; date < dateTo; date = date.AddHours(intervalInHours))
            {
                commentOfDate = string.Empty;
                commentReasonColor = "";

                foreach (Reason reason in reasons)
                {
                    if (reason != null)
                    {
                        List<OEEReportProductionDataAbstract> dataListOfReasons = prodDataList.Where(x => x.ReasonId == reason.Id).ToList();
                        string c = JoinCommentSumOfInterval2(dataListOfReasons, date, intervalInHours);
                        if (c.Length > 1)
                        {
                            commentOfDate += c;
                            commentReasonColor += reason.Color + "\n";
                        }
                    }
                }
                total.comments.Add(commentOfDate);
                total.data.Add(commentOfDate.Length > 1? 0.01m : -1);
                total.backgroundColor.Add(commentReasonColor);
            }

            return total;
        }

        public virtual int CalculateTargetInSecOfInterval(int previousValue, DateTime date)
        {
            return CalculateTargetInSecOfInterval(date);
        }
        public virtual int CalculateReasonSumOfInterval(List<OEEReportProductionDataAbstract> dataListOfReason, DateTime date, int previousValue)
        {
            return (int)dataListOfReason.Where(x => x.ProductionDate >= date && x.ProductionDate < date.AddHours(intervalInHours)).Sum(x => x.UsedTime);
        }
        public string JoinCommentSumOfInterval(List<OEEReportProductionDataAbstract> reasonList, DateTime date, int intervalInHours)
        {
            string returnString = string.Empty;
            List<OEEReportProductionDataAbstract> listOfReasons =  reasonList.Where(x => x.ProductionDate >= date && x.ProductionDate < date.AddHours(intervalInHours)).ToList();
            foreach (var item in listOfReasons)
            {
                if(item.Detail != null && item.Detail.Comment != null)
                {
                    returnString += item.Detail.Comment + Environment.NewLine;
                }
            }
            return returnString;
        }
        public string JoinCommentSumOfInterval2(List<OEEReportProductionDataAbstract> reasonList, DateTime date, int intervalInHours)
        {
            string returnString = string.Empty;
            List<OEEReportProductionDataAbstract> listOfReasons = reasonList.Where(x => x.ProductionDate >= date && x.ProductionDate < date.AddHours(intervalInHours)).ToList();
            foreach (var item in listOfReasons)
            {
                if (item.Detail != null && item.Detail.Comment != null)
                {
                    returnString += item.Reason.Name +
                        "(" + (item.UsedTime / 60).ToString("0") + "min): " +
                        item.Detail.Comment +
                        "\n";
                }
            }
            return returnString;
        }
        public int CalculateTargetInSecOfInterval(DateTime date)
        {
            int numberOfOccurenceShifts;
            if (intervalInHours == 24)
            {
                numberOfOccurenceShifts = openShifts.Count(x => x.Date == date.Date);
            }
            else if (intervalInHours == 8)
            {
                numberOfOccurenceShifts = openShifts.Count(x => x == date);
            }
            else
            {
                numberOfOccurenceShifts = openShifts.Count(x => x >= date && x < date.AddHours(intervalInHours));
            }

            return numberOfOccurenceShifts * ShiftTarget;
        }
    }
}