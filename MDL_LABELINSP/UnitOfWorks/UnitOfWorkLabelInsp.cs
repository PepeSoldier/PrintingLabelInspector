using MDL_LABELINSP.Interfaces;
using MDL_LABELINSP.Models.Repos;
using MDL_ONEPROD.Repo;

namespace MDL_LABELINSP.UnitOfWorkLabelInsp
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