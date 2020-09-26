using MDL_BASE.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using XLIB_COMMON.Interface;
using System.Data.Entity.Migrations;

namespace XLIB_COMMON.Repo
{
    public abstract class RepoGenericAbstract<TEntity> where TEntity : class, IModelEntity
    {
        protected IDbContext db;
        protected IAlertManager alertManager;
        protected DbSet<TEntity> dbSet;
        private object locker = new object();

        public DbSet DbSet { get { return dbSet; } }

        public RepoGenericAbstract(IDbContext db, IAlertManager alertManager = null)
        {
            this.db = db;
            this.dbSet = db.Set<TEntity>();
            this.alertManager = alertManager;
        }
 
        //abstract methods
        public virtual TEntity GetById(int id)
        {
            if (id > 0)
                return dbSet.FirstOrDefault(x => x.Id == id);
            else
                return null;
        }
        public virtual TEntity GetByIdAsNoTracking(int id)
        {
            return dbSet.AsNoTracking().FirstOrDefault(x => x.Id == id);
        }
        public virtual IQueryable<TEntity> GetList()
        {
            return dbSet.OrderBy(x=>x.Id);
        }
        //virtual methods

        //public virtual TEntity Add_(TEntity entity)
        //{
        //    return dbSet.Add(entity);
        //}
        //public virtual TEntity Attach_(TEntity entity)
        //{
        //    return dbSet.Attach(entity);
        //}
        //public virtual TEntity Update_(TEntity entity)
        //{
        //    return dbSet.Add(entity);
        //}
        //public virtual TEntity Remove_(TEntity entity)
        //{
        //    return dbSet.Remove(entity);
        //}

        public virtual int Add(IModelEntity entity)
        {
            lock (locker)
            {
                try
                {
                    db.SetEntryState_Added(entity);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    db.SetEntryState_Detached(entity);
                    throw e;
                }
                return entity.Id;
            }
        }
        //public virtual bool AddRange(IEnumerable<TEntity> entities)
        //{
        //    lock (locker)
        //    {
        //        try
        //        {
        //            db.Configuration.AutoDetectChangesEnabled = false;
        //            if (dbSet != null)
        //            {
        //                dbSet.AddRange(entities);
        //            }
        //            db.ChangeTracker.DetectChanges();
        //            db.SaveChanges();
        //            db.Configuration.AutoDetectChangesEnabled = true;
        //        }
        //        catch (Exception e)
        //        {
        //            //dbSet.RemoveRange(entities);
        //            db.Configuration.AutoDetectChangesEnabled = true;
        //            throw e;
        //        }
        //        return true;
        //    }
        //}
        public virtual bool AddOrUpdateRange(IEnumerable<TEntity> entities)
        {
            lock (locker)
            {
                try
                {
                    db.Configuration.AutoDetectChangesEnabled = false;

                    foreach (TEntity e in entities)
                    {
                        if (e.Id > 0)
                        {
                            db.SetEntryState_Modified(e);
                        }
                        else
                        {
                            db.SetEntryState_Added(e);
                        }
                    }

                    db.ChangeTracker.DetectChanges();
                    db.SaveChanges();
                    db.Configuration.AutoDetectChangesEnabled = true;
                }
                catch (Exception e)
                {
                    //dbSet.RemoveRange(entities);
                    db.Configuration.AutoDetectChangesEnabled = true;
                    throw e;
                }
                return true;
            }
        }
        public virtual int Update(IModelEntity entity)
        {
            lock (locker)
            {
                if (entity.Id > 0)
                {
                    try
                    {
                        db.SetEntryState_Modified(entity);
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        db.SetEntryState_Detached(entity);
                        throw e;
                    }
                    return entity.Id;
                }
                return 0;
            }
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
        //public virtual int AddOrUpdateUnique(IModelEntity entity)
        //{

        //    //dbSet.AddOrUpdate(entity);
        //    //DbSetMigrationsExtensions.AddOrUpdate<IModelEntity>(dbSet, entity);
        //}
        public virtual void Delete(IModelEntity entity)
        {
            lock (locker)
            {
                db.SetEntryState_Deleted(entity);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    alertManager.AddAlert(AlertMessageType.danger, "Nie udało się usunąć obiektu", "fakeUser");
                    Console.WriteLine(e.Message + ". " + e.InnerException.Message);
                }
            }
        }
        
