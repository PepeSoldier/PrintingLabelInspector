using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using MDL_BASE.Interfaces;
using MDL_ONEPROD.Common;
using MDLX_MASTERDATA.Entities;

namespace MDL_ONEPROD.Model.Scheduling
{
    [Table("CORE_Resource", Schema = "ONEPROD")]
    public class ResourceOP : Resource2
    {
        //fields from ResourceGroup
        public int StageNo { get; set; }        //parameter for scheduling
        public bool IsBottleneck { get; set; }  //parameter for scheduling
        public int SafetyTime { get; set; }     //parameter for scheduling
        public bool ShowBatches { get; set; }   //parameter for GantChart
        public int ProdStartDay { get; set; }   //parameter for OEE

        [ForeignKey("ResourceGroupId")]
        public virtual ResourceOP ResourceGroupOP { get; set; }
        //public new int? ResourceGroupId { get; set; }

        [NotMapped]
        public bool Consider { get; set; }
        [NotMapped]
        public bool Forward { get; set; }
        //*fields from ResourceGroup

        //[DataType("decimal(18,2")]
        //public decimal OEE { get; set; }

        public bool IsOEE { get; set; }

        public decimal TargetOee { get; set; }
        public decimal TargetAvailability { get; set; }
        public decimal TargetPerformance { get; set; }
        public decimal TargetQuality { get; set; }
        //Przeniesione do nowej tabeli
        //public int TargetInSec_StopPlanned { get; set; }
        //public int TargetInSec_StopPlannedChangeOver { get; set; }
        //public int TargetInSec_StopUnplanned { get; set; }
        //public int TargetInSec_StopUnplannedBreakdown { get; set; }
        //public int TargetInSec_StopUnplannedPreformance { get; set; }
        //public int TargetInSec_StopUnplannedChangeOver { get; set; }

        public bool ToolRequired { get; set; }

        //[MaxLength()]
        public string Breaks { get; set; }
        
        
        public override string ToString()
        {
            return Name;
        }

        [NotMapped]
        public DateTime Load { get; set; }
        [NotMapped]
        public int EmptySeconds { get; set; }
        [NotMapped]
        public List<Workorder> Workorders { get; set; }
        [NotMapped]
        public List<WorkorderBatch> Batches { get; set; }

        [NotMapped]
        public decimal OeeResult { get; set; }
    }
}
