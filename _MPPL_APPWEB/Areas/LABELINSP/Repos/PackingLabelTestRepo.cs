using _MPPL_WEB_START.Areas.LABELINSP.Interfaces;
using MDL_CORE.ComponentCore.Entities;
using System.Collections.Generic;
using System.Linq;
using XLIB_COMMON.Repo;

namespace _MPPL_WEB_START.Areas.LABELINSP.Repos
{
    public class PackingLabelTestRepo : RepoGenericAbstract<PackingLabelTest>
    {
        protected new IDbContextLabelInsp db;

        public PackingLabelTestRepo(IDbContextLabelInsp db) : base(db)
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

        public List<PackingLabelTest> GetByPackingLabelId(int packingLabelId)
        {
            return db.PackingLabelTests.Where(x => x.PackingLabelId == packingLabelId).ToList();
        }
    }
}