        public virtual void Save()
        {
            lock (locker)
            {
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    alertManager.AddAlert(AlertMessageType.danger, "Zapis do bazy nie powiódł się", "fakeUser");
                    Console.WriteLine(e.Message + ". " + e.InnerException.Message);
                }
            }
        }

        public virtual void BulkUpdate(IModelEntity entity, IEnumerable<int> ids)
        {
            string tableName = GetTableName();
            var fieldValues = GetChanges(entity);
            ExecuteUpdate(fieldValues, ids, tableName);
        }
        public virtual void BulkUpdate(IModelEntity entity, IEnumerable<int> ids, string tableName)
        {
            var fieldValues = GetChanges(entity);
            ExecuteUpdate(fieldValues, ids, tableName);
        }
        private void ExecuteUpdate(Dictionary<string, string> FieldsValues, IEnumerable<int> ids, string tableName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE " + tableName + " SET ");

            string connector = string.Empty;
            foreach (var pair in FieldsValues)
            {
                sb.Append(connector + pair.Key + " = '" + pair.Value + "' ");
                connector = ",";
            }

            sb.Append(" WHERE Id IN(" + string.Join(",", ids) + ")");

            string query = sb.ToString();

            if (FieldsValues.Count > 0)
            {
                db.Database.ExecuteSqlCommand(query);
            }
        }

        public void SetChanges(object item, Dictionary<string, string> changes)
        {
            PropertyInfo[] piArr = item.GetType().GetProperties();
            PropertyInfo p;

            foreach (var dictEntry in changes)
            {
                p = piArr.FirstOrDefault(x => x.Name == dictEntry.Key);

                Type type = p.PropertyType;
                TypeCode typeCode = Type.GetTypeCode(type);

                if ( typeCode == TypeCode.Int32)
                {
                    p.SetValue(item, Convert.ToInt32(dictEntry.Value));
                }
                else if(typeCode == TypeCode.Decimal)
                {
                    p.SetValue(item, Convert.ToDecimal(dictEntry.Value));
                }
                else if(typeCode == TypeCode.DateTime)
                {
                    p.SetValue(item, Convert.ToDateTime(dictEntry.Value));
                }
                else
                {
                    p.SetValue(item, dictEntry.Value);
                }
            }

            //return new object();
        }

        public Dictionary<string, string> GetChanges(object item)
        {
            Dictionary<string, string> changes = new Dictionary<string, string>();
            PropertyInfo[] piArr = item.GetType().GetProperties();

            for (int i = 0; i < piArr.Length; i++)
            {
                string fieldName = piArr[i].Name;
                Type type = piArr[i].PropertyType;
                TypeCode typeCode = Type.GetTypeCode(type);
                object value = piArr.FirstOrDefault(x => x.Name == fieldName).GetValue(item);

                if ((Nullable.GetUnderlyingType(type) != null && value != null) ||                  //dla typu nullable wartosc zostala zmieniona gdy jest rozna od null
                   (type.IsEnum && Convert.ToInt32(value) > 0) ||                                  //dla enum'ów została zmieniona gdy jest wieksza od zera
                   //(typeCode == TypeCode.Boolean) ||                                                //boolean updatuje zawsze !!!!
                   (typeCode == TypeCode.String && value != null) ||                                //dla typu string wartosc zostala zmieniona gdy jest rozna od null
                   (typeCode == TypeCode.Int32 && Convert.ToInt32(value) != 0) ||                   //dla int musi byc rozna od zera by zostala uznana za zmienioną
                   (typeCode == TypeCode.Decimal && Convert.ToInt32(value) != 0) ||                 //dla decima musi byc rozna od zera by zostala uznana za zmienioną
                   (typeCode == TypeCode.DateTime && Convert.ToDateTime(value) != new DateTime())   //dla datetime musi byc rozna od daty domyślej by zostala uznana za zmienioną
                 )
                {
                    if (type.IsEnum)
                    {
                        changes.Add(fieldName, Convert.ToInt32(value).ToString());
                    }
                    else
                    {
                        changes.Add(fieldName, value.ToString());
                    }
                }
            }

            return changes;
        }

        public string GetTableName()
        {
            IQueryable<IModelEntity> query = dbSet.Where(x => x.Id == 1);
            string sql = query.ToString();
            Regex regex = new Regex(@"FROM\s+(?<table>.+)\s+AS");
            Match match = regex.Match(sql);
            string table = match.Groups["table"].Value;
            return table;
        }

        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return db.Database.ExecuteSqlCommand(sql, parameters);
        }
    }

}
