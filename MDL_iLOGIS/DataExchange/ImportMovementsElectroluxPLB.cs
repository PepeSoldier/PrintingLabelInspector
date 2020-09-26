using MDL_BASE.Interfaces;
using MDL_BASE.Models.MasterData;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_iLOGIS.ComponentWMS.Enums;
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
    public class ImportMovementsElectroluxPLB : ImportMovementsAbstract
    {
        List<MovementHeader> movementHeaders;
        List<MovementData> movementData;

        public ImportMovementsElectroluxPLB(IDbContextiLOGIS db) : base(db)
        {
            movementHeaders = new List<MovementHeader>();
            movementData = new List<MovementData>();
            items = new List<Item>();
        }


        protected override void ReadData()
        {
            using (var reader = new StreamReader(@"C:\Users\kamil\Desktop\WORKPLACE\Elux - PLB\SAP Interfaces\WMS_PRORD.txt"))
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
                var woh = new MovementHeader();

                woh.RECType = values[0];
                woh.DocID = values[1];
                woh.LIFNR = values[2];
                woh.KUNNR = values[3];
                woh.NAME1 = values[4];
                woh.BUDAT = ConvertToDate(values[5]);
                woh.CPUDT = ConvertToDate(values[6]);
                woh.CPUTM = ConvertToTime(values[7]);
                woh.USNAM = values[8];
                
                movementHeaders.Add(woh);
            }
            catch(Exception ex)
            {
                Errors.Add("Read MovementHeader error [" + string.Join(",", values) + "]");
                Errors.Add(ex.Message);
                Logger2FileSingleton.Instance.SaveLog("Read MovementHeader error [" + string.Join(",", values) + "]");
                Logger2FileSingleton.Instance.SaveLog(ex.Message);
            }
        }
        private void ReadData_WorkorderData(string[] values)
        {
            try
            {
                var wod = new MovementData();

                wod.RECType = values[0];
                wod.DocID = values[1];
                wod.MATNR = values[2];
                wod.MAKTX = values[3];
                wod.MENGE = Convert.ToDecimal(values[4].Replace(".", "").Replace(',', '.'));
                wod.MEINS = values[5];
                wod.BWART = values[6];
                wod.LGFrom = values[7];
                wod.ISFrom = values[8];
                wod.LGTo = values[9];
                wod.ISTo = values[10];

                movementData.Add(wod);
            }
            catch (Exception ex)
            {
                Errors.Add("Read MovementData error [" + string.Join(",", values) + "]");
                Errors.Add(ex.Message);
                Logger2FileSingleton.Instance.SaveLog("Read MovementData error [" + string.Join(",", values) + "]");
                Logger2FileSingleton.Instance.SaveLog(ex.Message);
            }
        }

        protected override void AdaptData()
        {
            foreach (var movH in movementHeaders)
            {
                List<MovementData> movH_movementData = movementData.Where(x => x.DocID == movH.DocID).ToList();

                foreach (var movD in movH_movementData)
                {
                    Movement mov = AdaptData_MovementH(movH, movD);
                    if (mov != null) 
                        movements.Add(mov);
                }
            }
        }
        private Movement AdaptData_MovementH(MovementHeader movH, MovementData movD)
        {
            Movement mov = null;
            ItemWMS itemWMS = GetItemWMSByCode(movD.MATNR);
            Warehouse whFrom = GetWarehouseByCode(movD.LGFrom);
            Warehouse whTo = GetWarehouseByCode(movD.LGTo);

            if (itemWMS == null) itemWMS = InsertMissingItemWMS(movD.LGFrom, "", ImportStocksElectroluxPLB.ConvertUnitOfMeasure(movD.MEINS) ,ItemTypeEnum.BuyedItem);
            if (whFrom == null) whFrom = InsertMissingWarehouse(movD.LGFrom, "");
            if (whTo == null) whTo = InsertMissingWarehouse(movD.LGTo, "");

            WarehouseLocation whLocFrom = GetWarehouseLocationByWarehouse(whFrom);
            WarehouseLocation whLocTo = GetWarehouseLocationByWarehouse(whTo);

            if (whLocFrom == null) whLocFrom = InsertMissingWarehouseLocation(whFrom);
            if (whLocTo == null) whLocTo = InsertMissingWarehouseLocation(whTo);

            if (whFrom != null && whTo != null && whLocFrom != null && whLocTo != null && itemWMS != null)
            {
                mov = new Movement();
                mov.Date = movH.CPUDT.Add(movH.CPUTM.TimeOfDay);
                mov.DestinationLocationId = whLocTo.Id;
                mov.DestinationStockUnitSerialNumber = "";
                mov.DestinationWarehouseId = whTo.Id;
                mov.ExternalId = movH.DocID;
                mov.ExternalUserName = movH.USNAM;
                mov.ItemWMS = itemWMS;
                mov.ItemWMSId = itemWMS.Id;
                mov.QtyMoved = movD.MENGE;
                mov.UnitOfMeasure = ImportStocksElectroluxPLB.ConvertUnitOfMeasure(movD.MEINS);
                mov.SourceLocationId = whLocFrom.Id;
                mov.SourceStockUnitSerialNumber = "";
                mov.SourceWarehouseId = whFrom.Id;
                mov.Type = (EnumMovementType)(Convert.ToInt32(movD.BWART));
                mov.UserId = null;
            }
            else
            {
                int errorCount = Errors.Count;

                if(itemWMS == null) Errors.Add("Item not found: " + movD.MATNR + ". Movement not loaded: " + movH.DocID);
                if(whTo == null) Errors.Add("Warehouse not found: " + movD.LGTo + ". Movement not loaded: " + movH.DocID);
                if(whFrom == null) Errors.Add("Warehouse not found: " + movD.LGFrom + ". Movement not loaded: " + movH.DocID);
                if(whFrom == null) Errors.Add("Warehouse not found: " + movD.LGFrom + ". Movement not loaded: " + movH.DocID);
                if(whLocFrom == null) Errors.Add("Warehouse Location not found: " + movD.LGFrom + ". Movement not loaded: " + movH.DocID);
                if(whLocTo == null) Errors.Add("Warehouse Location not found: " + movD.LGTo + ". Movement not loaded: " + movH.DocID);

                while(errorCount < Errors.Count)
                {
                    Logger2FileSingleton.Instance.SaveLog(Errors[errorCount]);
                    errorCount++;
                }
            }

            return mov;
        }

        //private UnitOfMeasure ConvertUnitOfMeasure(string unitOfMeasure)
        //{
        //    switch (unitOfMeasure)
        //    {
        //        case "ST": return UnitOfMeasure.szt;
        //        case "KG": return UnitOfMeasure.kg;
        //        default: return UnitOfMeasure.szt;
        //    }
        //}
    }

    class MovementHeader
    {
        ///<summary>Record Type [Fix to "H"]</summary>
        public string RECType { get; set; }
        ///<summary>Unique doc. number [CHAR(14)]</summary>
        public string DocID { get; set; }
        ///<summary>Supplier code [INT(10)]</summary>
        public string LIFNR { get; set; }
        ///<summary>Customer code [INT(10)]</summary>
        public string KUNNR { get; set; }
        ///<summary>Supplier or customer name [CHAR(35)]</summary>
        public string NAME1 { get; set; }
        ///<summary>Booking date [DATE(8)] YYYYMMDD</summary>
        public DateTime BUDAT { get; set; }
        ///<summary>Insert Date [DATE(8)] YYYYMMDD</summary>
        public DateTime CPUDT { get; set; }
        ///<summary>Insert time [DATE(6)] HHMMSS</summary>
        public DateTime CPUTM { get; set; }
        ///<summary>Transaction code [CHAR(20)]</summary>
        public string TCODE { get; set; }
        ///<summary>Username [CHAR(12)]</summary>
        public string USNAM { get; set; }

        public List<MovementData> MovementDataList { get; set; }

        public MovementHeader() {
            MovementDataList = new List<MovementData>();
        }
    }
    class MovementData
    {
        /// <summary>Record Type [CHAR] (Fix to "P")</summary>
        public string RECType {get;set;}
        ///<summary>Unique doc. number [CHAR(14)]</summary>
        public string DocID { get; set; }
        /// <summary>ANC code [CHAR(18)]</summary>
        public string MATNR { get; set; }
        /// <summary>ANC Description [CHAR(40)]</summary>
        public string MAKTX { get; set; }
        /// <summary>Quantity [DEC(13,3)]</summary>
        public Decimal MENGE { get; set; }
        /// <summary>Unit of Measure [CHAR(3)]</summary>
        public string MEINS { get; set; }
        /// <summary>Movement type [CHAR(3)]</summary>
        public string BWART { get; set; }
        /// <summary>Warehouse From [CHAR(4)]</summary>
        public string LGFrom { get; set; }
        /// <summary>Tipo Stock From [CHAR(1)]</summary>
        public string ISFrom { get; set; }
        /// <summary>MatnrTo [CHAR(18)]</summary>
        public string MatnrTo { get; set; }
        /// <summary>MaktxTo [CHAR(40)]</summary>
        public string MaktxTo { get; set; }
        /// <summary>Warehouse To [CHAR(4)]</summary>
        public string LGTo { get; set; }
        /// <summary>Tipo Stock To [CHAR(1)]</summary>
        public string ISTo { get; set; }
    }
}
