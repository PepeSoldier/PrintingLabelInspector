using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using _MPPL_WEB_START.Areas._APPWEB.Models;
using MDLX_CORE.ComponentCore.Entities;
using MDL_PFEP.Models.DEF;
using MDL_PRD.Model;
using XLIB_COMMON.Enums;

namespace _MPPL_WEB_START.Areas.PFEP.ViewModels
{
    public class PrintViewModel
    {
        public IQueryable<ProductionOrder> ProductionOrders { get; set; }
        public ProductionOrderFilter FilterObject { get; set; }
        public  HourRangeViewModel HourRange { get; set; }

        public List<Routine> Routines { get; set; }
        public IEnumerable<SelectListItem> Lines { get; set; }
        public IEnumerable<SelectListItem> Shifts { get; set; }

        public int routineVal { get; set; }
    }
    
}