using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MDL_LABELINSP.Models;
using Moq;
using System.Collections.Concurrent;

namespace _LABELINSP_TESTS
{
    /// <summary>
    /// Summary description for LabelsDownloaderTest
    /// </summary>
    [TestClass]
    public class LabelsDownloaderTest
    {
        [TestMethod]
        public void CheckPrinterLog_VerifyProcessed_Test()
        {
            //Prepare data
            LabelsDownloader labelsDownloader = new LabelsDownloader(new ConnectionParameters());
            
            List<PrintLog> printingLogs = new List<PrintLog>();
            printingLogs.Add(new PrintLog {IdBarcode = 1, Barcode = "05240570080040178388", Label_F = true, Label_R = true, Label_S = true, Downloaded_F = true, Downloaded_R = true, Downloaded_S = true, IsPrintEnd = true, });
            printingLogs.Add(new PrintLog {IdBarcode = 2, Barcode = "05240570080040178390", Label_F = false, Label_R = false, Label_S = true, Downloaded_F = false, Downloaded_R = false, Downloaded_S = false, IsPrintEnd = false });

            PrivateObject obj = new PrivateObject(labelsDownloader);
            obj.SetField("printingLogs", printingLogs);
            obj.SetField("lastId", 0);

            List<PrintLog> newPrintingLogs = new List<PrintLog>();
            newPrintingLogs.Add(new PrintLog {IdBarcode = 2, Barcode = "05240570080040178390", Label_F = true, Label_R = true, Label_S = true, Downloaded_F = false, Downloaded_R = false, Downloaded_S = false, IsPrintEnd = false });
            newPrintingLogs.Add(new PrintLog {IdBarcode = 3, Barcode = "05240570080040178400", Label_F = true, Label_R = false, Label_S = false, Downloaded_F = false, Downloaded_R = false, Downloaded_S = false, IsPrintEnd = false });

            //execute
            obj.Invoke("PrepareDownloadTasks", newPrintingLogs);

            //Assert
            List<PrintLog> pl = (List<PrintLog>)obj.GetField("printingLogs");
            ConcurrentQueue<DownloadTask> pt = (ConcurrentQueue<DownloadTask>)obj.GetField("downloadTasks");
            List<DownloadTask> pt_expected = new List<DownloadTask>();
            pt_expected.Add(new DownloadTask { Barcode = "05240570080040178390", LabelType = "Label_F", PrinterIp = "" });
            pt_expected.Add(new DownloadTask { Barcode = "05240570080040178390", LabelType = "Label_R", PrinterIp = "" });
            pt_expected.Add(new DownloadTask { Barcode = "05240570080040178400", LabelType = "Label_F", PrinterIp = "" });
            
            int lastId = (int)obj.GetField("lastId");

            Assert.AreEqual(2, pl.Count);
            Assert.AreEqual(3, pt.Count);
            Assert.IsTrue(pt_expected.Find(x => x.Barcode == "05240570080040178390" && x.LabelType == "Label_F") != null);
            Assert.IsTrue(pt_expected.Find(x => x.Barcode == "05240570080040178390" && x.LabelType == "Label_R") != null);
            Assert.IsTrue(pt_expected.Find(x => x.Barcode == "05240570080040178400" && x.LabelType == "Label_F") != null);
            Assert.AreEqual(1, lastId);
        }
    }
}
