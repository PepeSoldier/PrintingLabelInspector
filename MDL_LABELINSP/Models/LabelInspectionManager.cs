using IMPLEA.Text;
using IMPLEA.Utilities;
using MDL_LABELINSP.Entities;
using MDL_LABELINSP.Enums;
using MDL_LABELINSP.Interfaces;
using MDL_LABELINSP.Repos;
using MDL_LABELINSP.UnitOfWorks;
using MDL_LABELINSP.ViewModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XLIB_COMMON.Model;

namespace MDL_LABELINSP.Models
{
    public class LabelInspectionManager : IObserver
    {
        private bool appIsRunning = true;
        UnitOfWorkLabelInsp uow;
        List<LabelsDownloader> LabelsDownloaders;
        ImageProcessing imgProcessing;
        ConcurrentQueue<string> labelsToBeInspected = new ConcurrentQueue<string>();
        readonly string RAW_LABELS_PATH;
        readonly string INSPECTED_LABELS_PATH;
        readonly string INVALID_LABELS_PATH;
        readonly string WORKORDERS_LABELS_PATH;
        bool isTest;

        public LabelInspectionManager(IDbContextLabelInsp db, bool isTest)
        {
            this.isTest = true;
            labelsToBeInspected = new ConcurrentQueue<string>();
            uow = new UnitOfWorkLabelInsp(db);
            imgProcessing = new ImageProcessing();

            RAW_LABELS_PATH = @"C:\inetpub\wwwroot\LABELINSP_DATA\TestLabels\";
            INSPECTED_LABELS_PATH = @"C:\inetpub\wwwroot\LABELINSP_DATA\TestLabels\InspectedLabels\";
            INVALID_LABELS_PATH = @"C:\inetpub\wwwroot\LABELINSP_DATA\TestLabels\";
            WORKORDERS_LABELS_PATH = @"C:\inetpub\wwwroot\LABELINSP_DATA\WorkordersLabels\";
        }
        public LabelInspectionManager(IDbContextLabelInsp db)
        {
            labelsToBeInspected = new ConcurrentQueue<string>();
            uow = new UnitOfWorkLabelInsp(db);
            imgProcessing = new ImageProcessing();

            RAW_LABELS_PATH = @"C:\inetpub\wwwroot\LABELINSP_DATA\RawLabels\";
            INSPECTED_LABELS_PATH = @"C:\inetpub\wwwroot\LABELINSP_DATA\InspectedLabels\";
            INVALID_LABELS_PATH = @"C:\inetpub\wwwroot\LABELINSP_DATA\InvalidLabels\";
            WORKORDERS_LABELS_PATH = @"C:\inetpub\wwwroot\LABELINSP_DATA\WorkordersLabels\";

            ConnectionParameters cp = new ConnectionParameters();
            cp.RawLabelsPath = RAW_LABELS_PATH;
            cp.DbSever = @"192.168.0.221\SQLEXPRESS";
            cp.DbName = "LabelPrinter_2";
            cp.DbUser = "labelinsp_user";
            cp.DbPassword = "labelinsp";
            cp.PrinterS = "192.168.0.216";
            cp.PrinterR = "192.168.0.217";
            cp.PrinterF = "192.168.0.218";

            ConnectionParameters cp2 = new ConnectionParameters();
            cp2.RawLabelsPath = RAW_LABELS_PATH;
            cp2.DbSever = @"192.168.0.220\SQLEXPRESS";
            cp2.DbName = "LabelPrinter_1";
            cp2.DbUser = "labelinsp_user";
            cp2.DbPassword = "labelinsp";
            cp2.PrinterS = "192.168.0.211";
            cp2.PrinterR = "192.168.0.214";
            cp2.PrinterF = "192.168.0.213";

            ConnectionParameters cp3 = new ConnectionParameters();
            cp3.RawLabelsPath = RAW_LABELS_PATH;
            cp3.DbSever = @"192.168.0.210";
            cp3.DbName = "LabelPrinter_3";
            cp3.DbUser = "labelinsp_user";
            cp3.DbPassword = "labelinsp";
            cp3.PrinterS = "192.168.0.242";
            cp3.PrinterR = "192.168.0.240";
            cp3.PrinterF = "192.168.0.241";

            LabelsDownloaders = new List<LabelsDownloader>();
            LabelsDownloaders.Add(new LabelsDownloader(cp));
            LabelsDownloaders.Add(new LabelsDownloader(cp2));
            LabelsDownloaders.Add(new LabelsDownloader(cp3));

            foreach (var ld in LabelsDownloaders)
            {
                ld.RegisterObserver(this);
            }
            //LabelsDownloaders.Add(new LabelsDownloader(cp3));
        }

        public void Start()
        {
            if (isTest) return;

            Logger2FileSingleton.Instance.SaveLog("LabelInspectionManager.Starting...");

            Thread t = new Thread(InspectLabel_Thread);
            t.Start();

            foreach (var labelDownloader in LabelsDownloaders)
            {
                labelDownloader.Start();
            }
        }
        public void Stop()
        {
            Logger2FileSingleton.Instance.SaveLog("LabelInspectionManager.Stop...");
            foreach (var ld in LabelsDownloaders)
            {
                ld.Stop();
            }
            appIsRunning = false;
        }

