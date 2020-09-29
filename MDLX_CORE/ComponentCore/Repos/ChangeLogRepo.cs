using XLIB_COMMON.Repo;
using System.Linq;
using MDL_BASE.Interfaces;
using System.IO;
using System.Web.Hosting;
using MDL_BASE.Models.Base;
using System.Collections.Generic;
using System;
using XLIB_COMMON.Model;

namespace XLIB_COMMON.Repo.Base
{
    public class ChangeLogRepo : RepoGenericAbstract<ChangeLog>
    {
        protected new IDbContextCore db;
        
        public ChangeLogRepo(IDbContextCore db) : base(db)
        {
            this.db = db;
        }

        //public override ChangeLog GetById(int id)
        //{
        //    return db.ChangeLogs.FirstOrDefault(d => d.Id == id);
        //}
        //public override IQueryable<ChangeLog> GetList()
        //{
        //    return db.ChangeLogs.OrderByDescending(x => x.Id);
        //}
        //public IQueryable<ChangeLog> GetListByDescription(string description)
        //{
        //    return db.ChangeLogs.Where(x =>
        //        description == null || (x.ParentObjectDescription.Contains(description) || x.ObjectDescription.Contains(description))
        //    );
        //} 
        //public IQueryable<ChangeLog> GetList(int id, string description, int parentId, string parentDescription, string[] tablesToConsider)
        //{
        //    int arrayLenght = tablesToConsider.Length;
        //    return db.ChangeLogs.Where(x =>
        //        (id == 0 || x.ObjectId == id) &&
        //        (parentId == 0 || x.ParentObjectId == parentId) &&
        //        (description == null || x.ObjectDescription.Contains(description)) &&
        //        (parentDescription == null || x.ParentObjectDescription.Contains(parentDescription)) &&
        //        (arrayLenght > 0 && tablesToConsider.Contains(x.ObjectName))
        //    );
        //} 
        //public List<ChangeLog> GetListByParentIdAndName(int parentId, string objectName)
        //{
        //    return db.ChangeLogs.Where(x => x.ParentObjectId == parentId && x.ObjectName == objectName).ToList();
        //}
        //public IQueryable<ChangeLog> GetListByObjectIdAndName(int objectId, string objectName)
        //{
        //    return db.ChangeLogs.Where(x => x.ObjectId == objectId && x.ObjectName == objectName);

        //}
        //public void AddChangeLogs(List<ObjectDataChange> oDataChanges, string userId)
        //{
        //    foreach(ObjectDataChange odc in oDataChanges)
        //    {
        //        AddChangeLog(odc, userId);
        //    }
        //}
        //public void AddChangeLog(ObjectDataChange oDataChange, string userId)
        //{
        //    string oldValue = oDataChange.oldValue != null ? oDataChange.oldValue.ToString() : string.Empty;
        //    string newValue = oDataChange.newValue != null ? oDataChange.newValue.ToString() : string.Empty;

        //    if (oldValue.Length < 255 && newValue.Length < 255)
        //    {
        //        ChangeLog chl = new ChangeLog
        //        {
        //            ObjectId = oDataChange.objectId,
        //            ObjectDescription = oDataChange.objectDescription,
        //            ParentObjectId = oDataChange.parentObjectId,
        //            ParentObjectDescription = oDataChange.parentObjectDescription,
        //            ObjectName = oDataChange.objClassName,
        //            FieldName = oDataChange.fieldName,
        //            FieldDisplayName = oDataChange.fieldDisplayName,
        //            OldValue = oldValue,
        //            NewValue = newValue,
        //            Date = DateTime.Now,
        //            UserId = userId
        //        };
        //        Add(chl);
        //    }
        //}
    }
}