using XLIB_COMMON.Repo;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Interface;
using MDL_iLOGIS.ComponentConfig.Entities;

namespace MDL_iLOGIS.ComponentConfig.Repos
{
    public class WarehouseItemRepo : RepoGenericAbstract<WarehouseItem>
    {
        protected new IDbContextiLOGIS db;
        //private UnitOfWorkOneprod unitOfWork;

        public WarehouseItemRepo(IDbContextiLOGIS db, IAlertManager alertManager = null)
            : base(db)
        {
            this.db = db;
        }

        public override WarehouseItem GetById(int id)
        {
            return db.WarehouseItems.FirstOrDefault(x => x.Id == id);
        }
        public override IQueryable<WarehouseItem> GetList()
        {
            return db.WarehouseItems.OrderBy(x => x.Id);
        }

        public IQueryable<WarehouseItem> GetList(int boxId)
        {
            return db.WarehouseItems.Where(x => x.WarehouseId == boxId && x.Deleted == false).OrderBy(x => x.Id);
        }

        public IQueryable<WarehouseItem> GetListAsNoTracking(int? areaId = null)
        {
            return db.WarehouseItems.AsNoTracking().Where(x=> areaId == null || x.ItemGroup.Item.ResourceGroupId == areaId).OrderBy(x => x.Id);
        }
        public IQueryable<WarehouseItem> GetListByItemGroup(int itemWMSGroupId = 0, int itemGroupId = 0)
        {
            return db.WarehouseItems.AsNoTracking().Where(x => 
                (itemWMSGroupId > 0 || itemGroupId > 0) &&
                (itemGroupId <= 0 || x.ItemGroup.ItemId == itemGroupId) &&
                (itemWMSGroupId <= 0 || x.ItemGroupId == itemWMSGroupId)
                ).OrderBy(x => x.Id);
        }

       
    }
}