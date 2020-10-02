using MDL_LABELINSP.Entities;
using MDL_LABELINSP.Interfaces;
using System.Collections.Generic;
using System.Linq;
using XLIB_COMMON.Repo;

namespace MDL_LABELINSP.Models.Repos
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