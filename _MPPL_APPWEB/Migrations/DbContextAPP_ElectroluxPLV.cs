using MDL_AP.Interfaces;
using MDL_AP.Models.ActionPlan;
using MDL_AP.Models.DEF;
using MDL_BASE.ComponentBase.Entities;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWHDOC.Entities;
using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_ONEPROD.ComponentMes.Etities;
using MDL_ONEPROD.ComponentMes.Etities.MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.ComponentQuality._Interfaces;
using MDL_ONEPROD.ComponentQuality.Entities;
using MDL_ONEPROD.ComponentRTV._Interfaces;
using MDL_ONEPROD.ComponentRTV.Entities;
using MDL_ONEPROD.ComponentWMS._Interfaces;
using MDL_ONEPROD.ComponetMes.Entities;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Model.Scheduling;
using MDL_OTHER.ComponentHSE._Interfaces;
using MDL_OTHER.ComponentPickByLight._Interfaces;
using MDL_OTHER.ComponentPickByLight.Entities;
using MDL_OTHER.ComponentVisualControl.Entities;
using MDL_PFEP.Interface;
using MDL_PFEP.Model.PFEP;
using MDL_PFEP.Models.DEF;
//using MDL_PFEP.Models.PFEP;
using MDL_PRD.Entity;
using MDL_PRD.Interface;
using MDL_PRD.Model;
using System;
using System.Data.Entity;

namespace _MPPL_WEB_START.Migrations
{
    public class DbContextAPP_ElectroluxPLV_Prod : DbContextAPP_ElectroluxPLV
    {
        //enable-migrations -ContextTypeName  _MPPL_WEB_START.Migrations.DbContextAPP_ElectroluxPLV -MigrationsDirectory:Migrations.ElectroluxPLV
        //Add-Migration -ConfigurationTypeName _MPPL_WEB_START.Migrations.ElectroluxPLV.Configuration 1K
        //Update-Database -ConfigurationTypeName _MPPL_WEB_START.Migrations.ElectroluxPLV.Configuration
        //Update-Database -ConfigurationTypeName _MPPL_WEB_START.Migrations.ElectroluxPLV.Configuration -TargetMigration:"138K"

        //UPDATE [EPSS_2_PROD].[_MPPL].[__MigrationHistory] SET [ContextKey] = '_MPPL_WEB_START.Migrations.ElectroluxPLV.Configuration'
        public DbContextAPP_ElectroluxPLV_Prod() : base("ElectroluxPLVConnection")
        {
            this.Configuration.LazyLoadingEnabled = true;
        }
    }
    public class DbContextAPP_ElectroluxPLV_Staging : DbContextAPP_ElectroluxPLV
    {
        //enable-migrations -ContextTypeName  _MPPL_WEB_START.Migrations.DbContextAPP_ElectroluxPLV_Staging -MigrationsDirectory:Migrations.ElectroluxPLV_Staging
        //Add-Migration -ConfigurationTypeName _MPPL_WEB_START.Migrations.ElectroluxPLV_Staging.Configuration 1K
        //Update-Database -ConfigurationTypeName _MPPL_WEB_START.Migrations.ElectroluxPLV_Staging.Configuration
        public DbContextAPP_ElectroluxPLV_Staging() : base("ElectroluxPLV_StagingConnection")
        {
            this.Configuration.LazyLoadingEnabled = true;
        }
    }

