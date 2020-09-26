using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.Common
{
    public class TaskGap
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime TaskBestEndDate { get; set; }
        public string StartTimeStr { get; set; }
        public string EndTimeStr { get; set; }

        public int MarginLeft { get; set; }

        public int TotalSeconds { get; set; }
        public int MachineId { get; set; }
        public int TaskId { get; set; }
        public double TaskProcessingTime { get; set; }
        public double index { get; set; }
    }
}