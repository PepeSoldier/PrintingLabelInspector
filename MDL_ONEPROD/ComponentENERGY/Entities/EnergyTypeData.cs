using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.ComponentENERGY.Entities
{
    public class EnergyTypeData
    {
        public EnergyTypeData()
        {

        }
        public decimal Qty { get; set; }
        public decimal QtyPerUnit { get; set; }
        public decimal Percentage { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal TotalValue { get; set; }
    }
}