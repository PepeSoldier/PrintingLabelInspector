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

namespace MDL_iLOGIS.DataExchange
{
    public abstract class ExportMovementsAbstract : Export
    {
        protected iLogisMovementManager movModel;
        protected List<Movement> movementsToBeExported;
        private int lastId = 0;

        public ExportMovementsAbstract(IDbContextiLOGIS db) : base(db) {
            movModel = new iLogisMovementManager(db);
            uow = new UnitOfWork_iLogis(db);
            lastId = uow.SystemVariableRepo.GetValueInt("ExportMovementsLastId");
        }

        public override void ExportData()
        {
            base.ExportData();
        }
        protected override void ReadData()
        {
            movementsToBeExported = uow.MovementRepo.GetList().Where(x => x.Id > lastId && x.Date < DateTime.Now).ToList();
        }
        protected override void SavePointerToLastId()
        {
            int maxId = movementsToBeExported.Max(x => x.Id);
            uow.SystemVariableRepo.UpdateValue("ExportMovementsLastId", maxId.ToString(), MDL_CORE.ComponentCore.Enums.EnumVariableType.Int);
        }
    }
}
