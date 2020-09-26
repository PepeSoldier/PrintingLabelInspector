using MDL_BASE.Models.MasterData;
using MDL_iLOGIS.ComponentWMS.EntityInterfaces;
using MDL_iLOGIS.ComponentWMS.Enums;
using MDLX_MASTERDATA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_iLOGIS.ComponentWMS.ViewModels
{
    public class DeliveryViewModel : IDeliveryFilter //,IDeliveryItemFilter
    {
        public int Id { get; set; }
        public int? SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCode { get; set; }
        public string DocumentNumber { get; set; }
        public string ItemCode { get; set; }
        public string UserName { get; set; }
        public string Guid { get; set; }
        public EnumDeliveryStatus Status { get; set; }
        public int LocateProgress { get; set; }
        public int LocateAssignedProgress { get; set; }

        public DateTime DocumentDate { get; set; }
        public DateTime StampTime { get; set; }

        public decimal TotalItems { get; set; }
        public decimal ItemsCount { get; set; }
    }
}
