using MDL_BASE.Interfaces;
using MDL_BASE.Models.IDENTITY;
using MDL_BASE.Models.MasterData;
using MDLX_CORE.ComponentCore.Entities;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWMS.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XLIB_COMMON.Enums;

namespace MDL_iLOGIS.ComponentWMS.Entities
{
    [Table("WMS_PickingListItem", Schema = "iLOGIS")]
    public class PickingListItem : IModelEntity
    {
        public PickingListItem() {
            //WarehouseLocation = new WarehouseLocation();
            //Platform = new WarehouseLocation();
        }

        public int Id { get; set; }

        public decimal QtyRequested { get; set; }
        public decimal QtyPicked { get; set; }
        public decimal Bilance { get; set; }
        public decimal BomQty { get; set; }

        public UnitOfMeasure UnitOfMeasure { get; set; }

        [MaxLength(100)]
        public string Comment { get; set; }
        
        public EnumPickingListItemStatus Status { get; set; }

        public EnumDeliveryListItemStatus StatusLFI { get; set; }

        public virtual ItemWMS ItemWMS { get; set; }
        public int ItemWMSId { get; set; }

        public virtual WarehouseLocation WarehouseLocation { get; set; }
        public int WarehouseLocationId { get; set; }

        public virtual StockUnit StockUnit { get; set; }
        public int? StockUnitId { get; set; }

        public virtual WarehouseLocation Platform { get; set; }
        public int? PlatformId { get; set; }

        //public virtual ProductionOrder WorkOrder { get; set; }
        //public int WorkOrderId { get; set; }

        //public virtual Transporter Transporter { get; set; }
        //public int TransporterId { get; set; }

        public virtual PickingList PickingList { get; set; }
        public int PickingListId { get; set; }

        public virtual User User { get; set; }
        public string UserId { get; set; }
    }
}
