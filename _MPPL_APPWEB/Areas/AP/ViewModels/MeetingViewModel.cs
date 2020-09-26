using MDL_AP.Models.ActionPlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.AP.ViewModel
{
    public class MeetingViewModel
    {
        public Meeting NewObject { get; set; }

        public IQueryable<ActionModel> ActionList { get; set; }
        public IQueryable<Meeting> MeetingList { get; set; }
    }
}