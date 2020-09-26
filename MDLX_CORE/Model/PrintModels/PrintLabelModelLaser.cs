//using PDFtoPrinter;
using PDFtoPrinter;
using System;
using System.IO;
using System.Net;
using System.Text;
using XLIB_COMMON.Enums;
using Zebra.Sdk.Comm;
using Zebra.Sdk.Printer;

namespace MDLX_CORE.Model.PrintModels
{
    public class PrintLabelModelLaser : PrintLabelModelAbstract
    {
        private Connection thePrinterConn;
        private ZebraPrinter printer;

        public PrintLabelModelLaser(string printerIp) : base(printerIp)
        {
            PrinterIP = printerIp;
            thePrinterConn = new TcpConnection(PrinterIP, TcpConnection.DEFAULT_ZPL_TCP_PORT);
            printer = PrintLabelModelZEBRA_Helper.Connect(thePrinterConn, PrinterLanguage.ZPL);
            fileExtension = "zpl";
        }

        protected override PrintingStatus SendLabelToPrinter()
        {
            byte[] zpl = Encoding.UTF8.GetBytes(labelFullText);

            // adjust print density (8dpmm), label width (4 inches), label height (6 inches), and label index (0) as necessary
            var request = (HttpWebRequest)WebRequest.Create("http://api.labelary.com/v1/printers/8dpmm/labels/6x4/0/");
            request.Method = "POST";
            request.Accept = "application/pdf"; // omit this line to get PNG images back
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = zpl.Length;

            var requestStream = request.GetRequestStream();
            requestStream.Write(zpl, 0, zpl.Length);
            requestStream.Close();
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                var responseStream = response.GetResponseStream();
                var fileStream = File.Create(@"C:\inetpub\label.pdf"); // change file name for PNG images
                responseStream.CopyTo(fileStream);
                responseStream.Close();
                fileStream.Close();

                string filePath = @"C:\inetpub\label.pdf";
                var printerName = "Brother DCP-1610W series";

                //comment 20200623
                var printer = new PDFtoPrinterPrinter();
                printer.Print(new PrintingOptions(printerName, filePath)).Wait();
            }
            catch (WebException e)
            {
                Console.WriteLine("Error: {0}", e.Status);
            }
            return PrintingStatus.Printed;
        }
    }
}