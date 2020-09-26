using MDL_AP.Models.ActionPlan;
using System.Collections.Generic;

namespace _MPPL_WEB_START.Areas.AP.ViewModel
{
    public class ActionActivityViewModel
    {
        public List<ActionActivity> ActionActivities { get; set; }
        public ActionActivity ActionActivity { get; set; }
        public int ActionId { get; set; }
        public int MeetingId { get; set; }
    }
}