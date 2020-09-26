using MDL_AP.Interfaces;
using MDL_AP.Models.ActionPlan;
using MDL_AP.Models.DEF;
using MDL_BASE.ComponentBase.Entities;
using MDL_BASE.Models.MasterData;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWHDOC.Entities;
using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_ONEPROD.ComponentMes.Etities;
using MDL_ONEPROD.ComponentMes.Etities.MDL_ONEPROD.Model.Scheduling;
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
using MDLX_CORE.ComponentCore.Entities;
using MDLX_MASTERDATA.Entities;
using System;
using System.Data.Entity;

namespace _MPPL_WEB_START.Migrations
{
    public class DbContextAPP_ElectroluxPLS : 
                    DbContextAPP_, 
                    IDbContextiLOGIS
    {

        public DbContextAPP_ElectroluxPLS() : base("ElectroluxPLSConnection")
        {
            this.Configuration.LazyLoadingEnabled = true;
        }
        public DbContextAPP_ElectroluxPLS(string connectionName) : base(connectionName)
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        public static string GetConnectionName()
        {
            string clientName = _MPPL_WEB_START.Properties.Settings.Default.Client;
            return clientName + "Connection";
        }
        public static new DbContextAPP_ElectroluxPLS Create()
        {
            return new DbContextAPP_ElectroluxPLS(GetConnectionName());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Ignore<Workstation>();
            modelBuilder.Ignore<Warehouse>();
            modelBuilder.Ignore<WarehouseItem>();
            modelBuilder.Ignore<StockUnit>();
            modelBuilder.Ignore<WarehouseLocation>();
            modelBuilder.Ignore<WarehouseLocationType>();
            modelBuilder.Ignore<WarehouseLocationSort>();
            modelBuilder.Ignore<WorkstationItem>();
            modelBuilder.Ignore<AutomaticRule>();
            modelBuilder.Ignore<Delivery>();
            modelBuilder.Ignore<DeliveryItem>();
            modelBuilder.Ignore<DeliveryList>();
            modelBuilder.Ignore<DeliveryListItem>();
            modelBuilder.Ignore<PickingListItem>();
            modelBuilder.Ignore<PickingList>();
            modelBuilder.Ignore<Transporter>();
            modelBuilder.Ignore<TransporterLog>();
            modelBuilder.Ignore<Movement>();
            modelBuilder.Ignore<ProductionOrder>();
            modelBuilder.Ignore<LabourBrigade>();
            modelBuilder.Ignore<BomWorkorder>();
            modelBuilder.Ignore<Bom>();
            modelBuilder.Ignore<Resource2>();
            modelBuilder.Ignore<Process>();
            modelBuilder.Ignore<Area>();

            //OnModelCreating_iLOGIS_CORE(modelBuilder);
            //OnModelCreating_iLOGIS_WMS(modelBuilder);
        }

        //----------------MODULE-iLOGIS--------------------------------------------------
        public DbSet<Package> Packages { get; set; }
        public DbSet<PackageItem> PackageItems { get; set; }

        //Ignored
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WarehouseItem> WarehouseItems { get; set; }
        public DbSet<ItemWMS> ItemWMS { get; set; }
        public DbSet<StockUnit> StockUnits { get; set; }
        public DbSet<WarehouseLocation> WarehouseLocations { get; set; }
        public DbSet<WarehouseLocationType> WarehouseLocationTypes { get; set; }
        public DbSet<WarehouseLocationSort> WarehouseLocationSorts { get; set; }
        public DbSet<WorkstationItem> WorkstationItems { get; set; }
        public DbSet<AutomaticRule> AutomaticRules { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<DeliveryItem> DeliveryItems { get; set; }
        public DbSet<DeliveryList> DeliveryLists { get; set; }
        public DbSet<DeliveryListItem> DeliveryListItems { get; set; }
        public DbSet<PickingListItem> PickingListItems { get; set; }
        public DbSet<PickingList> PickingLists { get; set; }
        public DbSet<Transporter> Transporters { get; set; }
        public DbSet<TransporterLog> TransporterLogs { get; set; }
        public DbSet<Movement> Movements { get; set; }
        //Ignored*

        public DbSet<WhDocumentWZ> WhDocumentWZs { get; set; }
        public DbSet<WhDocumentCMR> WhDocumentCMRs { get; set; }
        public DbSet<WhDocumentItem> WhDocumentItems { get; set; }

    }
}


