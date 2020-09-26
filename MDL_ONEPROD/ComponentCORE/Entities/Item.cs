using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using MDL_BASE.Interfaces;
using MDLX_MASTERDATA.Entities;

namespace MDL_ONEPROD.Model.Scheduling
{
    [Table("CORE_Item", Schema = "ONEPROD")]
    public class ItemOP : Item
    { 
        public int MinBatch { get; set; }
        public bool WorkOrderGenerator { get; set; }

        [ForeignKey("ItemGroupId")]
        //public virtual new ItemOP ItemGroup { get;set; }
        public virtual ItemOP ItemGroupOP { get; set; }
        //public new int? ItemGroupId { get; set; }
        [ForeignKey("ResourceGroupId")]
        //public virtual new ResourceOP ResourceGroup { get; set; }
        public virtual ResourceOP ResourceGroupOP { get; set; }
        //public new int? ResourceGroupId { get; set; }

        [NotMapped]
        public int NM_PartCount { get; set; }
        [NotMapped]
        public int NM_ToolCount { get; set; }
        [NotMapped]
        public int NM_BoxCount { get; set; }
        [NotMapped]
        public int NM_CycleTimeCount { get; set; }
    }

}
