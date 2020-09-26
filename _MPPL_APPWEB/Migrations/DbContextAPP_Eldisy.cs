using MDL_BASE.ComponentBase.Entities;


using MDL_BASE.Models.MasterData;
using MDLX_CORE.ComponentCore.Entities;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_ONEPROD.ComponentBase.Entities;
using MDL_ONEPROD.ComponetMes.Entities;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Model.Scheduling;
using MDL_PFEP.Interface;
using MDL_PFEP.Model.ELDISY_PFEP;
using MDL_PFEP.Model.PFEP;
using MDL_PFEP.Models.DEF;
//using MDL_PFEP.Models.PFEP;
using MDL_PRD.Interface;
using MDL_PRD.Model;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using MDL_ONEPROD.ComponentMes.Etities;
using MDL_ONEPROD.ComponentMes.Etities.MDL_ONEPROD.Model.Scheduling;

namespace _MPPL_WEB_START.Migrations
{
    public class DbContextAPP_Eldisy2 : 
                    DbContextAPP_, 
                    IDbContextPFEP_Eldisy,
                    IDbContextOneprod,
                    IDbContextOneProdOEE,
                    IDbContextOneprodMes
    {
        //enable-migrations -ContextTypeName  _MPPL_WEB_START.Migrations.DbContextAPP_Eldisy2 -MigrationsDirectory:Migrations.Eldisy2
        //Add-Migration -ConfigurationTypeName _MPPL_WEB_START.Migrations.Eldisy2.Configuration 1K
        //Update-database -ConfigurationTypeName _MPPL_WEB_START.Migrations.Eldisy2.Configuration

        public DbSet<Package> Packages { get; set; }
        //PFEP-ELDISY----------------------------------------------------------------------
        public DbSet<PackingInstructionPackage> PackingInstructionPackages { get; set; }
        public DbSet<PackingInstruction> PackingInstructions { get; set; }
        public DbSet<PackingInstructionPhoto> PackingInstructionPhotos { get; set; }
        public DbSet<Correction> Corrections { get; set; }
        public DbSet<Calculation> Calculations { get; set; }
        
        public DbContextAPP_Eldisy2() : base("Eldisy2Connection")
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Package>().Property(x => x.UnitPrice).HasPrecision(18, 2);
            modelBuilder.Entity<PackingInstruction>().Property(x => x.CalculationPrice).HasPrecision(18, 2);
            modelBuilder.Entity<Calculation>().Property(x => x.PackingInstructionPrice).HasPrecision(18, 2);
            modelBuilder.Entity<Calculation>().Property(x => x.CalculatedInstructionPrice).HasPrecision(18, 2);
            modelBuilder.Entity<Calculation>().Property(x => x.SetInstructionPrice).HasPrecision(18, 2);
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Ignore<MontageType>();
            //modelBuilder.Ignore<FeederType>();
            modelBuilder.Ignore<PackageItem>();
            modelBuilder.Ignore<WorkstationItem>();
            //modelBuilder.Ignore<Results1>();
            //modelBuilder.Ignore<Results2>();
            modelBuilder.Ignore<PrintHistory>();
            modelBuilder.Ignore<ProductionOrder>();
            modelBuilder.Ignore<Prodorder20>();
            //modelBuilder.Ignore<LabourBrigade>();
            modelBuilder.Ignore<Bom>();
            //modelBuilder.Ignore<AncType>();
            //modelBuilder.Ignore<PncType>();
            modelBuilder.Ignore<LabourBrigade>();
            modelBuilder.Ignore<Tool>();
            modelBuilder.Ignore<ToolGroup>();
            modelBuilder.Ignore<OEEReportProductionDataDetails>();
        }

        //---------------MODULE-ONEPROD---------------------------------------------------
        //--------------------------------------------------------------------------------
        //public DbSet<MDL_ONEPROD.Model.Scheduling.ResourceGroup> Areas2 { get; set; }
        //public DbSet<Warehouse> Buffor_Box { get; set; }
        //public DbSet<ItemGroup_Warehouse> Buffor_BoxItemGroup { get; set; }
        //public DbSet<BufforLog> BufforLog { get; set; }
        //public DbSet<MDL_ONEPROD.Model.Scheduling.Calendar2> Calendar2 { get; set; }
        //public DbSet<ChangeOver> ChangeOvers { get; set; }
        public DbSet<ResourceOP> ResourcesOP { get; set; }
        public DbSet<ItemOP> ItemsOP { get; set; }
        public DbSet<MCycleTime> CycleTimes { get; set; }
        public DbSet<Param> Params { get; set; }
        //public DbSet<Item> Items { get; set; }
        //public DbSet<MDLX_MASTERDATA.Entities.Process> Processes { get; set; }
        //public DbSet<ItemGroupTool> ItemGroupTools { get; set; }
        public DbSet<ItemInventory> ItemInventories { get; set; }
        public DbSet<ClientOrder> ClientOrders { get; set; }
        public DbSet<BomOneprod> BomOneprod { get; set; }
        
