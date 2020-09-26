using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_iLOGIS.ComponentWMS.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using XLIB_COMMON.Enums;

namespace MDL_iLOGIS.ComponentWMS.ViewModels
{
    public class PickingListItemManageViewModel
    {
        public int PickingListItemId { get; set; }
        public int PickingListId { get; set; }
        public int WorkOrderId { get; set; }
        public int PickerId { get; set; }
        public int MaximumItemNumbersToPackage { get; set; }
        public string Barcode { get; set; }
        public string PickingListItemCode { get; set; }
        public string PickingListItemName { get; set; }
        public string PickingListItemLocationName { get; set; }

        public string SerialNumberScanned { get; set; }
        public string ItemCodeScanned { get; set; }
        public bool isDifferentSerialNumber { get; set; }
        
        public string BarcodeTemplate { get; set; }

        [DefaultValue(true)]
        public bool isWarehouseLocationSet { get; set; }

        public decimal QtyRequested { get; set; }
        public decimal QtyPicked { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }

        public EnumTransportingStatus PickingListItemStatus { get; set; }

        public string PickingListItemPlatformName { get; set; }
        public int PickingListItemPlatformId { get; set; }

        public IEnumerable<object> CommentList { get; set; }
        public int CommentListId { get; set; }
        public string CommentItemString { get; set; }

        public string StockUnitSerialNumber { get; set; }
        public decimal StockUnitCurrentQtyInPackage { get; set; }
    }
}
