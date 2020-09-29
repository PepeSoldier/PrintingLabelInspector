using MDL_BASE.ComponentBase.Entities;
using MDL_BASE.Interfaces;
using MDL_BASE.Models.Base;
using MDL_BASE.Models.IDENTITY;
using MDL_BASE.Models.MasterData;
using MDL_CORE.ComponentCore.Entities;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_ONEPROD.ComponentMes.Etities;
using MDL_ONEPROD.Model.Scheduling;
using MDLX_CORE.ComponentCore.Entities;
using MDLX_MASTERDATA.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using Process = MDLX_MASTERDATA.Entities.Process;

namespace _MPPL_WEB_START.Migrations
{
    public class DbContextAPP_ : IdentityDbContext<User, ApplicationRole, string, UserLogin, UserRole, UserClaim>, IDbContextCore
    {
        public DbContextAPP_(string nameOrConnectionString) : base(nameOrConnectionString) //, throwIfV1Schema: false)
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        public DbContextAPP_(DbConnection connection) : base(connection, false)
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        public DbContextAPP_() : base("DefaultConnection") //, throwIfV1Schema: false)
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        public virtual DbContextTransaction BeginTransaction()
        {
            return this.Database.BeginTransaction();
        }
        public void SetEntryState_Added(IModelEntity entity)
        {
            Entry(entity).State = EntityState.Added;
        }
        public void SetEntryState_Modified(IModelEntity entity)
        {
            Entry(entity).State = EntityState.Modified;
        }
        public void SetEntryState_Detached(IModelEntity entity)
        {
            Entry(entity).State = EntityState.Detached;
        }
        public void SetEntryState_Unchanged(IModelEntity entity)
        {
            Entry(entity).State = EntityState.Unchanged;
        }
        public void SetEntryState_Deleted(IModelEntity entity)
        {
            Entry(entity).State = EntityState.Deleted;
        }

        //IDENTITY
        public override IDbSet<User> Users { get; set; }
        public override IDbSet<ApplicationRole> Roles { get; set; }
        public virtual IDbSet<UserLogin> UserLogins { get; set; }
        public virtual IDbSet<UserClaim> UserClaims { get; set; }
        public virtual IDbSet<UserRole> UserRoles { get; set; }

