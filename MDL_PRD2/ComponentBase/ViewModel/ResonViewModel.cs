using MDL_PRD.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_PRD.ViewModel
{
    public class ResonViewModel
    {
        public List<ReasonModel> Resons { get; set; }

        public List<int> FastResons { get; set; }

        public int SelectedReasonId { get; set; }
        public int SelectedOrderId { get; set; }
    }
}