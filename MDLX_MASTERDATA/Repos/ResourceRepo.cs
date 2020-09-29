using XLIB_COMMON.Repo;
using MDLX_MASTERDATA._Interfaces;
using MDLX_MASTERDATA.Entities;
using MDLX_MASTERDATA.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Interface;

namespace MDLX_MASTERDATA.Repos
{
    public class ResourceRepo //: ResourceAbstractRepo<Resource2>
    {        
        public ResourceRepo (IDbContextMasterData db) //: base(db)
        {
        }

    //    public IQueryable<Resource2> GetList(Resource2 filter)
    //    {
    //        return db.Resources2.Where(x =>
    //                    (filter.AreaId == null || x.AreaId == filter.AreaId) &&
    //                    (filter.Name == null || x.Name.Contains(filter.Name)) &&
    //                    (filter.ResourceGroupId == null || filter.ResourceGroupId == 0 || x.ResourceGroupId == filter.ResourceGroupId) &&
    //                    (x.Deleted == false) )
    //                .OrderBy(x => x.AreaId)
    //                .ThenBy(x => x.ResourceGroupId)
    //                .ThenBy(x => x.Name);
    //    }
    //    public IQueryable<Resource2> GetByName(string name)
    //    {
    //        return db.Resources2.Where(x =>
    //                    (name == null || x.Name == name) &&
    //                    (x.Deleted == false))
    //                .OrderBy(x => x.AreaId)
    //                .ThenBy(x => x.ResourceGroupId)
    //                .ThenBy(x => x.Name);
    //    }
    //}

    //public class ResourceAbstractRepo<TEntity> : RepoGenericAbstract<TEntity> where TEntity : Resource2
    //{
    //    protected new IDbContextMasterData db;

    //    public ResourceAbstractRepo(IDbContextMasterData db) : base(db)
    //    {
    //        this.db = db;
    //    }

    //    public IQueryable<TEntity> GetResources()
    //    {
    //        return dbSet.Where(x => x.Deleted == false &&
    //                (x.Type == ResourceTypeEnum.Resource || x.Type == ResourceTypeEnum.VirtualResource))
    //            .OrderBy(m => m.Name);
    //    }
    //    public IQueryable<TEntity> GetSubResources()
    //    {
    //        return dbSet.Where(x => x.Type == ResourceTypeEnum.Subresource && x.Deleted == false).OrderBy(m => m.Name);
    //    }
    //    public IQueryable<TEntity> GetGroups()
    //    {
    //        return dbSet.Where(x => x.Type == ResourceTypeEnum.Group && x.Deleted == false).OrderBy(m => m.Name);
    //    }
    //    public IQueryable<TEntity> GetWorkplaces()
    //    {
    //        return dbSet.Where(x => x.Type == ResourceTypeEnum.Workplace && x.Deleted == false).OrderBy(m => m.Name);
    //    }
    //    public IQueryable<TEntity> GetListByGroup(int groupId)
    //    {
    //        return dbSet.AsNoTracking().Where(m =>
    //                m.Deleted == false &&
    //                m.Type == ResourceTypeEnum.Resource &&
    //                m.ResourceGroupId == groupId)
    //            .OrderBy(m => m.Name);
    //    }
    //    public IQueryable<TEntity> GetListByArea(int areaId)
    //    {
    //        return dbSet.AsNoTracking().Where(m =>
    //                m.Deleted == false &&
    //                m.Type == ResourceTypeEnum.Resource &&
    //                m.AreaId == areaId)
    //            .OrderBy(m => m.Name);
    //    }
    //    public IQueryable<TEntity> GetAutocomplete(string prefix)
    //    {
    //        return dbSet.Where(x =>
    //                x.Deleted == false &&
    //                x.Type == ResourceTypeEnum.Resource &&
    //                x.Name.StartsWith(prefix))
    //            .OrderByDescending(x => x.Name);
    //    }
    }
}