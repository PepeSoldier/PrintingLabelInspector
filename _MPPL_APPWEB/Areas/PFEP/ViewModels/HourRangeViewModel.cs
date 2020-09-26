using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.PFEP.ViewModels
{
    public class HourRangeViewModel
    {
        public HourRangeViewModel()
        {
            BegDate = new DateTime();
            EndDate = new DateTime();
        }
        public DateTime BegDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}