        //MASTERDATA
        public virtual DbSet<ProductionOrder> ProductionOrders { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<ItemUoM> ItemUoMs { get; set; }
        public virtual DbSet<Resource2> Resources2 { get; set; }
        public virtual DbSet<Process> Processes { get; set; }
        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<NotificationDevice> NotificationDevices { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Workstation> Workstations { get; set; }
        public virtual DbSet<Contractor> Contractors { get; set; }
        public virtual DbSet<LabourBrigade> LabourBrigades { get; set; }
        public virtual DbSet<MDL_CORE.ComponentCore.Entities.PackingLabel> PackingLabels { get; set; }
        public virtual DbSet<PackingLabelTest> PackingLabelTests { get; set; }

        //CORE
        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<ChangeLog> ChangeLogs { get; set; }
        public virtual DbSet<Printer> Printers { get; set; }
        public virtual DbSet<BomWorkorder> BomWorkorders { get; set; }
        public virtual DbSet<Bom> Boms { get; set; }
        public virtual DbSet<SystemVariable> SystemVariables { get; set; }

        private object lockObj = new object();
        public object LockObj { get { return lockObj; } }

        public static DbContextAPP_ Create()
        {
            return new DbContextAPP_();
        }

        public override int SaveChanges()
        {
            lock (LockObj)
            {
                try
                {
                    return base.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("Y?{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);

                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("_MPPL");
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            //modelBuilder.Entity<IdentityUserLogin>().HasKey(x => x.UserId);
            //modelBuilder.Entity<IdentityUserClaim>().HasKey(x => x.UserId);

            modelBuilder.Entity<User>().ToTable("IDENTITY_User");
            modelBuilder.Entity<ApplicationRole>().ToTable("IDENTITY_Role");
            modelBuilder.Entity<UserRole>().ToTable("IDENTITY_UserRole");
            //modelBuilder.Entity<IdentityUserRole>().ToTable("IDENTITY_UserRole");
            modelBuilder.Entity<UserLogin>().ToTable("IDENTITY_UserLogin");
            //modelBuilder.Entity<IdentityUserLogin>().ToTable("IDENTITY_UserLogin");
            modelBuilder.Entity<UserClaim>().ToTable("IDENTITY_UserClaim");
            //modelBuilder.Entity<IdentityUserClaim>().ToTable("IDENTITY_UserClaim");

        }
        public void OnModelCreating_iLOGIS_CORE(DbModelBuilder modelBuilder)
        {
            Debug.WriteLine("DbContextAPP_Dev.OnModelCreating_iLOGIS_CORE");
            modelBuilder.Entity<WarehouseLocation>().Property(e => e.Utilization).HasPrecision(14, 12); //00.000000000000
            modelBuilder.Entity<Warehouse>().HasOptional(u => u.AccountingWarehouse).WithMany().HasForeignKey(t => t.AccountingWarehouseId).WillCascadeOnDelete(false);
            modelBuilder.Entity<Warehouse>().HasOptional(u => u.ParentWarehouse).WithMany().HasForeignKey(t => t.ParentWarehouseId).WillCascadeOnDelete(false);
            modelBuilder.Entity<ItemWMS>().Property(e => e.Weight).HasPrecision(18, 5);
            modelBuilder.Entity<StockUnit>().Property(e => e.CurrentQtyinPackage).HasPrecision(18, 5);
            modelBuilder.Entity<StockUnit>().Property(e => e.MaxQtyPerPackage).HasPrecision(18, 5);
            modelBuilder.Entity<StockUnit>().Property(e => e.WMSQtyinPackage).HasPrecision(18, 5);
            modelBuilder.Entity<StockUnit>().Property(e => e.ReservedQty).HasPrecision(18, 5);
            modelBuilder.Entity<PackageItem>().Property(e => e.QtyPerPackage).HasPrecision(18, 5);
            
            modelBuilder.Entity<DeliveryListItem>().Property(e => e.QtyRequested).HasPrecision(18, 5);
            modelBuilder.Entity<DeliveryListItem>().Property(e => e.QtyDelivered).HasPrecision(18, 5);
            modelBuilder.Entity<DeliveryListItem>().Property(e => e.QtyUsed).HasPrecision(18, 5);
            modelBuilder.Entity<DeliveryListItem>().Property(e => e.QtyPerPackage).HasPrecision(18, 5);
        }
        public void OnModelCreating_iLOGIS_WMS(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movement>().Property(e => e.QtyMoved).HasPrecision(18, 5);
            modelBuilder.Entity<DeliveryItem>().Property(e => e.TotalQty).HasPrecision(18, 5);
            modelBuilder.Entity<DeliveryItem>().Property(e => e.QtyInPackage).HasPrecision(18, 5);
        }
        public void OnModelCreating_ONEPROD(DbModelBuilder modelBuilder)
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

            modelBuilder.Entity<ItemOP>().HasOptional(u => u.Process).WithMany().HasForeignKey(t => t.ProcessId);//.WillCascadeOnDelete(true);
            //modelBuilder.Entity<ItemOP>().HasOptional(u => u.ItemGroup).WithMany().Map(x => { x.ToTable("ONEPROD.CORE_Item"); });//.HasForeignKey(t => t.ItemGroupId);//.WillCascadeOnDelete(true);
            //modelBuilder.Entity<ItemOP>().HasOptional(u => u.ResourceGroup).WithMany().HasForeignKey(t => t.ResourceGroupId);//.WillCascadeOnDelete(true);
            modelBuilder.Entity<ItemInventory>().HasRequired(u => u.Item).WithMany().HasForeignKey(t => t.ItemId);//.WillCascadeOnDelete(true);

            modelBuilder.Entity<Calendar2>().HasRequired(u => u.Machine).WithMany().HasForeignKey(t => t.MachineId).WillCascadeOnDelete(true);
            //modelBuilder.Entity<MDL_ONEPROD.Model.Scheduling.ResourceOP>().HasOptional(u => u.ResourceGroupOP).WithMany().HasForeignKey(t => t.ResourceGroupId);
        }
    }
}