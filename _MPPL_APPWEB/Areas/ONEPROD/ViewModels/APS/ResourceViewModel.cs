using MDL_ONEPROD.Model.Scheduling;
using MDLX_MASTERDATA.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.ONEPROD.ViewModels.APS
{
    public class ResourceViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ResourceTypeEnum Type { get; set; }

        public int? ResourceGroupId { get; set; }
        public int ResourceGroupStageNo { get; set; }
        public int ResourceGroupSafetyTime { get; set; }
        
        public bool ShowBatches { get; set; }   //parameter for GantChart
        public bool Forward { get; set; }
        public string Color { get; set; }
        public bool ToolRequired { get; set; }
        public List<WorkorderViewModel> Workorders { get; set; }
        public List<WorkorderBatchViewModel> Batches { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}