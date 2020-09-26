using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.ONEPROD.Base.ViewModels
{
    public class ItemViewModel
    {
        public IQueryable<ItemOP> Items { get; set; }
        public IQueryable<ItemOP> ItemGroups { get; set; }
        public IQueryable<Tool> Tools { get; set; }
        public IEnumerable<SelectListItem> ResourceGroups {get;set;}
        
        public int SelectedResourceGroupId { get; set; }
        public int SelectedItemGroupId {get;set;}


    }
}