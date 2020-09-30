using System;
using Emgu.CV;
using Emgu.CV.Structure;
using MDL_LABELINSP.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKEA_Labels_TEST
{
    [TestClass]
    public class ReadIKEAProductCodeTest
    {
        [TestMethod]
        public void ReadIKEAProductCode_076044()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911076044.jpg"));
            
            string productCode = ip.ReadIKEAProductCode();

            Assert.AreEqual("20385799", productCode);
        }
        [TestMethod]
        public void ReadIKEAProductCode_076047()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911076047.png"));
            
            string productCode = ip.ReadIKEAProductCode();

            Assert.AreEqual("00376319", productCode);
        }
        [TestMethod]
        public void ReadIKEAProductCode_076076()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911076076.png"));
            
            string productCode = ip.ReadIKEAProductCode();

            Assert.AreEqual("10475502", productCode);
        }
        [TestMethod]
        public void ReadIKEAProductCode_076077()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911076077.png"));
            
            string productCode = ip.ReadIKEAProductCode();

            Assert.AreEqual("70475504", productCode);
        }
        [TestMethod]
        public void ReadIKEAProductCode_076078()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911076078.png"));
            
            string productCode = ip.ReadIKEAProductCode();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("40475553", productCode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadIKEAProductCode_079036()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911079036.png"));
            
            string productCode = ip.ReadIKEAProductCode();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("30385789", productCode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadIKEAProductCode_535086()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911535086.png"));
            
            string productCode = ip.ReadIKEAProductCode();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("80376320", productCode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadIKEAProductCode_535219()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911535219.png"));
            
            string productCode = ip.ReadIKEAProductCode();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("80426179", productCode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadIKEAProductCode_535233()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911535233.png"));
            
            string productCode = ip.ReadIKEAProductCode();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("40443901", productCode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadIKEAProductCode_535234()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911535234.png"));
            
            string productCode = ip.ReadIKEAProductCode();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("10443243", productCode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadIKEAProductCode_535235()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911535235.png"));
            
            string productCode = ip.ReadIKEAProductCode();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("90443244", productCode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadIKEAProductCode_535236()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911535236.png"));
            
            string productCode = ip.ReadIKEAProductCode();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("60443245", productCode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadIKEAProductCode_535250()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911535250.png"));
            
            string productCode = ip.ReadIKEAProductCode();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("50475425", productCode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadIKEAProductCode_535255()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911535252.png"));
            
            string productCode = ip.ReadIKEAProductCode();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("30475426", productCode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadIKEAProductCode_535253()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911535253.png"));
            
            string productCode = ip.ReadIKEAProductCode();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("10475427", productCode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadIKEAProductCode_536310()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911536310.png"));
            
            string productCode = ip.ReadIKEAProductCode();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("60376321", productCode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadIKEAProductCode_536377()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911536377.png"));
            
            string productCode = ip.ReadIKEAProductCode();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("60426180", productCode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadIKEAProductCode_536492()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911536492.png"));
            
            string productCode = ip.ReadIKEAProductCode();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("90475616", productCode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadIKEAProductCode_536493()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911536493.png"));
            
            string productCode = ip.ReadIKEAProductCode();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("70475617", productCode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadIKEAProductCode_536494()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911536494.png"));
            
            string productCode = ip.ReadIKEAProductCode();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("50475618", productCode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadIKEAProductCode_536497()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911536497.png"));
            
            string productCode = ip.ReadIKEAProductCode();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("20475610", productCode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadIKEAProductCode_536499()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911536499.png"));
            
            string productCode = ip.ReadIKEAProductCode();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("00475611", productCode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadIKEAProductCode_536500()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911536500.png"));
            
            string productCode = ip.ReadIKEAProductCode();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("80475612", productCode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadIKEAProductCode_539181()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911539181.png"));
            
            string productCode = ip.ReadIKEAProductCode();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("20376318", productCode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadIKEAProductCode_539237()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911539237.png"));
            
            string productCode = ip.ReadIKEAProductCode();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("00426178", productCode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
    }
}
