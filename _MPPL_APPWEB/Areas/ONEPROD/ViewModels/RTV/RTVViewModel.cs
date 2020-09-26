using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.ONEPROD.ViewModels.RTV
{
    public class RTVViewModel
    {
        public ResourceOP Machine { get; set; }
        public List<ReasonType> Reasontypes { get; set; }
    }
}