    public class DbContextAPP_ElectroluxPLV : 
                    DbContextAPP_, 
                    IDbContextAP, 
                    IDbContextPRD, 
                    IDbContextPFEP, 
                    IDbContextOneprod,
                    IDbContextOneprodAPS,
                    IDbContextOneProdOEE, 
                    IDbContextOneprodMes,
                    IDbContextOneProdRTV,
                    IDbContextOneprodWMS,
                    IDbContextOneprodQuality,
                    IDbContextiLOGIS,
                    IDbContextVisualControl,
                    IDbContextPickByLight
    {
        public DbContextAPP_ElectroluxPLV(string connectionName) : base(connectionName)
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        public static string GetConnectionName()
        {
            string clientName = _MPPL_WEB_START.Properties.Settings.Default.Client;
            return clientName + "Connection";
        }
        public static new DbContextAPP_ElectroluxPLV Create()
        {
            return new DbContextAPP_ElectroluxPLV(GetConnectionName());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PrintHistory>().HasRequired(r => r.Order20).WithMany().WillCascadeOnDelete(true);
            //modelBuilder.Entity<AbstractDEFType>().ToTable("DEF_Types", schemaName: "PFEP"); //[types] montage, feeder, buffer 

            modelBuilder.Entity<OEEReportProductionData>().Property(x => x.CycleTime).HasPrecision(18, 2);
            modelBuilder.Entity<OEEReportProductionData>().Property(x => x.UsedTime).HasPrecision(18, 2);
            modelBuilder.Entity<OEEReportProductionDataDetails>().Property(x => x.ProductionCycleTime).HasPrecision(18, 2);
            modelBuilder.Entity<RTVOEEProductionData>().Ignore(x => x.Detail);
            modelBuilder.Entity<RTVOEEProductionData>().Ignore(x => x.DetailId);

            modelBuilder.Ignore<WhDocumentWZ>();
            modelBuilder.Ignore<WhDocumentCMR>();
            modelBuilder.Ignore<WhDocumentItem>();

            OnModelCreating_iLOGIS_CORE(modelBuilder);
            //OnModelCreating_iLOGIS_WMS(modelBuilder);
            //modelBuilder.Entity<WarehouseLocation>().Property(e => e.Utilization).HasPrecision(14, 12); //00.000000000000
            //modelBuilder.Entity<Warehouse>().HasOptional(u => u.AccountingWarehouse).WithMany().HasForeignKey(t => t.AccountingWarehouseId).WillCascadeOnDelete(false);
            //modelBuilder.Entity<Warehouse>().HasOptional(u => u.ParentWarehouse).WithMany().HasForeignKey(t => t.ParentWarehouseId).WillCascadeOnDelete(false);
        }

        //---------------MODULE-AP-------------------------
        public DbSet<ActionModel> Actions { get; set; }
        public DbSet<ActionActivity> ActionActivities { get; set; }
        public DbSet<ActionObserver> ActionObservers { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<MDL_AP.Models.DEF.Type> Types { get; set; }
        public DbSet<Category> Categories { get; set; }

        //---------------MODULE-PRD------------------------
        public DbSet<OrderModel> PA_Order { get; set; }
        public DbSet<OrderArchiveModel> PA_OrderArch { get; set; }
        public DbSet<ReasonModel> PA_Reason { get; set; }

        //public DbSet<ProductionOrder> ProductionOrders { get; set; }
        public DbSet<ProdOrderStatus> ProdOrderStatuses { get; set; }
        public DbSet<ProdOrderSequence> ProdOrderSequence { get; set; }
        public DbSet<Prodorder20> ProdOrder20 { get; set; }
        public DbSet<Calendar> Calendar { get; set; }

        //---------------MODULE-PFEP-----------------------
        //public DbSet<MontageType> MontageTypes { get; set; }
        //public DbSet<FeederType> FeederTypes { get; set; }
        //public DbSet<BufferType> BufferTypes { get; set; }
        //public DbSet<Package> Packages { get; set; }
        //public DbSet<MDL_PFEP.Models.PFEP.PackageItem> PackageItems { get; set; }
        //public DbSet<MDL_PFEP.Models.PFEP.WorkstationItem> WorkstationItems { get; set; }
        //public DbSet<Results1> Results1 { get; set; }
        //public DbSet<Results2> Results2 { get; set; }
        public DbSet<PrintHistory> PrintHistory { get; set; }
        public DbSet<AncFixedLocation> AncFixedLocations { get; set; }

        //---------------MODULE-ONEPROD---------------------------------------------------
        //--------------------------------------------------------------------------------
        //public DbSet<MDL_ONEPROD.Model.Scheduling.ResourceGroup> Areas2 { get; set; }
        public DbSet<ItemOP> ItemsOP { get; set; }
        public DbSet<ResourceOP> ResourcesOP { get; set; }
        //public DbSet<MDLX_MASTERDATA.Entities.Process> Processes { get; set; }

        public DbSet<BufforLog> BufforLog { get; set; }
        public DbSet<Calendar2> Calendar2 { get; set; }
        public DbSet<ChangeOver> ChangeOvers { get; set; }
        public DbSet<MCycleTime> CycleTimes { get; set; }
        public DbSet<Param> Params { get; set; }
        public DbSet<ItemGroupTool> ItemGroupTools { get; set; }
        public DbSet<ItemInventory> ItemInventories { get; set; }
        public DbSet<ClientOrder> ClientOrders { get; set; }
        public DbSet<Workorder> Workorders { get; set; }
        public DbSet<Tool> Tools { get; set; }
        public DbSet<ToolChangeOver> ToolChangeOvers { get; set; }
        public DbSet<ToolGroup> ToolGroups { get; set; }
        public DbSet<ToolMachine> ToolMachines { get; set; }
        //---------------MODULE-ONEPROD-OEE----------------------------------------------
        public DbSet<OEEReport> OEEReports { get; set; }
        public DbSet<OEEReportEmployee> OEEReportEmployees { get; set; }
        public DbSet<OEEReportProductionData> OEEReportProductionData { get; set; }
        public DbSet<OEEReportProductionDataDetails> OEEReportProductionDataDetails { get; set; }
        public DbSet<Reason> Reasons { get; set; }
        public DbSet<ReasonType> ReasonTypes { get; set; }
        public DbSet<MachineReason> MachineReasons { get; set; }
        public DbSet<MachineTarget> MachineTargets { get; set; }
        //public DbSet<LabourBrigade> LabourBrigades { get; set; }
        //---------------MODULE-ONEPROD-RTV----------------------------------------------
        public DbSet<RTVOEEPLCData> RTVOEEPLCData { get; set; }
        public DbSet<RTVOEEProductionData> RTVOEEProductionData { get; set; }
        public DbSet<RTVOEEProductionDataDetails> RTVOEEProductionDataDetails { get; set; }
        public DbSet<RTVOEEProductionDataParameter> RTVOEEProductionDataParameters { get; set; }
        public DbSet<RTVOEEParameter> RTVOEEParameters { get; set; }
        //---------------MODULE-ONEPROD-MES----------------------------------------------
        public DbSet<Workplace> Workplaces { get; set; }
        public DbSet<WorkplaceBuffer> WorkplaceBuffers { get; set; }
        public DbSet<ProductionLog> ProductionLogs { get; set; }
        public DbSet<ProductionLogTraceability> ProductionLogTraceabilities { get; set; }
        public DbSet<ProductionLogRawMaterial> ProductionLogRawMaterials { get; set; }

        //---------------MODULE-ONEPROD-QUALITY------------------------------------------
        public DbSet<ItemParameter> ItemParameters { get; set; }
        public DbSet<ItemMeasurement> ItemMeasurements { get; set; }

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

        //-----------------OTHER----------------------------------------------------------
        public DbSet<JobItemConfig> JobItemConfigs { get; set; }
        public DbSet<PickByLightInstance> PickByLightInstances { get; set; }
        public DbSet<PickByLightInstanceElement> PickByLightInstanceElements { get; set; }
    }
}


