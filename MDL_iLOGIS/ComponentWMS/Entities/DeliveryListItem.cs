using MDL_BASE.Interfaces;
using MDL_BASE.Models.IDENTITY;
using MDL_BASE.Models.MasterData;
using MDLX_CORE.ComponentCore.Entities;
using MDL_iLOGIS.ComponentConfig.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using MDL_iLOGIS.ComponentWMS.Enums;
using XLIB_COMMON.Enums;

namespace MDL_iLOGIS.ComponentWMS.Entities
{
    [Table("WMS_DeliveryListItem", Schema = "iLOGIS")]
    public class DeliveryListItem : IModelEntity
    {
        public int Id { get; set; }

        public virtual ItemWMS ItemWMS { get; set; }
        [Index("IX_ItemWorkstationWorkorderTransporterPickignListItem", 4, IsUnique = true)]
        public int ItemWMSId { get; set; }

        public decimal QtyRequested { get; set; }
        public decimal QtyDelivered { get; set; }
        public decimal QtyUsed { get; set; }
        public decimal QtyPerPackage { get; set; }
        public decimal BomQty { get; set; }

        public UnitOfMeasure UnitOfMeasure { get; set; }

        public virtual Workstation Workstation { get; set; }
        [Index("IX_ItemWorkstationWorkorderTransporterPickignListItem", 3, IsUnique = true)]
        public int? WorkstationId { get; set; }

        public virtual ProductionOrder WorkOrder { get; set; }
        [Index("IX_ItemWorkstationWorkorderTransporterPickignListItem", 2, IsUnique = true)]
        public int WorkOrderId { get; set; }

        public virtual Transporter Transporter { get; set; }
        [Index("IX_ItemWorkstationWorkorderTransporterPickignListItem", 1, IsUnique = true)]
        public int TransporterId { get; set; }

        public virtual User User { get; set; }
        public string UserId { get; set; }

        public bool IsSubstituteData { get; set; }

        //For line feeding (from picking)
        public virtual PickingListItem PickingListItem { get; set; }
        [Index("IX_ItemWorkstationWorkorderTransporterPickignListItem", 5, IsUnique = true)]
        public int? PickingListItemId { get; set; }
        //For line feeding (from picking)
        public virtual DeliveryList DeliveryList { get; set; }
        public int? DeliveryListId { get; set; }
        //For line feeding (from picking)
        public virtual StockUnit StockUnit { get; set; }
        public int? StockUnitId { get; set; }
        //For line feeding (from picking)
        public virtual WarehouseLocation WarehouseLocation { get; set; }
        public int? WarehouseLocationId { get; set; }
        //For line feeding (from picking)
        public EnumDeliveryListItemStatus Status { get; set; }
    }
}
