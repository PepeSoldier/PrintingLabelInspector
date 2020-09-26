using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.ONEPROD.Base.ViewModels
{
    public class ToolViewModel
    {
        public List<Tool> Tools { get; set; }
        public List<ToolGroup> ToolGroups { get; set; }
        public List<ItemGroupTool> ItemGroupsTools { get; set; }

        public Tool NewObject { get; set; }
    }
}