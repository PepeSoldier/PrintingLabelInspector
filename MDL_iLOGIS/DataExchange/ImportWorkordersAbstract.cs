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
    public abstract class ImportWorkordersAbstract : Import
    {
        protected List<BomWorkorder> bomWorkorders;
        protected List<ProductionOrder> productionOrders;
        protected string _filePath;

        public ImportWorkordersAbstract(IDbContextiLOGIS db) : base(db) {
            bomWorkorders = new List<BomWorkorder>();
            productionOrders = new List<ProductionOrder>();
        }


        public override void ImportData()
        {
            base.ImportData();
        }
        protected override void InsertToDatabase()
        {
            try
            {
                uow.ProductionOrderRepo.AddOrUpdateRange(productionOrders);
            }
            catch (Exception ex)
            {
                Errors.Add("Production orders saving error. " + ex.Message);
                Logger2FileSingleton.Instance.SaveLog("Production orders saving error. " + ex.Message);
            }

            try
            {
                uow.ProductionOrderRepo.SetDeletedNotUpdatedWorkorders();
            }
            catch (Exception ex)
            {
                Errors.Add("Delefion of old production orders error. " + ex.Message);
                Logger2FileSingleton.Instance.SaveLog("Delefion of old production orders error. " + ex.Message);
            }

            using (var transaction = uow.BeginTransaction())
            {
                try
                {
                    uow.BomWorkorderRepo.TruncateTable();
                    uow.BomWorkorderRepo.AddOrUpdateRange(bomWorkorders);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Errors.Add("Production order bom saving error." + ex.Message);
                    Logger2FileSingleton.Instance.SaveLog("Production order bom saving error." + ex.Message);
                }
            }
        }
    }
}
