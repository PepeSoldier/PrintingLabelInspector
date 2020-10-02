using MDL_LABELINSP.Entities;
using MDL_LABELINSP.Interfaces;
using System.Linq;
using XLIB_COMMON.Repo;

namespace MDL_LABELINSP.Models.Repos
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