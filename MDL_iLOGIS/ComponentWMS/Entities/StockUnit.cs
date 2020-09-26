using MDL_BASE.Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWMS.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using XLIB_COMMON.Enums;

namespace MDL_iLOGIS.ComponentWMS.Entities
{
    [Table("WMS_StockUnit", Schema = "iLOGIS")]
    public class StockUnit : IModelEntity, IModelDeletableEntity
    {
        public StockUnit()
        {
            BestBeforeDate = DateTime.Now;
            CreatedDate = DateTime.Now;
            WMSLastCheck = DateTime.Now;
            IncomeDate = DateTime.Now;
        }
        //old name: WarehaouseLocationItem
        //Id będzie unikalnym numerem opakowania 

        public int Id { get; set; }

        public decimal CurrentQtyinPackage { get; set; }
        public decimal ReservedQty { get; set; }
        public decimal WMSQtyinPackage { get; set; }
        public decimal MaxQtyPerPackage { get; set; } //TODO: też powinno być decimalem
        public decimal InitialQty { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }

        public virtual WarehouseLocation WarehouseLocation { get; set; }
        public int WarehouseLocationId { get; set; }
        public string SerialNumber { get; set; }

        public virtual ItemWMS ItemWMS { get; set; }
        public int ItemWMSId { get; set; }

        public virtual PackageItem PackageItem { get; set; } 
        public int? PackageItemId { get; set; }

        public DateTime WMSLastCheck { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime BestBeforeDate { get; set; }
        public DateTime IncomeDate { get; set; }
        
        public StatusEnum Status { get; set; }

        public bool IsLocated { get; set; }
        public bool Deleted { get; set; }

        public int ReferenceDeliveryItemId { get; set; }

        //Grupowanie dla etykiety zbiorczej!
        public bool IsGroup { get; set; }
        public StockUnit Group { get; set; }
        public int? GroupId { get; set; }

        [NotMapped]
        public Warehouse AccountingWarehouse { get; set; }
    }
}