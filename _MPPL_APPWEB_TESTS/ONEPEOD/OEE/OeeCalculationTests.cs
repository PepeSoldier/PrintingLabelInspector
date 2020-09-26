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
    public class OeeCalculationTests
    {
        [TestMethod]
        public void CalculateOEEResult()
        {
            //Przygotuj dane
            OeeCalculationModel model = new OeeCalculationModel();
            model.Availability = (decimal)0.95;
            model.Performance = (decimal)0.87;
            model.Quality = (decimal)0.76;

            decimal resultExpected = model.Availability * model.Performance * model.Quality;

            //uruchom test
            model.CalculateResult();

            //sprawdz wyniki
            Assert.AreEqual(model.Result, resultExpected);

        }
        [TestMethod]
        public void CalculateAvailability()
        {
            //Przygotuj dane
            OeeCalculationModel model = new OeeCalculationModel();
            model.GoodProductionTime = 2664; 
            //model.ScrapMaterialTime = (decimal)133.6;
            model.ScrapProcessTime = (decimal)43.2;
            //model.StopUnplannedBreakdownsTime = 956;
            model.StopPlannedTime = 6100;
            model.StopUnplannedTime = 8130;
            model.StopQualityTime = 900;
            model.ShiftTime = 28800;

            //spodziewane wyniki
            //Availability = Run Time / Planned Production Time
            //plannedProductionTime = ShiftTime - StopPlanned => 28800 - 6100 = 22700;
            //runTime = plannedProductionTime - StopUnplanned => 22700 - 8130 = 14570
            decimal resultExpected = (decimal)14570 / (decimal)22700;

            //uruchom test
            model.CalculateAvailability();

            //sprawdz wyniki
            Assert.AreEqual(resultExpected, model.Availability);
        }
        [TestMethod]
        public void CalculatePerformance()
        {
            //Przygotuj dane
            OeeCalculationModel model = new OeeCalculationModel();
            model.GoodProductionTime = 2664;
            //model.ScrapMaterialTime = (decimal)133.6;
            model.ScrapProcessTime = (decimal)43.2;
            //model.StopUnplannedBreakdownsTime = 956;
            model.StopPlannedTime = 6100;
            model.StopUnplannedTime = 8130;
            model.StopQualityTime = 900;
            model.StopPerformanceTime = 900; // jest ignorowany przy wyliczeniu! prawdziwe wyliczenie wynika z wyprodukowanych sztuk * idealby czas cyklu.
            model.ShiftTime = 28800;

            //spodziewane wyniki
            //Performance = (Ideal Cycle Time × Total Count) / Run Time
            //(Ideal Cycle Time × Total Count) => GoodProductionTime + ScrapMaterialTime + ScrapProcessTime
            //plannedProductionTime = ShiftTime - StopPlanned => 28800 - 6100 = 22700;
            //runTime = plannedProductionTime - StopUnplanned => 22700 - 8130 = 14570
            decimal resultExpected = (decimal)2707.2 / (decimal)14570;

            //uruchom test
            model.CalculatePerformance();

            //sprawdz wyniki
            Assert.AreEqual(resultExpected, model.Performance);
        }
        [TestMethod]
        public void CalculateQuality_old()
        {
            //Przygotuj dane
            OeeCalculationModel model = new OeeCalculationModel();
            model.GoodCount = 18;
            model.TotalCount = 22;

            //spodziewane wyniki
            //Quality = Good Count / Total Count
            decimal resultExpected = (decimal)18.0 / (decimal)22.0;

            //uruchom test
            model.CalculateQuality();

            //sprawdz wyniki
            Assert.AreNotEqual(resultExpected, model.Quality);
        }
        [TestMethod]
        public void CalculateQuality_new()
        {
            //Przygotuj dane
            OeeCalculationModel model = new OeeCalculationModel();
            model.GoodProductionTime = 2664;
            //model.ScrapMaterialTime = (decimal)133.6;
            model.ScrapProcessTime = (decimal)43.2;
            //model.StopUnplannedBreakdownsTime = 956;
            model.StopPlannedTime = 6100;
            model.StopUnplannedTime = 8130;
            model.StopQualityTime = 900;
            model.ShiftTime = 28800;

            //spodziewane wyniki
            //totalQualityStopTime = stopQualityTime + scrapProcessTime = 900 + 43.2 = 943.2
            //netRunTime = shiftTime – StopPerformanceTime – StopPlannedTime - StopUnplannedTime = 28800 - 0 - 6100 - 8130 = 14570
            //Q = totalQualityStopTime / netRunTime

            decimal resultExpected = 1 - (decimal)943.2 / (decimal)14570;

            //uruchom test
            model.CalculateQuality();

            //sprawdz wyniki
            Assert.AreEqual(resultExpected, model.Quality);
        }

        //[TestMethod]
        //public void CalculateAvailability_2()
        //{
        //    //Przygotuj dane
        //    OeeCalculationModel model = new OeeCalculationModel();
        //    model.GoodProductionTime = 2664;
        //    model.ScrapMaterialTime = (decimal)133.6;
        //    model.ScrapProcessTime = (decimal)43.2;
        //    model.StopUnplannedBreakdownsTime = 956;
        //    model.StopPlannedChangeOverTime = 800;
        //    model.StopPlannedTime = 6100 - 800;
        //    model.StopUnplannedTime = 8130;
        //    model.ShiftTime = 28800;

        //    //spodziewane wyniki
        //    //Availability = Run Time / Planned Production Time
        //    //plannedProductionTime = ShiftTime - StopPlanned => 28800 - 6100 = 22700;
        //    //runTime = plannedProductionTime - StopUnplanned => 22700 - 8130 - 956 = 13614
        //    decimal resultExpected = (decimal)13614 / (decimal)22700;

        //    //BYŁ BŁĄD:
        //    //plannedProductionTime = ShiftTime - StopOverUnPlanned- - Breakdown => 28800 - 8130 - 956 = 19714
        //    //runTime = plannedProductionTime - StopOverPlanned => 19714 - 6100 = 13614            
        //    //decimal resultExpected = (decimal)13614/(decimal)19714;

        //    //uruchom test
        //    model.CalculateAvailability();

        //    //sprawdz wyniki
        //    Assert.AreEqual(resultExpected, model.Availability);
        //}

    }
}

