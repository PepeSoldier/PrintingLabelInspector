using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.iLOGIS.ViewModels.PickingList
{
    public class ApiLocationViewModel
    {
        public string LocationCode { get; set; }
        public int QtyInBox { get; set; }
        public int LocationPosition { get; set; }
    }
}