using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.ComponentOEE.Models
{
    public class MachineTargets
    {
        public decimal OeeTarget { get; set; }
        public decimal AvailabilityTarget { get; set; }
        public decimal PerformanceTarget { get; set; }
        public decimal QualityTarget { get; set; }
    }
}