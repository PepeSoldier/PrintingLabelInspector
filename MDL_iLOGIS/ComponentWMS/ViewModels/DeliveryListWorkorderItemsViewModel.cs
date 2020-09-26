using MDL_iLOGIS.ComponentWMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_iLOGIS.ComponentWMS.ViewModels
{
    public class DeliveryListWorkorderItemsViewModel
    {
        public int WoId { get; set; }
        public List<DeliveryListItemViewModel> Items { get; set; }
        public DeliveryListDataStatus DataStatus { get; set; }
    }

    public class DeliveryListWorkorderItemsListViewModel
    {
        public List<DeliveryListWorkorderItemsViewModel> WorkorderItems { get; set; }
    }

    public enum DeliveryListDataStatus
    {
        DataOK = 0,
        DataFixed = 1,
        DataSubstitute = 2,
    }
}