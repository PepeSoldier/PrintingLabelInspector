using _MPPL_WEB_START.Areas._APPWEB.ViewModels;
using MDL_ONEPROD.ComponentENERGY.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.ONEPROD.Models
{
    public class OEEEnergyReport
    {
        public OEEEnergyReport()
        {

        }
        public decimal TotalCost { get; set; }
        public decimal PricePerProductionUnit { get; set; }
        public decimal UsePerProductionUnit { get; set; }
        public EnergyTypeData ElectricityMetersData { get; set; }
        public EnergyTypeData WaterMetersData { get; set; }
        public EnergyTypeData GasMetersData { get; set; }
        public EnergyTypeData HeatMetersData { get; set; }

        public ChartViewModel ChartPricePerUnit { get; set; }
        public ChartViewModel ChartTotalCostByType { get; set; }
        public ChartViewModel ChartUseEnergyPerUnit { get; set; }
        public ChartViewModel ChartEnergyConsumption { get; set; }
    }
}