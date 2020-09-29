using System;
using Emgu.CV;
using Emgu.CV.Structure;
using MDL_LABELINSP.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKEA_Labels_TEST
{
    [TestClass]
    public class ReadModelNameTest
    {
        [TestMethod]
        public void ReadModelNameTest_076044()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911076044.jpg"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            string modelName = ip.ReadModelName();

            Assert.AreEqual("MEDELSTOR", modelName);
        }
        [TestMethod]
        public void ReadModelNameTest_076047()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911076047.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            string modelName = ip.ReadModelName();

            Assert.AreEqual("MEDELSTOR", modelName);
        }
        [TestMethod]
        public void ReadModelNameTest_076076()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911076076.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            string modelName = ip.ReadModelName();

            Assert.AreEqual("MEDELSTOR", modelName);
        }
        [TestMethod]
        public void ReadModelNameTest_076077()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911076077.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            string modelName = ip.ReadModelName();

            Assert.AreEqual("MEDELSTOR", modelName);
        }
        [TestMethod]
        public void ReadModelNameTest_076078()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911076078.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("MEDELSTOR", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_079036()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911079036.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("HJÄLPSAM", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_535086()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911535086.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("RENGÖRA", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_535219()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911535219.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("RENGÖRA", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_535233()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911535233.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("RENGÖRA", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_535234()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911535234.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("RENGÖRA", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_535235()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911535235.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("RENGÖRA", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_535236()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911535236.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("RENGÖRA", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_535250()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911535250.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("LAGAN", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_535255()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911535252.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("LAGAN", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_535253()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911535253.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("LAGAN", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_536310()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911536310.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("SKINANDE", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_536377()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911536377.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("SKINANDE", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_536492()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911536492.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("RENODLAD", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_536493()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911536493.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("RENODLAD", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_536494()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911536494.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("RENODLAD", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_536497()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911536497.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("HYGIENISK", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_536499()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911536499.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("HYGIENISK", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_536500()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911536500.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("HYGIENISK", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_539181()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911539181.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("LAGAN", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadModelNameTest_539237()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911539237.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            string modelName = ip.ReadModelName();
            //string bigBarcode = ip.BarcodeDetectReadAddFrame();

            Assert.AreEqual("LAGAN", modelName);
            //Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
    }
}
