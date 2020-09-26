using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDL_ONEPROD.Model.Scheduling;
using System.Web.Mvc;
using MDL_iLOGIS.ComponentConfig.Entities;

namespace _MPPL_WEB_START.Areas.ONEPROD.Base.ViewModels
{
    public class BoxItemGroupViewModel
    {
        public int BoxId { get; set; }

        public Warehouse Box { get; set; }
        public WarehouseItem NewObject { get; set; }
        public List<WarehouseItem> BoxItemGroups { get; set; }
        public IEnumerable<SelectListItem> ItemGroups { get; set; }
        public int SelectedItemGroupId { get; set; }
    }
}