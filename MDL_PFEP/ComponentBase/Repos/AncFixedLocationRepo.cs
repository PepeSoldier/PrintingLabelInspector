using XLIB_COMMON.Repo;
using MDL_PFEP.Interface;
using MDL_PFEP.Model.PFEP;
using MDL_PFEP.Models.PFEP;
using System.Linq;

namespace MDL_PFEP.Repo.PFEP
{
    public class AncFixedLocationRepo : RepoGenericAbstract<AncFixedLocation>
    {
        protected new IDbContextPFEP db;

        public AncFixedLocationRepo(IDbContextPFEP db)
            : base(db)
        {
            this.db = db;
        }

        public override AncFixedLocation GetById(int id)
        {
            return db.AncFixedLocations.FirstOrDefault(d => d.Id == id);
        }
        public override IQueryable<AncFixedLocation> GetList()
        {
            return db.AncFixedLocations.OrderByDescending(x => x.Id);
        }

        public AncFixedLocation GetByAncId(int ancId)
        {
            return db.AncFixedLocations.FirstOrDefault(d => d.AncId == ancId);
        }
    }
}