using MDL_iLOGIS.ComponentWMS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_iLOGIS.ComponentWMS.Models
{
    public class PickingListWorkOrdersStatus
    {
        public PickingListWorkOrdersStatus()
        {
            GuidCreationDateTime = new DateTime(1900,1,1);
        }
        public int Id { get; set; }
        public int PickingListId { get; set; }
        public int PickerId { get; set; }
        public int WorkOrderId { get; set; }
        public string WorkOrderNumber { get; set; }
        public string ItemCode { get; set; }
        public string Guid { get; set; }

        public int QtyRemain { get; set; }
        public int QtyPlanned { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime GuidCreationDateTime { get; set; }
        public string StartDateStr { get; set; }
        public string StartTimeStr { get; set; }

        public int ResourceId { get; set; }
        public string ResourceName { get; set; }

        public EnumPickingListStatus Status { get; set; }
        public EnumDeliveryListStatus StatusLF { get; set; }
    }
}
