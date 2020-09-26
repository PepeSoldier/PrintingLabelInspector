using MDL_BASE.Models.Base;
using MDL_BASE.Models.MasterData;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_ONEPROD.ComponentENERGY;
using MDL_ONEPROD.ComponentENERGY.Entities;
using MDL_ONEPROD.ComponentMes.Etities;
using MDL_ONEPROD.ComponentMes.Etities.MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.ComponentRTV._Interfaces;
using MDL_ONEPROD.ComponentRTV.Entities;
using MDL_ONEPROD.ComponetMes.Entities;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Model.Scheduling;
using MDL_OTHER.ComponentVisualControl.Entities;
using MDLX_CORE.ComponentCore.Entities;
using System;
using System.Data.Entity;

namespace _MPPL_WEB_START.Migrations
{

    public class DbContextAPP_WRP_Staging : DbContextAPP_WRP
    {
        //enable-migrations -ContextTypeName  _MPPL_WEB_START.Migrations.DbContextAPP_WRP_Staging -MigrationsDirectory:Migrations.WRP_Staging
        //Add-Migration -ConfigurationTypeName _MPPL_WEB_START.Migrations.WRP_Staging.Configuration 1K
        //Update-Database -ConfigurationTypeName _MPPL_WEB_START.Migrations.WRP_Staging.Configuration
        //.\migrate.exe _MPPL_WEB_START.dll _MPPL_WEB_START.Migrations.WRP.Configuration /startupConfigurationFile= "..\\web_k.config"

        public DbContextAPP_WRP_Staging() : base("WRP_StagingConnection")
        {
            this.Configuration.LazyLoadingEnabled = true;
        }
    }
    public class DbContextAPP_WRP_Prod : DbContextAPP_WRP
    {
        //enable-migrations -ContextTypeName  _MPPL_WEB_START.Migrations.DbContextAPP_WRP -MigrationsDirectory:Migrations.WRP
        //Add-Migration -ConfigurationTypeName _MPPL_WEB_START.Migrations.WRP.Configuration 1K
        //Update-Database -ConfigurationTypeName _MPPL_WEB_START.Migrations.WRP.Configuration

        //UPDATE [EPSS_2_PROD].[_MPPL].[__MigrationHistory] SET [ContextKey] = '_MPPL_WEB_START.Migrations.WRP.Configuration'
        public DbContextAPP_WRP_Prod() : base("WRPConnection")
        {
            this.Configuration.LazyLoadingEnabled = true;
        }
    }
    public class DbContextAPP_WRP : 
                    DbContextAPP_, 
                    IDbContextOneprod,
                    IDbContextOneProdOEE, 
                    IDbContextOneprodMes,
                    IDbContextOneProdRTV,
                    IDbContextOneProdENERGY
    {
        public DbContextAPP_WRP(string connectionName) : base(connectionName)
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        public static string GetConnectionName()
        {
            string clientName = Properties.Settings.Default.Client;
            return clientName + "Connection";
        }
        public static new DbContextAPP_WRP Create()
        {
            return new DbContextAPP_WRP(GetConnectionName());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Ignore<ProductionOrder>();
            //modelBuilder.Ignore<BomWorkorder>();
            //modelBuilder.Ignore<Bom>();
            modelBuilder.Ignore<Attachment>();
            //modelBuilder.Ignore<ProductionLogTraceability>();
            //modelBuilder.Ignore<WorkplaceBuffer>();
            modelBuilder.Entity<OEEReportProductionData>().Property(x => x.CycleTime).HasPrecision(18, 2);
            modelBuilder.Entity<OEEReportProductionData>().Property(x => x.UsedTime).HasPrecision(18, 2);
            modelBuilder.Entity<OEEReportProductionDataDetails>().Property(x => x.ProductionCycleTime).HasPrecision(18, 2);
            modelBuilder.Entity<RTVOEEProductionData>().Ignore(x => x.Detail);
            modelBuilder.Entity<RTVOEEProductionData>().Ignore(x => x.DetailId);
        }

        //---------------MODULE-ONEPROD---------------------------------------------------
        //--------------------------------------------------------------------------------
        //public DbSet<MDL_ONEPROD.Model.Scheduling.ResourceGroup> Areas2 { get; set; }
        public DbSet<ItemOP> ItemsOP { get; set; }
        public DbSet<ResourceOP> ResourcesOP { get; set; }
        //public DbSet<MDLX_MASTERDATA.Entities.Process> Processes { get; set; }

        
        //public DbSet<BufforLog> BufforLog { get; set; }
        public DbSet<MDL_ONEPROD.Model.Scheduling.Calendar2> Calendar2 { get; set; }
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

        //---------------MODULE-ONEPROD-ENERGY----------------------------------------
        public DbSet<EnergyMeter> EnergyMeters { get; set; }
        public DbSet<EnergyCost> EnergyCosts { get; set; }
        public DbSet<EnergyConsumptionData> EnergyConsumptionDatas { get; set; }

        //---------------MODULE-ONEPROD-RTV----------------------------------------------
        public DbSet<RTVOEEPLCData> RTVOEEPLCData { get; set; }
        public DbSet<RTVOEEProductionData> RTVOEEProductionData { get; set; }
        public DbSet<RTVOEEProductionDataDetails> RTVOEEProductionDataDetails { get; set; }
        public DbSet<RTVOEEProductionDataParameter> RTVOEEProductionDataParameters { get; set; }
        public DbSet<RTVOEEParameter> RTVOEEParameters { get; set; }
        //---------------MODULE-ONEPROD-MES----------------------------------------
        public DbSet<Workplace> Workplaces { get; set; }
        public DbSet<WorkplaceBuffer> WorkplaceBuffers { get; set; }
        public DbSet<ProductionLog> ProductionLogs { get; set; }
        public DbSet<ProductionLogTraceability> ProductionLogTraceabilities { get; set; }
        public DbSet<ProductionLogRawMaterial> ProductionLogRawMaterials { get; set; }
    }
}


