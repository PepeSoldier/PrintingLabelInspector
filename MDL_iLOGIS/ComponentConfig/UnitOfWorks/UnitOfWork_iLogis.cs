using MDLX_CORE.ComponentCore.UnitOfWorks;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Repos;
using MDL_iLOGIS.ComponentWMS.Repos;
using MDL_ONEPROD.ComponentWMS.Repos;
using MDL_ONEPROD.Repo;
using MDL_WMS.ComponentConfig.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDL_iLOGIS.ComponentWMS._Interfaces;
using MDL_iLOGIS.ComponentWHDOC.Entities;
using MDL_iLOGIS.ComponentWHDOC.Repos;

namespace MDL_WMS.ComponentConfig.UnitOfWorks
{
   public class UnitOfWork_iLogis : UnitOfWorkCore
    {
        IDbContextiLOGIS db;
        ILocataionManager locataionManager;

        public UnitOfWork_iLogis(IDbContextiLOGIS dbContext, ILocataionManager locataionManager = null) : base(dbContext)
        {
            db = dbContext;
            this.locataionManager = locataionManager;
        }

        private ItemWMSRepo itemWMSRepo;
        private PackageRepo packageRepo;
        private PackageItemRepo packageItemRepo;
        private WarehouseRepo warehouseRepo;
        private WarehouseLocationRepo warehouseLocationRepo;
        private WarehouseLocationTypeRepo warehouseLocationTypeRepo;
        private WarehouseLocationSortRepo warehouseLocationSortRepo;
        private WorkstationItemRepo workstationItemRepo;
        private AutomaticRuleRepo automaticRuleRepo;
        private StockUnitRepo stockUnitRepo;
        private PickingListItemRepo pickingListItemRepo;
        private PickingListRepo pickingListRepo;
        private TransporterRepo transporterRepo;
        private TransporterLogRepo transporterLogRepo;
        private DeliveryListRepo deliveryListRepo;
        private DeliveryListItemRepo deliveryListItemRepo;
        private DeliveryRepo deliveryRepo;
        private DeliveryItemRepo deliveryItemRepo;
        private MovementRepo movementRepo;
        //private WarehouseItemRepo warehouseItemRepo;

        //WHDoc
        private WhDocumentWZRepo whDocumentWZRepo;
        private WhDocumentCMRRepo whDocumentCMRRepo;
        private WhDocumentItemRepo whDocumentItemRepo;

