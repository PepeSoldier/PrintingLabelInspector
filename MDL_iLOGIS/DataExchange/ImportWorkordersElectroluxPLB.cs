using MDL_BASE.Interfaces;
using MDL_BASE.Models.MasterData;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDLX_CORE.ComponentCore.Entities;
using MDLX_CORE.ComponentCore.UnitOfWorks;
using MDLX_MASTERDATA.Entities;
using MDLX_MASTERDATA.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLIB_COMMON.Enums;
using XLIB_COMMON.Model;

namespace MDL_iLOGIS.DataExchange
{
    public class ImportWorkordersElectroluxPLB : ImportWorkordersAbstract
    {
        List<WorkorderHeader> workorderHeaders;
        List<WorkorderData> workorderData;

        public ImportWorkordersElectroluxPLB(IDbContextiLOGIS db) : base(db)
        {
            Init();
            items = new List<Item>();
        }

        protected override void ReadData()
        {
            string[] filePaths = Directory.GetFiles(@"\\plws3807\iLOGIS\iLOGIS_In\");
            foreach (string filePath in filePaths)
            {
                if (Path.GetFileName(filePath).StartsWith("PRORD"))
                {
                    try
                    {
                        _filePath = filePath;
                        Init();
                        ReadFile(filePath);
                        AdaptData();
                        InsertToDatabase();
                        MoveFileToArch(filePath);
                    }
                    catch (Exception ex)
                    {
                        Errors.Add(Path.GetFileName(filePath) + ". Error: " + ex.Message);
                        Logger2FileSingleton.Instance.SaveLog(Path.GetFileName(filePath) + ". Error: " + ex.Message);
                    }
                }
            }
        }
        private void Init()
        {
            workorderHeaders = new List<WorkorderHeader>();
            workorderData = new List<WorkorderData>();
        }
        private void ReadFile(string filePath)
        {
            //string filePath = @"\\plws3807\iLOGIS\iLOGIS_In\PRORD.txt";
            //using (var reader = new StreamReader(@"C:\Users\kamil\Desktop\WORKPLACE\Elux - PLB\SAP Interfaces\PRORD.txt"))
            using (var reader = new StreamReader(filePath))
            {
                List<string> listA = new List<string>();
                List<string> listB = new List<string>();
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(';');

                    if (values.Length > 0 && values[0] == "H")
                    {
                        ReadData_WorkorderHeaders(values);
                    }
                    else if (values.Length > 0 && values[0] == "P")
                    {
                        ReadData_WorkorderData(values);
                    }
                    else
                    {
                        Errors.Add("Undecognized row identifier: " + values[0]);
                        Logger2FileSingleton.Instance.SaveLog("Undecognized row identifier: " + values[0]);
                    }
                }
            }
        }
        private void ReadData_WorkorderHeaders(string[] values)
        {
            try
            {
                var woh = new WorkorderHeader();

                woh.RECType = values[0];
                woh.AUFNR = values[1];
                woh.MATNR = values[2];
                woh.ELC = Convert.ToInt32(values[3].Length > 0 ? values[3] : "0");
                woh.WERKS = values[4];
                woh.LGORT = values[5];
                woh.PSMNG = Convert.ToDecimal(values[6].Replace(".", "").Replace(',', '.'));
                woh.WEMNG = Convert.ToDecimal(values[7].Replace(".", "").Replace(',', '.'));
                woh.MEINS = values[8];
                woh.ARBPL = values[9];
                woh.GSTRP = ConvertToDate(values[10]);
                woh.FSAVD = ConvertToTime(values[11]);
                
                workorderHeaders.Add(woh);
            }
            catch(Exception ex)
            {
                Errors.Add("Read DeliveryData error [" + string.Join(",", values) + "]");
                Errors.Add(ex.Message);
                Logger2FileSingleton.Instance.SaveLog("Read DeliveryData error [" + string.Join(",", values) + "]");
                Logger2FileSingleton.Instance.SaveLog(ex.Message);
            }
        }
        private void ReadData_WorkorderData(string[] values)
        {
            try
            {
                var wod = new WorkorderData();

                wod.RECType = values[0];
                wod.AUFNR = values[1];
                wod.POSNR = Convert.ToInt32(values[2]);
                wod.IDNRK = values[3];
                wod.KTEXT = values[4];
                wod.WERKC = Convert.ToInt32(values[5]);
                wod.LGORC = values[6];
                wod.VORNR = Convert.ToInt32(values[7]);
                wod.BDMNG = Convert.ToDecimal(values[8].Replace(".","").Replace(',','.'));
                wod.DFMENG = Convert.ToDecimal(values[9].Replace(".", "").Replace(',', '.'));
                wod.MEINC = values[10];

                workorderData.Add(wod);
            }
            catch (Exception ex)
            {
                Errors.Add("Read DeliveryData error [" + string.Join(",", values) + "]");
                Errors.Add(ex.Message);
                Logger2FileSingleton.Instance.SaveLog("Read DeliveryData error [" + string.Join(",", values) + "]");
                Logger2FileSingleton.Instance.SaveLog(ex.Message);
            }
        }

