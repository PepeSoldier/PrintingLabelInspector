using MDL_BASE.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace XLIB_COMMON.Interface
{
    public abstract class UnitOfWorkAbstract
    {
        IDbContext db;

        public UnitOfWorkAbstract(IDbContext dbContext)
        {
            this.db = dbContext;
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }
        public DbContextTransaction BeginTransaction()
        {
            return db.Database.BeginTransaction();
        }

        public void DisableProxyCreation()
        {
            db.Configuration.ProxyCreationEnabled = false;
        }
    }
}