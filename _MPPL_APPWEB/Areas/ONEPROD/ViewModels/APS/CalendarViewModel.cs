using MDL_ONEPROD.Common;
using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.ONEPROD.APS.ViewModels
{
    public class CalendarViewModel
    {
        public CalendarViewModel()
        {
            Days = new string[] { "Pn", "Wt", "Śr", "Cz", "Pt", "Sb", "Nd"};
        }
        public List<Calendar2> Calendar { get; set; }
        public List<CalendarMonth> Months { get; set; }
        public List<ResourceOP> Machines { get; set; }

        public int MachineId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string[] Days { get; set; }
    }
}