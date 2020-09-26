using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.ONEPROD.Base.ViewModels
{
    public class ResourceGroupViewModel
    {
        public List<ResourceOP> ResourceGroups { get; set; }
        public ResourceOP NewObject { get; set; }

        public ResourceGroupViewModel()
        {
        }

        
    }
}