        public ItemWMSRepo ItemWMSRepo
        {
            get
            {
                itemWMSRepo = (itemWMSRepo != null) ? itemWMSRepo : new ItemWMSRepo(db);
                return itemWMSRepo;
            }
        }
        public PackageRepo PackageRepo
        {
            get
            {
                packageRepo = (packageRepo != null) ? packageRepo : new PackageRepo(db);
                return packageRepo;
            }
        }
        public PackageItemRepo PackageItemRepo
        {
            get
            {
                packageItemRepo = (packageItemRepo != null) ? packageItemRepo : new PackageItemRepo(db);
                return packageItemRepo;
            }
        }
        public WarehouseRepo WarehouseRepo
        {
            get
            {
                warehouseRepo = (warehouseRepo != null) ? warehouseRepo : new WarehouseRepo(db);
                return warehouseRepo;
            }
        }
        public WarehouseLocationRepo WarehouseLocationRepo
        {
            get
            {
                warehouseLocationRepo = (warehouseLocationRepo != null) ? warehouseLocationRepo : new WarehouseLocationRepo(db);
                return warehouseLocationRepo;
            }
        }
        public WarehouseLocationTypeRepo WarehouseLocationTypeRepo
        {
            get
            {
                warehouseLocationTypeRepo = (warehouseLocationTypeRepo != null) ? warehouseLocationTypeRepo : new WarehouseLocationTypeRepo(db);
                return warehouseLocationTypeRepo;
            }
        }
        public WarehouseLocationSortRepo WarehouseLocationSortRepo
        {
            get
            {
                warehouseLocationSortRepo = (warehouseLocationSortRepo != null) ? warehouseLocationSortRepo : new WarehouseLocationSortRepo(db);
                return warehouseLocationSortRepo;
            }
        }
        public WorkstationItemRepo WorkstationItemRepo
        {
            get
            {
                workstationItemRepo = (workstationItemRepo != null) ? workstationItemRepo : new WorkstationItemRepo(db);
                return workstationItemRepo;
            }
        }
        public AutomaticRuleRepo AutomaticRuleRepo
        {
            get
            {
                automaticRuleRepo = (automaticRuleRepo != null) ? automaticRuleRepo : new AutomaticRuleRepo(db);
                return automaticRuleRepo;
            }
        }
        public StockUnitRepo StockUnitRepo
        {
            get
            {
                stockUnitRepo = (stockUnitRepo != null) ? stockUnitRepo : new StockUnitRepo(db, locataionManager);
                return stockUnitRepo;
            }
        }
        public PickingListItemRepo PickingListItemRepo
        {
            get
            {
                pickingListItemRepo = (pickingListItemRepo != null) ? pickingListItemRepo : new PickingListItemRepo(db);
                return pickingListItemRepo;
            }
        }
        public PickingListRepo PickingListRepo
        {
            get
            {
                pickingListRepo = (pickingListRepo != null) ? pickingListRepo : new PickingListRepo(db);
                return pickingListRepo;
            }
        }
        public TransporterRepo TransporterRepo
        {
            get
            {
                transporterRepo = (transporterRepo != null) ? transporterRepo : new TransporterRepo(db);
                return transporterRepo;
            }
        }
        public TransporterLogRepo TransporterLogRepo
        {
            get
            {
                transporterLogRepo = (transporterLogRepo != null) ? transporterLogRepo : new TransporterLogRepo(db);
                return transporterLogRepo;
            }
        }
        public DeliveryListRepo DeliveryListRepo
        {
            get
            {
                deliveryListRepo = (deliveryListRepo != null) ? deliveryListRepo : new DeliveryListRepo(db);
                return deliveryListRepo;
            }
        }
        public DeliveryListItemRepo DeliveryListItemRepo
        {
            get
            {
                deliveryListItemRepo = (deliveryListItemRepo != null) ? deliveryListItemRepo : new DeliveryListItemRepo(db);
                return deliveryListItemRepo;
            }
        }
        public DeliveryRepo DeliveryRepo
        {
            get
            {
                deliveryRepo = (deliveryRepo != null) ? deliveryRepo : new DeliveryRepo(db);
                return deliveryRepo;
            }
        }
        public DeliveryItemRepo DeliveryItemRepo
        {
            get
            {
                deliveryItemRepo = (deliveryItemRepo != null) ? deliveryItemRepo : new DeliveryItemRepo(db);
                return deliveryItemRepo;
            }
        }
        public MovementRepo MovementRepo
        {
            get
            {
                movementRepo = (movementRepo != null) ? movementRepo : new MovementRepo(db);
                return movementRepo;
            }
        }
        public WhDocumentWZRepo WhDocumentWZRepo
        {
            get
            {
                whDocumentWZRepo = (whDocumentWZRepo != null) ? whDocumentWZRepo : new WhDocumentWZRepo(db);
                return whDocumentWZRepo;
            }
        }
        public WhDocumentCMRRepo WhDocumentCMRRepo
        {
            get
            {
                whDocumentCMRRepo = (whDocumentCMRRepo != null) ? whDocumentCMRRepo : new WhDocumentCMRRepo(db);
                return whDocumentCMRRepo;
            }
        }
        public WhDocumentItemRepo WhDocumentItemRepo
        {
            get
            {
                whDocumentItemRepo = (whDocumentItemRepo != null) ? whDocumentItemRepo : new WhDocumentItemRepo(db);
                return whDocumentItemRepo;
            }
        }
        
        //public WarehouseItemRepo WarehouseItemRepo
        //{
        //    get
        //    {
        //        warehouseItemRepo = (warehouseItemRepo != null) ? warehouseItemRepo : new WarehouseItemRepo(db);
        //        return warehouseItemRepo;
        //    }
        //}
    }
}
