using MDL_BASE.Models.MasterData;
using MDLX_CORE.ComponentCore.Entities;
using MDL_PRD.Model;
using MDLX_MASTERDATA.Entities;
using System.Collections.Generic;

namespace MDL_PRD.ViewModel
{
    public class SubAssViewModel
    {
        public List<Resource2> Lines { get; set; }
        public List<ProductionOrder> ProductionOrders { get; set; }
    }
}