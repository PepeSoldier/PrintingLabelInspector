using MDL_iLOGIS.ComponentWMS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_iLOGIS.ComponentWMS.ViewModels
{
    public class DeliveryListViewModel
    {
        public int Id { get; set; }
        public string ItemCode { get; set; }
        public int WorkorderId { get; set; }
        public string WorkorderNumber { get; set; }
        public bool WorkorderDeleted { get; set; }
        public DateTime StartDateTime { get; set; }
        public string StartDateStr { get; set; }
        public string StartTimeStr { get; set; }
        public string ResourceName { get; set; }
        public decimal Qty { get; set; }
        public int TransporterId { get; set; }
        public string TransporterName { get; set; }
        public EnumDeliveryListStatus Status { get; set; }

        public List<DeliveryListItemViewModel> DeliveryListItems { get; set; }
        public IEnumerable<EnumPickingListStatus> PickingListStatuses { get; set; }
    }
}