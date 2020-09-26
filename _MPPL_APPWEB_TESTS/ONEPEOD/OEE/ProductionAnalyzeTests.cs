using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDL_ONEPROD.Model.Scheduling.Interface;
using MDL_ONEPROD.ComponentOEE.Models;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Model.Scheduling;

namespace _MPPL_APPWEB_TESTS.ONEPEOD.OEE
{
    [TestClass]
    public class ProductionAnalyzeTests
    {
        [TestMethod]
        public void ProductionQtyToTime()
        {
            //przygotuj dane
            ReasonType prod = new ReasonType() { EntryType = EnumEntryType.Production };

            List<OEEReportProductionDataAbstract> productionData = new List<OEEReportProductionDataAbstract>();
            productionData.Add(new OEEReportProductionData { ProdQty = 80, CycleTime = 0, ReasonType = prod });
            productionData.Add(new OEEReportProductionData { ProdQty = 120, CycleTime = 15, ReasonType = prod });
            productionData.Add(new OEEReportProductionData { ProdQty = 90, CycleTime = (decimal)9.6, ReasonType = prod });

            //wykonaj test
            ProductionAnalyzeModel model = new ProductionAnalyzeModel();
            decimal ts = model.ProductionQtyToTime(productionData.ToList<OEEReportProductionDataAbstract>(), EnumEntryType.Production);

            //Expected Values
            decimal tsExpected = 2664;

            //zrob asserty
            Assert.AreEqual(tsExpected, ts);
        }
        [TestMethod]
        public void ProductionQtyToTime2()
        {
            //przygotuj dane
            ReasonType prod = new ReasonType() { EntryType = EnumEntryType.Production };

            List<OEEReportProductionDataAbstract> productionData = new List<OEEReportProductionDataAbstract>();
            productionData.Add(new OEEReportProductionData { ProdQty = 80, CycleTime = 0, ReasonType = prod });
            productionData.Add(new OEEReportProductionData { ProdQty = 120, CycleTime = 15, ReasonType = prod });
            productionData.Add(new OEEReportProductionData { ProdQty = 9, CycleTime = (decimal)9.6, ReasonType = prod });

            //wykonaj test
            ProductionAnalyzeModel model = new ProductionAnalyzeModel();
            decimal ts = model.ProductionQtyToTime(productionData.ToList<OEEReportProductionDataAbstract>(), EnumEntryType.Production);

            //Expected Values
            decimal tExpected = (decimal)1886.4;

            //zrob asserty
            Assert.AreEqual(tExpected, ts);
        }
        [TestMethod]
        public void AnalyzeTimes()
        {
            //przygotuj dane
            List<OEEReportProductionDataAbstract> productionData = new List<OEEReportProductionDataAbstract>();

            ReasonType prod = new ReasonType() { EntryType = EnumEntryType.Production };
            ReasonType scrapMat = new ReasonType() { EntryType = EnumEntryType.ScrapMaterial };
            ReasonType scrapProc = new ReasonType() { EntryType = EnumEntryType.ScrapProcess };
            ReasonType stopPlanned = new ReasonType() { EntryType = EnumEntryType.StopPlanned };
            ReasonType stopUnplanned = new ReasonType() { EntryType = EnumEntryType.StopUnplanned };
            ReasonType stopQuality = new ReasonType() { EntryType = EnumEntryType.StopQuality };
            

            productionData.Add(new OEEReportProductionData { ProdQty = 80, CycleTime = 0, ReasonType = prod });
            productionData.Add(new OEEReportProductionData { ProdQty = 120, CycleTime = 15, ReasonType = prod });
            productionData.Add(new OEEReportProductionData { ProdQty = 90, CycleTime = (decimal)9.6, ReasonType = prod });
            productionData.Add(new OEEReportProductionData { ProdQty = 10, CycleTime = (decimal)7.6, ReasonType = scrapMat });
            productionData.Add(new OEEReportProductionData { ProdQty = 8, CycleTime = (decimal)7.2, ReasonType = scrapMat });
            productionData.Add(new OEEReportProductionData { ProdQty = 12, CycleTime = (decimal)3.6, ReasonType = scrapProc });
            //productionData.Add(new OEEReportProductionData { UsedTime = 378, EntryType = EnumEntryType.StopUnplannedBreakdown });
            //productionData.Add(new OEEReportProductionData { UsedTime = 578, EntryType = EnumEntryType.StopUnplannedBreakdown });
            productionData.Add(new OEEReportProductionData { UsedTime = 2500, ReasonType = stopPlanned });
            productionData.Add(new OEEReportProductionData { UsedTime = 2100, ReasonType = stopPlanned });
            productionData.Add(new OEEReportProductionData { UsedTime = 1500, ReasonType = stopPlanned });
            productionData.Add(new OEEReportProductionData { UsedTime = 4500, ReasonType = stopUnplanned });
            productionData.Add(new OEEReportProductionData { UsedTime = 3400, ReasonType = stopUnplanned });
            productionData.Add(new OEEReportProductionData { UsedTime = 230, ReasonType = stopUnplanned });
            productionData.Add(new OEEReportProductionData { UsedTime = 300, ReasonType = stopQuality });
            //productionData.Add(new OEEReportProductionData { UsedTime = 400, EntryType = EnumEntryType.StopUnplannedChangeOver });
            //productionData.Add(new OEEReportProductionData { UsedTime = 800, EntryType = EnumEntryType.StopUnplannedChangeOver });

            //wykonaj test
            ProductionAnalyzeModel model = new ProductionAnalyzeModel();
            //decimal tsBreakDown = model.ProductionQtyToTime(productionData, EnumEntryType.StopUnplannedBreakdown);
            decimal tsProduction = model.ProductionQtyToTime(productionData, EnumEntryType.Production);
            decimal tsScrapMaterial = model.ProductionQtyToTime(productionData, EnumEntryType.ScrapMaterial);
            decimal tsScrapProcess = model.ProductionQtyToTime(productionData, EnumEntryType.ScrapProcess);
            decimal tsStopOverPlanned = model.ProductionQtyToTime(productionData, EnumEntryType.StopPlanned);
            decimal tsStopOverUnPlanned = model.ProductionQtyToTime(productionData, EnumEntryType.StopUnplanned);
            decimal tsStopQuality = model.ProductionQtyToTime(productionData, EnumEntryType.StopQuality);
            //decimal tsStopOverUnPlannedChangeOver = model.ProductionQtyToTime(productionData, EnumEntryType.StopUnplannedChangeOver);

            //Expected Values
            decimal ProductionTime = 2664;
            //decimal ScrapMaterialTime = (decimal)133.6;
            decimal ScrapProcessTime = (decimal)43.2;
            //decimal Breakdown = 956;
            decimal StopOverPlanned = 6100;
            decimal StopOverUnPlanned = 8130;
            decimal StopQuality = 300;
            //decimal StopOverUnPlannedChangeOver = 1200;

            //zrob asserty
            //Assert.AreEqual(tsBreakDown, Breakdown);
            Assert.AreEqual(ProductionTime, tsProduction);
            //Assert.AreEqual(ScrapMaterialTime, tsScrapMaterial);
            Assert.AreEqual(ScrapProcessTime, tsScrapProcess);
            Assert.AreEqual(StopOverPlanned, tsStopOverPlanned);
            Assert.AreEqual(StopOverUnPlanned, tsStopOverUnPlanned);
            Assert.AreEqual(StopQuality, tsStopQuality);
            //Assert.AreEqual(tsStopOverUnPlannedChangeOver, StopOverUnPlannedChangeOver);
        }
        [TestMethod]
        public void CalculateGoodCount()
        {
            //przygotuj dane

            ReasonType prod = new ReasonType() { EntryType = EnumEntryType.Production };
            ReasonType scrapMat = new ReasonType() { EntryType = EnumEntryType.ScrapMaterial };
            ReasonType scrapProc = new ReasonType() { EntryType = EnumEntryType.ScrapProcess };

            List<OEEReportProductionDataAbstract> productionData = new List<OEEReportProductionDataAbstract>();
            productionData.Add(new OEEReportProductionData { ProdQty = 75, ReasonTypeEntryType = EnumEntryType.Production, ReasonType = prod });
            productionData.Add(new OEEReportProductionData { ProdQty = 10, ReasonTypeEntryType = EnumEntryType.ScrapMaterial, ReasonType = scrapMat });
            productionData.Add(new OEEReportProductionData { ProdQty = 8,  ReasonTypeEntryType = EnumEntryType.ScrapMaterial, ReasonType = scrapMat });
            productionData.Add(new OEEReportProductionData { ProdQty = 98, ReasonTypeEntryType = EnumEntryType.Production, ReasonType = prod });
            productionData.Add(new OEEReportProductionData { ProdQty = 12, ReasonTypeEntryType = EnumEntryType.ScrapProcess, ReasonType = scrapProc });
            productionData.Add(new OEEReportProductionData { ProdQty = 90, ReasonTypeEntryType = EnumEntryType.Production, ReasonType = prod });
            //podaj spodziewane wyniki
            int expVal = 263;

            //wykonaj dunkcje
            ProductionAnalyzeModel model = new ProductionAnalyzeModel();
            int val = (int)model.GoodQtyCount(productionData);

            //asertuj
            Assert.AreEqual(expVal, val);
        }
        [TestMethod]
        public void CalculateTotalCount()
        {
            //przygotuj dane
            List<OEEReportProductionDataAbstract> productionData = new List<OEEReportProductionDataAbstract>();
            productionData.Add(new OEEReportProductionData { ProdQty = 75, ReasonTypeEntryType = EnumEntryType.Production });
            productionData.Add(new OEEReportProductionData { ProdQty = 10, ReasonTypeEntryType = EnumEntryType.ScrapMaterial });
            productionData.Add(new OEEReportProductionData { ProdQty = 8, ReasonTypeEntryType = EnumEntryType.ScrapMaterial });
            productionData.Add(new OEEReportProductionData { ProdQty = 98, ReasonTypeEntryType = EnumEntryType.Production });
            productionData.Add(new OEEReportProductionData { ProdQty = 12, ReasonTypeEntryType = EnumEntryType.ScrapProcess });
            productionData.Add(new OEEReportProductionData { ProdQty = 90, ReasonTypeEntryType = EnumEntryType.Production });
            //podaj spodziewane wyniki
            int expVal = 293;

            //wykonaj dunkcje
            ProductionAnalyzeModel model = new ProductionAnalyzeModel();
            int val = (int)model.TotalQtyCount(productionData);

            //asertuj
            Assert.AreEqual(expVal, val);
        }
    }
}
