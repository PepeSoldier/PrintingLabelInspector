using MDL_ONEPROD.Model.Scheduling;
using MDLX_CORE.Model;
using MDLX_CORE.Model.PrintModels;
using System;

using XLIB_COMMON.Enums;

namespace MDL_ONEPROD.ComponetMes.Models
{
    public class PrintLabelManager
    {
        public bool PrepareAndPrintLabel(Workplace wrkp, string woNumber, string ItemCode, int serialNumber, int qty)
        {
            bool labelPritned = false;
            PrintLabelModelAbstract p = null;
            switch (wrkp.PrinterType)
            {
                case PrinterType.Zebra: p = new PrintLabelModelZEBRA(wrkp.PrinterIPv4); break;
                case PrinterType.CAB: p = new PrintLabelModelCAB(wrkp.PrinterIPv4); break;
            }

            if (p != null && wrkp.PrintLabel == true)
            {
                LabelData labelData = new LabelData();
                labelData.Code = ItemCode; //wo.Item.Code; //wrkp.LabelANC;
                labelData.Barcode = "";
                labelData.ClientName = "";
                labelData.MachineNumber = wrkp.LabelName;
                labelData.OrderNo = woNumber;
                labelData.PrintDateTime = DateTime.Now.ToString("dd-MM-yyyy;HH:mm");
                labelData.PrintDate = DateTime.Now.ToString("dd-MM-yyyy");
                labelData.PrintTime = DateTime.Now.ToString("HH:mm:ss");
                labelData.SerialNumber = serialNumber.ToString();
                labelData.Qty = qty.ToString();

                try
                {
                    p.PrepareLabelFromLayout(wrkp.LabelLayoutNo, labelData);
                    p.Print();
                    labelPritned = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    labelPritned = false;
                }
            }

            return labelPritned;
        }
    }
}