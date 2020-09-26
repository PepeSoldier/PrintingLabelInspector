using System;
using System.Collections.Generic;
using MDLX_MASTERDATA.Entities;
using MDLX_MASTERDATA.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XLIB_COMMON.Enums;

namespace _MPPL_APPWEB_TESTS.MASTERDATA
{
    [TestClass]
    public class ConverterUnitOfMeasureTest
    {
        [TestMethod]
        public void Convert_Test_Standard_1()
        {
            decimal g = ConverterUoM.Convert(1, UnitOfMeasure.kg, UnitOfMeasure.g);
            decimal kg = ConverterUoM.Convert(1, UnitOfMeasure.g, UnitOfMeasure.kg);

            Assert.AreEqual(1000, g);
            Assert.AreEqual(0.001m, kg);
        }
        [TestMethod]
        public void Convert_Test_Standard_2()
        {
            decimal cm = ConverterUoM.Convert(1, UnitOfMeasure.m, UnitOfMeasure.cm);

            Assert.AreEqual(100, cm);
        }
        [TestMethod]
        public void Convert_Test_Standard_3()
        {
            decimal cm = ConverterUoM.Convert(2, UnitOfMeasure.FT, UnitOfMeasure.cm);

            Assert.AreEqual(60.96m, cm);
        }

        [TestMethod]
        public void Convert_Test_Reversed_1()
        {
            decimal kg = ConverterUoM.Convert(1, UnitOfMeasure.g, UnitOfMeasure.kg);

            Assert.AreEqual(0.001m, kg);
        }
        [TestMethod]
        public void Convert_Test_Reversed_2()
        {
            decimal m = ConverterUoM.Convert(1000, UnitOfMeasure.cm, UnitOfMeasure.m);

            Assert.AreEqual(10, m);
        }

        [TestMethod]
        public void Convert_Test_Combined_1()
        {
            decimal mm = ConverterUoM.Convert(10, UnitOfMeasure.cm, UnitOfMeasure.mm);

            Assert.AreEqual(100, mm);
        }
        [TestMethod]
        public void Convert_Test_Combined_2()
        {
            decimal mm = ConverterUoM.Convert(2, UnitOfMeasure.FT, UnitOfMeasure.mm);

            Assert.AreEqual(609.6m, mm);
        }

        [TestMethod]
        public void GetAlternativeUnitOfMeasures_1()
        {
           List<UnitOfMeasure> UoMs = ConverterUoM.AlternativeUnitOfMeasures(UnitOfMeasure.m);
            
            Assert.IsTrue(UoMs.Contains(UnitOfMeasure.cm));
            Assert.IsTrue(UoMs.Contains(UnitOfMeasure.mm));
        }
        [TestMethod]
        public void GetAlternativeUnitOfMeasures_2()
        {
            List<UnitOfMeasure> UoMs = ConverterUoM.AlternativeUnitOfMeasures(UnitOfMeasure.cm);

            Assert.IsTrue(UoMs.Contains(UnitOfMeasure.m));
            Assert.IsTrue(UoMs.Contains(UnitOfMeasure.mm));
        }

        [TestMethod]
        public void Convert_Test_ItemUoMs_Standard_1()
        {
            List<ItemUoM> itemUoMs = new List<ItemUoM>();
            itemUoMs.Add(new ItemUoM() { DefaultUnitOfMeasure = UnitOfMeasure.szt, QtyForDefaultUnitOfMeasure = 1, AlternativeUnitOfMeasure = UnitOfMeasure.m, QtyForAlternativeUnitOfMeasure = 66 });

            decimal m = ConverterUoM.Convert(10, UnitOfMeasure.szt, UnitOfMeasure.m, itemUoMs);

            Assert.AreEqual(660, m);
        }

        [TestMethod]
        public void Convert_Test_ItemUoMs_Reversed_1()
        {
            List<ItemUoM> itemUoMs = new List<ItemUoM>();
            itemUoMs.Add(new ItemUoM() { DefaultUnitOfMeasure = UnitOfMeasure.szt, QtyForDefaultUnitOfMeasure = 1, AlternativeUnitOfMeasure = UnitOfMeasure.m, QtyForAlternativeUnitOfMeasure = 66 });

            decimal szt = ConverterUoM.Convert(132, UnitOfMeasure.m, UnitOfMeasure.szt, itemUoMs);

            Assert.AreEqual(2, szt);
        }

    }
}
