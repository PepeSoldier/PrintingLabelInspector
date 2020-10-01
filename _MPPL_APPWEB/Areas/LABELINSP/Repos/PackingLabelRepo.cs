using _MPPL_WEB_START.Areas.LABELINSP.Interfaces;
using MDL_CORE.ComponentCore.Entities;
using System.Linq;
using XLIB_COMMON.Repo;

namespace _MPPL_WEB_START.Areas.LABELINSP.Repos
{
    public class PackingLabelRepo : RepoGenericAbstract<PackingLabel>
    {
        protected new IDbContextLabelInsp db;

        public PackingLabelRepo(IDbContextLabelInsp db) : base(db)
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