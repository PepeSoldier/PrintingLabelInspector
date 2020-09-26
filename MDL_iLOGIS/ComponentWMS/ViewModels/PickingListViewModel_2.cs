using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_iLOGIS.ComponentWMS.Models;
using MDLX_CORE.ComponentCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_iLOGIS.ComponentWMS.ViewModels
{
    public class PickingListViewModel_2
    {
        public PickingListViewModel_2()
        {
            PickingListItems = new List<PickingListItemViewModel_2>();
            PlatformList = new List<PickingListPlatformViewModel>();
            ProductionOrderList = new List<ProductionOrder>();
            IsDataNull = true;
        }

        public int PickingListStatus { get; set; }

        public int NumberToPick { get; set; }
        public string WorkOrderNo { get; set; }
        public bool IsDataNull { get; set; }
        public string PickingListGuid { get; set; }
        public string PncCode { get; set; }
        public string ResourceName { get; set; }
        public int PncQtyRemaining { get; set; }
        public List<PickingListItemViewModel_2> PickingListItems;
        public List<PickingListPlatformViewModel> PlatformList;
        public List<ProductionOrder> ProductionOrderList;

    }


    public class PickingListPlatformViewModel
    {
        public int PlatformId { get; set; }
        public string PlatformName { get; set; }

        public string PlatformLocationName { get; set; }
    }

}
