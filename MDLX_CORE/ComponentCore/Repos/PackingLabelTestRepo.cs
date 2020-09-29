using MDL_BASE.Interfaces;
using MDL_CORE.ComponentCore.Entities;
using System.Linq;
using XLIB_COMMON.Repo;

namespace MDLX_CORE.ComponentCore.Repos
{
    public class PackingLabelTestRepo : RepoGenericAbstract<PackingLabelTest>
    {
        protected new IDbContextCore db;

        public PackingLabelTestRepo(IDbContextCore db) : base(db)
        {
            this.db = db;
        }

        public override PackingLabelTest GetById(int id)
        {
            return db.PackingLabelTests.FirstOrDefault(d => d.Id == id);
        }

        public override IQueryable<PackingLabelTest> GetList()
        {
            return db.PackingLabelTests.OrderBy(x => x.Id);
        }
    }
}