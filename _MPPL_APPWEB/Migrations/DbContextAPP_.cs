using MDL_BASE.Interfaces;
using MDL_BASE.Models.IDENTITY;
using MDL_CORE.ComponentCore.Entities;
using MDL_LABELINSP.Entities;
using MDL_LABELINSP.Interfaces;
using MDLX_CORE.ComponentCore.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace _LABELINSP_APPWEB.Migrations
{
    public class DbContextAPP_ : IdentityDbContext<User, ApplicationRole, string, UserLogin, UserRole, UserClaim>, IDbContextCore, IDbContextLabelInsp
    {
        private object lockObj = new object();
        public object LockObj { get { return lockObj; } }

        //IDENTITY
        public override IDbSet<User> Users { get; set; }

        public override IDbSet<ApplicationRole> Roles { get; set; }
        public virtual IDbSet<UserLogin> UserLogins { get; set; }
        public virtual IDbSet<UserClaim> UserClaims { get; set; }
        public virtual IDbSet<UserRole> UserRoles { get; set; }

        //MasterData
        public DbSet<MDL_LABELINSP.Entities.WorkorderLabel> WorkorderLabels { get; set; }

        public DbSet<WorkorderLabelInspection> WorkorderLabelInspections { get; set; }
        public DbSet<SystemVariable> SystemVariables { get; set; }
        public DbSet<Printer> Printers { get; set; }
        public DbSet<ItemData> ItemData { get; set; }
        public DbSet<Workorder> Workorders { get; set; }


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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("_LABELINSP");
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            //modelBuilder.Entity<IdentityUserLogin>().HasKey(x => x.UserId);
            //modelBuilder.Entity<IdentityUserClaim>().HasKey(x => x.UserId);

            modelBuilder.Entity<User>().ToTable("IDENTITY_User");
            modelBuilder.Entity<ApplicationRole>().ToTable("IDENTITY_Role");
            modelBuilder.Entity<UserRole>().ToTable("IDENTITY_UserRole");
            modelBuilder.Entity<UserLogin>().ToTable("IDENTITY_UserLogin");
            modelBuilder.Entity<UserClaim>().ToTable("IDENTITY_UserClaim");
        }

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
    }
}