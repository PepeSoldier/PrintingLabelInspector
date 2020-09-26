using MDL_ONEPROD.ComponentMes.Models;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Model.Scheduling.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.ComponentOEE.ViewModels
{
    public class CreateReportStoppageViewModel : CreateReportProdDataDetailViewModel,
        IOeeProductionData
    {
        public int Id { get; set; }
        public int ReportId { get; set; }
        //public EnumEntryType EntryType { get; set; }
        //public ReasonType ReasonType { get; set; }//
        public string ReasonTypeName { get; set; }
        public int? ReasonTypeId { get; set; }
        public EnumEntryType ReasonTypeEntryType { get; set; }

        public string Hour { get; set; }
        public int? ReasonId { get; set; }
        public string ReasonName { get; set; }
        public decimal UsedTime { get; set; }

        public DateTime ProductionDate { get; set; }
        public decimal CycleTime { get; set; }
        public decimal ProdQty { get; set; }
        public int? ItemId { get; set; }
    }
}