using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Repo;
using MDL_ONEPROD.Repo.Scheduling;

namespace MDL_ONEPROD.ComponetExecution.Models
{
    public class UnitOfWorkOneprodExecution : UnitOfWorkONEPROD
    {
        IDbContextOneProdExecution db;
        public UnitOfWorkOneprodExecution(IDbContextOneProdExecution dbContext) : base(dbContext)
        {
            this.db = dbContext;
        }

        private ProductionLogRepo productionLogRepo;
        private ProductionLogRawMaterialRepo productionLogRawMaterialRepo;
        private WorkplaceRepo workplaceRepo;

        public ItemWarehouseLocationRepo ItemWarehouseLocationRepo
        {
            get
            {
                itemWarehouseLocationRepo = (itemWarehouseLocationRepo != null) ? itemWarehouseLocationRepo : new ItemWarehouseLocationRepo(db, AlertManager.Instance);
                return ItemWarehouseLocationRepo;
            }
        }

    }
}