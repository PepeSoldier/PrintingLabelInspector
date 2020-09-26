using MDL_BASE.Interfaces;
using MDL_BASE.Models.MasterData;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_iLOGIS.ComponentWMS.Models;
using MDL_WMS.ComponentConfig.UnitOfWorks;
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
    public abstract class ImportDeliveriesAbstract : Import
    {
        protected DeliveryModel dm;

        public ImportDeliveriesAbstract(IDbContextiLOGIS db) : base(db) {
            dm = new DeliveryModel(db);
        }

        public override void ImportData()
        {
            base.ImportData();
        }
        protected override void InsertToDatabase()
        {
            try
            {
                dm.Save();
            }
            catch(Exception ex)
            {
                Errors.Add("Delivery saving error." + ex.Message);
                Logger2FileSingleton.Instance.SaveLog("Delivery saving error." + ex.Message);
            }
        }
    }
}
