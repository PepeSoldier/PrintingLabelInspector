using MDL_iLOGIS.ComponentWMS.Enums;
using XLIB_COMMON.Enums;

namespace MDL_iLOGIS.ComponentWMS.ViewModels
{
    public class StockUnitViewModel
    {
        public int Id { get; set; }

        public int ItemWMSId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemGroup { get; set; }
        public UnitOfMeasure ItemUoM { get; set; }


        public decimal CurrentQtyinPackage { get; set; }
        public decimal MaxQtyPerPackage { get; set; }
        public decimal ReservedQty { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }

        public int ReservedForPickingListId { get; set; }
        public StatusEnum Status { get; set; }

        public string SerialNumber { get; set; }

        public int? WarehouseId { get; set; }
        public string WarehouseCode { get; set; }
        public string WarehouseName { get; set; }
        public int? AccountingWarehouseId { get; set; }
        public string AccountingWarehouseCode { get; set; }
        public string AccountingWarehouseName { get; set; }
        public bool WarehouseIsOutOfScope { get; set; }
        public bool WarehouseIsMRP { get; set; }

        public int? WarehouseLocationId { get; set; }
        public string WarehouseLocationName { get; set; }
        public string WarehouseLocationFormatted { get; set; }
        public decimal WarehouseLocationUtilization{ get; set; }
        public string WarehouseLocationTypeName { get; set; }

        public bool IsLocated { get; set; }        
    }
}