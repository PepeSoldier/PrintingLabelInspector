using MDL_ONEPROD.Model.Scheduling.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.ONEPROD.ViewModels.OEE
{
    public class OEEDataViewModel
    {
        public OEEDataViewModel()
        {
            machineIds = new List<int>();
            labourBrigadeIds = new List<int>();
        }
        public int Id { get; set; }
        public int ReportId { get; set; }

        public string ResourceName { get; set; }

        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ReasonName { get; set; }
        public int? ReasonId { get; set; }
        //public EnumEntryType EntryType { get; set; }
        public string ReasonTypeName { get; set; }
        public int? ReasonTypeId { get; set; }
        public EnumEntryType ReasonTypeEntryType { get; set; }

        public decimal UsedTime { get; set; }
        public decimal CycleTime { get; set; }
        public decimal ProdQty { get; set; }

        public decimal ProductionCycleTime { get; set; }
        public string LabourBrigadeName { get; set; }
        public string Shift { get; set; }

        public DateTime ProductionDate { get; set; }
        public string ProductionDateFormatted { get; set; }
        public string YearWeek { get; set; }

        public string UserName { get; set; }
        public DateTime EntryTimeStamp { get; set; }
        public string EntryTimeStampFormatted { get; set; }

        public string dateFrom { get; set; }
        public string dateTo { get; set; }
        //public int reasonId { get; set; }
        public List <int> machineIds { get; set; }
        public List <int> labourBrigadeIds { get; set; }

        public int Type { get; set; } //0 = produkcja, 1 = postoje, -1 = wszystko
    }
}