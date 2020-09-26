using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using _MPPL_WEB_START.Areas._APPWEB.ViewModels;
using MDL_BASE.ComponentBase.Entities;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Model.Scheduling;

namespace _MPPL_WEB_START.Areas.ONEPROD.OEE.ViewModels
{
    public class OeeViewModel
    {
        public OEEReport OeeReport { get; set; }
        public int ReportId { get; set; }

        public List<SelectListItem> Shifts { get; set; }
        public int ShiftId { get; set; }

        public IEnumerable<ResourceOP> Machines { get; set; }
        public int MachineId { get; set; }

        public int AreaId { get; set; }

        public IEnumerable<SelectListItem> LabourBrigades { get; set; }
        public int LabourBrigadeId { get; set; }

        public List<SelectListItem> Lines { get; set; }
        public int LineId { get; set; }

        public int Stage { get; set; }

        public List<ReasonType> ReasonTypes { get; set; }
        
    }
}