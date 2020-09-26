using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.ONEPROD.Base.ViewModels
{
    public class ItemGroupViewModel
    {
        public List<ItemOP> ItemGroups { get; set; }
        public List<ResourceOP> Areas {get;set;}
        public List<MDLX_MASTERDATA.Entities.Process> ItemGroupGroups { get; set; }

        public IEnumerable<SelectListItem> AreasSelectList { get; set; }
        public IEnumerable<SelectListItem> ItemGroupGroupsSelectList { get; set; }

        [DisplayName("Wybierz obszar")]
        public int SelectedAreaId { get; set; }
        [DisplayName("Wybierz kategorię")]
        public int SelectedItemGroupGroupId { get; set; }

        public ItemOP NewObject { get; set; }
    }
}