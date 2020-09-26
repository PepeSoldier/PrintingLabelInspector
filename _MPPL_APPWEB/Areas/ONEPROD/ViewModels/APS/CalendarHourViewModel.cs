using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.ONEPROD.APS.ViewModels
{
    public class CalendarHourViewModel
    {
        public List<Calendar2> Calendar { get; set; }
        public DateTime SelectedDate { get; set; }
        public DateTime SelectedHour { get; set; }
        public int SelectedMachineId { get; set; }
        public int SelectedShift { get; set; }
    }
}