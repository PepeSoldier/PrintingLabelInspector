using System;
using Emgu.CV;
using Emgu.CV.Structure;
using MDL_LABELINSP.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKEA_Labels_TEST
{
    [TestClass]
    public class ReadBarcodeSmallTest
    {
        [TestMethod]
        public void ReadBarcodeSmallTest_076044()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911076044.jpg"));
            
            string smallBarcode = ip.BarcodeDetectReadAddFrame_Small();

            Assert.AreEqual("20385799215529", smallBarcode);
        }
        [TestMethod]
        public void ReadBarcodeSmallTest_076047()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911076047.png"));
            
            string smallBarcode = ip.BarcodeDetectReadAddFrame_Small();

            Assert.AreEqual("00376319215529", smallBarcode);
        }
        [TestMethod]
        public void ReadBarcodeSmallTest_076076()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911076076.png"));
            
            string smallBarcode = ip.BarcodeDetectReadAddFrame_Small();

            Assert.AreEqual("10475502215529", smallBarcode);
        }
        [TestMethod]
        public void ReadBarcodeSmallTest_076077()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911076077.png"));
            
            string smallBarcode = ip.BarcodeDetectReadAddFrame_Small();

            Assert.AreEqual("70475504215529", smallBarcode);
        }
        [TestMethod]
        public void ReadBarcodeSmallTest_076078()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911076078.png"));
            
            string smallBarcode = ip.BarcodeDetectReadAddFrame_Small();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("40475553215529", smallBarcode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeSmallTest_079036()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911079036.png"));
            
            string smallBarcode = ip.BarcodeDetectReadAddFrame_Small();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("30385789215529", smallBarcode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeSmallTest_535086()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911535086.png"));
            
            string smallBarcode = ip.BarcodeDetectReadAddFrame_Small();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("80376320215529", smallBarcode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeSmallTest_535219()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911535219.png"));
            
            string smallBarcode = ip.BarcodeDetectReadAddFrame_Small();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("80426179215529", smallBarcode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeSmallTest_535233()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911535233.png"));
            
            string smallBarcode = ip.BarcodeDetectReadAddFrame_Small();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("40443901215529", smallBarcode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeSmallTest_535234()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911535234.png"));
            
            string smallBarcode = ip.BarcodeDetectReadAddFrame_Small();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("10443243215529", smallBarcode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeSmallTest_535235()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911535235.png"));
            
            string smallBarcode = ip.BarcodeDetectReadAddFrame_Small();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("90443244215529", smallBarcode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeSmallTest_535236()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911535236.png"));
            
            string smallBarcode = ip.BarcodeDetectReadAddFrame_Small();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("60443245215529", smallBarcode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeSmallTest_535250()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911535250.png"));
            
            string smallBarcode = ip.BarcodeDetectReadAddFrame_Small();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("50475425215529", smallBarcode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeSmallTest_535255()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911535252.png"));
            
            string smallBarcode = ip.BarcodeDetectReadAddFrame_Small();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("30475426215529", smallBarcode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeSmallTest_535253()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911535253.png"));
            
            string smallBarcode = ip.BarcodeDetectReadAddFrame_Small();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("10475427215529", smallBarcode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeSmallTest_536310()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911536310.png"));
            
            string smallBarcode = ip.BarcodeDetectReadAddFrame_Small();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("60376321215529", smallBarcode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeSmallTest_536377()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911536377.png"));
            
            string smallBarcode = ip.BarcodeDetectReadAddFrame_Small();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("60426180215529", smallBarcode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeSmallTest_536492()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911536492.png"));
            
            string smallBarcode = ip.BarcodeDetectReadAddFrame_Small();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("90475616215529", smallBarcode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeSmallTest_536493()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911536493.png"));
            
            string smallBarcode = ip.BarcodeDetectReadAddFrame_Small();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("70475617215529", smallBarcode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeSmallTest_536494()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911536494.png"));
            
            string smallBarcode = ip.BarcodeDetectReadAddFrame_Small();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("50475618215529", smallBarcode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeSmallTest_536497()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911536497.png"));
            
            string smallBarcode = ip.BarcodeDetectReadAddFrame_Small();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("20475610215529", smallBarcode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeSmallTest_536499()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911536499.png"));
            
            string smallBarcode = ip.BarcodeDetectReadAddFrame_Small();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("00475611215529", smallBarcode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeSmallTest_536500()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911536500.png"));
            
            string smallBarcode = ip.BarcodeDetectReadAddFrame_Small();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("80475612215529", smallBarcode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeSmallTest_539181()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911539181.png"));
            
            string smallBarcode = ip.BarcodeDetectReadAddFrame_Small();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("20376318215529", smallBarcode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeSmallTest_539237()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911539237.png"));
            
            string smallBarcode = ip.BarcodeDetectReadAddFrame_Small();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("00426178215529", smallBarcode);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
    }
}
