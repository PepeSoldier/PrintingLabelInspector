using MDL_AP.Models.ActionPlan;
using MDL_BASE.Models.Base;
using System.Collections.Generic;
using System.Web.Mvc;
using XLIB_COMMON.Interface;

namespace _MPPL_WEB_START.Areas.AP.ViewModel
{
    public class ActionShowViewModel
    {
        public ActionModel Action { get; set; }
        public List<ActionActivity> ActionActivities { get; set; }
        public List<ActionModel> ChildActions { get; set; }
        public List<Attachment> Attachments { get; set; }
        public ActionActivity ActionActivity { get; set; }
        public ActionStateEnum ActionStateEnum { get; set; }
        public bool IsUserAllowedToEdit { get; set; }

        public int SubactionsCount { get; set; }
        public int SubactionsFinishedCount { get; set; }
        public int SubactionsDistinctResponsibleCount { get; set; }
        public int MetingId { get; set; }

        //AssignedUsers
        public int ActionId { get; set; }
        public IEnumerable<SelectListItem> Users { get; set; }
        public string AssignedId { get; set; }

    }
}