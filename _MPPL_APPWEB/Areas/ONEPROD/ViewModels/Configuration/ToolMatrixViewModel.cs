using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.ONEPROD.Base.ViewModels
{
    public class ToolMatrixViewModel
    {
        public List<Tool> Tools { get; set; }
        public List<Tool> Tools2 { get; set; }
        public List<ToolChangeOver> ToolChangeOvers { get; set; }
    }
}