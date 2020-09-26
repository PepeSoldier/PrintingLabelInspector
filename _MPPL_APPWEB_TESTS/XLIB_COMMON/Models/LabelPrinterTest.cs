using MDL_CORE.ComponentCore.Entities;
using MDLX_CORE.Model;
using MDLX_CORE.Model.PrintModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Net;
using System.Text;
using XLIB_COMMON.Enums;

namespace _MPPL_APPWEB_TESTS.XLIB_COMMON.Models
{
    [TestClass]
    public class LabelPrinterTest
    {
        // funkcje nie mają ASSERT'ów. Uzależnione są od zewnętrznych plików. To nie są testy jednostkowe

        //[TestMethod]
        //public void TestPrintZebraPLB_REPRINT()
        //{
        //    Printer printer = new Printer()
        //    {
        //        Id = 1,
        //        IpAdress = "192.168.10.2",
        //        Model = "ZPL 410",
        //        Name = "Zebra 1",
        //        PrinterType = PrinterType.Zebra,
        //        SerialNumber = "SN213",
        //    };

        //    LabelData ld = new LabelData()
        //    {
        //        Location = "10-12-13",
        //        Code = "130 450 657",
        //        PrintDate = "2020-10-14",
        //        PrintTime = "10:23",
        //        Qty = "100",
        //        WarehouseName = "WM-TECHN."
        //    };

        //    PrepareLabelModel plm = new PrepareLabelModel(printer, ld, "PLB_ZEBRA_QTY_REPRINT" + ".txt");
        //    LabelStatus labelStatus = plm.CheckLabelFinalText();

        //    PrivateObject po = new PrivateObject(plm);
        //    PrintLabelModelAbstract printerModel = (PrintLabelModelAbstract)po.GetField("printerModel");
        //    po = new PrivateObject(printerModel);
        //    string labelFullText = (string)po.GetField("labelFullText");
        //    //plm.GetLabelText();

        //    //byte[] zpl = Encoding.UTF8.GetBytes("^xa^cfa,50^fo100,100^fdHello World^fs^xz");
        //    byte[] zpl = Encoding.UTF8.GetBytes(labelFullText);

        //    // adjust print density (8dpmm), label width (4 inches), label height (6 inches), and label index (0) as necessary
        //    var request = (HttpWebRequest)WebRequest.Create("http://api.labelary.com/v1/printers/8dpmm/labels/2x5/0/");
        //    request.Method = "POST";
        //    request.Accept = "application/pdf"; // omit this line to get PNG images back
        //    request.ContentType = "application/x-www-form-urlencoded";
        //    request.ContentLength = zpl.Length;

        //    var requestStream = request.GetRequestStream();
        //    requestStream.Write(zpl, 0, zpl.Length);
        //    requestStream.Close();
        //    try
        //    {
        //        var response = (HttpWebResponse)request.GetResponse();
        //        var responseStream = response.GetResponseStream();
        //        var fileStream = File.Create("label.pdf"); // change file name for PNG images
        //        responseStream.CopyTo(fileStream);
        //        responseStream.Close();
        //        fileStream.Close();
        //    }
        //    catch (WebException e)
        //    {
        //        Console.WriteLine("Error: {0}", e.Status);
        //    }
        //}

        //[TestMethod]
        //public void TestPrintZebraPLB_CUMMUTITIVE()
        //{
        //    Printer printer = new Printer()
        //    {
        //        Id = 1,
        //        IpAdress = "192.168.10.2",
        //        Model = "ZPL 410",
        //        Name = "Zebra 1",
        //        PrinterType = PrinterType.Zebra,
        //        SerialNumber = "SN213",
        //    };

        //    LabelData ld = new LabelData()
        //    {
        //        Location = "10-12-13",
        //        Code = "130 450 657",
        //        PrintDate = "2020-10-14",
        //        PrintTime = "10:23",
        //        Qty = "100",
        //        WarehouseName = "WM-TECHN.",
        //    };
        //    PrepareLabelModel plm = new PrepareLabelModel(printer, ld, "PLB_ZEBRA_CUMMUTIVE");
        //    LabelStatus labelStatus = plm.CheckLabelFinalText();

        //    PrivateObject po = new PrivateObject(plm);
        //    PrintLabelModelAbstract printerModel = (PrintLabelModelAbstract)po.GetField("printerModel");
        //    po = new PrivateObject(printerModel);
        //    string labelFullText = (string)po.GetField("labelFullText");
        //    //plm.GetLabelText();
        //    //byte[] zpl = Encoding.UTF8.GetBytes("^xa^cfa,50^fo100,100^fdHello World^fs^xz");
        //    byte[] zpl = Encoding.UTF8.GetBytes(labelFullText);

        //    //adjust print density(8dpmm), label width(4 inches), label height(6 inches), and label index(0) as necessary
        //    var request = (HttpWebRequest)WebRequest.Create("http://api.labelary.com/v1/printers/8dpmm/labels/6x4/0/");
        //    request.Method = "POST";
        //    request.Accept = "application/pdf"; // omit this line to get PNG images back
        //    request.ContentType = "application/x-www-form-urlencoded";
        //    request.ContentLength = zpl.Length;
        //    var requestStream = request.GetRequestStream();
        //    requestStream.Write(zpl, 0, zpl.Length);
        //    requestStream.Close();
        //    try
        //    {
        //        var response = (HttpWebResponse)request.GetResponse();
        //        var responseStream = response.GetResponseStream();
        //        var fileStream = File.Create("labelCummutive.pdf"); // change file name for PNG images
        //        responseStream.CopyTo(fileStream);
        //        responseStream.Close();
        //        fileStream.Close();
        //    }
        //    catch (WebException e)
        //    {
        //        Console.WriteLine("Error: {0}", e.Status);
        //    }
        //}
    }
}