using MDL_CORE.ComponentCore.Entities;
using MDLX_CORE.Model.PrintModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using XLIB_COMMON.Enums;

namespace MDLX_CORE.Model
{
    public class LabelPrintManager
    {
        public static string LabelPath = @"C:\inetpub\";
        private string labelName;
        private List<LabelData> labelDataList;
        private PrintingStatus printingStatus;
        private Printer printer;
        private PrintLabelModelAbstract printerModel = null;

        public LabelPrintManager(Printer printer, LabelData labelData, string labelName)
        {
            this.labelDataList = new List<LabelData>();
            this.labelName = labelName;
            this.printer = printer;
            this.labelDataList.Add(labelData);
        }
        public LabelPrintManager(Printer printer, List<LabelData> labelsData, string labelName)
        {
            this.labelDataList = new List<LabelData>();
            this.labelName = labelName;
            this.printer = printer;
            this.labelDataList = labelsData;
        }

        public PrintingStatus Print()
        {
            printingStatus = InjectPrintLabelModel();
            if (printingStatus == PrintingStatus.ReadyToConnect)
            {
                foreach (LabelData labelData in labelDataList)
                {
                    labelData.PrintDateTime = DateTime.Now.ToString("dd-MM-yyyy;HH:mm:ss");
                    labelData.PrintDate = DateTime.Now.ToString("dd-MM-yyyy");
                    labelData.PrintTime = DateTime.Now.ToString("HH:mm:ss");
                    printerModel.PrepareLabel(ReadLabelDefinition(), labelData);
                    printingStatus = printerModel.Print();
                }
            }
            return printingStatus;
        }
        
        private string ReadLabelDefinition()
        {
            try
            {
                return File.ReadAllText(LabelPath + labelName + "." + printerModel.FileExtension);
            }
            catch
            {
                return null;
            }
        }
        private PrintingStatus InjectPrintLabelModel()
        {
            switch (printer.PrinterType)
            {
                case PrinterType.Zebra: printerModel = new PrintLabelModelZEBRA(printer.IpAdress); break;
                case PrinterType.CAB: printerModel = new PrintLabelModelCAB(printer.IpAdress); break;
                case PrinterType.Laser: printerModel = new PrintLabelModelLaser(printer.Name); break;
            }
            return printerModel == null ? PrintingStatus.NotDefinedPrinterType : PrintingStatus.ReadyToConnect;
        }
    }
}