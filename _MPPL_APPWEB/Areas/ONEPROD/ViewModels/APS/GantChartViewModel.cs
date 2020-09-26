using _MPPL_WEB_START.Areas.ONEPROD.ViewModels.APS;
using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.ONEPROD.APS.ViewModels
{
    public class GantChartViewModel
    {
        public List<ResourceViewModel> Resources { get; set; }
        public List<ClientOrderResourceViewModel> ClientOrderResource { get; set; }

        public List<ResourceOP> ResourceGroups { get; set; }
                
        public DateTime SelectedDateFrom { get; set; }
        public DateTime SelectedDateTo { get; set; }

        public int CurrentTimeRedLinePosition { get; set; }

        public int Zoom { get; set; }
        //public double SecToPixelRatio { get; set; }
    }

    public class GantChart2ViewModel
    {
        public List<ResourceGroupViewModel> ResourceGroups { get; set; }
        public List<ClientOrderResourceViewModel> ClientOrderResource { get; set; }

        public DateTime SelectedDateFrom { get; set; }
        public DateTime SelectedDateTo { get; set; }

        public int Zoom { get; set; }
        //public int CurrentTimeRedLinePosition { get; set; }
        //public double SecToPixelRatio { get; set; }
    }
}