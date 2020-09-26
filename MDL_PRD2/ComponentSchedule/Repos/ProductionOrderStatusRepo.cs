using MDL_BASE.Models.MasterData;
using XLIB_COMMON.Repo;
using MDL_PRD.Interface;
using MDL_PRD.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MDL_PRD.Repo.Schedule
{
    public class ProductionOrderStatusRepo : RepoGenericAbstract<ProdOrderStatus>
    {
        protected new IDbContextPRD db;

        public ProductionOrderStatusRepo(IDbContextPRD db) : base(db)
        {
            this.db = db;
        }

        public override ProdOrderStatus GetById(int id)
        {
            return db.ProdOrderStatuses.FirstOrDefault(d => d.Id == id);
        }
        public override IQueryable<ProdOrderStatus> GetList()
        {
            return db.ProdOrderStatuses.OrderByDescending(x => x.Id);
        }

        public List<ProdOrderStatus> GetListByOrderId(int orderId)
        {
            return db.ProdOrderStatuses.Where(d => d.OrderId == orderId).ToList();
        }
        

        
    }
}