using MDL_LABELINSP.Entities;
using MDL_LABELINSP.Interfaces;
using System.Linq;
using XLIB_COMMON.Repo;

namespace MDL_LABELINSP.Models.Repos
{
    public class WorkordersRepo : RepoGenericAbstract<Workorders>
    {
        protected new IDbContextLabelInsp db;

        public WorkordersRepo(IDbContextLabelInsp db) : base(db)
        {
            this.db = db;
        }

        public override Workorders GetById(int id)
        {
            return db.Workorders.FirstOrDefault(d => d.Id == id);
        }

        public override IQueryable<Workorders> GetList()
        {
            return db.Workorders.OrderBy(x => x.Id);
        }
    }
}