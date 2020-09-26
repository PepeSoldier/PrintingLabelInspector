using MDL_iLOGIS.ComponentWMS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLIB_COMMON.Enums;

namespace MDL_iLOGIS.ComponentWMS.Models
{
    public class PickingListItemViewModel_2
    {
        public int Id { get; set; }
        public int PickingListId { get; set; }
        public int WorkOrderId { get; set; }
        public string WorkOrderNo { get; set; }
        public int ItemId { get; set; }
        public int TransporterId { get; set; }
        public string TransporterName { get; set; }
        //public string PlatformLocationName { get; set; }
        public int PlatformId { get; set; }
        public string PlatformName { get; set; }
        public int PlatformReservedValue { get; set; }
        public string ConnectedTransporters { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string PickingListGuid { get; set; }
        public int ResourceId { get; set; }
        public string ResourceName { get; set; }

        public string WarehouseLocationName { get; set; }
        public int WarehouseLocationId { get; set; }

        public string WorkstationName { get; set; }
        public int WorkstationId { get; set; }
        public int WorkstationSortOrder { get; set; }
        public string PutTo { get; set; }

        public decimal? QtyRequested { get; set; }
        public decimal? QtyPicked { get; set; }
        public decimal? Balance { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }

        public int H { get; set; }
        public EnumPickingListItemStatus Status { get; set; }
        public EnumDeliveryListItemStatus StatusLFI { get; set; }
    }
}
