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
using System.Diagnostics;
using System;
using MDL_BASE.Models.IDENTITY;
using MDL_CORE.ComponentCore.Entities;
using MDL_BASE.ComponentBase.Entities;
using MDL_BASE.Models.Base;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Threading;
using MDL_BASE.Interfaces;
using System.Reflection;

namespace _MPPL_WEB_START.Migrations
{
    public class DatabaseMPPL
    {
        public DbContextTransaction BeginTransaction()
        {
            return new DbContextAPP_Dev().BeginTransaction();
        }
    }

    public class DbContextAPP_Dev_Test : DbContextAPP_Dev
    {
        public DbModelBuilder ModelBuilder { get; private set; }

        public DbContextAPP_Dev_Test() : base("Dev")
        {
            this.Configuration.LazyLoadingEnabled = true;
        }
        public DbContextAPP_Dev_Test(DbModelBuilder modelBuilder) : base("Dev")
        {
            this.Configuration.LazyLoadingEnabled = true;
            OnModelCreating(modelBuilder);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Debug.WriteLine("DbContextAPP_Dev.OnModelCreating");
            base.OnModelCreating(modelBuilder);
            
            //OnModelCreating_ONEPROD(modelBuilder);
            /*OnModelCreating_iLOGIS_CORE(modelBuilder)*/;
            //OnModelCreating_iLOGIS_WMS(modelBuilder);
        }
    }

    public class DbContextAPP_Dev_Test2:
                    DbContext,
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
                    IDbContextOneProdENERGY
    {
        public DbContextAPP_Dev_Test2()
        {
        }

        DbContext fakeDb = new DbContextFake();

        public Database Database { get { return fakeDb.Database; } }
        public DbChangeTracker ChangeTracker { get { return fakeDb.ChangeTracker; } }
        public DbContextConfiguration Configuration { get { return fakeDb.Configuration; } }


        public void Dispose() { }
        public DbEntityEntry Entry(object entity)
        {
            //return base.Entry(null);
            return fakeDb.Entry(new FakeEntity() { Id = 0 });
        }
        public DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
        {
            //object o = 1;
            //return (DbEntityEntry<TEntity>)o;
            return fakeDb.Entry<TEntity>(entity);
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            return true;
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return 0;
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public System.Type GetType()
        {
            System.Type t = typeof(DbContextAPP_Dev_Test2);
            return t;
        }
        public IEnumerable<DbEntityValidationResult> GetValidationErrors()
        {
            //return new List<DbEntityValidationResult>();
            return fakeDb.GetValidationErrors();
        }
        public virtual int SaveChanges()
        {
            //return 0;
            return fakeDb.SaveChanges();
        }
        public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return new Task<int>(() => { return 0; });
        }
        public virtual Task<int> SaveChangesAsync()
        {
            return new Task<int>(() => { return 0; });
        }
        public override DbSet Set(System.Type entityType)
        {
            //return fakeDb.Set(entityType);
            System.Type typeSrc = this.GetType();
            PropertyInfo[] srcProps = typeSrc.GetProperties();

            foreach (PropertyInfo srcProp in srcProps)
            {
                if(srcProp.PropertyType.IsGenericType && 
                   srcProp.PropertyType.GetGenericTypeDefinition() == typeof(DbSet) &&
                   srcProp.PropertyType.GetGenericArguments()[0] == entityType)
                {
                    //System.Type generic = typeof(DbSet<>);
                    //System.Type[] typeArgs = { entityType };
                    //System.Type constructed = generic.MakeGenericType(typeArgs);

                    return (DbSet)typeSrc.GetProperty(srcProp.Name).GetValue(this);
                }
            }

            return null;
        }
        public object Set2(System.Type entityType)
        {
            //return fakeDb.Set(entityType);
            System.Type typeSrc = this.GetType();
            PropertyInfo[] srcProps = typeSrc.GetProperties();

            foreach (PropertyInfo srcProp in srcProps)
            {
                if (srcProp.PropertyType.IsGenericType) 
                {
                    System.Type a = srcProp.PropertyType.GetGenericTypeDefinition();
                    System.Type b = srcProp.PropertyType.GetGenericArguments()[0];

                    if (a == typeof(DbSet<>) && b == entityType)
                    {
                        //System.Type generic = typeof(DbSet<>);
                        //System.Type[] typeArgs = { entityType };
                        //System.Type constructed = generic.MakeGenericType(typeArgs);

                        return typeSrc.GetProperty(srcProp.Name).GetValue(this);
                    }
                }
            }

            return null;
        }
        public virtual new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            //return fakeDb.Set<TEntity>();

            System.Type typeSrc = this.GetType();
            PropertyInfo[] srcProps = typeSrc.GetProperties();

            foreach (PropertyInfo srcProp in srcProps)
            {
                if (srcProp.PropertyType.IsGenericType &&
                   srcProp.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) &&
                   srcProp.PropertyType.GetGenericArguments()[0] == typeof(TEntity))
                {
                    return (DbSet<TEntity>)typeSrc.GetProperty(srcProp.Name).GetValue(this);
                }
            }

