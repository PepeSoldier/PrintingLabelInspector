using _MPPL_WEB_START.Areas.LABELINSP.Interfaces;
using _MPPL_WEB_START.Areas.LABELINSP.Repos;
using MDL_BASE.Interfaces;
using MDL_ONEPROD.Repo;

namespace _MPPL_WEB_START.Areas.LABELINSP.UnitOfWorks
{
    public class UnitOfWorkLabelInsp : UnitOfWorkMasterData
    {
        private IDbContextLabelInsp db;

        public UnitOfWorkLabelInsp(IDbContextLabelInsp db) : base(db)
        {
            this.db = db;
        }

        private PackingLabelRepo packingLabelRepo;
        private PackingLabelTestRepo packingLabelTestRepo;

        public PackingLabelTestRepo PackingLabelTestRepo
        {
            get
            {
                packingLabelTestRepo = (packingLabelTestRepo != null) ? packingLabelTestRepo : new PackingLabelTestRepo(db);
                return packingLabelTestRepo;
            }
        }

        public PackingLabelRepo PackingLabelRepo
        {
            get
            {
                packingLabelRepo = (packingLabelRepo != null) ? packingLabelRepo : new PackingLabelRepo(db);
                return packingLabelRepo;
            }
        }
    }
}