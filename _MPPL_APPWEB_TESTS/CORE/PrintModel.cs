using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MDLX_CORE.Model.PrintModels;
using MDLX_CORE.Model;

namespace _MPPL_APPWEB_TESTS.CORE
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class PrintModel
    {
        [TestMethod]
        public void Text_ReturnEmptyForNoData()
        {
            PrintLabelModelAbstract plma = new PrintLabelModelZEBRA("");
            PrivateObject po = new PrivateObject(plma);
            po.Invoke("PrepareLabel", "", null);
            string labelFullText = (string)po.GetField("labelFullText");

            Assert.AreEqual("", labelFullText);
        }

        [TestMethod]
        public void Test_ReplaceMustachToValues()
        {
            LabelData labelData = new LabelData();
            labelData.Code = "999";
            labelData.SerialNumber = "0101";
            string labelDefinitionText = "Code:{{Code}}.{{SerialNumber}}";

            PrintLabelModelAbstract plma = new PrintLabelModelZEBRA("");
            PrivateObject po = new PrivateObject(plma);
            po.Invoke("PrepareLabel", labelDefinitionText, labelData);

            string labelFullText = (string)po.GetField("labelFullText");

            Assert.AreEqual("Code:999.0101", labelFullText);
        }

        [TestMethod]
        public void Test_ReturnDoubleQuestionMarkForUnknownProperty()
        {
            LabelData labelData = new LabelData();
            labelData.Code = "999";
            labelData.SerialNumber = "0101";
            string labelDefinitionText = "Code:{{Code}}.{{Fejk}}";

            PrintLabelModelAbstract plma = new PrintLabelModelZEBRA("");
            PrivateObject po = new PrivateObject(plma);
            po.Invoke("PrepareLabel", labelDefinitionText, labelData);

            string labelFullText = (string)po.GetField("labelFullText");

            Assert.AreEqual("Code:999.??", labelFullText);
        }

    }
}
