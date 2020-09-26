using MDL_BASE.Enums;
using MDL_BASE.Interfaces;
using MDLX_MASTERDATA.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XLIB_COMMON.Enums;

namespace MDL_BASE.Models.MasterData
{
    [Table("BOM_Workorder", Schema = "CORE")]
    public class BomWorkorder : IModelEntity
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(20)]
        public string OrderNo { get; set; }
        
        public virtual Item Parent { get; set; }
        public int ParentId { get; set; }
        public virtual Item Child { get; set; }
        public int ChildId { get; set; }

        [MaxLength(3)]
        public string BC1 { get; set; }
        public int LV { get; set; }
        public decimal QtyUsed { get; set; }

        [MaxLength(3)]
        public string BC2 { get; set; }
        [MaxLength(3)]
        public string DEF { get; set; }
        [MaxLength(12)]
        public string Prefix { get; set; }
        [MaxLength(12)]
        public string Suffix { get; set; }
        [MaxLength(12)]
        public string IDCO { get; set; }

        [MaxLength(20)]
        public string DirPar { get; set; }
        //public int UM { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }
        public string InterItm { get; set; }

        public DateTime InsertDate { get; set; }
    }
}