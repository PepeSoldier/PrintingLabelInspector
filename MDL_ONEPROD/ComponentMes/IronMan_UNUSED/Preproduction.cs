using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.Model.IronMan
{
    [Table("IRONMAN_Preproduction")]
    public class Preproduction
    {
        public int Id {get;set;}
        [MaxLength(10)]
        public string User {get;set;}
        [Column(TypeName = "Date")]
        public DateTime Date {get;set;}
        //[Column(TypeName = "Time")]
        public DateTime Time {get;set;}
        public ItemOP Part {get;set;}
        public int PartId { get; set; }
        public Coil Coil {get;set;}
        public int CoilID {get;set;}
        public decimal InitialScrap {get;set;}
        public int QtyScrapProcess { get; set; }
        public int QtyScrapMaterial { get; set; }
        public int QtyProduced {get;set;}
        public decimal PaperAndOther { get; set; }
        public decimal TubeWeight { get; set; }
        public decimal BOM {get;set;}
        public decimal Consumption {get;set;}
        public EnumDeclarationStatus Status { get; set; }
    }
}