        private void InspectLabel_Thread()
        {
            Logger2FileSingleton.Instance.SaveLog("LabelInspectionManager.Started");
            int i = 0;
            while (appIsRunning)
            {
                try
                {
                    bool isData = labelsToBeInspected.TryDequeue(out string data);

                    if (isData && data != null && data.Length > 0)
                    {
                        InspectLabel(data);
                        i++;
                    }
                    else
                    {
                        DeleteOldFiles();
                        i = 0;
                        Thread.Sleep(1000);
                    }

                    if(i >= 30)
                    {
                        DeleteOldFiles();
                        i = 0;
                    }
                }
                catch (Exception ex)
                {
                    Logger2FileSingleton.Instance.SaveLog("LabelInspectionManager.InspectLabel_Thread Exception: " + ex.Message);
                    Thread.Sleep(1000);
                }
            }
            Logger2FileSingleton.Instance.SaveLog("LabelInspectionManager.Stopped!");
        }
        private void InspectLabel(string fileName)
        {
            try
            {
                Logger2FileSingleton.Instance.SaveLog("LabelInspectionManager.InspectLabel(" + fileName + ") START");
                InspectLabel_AnalizeImage(fileName, out ItemData itemData, out string serialNumber, out EnumLabelType labelType);

                if (itemData != null)
                {
                    Workorder wo = uow.WorkorderRepo.Get(serialNumber, itemData.ItemCode);
                    WorkorderLabel workorderLabel = uow.WorkorderLabelRepo.GetOrCreate(wo, serialNumber);
                    bool allTestsPassed = uow.WorkorderLabelInspectionRepo.SaveInspectionResults(workorderLabel, itemData, labelType);
                    uow.WorkorderRepo.UpdateStats(wo, allTestsPassed);
                    ArchiveOrDeleteRawLabel(fileName, allTestsPassed);
                    CopyWorkorderLabelIfNotExists(wo.WorkorderNumber, $"{serialNumber}_{labelType.ToString()}");
                    Logger2FileSingleton.Instance.SaveLog("LabelInspectionManager.InspectLabel(" + fileName + ") END");
                }
                else
                {
                    ArchiveOrDeleteRawLabel(fileName, true);
                    Logger2FileSingleton.Instance.SaveLog("LabelInspectionManager.InspectLabel(" + fileName + ") END - ItemData is NULL");
                }
            }
            catch (Exception ex)
            {
                Logger2FileSingleton.Instance.SaveLog("LabelInspectionManager.InspectLabel(" + fileName + ") Exception: " + ex.Message +
                    Environment.NewLine + " d:" + ex.InnerException?.Message +
                    Environment.NewLine + " s:" + ex.ToString());
            }
        }
        public LabelinspViewModel InspectLabelTest(string fileName)
        {
            LabelinspViewModel packingLabelViewModel = new LabelinspViewModel();
            packingLabelViewModel.WorkorderLabel = new WorkorderLabel();
            packingLabelViewModel.WorkorderLabelInspections = new List<WorkorderLabelInspection>();

            try
            {
                Logger2FileSingleton.Instance.SaveLog("LabelInspectionManager.InspectLabelTest(" + fileName + ") START");
                InspectLabel_AnalizeImage(fileName, out ItemData itemData, out string serialNumber, out EnumLabelType labelType, true);

                if (itemData != null)
                {
                    Workorder wo = uow.WorkorderRepo.Get(serialNumber, itemData.ItemCode);
                    packingLabelViewModel.WorkorderLabel = new WorkorderLabel()
                    {
                        Id = 0,
                        SerialNumber = serialNumber,
                        TimeStamp = DateTime.Now,
                        Workorder = new Workorder() { 
                            ItemCode = wo?.ItemCode ?? "", 
                            ItemName = wo?.ItemName ?? "", 
                            WorkorderNumber = wo?.WorkorderNumber ?? "" 
                        }
                    };
                    packingLabelViewModel.WorkorderLabelInspections = WorkorderLabelInspectionRepo.PrepareInspectionResults(0, itemData, labelType);
                    Logger2FileSingleton.Instance.SaveLog("LabelInspectionManager.InspectLabelTest(" + fileName + ") END");
                }
                else
                {
                    //ArchiveOrDeleteRawLabel(fileName, true);
                    Logger2FileSingleton.Instance.SaveLog("LabelInspectionManager.InspectLabelTest(" + fileName + ") END - ItemData is NULL");
                }
            }
            catch (Exception ex)
            {
                Logger2FileSingleton.Instance.SaveLog("LabelInspectionManager.InspectLabelTest(" + fileName + ") Exception: " + ex.Message +
                    Environment.NewLine + " d:" + ex.InnerException?.Message +
                    Environment.NewLine + " s:" + ex.ToString());
            }

            return packingLabelViewModel;
        }
        private void InspectLabel_AnalizeImage(string fileName, out ItemData itemData, out string serialNumber, out EnumLabelType labelType, bool force = false)
        {
            string[] barcode = fileName.Split('_'); //FILENAME EXAMPLE: 20201002124748_05292260050040276228_Label_S
            string labelType_ = barcode[barcode.Length - 1];
            labelType = GetLabelTypeByChar(labelType_);

            imgProcessing.SetImage(string.Format("{0}{1}.{2}", RAW_LABELS_PATH, fileName, "png"));
            imgProcessing.RotateImage(180);
            string bigBarcode = imgProcessing.BarcodeDetectReadAddFrame_Big("");

            ParseBarcode_RatingLabel(barcode.Length >= 2 ? barcode[1] : "", out string serialNumberRating, out string elc);
            ParseBarcode_PackLabel(bigBarcode, out string serialNumberBarcode, out string pncBarcode);

            itemData = null;
            serialNumber = serialNumberBarcode;

            if (serialNumberRating == serialNumberBarcode || force)
            {
                itemData = uow.ItemDataRepo.GetByItemCodeAndVersion(pncBarcode, elc);

                if (itemData != null)
                {
                    itemData.ActualBarcode = imgProcessing.BarcodeDetectReadAddFrame_Small(itemData.ExpectedBarcodeSmall);
                    itemData.ActualName = imgProcessing.ReadModelName(itemData.ExpectedName);
                    itemData.ActualProductCode = imgProcessing.ReadIKEAProductCode(itemData.ExpectedProductCode);
                    itemData.ActualWeightKG = imgProcessing.ReadWeightBig(itemData.ExpectedWeightKG);
                }
                else
                {
                    itemData = new ItemData()
                    {
                        ItemCode = pncBarcode,
                        IsDataEmpty = true
                    };
                }
                imgProcessing.SaveFinalPreviewImage(string.Format("{0}{1}_{2}.{3}", INSPECTED_LABELS_PATH, serialNumber, labelType_, "png"));
            }
            else
            {
                Logger2FileSingleton.Instance.SaveLog("LabelInspectionManager.InspectLabel_AnalizeImage(" + fileName + ") UNEXPECTED CONTENT. WRONG SERIAL NUMBER!");
            }
        }

