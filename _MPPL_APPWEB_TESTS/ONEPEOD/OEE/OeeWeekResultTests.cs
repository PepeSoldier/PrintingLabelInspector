using MDL_ONEPROD.ComponentOEE.Models;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Model.Scheduling.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _MPPL_APPWEB_TESTS.ONEPEOD.OEE
{
    [TestClass]
    public class OeeWeekResultTests
    {
        [TestMethod]
        public void OeeForDateRangeAvailavilityAverageTest()
        {
            OeeDataOfDateRange oddr = new OeeDataOfDateRange();
            oddr.OeeDataList = new List<OeeDataOfDate>();

            oddr.OeeDataList.Add(new OeeDataOfDate
            {
                Date = new DateTime(2018, 10, 1), OeeCalculatedData = new OeeCalculationModel { Availability = 60 }
            });
            oddr.OeeDataList.Add(new OeeDataOfDate
            {
                Date = new DateTime(2018, 10, 2), OeeCalculatedData = new OeeCalculationModel { Availability = 70}
            });
            oddr.OeeDataList.Add(new OeeDataOfDate
            {
                Date = new DateTime(2018, 10, 3), OeeCalculatedData = new OeeCalculationModel { Availability = 90}
            });

            oddr.CalcResults();
            decimal actual = oddr.AvailabilityAverage; //.AvailabilityAverageResult();
            decimal expected = (decimal)(60 + 70 + 90) / (decimal)3;

            Assert.AreEqual(expected, actual);
        }
    }
}
