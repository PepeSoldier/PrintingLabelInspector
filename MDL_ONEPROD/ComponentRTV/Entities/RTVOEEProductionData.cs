using MDL_BASE.Interfaces;
using MDL_BASE.Models.IDENTITY;
using MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.Model.Scheduling.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using XLIB_COMMON.Enums;

namespace MDL_ONEPROD.Model.OEEProd
{
    [Table("RTV_RTVOEEReportProductionData", Schema = "ONEPROD")]
    public class RTVOEEProductionData : OEEReportProductionDataAbstract, IModelDeletableEntity
    {
        public RTVOEEProductionData()
        {
        }

        [Index]
        public int MachineId { get; set; }

        public int ProdQtyTotal { get; set; }
        public int ProdQtyShift { get; set; }
        public int ProdQtyCorrector { get; set; }
        public int PiecesPerPallet { get; set; }
        
        //[NotMapped]
        //public virtual new OEEReportProductionDataDetails Detail { get; set; }
        //[NotMapped]
        //public new int? DetailId { get; set; }

    }
}
