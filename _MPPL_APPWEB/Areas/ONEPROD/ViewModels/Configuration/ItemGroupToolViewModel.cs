using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.ONEPROD.Base.ViewModels
{
    public class ItemGroupToolViewModel
    {
        public ItemOP ItemGroup { get; set; }
        public ItemGroupTool NewObject { get; set; }
        public List<ItemGroupTool> ItemGroupTools { get; set; }
        public IEnumerable<SelectListItem> Tools { get; set; }
        public int SelectedToolId { get; set; }
    }
}