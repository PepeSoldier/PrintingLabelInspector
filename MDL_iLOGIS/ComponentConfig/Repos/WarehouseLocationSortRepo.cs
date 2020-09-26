using XLIB_COMMON.Repo;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using System.Linq;
using XLIB_COMMON.Interface;
using MDL_iLOGIS.ComponentConfig.Entities;

namespace MDL_iLOGIS.ComponentConfig.Repos
{
    public class WarehouseLocationSortRepo : RepoGenericAbstract<WarehouseLocationSort>
    {
        protected new IDbContextiLOGIS db;
        public WarehouseLocationSortRepo(IDbContextiLOGIS db, IAlertManager alertManager = null) : base(db)
        {
            this.db = db;
        }

        public override WarehouseLocationSort GetById(int id)
        {
            return db.WarehouseLocationSorts.FirstOrDefault(x => x.Id == id);
        }

        public override IQueryable<WarehouseLocationSort> GetList()
        {
            return db.WarehouseLocationSorts.Where(x => x.Deleted == false).OrderBy(x => x.Id);
        }

    }
}