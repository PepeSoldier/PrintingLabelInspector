using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.Model.IronMan
{
    [Table("IRONMAN_Coil")]
    public class Coil
    {
        public int Id {get;set;}
        [MaxLength(9)]
        public string ANC {get;set;}
        [MaxLength(50)]
        public string Name {get;set;}

        public decimal WeightN {get;set;}
        public decimal WeightB { get; set; }
        [MaxLength(20)]
        public string SupplierCoilID {get;set;}
        [MaxLength(8)]
        public string Location {get;set;}
        [MaxLength(50)]
        public string Supplier {get;set;}
        public DateTime DeliveryDate {get;set;}

        public DateTime TimeStamp {get;set;}
        [MaxLength(10)]
        public string User {get;set;}
        public EnumCoilStatus Status { get; set; }
    }
}