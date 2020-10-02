using MDL_LABELINSP.Entities;
using MDL_LABELINSP.Interfaces;
using System.Linq;
using XLIB_COMMON.Repo;

namespace MDL_LABELINSP.Models.Repos
{
    public class ExpectedValuesRepo : RepoGenericAbstract<ExpectedValues>
    {
        protected new IDbContextLabelInsp db;

        public ExpectedValuesRepo(IDbContextLabelInsp db) : base(db)
        {
            this.db = db;
        }

        public override ExpectedValues GetById(int id)
        {
            return db.ExpectedValues.FirstOrDefault(d => d.Id == id);
        }

        public override IQueryable<ExpectedValues> GetList()
        {
            return db.ExpectedValues.OrderBy(x => x.Id);
        }

    }
}