using XLIB_COMMON.Repo;
using MDL_ONEPROD.Common;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Interface;
using MDL_ONEPROD.ComponentMes.Models;
using MDL_ONEPROD.ComponentMes._Interfaces;
using MDL_ONEPROD.Model.Scheduling.Interface;
using MDL_ONEPROD.ComponentMes.Etities;

namespace MDL_ONEPROD.Repo.Scheduling
{
    public class ProductionLogTraceabilityRepo: RepoGenericAbstract<ProductionLogTraceability>
    {
        protected new IDbContextOneprodMes db;
        
        public ProductionLogTraceabilityRepo(IDbContextOneprodMes db, IAlertManager alertManager) : base(db)
        {
            this.db = db;
        }

        public override ProductionLogTraceability GetById(int id)
        {
            return db.ProductionLogTraceabilities.FirstOrDefault(x => x.Id == id);
        }
        public override IQueryable<ProductionLogTraceability> GetList()
        {
            return db.ProductionLogTraceabilities.Where(x => x.Deleted == false).OrderBy(x => x.Id);
        }
        public IQueryable<ProductionLogTraceability> GetByParentId(int parentProductionLogId)
        {
            return db.ProductionLogTraceabilities.Where(x => x.ParentId == parentProductionLogId);
        }

        public IQueryable<ProductionLogTraceability> GetByChildId(int childId)
        {
            return db.ProductionLogTraceabilities.Where(x => x.ChildId == childId);
        }

        public int DeleteById(int id)
        {
            ProductionLogTraceability plt = db.ProductionLogTraceabilities.FirstOrDefault(p => p.Id == id);
            if (plt != null)
            {
                plt.Deleted = true;
                return AddOrUpdate(plt);
            }
            return 0;
        }

    }
}