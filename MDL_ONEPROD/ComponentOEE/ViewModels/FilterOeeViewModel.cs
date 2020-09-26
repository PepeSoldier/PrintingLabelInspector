using MDL_BASE.ComponentBase.Entities;
using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace MDL_ONEPROD.ComponentOEE.ViewModels
{
    public class FilterOeeViewModel
    {
        public List<ResourceOP> MachineList { get; set; }
        public IEnumerable<object> LabourBrigades { get; set; }
        //public SelectList LabourBrigades { get; set; }

    }
}