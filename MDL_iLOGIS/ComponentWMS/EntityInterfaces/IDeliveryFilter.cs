using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_iLOGIS.ComponentWMS.EntityInterfaces
{
    public interface IDeliveryFilter
    {
        int Id { get; set; }
        int? SupplierId { get; set; }
        string SupplierCode { get; set; }
        string SupplierName { get; set; }
        string DocumentNumber { get; set; }
        string ItemCode { get; set; }
        string UserName { get; set; }
        string Guid { get; set; }
        DateTime DocumentDate { get; set; }
        DateTime StampTime { get; set; }
    }
}
