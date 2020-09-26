using MDL_BASE.Models.IDENTITY;
using MDL_iLOGIS.ComponentWMS.EntityInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLIB_COMMON.Enums;

namespace MDL_iLOGIS.ComponentWMS.ViewModels
{
    public class MovementHistoryViewModel : IMovementFilter
    {
        public int Id { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string SourceAccountingWarehouseCode { get; set; }
        public string SourceAccountingWarehouseName { get; set; }
        public string SourceWarehouseName { get; set; }
        public string SourceLocationName { get; set; }
        public string SourceStockUnitSerialNumber { get; set; }
        public string DestinationAccountingWarehouseCode { get; set; }
        public string DestinationAccountingWarehouseName { get; set; }
        public string DestinationWarehouseName { get; set; }
        public string DestinationLocationName { get; set; }
        public string DestinationStockUnitSerialNumber { get; set; }
        public decimal QtyMoved { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }
        public int MovementType { get; set; }
        public string UserName{ get; set; }
        public string MovementDate { get; set; }
    }
}
