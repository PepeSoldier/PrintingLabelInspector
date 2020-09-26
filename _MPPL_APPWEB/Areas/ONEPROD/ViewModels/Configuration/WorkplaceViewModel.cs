using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.ONEPROD.Base.ViewModels
{
    public class WorkplaceViewModel
    {
        public IQueryable<Workplace> Workplaces { get; set; }
        public Workplace NewObject { get; set; }

        [DisplayName("Wybierz Maszynę")]
        public int SelectedMachineId { get; set; }

        public IEnumerable<SelectListItem> MachinesList { get; set; }
    }
}