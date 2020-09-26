using MDL_ONEPROD.ComponentCORE.ViewModels;
using MDL_ONEPROD.Model.OEEProd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.ONEPROD.OEE.ViewModels
{
    public class ReasonViewModel
    {
        public ReasonViewModel()
        { 
        }
        public IEnumerable<Reason> StopPlanned { get; set; }
        public IEnumerable<Reason> StopChangeOver { get; set; }
        public IEnumerable<Reason> StopUnplanned { get; set; }
        public IEnumerable<Reason> StopBreakdown { get; set; }
        public IEnumerable<Reason> StopPreformance { get; set; }
    }
}