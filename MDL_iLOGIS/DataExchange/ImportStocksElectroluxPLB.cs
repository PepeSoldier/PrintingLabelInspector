using MDL_BASE.Interfaces;
using MDL_BASE.Models.MasterData;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_iLOGIS.ComponentWMS.Enums;
using MDL_iLOGIS.ComponentWMS.Models;
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
    public class ImportStocksElectroluxPLB : ImportStocksAbstract
    {
        List<StockData> stockData;
        string _filePath = string.Empty;

        public ImportStocksElectroluxPLB(IDbContextiLOGIS db) : base(db)
        {
            sum = new StockUnitModel(db);
            Init();
            items = new List<Item>();
            itemWMSs = uow.ItemWMSRepo.GetList().ToList();
        }

        protected override void ReadData()
        {
            //WMS_Stk
            string[] filePaths = Directory.GetFiles(@"\\plws3807\iLOGIS\iLOGIS_In\");
            //string[] filePaths = Directory.GetFiles(@"C:\Users\kamil\Desktop\WORKPLACE\Elux - PLB\SAP Interfaces\");
            foreach (string filePath in filePaths)
            {
                if (Path.GetFileName(filePath).StartsWith("WMS_Stk_"))
                {
                    try
                    {
                        _filePath = filePath;
                        Init();
                        ReadFile(filePath);
                        AdaptData();
                        InsertToDatabase();
                        JoinILogisDataWithSapData();
                        SaveCSVOutput(filePath);
                        //MoveFileToArch(filePath);
                    }
                    catch (Exception ex)
                    {
                        Errors.Add(Path.GetFileName(filePath) + ". Error: " + ex.Message);
                        Logger2FileSingleton.Instance.SaveLog(Path.GetFileName(filePath) + ". Error: " + ex.Message);
                    }
                }
            }
        }
        private void JoinILogisDataWithSapData()
        {
            List<StockUnit> sus = uow.StockUnitRepo.GetGrouppedByWarehouses(new List<string>() { "9103", "9131", "9140", "9141" }).ToList();

            foreach (var su in stockUnits)
            {
                su.CurrentQtyinPackage = sus
                    .Where(x => x.ItemWMSId == su.ItemWMSId && x.Status == su.Status && (x.AccountingWarehouse != null && su.AccountingWarehouse != null && x.AccountingWarehouse.Id == su.AccountingWarehouse.Id))
                    .Sum(x => x.CurrentQtyinPackage);
            }
        }
        private void SaveCSVOutput(string filePath)
        {
            string dir = Path.GetDirectoryName(filePath);


            var csv = new StringBuilder();
            csv.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6}", "Magazyn", "ANC", "Nazwa", "Status", "Ilosc iLOGIS", "Ilosc SAP", "Roznica"));

            foreach (var s in stockUnits)
            {
                csv.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6}", s.AccountingWarehouse.Code, s.ItemWMS.Item.Code, s.ItemWMS.Item.Name, s.Status.ToString(), s.CurrentQtyinPackage, s.WMSQtyinPackage, s.CurrentQtyinPackage - s.WMSQtyinPackage));
            }

            File.WriteAllText(dir + @"\STOCK_COMPARE_REPORT_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv", csv.ToString());
        }

        private void Init()
        {
            stockData = new List<StockData>();
        }
        private void ReadFile(string filePath)
        {
            //using (var reader = new StreamReader(@"C:\Users\kamil\Desktop\WORKPLACE\Elux - PLB\SAP Interfaces\WMS_PRORD.txt"))
            using (var reader = new StreamReader(filePath))
            {
                List<string> listA = new List<string>();
                List<string> listB = new List<string>();
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(';');

                    ReadData_StockData(values);
                }
            }
        }
        private void ReadData_StockData(string[] values)
        {
            try
            {
                var stockD = new StockData();

                stockD.MATNR = values[0];
                stockD.MAKTX = values[1];
                stockD.WERKS = values[2];
                stockD.LGORT = values[3];
                stockD.SOBKZ = Convert.ToBoolean(Convert.ToInt32(values[4].Length > 0 ? values[4] : "0"));
                stockD.INSMK = values[5];
                stockD.MEINS = values[6];
                stockD.MENGE = Convert.ToDecimal(values[7].Replace(".", "").Replace(',', '.'));
                stockD.CPUDT = ConvertToDate(values[8]);
                stockD.CPUTM = ConvertToDate(values[9]);

                stockData.Add(stockD);
            }
            catch (Exception ex)
            {
                Errors.Add("Read StockData error [" + string.Join(",", values) + "]");
                Errors.Add(ex.Message);
                Logger2FileSingleton.Instance.SaveLog("Read StockData error [" + string.Join(",", values) + "]");
                Logger2FileSingleton.Instance.SaveLog(ex.Message);
            }
        }

        protected override void AdaptData()
        {
            foreach(var sd in stockData)
            {
                AdaptData_Stock(sd);
            }
            SaveItemWMSs();
        }
        private void AdaptData_Stock(StockData sd)
        {
            ItemWMS anc = GetItemWMSByCode(sd.MATNR);
            if (anc == null) anc = InsertMissingItemWMS(sd.MATNR, sd.MAKTX, ConvertUnitOfMeasure(sd.MEINS), ItemTypeEnum.BuyedItem);
            //else UpdateItemWMSData(anc, sd.MAKTX, ConvertUnitOfMeasure(sd.MEINS));

            Warehouse wh = GetWarehouseByCode(sd.LGORT);
            if (wh == null) wh = InsertMissingWarehouse(sd.LGORT, "");

            WarehouseLocation wl = GetWarehouseLocationByWarehouse(wh);
            if (wl == null) wl = InsertMissingWarehouseLocation(wh);

            StockUnit su = new StockUnit();
            su.CreatedDate = DateTime.Now;
            su.IncomeDate = DateTime.Now;
            su.Deleted = false;
            su.IsLocated = false;
            su.ItemWMS = anc;
            su.ItemWMSId = anc.Id;
            su.ReservedQty = 0;
            su.CurrentQtyinPackage = 0;
            su.MaxQtyPerPackage = 0;
            su.WMSQtyinPackage = sd.MENGE;
            su.Status = ConvertStatus(sd.INSMK);
            su.SerialNumber = "0";
            su.AccountingWarehouse = wh;
            su.WarehouseLocation = wl;
            su.WarehouseLocationId = wl.Id;
            su.WMSLastCheck = sd.CPUDT.Add(sd.CPUTM.TimeOfDay);

            stockUnits.Add(su);

            //StockUnit su1 = sum.CreateStockUnit_OnVirtual(anc, wh, null, su.WMSQtyinPackage, su.WMSQtyinPackage, "0", su.Status, true);
        }

        public static StatusEnum ConvertStatus(string status)
        {
            switch (status)
            {
                case null: return StatusEnum.Available;
                case "2": return StatusEnum.QualityInspection;
                case "3": return StatusEnum.Blocked;
                default: return StatusEnum.Available;
            }
        }
        public static UnitOfMeasure ConvertUnitOfMeasure(string unitOfMeasure)
        {
            switch (unitOfMeasure)
            {
                case "ST": return UnitOfMeasure.szt;
                case "KG": return UnitOfMeasure.kg;
                case "L": return UnitOfMeasure.l;
                case "M": return UnitOfMeasure.m;
                case "G": return UnitOfMeasure.g;
                case "M2": return UnitOfMeasure.m2;
                case "MM": return UnitOfMeasure.mm;
                case "CM": return UnitOfMeasure.cm;
                case "CS": return UnitOfMeasure.CS;
                case "FT": return UnitOfMeasure.FT;
                case "BOT": return UnitOfMeasure.BOT;
                case "ML": return UnitOfMeasure.ML;
                case "M3": return UnitOfMeasure.m3;
                case "TH": return UnitOfMeasure.TH;
                default: return UnitOfMeasure.szt;
            }
        }
    }

    class StockData
    {
        ///<summary>Material CHAR[18]</summary>
        public string MATNR { get; set; }
        /// <summary>ANC Description[CHAR(40)]</summary>
        public string MAKTX { get; set; }
        ///<summary>Plant [Fix to 9100]</summary>
        public string WERKS { get; set; }
        /// <summary>Warehouse[CHAR(4)]</summary>
        public string LGORT { get; set; }
        /// <summary>Special stock[CHAR(1)]</summary>
        public bool SOBKZ { get; set; }
        /// <summary>Tipo Stock[CHAR(1)]"Null = Free, 2 = Quality check, 3 = Blocked Stock"</summary>
        public string INSMK { get; set; }
        /// <summary>Quantity[DEC(13,3)]</summary>
        public decimal MENGE { get; set; }
        /// <summary>Unit of Measure[CHAR(3)]</summary>
        public string MEINS { get; set; }
        ///<summary>Insert date  [DATS] YYYYMMDD</summary>
        public DateTime CPUDT { get; set; }
        ///<summary>Insert time [TIMS] HHmmss</summary>
        public DateTime CPUTM { get; set; }
    }
}
