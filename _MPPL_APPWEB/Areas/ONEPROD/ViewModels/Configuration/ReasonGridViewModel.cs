using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.Model.Scheduling.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.ONEPROD.Base.ViewModels
{
    public class ReasonGridViewModel
    {
        public List<Reason> Reasons { get; set; }
        public List<MachineReason> MachineReason { get; set; }

        public IEnumerable<SelectListItem> Machines { get; set; }
    }
}