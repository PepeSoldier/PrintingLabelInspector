using MDL_AP.Interfaces;
using MDL_AP.Models.ActionPlan;
using MDL_AP.Models.DEF;
using MDL_BASE.Models.MasterData;
using MDLX_CORE.ComponentCore.Entities;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_ONEPROD.ComponentRTV._Interfaces;
using MDL_ONEPROD.ComponentRTV.Entities;
using MDL_ONEPROD.ComponentWMS._Interfaces;
using MDL_ONEPROD.ComponetMes.Entities;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Model.Scheduling;
using MDL_PFEP.Interface;
using MDL_PFEP.Model.ELDISY_PFEP;
using MDL_PFEP.Model.PFEP;
using MDL_PFEP.Models.DEF;
//using MDL_PFEP.Models.PFEP;
using MDL_PRD.Entity;
using MDL_PRD.Interface;
using MDL_PRD.Model;
using System.Collections.Generic;
using System.Data.Entity;
using MDL_OTHER.ComponentPickByLight._Interfaces;
using MDL_OTHER.ComponentPickByLight.Entities;
using MDL_ONEPROD.ComponentENERGY;
using MDL_ONEPROD.ComponentENERGY.Entities;
using MDL_ONEPROD.ComponentMes.Etities;
using MDL_ONEPROD.ComponentMes.Etities.MDL_ONEPROD.Model.Scheduling;
using MDLX_MASTERDATA.Entities;
using MDL_iLOGIS.ComponentWHDOC.Entities;
using System.Data.Common;
using MDL_ONEPROD.ComponentQuality._Interfaces;
using MDL_ONEPROD.ComponentQuality.Entities;

namespace _MPPL_WEB_START.Migrations
{

    public class DbContextAPP_DevK : DbContextAPP_Dev
    {
        public DbContextAPP_DevK() : base("DevKConnection")
        {
            this.Configuration.LazyLoadingEnabled = true;
            //enable-migrations -ContextTypeName  _MPPL_WEB_START.Migrations.DbContextAPP_DevK -MigrationsDirectory:Migrations.DevK
            //Add-Migration -ConfigurationTypeName _MPPL_WEB_START.Migrations.DevK.Configuration 1K
            //Update-database -ConfigurationTypeName _MPPL_WEB_START.Migrations.DevK.Configuration
        }
    }

    public class DbContextAPP_DevP : DbContextAPP_Dev
    {
        public DbContextAPP_DevP() : base("DevPConnection")
        {
            this.Configuration.LazyLoadingEnabled = true;
            //enable-migrations -ContextTypeName  _MPPL_WEB_START.Migrations.DbContextAPP_DevP -MigrationsDirectory:Migrations.DevP
            //Add-Migration -ConfigurationTypeName _MPPL_WEB_START.Migrations.DevP.Configuration 1K
            //Update-database -ConfigurationTypeName _MPPL_WEB_START.Migrations.DevP.Configuration
        }
    }

    public class DbContextAPP_DevPE : DbContextAPP_Dev
    {
        public DbContextAPP_DevPE() : base("DevPEConnection")
        {
            this.Configuration.LazyLoadingEnabled = true;
            //enable-migrations -ContextTypeName  _MPPL_WEB_START.Migrations.DbContextAPP_DevPE -MigrationsDirectory:Migrations.DevPE
            //Add-Migration -ConfigurationTypeName _MPPL_WEB_START.Migrations.DevPE.Configuration 1K
            //Update-database -ConfigurationTypeName _MPPL_WEB_START.Migrations.DevPE.Configuration
        }
    }

