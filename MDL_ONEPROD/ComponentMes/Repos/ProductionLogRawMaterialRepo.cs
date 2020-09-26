using XLIB_COMMON.Repo;
using MDL_ONEPROD.Common;
using MDL_ONEPROD.ComponetMes.Entities;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Interface;

namespace MDL_ONEPROD.Repo.Scheduling
{
    public class ProductionLogRawMaterialRepo: RepoGenericAbstract<ProductionLogRawMaterial>
    {
        protected new IDbContextOneprodMes db;
        
        public ProductionLogRawMaterialRepo(IDbContextOneprodMes db, IAlertManager alertManager) : base(db)
        {
            this.db = db;
        }

        public override ProductionLogRawMaterial GetById(int id)
        {
            return db.ProductionLogRawMaterials.FirstOrDefault(x => x.Id == id);
        }

        internal List<ProductionLogRawMaterial> GetBy_PartCodeSerialNumber(int productionLogId, string partCode, string serialNumber)
        {
            return db.ProductionLogRawMaterials
                    .Where(x => 
                        x.ProductionLogId == productionLogId && 
                        x.ProductionLog.InternalWorkOrderNumber == serialNumber && 
                        x.ProductionLog.Item.Code == partCode)
                    .ToList();
        }

        public override IQueryable<ProductionLogRawMaterial> GetList()
        {
            return db.ProductionLogRawMaterials.Where(x => x.Deleted == false).OrderBy(x => x.Id);
        }

        public IQueryable<ProductionLogRawMaterial> GetByProductionLogId(int productionLogIg)
        {
            return db.ProductionLogRawMaterials.Where(x => x.Deleted == false && x.ProductionLogId == productionLogIg).OrderBy(x => x.Id);
        }

    }
}