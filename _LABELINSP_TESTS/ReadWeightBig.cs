using System;
using System.Linq;
using Emgu.CV;
using Emgu.CV.Structure;
using MDL_LABELINSP.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _LABELINSP_TESTS
{
    [TestClass]
    public class ReadWeightBig
    {
        private static void TestWeightBig(string itemCode)
        {
            ImageProcessing ip = new ImageProcessing();
            ip.SetImage(Helper.GetImgPath(itemCode + ".png"));
            string bigWeight = ip.ReadWeightBig();
            Assert.AreEqual(Helper.ItemData.FirstOrDefault(x => x.ItemCode == itemCode)?.ExpectedWeightKG, bigWeight);
        }

        [TestMethod]
        public void ReadWeightBig_076047()
        {
            string itemCode = "911076047";
            TestWeightBig(itemCode);
        }
        [TestMethod]
        public void ReadWeightBig_076076()
        {
            string itemCode = "911076076";
            TestWeightBig(itemCode);
        }
        [TestMethod]
        public void ReadWeightBig_076077()
        {
            string itemCode = "911076077";
            TestWeightBig(itemCode);
        }
        [TestMethod]
        public void ReadWeightBig_076078()
        {
            string itemCode = "911076078";
            TestWeightBig(itemCode);
        }
        [TestMethod]
        public void ReadWeightBig_079036()
        {
            string itemCode = "911079036";
            TestWeightBig(itemCode);
        }
        [TestMethod]
        public void ReadWeightBig_535086()
        {
            string itemCode = "911535086";
            TestWeightBig(itemCode);
        }
        [TestMethod]
        public void ReadWeightBig_535219()
        {
            string itemCode = "911535219";
            TestWeightBig(itemCode);
        }
        [TestMethod]
        public void ReadWeightBig_535233()
        {
            string itemCode = "911535233";
            TestWeightBig(itemCode);
        }
        [TestMethod]
        public void ReadWeightBig_535234()
        {
            string itemCode = "911535234";
            TestWeightBig(itemCode);
        }
        [TestMethod]
        public void ReadWeightBig_535235()
        {
            string itemCode = "911535235";
            TestWeightBig(itemCode);
        }
        [TestMethod]
        public void ReadWeightBig_535236()
        {
            string itemCode = "911535236";
            TestWeightBig(itemCode);
        }
        [TestMethod]
        public void ReadWeightBig_535250()
        {
            string itemCode = "911535250";
            TestWeightBig(itemCode);
        }
        [TestMethod]
        public void ReadWeightBig_535252()
        {
            string itemCode = "911535252";
            TestWeightBig(itemCode);
        }
        [TestMethod]
        public void ReadWeightBig_535253()
        {
            string itemCode = "911535253";
            TestWeightBig(itemCode);
        }
        [TestMethod]
        public void ReadWeightBig_536310()
        {
            string itemCode = "911536310";
            TestWeightBig(itemCode);
        }
        [TestMethod]
        public void ReadWeightBig_536377()
        {
            string itemCode = "911536377";
            TestWeightBig(itemCode);
        }
        [TestMethod]
        public void ReadWeightBig_536492()
        {
            string itemCode = "911536492";
            TestWeightBig(itemCode);
        }
        [TestMethod]
        public void ReadWeightBig_536493()
        {
            string itemCode = "911536493";
            TestWeightBig(itemCode);
        }
        [TestMethod]
        public void ReadWeightBig_536494()
        {
            string itemCode = "911536494";
            TestWeightBig(itemCode);
        }
        [TestMethod]
        public void ReadWeightBig_536497()
        {
            string itemCode = "911536497";
            TestWeightBig(itemCode);
        }
        [TestMethod]
        public void ReadWeightBig_536499()
        {
            string itemCode = "911536499";
            TestWeightBig(itemCode);
        }
        [TestMethod]
        public void ReadWeightBig_536500()
        {
            string itemCode = "911536500";
            TestWeightBig(itemCode);
        }
        [TestMethod]
        public void ReadWeightBig_539181()
        {
            string itemCode = "911539181";
            TestWeightBig(itemCode);
        }
        [TestMethod]
        public void ReadWeightBig_539237()
        {
            string itemCode = "911539237";
            TestWeightBig(itemCode);
        }
    }
}
