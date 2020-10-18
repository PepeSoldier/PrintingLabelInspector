using MDLX_CORE.Interfaces;
using MDL_LABELINSP.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MDL_LABELINSP.Entities
{
    [Table("WorkorderLabelInspection", Schema = "LABELINSP")]
    public class WorkorderLabelInspection : IModelEntity
    {
        public int Id { get; set; }

        public WorkorderLabel WorkorderLabel { get; set; }
        public int WorkorderLabelId { get; set; }
        [MaxLength(50)]
        public string TestName { get; set; }

        public decimal ExpectedValue { get; set; }
        public decimal ActualValue { get; set; }
        public decimal Tolerance { get; set; }

        public string ExpectedValueText { get; set; }
        public string ActualValueText { get; set; }

        public EnumLabelType LabelType { get; set; }
        public DateTime TimeStamp { get; set; }

        public bool Result { get; set; }
    }
}