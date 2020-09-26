using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.ONEPROD.ViewModels.RTV
{
    class RTVOEEChartMinutesViewModel
    {
        public RTVOEEChartMinutesViewModel()
        {
            Date = "";
            CycleTime = 0;
            UsedTime = 0;
            ReasonTypeId = 0;
            EntryType = -1;
            ProdQty = 0;
            Minute = 0;
        }
        public string Date;
        public decimal CycleTime;
        public decimal UsedTime;
        public int ReasonTypeId;
        public int EntryType;
        public decimal ProdQty;
        public int Minute;
        //public string Color;
    }
}