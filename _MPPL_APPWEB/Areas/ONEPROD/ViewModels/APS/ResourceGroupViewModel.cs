using MDL_ONEPROD.Model.Scheduling;
using MDLX_MASTERDATA.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.ONEPROD.ViewModels.APS
{
    public class ResourceGroupViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StageNo { get; set; }
        public List<ResourceViewModel> Resources { get; set; }
    }
}