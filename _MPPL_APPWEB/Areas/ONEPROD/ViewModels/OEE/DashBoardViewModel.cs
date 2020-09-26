using MDL_ONEPROD.ComponentOEE.Models;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.ONEPROD.OEE.ViewModels
{
    public class DashBoardViewModel
    {
        public ResourceOP Area { get; set; }
        public List<ResourceOP> MachineList { get; set; }
        public List<ReasonType> ReasonTypes { get; set; }

        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }

    public class DashBoardMachineViewModel
    {
        public int MachineId { get; set; }
        public int NumberOfRecords { get; set; }
        public string MachineName { get; set; }
        public decimal MachineExpectedOEE { get; set; }

        public MachineTargets Targets { get; set;  }

        public decimal AvailabilityResult { get; set; }
        public decimal QualityResult { get; set; }
        public decimal PerformanceResult { get; set; }
        public decimal OeeResult { get; set; }

        public decimal ProductionTimeInMin { get; set; }
        public decimal StopsPlannedInMin { get; set; }
        //public decimal StopsUnplannedChangeoverInMin { get; set; }
        public decimal StopsPerformanceInMin { get; set; }
        public decimal StopsUnplannedInMin { get; set; }
        //public decimal StopsBreakdownInMin { get; set; }
        public decimal ScrapProcessQty { get; set; }
        public decimal ScrapMaterialQty { get; set; }
        public decimal ProducedGoodQty { get; set; }

        public int NRFT { get; set; }
        public int UndefunedStopsCount { get; set; }
    }
}