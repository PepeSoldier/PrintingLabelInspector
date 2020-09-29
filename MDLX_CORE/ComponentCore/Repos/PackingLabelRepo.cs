using MDL_BASE.Interfaces;
using MDL_CORE.ComponentCore.Entities;
using System;
using System.Linq;
using XLIB_COMMON.Repo;

namespace MDLX_CORE.ComponentCore.Repos
{
    public class PackingLabelRepo : RepoGenericAbstract<PackingLabel>
    {
        protected new IDbContextCore db;

        public PackingLabelRepo(IDbContextCore db) : base(db)
        {
            this.db = db;
        }

        public override PackingLabel GetById(int id)
        {
            return db.PackingLabels.FirstOrDefault(d => d.Id == id);
        }

        public override IQueryable<PackingLabel> GetList()
        {
            return db.PackingLabels.OrderBy(x => x.Id);
        }

        public PackingLabel GetBySerialNumber(string serialNumber)
        {
            return db.PackingLabels.Where(x => x.SerialNumber == serialNumber).FirstOrDefault();
        }
    }
}