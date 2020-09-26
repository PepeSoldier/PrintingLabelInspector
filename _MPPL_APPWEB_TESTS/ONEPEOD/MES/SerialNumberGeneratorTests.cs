using MDL_CORE.ComponentCore.Models;
using MDL_ONEPROD.ComponentMes.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using XLIB_COMMON.Enums;

namespace _MPPL_APPWEB_TESTS.ONEPEOD.MES
{
    [TestClass]
    public class SerialNumberGeneratorTests
    {
        [TestMethod]
        public void FormatSerialNumber_YWWD5()
        {
            System.Globalization.CultureInfo cul = System.Globalization.CultureInfo.CurrentCulture;
            int weekNum = cul.Calendar.GetWeekOfYear(
                DateTime.Now,
                System.Globalization.CalendarWeekRule.FirstFourDayWeek,
                DayOfWeek.Monday); ;

            int value = 6;
            string y = DateTime.Now.Year.ToString().Substring(3, 1);
            string ww = weekNum.ToString("00");

            string formattedSerialNumber = SerialNumberManager.Format(value, SerialNumberType.YWWD5);
            string expectedSerialNumber = y + ww + "0000" + value;

            Assert.AreEqual(expectedSerialNumber, formattedSerialNumber);
        }

        [TestMethod]
        public void FormatSerialNumber_YWWD5_overflow()
        {
            System.Globalization.CultureInfo cul = System.Globalization.CultureInfo.CurrentCulture;
            int weekNum = cul.Calendar.GetWeekOfYear(
                DateTime.Now,
                System.Globalization.CalendarWeekRule.FirstFourDayWeek,
                DayOfWeek.Monday); ;

            int value = 564257;
            string y = DateTime.Now.Year.ToString().Substring(3, 1);
            string ww = weekNum.ToString("00");

            string formattedSerialNumber = SerialNumberManager.Format(value, SerialNumberType.YWWD5);
            string expectedSerialNumber = y + ww + 64257.ToString();

            Assert.AreEqual(expectedSerialNumber, formattedSerialNumber);
        }

        [TestMethod]
        public void FormatSerialNumber_YWWD6()
        {
            System.Globalization.CultureInfo cul = System.Globalization.CultureInfo.CurrentCulture;
            int weekNum = cul.Calendar.GetWeekOfYear(
                DateTime.Now,
                System.Globalization.CalendarWeekRule.FirstFourDayWeek,
                DayOfWeek.Monday); ;

            int value = 6;
            string y = DateTime.Now.Year.ToString().Substring(3, 1);
            string ww = weekNum.ToString("00");

            string formattedSerialNumber = SerialNumberManager.Format(value, SerialNumberType.YWWD6);
            string expectedSerialNumber = y + ww + "00000" + value;

            Assert.AreEqual(expectedSerialNumber, formattedSerialNumber);
        }

        [TestMethod]
        public void FormatSerialNumber_D6()
        {
            int value = 123456789;

            string formattedSerialNumber = SerialNumberManager.Format(value, SerialNumberType.D6);
            string expectedSerialNumber = "456789";

            Assert.AreEqual(expectedSerialNumber, formattedSerialNumber);
        }

        [TestMethod]
        public void FormatSerialNumber_D9()
        {
            int value = 123456789;

            string formattedSerialNumber = SerialNumberManager.Format(value, SerialNumberType.D9);
            string expectedSerialNumber = value.ToString();

            Assert.AreEqual(expectedSerialNumber, formattedSerialNumber);
        }

        [TestMethod]
        public void FormatSerialNumber_D9_length()
        {
            int value = 60;
            string formattedSerialNumber = SerialNumberManager.Format(value, SerialNumberType.D9);
            string expectedSerialNumber = "000000060";

            Assert.AreEqual(expectedSerialNumber, formattedSerialNumber);
        }

        [TestMethod]
        public void FormatSerialNumber_D9_minus()
        {
            int value = -1;
            string formattedSerialNumber = SerialNumberManager.Format(value, SerialNumberType.D9);
            string expectedSerialNumber = "000000000";

            Assert.AreEqual(expectedSerialNumber, formattedSerialNumber);
        }

        [TestMethod]
        public void FormatSerialNumber_D9_overflow()
        {
            int value = 1234567890;

            string formattedSerialNumber = SerialNumberManager.Format(value, SerialNumberType.D9);
            string expectedSerialNumber = 234567890.ToString();

            Assert.AreEqual(expectedSerialNumber, formattedSerialNumber);
        }
    }
}