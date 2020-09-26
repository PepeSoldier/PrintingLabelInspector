using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.ONEPROD.ViewModels.APS
{
    public class WorkorderBatchViewModel : WorkorderViewModel
    {
        public WorkorderBatchViewModel()
        {
            Workorders = new List<WorkorderViewModel>();
        }

        public List<WorkorderViewModel> Workorders { get; set; }
        public int Qty { get; set; }
        public int SetupTime { get; set; }
        public string Color { get; set; }

        public override string ToString()
        {
            return "BachNo: " + BatchNumber.ToString() + ", Count: " + Workorders.Count.ToString() + ", ResourceId: " + ResourceId;
        }
    }
}