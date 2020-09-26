using MDL_BASE.Interfaces;
using MDL_BASE.Models.IDENTITY;
using MDL_BASE.Models.MasterData;
using MDLX_CORE.ComponentCore.Entities;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWMS.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MDL_iLOGIS.ComponentWMS.Entities
{
    [Table("WMS_LineFeedListItem", Schema = "iLOGIS")]
    public class _LineFeedListItem : IModelEntity
    {
        public _LineFeedListItem() {
            //WarehouseLocation = new WarehouseLocation();
            //Platform = new WarehouseLocation();
        }

        public int Id { get; set; }

        public decimal QtyRequested { get; set; }
        public decimal QtyDelivered { get; set; }
        public decimal Bilance { get; set; }
        public decimal BomQty { get; set; }

        [MaxLength(100)]
        public string Comment { get; set; }
        
        public EnumDeliveryListItemStatus Status { get; set; }

        public virtual ItemWMS ItemWMS { get; set; }
        public int ItemWMSId { get; set; }

        public virtual StockUnit StockUnit { get; set; }
        public int? StockUnitId { get; set; }

        public virtual WarehouseLocation Platform { get; set; }
        public int? PlatformId { get; set; }

        public virtual ProductionOrder WorkOrder { get; set; }
        public int WorkOrderId { get; set; }

        public virtual Transporter Transporter { get; set; }
        public int TransporterId { get; set; }

        //public virtual PickingList PickingList { get; set; }
        //public int PickingListId { get; set; }

        //public virtual User User { get; set; }
        //public string UserId { get; set; }
    }
}
