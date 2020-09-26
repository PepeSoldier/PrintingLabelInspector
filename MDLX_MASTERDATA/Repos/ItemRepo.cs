using MDLX_MASTERDATA._Interfaces;
using MDLX_MASTERDATA.Entities;
using MDLX_MASTERDATA.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Interface;
using XLIB_COMMON.Repo;

namespace MDLX_MASTERDATA.Repos
{
    public class ItemRepo : ItemAbstractRepo<Item>
    {
        public ItemRepo(IDbContextMasterData db) : base(db)
        {
        }
    }

    public abstract class ItemAbstractRepo<TEntity> : RepoGenericAbstract<TEntity> where TEntity : Item
    {
        protected new IDbContextMasterData db;

        public ItemAbstractRepo(IDbContextMasterData db) : base(db)
        {
            this.db = db;
        }

        public IQueryable<TEntity> GetListOnlyActive()
        {
            return dbSet.Where(p => p.ItemGroupId != null && !p.Deleted && p.Type != ItemTypeEnum.ItemGroup);
        }
        public IQueryable<TEntity> GetListByGroup(int itemGoupId)
        {
            return dbSet.AsNoTracking().Where(p => p.ItemGroupId == itemGoupId);
        }

        public virtual IQueryable<TEntity> GetList(TEntity item)
        {
            return dbSet.Where(p =>
                            (p.Deleted != true) &&
                            (p.Type != ItemTypeEnum.ItemGroup) &&
                            ((p.ItemGroup.ResourceGroupId == item.ResourceGroupId || item.ResourceGroupId == 0) || (p.ItemGroup == null && item.ResourceGroupId == -1)) &&
                            (p.OriginalName.StartsWith(item.Name) || item.OriginalName == null) &&
                            (p.Code.StartsWith(item.Code) || item.Code == null) &&
                            (item.Type == 0 || p.Type == item.Type) &&
                            (p.ItemGroupId == item.ItemGroupId || item.ItemGroupId == null || item.ItemGroupId == -1))
                        .OrderBy(p => p.ItemGroup.ResourceGroup.Name)//.OrderBy(p => p.ItemGroup.ResourceGroup.StageNo)
                        .ThenBy(p => p.OriginalName);
        }
        public Item GetByCode(string itemCode)
        {
            return dbSet.FirstOrDefault(x => x.Code == itemCode);
        }

        public int GetCounter(int itemGroupId)
        {
            return dbSet.AsNoTracking().Where(p => p.ItemGroupId == itemGroupId).Count();
        }
        public int SetDeleted(int id)
        {
            TEntity item = dbSet.FirstOrDefault(p => p.Id == id);
            if (item != null)
            {
                item.Deleted = true;
                return AddOrUpdate(item);
            }
            return 0;
        }
        public void Detach(Item item)
        {
            db.Entry(item).State = System.Data.Entity.EntityState.Detached;
        }
    }
}