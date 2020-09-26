using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.ONEPROD.Base.ViewModels
{
    public class ToolMachineViewModel
    {
        public List<ResourceOP> Machines { get; set; }
        public List<Tool> Tools { get; set; }
        public List<ToolMachine> ToolMachines { get; set; }

        public int ToolId { get; set; }
        public int MachineId { get; set; }
        public bool Assigned { get; set; }
        public bool Preffered { get; set; }
        public bool Placed { get; set; }
    }
}