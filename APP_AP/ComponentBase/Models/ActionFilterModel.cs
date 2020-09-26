using MDL_AP.Models.ActionPlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_AP.ComponentBase.Models
{
    public class ActionFilterModel
    {
        public int Id { get; set; }
        public int ParentActionId { get; set; }

        public DateTime? DateCreated { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDateFrom { get; set; }
        public DateTime? EndDateTo { get; set; }
        public DateTime? PlannedEndDate { get; set; }

        public string Title { get; set; }
        public string Problem { get; set; }
        public string CreatorId { get; set; }
        public string CreatorFullName { get; set; }
        public string AssignedId { get; set; }
        public string AssignedFullName { get; set; }

        public int? DepartmentId { get; set; }
        public int? AreaId { get; set; }
        public int? LineId { get; set; }
        public int? ShiftCodeId { get; set; }
        public int? WorkstationId { get; set; }
        public int? CategoryId { get; set; }
        public int? TypeId { get; set; }

        //public ActionStateEnum State { get; set; }
        public string State { get; set; }

        public bool ShowChildActions { get; set; }
        public bool ShowOnlyOpenedActions { get; set; }
    }
}