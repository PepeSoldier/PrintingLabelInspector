using MDLX_CORE.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MDL_LABELINSP.Entities
{
    [Table("Workorder", Schema = "LABELINSP")]
    public class Workorder : IModelEntity
    {
        public Workorder()
        {
            FirstInspectionDate = new DateTime(1900, 1, 1);
            LastInspectionDate = new DateTime(1900, 1, 1);
        }

        public int Id { get; set; }
        [MaxLength(50)]
        public string WorkorderNumber { get; set; }
        [MaxLength(50)]
        public string ItemCode { get; set; }
        [MaxLength(50)]
        public string ItemName { get; set; }

        public decimal Qty { get; set; }
        [MaxLength(50)]
        public string SerialNumberFrom{ get; set; }
        [MaxLength(50)]
        public string SerialNumberTo { get; set; }

        public int SerialNumberFromInt { get; set; }
        public int SerialNumberToInt { get; set; }

        public DateTime FirstInspectionDate { get; set; }
        public DateTime LastInspectionDate { get; set; }

        public int SuccessfullInspections { get; set; }
        public int FailfullInspections { get; set; }
        public string FailInspectionLabelPath { get; set; }

        public override string ToString()
        {
            return $"{Id}. {WorkorderNumber}. {FirstInspectionDate.ToString()}. {LastInspectionDate.ToString()}";
        }
    }
}