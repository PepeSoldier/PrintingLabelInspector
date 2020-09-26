using MDL_BASE.Interfaces;
using MDL_BASE.Models.MasterData;
using System.Linq;

namespace XLIB_COMMON.Repo.Base
{
    public class JobListRepo //: RepoGenericAbstract<JobList>
    {
        IDbContextCore db;

        public JobListRepo(IDbContextCore db) //: base(db)
        {
            this.db = db;
        }

        //public override JobList GetById(int id)
        //{
        //    return db.JobLists.FirstOrDefault(d => d.Id == id);
        //}
        //public override IQueryable<JobList> GetList()
        //{
        //    return db.JobLists.OrderByDescending(x => x.Id);
        //}
    }
}