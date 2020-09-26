using MDL_AP.ComponentBase.Models;
using MDL_AP.Models.ActionPlan;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using XLIB_COMMON.Interface;

namespace _MPPL_WEB_START.Areas.AP.ViewModel
{
    public class ActionBrowsenewViewModel
    {
        //public ActionModel FilterObject { get; set; }
        public IQueryable<ActionModel> ActionModels { get; set; }
        public List<ActionState> ActionStates { get; set; }
        public ActionFilterModel FilterObject { get; set; }

        public string TypeIds { get; set; }
        public string CategoryIds { get; set; }
        public string States { get; set; }

        public int CurrentPage { get; set; }
        public int TotalRows { get; set; }
        public int RowsOnPage { get; set; }

        //public DateTime? EndDateFrom { get; set; }
        //public DateTime? EndDateTo { get; set; }
        //public string State { get; set; }
        //public bool ShowChildActions { get; set; }

        //public int Id { get; set; }
        //public string CreatorId { get; set; }
        //public string AssignedId { get; set; }
        //public string CreatorFullName { get; set; }
        //public string AssignedFullName { get; set; }

        //Dropdowns
        public IEnumerable<SelectListItem> Workstations { get; set; }
        [Display(Name="Stanowisko")]
        public int? WorkstationId { get; set; }

        public IEnumerable<SelectListItem> ShiftCodes { get; set; }
        [Display(Name = "Kod zmiany")]
        public int? ShiftCodeId { get; set; }

        public IEnumerable<SelectListItem> Areas { get; set; }
        [Display(Name = "Obszar")]
        public int? AreaId { get; set; }

        public IEnumerable<SelectListItem> Departments { get; set; }
        [Display(Name = "Dział")]
        public int? DepartmentId { get; set; }

        public IEnumerable<SelectListItem> Lines { get; set; }
        [Display(Name = "Linia")]
        public int? LineId { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
        [Display(Name = "Kategoria")]
        public int? CategoryId { get; set; }

        public IEnumerable<SelectListItem> Types { get; set; }
        [Display(Name = "Typ")]
        public int? TypeId { get; set; }
    }
}