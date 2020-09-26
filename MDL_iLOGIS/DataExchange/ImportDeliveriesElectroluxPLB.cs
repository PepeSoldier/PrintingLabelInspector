using MDL_BASE.Interfaces;
using MDL_BASE.Models.IDENTITY;
using MDL_BASE.Models.MasterData;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_iLOGIS.ComponentWMS.Enums;
using MDLX_CORE.ComponentCore.Entities;
using MDLX_CORE.ComponentCore.UnitOfWorks;
using MDLX_MASTERDATA.Entities;
using MDLX_MASTERDATA.Enums;
using MDLX_MASTERDATA.Models;
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
    public class ImportDeliveries_ElectroluxPLB : ImportDeliveriesAbstract
    {
        protected List<DeliveryHeader> deliveryHeaders;
        protected List<DeliveryData> deliveryData;
        string _filePath = string.Empty;

        public ImportDeliveries_ElectroluxPLB(IDbContextiLOGIS db) : base(db)
        {
            Init();
            items = new List<Item>();
        }


        protected override void ReadData()
        {
            //string[] filePaths = Directory.GetFiles(@"C:\Users\kamil\Desktop\WORKPLACE\Elux - PLB\SAP Interfaces\");
            string[] filePaths = Directory.GetFiles(@"\\plws3807\iLOGIS\iLOGIS_In\");
            foreach (string filePath in filePaths)
            {
                if (Path.GetFileName(filePath).StartsWith("WMS_Del_"))
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
            deliveryHeaders = new List<DeliveryHeader>();
            deliveryData = new List<DeliveryData>();
        }
        private void ReadFile(string filePath)
        {
            using (var reader = new StreamReader(filePath)) //@"\\plws3807\iLOGIS\iLOGIS_In\Del.csv"))
            {
                List<string> listA = new List<string>();
                List<string> listB = new List<string>();
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(';');

                    if (values.Length > 0 && values[0] == "H")
                    {
                        ReadData_DeliveryHeaders(values);
                    }
                    else if (values.Length > 0 && values[0] == "P")
                    {
                        ReadData_DeliveryData(values);
                    }
                    else
                    {
                        Errors.Add(Path.GetFileName(filePath) + ". Undecognized row identifier: " + values[0]);
                        Logger2FileSingleton.Instance.SaveLog(Path.GetFileName(filePath) + ". Undecognized row identifier: " + values[0]);
                    }
                }
            }
        }
        
        private void ReadData_DeliveryHeaders(string[] values)
        {
            try
            {
                var dh = new DeliveryHeader();

                dh.RECType = values[0];
                dh.DelID = values[1];
                dh.LIFNR = values[2];
                dh.NAME1 = values[3];
                dh.XBLNR = values[4];
                dh.BLDAT = ConvertToDate(values[5]);
                dh.BUDAT = ConvertToDate(values[6]);
                dh.CPUDT = ConvertToDate(values[7]);
                dh.CPUTM = ConvertToTime(values[8]);
                dh.USNAM = values[10];

                deliveryHeaders.Add(dh);
            }
            catch(Exception ex)
            {
                Errors.Add(Path.GetFileName(_filePath) + ". Read DeliveryHeader error [" + string.Join(",", values) + "]");
                Errors.Add(ex.Message);
                Logger2FileSingleton.Instance.SaveLog(Path.GetFileName(_filePath) + ". Read DeliveryHeader error [" + string.Join(",", values) + "]");
                Logger2FileSingleton.Instance.SaveLog(ex.Message);
            }
        }
        private void ReadData_DeliveryData(string[] values)
        {
            try
            {
                var dd = new DeliveryData();

                dd.RECType = values[0];
                dd.DelID = values[1];
                dd.MATNR = values[2];
                dd.MAKTX = values[3];
                dd.MENGE = Convert.ToDecimal(values[4].Replace(".", "").Replace(',', '.'));
                dd.MEINS = values[5];
                dd.BWART = values[6];
                dd.LGORT = values[7];
                dd.INSMK = values[8];
                dd.SOBKZ = Convert.ToBoolean(Convert.ToInt32(values[9].Length > 0 ? values[9] : "0"));

                deliveryData.Add(dd);
            }
            catch(Exception ex)
            {
                Errors.Add(Path.GetFileName(_filePath) + "Read DeliveryData error [" + string.Join(",", values) + "]");
                Errors.Add(ex.Message);
                Logger2FileSingleton.Instance.SaveLog(Path.GetFileName(_filePath) + "Read DeliveryData error [" + string.Join(",", values) + "]");
                Logger2FileSingleton.Instance.SaveLog(ex.Message);
            }
        }

        protected override void AdaptData()
        {
            foreach(var delH in deliveryHeaders)
            {
                Delivery delivery = AdaptData_Delivery(delH);
                delivery.DeliveryItems = new List<DeliveryItem>();
                List<DeliveryData> delH_deliveryData = deliveryData.Where(x => x.DelID == delH.DelID).ToList();

                foreach (var delD in delH_deliveryData)
                {
                    var di_temp = AdaptData_DeliveryItem(delH, delD);

                    if(di_temp != null)
                        delivery.DeliveryItems.Add(di_temp);
                }

                //deliveries.Add(delivery);
                dm.DeliveryAdd(delivery);
            }
            SaveItemWMSs();
        }
        private Delivery AdaptData_Delivery(DeliveryHeader delH)
        {
            Contractor supplier = GetContractorByCode(delH.LIFNR);

            if (supplier == null)
                supplier = InsertMissingContractor(delH.LIFNR, delH.NAME1);


            Delivery del = uow.DeliveryRepo.Get(delH.XBLNR, delH.LIFNR, delH.DelID);

            if (del == null)
            {
                del = new Delivery();
                del.EnumDeliveryStatus = EnumDeliveryStatus.Created;
            }

            del.DocumentDate = delH.BLDAT;
            del.DocumentNumber = delH.XBLNR;
            del.StampTime = delH.CPUDT.Add(delH.CPUTM.TimeOfDay);
            del.SupplierId = supplier.Id;
            del.Deleted = false;
            del.ExternalId = delH.DelID;
            del.ExternalUserName = delH.USNAM;
           
            return del;
        }
        private DeliveryItem AdaptData_DeliveryItem(DeliveryHeader delH, DeliveryData delD)
        {
            UnitOfMeasure uom = ImportStocksElectroluxPLB.ConvertUnitOfMeasure(delD.MEINS);
            ItemWMS itemWMS = GetItemWMSByCode(delD.MATNR);
            decimal qty = delD.MENGE;

            if (itemWMS == null) 
                itemWMS = InsertMissingItemWMS(delD.MATNR, delD.MAKTX, ImportStocksElectroluxPLB.ConvertUnitOfMeasure(delD.MEINS), ItemTypeEnum.BuyedItem);
            if (itemWMS.Item.UnitOfMeasure != uom)
                //UpdateItemWMSData(itemWMS, itemWMS.Item.Name, uom);
                qty = ConverterUoM.Convert(qty, uom, itemWMS.Item.UnitOfMeasure, itemWMS.Item.UnitOfMeasures.ToList());

            DeliveryItem delItem = null;

            if (qty != 0)
            {
                delItem = new DeliveryItem();
                //delItem.DeliveryId = 0;
                //delItem.UserId = 
                delItem.AdminEntry = true;
                delItem.OperatorEntry = false;
                delItem.ItemWMS = itemWMS;
                delItem.ItemWMSId = itemWMS.Id;
                delItem.NumberOfPackages = 1;
                delItem.QtyInPackage = qty;
                delItem.TotalQty = qty;
                delItem.WasPrinted = false;
                delItem.Deleted = false;
                delItem.UnitOfMeasure = itemWMS.Item.UnitOfMeasure;
                delItem.StockStatus = ImportStocksElectroluxPLB.ConvertStatus(delD.INSMK);
                delItem.IsSpecialStock = delD.SOBKZ;
                delItem.DestinationWarehouseCode = delD.LGORT;
                delItem.MovementType = ConvertNovementType(delD.BWART);
            }
            else
            {
                Logger2FileSingleton.Instance.SaveLog("AdaptData_DeliveryItem. Qty = 0. " + itemWMS.Item.Code + " del:" + delH.XBLNR);
            }
            
            return delItem;
        }

        //private UnitOfMeasure ConvertUnitOfMeasure(string unitOfMeasure)
        //{
        //    switch (unitOfMeasure)
        //    {
        //        case "ST": return UnitOfMeasure.szt;
        //        case "KG": return UnitOfMeasure.kg;
        //        case "L": return UnitOfMeasure.l;
        //        case "M": return UnitOfMeasure.m;
        //        case "G": return UnitOfMeasure.g;
        //        default: return UnitOfMeasure.szt;
        //    }
        //}
        //private StatusEnum ConvertStatus(string status)
        //{
        //    switch (status)
        //    {
        //        case null: return StatusEnum.Available;
        //        case "2": return StatusEnum.QualityInspection;
        //        case "3": return StatusEnum.Blocked;
        //        default: return StatusEnum.Available;
        //    }
        //}
        private EnumMovementType ConvertNovementType(string movementType)
        {
            EnumMovementType movType = EnumMovementType.Unassigned;
            int movementInt = 0;
            if(int.TryParse(movementType, out movementInt))
            {
                movType = (EnumMovementType)movementInt;
            }

            return movType;
        }
    }

    public class DeliveryHeader
    {
        ///<summary>Record Type [Fix to "H"]</summary>
        public string RECType { get; set; }
        ///<summary>Delivery ID [CHAR(14)]</summary>
        public string DelID { get; set; }
        ///<summary>Supplier code [NUMC(10)]</summary>
        public string LIFNR { get; set; }
        ///<summary>Supplier name [CHAR(35)]</summary>
        public string NAME1 { get; set; }
        ///<summary>Document number [CHAR(16)]</summary>
        public string XBLNR { get; set; }
        ///<summary>Document date [DATS] YYYYMMDD</summary>
        public DateTime BLDAT { get; set; }
        ///<summary>Booking date [DATS] YYYYMMDD</summary>
        public DateTime BUDAT { get; set; }
        ///<summary>Insert date  [DATS] YYYYMMDD</summary>
        public DateTime CPUDT { get; set; }
        ///<summary>Insert time [TIMS] HHmmss</summary>
        public DateTime CPUTM { get; set; }
        ///<summary>Username [CHAR(12)]</summary>
        public string USNAM { get; set; }
    }

    public class DeliveryData
    {
        /// <summary>Record Type [CHAR] (Fix to "P")</summary>
        public string RECType { get; set; }
        ///<summary>Delivery ID [CHAR(14)]</summary>
        public string DelID { get; set; }
        /// <summary>ANC code[CHAR(18)]</summary>
        public string MATNR { get; set; }
        /// <summary>ANC Description[CHAR(40)]</summary>
        public string MAKTX { get; set; }
        /// <summary>Quantity[DEC(13,3)]</summary>
        public decimal MENGE { get; set; }
        /// <summary>Unit of Measure[CHAR(3)]</summary>
        public string MEINS { get; set; }
        /// <summary>Movement type[CHAR(3)]</summary>
        public string BWART { get; set; }
        /// <summary>Warehouse[CHAR(4)]</summary>
        public string LGORT { get; set; }
        /// <summary>Tipo Stock[CHAR(1)]"Null = Free, 2 = Quality check, 3 = Blocked Stock"</summary>
        public string INSMK { get; set; }
        /// <summary>Special stock[CHAR(1)]</summary>
        public bool SOBKZ { get; set; }
    }
}
