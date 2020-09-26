using MDL_iLOGIS.ComponentConfig.EntityInterfaces;
using MDL_iLOGIS.ComponentWMS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Enums;

namespace MDL_iLOGIS.ComponentConfig.ViewModels
{
    public class PackageItemViewModel : IPackageItemFilter
    {
        public int Id { get; set; }
        public int PackageId { get; set; }
        public string PackageCode { get; set; }
        public string PackageName { get; set; }

        public int ItemWMSId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }

        public int? WarehouseId { get; set; }
        public string WarehouseName { get; set; }

        public int? WarehouseLocationTypeId { get; set; }
        public string WarehouseLocationTypeName { get; set; }

        public PickingStrategyEnum PickingStrategy { get; set; }

        public decimal QtyPerPackage { get; set; }
        public int PackagesPerPallet { get; set; }
        public int PalletW { get; set; }
        public int PalletD { get; set; }
        public int PalletH { get; set; }
        public decimal WeightGross { get; set; }

        public decimal QtyPerPallet { get { return QtyPerPackage * PackagesPerPallet; } } //not mapped
        public decimal WeightNet { get; set; } //not mapped

        public UnitOfMeasure UnitOfMeasure { get; set; }

        //for filters:
        public string p { get; set; }
        public string w { get; set; }
        public string h { get; set; }
        public string d { get; set; }

        
        

    }
}