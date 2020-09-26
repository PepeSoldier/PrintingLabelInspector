using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_iLOGIS.ComponentWMS.Models
{
    public class ApiLocation
    {
        public string ItemCode { get; set; }
        public string LocationCode { get; set; }
        public DateTime InstantChanged { get; set; }
        public int QtyInBox { get; set; }
        public int QtyReserved { get; set; }
        public string WarehouseCode { get; set; }

        public string RegalNumber { get; set; }
        public int ColumnNumber { get; set; }
        public int ShelfNumber { get; set; }
    }

   
}
