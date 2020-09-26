using MDL_BASE.Interfaces;
using MDL_BASE.Models.MasterData;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentWMS.Entities;
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
    public abstract class ImportMovementsAbstract : Import
    {
        protected List<Movement> movements;
        
        public ImportMovementsAbstract(IDbContextiLOGIS db) : base(db) {
            movements = new List<Movement>();
            //productionOrders = new List<ProductionOrder>();
        }
        
        public override void ImportData()
        {
            base.ImportData();
        }
        protected override void InsertToDatabase()
        {            
            using (var transaction = uow.BeginTransaction())
            {
                try
                {
                    uow.MovementRepo.AddOrUpdateRange(movements);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Errors.Add("Movement import saving error." + ex.Message);
                    Logger2FileSingleton.Instance.SaveLog("Movement import saving error." + ex.Message);
                }
            }
        }
    }
}
