using MDL_BASE.Interfaces;
using MDL_ONEPROD.ComponentMes.Enums;
using MDL_ONEPROD.Model.Scheduling;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XLIB_COMMON.Enums;

namespace MDL_ONEPROD.ComponentQuality.Entities
{
    [Table("QUALITY_ItemMeasurement", Schema = "ONEPROD")]
    public class ItemMeasurement : IModelEntity, IModelDeletableEntity
    {
        public int Id { get; set; }

        public virtual ItemOP ItemOP { get; set; }
        public int ItemOPId { get; set; }

        public string SerialNumber { get; set; }
        public int Counter { get; set; }

        public decimal Result0 { get; set; }
        public decimal Result1 { get; set; }
        public decimal Result2 { get; set; }
        public decimal Result3 { get; set; }
        public decimal Result4 { get; set; }
        public decimal Result5 { get; set; }
        public decimal Result6 { get; set; }
        public decimal Result7 { get; set; }
        public decimal Result8 { get; set; }
        public decimal Result9 { get; set; }

        public bool Deleted { get; set; }
    }
}