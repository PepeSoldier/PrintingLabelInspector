using MDLX_CORE.Interfaces;
using System.Collections.Generic;
using System.Linq;
using XLIB_COMMON.Interface;

namespace XLIB_COMMON.Repo
{
    public class RepoCommon
    {
        private IDbContext db;
        protected IAlertManager alertManager;

        public RepoCommon(IDbContext db, IAlertManager alertManager = null)
        {
            this.db = db;
            this.alertManager = (alertManager != null) ? alertManager : null;
        }
                
        //virtual methods
        public virtual int Add(IModelEntity entity)
        {
            db.Entry(entity).State = System.Data.Entity.EntityState.Added;
            db.SaveChanges();
            return entity.Id;
        }
        public virtual int Update(IModelEntity entity)
        {
            if (entity.Id > 0)
            {
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return entity.Id;
            }
            
            return 0;   
        }
        public virtual int AddOrUpdate(IModelEntity entity)
        {
            if (entity.Id > 0)
            {
                return Update(entity);
            }
            else
            {
                return Add(entity);
            }
        }
        public virtual bool Delete(IModelEntity entity)
        {
            db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
            try
            {
                db.SaveChanges();
                return true;
            }
            catch
            {
                alertManager.AddAlert(AlertMessageType.danger, "Nie udało się usunąć obiektu", "fakeUser");
                return false;
            }
        }
        public virtual void MakeDeleted(IDefModel entity)
        {
            if (entity.Id > 0)
            {
                entity.Deleted = true;
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}
