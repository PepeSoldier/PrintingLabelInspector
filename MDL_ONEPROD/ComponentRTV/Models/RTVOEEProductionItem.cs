using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.Model.Scheduling.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.ComponentRTV.Models
{
    public class RTVOEEProductionItem
    {
        //public DateTime? ProductionDate { get; set; }

        public virtual ItemOP Item { get; set; }
        public int? ItemId { get; set; }

        public decimal ProdQty { get; set; }

        //public EnumEntryType entryType { get; set; }
        public ReasonType ReasonType { get; set; }

        public DateTime TimeStamp { get; set; }

        public int ProgramNo { get; set; }
        public string ProgramName { get; set; }
    }
}