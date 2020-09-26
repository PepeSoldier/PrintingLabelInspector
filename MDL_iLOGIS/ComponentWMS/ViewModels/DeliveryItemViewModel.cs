
using MDL_iLOGIS.ComponentWMS.EntityInterfaces;
using MDLX_MASTERDATA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLIB_COMMON.Enums;

namespace MDL_iLOGIS.ComponentWMS.ViewModels
{
    public class DeliveryItemViewModel : IDeliveryItemFilter
    {
        public int Id { get; set; }
        public int ItemWMSId { get; set; }
        public int DeliveryId { get; set; }
        public string DeliveryGroupGuid { get; set; }
        public int? SupplierId { get; set; }
        public int PackageItemId { get; set; }
        public int LocateAssignedProgress { get; set; }

        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int NumberOfPackages { get; set; }
        public decimal QtyInPackage { get; set; }
        public decimal TotalQty { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }

        public bool AdminEntry { get; set; }
        public bool OperatorEntry { get; set; }
        public bool WasPrinted { get; set; }
        public bool IsLocationAssigned { get; set; }
        public bool IsLocated { get; set; }

        public bool Deleted { get; set; }

        public decimal RemainingQty { get; set; }
        public decimal TotalQtyDocument { get; set; }
        public decimal TotalQtyFound { get; set; }
        public decimal TotalLocatedQty { get; set; }
        public string DestinationLocationName { get; set; }
    }
}
