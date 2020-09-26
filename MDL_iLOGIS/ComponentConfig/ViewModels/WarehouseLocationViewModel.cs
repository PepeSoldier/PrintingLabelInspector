using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_iLOGIS.ComponentConfig.ViewModels
{
    public class WarehouseLocationViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameFormatted { get; set; }
        public int? WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public string WarehouseCode{ get; set; }
        public int? TypeId { get; set; }
        public string TypeName { get; set; }
        public string TypeDisplayFormat { get; set; }
        public decimal Utilization { get; set; }
        public int QtyOfSubLocations { get; set; }
        public int? ParentLocationId { get; set; }
        public string ParentLocationName { get; set; }
        public string ParentWarehouseName { get; set; }
    }
}