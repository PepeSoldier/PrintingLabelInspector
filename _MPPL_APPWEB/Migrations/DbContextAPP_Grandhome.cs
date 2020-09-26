using MDL_BASE.Models.Base;
using MDL_BASE.Models.MasterData;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWHDOC.Entities;
using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_ONEPROD.ComponentMes.Etities;
using MDL_ONEPROD.ComponentMes.Etities.MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.ComponentWMS._Interfaces;
using MDL_ONEPROD.ComponetMes.Entities;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.Scheduling;
using MDL_PRD.Model;
using System.Data.Entity;

namespace _MPPL_WEB_START.Migrations
{
    public class DbContextAPP_Grandhome : DbContextAPP_, 
        IDbContextOneprod, 
        IDbContextOneprodMes, 
        IDbContextOneprodAPS, 
        IDbContextiLOGIS
    {
        //enable-migrations -ContextTypeName  _MPPL_WEB_START.Migrations.DbContextAPP_Grandhome -MigrationsDirectory:Migrations.Grandhome
        //Add-Migration -ConfigurationTypeName _MPPL_WEB_START.Migrations.Grandhome.Configuration 1K
        //Update-database -ConfigurationTypeName _MPPL_WEB_START.Migrations.Grandhome.Configuration

        public DbContextAPP_Grandhome() : base("GrandhomeConnection")
        {
            this.Configuration.LazyLoadingEnabled = true;
        }
        public static new DbContextAPP_Grandhome Create()
        {
            return new DbContextAPP_Grandhome();
        }


        //---------------MODULE-ONEPROD---------------------------------------------------
        //--------------------------------------------------------------------------------
        //public DbSet<MDL_ONEPROD.Model.Scheduling.ResourceGroup> Areas2 { get; set; }
        public DbSet<MDL_ONEPROD.Model.Scheduling.ItemOP> ItemsOP { get; set; }
        public DbSet<MDL_ONEPROD.Model.Scheduling.ResourceOP> ResourcesOP { get; set; }
        //public DbSet<BufforLog> BufforLog { get; set; }
        public DbSet<MDL_ONEPROD.Model.Scheduling.Calendar2> Calendar2 { get; set; }
        public DbSet<ChangeOver> ChangeOvers { get; set; }
        public DbSet<MCycleTime> CycleTimes { get; set; }
        public DbSet<Param> Params { get; set; }
        //public DbSet<Process> Processes { get; set; }
        public DbSet<ItemGroupTool> ItemGroupTools { get; set; }
        public DbSet<ItemInventory> ItemInventories { get; set; }
        public DbSet<ClientOrder> ClientOrders { get; set; }
        public DbSet<Workorder> Workorders { get; set; }
        public DbSet<Tool> Tools { get; set; }
        public DbSet<ToolChangeOver> ToolChangeOvers { get; set; }
        public DbSet<ToolGroup> ToolGroups { get; set; }
        public DbSet<ToolMachine> ToolMachines { get; set; }
        
        public DbSet<Workplace> Workplaces { get; set; }
        public DbSet<WorkplaceBuffer> WorkplaceBuffers { get; set; }
        //public DbSet<LabourBrigade> LabourBrigades { get; set; }
        public DbSet<ProductionLog> ProductionLogs { get; set; }
        public DbSet<ProductionLogTraceability> ProductionLogTraceabilities { get; set; }
        public DbSet<ProductionLogRawMaterial> ProductionLogRawMaterials { get; set; }

        //----------------MODULE-iLOGIS--------------------------------------------------
        public DbSet<Package> Packages { get; set; }
        public DbSet<PackageItem> PackageItems { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WarehouseItem> WarehouseItems { get; set; }
        public DbSet<ItemWMS> ItemWMS { get; set; }
        public DbSet<StockUnit> StockUnits { get; set; }
        public DbSet<WarehouseLocation> WarehouseLocations { get; set; }
        public DbSet<WarehouseLocationType> WarehouseLocationTypes { get; set; }
        public DbSet<WarehouseLocationSort> WarehouseLocationSorts { get; set; }
        public DbSet<WorkstationItem> WorkstationItems { get; set; }
        public DbSet<AutomaticRule> AutomaticRules { get; set; }
        public DbSet<DeliveryList> DeliveryLists { get; set; }
        public DbSet<DeliveryListItem> DeliveryListItems { get; set; }
        public DbSet<PickingListItem> PickingListItems { get; set; }
        public DbSet<PickingList> PickingLists { get; set; }
        public DbSet<Transporter> Transporters { get; set; }
        public DbSet<TransporterLog> TransporterLogs { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<DeliveryItem> DeliveryItems { get; set; }
        public DbSet<Movement> Movements { get; set; }
        public DbSet<WhDocumentWZ> WhDocumentWZs { get; set; }
        public DbSet<WhDocumentCMR> WhDocumentCMRs { get; set; }
        public DbSet<WhDocumentItem> WhDocumentItems { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Ignore<Attachment>();
            modelBuilder.Ignore<ChangeLog>();
            //modelBuilder.Ignore<AncType>();
            //modelBuilder.Ignore<PncType>();
            //modelBuilder.Ignore<Resource2>();
            //modelBuilder.Ignore<LabourBrigade>();
            //modelBuilder.Ignore<Client>();

            //modelBuilder.Ignore<BomWorkorder>();
            //modelBuilder.Ignore<Bom>();       
            modelBuilder.Ignore<OrderModel>();
            modelBuilder.Ignore<OrderArchiveModel>();
            modelBuilder.Ignore<ReasonModel>();
            modelBuilder.Ignore<WhDocumentWZ>();
            modelBuilder.Ignore<WhDocumentCMR>();
            modelBuilder.Ignore<WhDocumentItem>();

            OnModelCreating_ONEPROD(modelBuilder);
            OnModelCreating_iLOGIS_CORE(modelBuilder);
            OnModelCreating_iLOGIS_WMS(modelBuilder);
        }

    }
}


