using MDL_ONEPROD.Model.Scheduling.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.ComponentOEE.ViewModels
{
    public class CreateReportProdDataDetailViewModel
    {
        public int? DetailId { get; set; }

        public string Comment { get; set; }

        public decimal ProductionCycleTime { get; set; }
        public decimal FormWeightProcess { get; set; }
        public decimal FormWeightScrap { get; set; }
        public decimal PaperWeight { get; set; }
        public decimal TubeWeight { get; set; }

        public int CoilId { get; set; }
    }
}