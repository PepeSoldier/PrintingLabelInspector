﻿using IMPLEA.Data;
using IMPLEA.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XLIB_COMMON.Model;
using DbConnector = IMPLEA.Data.DbConnector;

namespace MDL_LABELINSP.Models
{
    public class LabelsDownloader
    {
        private bool appIsRunning = true;
        private int lastId = 0;
        private List<IObserver> observers;
        private List<string> savedImages;
        private List<PrintLog> printingLogs;
        private List<DownloadTask> downloadTasks;
        private IMPLEA.Data.DbConnector dbConnector;
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
            Logger2FileSingleton.Instance.SaveLog("LabelsDownloader.Start()");

            appIsRunning = true;
            lastId = GetLastId();
            Thread t = new Thread(Thread_CheckForNewLabels);
            t.Start();
        }
        public void Stop()
        {
            appIsRunning = false;
        }

        private void Thread_CheckForNewLabels()
        {
            Logger2FileSingleton.Instance.SaveLog("LabelsDownloader.Thread_CheckForNewLabels");

            while (appIsRunning) {
                try
                {
                    GetPrintingLogs(out List<PrintLog> newPrintingLogs);
                    PrepareDownloadTasks(newPrintingLogs);
                    DownloadLabelFromPrinter();
                    NotifyObservers();
                    Logger2FileSingleton.Instance.SaveLog("LabelsDownloader.Thread_CheckForNewLabels is Alive");
                }
                catch(Exception ex)
                {
                    Logger2FileSingleton.Instance.SaveLog("LabelsDownloader.Thread_CheckForNewLabels Exception: " + ex.Message);
                }

                Thread.Sleep(3000);
            }
        }

