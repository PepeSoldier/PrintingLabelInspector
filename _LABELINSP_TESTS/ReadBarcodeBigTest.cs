using System;
using Emgu.CV;
using Emgu.CV.Structure;
using MDL_LABELINSP.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKEA_Labels_TEST
{
    [TestClass]
    public class ReadBarcodeBigTest
    {
        [TestMethod]
        public void ReadBarcodeBigTest_076044()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911076044.jpg"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            string bigBarcode = ip.BarcodeDetectReadAddFrame_Big();

            Assert.AreEqual("2409110760442103018600", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeBigTest_076047()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911076047.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            string bigBarcode = ip.BarcodeDetectReadAddFrame_Big();

            Assert.AreEqual("2409110760472103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeBigTest_076076()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911076076.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            string bigBarcode = ip.BarcodeDetectReadAddFrame_Big();

            Assert.AreEqual("2409110760762103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeBigTest_076077()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911076077.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            string bigBarcode = ip.BarcodeDetectReadAddFrame_Big();

            Assert.AreEqual("2409110760772103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeBigTest_076078()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911076078.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            //string smallBarcode = ip.BarcodeDetectReadAddFrame_Manual();
            string bigBarcode = ip.BarcodeDetectReadAddFrame_Big();

            //Assert.AreEqual("40475553215529", smallBarcode);
            Assert.AreEqual("2409110760782103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeBigTest_079036()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911079036.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            //string smallBarcode = ip.BarcodeDetectReadAddFrame_Manual();
            string bigBarcode = ip.BarcodeDetectReadAddFrame_Big();

            //Assert.AreEqual("40475553215529", smallBarcode);
            Assert.AreEqual("2409110790362103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeBigTest_535086()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911535086.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            //string smallBarcode = ip.BarcodeDetectReadAddFrame_Manual();
            string bigBarcode = ip.BarcodeDetectReadAddFrame_Big();

            //Assert.AreEqual("40475553215529", smallBarcode);
            Assert.AreEqual("2409115350862103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeBigTest_535219()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911535219.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            //string smallBarcode = ip.BarcodeDetectReadAddFrame_Manual();
            string bigBarcode = ip.BarcodeDetectReadAddFrame_Big();

            //Assert.AreEqual("40475553215529", smallBarcode);
            Assert.AreEqual("2409115352192103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeBigTest_535233()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911535233.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            //string smallBarcode = ip.BarcodeDetectReadAddFrame_Manual();
            string bigBarcode = ip.BarcodeDetectReadAddFrame_Big();

            //Assert.AreEqual("40475553215529", smallBarcode);
            Assert.AreEqual("2409115352332103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeBigTest_535234()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911535234.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            //string smallBarcode = ip.BarcodeDetectReadAddFrame_Manual();
            string bigBarcode = ip.BarcodeDetectReadAddFrame_Big();

            //Assert.AreEqual("40475553215529", smallBarcode);
            Assert.AreEqual("2409115352342103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeBigTest_535235()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911535235.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            //string smallBarcode = ip.BarcodeDetectReadAddFrame_Manual();
            string bigBarcode = ip.BarcodeDetectReadAddFrame_Big();

            //Assert.AreEqual("40475553215529", smallBarcode);
            Assert.AreEqual("2409115352352103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeBigTest_535236()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911535236.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            //string smallBarcode = ip.BarcodeDetectReadAddFrame_Manual();
            string bigBarcode = ip.BarcodeDetectReadAddFrame_Big();

            //Assert.AreEqual("40475553215529", smallBarcode);
            Assert.AreEqual("2409115352362103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeBigTest_535250()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911535250.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            //string smallBarcode = ip.BarcodeDetectReadAddFrame_Manual();
            string bigBarcode = ip.BarcodeDetectReadAddFrame_Big();

            //Assert.AreEqual("40475553215529", smallBarcode);
            Assert.AreEqual("2409115352502102612345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeBigTest_535252()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911535252.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            //string smallBarcode = ip.BarcodeDetectReadAddFrame_Manual();
            string bigBarcode = ip.BarcodeDetectReadAddFrame_Big();

            //Assert.AreEqual("40475553215529", smallBarcode);
            Assert.AreEqual("2409115352522102612345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeBigTest_535253()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911535253.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            //string smallBarcode = ip.BarcodeDetectReadAddFrame_Manual();
            string bigBarcode = ip.BarcodeDetectReadAddFrame_Big();

            //Assert.AreEqual("40475553215529", smallBarcode);
            Assert.AreEqual("2409115352532102612345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeBigTest_536310()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911536310.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            //string smallBarcode = ip.BarcodeDetectReadAddFrame_Manual();
            string bigBarcode = ip.BarcodeDetectReadAddFrame_Big();

            //Assert.AreEqual("40475553215529", smallBarcode);
            Assert.AreEqual("2409115363102103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeBigTest_536377()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911536377.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            //string smallBarcode = ip.BarcodeDetectReadAddFrame_Manual();
            string bigBarcode = ip.BarcodeDetectReadAddFrame_Big();

            //Assert.AreEqual("40475553215529", smallBarcode);
            Assert.AreEqual("2409115363772103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeBigTest_536492()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911536492.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            //string smallBarcode = ip.BarcodeDetectReadAddFrame_Manual();
            string bigBarcode = ip.BarcodeDetectReadAddFrame_Big();

            //Assert.AreEqual("40475553215529", smallBarcode);
            Assert.AreEqual("2409115364922102612345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeBigTest_536493()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911536493.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            //string smallBarcode = ip.BarcodeDetectReadAddFrame_Manual();
            string bigBarcode = ip.BarcodeDetectReadAddFrame_Big();

            //Assert.AreEqual("40475553215529", smallBarcode);
            Assert.AreEqual("2409115364932102612345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeBigTest_536494()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911536494.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            //string smallBarcode = ip.BarcodeDetectReadAddFrame_Manual();
            string bigBarcode = ip.BarcodeDetectReadAddFrame_Big();

            //Assert.AreEqual("40475553215529", smallBarcode);
            Assert.AreEqual("2409115364942102612345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeBigTest_536497()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911536497.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            //string smallBarcode = ip.BarcodeDetectReadAddFrame_Manual();
            string bigBarcode = ip.BarcodeDetectReadAddFrame_Big();

            //Assert.AreEqual("40475553215529", smallBarcode);
            Assert.AreEqual("2409115364972102812345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeBigTest_536499()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911536499.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            //string smallBarcode = ip.BarcodeDetectReadAddFrame_Manual();
            string bigBarcode = ip.BarcodeDetectReadAddFrame_Big();

            //Assert.AreEqual("40475553215529", smallBarcode);
            Assert.AreEqual("2409115364992102612345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeBigTest_536500()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911536500.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            //string smallBarcode = ip.BarcodeDetectReadAddFrame_Manual();
            string bigBarcode = ip.BarcodeDetectReadAddFrame_Big();

            //Assert.AreEqual("40475553215529", smallBarcode);
            Assert.AreEqual("2409115365002102612345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeBigTest_539181()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911539181.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            //string smallBarcode = ip.BarcodeDetectReadAddFrame_Manual();
            string bigBarcode = ip.BarcodeDetectReadAddFrame_Big();

            //Assert.AreEqual("40475553215529", smallBarcode);
            Assert.AreEqual("2409115391812103412345", bigBarcode);
        }
        [TestMethod]
        public void ReadBarcodeBigTest_539237()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(Helper.GetImgPath("911539237.png"));
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(image);
            //string smallBarcode = ip.BarcodeDetectReadAddFrame_Manual();
            string bigBarcode = ip.BarcodeDetectReadAddFrame_Big();

            //Assert.AreEqual("40475553215529", smallBarcode);
            Assert.AreEqual("2409115392372103412345", bigBarcode);
        }
    }
}
