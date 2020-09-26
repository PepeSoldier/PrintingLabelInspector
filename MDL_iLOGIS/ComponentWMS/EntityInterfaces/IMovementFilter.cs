using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_iLOGIS.ComponentWMS.EntityInterfaces
{
    public interface IMovementFilter
    {
        int Id { get; set; }
        string ItemCode { get; set; }
        string ItemName { get; set; }
        string SourceAccountingWarehouseName { get; set; }
        string DestinationAccountingWarehouseName { get; set; }
        string SourceLocationName { get; set; }
        string SourceWarehouseName { get; set; }
        string SourceStockUnitSerialNumber { get; set; }
        string DestinationLocationName { get; set; }
        string DestinationWarehouseName { get; set; }
        string DestinationStockUnitSerialNumber { get; set; }
        decimal QtyMoved { get; set; }
        int MovementType { get; set; }
        string UserName { get; set; }
        string MovementDate { get; set; }
    }
}