        private int GetLastId()
        {
            try
            {
                var query =
                "SELECT MAX(p.[IdBarcode]) as MAxIdBarcode "
                    + " FROM [dbo].[ResPrintedProducts] p "
                    + " LEFT JOIN [dbo].[ResPrintedProductsDetails] pd ON pd.IdBarcode = p.IdBarcode "
                    + " WHERE IsPrintEnd = 1";

                dbConnector.connect();
                DataTable dt = dbConnector.runCommand(query);
                dbConnector.disconnect();

                int maxIdBarcode = 0;

                if (dt.Rows.Count > 0)
                {
                    maxIdBarcode = Convert.ToInt32(dt.Rows[0][0]);
                }

                Logger2FileSingleton.Instance.SaveLog("LabelsDownloader.GetLastId: result: " + maxIdBarcode);

                return maxIdBarcode;
            }
            catch (Exception ex)
            {
                Logger2FileSingleton.Instance.SaveLog("LabelsDownloader.GetLastId Exception: " +  ex.Message);
                return 0;
            }
            
        }
        private void GetPrintingLogs(out List<PrintLog> newPrintingLogs)
        {
            newPrintingLogs = new List<PrintLog>();
            
            string query =
                "SELECT TOP(100) "
                      + "p.[IdBarcode] "
                      + ",[Barcode]"
                      + ",[LastIdSession]"
                      + ",pd.CurrentAngle"
                      + ",pd.IsPrinted_0 as Label_F"
                      + ",pd.IsPrinted_90 as label_"
                      + ",pd.IsPrinted_180 as Label_R"
                      + ",pd.IsPrinted_270  as Label_S"
                      + ",pd.IsPrintEnd"
                      + ",pd.PrintDate"
                + " FROM [dbo].[ResPrintedProducts] p "
                + " LEFT JOIN [dbo].[ResPrintedProductsDetails] pd ON pd.IdBarcode = p.IdBarcode "
                + " WHERE p.IdBarcode > " + lastId
                + " ORDER BY p.IdBarcode DESC";

            dbConnector.connect();
            DataTable dt = dbConnector.runCommand(query);
            dbConnector.disconnect();

            foreach(DataRow dr in dt.Rows)
            {
                PrintLog pl = new PrintLog();
                pl.IdBarcode = MyConvert.ToInt32(dr["IdBarcode"]);
                pl.Barcode = MyConvert.ToString(dr["Barcode"]);
                pl.Label_F = Convert.ToBoolean(MyConvert.ToInt32(dr["Label_F"]));
                pl.Label_R = Convert.ToBoolean(MyConvert.ToInt32(dr["Label_R"]));
                pl.Label_S = Convert.ToBoolean(MyConvert.ToInt32(dr["Label_S"]));
                pl.IsPrintEnd = Convert.ToBoolean(MyConvert.ToInt32(dr["IsPrintEnd"]));

                newPrintingLogs.Add(pl);
            }

            Logger2FileSingleton.Instance.SaveLog("LabelsDownloader.GetPrintingLogs: newPrintingLogs.Count: " + newPrintingLogs.Count);
        }
        private void PrepareDownloadTasks(List<PrintLog> newPrintLogs)
        {
            if (newPrintLogs == null || newPrintLogs.Count == 0) return;

            foreach(var newPL in newPrintLogs)
            {
                var existingPL = printingLogs.FirstOrDefault(x => x.IdBarcode == newPL.IdBarcode);

                if (existingPL != null)
                {
                    if (existingPL.Label_F != newPL.Label_F && !existingPL.Downloaded_F)
                    {
                        downloadTasks.Add(new DownloadTask { Barcode = newPL.Barcode, LabelType = "Label_F" });
                        existingPL.Downloaded_F = true;
                    }
                    if (existingPL.Label_R != newPL.Label_R && !existingPL.Downloaded_R)
                    {
                        downloadTasks.Add(new DownloadTask { Barcode = newPL.Barcode, LabelType = "Label_R" });
                        existingPL.Downloaded_R = true;
                    }
                    if (existingPL.Label_S != newPL.Label_S && !existingPL.Downloaded_S)
                    {
                        downloadTasks.Add(new DownloadTask { Barcode = newPL.Barcode, LabelType = "Label_S" });
                        existingPL.Downloaded_S = true;
                    }

                    existingPL.Label_F = newPL.Label_F;
                    existingPL.Label_R = newPL.Label_R;
                    existingPL.Label_S = newPL.Label_S;
                }
                else
                {
                    printingLogs.Add(newPL);

                    if (newPL.Label_F)
                    {
                        downloadTasks.Add(new DownloadTask { Barcode = newPL.Barcode, LabelType = "Label_F" });
                        newPL.Downloaded_F = true;
                    }
                    if (newPL.Label_R)
                    {
                        downloadTasks.Add(new DownloadTask { Barcode = newPL.Barcode, LabelType = "Label_R" });
                        newPL.Downloaded_R = true;
                    }
                    if (newPL.Label_S)
                    {
                        downloadTasks.Add(new DownloadTask { Barcode = newPL.Barcode, LabelType = "Label_S" });
                        newPL.Downloaded_S = true;
                    }
                }
            }

            Logger2FileSingleton.Instance.SaveLog("LabelsDownloader.PrepareDownloadTasks: printingLogs.Count: " + printingLogs.Count + ", lastId: " + lastId);

            int lastId_tmp = printingLogs.Where(x => x.IsProcessingEnd).DefaultIfEmpty().Max(x => x != null ? x.IdBarcode : 0);
            lastId = Math.Max(lastId, lastId_tmp);
            printingLogs.RemoveAll(x => x.IsProcessingEnd);
            //lastId = printingLogs.DefaultIfEmpty().Min(x => x != null ? x.IdBarcode : lastId);
            Logger2FileSingleton.Instance.SaveLog("LabelsDownloader.PrepareDownloadTasks: printingLogs.Count: " + printingLogs.Count + ", lastId: " + lastId);
        }
        private void DownloadLabelFromPrinter()
        {
            savedImages = new List<string>();
            string printerIp = "";

            foreach (var dt in downloadTasks)
            {
                if (dt.LabelType == "Label_F") printerIp = connparams.PrinterF;
                else if (dt.LabelType == "Label_R") printerIp = connparams.PrinterR;
                else if (dt.LabelType == "Label_S") printerIp = connparams.PrinterS;

                if (dt.LabelType == "Label_S")
                {
                    Logger2FileSingleton.Instance.SaveLog("LabelsDownloader.DownloadLabelFromPrinter: " + printerIp + " / " + dt.Barcode);

                    using (var client = new WebClient())
                    {
                        try
                        {
                            string dateNow = DateTime.Now.ToString("yyyyMMddHHmmss");
                            client.DownloadFile("http://" + printerIp + "/printer/label.png", connparams.RawLabelsPath + dateNow + "_" + dt.Barcode + "_" + dt.LabelType + ".png");
                            savedImages.Add(dateNow + "_" + dt.Barcode + "_" + dt.LabelType);
                        }
                        catch (Exception ex)
                        {
                            Logger2FileSingleton.Instance.SaveLog("LabelsDownloader.DownloadLabelFromPrinter Exception: " + ex.Message + " d:" + ex.InnerException?.Message);
                        }
                    }
                }
            }

            downloadTasks.Clear();
        }
        private void NotifyObservers()
        {
            //Informujemy LabelInspectora o tym, że pojawiły się nowe obrazki
            foreach(var savedImage in savedImages)
            {
                foreach(IObserver observer in observers)
                {
                    Logger2FileSingleton.Instance.SaveLog("LabelsDownloader.NotifyObservers - savedImage: " + savedImage);
                    observer.Update(savedImage);
                }
            }
        }
        public void RegisterObserver(IObserver observer)
        {
            Logger2FileSingleton.Instance.SaveLog("LabelsDownloader.RegisterObserver");
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
        public bool IsProcessingEnd { get { return Downloaded_F && Downloaded_S && Downloaded_R; } }
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
