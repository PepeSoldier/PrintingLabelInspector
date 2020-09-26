using MDL_ONEPROD.ComponentOEE.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.ComponentMes.ViewModels
{
    public class FilterProductionLogViewModel : FilterOeeViewModel
    {
        public int? MachineId { get; set; }
        public List<int> MachineIds { get; set; }

        public string SerialNumber { get; set; }
        public string ItemCode { get; set; }
        public string WorkOrder { get; set; }

        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string SelectedDate { get; set; }
        public string SelectedShift { get; set; }
    }
}