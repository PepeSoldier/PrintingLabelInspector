using IMPLEA.Data;
using IMPLEA.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace MDL_LABELINSP.Models
{
    public class LabelsDownloader
    {
        private bool appIsRunning = true;
        private int lastProcessedId = 0;
        private List<IObserver> observers;
        private List<string> savedPhotos;
        private List<PrintLog> printingLogs;
        private List<DownloadTask> downloadTasks;
        private DbConnector dbConnector;
        private ConnectionParameters connparams;

        public LabelsDownloader(ConnectionParameters connectionParameters) {
            connparams = connectionParameters;
            observers = new List<IObserver>();
            printingLogs = new List<PrintLog>();
            downloadTasks = new List<DownloadTask>();
            dbConnector = new DbConnector(XLIB_COMMON.Model.Logger2FileSingleton.Instance, connparams.DbSever, connparams.DbName, connparams.DbUser, connparams.DbPassword, "");
        }

        public void Start()
        {
            appIsRunning = true;
            Thread t = new Thread(Thread_CheckForNewLabels);
            t.Start();
        }
        public void Stop()
        {
            appIsRunning = false;
        }

        private void Thread_CheckForNewLabels()
        {
            while (appIsRunning) {
                GetPrintingLogs(out List<PrintLog> newPrintingLogs);
                PrepareDownloadTasks(newPrintingLogs);
                DownloadLabelFromPrinter();
                NotifyObservers();
                Thread.Sleep(1000);
            }
        }

        private void GetPrintingLogs(out List<PrintLog> newPrintingLogs)
        {
            newPrintingLogs = new List<PrintLog>();
            int lastId = 3353872;

            string query =
                "SELECT TOP(100) p.[IdBarcode] "
                      + ",[Barcode]"
                      + ",[LastIdSession]"
                      + ",pd.CurrentAngle"
                      + ",pd.IsPrinted_0 as Label_F"
                      + ",pd.IsPrinted_90 as label_"
                      + ",pd.IsPrinted_180 as Label_R"
                      + ",pd.IsPrinted_270  as Label_S"
                      + ",pd.IsPrintEnd"
                      + ",pd.PrintDate"
                + " FROM[LabelPrinter_1].[dbo].[ResPrintedProducts] p "
                + " LEFT JOIN[LabelPrinter_1].[dbo].[ResPrintedProductsDetails] pd ON pd.IdBarcode = p.IdBarcode "
                + " WHERE p.IdBarcode > " + lastId
                + " ORDER BY p.IdBarcode DESC";

            dbConnector.connect();
            DataTable dt = dbConnector.runCommand(query);

            foreach(DataRow dr in dt.Rows)
            {
                PrintLog pl = new PrintLog();
                pl.Barcode = MyConvert.ToString(dr["Barcode"]);
                pl.Label_F = Convert.ToBoolean(MyConvert.ToInt32(dr["Label_F"]));
                pl.Label_R = Convert.ToBoolean(MyConvert.ToInt32(dr["Label_R"]));
                pl.Label_S = Convert.ToBoolean(MyConvert.ToInt32(dr["Label_S"]));
                pl.IsPrintEnd = Convert.ToBoolean(MyConvert.ToInt32(dr["IsPrintEnd"]));

                newPrintingLogs.Add(pl);
            }
        }
        private void PrepareDownloadTasks(List<PrintLog> newPrintLogs)
        {
            printingLogs.RemoveAll(x => x.IsProcessingEnd);

            foreach(var newPL in newPrintLogs)
            {
                var existingPL = printingLogs.FirstOrDefault(x => x.IdBarcode == newPL.IdBarcode);

                if (existingPL != null)
                {
                    if (existingPL.Label_F != newPL.Label_F && !existingPL.Downloaded_F)
                        downloadTasks.Add(new DownloadTask { Barcode = newPL.Barcode, LabelType = "Label_F" });
                    if (existingPL.Label_R != newPL.Label_R && !existingPL.Downloaded_R)
                        downloadTasks.Add(new DownloadTask { Barcode = newPL.Barcode, LabelType = "Label_R" });
                    if (existingPL.Label_S != newPL.Label_S && !existingPL.Downloaded_S)
                        downloadTasks.Add(new DownloadTask { Barcode = newPL.Barcode, LabelType = "Label_S" });

                    existingPL.Label_F = newPL.Label_F;
                    existingPL.Label_R = newPL.Label_R;
                    existingPL.Label_S = newPL.Label_S;
                }
                else
                {
                    printingLogs.Add(newPL);

                    if (newPL.Label_F)
                        downloadTasks.Add(new DownloadTask { Barcode = newPL.Barcode, LabelType = "Label_F" });
                    if (newPL.Label_R)
                        downloadTasks.Add(new DownloadTask { Barcode = newPL.Barcode, LabelType = "Label_R" });
                    if (newPL.Label_S)
                        downloadTasks.Add(new DownloadTask { Barcode = newPL.Barcode, LabelType = "Label_S" });

                }
            }
        }
        private void DownloadLabelFromPrinter()
        {
            savedPhotos = new List<string>();
            foreach (var dt in downloadTasks)
            {
                using (var client = new WebClient())
                {
                    string printerIp = "";

                    if (dt.LabelType == "Label_F") printerIp = connparams.PrinterF;
                    else if (dt.LabelType == "Label_R") printerIp = connparams.PrinterR;
                    else if (dt.LabelType == "Label_S") printerIp = connparams.PrinterS;

                    client.DownloadFile("http://" + printerIp + "/printer/label.png", connparams.RawLabelsPath + dt.Barcode + "_" + dt.LabelType + ".png");
                }
                savedPhotos.Add(dt.Barcode + "_" + dt.LabelType);
            }
        }
        private void NotifyObservers()
        {
            foreach(var savedPhoto in savedPhotos)
            {
                foreach(IObserver observer in observers)
                {
                    observer.Update(savedPhoto);
                }
            }
        }
        public void RegisterObserver(IObserver observer)
        {
            observers.Add(observer);
        }
    }

    public class PrintLog
    {
        public int IdBarcode { get; set; }
        
        public string Barcode { get; set; }

        public bool Label_F { get; set; }
        public bool Label_S { get; set; }
        public bool Label_R { get; set; }
        public bool IsPrintEnd { get; set; }

        public bool Downloaded_F { get; set; }
        public bool Downloaded_S { get; set; }
        public bool Downloaded_R { get; set; }
        public bool IsProcessingEnd { get; set; }
    }
    public class DownloadTask
    {
        public string Barcode { get; set; }
        public string LabelType { get; set; }
        public string PrinterIp { get; set; }
    }
    public class ConnectionParameters
    {
        public string RawLabelsPath { get; set; }
        public string DbSever { get; set; }
        public string DbName { get; set; }
        public string DbUser { get; set; }
        public string DbPassword { get; set; }
        public string PrinterF { get; set; }
        public string PrinterS { get; set; }
        public string PrinterR { get; set; }
    }

}