        public DbSet<Workorder> Workorders { get; set; }
        //public DbSet<Tool> Tools { get; set; }
        //public DbSet<ToolChangeOver> ToolChangeOvers { get; set; }
        //public DbSet<ToolGroup> ToolGroups { get; set; }
        //public DbSet<ToolMachine> ToolMachines { get; set; }
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
        //public DbSet<RTVOEEPLCData> RTVOEEPLCData { get; set; }
        //public DbSet<RTVOEEProductionData> RTVOEEProductionData { get; set; }
        //public DbSet<RTVOEEProductionDataDetails> RTVOEEProductionDataDetails { get; set; }
        //---------------MODULE-ONEPROD-MES----------------------------------------
        public DbSet<Workplace> Workplaces { get; set; }
        public DbSet<WorkplaceBuffer> WorkplaceBuffers { get; set; }
        public DbSet<ProductionLog> ProductionLogs { get; set; }
        public DbSet<ProductionLogTraceability> ProductionLogTraceabilities { get; set; }
        public DbSet<ProductionLogRawMaterial> ProductionLogRawMaterials { get; set; }

        //public DbSet<ItemWarehouseLocation> ItemWarehouseLocations { get; set; }
        //public DbSet<WarehouseLocation> WarehouseLocations { get; set; }
    }

    public class DbContextAPP_Eldisy : DbContextAPP_, IDbContextPFEP_Eldisy
    {
        //enable-migrations -ContextTypeName  _MPPL_WEB_START.Migrations.DbContextAPP_Eldisy -MigrationsDirectory:Migrations.Eldisy
        //Add-Migration -ConfigurationTypeName _MPPL_WEB_START.Migrations.Eldisy.Configuration 1K
        //Update-database -ConfigurationTypeName _MPPL_WEB_START.Migrations.Eldisy.Configuration

        //---------------MODULE-PFEP-COMMON------------------------------------------------
        //---------------------------------------------------------------------------------
        public DbSet<Package> Packages { get; set; }
        //PFEP-ELDISY----------------------------------------------------------------------
        public DbSet<PackingInstructionPackage> PackingInstructionPackages { get; set; }
        public DbSet<PackingInstruction> PackingInstructions { get; set; }
        public DbSet<PackingInstructionPhoto> PackingInstructionPhotos { get; set; }
        public DbSet<Correction> Corrections { get; set; }
        public DbSet<Calculation> Calculations { get; set; }


        public DbContextAPP_Eldisy() : base("EldisyConnection")
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Package>().Property(x => x.UnitPrice).HasPrecision(18, 2);
            modelBuilder.Entity<PackingInstruction>().Property(x => x.CalculationPrice).HasPrecision(18, 2);
            modelBuilder.Entity<Calculation>().Property(x => x.PackingInstructionPrice).HasPrecision(18, 2);
            modelBuilder.Entity<Calculation>().Property(x => x.CalculatedInstructionPrice).HasPrecision(18, 2);
            modelBuilder.Entity<Calculation>().Property(x => x.SetInstructionPrice).HasPrecision(18, 2);
            
            //modelBuilder.Ignore<MontageType>();
            //modelBuilder.Ignore<FeederType>();
            modelBuilder.Ignore<PackageItem>();
            modelBuilder.Ignore<WorkstationItem>();
            //modelBuilder.Ignore<Results1>();
            //modelBuilder.Ignore<Results2>();
            modelBuilder.Ignore<PrintHistory>();
            modelBuilder.Ignore<ProductionOrder>();
            modelBuilder.Ignore<Prodorder20>();
            //modelBuilder.Ignore<MDL_BASE.Models.MasterData.LabourBrigade>();
            modelBuilder.Ignore<Bom>();
            //modelBuilder.Ignore<AncType>();
            //modelBuilder.Ignore<PncType>();
            modelBuilder.Ignore<MDL_BASE.ComponentBase.Entities.LabourBrigade>();

        }
    }
}


