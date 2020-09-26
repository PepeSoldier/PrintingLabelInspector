using MDL_ONEPROD.ComponentENERGY.Entities;
using MDL_ONEPROD.Model.OEEProd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.ComponentENERGY.Models
{
    public class EnergyAnalyzeModel
    {
        public EnergyAnalyzeModel(List<EnergyConsumptionData> energyData, List<OEEReportProductionData> prodDataRepo)
        {
            this.EnergyData = energyData;
            this.ProdDataRepo = prodDataRepo;
            GetEnergyUnitsQty();
            GetProductionQty();
        }

        protected List<OEEReportProductionData> ProdDataRepo { get; set; }
        protected List<EnergyConsumptionData> EnergyData { get; set; }
        protected decimal ProductionQty { get; set; }
        protected decimal EnergyUnitsQty { get; set; }
        
        public void GetEnergyUnitsQty()
        {
            decimal? temp = EnergyData.Sum(x => x.Qty);
            if(temp == null)
            {
                EnergyUnitsQty = 0;
            }
            else
            {
                EnergyUnitsQty = (decimal)temp;
            }
            
        }

        public void GetProductionQty()
        {
            decimal? temp = ProdDataRepo.Sum(x => x.ProdQty);
            if (temp == null)
            {
                ProductionQty = 0;
            }
            else
            {
                ProductionQty = (decimal)temp;
            }

        }

        public decimal GetProductionQty(DateTime dateFrom, DateTime dateTo)
        {
            decimal dataReturn = 0;
            decimal ProdQtyPeriod = ProdDataRepo.Where(x => x.ProductionDate >= dateFrom && x.ProductionDate < dateTo).Sum(x => x.ProdQty);
            if (ProdQtyPeriod == 0)
            {
                dataReturn = 0;
            }
            else
            {
                dataReturn = (decimal)ProdQtyPeriod;
            }
            return dataReturn;
        }

        public decimal TotalCostOnMachine()
        {
            decimal dataReturn = 0;
            dataReturn = (decimal)EnergyData.Sum(x => x.TotalValue != null ? x.TotalValue : 0);
            return dataReturn;
        }

        public decimal TotalCostByEnergyType(EnumEnergyType enumEnergyType)
        {
            decimal dataReturn = 0;
            dataReturn = (decimal)EnergyData.Where(x => x.EnergyMeter.EnergyType == enumEnergyType).Sum(x => x.TotalValue != null ? x.TotalValue : 0);
            return dataReturn;
        }

        public decimal? TotalCostByEnergyType(EnumEnergyType enumEnergyType, DateTime dateFrom, DateTime dateTo)
        {
            decimal? dataReturn = 0;
            dataReturn = EnergyData.Where( x=> x.DateFrom >= dateFrom && x.DateTo < dateTo && x.EnergyMeter.EnergyType == enumEnergyType).Sum(x => (decimal?)x.TotalValue);
            if (dataReturn == null)
            {
                dataReturn = 0;
            }
            return dataReturn;
        }

        public decimal UsePerUnitByEnergyType(EnumEnergyType enumEnergyType)
        {
            decimal dataReturn = 0;
            if (ProductionQty == 0)
            {
                dataReturn = EnergyData.Where(x => x.EnergyMeter.EnergyType == enumEnergyType).Sum(x => x.Qty);
            }
            else
            {
                dataReturn = EnergyData.Where(x => x.EnergyMeter.EnergyType == enumEnergyType).Sum(x => x.Qty) / ProductionQty;
            }
            return dataReturn;
        }

        public decimal PricePerProductionUnit(DateTime dateFrom, DateTime dateTo)
        {
            decimal ProdQtyPeriod = 0;
            decimal dataReturn = 0;
            ProdQtyPeriod = ProdDataRepo.Where(x => x.ProductionDate >= dateFrom && x.ProductionDate < dateTo).Sum(x => x.ProdQty);
            if (ProdQtyPeriod == 0)
            {
                //Client wants that, when there is no production then price per cost must be equal 0
                //dataReturn = (decimal)EnergyData.Where(x => x.DateFrom >= dateFrom && x.DateTo < dateTo).Sum(x => x.TotalValue != null ? x.TotalValue : 0);
                dataReturn = 0;
            }
            else
            {
                dataReturn = (decimal)EnergyData.Where(x => x.DateFrom >= dateFrom && x.DateTo < dateTo).Sum(x => x.TotalValue != null ? x.TotalValue : 0) / ProdQtyPeriod;
            }
            return dataReturn;
        }

        public decimal UsePerUnit()
        {
            decimal dataReturn = 0;
            if (ProductionQty == 0)
            {
                dataReturn = EnergyData.Sum(x => x.Qty);
            }
            else
            {
                dataReturn = EnergyData.Sum(x => x.Qty) / ProductionQty;
            }
            return dataReturn;
        }


        public EnergyTypeData SetDataForEnergyMeter(EnumEnergyType enumEnergyType)
        {
            EnergyTypeData energyTypeData = new EnergyTypeData();
            energyTypeData.Qty = EnergyData.Where(x => x.EnergyMeter.EnergyType == enumEnergyType).Sum(x => x.Qty);
            energyTypeData.Percentage = EnergyUnitsQty == 0 ? 0 : energyTypeData.Qty / EnergyUnitsQty * 100;
            energyTypeData.QtyPerUnit = EnergyUnitsQty == 0 ? energyTypeData.Qty : energyTypeData.Qty / EnergyUnitsQty;
            energyTypeData.TotalValue = (decimal)EnergyData.Where(x => x.EnergyMeter.EnergyType == enumEnergyType).Sum(x => x.TotalValue != null ? x.TotalValue : 0);
            energyTypeData.PricePerUnit = energyTypeData.Qty == 0 ? 0 : energyTypeData.TotalValue / energyTypeData.Qty;

            return energyTypeData;
        }

    }
}