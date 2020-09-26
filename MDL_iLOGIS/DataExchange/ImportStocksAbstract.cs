using MDL_BASE.Interfaces;
using MDL_BASE.Models.MasterData;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentWMS.Entities;
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
    public abstract class ImportStocksAbstract : Import
    {
        protected StockUnitModel sum;
        protected List<StockUnit> stockUnits;
        
        public ImportStocksAbstract(IDbContextiLOGIS db) : base(db) {
            stockUnits = new List<StockUnit>();
        }

        public override void ImportData()
        {
            base.ImportData();
        }
        protected override void InsertToDatabase()
        {
            try
            {
                sum.Save();
                //uow.ProductionOrderRepo.AddOrUpdateRange(productionOrders);   
            }
            catch (Exception ex)
            {
                Debug.WriteLine("import stocku nie powiódł się");
                Errors.Add("Stock saving error." + ex.Message);
                Logger2FileSingleton.Instance.SaveLog("Stock saving error." + ex.Message);
            }
        }
    }
}
