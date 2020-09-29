using MDL_BASE.Interfaces;
using System.Linq;
using MDL_BASE.Models.MasterData;
using System.Collections.Generic;
using MDLX_MASTERDATA._Interfaces;
using XLIB_COMMON.Repo;

namespace MDLX_MASTERDATA.Repos
{
    public class RepoArea : RepoGenericAbstract<Area>
    {
        protected new IDbContextMasterData db;
        
        public RepoArea(IDbContextMasterData db) : base(db)
        {
            this.db = db;
        }

        //public override Area GetById(int id)
        //{
        //    return db.Areas.FirstOrDefault(d => d.Id == id);
        //}

        //public override IQueryable<Area> GetList()
        //{
        //    return db.Areas.Where(x => x.Deleted == false).OrderBy(x => x.Name);
        //}
        //public IQueryable<Area> GetList(Area filter)
        //{
        //    return db.Areas.Where(x => 
        //        (x.Deleted == false) &&
        //        filter.Name == null || x.Name.Contains(filter.Name))
        //    .OrderBy(x => x.Name);
        //}

        //public List<Area> GetAreaOrderList(string prefix)
        //{
        //    return db.Areas.Where(x => x.Name.StartsWith(prefix) && x.Deleted == false).Distinct().Take(5).ToList();
        //}
    }
}