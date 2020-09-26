using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Model.Scheduling.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.ComponentMes.Models
{
    public interface IOeeProductionData
    {
        int Id { get; set; }
        int ReportId { get; set; }
        int? ReasonId { get; set; }
        int? ItemId { get; set; }
        decimal ProdQty { get; set; }
        decimal CycleTime { get; set; }
        decimal UsedTime { get; set; }
        //EnumEntryType EntryType { get; set; }
        //ReasonType ReasonType { get; set; }
        string ReasonTypeName { get; set; }
        int? ReasonTypeId { get; set; }
        EnumEntryType ReasonTypeEntryType { get; set; }
        DateTime ProductionDate { get; set; }
    }

    public class ReportOnlineModel : IOeeProductionData
    {
        public int Id { get; set; }
        public int ReportId { get; set; }
        public int? ReasonId { get; set; }
        public string ReasonName { get; set; }
        public int? ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public decimal ProdQty { get; set; }
        public decimal ReportedProdQty { get; set; }
        public decimal CycleTime { get; set; }
        public decimal UsedTime { get; set; }
        //public EnumEntryType EntryType { get; set; }
        //public ReasonType ReasonType { get; set; }
        public string ReasonTypeName { get; set; }
        public int? ReasonTypeId { get; set; }
        public EnumEntryType ReasonTypeEntryType { get; set; }
        public DateTime ProductionDate { get; set; }
    }
}