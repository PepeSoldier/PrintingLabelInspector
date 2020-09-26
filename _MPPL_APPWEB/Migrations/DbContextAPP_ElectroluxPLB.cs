using MDL_BASE.ComponentBase.Entities;
using MDL_BASE.Models.Base;
using MDL_BASE.Models.MasterData;
using MDLX_CORE.ComponentCore.Entities;
using MDL_OTHER.ComponentHSE._Interfaces;
using MDL_OTHER.ComponentHSE.Entities;
using MDLX_MASTERDATA.Entities;
using System;
using System.Data.Entity;
using MDL_PRD.Interface;
using MDL_PFEP.Interface;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_PRD.Model;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_PRD.Entity;
using MDL_iLOGIS.ComponentWHDOC.Entities;

namespace _MPPL_WEB_START.Migrations
{
    public class DbContextAPP_ElectroluxPLB : 
                    DbContextAPP_, 
                    IDbContextOtherHSE,
                    IDbContextPRD,
                    IDbContextiLOGIS
    {
        //enable-migrations -ContextTypeName  _MPPL_WEB_START.Migrations.DbContextAPP_ElectroluxPLB -MigrationsDirectory:Migrations.ElectroluxPLB
        //Add-Migration -ConfigurationTypeName _MPPL_WEB_START.Migrations.ElectroluxPLB.Configuration 1K
        //Update-Database -ConfigurationTypeName _MPPL_WEB_START.Migrations.ElectroluxPLB.Configuration
        //.\migrate.exe _MPPL_WEB_START.dll _MPPL_WEB_START.Migrations.ElectroluxPLB.Configuration /startupConfigurationFile="..\\Web.config"

        public DbSet<SafetyCross> SafetyCrosses { get; set; }

        public DbContextAPP_ElectroluxPLB() : base("ElectroluxPLBConnection")
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        public static new DbContextAPP_ElectroluxPLB Create()
        {
            return new DbContextAPP_ElectroluxPLB();
        }

        public DbSet<OrderModel> PA_Order { get; set; }             //ignored
        public DbSet<OrderArchiveModel> PA_OrderArch { get; set; }  //ignored
        public DbSet<ReasonModel> PA_Reason { get; set; }           //ignored

        //public DbSet<ProductionOrder> ProductionOrders { get; set; }
        public DbSet<ProdOrderStatus> ProdOrderStatuses { get; set; }
        public DbSet<ProdOrderSequence> ProdOrderSequence { get; set; }
        public DbSet<Prodorder20> ProdOrder20 { get; set; }
        public DbSet<Calendar> Calendar { get; set; }
        
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
            //modelBuilder.Ignore<Client>();
            //modelBuilder.Ignore<MDLX_MASTERDATA.Entities.Item>();
            //modelBuilder.Ignore<MDLX_MASTERDATA.Entities.Resource2>();
            //modelBuilder.Ignore<WarehouseLocationItem>();
            //modelBuilder.Ignore<WarehouseLocation>();
            //modelBuilder.Ignore<Resource2>();
            //modelBuilder.Ignore<Process>();
            //modelBuilder.Ignore<Item>();
            //modelBuilder.Ignore<ChangeLog>();
            //modelBuilder.Ignore<BomWorkorder>();
            //modelBuilder.Ignore<Bom>();
            //modelBuilder.Ignore<Attachment>();
            //modelBuilder.Ignore<ProductionOrder>();
            //modelBuilder.Ignore<Area>();
            //modelBuilder.Ignore<Department>();
            //modelBuilder.Ignore<LabourBrigade>();
            //modelBuilder.Ignore<Contractor>();
            //modelBuilder.Ignore<Workstation>();
            modelBuilder.Ignore<OrderModel>();
            modelBuilder.Ignore<OrderArchiveModel>();
            modelBuilder.Ignore<ReasonModel>();

            modelBuilder.Ignore<WhDocumentWZ>();
            modelBuilder.Ignore<WhDocumentCMR>();
            modelBuilder.Ignore<WhDocumentItem>();

            OnModelCreating_iLOGIS_CORE(modelBuilder);
            OnModelCreating_iLOGIS_WMS(modelBuilder);
        }
    }
}


