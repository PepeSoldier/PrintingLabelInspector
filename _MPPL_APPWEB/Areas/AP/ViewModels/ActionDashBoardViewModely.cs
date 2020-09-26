using MDL_BASE.Models.IDENTITY;
using MDL_AP.Models.Reports;
using System.Collections.Generic;


namespace _MPPL_WEB_START.Areas.AP.ViewModel
{
    public class ActionDashBoardViewModel
    {
        public string DashBoardTitleAll { get; set; }
        public string DashBoardTitleDepartment { get; set; }
        public string DashBoardTitleCreator { get; set; }

        public int TotalActionsAll { get; set; }
        public int TotalActionsDepartment { get; set; }
        public int TotalActionsCreator { get; set; }

        public List<string> ActionsStatusAll { get; set; }
        public List<int> ActionsCountAll { get; set; }

        public List<string> ActionsStatusDepartment { get; set; }
        public List<int> ActionsCountDepartment { get; set; }

        public List<string> ActionsStatusCreator { get; set; }
        public List<int> ActionsCountCreator { get; set; }

        public List<MainReport> ChartDataAll { get; set; }
        public List<MainReport> ChartDataDepartment { get; set; }
        public List<MainReport> ChartDataAssigned { get; set; }
        public List<MainReport> ChartDataCreator { get; set; }
    }

    public class ActionDashBoardViewModel2
    {
        public string DashBoardTitleAll { get; set; }
        
        public int TotalActionsAll { get; set; }
        
        public List<string> ActionsStatusAll { get; set; }
        public List<int> ActionsCountAll { get; set; }

        public ChartData ChartDataAll { get; set; }

        public string ChartDivId { get; set; }
    }

    public class ChartData
    {
        public List<MainReport> data { get; set; }
        public string chartType { get; set; }
        public User user { get; set; }
        public string dateFrom { get; set; }
    }

    public class ActionDashBoardFilterViewModel
    {
        public int TimeFilter { get; set; }
        public int PersonalFilter { get; set; }
        public string ChartTitle { get; set; }

        public int LogCurrentRow { get; set; }
        public int LogLoadRows { get; set; }
    }

}