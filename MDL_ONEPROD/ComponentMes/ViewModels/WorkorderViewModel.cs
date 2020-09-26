using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.ComponentMes.ViewModels
{
    public class WorkorderViewModel
    {
        public int Id { get; set; }
        
        public string UniqueTaskNumber { get; set; }

        public string OrderNumber { get; set; }
        public string Line { get; set; }

        public string PartCode { get; set; }
        public string PartName { get; set; }
        public int PartId { get; set; }
        public int MachineId { get; set; }

        public DateTime ReleaseDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public int Qty_Total { get; set; }
        public int Qty_Produced { get; set; }
        public int Qty_Used { get; set; }
        public int Qty_Remain { get { return Math.Max(Qty_Total - Math.Max(Qty_Produced, Qty_Used), 0); } }

        public int ProcessingTime { get; set; }
        public int BatchNumber { get; set; }
        public TaskScheduleStatus Status { get; set; }

        public int Param1 { get; set; }
        public int Param2 { get; set; }
    }
}