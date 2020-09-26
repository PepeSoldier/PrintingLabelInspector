using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.ONEPROD.APS.ViewModels
{
    public class PlanViewModel
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public int SelectedMachineId { get; set; }
        public ResourceOP SelectedMachine { get; set; }

        public List<Workorder> Tasks { get; set; }
        public IEnumerable<SelectListItem> Machines { get; set; }      
    }

    public class PlanMonitorViewModel : PlanViewModel
    {

    }

    public class PlanMonitorAreaViewModel
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public int MachineCount { get; set; }
        public int SelectedAreaId { get; set; }
        public ResourceOP SelectedArea { get; set; }

        public List<Workorder> Tasks { get; set; }
        public List<ResourceOP> MachinesList { get; set; }
        public IEnumerable<SelectListItem> Areas { get; set; }
    }

}