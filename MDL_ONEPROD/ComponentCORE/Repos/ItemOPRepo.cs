using XLIB_COMMON.Repo;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.Scheduling;
using MDLX_MASTERDATA.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Interface;
using MDLX_MASTERDATA.Repos;
using MDLX_MASTERDATA.Entities;

namespace MDL_ONEPROD.Repo.Scheduling
{
    public class ItemOPRepo : ItemAbstractRepo<ItemOP>
    {
        protected new IDbContextOneprod db;
        
        public ItemOPRepo(IDbContextOneprod db) : base(db)
        {
            this.db = db;
        }

        //public ItemOP GetByCode(string itemCode)
        //{
        //    return dbSet.FirstOrDefault(x => x.Code == itemCode);
        //}

        public override IQueryable<ItemOP> GetList(ItemOP item)
        {
            return dbSet.Where(p =>
                        (p.Deleted != true) &&
                        (p.Type != ItemTypeEnum.ItemGroup) &&
                        (item.ResourceGroupId == null || item.ResourceGroupId <= 0 || p.ItemGroupOP.ResourceGroupId == item.ResourceGroupId) &&
                        (item.Name == null || p.Name.StartsWith(item.Name)) &&
                        (item.Code == null || p.Code.StartsWith(item.Code)) &&
                        (item.Type == 0 || p.Type == item.Type) &&
                        (item.ItemGroupId == null || item.ItemGroupId == -1 || p.ItemGroupId == item.ItemGroupId))
                    .OrderBy(p => (p.ItemGroup != null && p.ItemGroup.ResourceGroup != null)? p.ItemGroupOP.ResourceGroupOP.StageNo : p.Id)
                    .ThenBy(p => p.Name);
        }

        public void ManualInsert(int id, int minBatch, bool woGenerator)
        {
            try
            {
                db.Database.ExecuteSqlCommand("INSERT INTO [ONEPROD].[CORE_Item] VALUES (" + string.Format("{0}, {1}, {2}", id, minBatch, woGenerator ? 1 : 0) + ")");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Attach(ItemOP item)
        {
            try
            {
                dbSet.Attach(item);
                //db.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }
    }
}