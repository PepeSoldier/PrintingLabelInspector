using MDL_BASE.Interfaces;
using System.Linq;
using MDL_BASE.Models.MasterData;
using MDLX_MASTERDATA._Interfaces;
using XLIB_COMMON.Repo;

namespace MDLX_MASTERDATA.Repos
{
    public class RepoWorkstation : RepoGenericAbstract<Workstation>
    {
        protected new IDbContextMasterData db;
        
        public RepoWorkstation(IDbContextMasterData db) : base(db)
        {
            this.db = db;
        }

        public override Workstation GetById(int id)
        {
            return db.Workstations.FirstOrDefault(d => d.Id == id);
        }

        public override IQueryable<Workstation> GetList()
        {
            return db.Workstations.OrderBy(x=>x.Name);
        }
        public IQueryable<Workstation> GetList(Workstation filter)
        {
            return db.Workstations.Where(x=>
                    (filter.LineId == null || x.LineId == filter.LineId) &&
                    (filter.Name == null || x.Name.Contains(filter.Name)) &&
                    (filter.AreaId == null || x.AreaId == filter.AreaId) &&
                    x.Deleted == false)
                .OrderBy(x => x.Name);
        }

        public IQueryable<Workstation> GetListByArea(int areaId)
        {
            return db.Workstations.Where(x => x.AreaId == areaId && x.Deleted == false).OrderBy(x => x.Name);
        }

        public string GetWorkstationsNames(int[] workstationsIds)
        {
            var namesList = db.Workstations.Where(x => workstationsIds.Contains(x.Id)).Select(x => x.Name);
            return string.Join(", ", namesList);
        }
    }
}