    public class DbContextAPP_Dev : 
                    DbContextAPP_, 
                    IDbContextAP, 
                    IDbContextPRD, 
                    IDbContextPFEP, 
                    IDbContextPFEP_Eldisy, 
                    IDbContextOneprod,
                    IDbContextOneprodAPS,
                    IDbContextOneProdOEE,
                    IDbContextOneprodMes,
                    IDbContextOneProdRTV,
                    IDbContextiLOGIS,
                    IDbContextOneprodWMS,
                    IDbContextPickByLight, 
                    IDbContextOneProdENERGY,
                    IDbContextOneprodQuality
    {
        public DbContextAPP_Dev() : base("Dev")
        {
            this.Configuration.LazyLoadingEnabled = true;
            //enable-migrations -ContextTypeName  _MPPL_WEB_START.Migrations.DbContextAPP_Dev -MigrationsDirectory:Migrations.Dev
            //Add-Migration -ConfigurationTypeName _MPPL_WEB_START.Migrations.Dev.Configuration 1K
            //Update-database -ConfigurationTypeName _MPPL_WEB_START.Migrations.Dev.Configuration
        }

        public DbContextAPP_Dev(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        public DbContextAPP_Dev(DbConnection connection) : base(connection)
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        public static string GetConnectionName()
        {
            string HostName = System.Environment.MachineName;
            List<string> HostsDevK = new List<string>(){ "DESKTOP-VTS936D", "SAMKAM10", "IN0001" };
            List<string> HostsDevP = new List<string>(){ "C4AEDF-NB", "IN0003" };

            if (HostsDevK.Contains(HostName))
            {
                return "DevKConnection";
            }
            else if(HostsDevP.Contains(HostName))
            {
                return "DevPConnection";
            }
            else
            {
                return "DevPConnection";
            }
        }

        public static new DbContextAPP_Dev Create()
        {
            return new DbContextAPP_Dev(GetConnectionName());
        }


        //---------------MODULE-AP--------------------------------------------------------
        //--------------------------------------------------------------------------------
        public virtual DbSet<ActionModel> Actions { get; set; }
        public virtual DbSet<ActionActivity> ActionActivities { get; set; }
        public virtual DbSet<ActionObserver> ActionObservers { get; set; }
        public virtual DbSet<Meeting> Meetings { get; set; }
        public virtual DbSet<MDL_AP.Models.DEF.Type> Types { get; set; }
        public virtual DbSet<Category> Categories { get; set; }

        //---------------MODULE-PRD-------------------------------------------------------
        //--------------------------------------------------------------------------------
        public virtual DbSet<OrderArchiveModel> PA_OrderArch { get; set; }
        public virtual DbSet<OrderModel> PA_Order { get; set; }
        public virtual DbSet<ReasonModel> PA_Reason { get; set; }

        //public DbSet<ProductionOrder> ProductionOrders { get; set; }
        public virtual DbSet<Prodorder20> ProdOrder20 { get; set; }
        public virtual DbSet<ProdOrderStatus> ProdOrderStatuses { get; set; }
        public virtual DbSet<ProdOrderSequence> ProdOrderSequence { get; set; }
        public virtual DbSet<MDL_PRD.Entity.Calendar> Calendar { get; set; }

        //---------------MODULE-PFEP-COMMON------------------------------------------------
        //--------------------------------------------------------------------------------
        //public DbSet<Package> Packages { get; set; }
        //PFEP-ELECTROLUX------------------------------------------------------------------
        //public virtual DbSet<MontageType> MontageTypes { get; set; }
        //public virtual DbSet<FeederType> FeederTypes { get; set; }
        //public virtual DbSet<BufferType> BufferTypes { get; set; }
        //public DbSet<MDL_PFEP.Models.PFEP.PackageItem> PackageItems { get; set; }
        //public DbSet<MDL_PFEP.Models.PFEP.WorkstationItem> WorkstationItems { get; set; }
        //public DbSet<Results1> Results1 { get; set; }
        //public DbSet<Results2> Results2 { get; set; }
        public virtual DbSet<PrintHistory> PrintHistory { get; set; }
        public virtual DbSet<AncFixedLocation> AncFixedLocations { get; set; }
        //PFEP-ELDISY----------------------------------------------------------------------
        public virtual DbSet<PackingInstructionPackage> PackingInstructionPackages { get; set; }
        public virtual DbSet<PackingInstruction> PackingInstructions { get; set; }
        public virtual DbSet<PackingInstructionPhoto> PackingInstructionPhotos { get; set; }
        public virtual DbSet<Correction> Corrections { get; set; }
        public virtual DbSet<Calculation> Calculations { get; set; }


        //---------------MODULE-ONEPROD---------------------------------------------------
        //--------------------------------------------------------------------------------
        //public DbSet<MDL_ONEPROD.Model.Scheduling.ResourceGroup> Areas2 { get; set; }
        public virtual DbSet<ItemOP> ItemsOP { get; set; }
        public virtual DbSet<MDL_ONEPROD.Model.Scheduling.ResourceOP> ResourcesOP { get; set; }

        public virtual DbSet<Warehouse> Warehouses { get; set; }
        public virtual DbSet<WarehouseItem> WarehouseItems { get; set; }
        public virtual DbSet<BufforLog> BufforLog { get; set; }
        public virtual DbSet<MDL_ONEPROD.Model.Scheduling.Calendar2> Calendar2 { get; set; }
        public virtual DbSet<ChangeOver> ChangeOvers { get; set; }
        public virtual DbSet<MCycleTime> CycleTimes { get; set; }
        public virtual DbSet<Param> Params { get; set; }
        //public DbSet<Item> Items { get; set; }
        //public DbSet<MDLX_MASTERDATA.Entities.Process> Processes { get; set; }
        public virtual DbSet<ItemGroupTool> ItemGroupTools { get; set; }
        public virtual DbSet<ItemInventory> ItemInventories { get; set; }
        public virtual DbSet<ClientOrder> ClientOrders { get; set; }
        public virtual DbSet<Workorder> Workorders { get; set; }
        public virtual DbSet<Tool> Tools { get; set; }
        public virtual DbSet<ToolChangeOver> ToolChangeOvers { get; set; }
        public virtual DbSet<ToolGroup> ToolGroups { get; set; }
        public virtual DbSet<ToolMachine> ToolMachines { get; set; }
        //---------------MODULE-ONEPROD-OEE----------------------------------------------
        public virtual DbSet<OEEReport> OEEReports { get; set; }
        public virtual DbSet<OEEReportEmployee> OEEReportEmployees { get; set; }
        public virtual DbSet<OEEReportProductionData> OEEReportProductionData { get; set; }
        public virtual DbSet<OEEReportProductionDataDetails> OEEReportProductionDataDetails { get; set; }
        public virtual DbSet<Reason> Reasons { get; set; }
        public virtual DbSet<ReasonType> ReasonTypes { get; set; }
        public virtual DbSet<MachineReason> MachineReasons { get; set; }
        public virtual DbSet<MachineTarget> MachineTargets { get; set; }
        //public DbSet<LabourBrigade> LabourBrigades { get; set; }
        //---------------MODULE-ONEPROD-RTV-OEE------------------------------------------
        public virtual DbSet<RTVOEEProductionData> RTVOEEProductionData { get; set; }
        public virtual DbSet<RTVOEEProductionDataDetails> RTVOEEProductionDataDetails { get; set; }
        public virtual DbSet<RTVOEEProductionDataParameter> RTVOEEProductionDataParameters { get; set; }
        public virtual DbSet<RTVOEEParameter> RTVOEEParameters { get; set; }

        //---------------MODULE-ONEPROD-EXECUTION----------------------------------------
        public virtual DbSet<Workplace> Workplaces { get; set; }
        public virtual DbSet<WorkplaceBuffer> WorkplaceBuffers { get; set; }
        public virtual DbSet<ProductionLog> ProductionLogs { get; set; }
        public virtual DbSet<ProductionLogTraceability> ProductionLogTraceabilities { get; set; }
        public virtual DbSet<ProductionLogRawMaterial> ProductionLogRawMaterials { get; set; }
        public virtual DbSet<RTVOEEPLCData> RTVOEEPLCData { get; set; }

        //---------------MODULE-ONEPROD-ENERGY----------------------------------------
        public virtual DbSet<EnergyMeter> EnergyMeters { get; set; }
        public virtual DbSet<EnergyCost> EnergyCosts { get; set; }
        public virtual DbSet<EnergyConsumptionData> EnergyConsumptionDatas { get; set; }

        //---------------MODULE-ONEPROD-QUALITY----------------------------------------
        public virtual DbSet<ItemParameter> ItemParameters { get; set; }
        public virtual DbSet<ItemMeasurement> ItemMeasurements { get; set; }

        //----------------MODULE-iLOGIS--------------------------------------------------
        public virtual DbSet<Package> Packages { get; set; }
        public virtual DbSet<MDL_iLOGIS.ComponentConfig.Entities.PackageItem> PackageItems { get; set; }
        //public DbSet<Warehouse> Warehouses { get; set; }
        //public DbSet<WarehouseItem> WarehouseItems { get; set; }
        public virtual DbSet<ItemWMS> ItemWMS { get; set; }
        public virtual DbSet<StockUnit> StockUnits { get; set; }
        public virtual DbSet<WarehouseLocation> WarehouseLocations { get; set; }
        public virtual DbSet<WarehouseLocationType> WarehouseLocationTypes { get; set; }
        public virtual DbSet<WarehouseLocationSort> WarehouseLocationSorts { get; set; }
        public virtual DbSet<MDL_iLOGIS.ComponentConfig.Entities.WorkstationItem> WorkstationItems { get; set; }
        public virtual DbSet<AutomaticRule> AutomaticRules { get; set; }
        public virtual DbSet<DeliveryList> DeliveryLists { get; set; }
        public virtual DbSet<DeliveryListItem> DeliveryListItems { get; set; }
        public virtual DbSet<PickingListItem> PickingListItems { get; set; }
        public virtual DbSet<PickingList> PickingLists { get; set; }
        public virtual DbSet<Transporter> Transporters { get; set; }
        public virtual DbSet<TransporterLog> TransporterLogs { get; set; }
        public virtual DbSet<Delivery> Deliveries { get; set; }
        public virtual DbSet<DeliveryItem> DeliveryItems { get; set; }
        public virtual DbSet<Movement> Movements { get; set; }

        public virtual DbSet<WhDocumentWZ> WhDocumentWZs { get; set; }
        public virtual DbSet<WhDocumentCMR> WhDocumentCMRs { get; set; }
        public virtual DbSet<WhDocumentItem> WhDocumentItems { get; set; }

        //---------------------MODULE-OTHER------------------------------------------------
        public virtual DbSet<PickByLightInstance> PickByLightInstances { get; set; }
        public virtual DbSet<PickByLightInstanceElement> PickByLightInstanceElements { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Ignore<Client>();
            modelBuilder.Entity<PackingInstruction>().Property(x => x.CalculationPrice).HasPrecision(18, 2);
            modelBuilder.Entity<PrintHistory>().HasRequired(r => r.Order20).WithMany().WillCascadeOnDelete(true);
            //modelBuilder.Entity<AbstractDEFType>().ToTable("DEF_Types", schemaName: "PFEP"); //[types] montage, feeder, buffer 
            
            //modelBuilder.Entity<RTVOEEProductionData>().Ignore(x => x.Detail);
            //modelBuilder.Entity<RTVOEEProductionData>().Ignore(x => x.DetailId);
            modelBuilder.Entity<RTVOEEProductionData>().Ignore(x => x.Detail);
            modelBuilder.Entity<RTVOEEProductionData>().Ignore(x => x.DetailId);

            OnModelCreating_ONEPROD(modelBuilder);
            OnModelCreating_iLOGIS_CORE(modelBuilder);
            OnModelCreating_iLOGIS_WMS(modelBuilder);
        }

        protected new void OnModelCreating_ONEPROD(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Workorder>().HasOptional(u => u.ClientOrder).WithMany().HasForeignKey(t => t.ClientOrderId).WillCascadeOnDelete(true);
            modelBuilder.Entity<Workorder>().HasOptional(u => u.Item).WithMany().HasForeignKey(t => t.ItemId);//.WillCascadeOnDelete(true);
            modelBuilder.Entity<Workorder>().HasOptional(u => u.Resource).WithMany().HasForeignKey(t => t.ResourceId);//.WillCascadeOnDelete(true);
            modelBuilder.Entity<Workorder>().HasOptional(u => u.Tool).WithMany().HasForeignKey(t => t.ToolId);//.WillCascadeOnDelete(true);

            modelBuilder.Entity<ToolMachine>().HasRequired(u => u.Machine).WithMany().HasForeignKey(t => t.MachineId).WillCascadeOnDelete(true);
            modelBuilder.Entity<ToolMachine>().HasRequired(u => u.Tool).WithMany().HasForeignKey(t => t.ToolId).WillCascadeOnDelete(true);

            modelBuilder.Entity<ToolChangeOver>().HasRequired(u => u.Tool1).WithMany().HasForeignKey(t => t.Tool1Id).WillCascadeOnDelete(true);
            modelBuilder.Entity<ToolChangeOver>().HasOptional(u => u.Tool2).WithMany().HasForeignKey(t => t.Tool2Id).WillCascadeOnDelete(false);

            modelBuilder.Entity<MCycleTime>().HasRequired(u => u.Machine).WithMany().HasForeignKey(t => t.MachineId).WillCascadeOnDelete(true);
            modelBuilder.Entity<MCycleTime>().HasRequired(u => u.ItemGroup).WithMany().HasForeignKey(t => t.ItemGroupId).WillCascadeOnDelete(true);

            modelBuilder.Entity<WarehouseItem>().HasRequired(u => u.Warehouse).WithMany().HasForeignKey(t => t.WarehouseId).WillCascadeOnDelete(true);
            modelBuilder.Entity<WarehouseItem>().HasRequired(u => u.ItemGroup).WithMany().HasForeignKey(t => t.ItemGroupId).WillCascadeOnDelete(true);

            //modelBuilder.Entity<ItemOP>().HasOptional(u => u.Process).WithMany().HasForeignKey(t => t.ProcessId);//.WillCascadeOnDelete(true);
            //modelBuilder.Entity<ItemOP>().HasOptional(u => u.ResourceGroupOP).WithMany().HasForeignKey(t => t.ResourceGroupId);//.WillCascadeOnDelete(true);
            modelBuilder.Entity<ItemInventory>().HasRequired(u => u.Item).WithMany().HasForeignKey(t => t.ItemId);//.WillCascadeOnDelete(true);

            modelBuilder.Entity<Calendar2>().HasRequired(u => u.Machine).WithMany().HasForeignKey(t => t.MachineId).WillCascadeOnDelete(true);
            //modelBuilder.Entity<ResourceOP>().HasOptional(u => u.ResourceGroupOP).WithMany().HasForeignKey(t => t.ResourceGroupId);
            //modelBuilder.Entity<MDL_ONEPROD.Model.Scheduling.ResourceOP>().Property(u => u.TargetOee).HasPrecision(18,2);
            //modelBuilder.Entity<MDL_ONEPROD.Model.Scheduling.ResourceOP>().Property(u => u.TargetAvailability).HasPrecision(18,2);
            //modelBuilder.Entity<MDL_ONEPROD.Model.Scheduling.ResourceOP>().Property(u => u.TargetPerformance).HasPrecision(18,2);
            //modelBuilder.Entity<MDL_ONEPROD.Model.Scheduling.ResourceOP>().Property(u => u.TargetQuality).HasPrecision(18,2);            
        }
    }   
}


