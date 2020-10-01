using IMPLEA.Text;
using IMPLEA.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_LABELINSP.Models
{
    public class LabelInspectionManager : IObserver
    {
        List<LabelsDownloader> LabelsDownloaders;
        ImageProcessing ip;

        public LabelInspectionManager()
        {
            ConnectionParameters cp = new ConnectionParameters();
            cp.RawLabelsPath = @"C:\inetpub\wwwroot\LABELINSP\RawLables\";
            cp.DbSever = @"192.168.0.220\SQLEXPRESS";
            cp.DbName = "LabelPrinter_1";
            cp.DbUser = "labelinsp_user";
            cp.DbPassword = "labelinsp";
            cp.PrinterF = "192.168.0.214";
            cp.PrinterR = "192.168.0.215";
            cp.PrinterS = "192.168.0.216";

            LabelsDownloaders = new List<LabelsDownloader>();
            LabelsDownloaders.Add(new LabelsDownloader(cp));
            ip = new ImageProcessing();
        }

        public void Start()
        {
            foreach(var ld in LabelsDownloaders)
            {
                ld.RegisterObserver(this);
                ld.Start();
            }
        }

        private async Task InspectLabel(string data)
        {
            await new Task(() =>
            {
                ip.SetImage(@"C:\inetpub\wwwroot\LABELINSP\RawLables\" + data + ".png");

                string expectedB = "2409110790362103412345";
                string expectedS = "30385789215529";
                string expectedN = "HJÄLPSAM";
                string expectedP = "30385789";

                ip.RotateImage(180);
                ip.BarcodeDetectReadAddFrame_Big(expectedB);
                ip.BarcodeDetectReadAddFrame_Small(expectedS);
                ip.ReadModelName(expectedN);
                ip.ReadIKEAProductCode(expectedP);

                ip.SaveFinalPreviewImage(@"C:\inetpub\wwwroot\LABELINSP\InspectedLabels\" + data + ".png");
                //ip.SaveAllImages(@"C:\inetpub\wwwroot\LABELINSP\InspectedLabels\", serialNumber);
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
            await InspectLabel(data);
        }
    }
}
