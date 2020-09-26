using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.ONEPROD.APS.ViewModels
{
    public class CalculationViewModel
    {
        public List<ResourceOP> Areas { get; set; }
        
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

        public string Guid { get; set; }
        public bool ConsiderCalendar { get; set; }
        public bool BatchTasks { get; set; }
        public List<int> ConsideredAreas { get; set; }


    }
}