using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_iLOGIS.ComponentWMS.ViewModels
{
    public class WarehouseSelectorViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? ParentWarehouseId { get; set; }
        public string ParentWarehouseName { get; set; }
        public int QtyOfSubLocations { get; set; }
    }
}