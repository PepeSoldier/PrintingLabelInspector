using MDL_ONEPROD.ComponentMes.Models;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Model.Scheduling.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.ComponentOEE.ViewModels
{
    public class CreateReportProdDataViewModel : CreateReportProdDataDetailViewModel, IOeeProductionData
    {
        public int Id { get; set; }
        public int ReportId { get; set; }
        public int? ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public decimal ProdQty { get; set; }
        public decimal CycleTime { get; set; }
        public decimal UsedTime { get; set; }
        //public EnumEntryType EntryType { get; set; }
        //public ReasonType ReasonType { get; set; }
        public string ReasonTypeName { get; set; }
        public int? ReasonTypeId { get; set; }
        public EnumEntryType ReasonTypeEntryType { get; set; }
        public int? ReasonId { get; set; }
        public DateTime ProductionDate { get; set; }
        //public string UserName { get; set; }
        //public string TimeStamp { get; set; }
    }
}