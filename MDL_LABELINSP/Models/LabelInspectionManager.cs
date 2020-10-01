using IMPLEA.Text;
using IMPLEA.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLIB_COMMON.Model;

namespace MDL_LABELINSP.Models
{
    public class LabelInspectionManager : IObserver
    {
        List<LabelsDownloader> LabelsDownloaders;
        ImageProcessing ip;
        string rawLabelsPath;
        string inspectedLabelsPath;

        public LabelInspectionManager()
        {
            rawLabelsPath = @"C:\inetpub\wwwroot\\LABELINSP_DATA\RawLabels\";
            inspectedLabelsPath = @"C:\inetpub\wwwroot\LABELINSP_DATA\InspectedLabels\";

            ConnectionParameters cp = new ConnectionParameters();
            cp.RawLabelsPath = rawLabelsPath;
            cp.DbSever = @"192.168.0.221\SQLEXPRESS";
            cp.DbName = "LabelPrinter_2";
            cp.DbUser = "labelinsp_user";
            cp.DbPassword = "labelinsp";
            cp.PrinterS = "192.168.0.216";
            cp.PrinterR = "192.168.0.217";
            cp.PrinterF = "192.168.0.218";

            LabelsDownloaders = new List<LabelsDownloader>();
            LabelsDownloaders.Add(new LabelsDownloader(cp));
            ip = new ImageProcessing();
        }

        public void Start()
        {
            Logger2FileSingleton.Instance.SaveLog("LabelInspectionManager.Start()");

            foreach (var ld in LabelsDownloaders)
            {
                ld.RegisterObserver(this);
                ld.Start();
            }
        }

        private async Task InspectLabel(string data)
        {
            await Task.Run(() =>
            {
                try
                {
                    Logger2FileSingleton.Instance.SaveLog("LabelInspectionManager.InspectLabel(" + data + ") START");

                    ip.SetImage(string.Format("{0}{1}.{2}", rawLabelsPath, data, "png"));

                    string expectedB = "2409110790362103412345";
                    string expectedS = "30385789215529";
                    string expectedN = "HJÄLPSAM";
                    string expectedP = "30385789";

                    ip.RotateImage(180);
                    ip.BarcodeDetectReadAddFrame_Big(expectedB);
                    ip.BarcodeDetectReadAddFrame_Small(expectedS);
                    ip.ReadModelName(expectedN);
                    ip.ReadIKEAProductCode(expectedP);

                    ip.SaveFinalPreviewImage(string.Format("{0}{1}.{2}", inspectedLabelsPath, data, "png"));
                    //ip.SaveAllImages(@"C:\inetpub\wwwroot\LABELINSP\InspectedLabels\", serialNumber);

                    Logger2FileSingleton.Instance.SaveLog("LabelInspectionManager.InspectLabel(" + data + ") END");
                }
                catch (Exception ex)
                {
                    Logger2FileSingleton.Instance.SaveLog("LabelInspectionManager.InspectLabel(" + data + ") Exception: " + ex.Message + " d:" + ex.InnerException?.Message);
                }
            });
        }
        private void ParseBarcode_PackLabel(string data, out string serialNumber, out string pnc)
        {
            //240|911076047|21|03412345|0
            serialNumber = data.Mid(14, 8);
            pnc = data.Mid(3, 9);
        }
        private void ParseBarcode_RatingLabel(string data, out string serialNumber, out string elc)
        {
            //05240570080040178388        
            serialNumber = data.Mid(12, 8);
            elc = data.Mid(1, 9);
        }

        public async void Update(string data)
        {
            //ParseBarcode_PackLabel(data, out string serialNumber, out string pnc);
            Logger2FileSingleton.Instance.SaveLog("LabelInspectionManager.Update(" + data + ") START");
            await InspectLabel(data);
            Logger2FileSingleton.Instance.SaveLog("LabelInspectionManager.Update(" + data + ") END");
        }
    }
}
