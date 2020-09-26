using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.CORE.ViewModels
{
    public class BarcodeParsedViewModel
    {
        public int StockUnitId { get; set; }
        public string ItemCode { get; set; }
        public decimal Qty { get; set; }
        public string Location { get; set; }
        public string SerialNumber { get; set; }
        public string ErrorText { get; set; }
    }
}