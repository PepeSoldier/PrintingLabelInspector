using _MPPL_WEB_START.Areas.ONEPROD.ViewModels.APS;
using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.ONEPROD.APS.ViewModels
{
    public class ClientOrderResourceViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<ClientOrderViewModel> ClientOrders { get; set; }
    }
}