        protected override void AdaptData()
        {
            foreach(var wh in workorderHeaders)
            {
                AdaptData_Workorders(wh);
            }
            foreach(var wd in workorderData)
            {
                AdaptData_BomWorkorder(wd);
            }
        }
        private void AdaptData_Workorders(WorkorderHeader wh)
        {
            Resource2 line = uow.ResourceRepo.GetByName(wh.ARBPL).FirstOrDefault();
            Item pnc = GetItemByCode(wh.MATNR);

            if (pnc == null)
            {
                pnc = InsertMissingItem(wh.MATNR, "", ImportStocksElectroluxPLB.ConvertUnitOfMeasure(wh.MEINS), ItemTypeEnum.FinishedItem);
            }

            if (line != null && pnc != null)
            {
                ProductionOrder po = uow.ProductionOrderRepo.GetByOrderNumber(wh.AUFNR);

                if (po == null)
                {
                    po = new ProductionOrder();
                }

                po.LineId = line.Id;
                po.OrderNumber = wh.AUFNR;
                po.StartDate = wh.GSTRP.Add(wh.FSAVD.TimeOfDay);
                po.EndDate = po.StartDate;
                po.PncId = pnc.Id;
                po.QtyPlanned = Convert.ToInt32(wh.PSMNG);
                po.QtyRemain = Convert.ToInt32(wh.WEMNG);
                po.Deleted = false;
                po.LastUpdate = ImportStartTime;
                po.Notice = "";
                po.SerialNoFrom = "0";
                po.SerialNoTo = "0";
                //po.Elc = wh.ELC
                //po.//wh.LGORT
                //po.//wh.MEINS
                //po.//plant = wh.WERKS
                productionOrders.Add(po);
            }
            else
            {
                Errors.Add("Line not found: " + wh.ARBPL + ". Workorder not loaded: " + wh.AUFNR);
                Logger2FileSingleton.Instance.SaveLog("Line not found: " + wh.ARBPL + ". Workorder not loaded: " + wh.AUFNR);
            }
        }
        private void AdaptData_BomWorkorder(WorkorderData wd)
        {
            string prevMARNR = string.Empty;
            WorkorderHeader wh = workorderHeaders.FirstOrDefault(x => x.AUFNR == wd.AUFNR);
            
            Item pnc = GetItemByCode(wh.MATNR);
            Item anc = GetItemByCode(wd.IDNRK);

            if(pnc == null)
                pnc = InsertMissingItem(wh.MATNR, "", ImportStocksElectroluxPLB.ConvertUnitOfMeasure(wh.MEINS), ItemTypeEnum.FinishedItem);
            if(anc == null)
                anc = InsertMissingItem(wd.IDNRK, wd.KTEXT, ImportStocksElectroluxPLB.ConvertUnitOfMeasure(wh.MEINS), ItemTypeEnum.BuyedItem);

            if (wh != null)
            {
                BomWorkorder bomWO = new BomWorkorder();
                bomWO.OrderNo = wd.AUFNR;
                bomWO.LV = 1;
                bomWO.ParentId = pnc.Id;
                bomWO.ChildId = anc.Id;
                bomWO.InsertDate = ImportStartTime;
                bomWO.QtyUsed = wd.BDMNG / wh.PSMNG;
                bomWO.UnitOfMeasure = ImportStocksElectroluxPLB.ConvertUnitOfMeasure(wd.MEINC);
                bomWO.Prefix = wd.POSNR.ToString("D6");

                bomWorkorders.Add(bomWO);
            }
        }
    }

    class WorkorderHeader
    {
        ///<summary>Record Type [Fix to "H"]</summary>
        public string RECType { get; set; }
        ///<summary>Production Order Number</summary>
        public string AUFNR { get; set; }
        ///<summary>Material</summary>
        public string MATNR { get; set; }
        ///<summary>Revision Level [Only numbers - from 00 to 99]</summary>
        public int ELC { get; set; }
        ///<summary>Plant [Fix to 9100]</summary>
        public string WERKS { get; set; }
        ///<summary>Warehouse</summary>
        public string LGORT { get; set; }
        ///<summary>Total Quantity</summary>
        public decimal PSMNG { get; set; }
        ///<summary>Remaining Quantity</summary>
        public decimal WEMNG { get; set; }
        ///<summary>Unit of Measure</summary>
        public string MEINS { get; set; }
        ///<summary>WorkCenter</summary>
        public string ARBPL { get; set; }
        ///<summary>Start Date [Format YYYYMMDD]</summary>
        public DateTime GSTRP { get; set; }
        ///<summary>Start Time [Format HHMMSS]</summary>
        public DateTime FSAVD { get; set; }
    }
    class WorkorderData
    {
        /// <summary>Record Type [CHAR] (Fix to "P")</summary>
        public string RECType {get;set;}
        /// <summary>Production Order Number [NUMC]</summary>
        public string AUFNR {get;set;}
        /// <summary>Position [NUMC] (BOM Position)</summary>
        public int POSNR {get;set;}
        /// <summary>Component [CHAR] (Component used)</summary>
        public string IDNRK {get;set;}
        /// <summary>Component Text [CHAR]</summary>
        public string KTEXT {get;set;}
        /// <summary>Plant [NUMC] (Fix to 9100)</summary>
        public int WERKC {get;set;}
        /// <summary>Warehouse [NUMC] (Consumption Warehouse)</summary>
        public string LGORC {get;set;}
        /// <summary>Operation [NUMC] (Linked to the workstation where is used the component)</summary>
        public int VORNR {get;set;}
        /// <summary>Total Quantity [DEC ]</summary>
        public decimal BDMNG   {get;set;}
        /// <summary>Remaining Quantity [DEC ]</summary>
        public decimal DFMENG {get;set;}
        /// <summary>Unit of Measure [CHAR]</summary>
        public string MEINC {get;set;}
    }
}
