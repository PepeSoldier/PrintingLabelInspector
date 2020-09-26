using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using XLIB_COMMON.Enums;
using XLIB_COMMON.Model;

namespace MDLX_CORE.Model.PrintModels
{
    public class PrintLabelModelCAB : PrintLabelModelAbstract
    {
        public PrintLabelModelCAB(string printerIp) : base(printerIp)
        {
        }

        protected override PrintingStatus SendLabelToPrinter()
        {
            string ftpUserName = "ftpprint";
            string ftpPassword = "1234";

            FileInfo objFile = new FileInfo("file1.txt");

            FtpWebRequest objFTPRequest;
            objFTPRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + PrinterIP + "/print.lbl"));            
            objFTPRequest.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
            objFTPRequest.KeepAlive = false;
            objFTPRequest.UseBinary = true;
            objFTPRequest.ContentLength = labelFullText.Length;
            objFTPRequest.Method = WebRequestMethods.Ftp.UploadFile;

            int intBufferLength = 16 * 1024;
            byte[] objBuffer = new byte[intBufferLength];

            byte[] objFileStream = Encoding.ASCII.GetBytes(labelFullText);

            try
            {
                Stream objStream = objFTPRequest.GetRequestStream();
                objStream.Write(objFileStream, 0, objFileStream.Length);
                objStream.Close();
                labelStatus = PrintingStatus.Printed;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                labelStatus = PrintingStatus.UnrecognizedProblem;
            }
            return labelStatus;
        }
    }
}