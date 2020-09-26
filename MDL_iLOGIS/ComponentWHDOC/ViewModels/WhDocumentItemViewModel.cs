using MDL_BASE.Models.MasterData;
using MDL_iLOGIS.ComponentWMS.EntityInterfaces;
using MDLX_MASTERDATA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLIB_COMMON.Enums;

namespace MDL_iLOGIS.ComponentWHDOC.ViewModels
{
    public class WhDocumentItemViewModel
    {
        public int? Id { get; set; }

        public int? ItemWMSId { get; set; }
        public int? PackageId{ get; set; }

        public int? WhDocumentId { get; set; }

        public string ItemCode { get; set; }
        public string ItemName { get; set; }

        
        public decimal DisposedQty { get; set; }
        public decimal IssuedQty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Value { get; set; }

        public UnitOfMeasure UnitOfMeasure { get; set; }

        public bool Deleted { get; set; }

        public override string ToString()
        {
            return Id.ToString() + ". " + ItemCode + "-" + ItemName + ": " + IssuedQty.ToString();
        }
    }
}
