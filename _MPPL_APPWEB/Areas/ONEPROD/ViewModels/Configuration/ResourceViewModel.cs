using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.ONEPROD.Base.ViewModels
{
    public class ResourceViewModel
    {
        public IQueryable<ResourceOP> Resources { get; set; }
        public ResourceOP NewObject { get; set; }

        [DisplayName("Wybierz Obszar")]
        public int SelectedResourceGroupId { get; set; }

        public IEnumerable<SelectListItem> ResourceGroupsList { get; set; }
    }
}