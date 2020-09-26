using MDL_BASE.Interfaces;
using MDL_BASE.Models.MasterData;
using MDLX_MASTERDATA._Interfaces;
using System.Linq;
using XLIB_COMMON.Repo;

namespace MDLX_MASTERDATA.Repos
{
    //public class RepoLabourBrigade : RepoGenericAbstract<LabourBrigade>
    //{
    //    protected new IDbContextMasterData db;
        
    //    public RepoLabourBrigade(IDbContextMasterData db) : base(db)
    //    {
    //        this.db = db;
    //    }

    //    public override LabourBrigade GetById(int id)
    //    {
    //        return db.ShiftCodes.FirstOrDefault(d => d.Id == id);
    //    }

    //    public override IQueryable<LabourBrigade> GetList()
    //    {
    //        return db.ShiftCodes.Where(x => x.Deleted == false).OrderBy(x => x.Name);
    //    }
    //}
}