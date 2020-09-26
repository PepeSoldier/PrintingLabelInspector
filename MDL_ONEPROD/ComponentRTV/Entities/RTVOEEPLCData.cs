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
    [Table("RTV_RTVOEEPLCData", Schema = "ONEPROD")]
    public class RTVOEEPLCData : IModelDeletableEntity
    {
        public RTVOEEPLCData()
        {
        }

        public int Id { get; set; }
        public bool Deleted { get; set; }
        [MaxLength(20)]
        public string PlcIP { get; set; }
        public bool PlcStatus { get; set; }
        public int MachineId { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime LastUpdate { get; set; }

        [MaxLength(100)]
        public string P1 { get; set; }
        [MaxLength(100)]
        public string P2 { get; set; }
        [MaxLength(100)]
        public string P3 { get; set; }
        [MaxLength(100)]
        public string P4 { get; set; }
        [MaxLength(100)]
        public string P5 { get; set; }
        [MaxLength(100)]
        public string P6 { get; set; }
        [MaxLength(100)]
        public string P7 { get; set; }
        [MaxLength(100)]
        public string P8 { get; set; }
        [MaxLength(100)]
        public string P9 { get; set; }
        [MaxLength(100)]
        public string P10 { get; set; }
        [MaxLength(100)]
        public string P11 { get; set; }
        [MaxLength(100)]
        public string P12 { get; set; }
        [MaxLength(100)]
        public string P13 { get; set; }
        [MaxLength(100)]
        public string P14 { get; set; }
        [MaxLength(100)]
        public string P15 { get; set; }
        [MaxLength(100)]
        public string P16 { get; set; }
        [MaxLength(100)]
        public string P17 { get; set; }
        [MaxLength(100)]
        public string P18 { get; set; }
        [MaxLength(100)]
        public string P19 { get; set; }
        [MaxLength(100)]
        public string P20 { get; set; }
        [MaxLength(100)]
        public string P21 { get; set; }
        [MaxLength(100)]
        public string P22 { get; set; }
        [MaxLength(100)]
        public string P23 { get; set; }
        [MaxLength(100)]
        public string P24 { get; set; }
        [MaxLength(100)]
        public string P25 { get; set; }
        [MaxLength(100)]
        public string P26 { get; set; }
        [MaxLength(100)]
        public string P27 { get; set; }
        [MaxLength(100)]
        public string P28 { get; set; }
        [MaxLength(100)]
        public string P29 { get; set; }
        [MaxLength(100)]
        public string P30 { get; set; }
        [MaxLength(100)]
        public string P31 { get; set; }
        [MaxLength(100)]
        public string P32 { get; set; }
        [MaxLength(100)]
        public string P33 { get; set; }
        [MaxLength(100)]
        public string P34 { get; set; }
        [MaxLength(100)]
        public string P35 { get; set; }
        [MaxLength(100)]
        public string P36 { get; set; }
        [MaxLength(100)]
        public string P37 { get; set; }
        [MaxLength(100)]
        public string P38 { get; set; }
        [MaxLength(100)]
        public string P39 { get; set; }
        [MaxLength(100)]
        public string P40 { get; set; }
    }
}
