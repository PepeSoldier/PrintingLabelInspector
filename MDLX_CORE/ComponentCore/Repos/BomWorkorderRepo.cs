using MDL_BASE.Interfaces;
using MDL_BASE.Models.MasterData;
using MDLX_CORE.ComponentCore.Entities;
using System.Collections.Generic;
using System.Linq;

namespace XLIB_COMMON.Repo.Base
{
    public class BomWorkorderRepo : RepoGenericAbstract<BomWorkorder>
    {
        protected new IDbContextCore db;

        public BomWorkorderRepo(IDbContextCore db) : base(db)
        {
            this.db = db;
        }

        //public override BomWorkorder GetById(int id)
        //{
        //    return db.BomWorkorders.FirstOrDefault(d => d.Id == id);
        //}
        //public override IQueryable<BomWorkorder> GetList()
        //{
        //    return db.BomWorkorders.OrderByDescending(x => x.Id);
        //}

        //public void TruncateTable()
        //{
        //    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [CORE].[BOM_Workorder]");
        //    //db.Database.ExecuteSqlCommand("TRUNCATE TABLE " + GetTableName());
        //}
        //public List<BomWorkorder> GetItemsOfWorkOrderFilteredByIds(ProductionOrder po, List<int> itemIds)
        //{
        //    return db.BomWorkorders.Where(x => 
        //        x.OrderNo == po.OrderNumber && 
        //        itemIds.Contains(x.ChildId)
        //    ).ToList();
        //}
    }
}