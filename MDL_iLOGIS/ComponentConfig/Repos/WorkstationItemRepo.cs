using MDL_BASE.Interfaces;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLIB_COMMON.Interface;
using XLIB_COMMON.Repo;
using System.Data.Entity;

namespace MDL_WMS.ComponentConfig.Repos
{
    public class WorkstationItemRepo : RepoGenericAbstract<WorkstationItem>
    {
        protected new IDbContextiLOGIS db;
        //private UnitOfWorkOneprod unitOfWork;

        public WorkstationItemRepo(IDbContextiLOGIS db) : base(db)
        {
            this.db = db;
        }
        
        public override WorkstationItem GetById(int id)
        {
            return db.WorkstationItems
                .Include(k => k.Workstation)
                .Include(i => i.ItemWMS)
                .FirstOrDefault(x => x.Id == id);
        }
        
        public WorkstationItem GetByWorkstationId(int id)
        {
            return db.WorkstationItems.Where(x => x.WorkstationId == id).FirstOrDefault();
        }
        public WorkstationItem GetByItemId(int id)
        {
            return db.WorkstationItems.Where(x => x.ItemWMSId == id).FirstOrDefault();
        }

        public IQueryable<WorkstationItem> GetList(WorkstationItem filter)
        {
            string itemName = filter.ItemWMS != null ? filter.ItemWMS.Item.Name : null;
            string itemCode = filter.ItemWMS != null ? filter.ItemWMS.Item.Code : null;
            string lineName = filter.Workstation != null ? filter.Workstation.Line.Name : null;
            string wrkstName = filter.Workstation != null ? filter.Workstation.Name : null;

            var query = db.WorkstationItems
                .Where(x =>
                    //(filter.WorkstationId <= 0 || x.WorkstationId == filter.WorkstationId) &&
                    (filter.MaxPackages <= 0 || x.MaxPackages == filter.MaxPackages) &&
                    (filter.SafetyStock <= 0 || x.SafetyStock == filter.SafetyStock) &&
                    (filter.MaxBomQty <= 0 || x.MaxBomQty == filter.MaxBomQty) &&
                    (itemName == null || x.ItemWMS.Item.Name.Contains(itemName)) &&
                    (itemCode == null || x.ItemWMS.Item.Code.Contains(itemCode)) &&
                    (lineName == null || x.Workstation.Line.Name.Contains(lineName)) &&
                    (wrkstName == null || x.Workstation.Name == wrkstName)
                //(filter.Item == null || filter.Item.Name == null || x.Item.Name.Contains(filter.Item.Name)) &&
                //(filter.Item == null || filter.Item.Code == null || x.Item.Code.Contains(filter.Item.Code))
                )
                .OrderByDescending(x => x.Id);
                //.OrderBy( x=> x.Workstation.Name)
                //.ThenBy( x=> x.Workstation.Line.Name);

            return query;
        }

        public int GetMaxPackegesByWorkstationId(int workstationId)
        {
            return db.WorkstationItems.Where(x => x.WorkstationId == workstationId).FirstOrDefault().MaxPackages;
        }
        public int GetSafetyStockByWorkstationId(int workstationId)
        {
            return db.WorkstationItems.Where(x => x.WorkstationId == workstationId).FirstOrDefault().SafetyStock;
        }
        public int GetMaxBomQtyByWorkstationId(int workstationId)
        {
            return db.WorkstationItems.Where(x => x.WorkstationId == workstationId).FirstOrDefault().MaxBomQty;
        }

        public List<int> GetItemIdsByWorkstationId(int workstationId)
        {
            return db.WorkstationItems
                .Where(x => x.WorkstationId == workstationId)
                .Select(w => w.ItemWMS.ItemId)
                .ToList();
        }
        public List<int> GetItemIdsByWorkstationIds(int[] workstationIds)
        {
            return db.WorkstationItems
                .Where(x => workstationIds.Contains(x.WorkstationId))
                .Select(w => w.ItemWMS.ItemId)
                .ToList();
        }

        public WorkstationItem GetByAncId(int itemId = 0, int itemWMSId = 0)
        {
            return db.WorkstationItems.Where(d => 
                    (itemId > 0 || itemWMSId > 0) &&
                    (itemId <= 0 || d.ItemWMS.ItemId == itemId) &&
                    (itemWMSId <= 0 || d.ItemWMSId == itemWMSId))
                .OrderBy(o => o.Workstation.SortOrder)
                .FirstOrDefault();
        }
        public WorkstationItem GetByAncIdAndLineId(int lineId, int itemId = 0, int itemWMSId = 0)
        {
            List<WorkstationItem> list = db.WorkstationItems.Where(d =>
                    (itemId > 0 || itemWMSId > 0) &&
                    (itemId <= 0 || d.ItemWMS.ItemId == itemId) &&
                    (itemWMSId <= 0 || d.ItemWMSId == itemWMSId) &&
                    (d.Workstation.LineId == lineId))
                .OrderBy(o => o.Workstation.SortOrder)
                .ToList();

            if (list.Count > 1)
            {
                list.RemoveAll(x => x.Workstation.Name == "PKF");
            }

            return list.FirstOrDefault();
        }

    }
}
