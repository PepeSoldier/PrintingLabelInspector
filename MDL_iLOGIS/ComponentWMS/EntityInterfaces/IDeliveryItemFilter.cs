using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_iLOGIS.ComponentWMS.EntityInterfaces
{
    public interface IDeliveryItemFilter
    {
       int DeliveryId { get; set; }
       string ItemCode { get; set; }
    }
}