        private static EnumLabelType GetLabelTypeByChar(string labelType_)
        {
            return labelType_ == "S" ? EnumLabelType.S : (labelType_ == "R" ? EnumLabelType.R : EnumLabelType.F);
        }
        private void ArchiveOrDeleteRawLabel(string labelFileName, bool allTestsPassed)
        {
            if (allTestsPassed)
            {
                File.Delete(string.Format("{0}{1}.{2}", RAW_LABELS_PATH, labelFileName, "png"));
            }
            else
            {
                File.Move(string.Format("{0}{1}.{2}", RAW_LABELS_PATH, labelFileName, "png"), string.Format("{0}{1}.{2}", INVALID_LABELS_PATH, labelFileName, "png"));
            }
        }       
        private void DeleteOldFiles()
        {
            try
            {
                Directory.GetFiles(INSPECTED_LABELS_PATH)
                    .Select(f => new FileInfo(f))
                    .Where(f => f.CreationTime < DateTime.Now.AddHours(-1))
                    .ToList()
                    .ForEach(f => f.Delete());
            }
            catch (Exception ex)
            {
                Logger2FileSingleton.Instance.SaveLog("LabelInspectionManager.DeleteOldFiles ex:" + ex.ToString());
            }
        }
        private void CopyWorkorderLabelIfNotExists(string orderNumber, string labelFileName)
        {
            string sourceFilePath = string.Format("{0}{1}.{2}", INSPECTED_LABELS_PATH, labelFileName, "png");
            string destFilePath = string.Format("{0}{1}.{2}", WORKORDERS_LABELS_PATH, orderNumber, "png");
            try
            {
                if (!File.Exists(string.Format("{0}{1}.{2}", WORKORDERS_LABELS_PATH, orderNumber, "png")))
                {
                    File.Copy( sourceFilePath, destFilePath, false);
                }
            }
            catch (Exception ex)
            {
                Logger2FileSingleton.Instance.SaveLog("LabelInspectionManager.CopyWorkorderLabelIfNotExists source:" + sourceFilePath);
                Logger2FileSingleton.Instance.SaveLog("LabelInspectionManager.CopyWorkorderLabelIfNotExists dest:" + destFilePath);
                Logger2FileSingleton.Instance.SaveLog("LabelInspectionManager.CopyWorkorderLabelIfNotExists ex:" + ex.ToString());
            }
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
            if (data.Length > 18)
            {
                serialNumber = data.Mid(11, 8);
                elc = data.Mid(1, 9);
            }
            else
            {
                serialNumber = null;
                elc = null;
            }
        }

        public void Update(string fileName)
        {
            labelsToBeInspected.Enqueue(fileName);
        }
    }
}
