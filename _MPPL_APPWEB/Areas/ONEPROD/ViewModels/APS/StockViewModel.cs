using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.ONEPROD.APS.ViewModels
{
    public class StockViewModel
    {
        public List<ItemInventory> Stock { get; set; }
        public DateTime SelectedDate { get; set; }
        public List<DateTime> StockDates { get; set; }
        public List<Warehouse> Boxes { get; set; }

        //update
        public int AreaId { get; set; }
        public int StockId { get; set; }
        public int StockQty { get; set; }

    }
}