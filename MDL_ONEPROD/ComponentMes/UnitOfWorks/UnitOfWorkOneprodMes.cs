using MDL_ONEPROD.Common;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Repo.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Model;

namespace MDL_ONEPROD.Repo
{
    public class UnitOfWorkOneProdMes : UnitOfWorkOneprod
    {
        IDbContextOneprodMes db;
        public UnitOfWorkOneProdMes(IDbContextOneprodMes dbContext) : base(dbContext)
        {
            db = dbContext;
        }
                
        private WorkplaceRepo workplaceRepo;
        private WorkplaceBufferRepo workplaceBufferRepo;
        private ProductionLogRepo productionLogRepo;
        private ProductionLogTraceabilityRepo productionLogTraceabilityRepo;
        private ProductionLogRawMaterialRepo productionLogRawMaterialRepo;


        public WorkplaceRepo WorkplaceRepo
        {
            get
            {
                workplaceRepo = (workplaceRepo != null) ? workplaceRepo : new WorkplaceRepo(db, AlertManager.Instance);
                return workplaceRepo;
            }
        }

        public WorkplaceBufferRepo WorkplaceBufferRepo
        {
            get
            {
                workplaceBufferRepo = (workplaceBufferRepo != null) ? workplaceBufferRepo : new WorkplaceBufferRepo(db, AlertManager.Instance);
                return workplaceBufferRepo;
            }
        }

        public ProductionLogRepo ProductionLogRepo
        {
            get
            {
                productionLogRepo = (productionLogRepo != null) ? productionLogRepo : new ProductionLogRepo(db, AlertManager.Instance);
                return productionLogRepo;
            }
        }

        public ProductionLogTraceabilityRepo ProductionLogTraceabilityRepo
        {
            get
            {
                productionLogTraceabilityRepo = (productionLogTraceabilityRepo != null) ? productionLogTraceabilityRepo : new ProductionLogTraceabilityRepo(db, AlertManager.Instance);
                return productionLogTraceabilityRepo;
            }
        }

        public ProductionLogRawMaterialRepo ProductionLogRawMaterialRepo
        {
            get
            {
                productionLogRawMaterialRepo = (productionLogRawMaterialRepo != null) ? productionLogRawMaterialRepo : new ProductionLogRawMaterialRepo(db, AlertManager.Instance);
                return productionLogRawMaterialRepo;
            }
        }
    }
}