using MDL_BASE.Interfaces;
using MDL_CORE.ComponentCore.Entities;
using MDL_iLOGIS.ComponentCore.Enums;
using MDL_iLOGIS.ComponentWMS.Entities;
using MDLX_CORE.ComponentCore.UnitOfWorks;
using MDLX_CORE.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using XLIB_COMMON.Model;
using XLIB_COMMON.Repo.Base;
using XLIB_COMMON.Repo.IDENTITY;

namespace MDL_iLOGIS.ComponentCore.Models
{
    public class iLogisPrintLabelManager
    {
        readonly IDbContextCore db;
        readonly bool print;
        public string userName;
        Dictionary<string, string> barcodeTemplates;

        public iLogisPrintLabelManager(IDbContextCore db, string userName, bool print = true)
        {
            barcodeTemplates = new Dictionary<string, string>();
            this.db = db;
            this.print = print;
            this.userName = userName;
        }

        public void PrintLabelForStockUnit(StockUnit stockUnit, bool? _print = null)
        {
            if ((_print == null || _print.Value == true) && print)
            {
                string labelLayout = "PLB_ZEBRA_CUMMUTIVE";
                //Printer printer = new UnitOfWorkCore(db).PrinterRepo.GetByName("Brother DCP-1610W series");
                int printerId = 0;

                if (stockUnit.WarehouseLocation.Warehouse.Code.Contains("9140"))
                {
                    printerId = new SystemVariableRepo(db).GetValueInt("TechBuforPrinterId");
                }
                else
                {
                    printerId = new SystemVariableRepo(db).GetValueInt("IncomingPrinterId");
                }

                if (printerId != 0)
                {
                    Printer printer = new UnitOfWorkCore(db).PrinterRepo.GetById(printerId);
                    if (printer != null)
                    {
                        LabelData labelData = PrepareLabelData(stockUnit, GetBarcodeTemplate(stockUnit.WarehouseLocation.WarehouseId));
                        LabelPrintManager lpm = new LabelPrintManager(printer, labelData, labelLayout);
                        //Task.Run(() => plm.Print());
                        lpm.Print();
                    }
                }
            }
        }
        public void PrintSmallLabelForStockUnit(StockUnit stockUnit, bool details, string userId, bool? _print = null, LabelExtraData extraData = null)
        {
            if ((_print == null || _print.Value == true) && print)
            {
                string labelLayout = details? "PLB_ZEBRA_QTY_REPRINT_DETAILS" : "PLB_ZEBRA_QTY_REPRINT";
                int printerId = new SystemVariableRepo(db).GetValueInt("MobilePrinterId", userId);
                Printer printer = new UnitOfWorkCore(db).PrinterRepo.GetById(printerId);
                if (printer != null)
                {
                    LabelData labelData = PrepareLabelData(stockUnit, GetBarcodeTemplate(stockUnit.WarehouseLocation.WarehouseId));
                    AppendExtraData(labelData, extraData);

                    LabelPrintManager lpm = new LabelPrintManager(printer, labelData, labelLayout);
                    lpm.Print();
                }
            }
        }

        private LabelData PrepareLabelData(StockUnit stockUnit, string barcodeTemplate)
        {
            BarcodeManager barcodeManager = new BarcodeManager();
            barcodeManager.ItemCode = stockUnit.ItemWMS.Item.Code;
            barcodeManager.Qty = stockUnit.CurrentQtyinPackage;
            barcodeManager.Location = stockUnit.WarehouseLocation.Name;
            barcodeManager.Generate(stockUnit.SerialNumber, barcodeTemplate); //"CCCCCCCCCQQQQQQDDDDDLLLLLLLLLSSSSSSSSSSSS");

            string accountingWarehouseName = stockUnit.WarehouseLocation.Warehouse.AccountingWarehouse != null ? stockUnit.WarehouseLocation.Warehouse.AccountingWarehouse.Code + " - " : "";

            LabelData labelData = new LabelData();
            labelData.Location = stockUnit.WarehouseLocation.NameFormatted;
            labelData.Code = PutSpaceBeforeLastFourChars(stockUnit.ItemWMS.Item.Code);
            labelData.Name = stockUnit.ItemWMS.Item.Name;
            labelData.Weight = GetWeight(stockUnit);
            labelData.PrintDateTime = stockUnit.CreatedDate.ToString("dd-MM-yyyy;HH:mm:ss");
            labelData.PrintDate = DateTime.Now.ToString("dd-MM-yyyy");
            labelData.PrintTime = DateTime.Now.ToString("HH:mm:ss");
            labelData.Qty = stockUnit.CurrentQtyinPackage.ToString("0.#####");
            labelData.WarehouseName = accountingWarehouseName + stockUnit.WarehouseLocation.Warehouse.Name.RemoveDiacritics();
            labelData.SerialNumber = stockUnit.SerialNumber;
            labelData.Barcode = barcodeManager.Barcode;
            labelData.UserName = userName;
            
            return labelData;
        }
        private static void AppendExtraData(LabelData labelData, LabelExtraData extraData)
        {
            if (extraData != null)
            {
                labelData.ExtraLabel1 = extraData.ExtraLabel1;
                labelData.ExtraLabel2 = extraData.ExtraLabel2;
                labelData.ExtraLabel3 = extraData.ExtraLabel3;
                labelData.ExtraData1 = extraData.ExtraData1;
                labelData.ExtraData2 = extraData.ExtraData2;
                labelData.ExtraData3 = extraData.ExtraData3;
            }
        }

        private static string GetWeight(StockUnit stockUnit)
        {
            string returnWeight = "";
            if(stockUnit.ItemWMS.Weight != 0)
            {
                returnWeight = (stockUnit.ItemWMS.Weight * stockUnit.CurrentQtyinPackage).ToString("0.00###") + " kg";
            }
            return returnWeight;
        }

        private static string PutSpaceBeforeLastFourChars(string text)
        {
            if(text != null)
            {
                if (text.Length > 4)
                {
                    text = text.Insert(text.Length - 4," ");
                }
            }
            return text;
        }


        private string GetBarcodeTemplate(int warehouseId)
        {
            string variableName = "BarcodeTemplate_WH_" + warehouseId;
            string barcodeTemplate = string.Empty;
            bool exists = barcodeTemplates.ContainsKey(variableName);
            
            if (!exists) 
            {
                SystemVariableRepo repo = new SystemVariableRepo(db);
                barcodeTemplate = repo.GetValueString(variableName);

                if(barcodeTemplate.Length <= 0)
                {
                    barcodeTemplate = repo.GetValueString("BarcodeTemplate_WH");
                }

                barcodeTemplates.Add(variableName, barcodeTemplate);
            }
            else
            {
                barcodeTemplates.TryGetValue(variableName, out barcodeTemplate);
            }

            return barcodeTemplate;
        }
    }
}
