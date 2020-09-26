using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using XLIB_COMMON.Model;

namespace _MPPL_APPWEB_TESTS.XLIB_COMMON.Models
{
    [TestClass]
    public class BarcodeParserTest
    {
        [TestMethod]
        public void Test_ParseCSQD()
        {
            string template = "CCCCSSSSQQQDDD";
            string barcode = "17501002001003";

            BarcodeManager barcodeParser = new BarcodeManager();
            barcodeParser.Parse(barcode, template);

            Assert.AreEqual("1750", barcodeParser.ItemCode);
            Assert.AreEqual("1002", barcodeParser.SerialNumber);
            Assert.AreEqual(1.003m, barcodeParser.Qty);
        }

        [TestMethod]
        public void Test_ParseCSQDL()
        {
            string template = "CCCCCCCCC-SSSSSS-QQQDDD-LL-LL-LLLL";
            string barcode =  "156900100-100201-999888-01-02-2200";

            BarcodeManager barcodeParser = new BarcodeManager();
            barcodeParser.Parse(barcode, template);

            Assert.AreEqual("156900100", barcodeParser.ItemCode);
            Assert.AreEqual("100201", barcodeParser.SerialNumber);
            Assert.AreEqual(999.888m, barcodeParser.Qty);
            Assert.AreEqual("01-02-2200", barcodeParser.Location);
        }

        [TestMethod]
        public void Test_ParseCSQDLYYYYMd()
        {
            string template = "CCCCCCCCC-SSSSSS-QQQDDD-LL-LL-LLLL-YYYY-MM-dd";
            string barcode = "156900100-100201-999888-01-02-2200-2020-12-12";

            BarcodeManager barcodeParser = new BarcodeManager();
            barcodeParser.Parse(barcode, template);

            Assert.AreEqual("156900100", barcodeParser.ItemCode);
            Assert.AreEqual("100201", barcodeParser.SerialNumber);
            Assert.AreEqual(999.888m, barcodeParser.Qty);
            Assert.AreEqual("01-02-2200", barcodeParser.Location);
            Assert.AreEqual("2020-12-12", barcodeParser.DateTime.ToString("yyyy-MM-dd"));
        }

