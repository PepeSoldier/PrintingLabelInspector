using MDL_BASE.Interfaces;
using MDL_BASE.Models.MasterData;
using XLIB_COMMON.Repo;
using MDLX_MASTERDATA._Interfaces;
using System.Linq;
using MDLX_MASTERDATA.Entities;

namespace MDLX_MASTERDATA.Repos
{
    //public class ResourceRepo : RepoGenericAbstract<Resource2>
    //{
    //    protected new IDbContextMasterData db;
        
    //    public ResourceRepo(IDbContextMasterData db) : base(db)
    //    {
    //        this.db = db;
    //    }

    //    public override Resource2 GetById(int id)
    //    {
    //        return db.Lines.FirstOrDefault(d => d.Id == id);
    //    }

    //    public override IQueryable<Resource2> GetList()
    //    {
    //        return db.Lines.Where(x => x.Deleted == false).OrderBy(x => x.Name);
    //    }

    //    public IQueryable<Resource2> GetListByArea(int areaId)
    //    {
    //        return db.Lines.Where(x => x.AreaId == areaId && x.Deleted == false).OrderBy(x => x.Name);
    //    }
    //}
}