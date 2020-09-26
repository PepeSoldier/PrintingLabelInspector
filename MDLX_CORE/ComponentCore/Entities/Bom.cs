using MDL_BASE.Enums;
using MDL_BASE.Interfaces;
using MDLX_MASTERDATA.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XLIB_COMMON.Enums;

namespace MDL_BASE.Models.MasterData
{
    [Table("BOM", Schema = "CORE")]
    public class Bom : IModelEntity
    {
        [Key]
        public int Id { get; set; }
        
        public virtual Item Anc { get; set; }
        public int? AncId { get; set; }

        public virtual Item Pnc { get; set; }
        public int? PncId { get; set; }
        
        public int LV { get; set; }
        public decimal PCS { get; set; } //pieces - ilośc sztuk

        public UnitOfMeasure UnitOfMeasure { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        //additional parameters
        [MaxLength(3)]
        public string BC { get; set; } //BC będzie typem itema: produkt, surowiec itp
        [MaxLength(3)]
        public string DEF { get; set; } //magazynowy podział komponentów 
        [MaxLength(12)]
        public string Prefix { get; set; } //Prefix odpowiada grupie itemów
        [MaxLength(12)]
        public string Suffix { get; set; }
        [MaxLength(12)]
        public string IDCO { get; set; } //IDCO to grupa materiałowa

        [MaxLength(50)]
        public string Task { get; set; }
        [MaxLength(50)]
        public string TaskForce { get; set; }
        [MaxLength(50)]
        public string Formula { get; set; }
        [MaxLength(50)]
        public string Condition { get; set; }
    }
}