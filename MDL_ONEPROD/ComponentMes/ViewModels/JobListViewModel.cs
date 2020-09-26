
using MDL_ONEPROD.ComponentCORE.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.ComponentMes.ViewModels
{
    public class JobListViewModel
    {
        public string WorkorderNo { get; set; }
        public string WorkorderQtyTotal { get; set; }
        public string WorkorderQtyProduced { get; set; }
        public string ItemSerialNo { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemVersion { get; set; }
        public int ItemId { get; set; }
        public int WorkorderQtyPlanned { get; set; }
        public bool JobListDataFound { get; set; }

        public List<JobListItem> JobListItems { get; set; }
    }
}