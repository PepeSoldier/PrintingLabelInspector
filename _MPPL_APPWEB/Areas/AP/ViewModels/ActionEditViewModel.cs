using System.Collections.Generic;
using System.Web.Mvc;
using MDL_AP.Models.ActionPlan;
using XLIB_COMMON.Interface;

namespace _MPPL_WEB_START.Areas.AP.ViewModel
{
    public class ActionEditViewModel
    {
        public ActionModel NewObject { get; set; }
        public string UserName { get; set; }
        public ActionStateEnum enumAC { get; set; }
        public int OldDepartmentId { get; set; }
        public int MeetingId { get; set; }

    //Dropdowns
        public IEnumerable<SelectListItem> Workstations { get; set; }
        public IEnumerable<SelectListItem> ShiftCodes { get; set; }
        public IEnumerable<SelectListItem> Areas { get; set; }
        public IEnumerable<SelectListItem> Departments { get; set; }
        public IEnumerable<SelectListItem> Lines { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> Types { get; set; }
   }
}