using _MPPL_WEB_START.Areas._APPWEB.ViewModels;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.Model.Scheduling.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XLIB_COMMON.Enums;

namespace _MPPL_WEB_START.Areas.ONEPROD.OEE.ViewModels
{
    public class BrowseReportsViewModel
    {
        //Filters
        public DateTime SelectedDate { get; set; }
        public Shift SelectedShift { get; set; }
        public int MachineId { get; set; }
        public string MachineName { get; set; }
        public List<SelectListItem> Shifts { get; set; }
        public IEnumerable<ResourceOP> Machines { get; set; }
        //Olg Grid Data
        //public List<OEEReportProductionData> ProductionData { get; set; }

        //Grid Data
        public List<BrowseOeeGridViewModel> BrowseOee { get; set; }

        //Right side data
        public List<OeePartData> OeePartData { get; set; }
        public decimal AvailabilityResult { get; set; }
        public decimal QualityResult { get; set; }
        public decimal PerformanceResult { get; set; }
        public decimal OeeResult { get; set; }
    }

    public class OeePartData
    {
        public string Name { get; set; }
        public decimal TimeInMin { get; set; }
        public decimal Qty { get; set; }
        public string CssClass { get; set; }
        public string Color { get; set; }
    }

    public class BrowseOeeGridViewModel
    {
        public int Id { get; set; }
        public int ReportId { get; set; }

        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ReasonName { get; set; }
        //public EnumEntryType EntryType { get; set; }
        public string ReasonTypeName {get;set;}
        public int? ReasonTypeId {get;set;}
        public EnumEntryType ReasonTypeEntryType { get; set; }

        public decimal UsedTime { get; set; }
        public decimal CycleTime { get; set; }
        public decimal ProdQty { get; set;}
        //public DateTime EntryTimeStampDT { get; set; }
        public string EntryTimeStamp { get; set; } //{ get { return EntryTimeStampDT.ToShortDateString(); } }

        public string UserName { get; set; }
        public decimal ProductionCycleTime { get; set; }
        public string Shift { get; set; }
        public string ProdDateFormatted { get; set; }
    }

}