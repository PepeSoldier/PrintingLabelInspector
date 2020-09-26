using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.ONEPROD.Base.ViewModels
{
    public class CycleTimeViewModel
    {
        public IEnumerable<SelectListItem> Machines { get; set; }
        public ResourceOP CurrentMachine { get; set; }
        public List<MCycleTime> CycleTimes { get; set; }
        public int SelectedMachineId { get; set; }

        public IEnumerable<SelectListItem> ItemGroups { get; set; }
        public MCycleTime NewObject { get; set; }
        public int SelectedItemGroupId { get; set; }
        public string SelectedItemGroupName { get; set; }
        public int? areaId { get; set; }

        public int ProgramNumber { get; set; }
        public string ProgramName { get; set; }
 
    }
}