using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_iLOGIS.ComponentWMS.ViewModels
{
    public class DeliveryListWorkorderViewModel
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public DateTime StartTime { get; set; }
        public string StartTimeStr { get; set; }
        public int LineId { get; set; }
        public string LineName { get; set; }

        public DateTime EndTime { get; set; }
        public string EndTimeStr { get; set; }

        /// <summary>
        /// Processing time its a time from StartTime to EndTime expressed in seconds.
        /// </summary>
        public int ProcessingTime { get; set; }

        public int Qty { get; set; }
        public int QtyIn { get; set; }
        public int QtyOut { get; set; }
        public int QtyTotal { get; set; }
        public DateTime? LastScanDate { get; set; }

        public string Notice { get; set; }
    }
}