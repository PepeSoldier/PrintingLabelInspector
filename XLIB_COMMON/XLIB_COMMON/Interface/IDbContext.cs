using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Threading.Tasks;

namespace MDL_BASE.Interfaces
{
    public interface IDbContext : IDisposable
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbSet Set(Type entityType);

        DbEntityEntry Entry(object entity);
        Database Database { get; }

        IEnumerable<DbEntityValidationResult> GetValidationErrors();

        DbChangeTracker ChangeTracker { get; }
        DbContextConfiguration Configuration { get; }

        int SaveChanges();
        Task<int> SaveChangesAsync();

        object LockObj { get; }

        DbContextTransaction BeginTransaction();

        void SetEntryState_Added(IModelEntity entity);
        void SetEntryState_Modified(IModelEntity entity);
        void SetEntryState_Detached(IModelEntity entity);
        void SetEntryState_Deleted(IModelEntity entity);
        void SetEntryState_Unchanged(IModelEntity entity);

        //DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class; //, IModelEntity;
        //string ConnectionString { get; set; }
        //bool AutoDetectChangedEnabled { get; set; }
        //void ExecuteSqlCommand(string p, params object[] o);
        //void ExecuteSqlCommand(string p);
    }
}
