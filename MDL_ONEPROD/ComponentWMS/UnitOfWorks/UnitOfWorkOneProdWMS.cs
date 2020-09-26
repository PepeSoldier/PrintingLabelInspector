using MDL_iLOGIS.ComponentConfig.Repos;
using MDL_iLOGIS.ComponentWMS.Repos;
using MDL_ONEPROD.ComponentWMS._Interfaces;
using MDL_ONEPROD.ComponentWMS.Repos;
using MDL_ONEPROD.Repo;
using MDL_ONEPROD.Repo.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Model;

namespace MDL_ONEPROD.ComponentWMS.UnitOfWorks
{
    public class UnitOfWorkOneprodWMS : UnitOfWorkOneprod
    {
        IDbContextOneprodWMS db;
        public UnitOfWorkOneprodWMS(IDbContextOneprodWMS dbContext) : base(dbContext)
        {
            this.db = dbContext;
        }

        private StockUnitRepo stockUnitRepo;
        private WarehouseLocationRepo warehouseLocationRepo;
        private WarehouseRepo warehouseRepo;
        private WarehouseItemRepo warehouseItemRepo;
        private BufforLogRepo bufforLogRepo;

        public StockUnitRepo StockUnitRepo
        {
            get
            {
                stockUnitRepo = (stockUnitRepo != null) ? stockUnitRepo : new StockUnitRepo(db);
                return stockUnitRepo;
            }
        }
        public WarehouseLocationRepo WarehouseLocationRepo
        {
            get
            {
                warehouseLocationRepo = (warehouseLocationRepo != null) ? warehouseLocationRepo : new WarehouseLocationRepo(db, AlertManager.Instance);
                return warehouseLocationRepo;
            }
        }
        public WarehouseRepo WarehouseRepo
        {
            get
            {
                warehouseRepo = (warehouseRepo != null) ? warehouseRepo : new WarehouseRepo(db, AlertManager.Instance);
                return warehouseRepo;
            }
        }
        public WarehouseItemRepo WarehouseItemRepo
        {
            get
            {
                warehouseItemRepo = (warehouseItemRepo != null) ? warehouseItemRepo : new WarehouseItemRepo(db, AlertManager.Instance);
                return warehouseItemRepo;
            }
        }
        public BufforLogRepo BufforLogRepo
        {
            get
            {
                bufforLogRepo = (bufforLogRepo != null) ? bufforLogRepo : new BufforLogRepo(db, AlertManager.Instance, this);
                return bufforLogRepo;
            }
        }
    }
}