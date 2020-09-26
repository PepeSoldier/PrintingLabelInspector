using MDL_PRD.ComponentSchedule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.PRD.ViewModels
{
    public class ScheduleMonitorOrderViewModel
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Line { get; set; }
        public string PNC { get; set; }
        public string OrderNo { get; set; }
        public int QtyPlanned { get; set; }
        public int QtyIN { get; set; }
        public int QtyOUT { get; set; }
        public int QtyFGW { get; set; }
        public List<OrderStatusViewModel> Statuses { get; set; }
    }

    public class OrderStatusViewModel
    {
        //public string StatusName { get; set; }
        //public List<string> StatusDetails { get; set; }

        public int Id { get; set; }
        public int OrderId { get; set; }
        public string StatusName { get; set; }
        public EnumOrderState StatusState { get; set; }
        public string StatusInfo { get; set; }
        public string StatusInfoExtra { get; set; }
        public string StatusInfoExtra2 { get; set; }
        public int StausInfoExtraNumber { get; set; }
    }
}