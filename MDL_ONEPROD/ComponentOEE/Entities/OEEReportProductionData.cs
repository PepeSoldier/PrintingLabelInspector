using MDL_BASE.Interfaces;
using MDL_BASE.Models.IDENTITY;
using MDL_ONEPROD.ComponentMes.Models;
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
    [Table("OEE_OEEReportProductionData", Schema = "ONEPROD")]
    public class OEEReportProductionData : OEEReportProductionDataAbstract, 
        IModelDeletableEntity, IOeeProductionData
    {
        public OEEReportProductionData()
        {
            
        }

        public virtual OEEReport Report { get; set; }
        public int ReportId { get; set; }

        public int ProdQtyCountedOnline { get; set; }

        [NotMapped]
        public OEEProdDataEntryStatus EntryStatus { get; set; }

        //TODO: Uważać na to IOeeProductionData
        [NotMapped]
        public string ReasonTypeName { get; set; }
        [NotMapped]
        public EnumEntryType ReasonTypeEntryType { get; set; }

        public override string ToString()
        {
            return base.ToString() + " reportId: " + ReportId;
        }

    }

    public enum OEEProdDataEntryStatus
    {
        Unknown = -1,
        Existing = 1,
        New = 2,
        Deleted = 0,
    }
}