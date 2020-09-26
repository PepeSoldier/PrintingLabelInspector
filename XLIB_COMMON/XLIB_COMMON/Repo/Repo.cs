using MDL_BASE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XLIB_COMMON.Repo
{
    public class Repo
    {
        protected IDbContext db;

        public Repo(IDbContext db)
        {
            this.db = db;
        }
        //public Repo(DbContextPreprod db)
        //{
        //    this.db = (db != null) ? db : new DbContextPreprod();
        //}

        //-------------------------------------------------------------------------------
        public int AddOrUpdateObject(IModelEntity myObject)
        {
            if (myObject.Id > 0)
            {
                db.Entry(myObject).State = System.Data.Entity.EntityState.Modified;
            }
            else
            {
                db.Entry(myObject).State = System.Data.Entity.EntityState.Added;
            }

            db.SaveChanges();
            return myObject.Id;
        }
        public int DeleteObjectPermanently(IModelEntity dr)
        {
            if (dr != null)
            {
                db.Entry(dr).State = System.Data.Entity.EntityState.Deleted;
                return db.SaveChanges();
            }
            return -2;
        }
        public int DeleteObject(IModelDeletableEntity dr)
        {
            dr.Deleted = true;
            db.Entry(dr).State = System.Data.Entity.EntityState.Modified;
            return db.SaveChanges();
        }


    }
}
