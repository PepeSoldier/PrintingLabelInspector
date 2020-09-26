using System;
using System.Text;
using XLIB_COMMON.Enums;
using Zebra.Sdk.Comm;
using Zebra.Sdk.Printer;

namespace MDLX_CORE.Model.PrintModels
{
    public class PrintLabelModelZEBRA : PrintLabelModelAbstract
    {
        Connection thePrinterConn;
        ZebraPrinter printer;
        

        public PrintLabelModelZEBRA(string printerIp) : base(printerIp)
        {
            PrinterIP = printerIp;
            thePrinterConn = new TcpConnection(PrinterIP, TcpConnection.DEFAULT_CPCL_TCP_PORT);
            printer = PrintLabelModelZEBRA_Helper.Connect(thePrinterConn, PrinterLanguage.ZPL);
            fileExtension = "zpl";
        }
      


        protected override PrintingStatus SendLabelToPrinter()
        {
            // Instantiate connection for ZPL TCP port at given address
            if (printer != null)
            {
                PrintLabelModelZEBRA_Helper.SetPageLanguage(printer);
                labelStatus = PrintLabelModelZEBRA_Helper.CheckStatus(printer);
                try
                {
                    // Open the connection - physical connection is established here.
                    //printer.SetConnection(thePrinterConn);
                    thePrinterConn.Open();

                    // Send the data to printer as a byte array.
                    thePrinterConn.Write(Encoding.UTF8.GetBytes(labelFullText));
                    labelStatus = PrintingStatus.Printed;
                }
                catch (ConnectionException e)
                {
                    //Handle communications error here.
                    Console.WriteLine(e.ToString());
                    labelStatus = PrintingStatus.UnrecognizedProblem;
                }
                finally
                {
                    //Close the connection to release resources.
                    thePrinterConn.Close();
                }
            }
            return labelStatus;
        }
    }
}