        [TestMethod]
        public void Test_ParseCSQDLYYMdHms()
        {
            string template = "CCCCCCCCC-SSSSSS-QQQDDD-LL-LL-LLLL-YY-MM-dd-HHmmss";
            string barcode = "156900100-100201-999888-01-02-2200-21-12-12-182545";

            BarcodeManager barcodeParser = new BarcodeManager();
            barcodeParser.Parse(barcode, template);

            Assert.AreEqual("156900100", barcodeParser.ItemCode);
            Assert.AreEqual("100201", barcodeParser.SerialNumber);
            Assert.AreEqual(999.888m, barcodeParser.Qty);
            Assert.AreEqual("01-02-2200", barcodeParser.Location);
            Assert.AreEqual("2021-12-12 18:25:45", barcodeParser.DateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        [TestMethod]
        public void Test_ParsePLB()
        {
            string template = "CCCCCCCCCCCCQQQQQQDDDDDLLLLLLLLLSSSSSSSSSSSS";
            string barcode =  "####0232038700002000000#External###018000007";

            BarcodeManager barcodeParser = new BarcodeManager();
            barcodeParser.Parse(barcode, template);

            Assert.AreEqual("02320387", barcodeParser.ItemCode);
            Assert.AreEqual("018000007", barcodeParser.SerialNumber);
            Assert.AreEqual(20, barcodeParser.Qty);
            Assert.AreEqual("External", barcodeParser.Location);
        }


        [TestMethod]
        public void Test_ParseUnexpectedQtyChar()
        {
            string template = "CCCCCCCCC-SSSSSS-QQQDDD-LL-LL-LLLL";
            string barcode = "156900100-100201-R02001-01-02-2200";

            BarcodeManager barcodeParser = new BarcodeManager();
            barcodeParser.Parse(barcode, template);

            Assert.AreEqual(true, barcodeParser.ErrorUnexpectedQtyChar);
        }

        [TestMethod]
        public void Test_ParseWrongLength()
        {
            string template = "CCCCCCCCC-SSSSSS-QQQDDD-LL-LL-LLLL";
            string barcode = "156900100-100201-002001-01-02-220";

            BarcodeManager barcodeParser = new BarcodeManager();
            barcodeParser.Parse(barcode, template);

            Assert.AreEqual(true, barcodeParser.ErrorWrongLength);
        }

        [TestMethod]
        public void Test_ParseReapeatedChar()
        {
            string template = "CCCCCCCCC-SSSSSS-QQQDDD-LL-LL-LLLL-YYMMDD";
            string barcode = "156900100-100201-002001-01-02-2200-200409";

            BarcodeManager barcodeParser = new BarcodeManager();
            barcodeParser.Parse(barcode, template);

            Assert.AreEqual(true, barcodeParser.ErrorReapeatedChar);
        }

        [TestMethod]
        public void Test_ParseTrimSerialNumberZeros()
        {
            string template = "CCCCCCCCCSSSSSSSSLLLLLLLLQQQQDDD";
            string barcode =  "A11919510086236930000T0030001000";

            BarcodeManager barcodeParser = new BarcodeManager();
            barcodeParser.Parse(barcode, template);

            Assert.AreEqual("8623693", barcodeParser.SerialNumber);
        }

        [TestMethod]
        public void Test_Parse_10()
        {
            string template = "10";
            string barcode = "10";

            BarcodeManager barcodeParser = new BarcodeManager();
            barcodeParser.Parse(barcode, template);

            Assert.AreEqual("", barcodeParser.ItemCode);
            Assert.AreEqual("", barcodeParser.SerialNumber);
            Assert.AreEqual(0, barcodeParser.Qty);
            Assert.AreEqual("", barcodeParser.Location);
        }
        [TestMethod]
        public void Test_Parse_Random()
        {
            string template = "FG20#$-E98";
            string barcode = "1234567890";

            BarcodeManager barcodeParser = new BarcodeManager();
            barcodeParser.Parse(barcode, template);

            Assert.AreEqual("", barcodeParser.ItemCode);
            Assert.AreEqual("", barcodeParser.SerialNumber);
            Assert.AreEqual(0, barcodeParser.Qty);
            Assert.AreEqual("", barcodeParser.Location);
        }

        [TestMethod]
        public void Test_Generate_CSLQD()
        {
            string rawSerialNo = "725";
            string template = "CCCCCCCCCSSSSSSSSLLLLLLLLQQQQDDD";
            //expected--------"A00200101000007280000T0030041000";

            BarcodeManager barcodeParser = new BarcodeManager();
            barcodeParser.Location = "T003";
            barcodeParser.ItemCode = "A00200101";
            barcodeParser.Qty = 41;
            barcodeParser.Generate(rawSerialNo, template);

            Assert.AreEqual("A00200101#####725####T0030041000", barcodeParser.Barcode);
            //--------------"CCCCCCCCCSSSSSSSSLLLLLLLLQQQQDDD"
            //--------------"0041000A002001010000T003"
        }

        [TestMethod]
        public void Test_Generate_CSLQDYMdHms()
        {
            string rawSerialNo = "725";
            string template = "CCCCCCCCCYYMMddHHmmssSSSSSSSSLLLLLLLLQQQQDDD";
            //expected--------"A00200101200409165904000007280000T0030041000";

            BarcodeManager barcodeParser = new BarcodeManager();
            barcodeParser.Location = "T003";
            barcodeParser.ItemCode = "A00200101";
            barcodeParser.Qty = 41;
            barcodeParser.DateTime = Convert.ToDateTime("2020-04-09 16:59:04");
            barcodeParser.Generate(rawSerialNo, template);

            Assert.AreEqual("A00200101200409165904#####725####T0030041000", barcodeParser.Barcode);
            //--------------"CCCCCCCCCYYMMddHHmmssSSSSSSSSLLLLLLLLQQQQDDD
            //--------------"A00200101010101000000000007250000T0030041000
        }
        [TestMethod]
        public void Test_Generate_PLB()
        {
            string rawSerialNo = "018000007";
            string template = "CCCCCCCCCCCCQQQQQQDDDDDLLLLLLLLLSSSSSSSSSSSS";
            //expected--------"A00200101200409165904000007280000T0030041000";

            BarcodeManager barcodeParser = new BarcodeManager();
            barcodeParser.Location = "External";
            barcodeParser.ItemCode = "02320387";
            barcodeParser.Qty = 20;
            barcodeParser.DateTime = Convert.ToDateTime("2020-04-29 17:37");
            barcodeParser.Generate(rawSerialNo, template);

            Assert.AreEqual("####0232038700002000000#External###018000007", barcodeParser.Barcode);
            //---------------####0232038700002000000#External###018000007
            /////////////////CCCCCCCCCCCCQQQQQQDDDDDLLLLLLLLLSSSSSSSSSSSS
            //------------------002320387000020000000External000018000007
        }

        [TestMethod]
        public void Test_Generate_CSLQDYMdHms_unknownChar()
        {
            string rawSerialNo = "725";
            string template = "CCCCCCCCCYYMMddHHmmssSSSSSSSSLLLLLLLLQQQQ.DDD";
            //expected--------"A00200101200409165904000007280000T0030041000";

            BarcodeManager barcodeParser = new BarcodeManager();
            barcodeParser.Location = "T003";
            barcodeParser.ItemCode = "A00200101";
            barcodeParser.Qty = 41.03m;
            barcodeParser.DateTime = Convert.ToDateTime("2020-04-09 16:59:04");
            barcodeParser.Generate(rawSerialNo, template);

            Assert.AreEqual("A00200101200409165904#####725####T0030041.030", barcodeParser.Barcode);
            //--------------"CCCCCCCCCYYMMddHHmmssSSSSSSSSLLLLLLLLQQQQ.DDD
            //--------------"A00200101200409165904000007250000T0030041030
        }

    }
}
