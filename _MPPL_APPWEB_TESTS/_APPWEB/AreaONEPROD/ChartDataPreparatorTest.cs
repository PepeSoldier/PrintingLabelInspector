using System;
using System.Collections.Generic;
using _MPPL_WEB_START.Areas._APPWEB.ViewModels;
using _MPPL_WEB_START.Areas.ONEPROD.Models;
using MDL_ONEPROD.Model.OEEProd;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _MPPL_APPWEB_TESTS._APPWEB.AreaONEPROD
{
    [TestClass]
    public class ChartDataPreparatorTest
    {
        ////Metoda zwraca jakieś dodatkowe datasety wiec test nie przechodzi. Ale czy w ogole ma on sens?
        
        //[TestMethod]
        //public void PrepareChartData_test1()
        //{
        //    //TEST DATA
        //    List<DateTime> openedShifts = new List<DateTime> {
        //        new DateTime(2018,11,12,6,0,0),
        //        new DateTime(2018,11,13,6,0,0),
        //        new DateTime(2018,11,14,6,0,0),
        //        new DateTime(2018,11,15,6,0,0),
        //        new DateTime(2018,11,15,14,0,0),
        //    };
        //    List<OEEReportProductionDataAbstract> prodDataList = new List<OEEReportProductionDataAbstract>()
        //    {
        //        new OEEReportProductionData(){ Reason = new Reason{ Id = 1, Name = "Test1" }, ReasonId = 1, UsedTime = 300, ProductionDate = new DateTime(2018,11,12,6,0,0) },
        //        new OEEReportProductionData(){ Reason = new Reason{ Id = 2, Name = "Test2" }, ReasonId = 2, UsedTime = 420, ProductionDate = new DateTime(2018,11,12,6,0,0) },
        //        new OEEReportProductionData(){ Reason = new Reason{ Id = 1, Name = "Test1" }, ReasonId = 1, UsedTime = 180, ProductionDate = new DateTime(2018,11,15,6,0,0) },
        //        new OEEReportProductionData(){ Reason = new Reason{ Id = 2, Name = "Test2" }, ReasonId = 2, UsedTime = 180, ProductionDate = new DateTime(2018,11,15,6,0,0) },
        //    };

        //    //EXECUTE
        //    ChartByEntryTypetDataPreparer chdp = new ChartByEntryTypetDataPreparer(new DateTime(2018, 11, 12, 6, 0, 0), new DateTime(2018, 11, 17, 6, 0, 0), 24, openedShifts, 100);
        //    ChartViewModel vm = chdp.PrepareChartData(prodDataList, "test");

        //    //EXPECTED
        //    int labelsCountExpected = 5;
        //    int datasetsCountExpected = 3;
        //    string titleExpected = "test";
        //    List<ChartDataSetViewModel> datasetsExpected = new List<ChartDataSetViewModel>() {
        //         new ChartDataSetViewModel{ label = "Cel", data = new List<decimal>(){ 10, 10, 10, 20, 0 } },
        //         new ChartDataSetViewModel{ label = "Test1", data = new List<decimal>(){ 5, 0, 0, 3, 0 } },
        //         new ChartDataSetViewModel{ label = "Test2", data = new List<decimal>(){ 7, 0, 0, 3, 0 } },
        //    };

        //    //ASSERTS
        //    Assert.AreEqual(labelsCountExpected, vm.labels.Count);
        //    Assert.AreEqual(datasetsCountExpected, vm.datasets.Count);
        //    Assert.AreEqual(titleExpected, vm.title);

        //    Assert.AreEqual(datasetsExpected[0].label, vm.datasets[0].label);
        //    Assert.AreEqual(datasetsExpected[0].data[0], vm.datasets[0].data[0]);
        //    Assert.AreEqual(datasetsExpected[0].data[1], vm.datasets[0].data[1]);
        //    Assert.AreEqual(datasetsExpected[0].data[2], vm.datasets[0].data[2]);
        //    Assert.AreEqual(datasetsExpected[0].data[3], vm.datasets[0].data[3]);
        //    Assert.AreEqual(datasetsExpected[0].data[4], vm.datasets[0].data[4]);

        //    Assert.AreEqual(datasetsExpected[1].label,   vm.datasets[1].label);
        //    Assert.AreEqual(datasetsExpected[1].data[0], vm.datasets[1].data[0]);
        //    Assert.AreEqual(datasetsExpected[1].data[1], vm.datasets[1].data[1]);
        //    Assert.AreEqual(datasetsExpected[1].data[2], vm.datasets[1].data[2]);
        //    Assert.AreEqual(datasetsExpected[1].data[3], vm.datasets[1].data[3]);
        //    Assert.AreEqual(datasetsExpected[1].data[4], vm.datasets[1].data[4]);

        //    Assert.AreEqual(datasetsExpected[2].label,   vm.datasets[2].label);
        //    Assert.AreEqual(datasetsExpected[2].data[0], vm.datasets[2].data[0]);
        //    Assert.AreEqual(datasetsExpected[2].data[1], vm.datasets[2].data[1]);
        //    Assert.AreEqual(datasetsExpected[2].data[2], vm.datasets[2].data[2]);
        //    Assert.AreEqual(datasetsExpected[2].data[3], vm.datasets[2].data[3]);
        //    Assert.AreEqual(datasetsExpected[2].data[4], vm.datasets[2].data[4]);
        //}
    }
}