            return null;
        }
        protected virtual void Dispose(bool disposing) { }
        protected virtual void OnModelCreating(DbModelBuilder modelBuilder) { }
        protected virtual bool ShouldValidateEntity(DbEntityEntry entityEntry) { return false; }
        protected virtual DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            DbEntityValidationResult r = null;
            return r;
        }

        public DbContextTransaction BeginTransaction()
        {
            DbContextTransaction t = fakeDb.Database.BeginTransaction();
            return t;
        }

        public void SetEntryState_Added(IModelEntity entity)
        {
            //return;
            //Entry(entity).State = EntityState.Added;

            System.Type generic = typeof(DbSet<>);
            System.Type[] typeArgs = { entity.GetType() };
            System.Type constructed = generic.MakeGenericType(typeArgs);
            //System.Type t = entity.GetType();
                
            object dbSet = Set2(entity.GetType());

            if (dbSet != null)
            {
                //var dataType = new String.Type[] { entity.GetType };
                //var genericBase = typeof(DbSet<>);
                //var combinedType = genericBase.MakeGenericType(dataType);
                //var listStringInstance = Activator.CreateInstance(combinedType);

                //var addMethod = listStringInstance.GetType().GetMethod("Add");
                //addMethod.Invoke(dbSet, new object[] { entity });

                //dbSet.Add(entity);
            }
        }
        public void SetEntryState_Modified(IModelEntity entity)
        {
            return;
            //Entry(entity).State = EntityState.Modified;
        }
        public void SetEntryState_Detached(IModelEntity entity)
        {
            //return;
            //Entry(entity).State = EntityState.Deleted;
            System.Type t = entity.GetType();
            DbSet dbSet = Set(t);
            dbSet.Remove(entity);
        }
        public void SetEntryState_Deleted(IModelEntity entity)
        {
            //return;
            //Entry(entity).State = EntityState.Deleted;
            System.Type t = entity.GetType();
            DbSet dbSet = Set(t);
            dbSet.Remove(entity);
        }
        public void SetEntryState_Unchanged(IModelEntity entity)
        {
            return;
            //Entry(entity).State = EntityState.Unchanged;
        }

        public object LockObj { get { return 0; } }

        //IDENTITY
        public virtual IDbSet<User> Users { get; set; }
        public virtual IDbSet<ApplicationRole> Roles { get; set; }
        public virtual IDbSet<UserLogin> UserLogins { get; set; }
        public virtual IDbSet<UserClaim> UserClaims { get; set; }
        public virtual IDbSet<UserRole> UserRoles { get; set; }

        //MASTERDATA
        public virtual DbSet<ProductionOrder> ProductionOrders { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<ItemUoM> ItemUoMs { get; set; }
        public virtual DbSet<Resource2> Resources2 { get; set; }
        public virtual DbSet<MDLX_MASTERDATA.Entities.Process> Processes { get; set; }
        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<NotificationDevice> NotificationDevices { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Workstation> Workstations { get; set; }
        public virtual DbSet<Contractor> Contractors { get; set; }
        public virtual DbSet<LabourBrigade> LabourBrigades { get; set; }

        //CORE
        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<ChangeLog> ChangeLogs { get; set; }
        public virtual DbSet<Printer> Printers { get; set; }
        public virtual DbSet<BomWorkorder> BomWorkorders { get; set; }
        public virtual DbSet<Bom> Boms { get; set; }
        public virtual DbSet<SystemVariable> SystemVariables { get; set; }

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

        //public void TestModelCreation(DbModelBuilder modelBuilder)
        //{
        //    //this.OnModelCreating(modelBuilder);

        //    base.OnModelCreating(modelBuilder);
        //    //modelBuilder.Ignore<Client>();
        //    //modelBuilder.Ignore<MDLX_MASTERDATA.Entities.Item>();
        //    //modelBuilder.Ignore<MDLX_MASTERDATA.Entities.Resource2>();
        //    //modelBuilder.Ignore<WarehouseLocationItem>();
        //    //modelBuilder.Ignore<WarehouseLocation>();
        //    //modelBuilder.Ignore<Resource2>();
        //    //modelBuilder.Ignore<Process>();
        //    //modelBuilder.Ignore<Item>();
        //    //modelBuilder.Ignore<ChangeLog>();
        //    //modelBuilder.Ignore<BomWorkorder>();
        //    //modelBuilder.Ignore<Bom>();
        //    //modelBuilder.Ignore<Attachment>();
        //    //modelBuilder.Ignore<ProductionOrder>();
        //    //modelBuilder.Ignore<Area>();
        //    //modelBuilder.Ignore<Department>();
        //    //modelBuilder.Ignore<LabourBrigade>();
        //    //modelBuilder.Ignore<Contractor>();
        //    //modelBuilder.Ignore<Workstation>();
        //    modelBuilder.Ignore<OrderModel>();
        //    modelBuilder.Ignore<OrderArchiveModel>();
        //    modelBuilder.Ignore<ReasonModel>();

        //    modelBuilder.Ignore<WhDocumentWZ>();
        //    modelBuilder.Ignore<WhDocumentCMR>();
        //    modelBuilder.Ignore<WhDocumentItem>();

        //    OnModelCreating_iLOGIS_CORE(modelBuilder);
        //    OnModelCreating_iLOGIS_WMS(modelBuilder);

        //}
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    //modelBuilder.Ignore<Client>();
        //    //modelBuilder.Ignore<MDLX_MASTERDATA.Entities.Item>();
        //    //modelBuilder.Ignore<MDLX_MASTERDATA.Entities.Resource2>();
        //    //modelBuilder.Ignore<WarehouseLocationItem>();
        //    //modelBuilder.Ignore<WarehouseLocation>();
        //    //modelBuilder.Ignore<Resource2>();
        //    //modelBuilder.Ignore<Process>();
        //    //modelBuilder.Ignore<Item>();
        //    //modelBuilder.Ignore<ChangeLog>();
        //    //modelBuilder.Ignore<BomWorkorder>();
        //    //modelBuilder.Ignore<Bom>();
        //    //modelBuilder.Ignore<Attachment>();
        //    //modelBuilder.Ignore<ProductionOrder>();
        //    //modelBuilder.Ignore<Area>();
        //    //modelBuilder.Ignore<Department>();
        //    //modelBuilder.Ignore<LabourBrigade>();
        //    //modelBuilder.Ignore<Contractor>();
        //    //modelBuilder.Ignore<Workstation>();
        //    modelBuilder.Ignore<OrderModel>();
        //    modelBuilder.Ignore<OrderArchiveModel>();
        //    modelBuilder.Ignore<ReasonModel>();

        //    modelBuilder.Ignore<WhDocumentWZ>();
        //    modelBuilder.Ignore<WhDocumentCMR>();
        //    modelBuilder.Ignore<WhDocumentItem>();

        //    OnModelCreating_iLOGIS_CORE(modelBuilder);
        //    OnModelCreating_iLOGIS_WMS(modelBuilder);
        //}
    }

}


