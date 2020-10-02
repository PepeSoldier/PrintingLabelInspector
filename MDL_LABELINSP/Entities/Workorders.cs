using MDL_BASE.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MDL_LABELINSP.Entities
{
    [Table("LABELINSP_Workorders")]
    public class Workorders : IModelEntity
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string WorkorderNumber { get; set; }
        [MaxLength(50)]
        public string ItemCode { get; set; }

        public decimal Qty { get; set; }
        [MaxLength(50)]
        public string SerialNumberFrom { get; set; }
        [MaxLength(50)]
        public string SerialNumberTo { get; set; }

        public DateTime FirstInspectionDate { get; set; }

        public DateTime LastInspectionDate { get; set; }

        public int SuccessfullInspections { get; set; }
        public int FailfullInspections { get; set; }
        public string FailInspectionLabelPath { get; set; }

    }
}