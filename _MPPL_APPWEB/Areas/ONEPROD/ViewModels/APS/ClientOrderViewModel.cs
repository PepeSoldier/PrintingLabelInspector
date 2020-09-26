using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.ONEPROD.ViewModels.APS
{
    public class ClientOrderViewModel
    {
        public int Id { get; set; }
        public string ResourceName { get; set; }

        public int? ClientId { get; set; }
        public string ClientName { get; set; }

        public string OrderNo { get; set; }
        public string ItemCode { get; set; }

        public int Qty_Total { get; set; }
        public int Qty_Produced { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int MarginLeft { get; set; }
        public int Width { get; set; }
    }
}