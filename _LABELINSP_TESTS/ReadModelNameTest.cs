using System;
using Emgu.CV;
using Emgu.CV.Structure;
using MDL_LABELINSP.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _LABELINSP_TESTS
{
    [TestClass]
    public class ReadModelNameTest
    {
        [TestMethod]
        public void ReadModelNameTest_076044()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911076044.jpg"));
            
            string modelName = ip.ReadModelName();

            Assert.AreEqual("MEDELSTOR", modelName);
        }
        [TestMethod]
        public void ReadModelNameTest_076047()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911076047.png"));
            
            string modelName = ip.ReadModelName();

            Assert.AreEqual("MEDELSTOR", modelName);
        }
        [TestMethod]
        public void ReadModelNameTest_076076()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911076076.png"));
            
            string modelName = ip.ReadModelName();

            Assert.AreEqual("MEDELSTOR", modelName);
        }
        [TestMethod]
        public void ReadModelNameTest_076077()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911076077.png"));
            
            string modelName = ip.ReadModelName();

            Assert.AreEqual("MEDELSTOR", modelName);
        }
        [TestMethod]
        public void ReadModelNameTest_076078()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911076078.png"));
            
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("MEDELSTOR", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_079036()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911079036.png"));
            
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("HJÄLPSAM", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_535086()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911535086.png"));
            
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("RENGÖRA", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_535219()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911535219.png"));
            
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("RENGÖRA", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_535233()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911535233.png"));
            
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("RENGÖRA", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_535234()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911535234.png"));
            
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("RENGÖRA", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_535235()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911535235.png"));
            
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("RENGÖRA", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_535236()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911535236.png"));
            
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("RENGÖRA", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_535250()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911535250.png"));
            
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("LAGAN", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_535255()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911535252.png"));
            
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("LAGAN", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_535253()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911535253.png"));
            
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("LAGAN", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_536310()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911536310.png"));
            
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("SKINANDE", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_536377()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911536377.png"));
            
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("SKINANDE", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_536492()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911536492.png"));
            
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("RENODLAD", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_536493()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911536493.png"));
            
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("RENODLAD", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_536494()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911536494.png"));
            
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("RENODLAD", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_536497()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911536497.png"));
            
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("HYGIENISK", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_536499()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911536499.png"));
            
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("HYGIENISK", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_536500()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911536500.png"));
            
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("HYGIENISK", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_539181()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911539181.png"));
            
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("LAGAN", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_539237()
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath("911539237.png"));
            
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("LAGAN", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
    }
}
