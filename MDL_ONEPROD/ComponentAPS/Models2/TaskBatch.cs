using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.Common
{
    public class WorkorderBatch
    {
        public WorkorderBatch()
        {
            Number = 0;
            Workorders = new List<Workorder>();
            Qty = 0;
        }
 
        public int Number { get; set; }
        public int Qty { get; set; }
        public List<Workorder> Workorders { get; set; }

        //public DateTime MaxDueDate { get { return Tasks.Max(x => x.DueDate); } }
        //public DateTime MinDueDate { get { return Tasks.Min(x => x.DueDate); } }

        public int Width { get; set; }
        public int MarginLeft { get; set; }
        public int ResourceId { get; set; }
        public int? ItemId { get; set; }
        public int? ToolId { get; set; }
        public int ProcessingTime { get; set; }
        public int SetupTime { get; set; }
        public string Color { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public override string ToString()
        {
            return "BachNo: " + Number.ToString() + ", Count: " + Workorders.Count.ToString() + ", Machine: " + ResourceId;
        }
    }
}