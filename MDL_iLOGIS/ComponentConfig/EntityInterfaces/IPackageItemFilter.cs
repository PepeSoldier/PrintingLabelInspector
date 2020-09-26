using MDL_iLOGIS.ComponentWMS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_iLOGIS.ComponentConfig.EntityInterfaces
{
    public interface IPackageItemFilter
    {
        int Id { get; set; }
        int PackageId { get; set; }
        string PackageCode { get; set; }
        string PackageName { get; set; }
        string ItemCode { get; set; }
        string ItemName { get; set; }
        string WarehouseName { get; set; }
        string WarehouseLocationTypeName { get; set; }
        PickingStrategyEnum PickingStrategy { get; set; }
        decimal QtyPerPackage { get; set; }        
    }
}
