using MDL_BASE.Interfaces;
using MDL_BASE.Models.MasterData;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_WMS.ComponentConfig.UnitOfWorks;
using MDLX_CORE.ComponentCore.UnitOfWorks;
using MDLX_MASTERDATA.Entities;
using MDLX_MASTERDATA.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_iLOGIS.DataExchange
{
    public abstract class Export
    {
        public List<string> Errors { get; protected set; }
        protected DateTime ExportStartTime;
        protected UnitOfWork_iLogis uow;
   
        public Export(IDbContextiLOGIS db)
        {
            uow = new UnitOfWork_iLogis(db);
            Errors = new List<string>();
        }

        public virtual void ExportData()
        {
            ExportStartTime = DateTime.Now;
            ReadData();
            AdaptData();
            SaveData();
            SavePointerToLastId();
        }
        protected abstract void ReadData();
        protected abstract void AdaptData();
        protected abstract void SaveData();
        protected abstract void SavePointerToLastId();
    }
}
