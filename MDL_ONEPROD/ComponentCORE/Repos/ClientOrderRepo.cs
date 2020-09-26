using XLIB_COMMON.Repo;
using MDL_ONEPROD.Common;
using MDL_ONEPROD.ComponentOEE.Models;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Interface;

namespace MDL_ONEPROD.Repo.Scheduling
{
    public class ClientOrderRepo : RepoGenericAbstract<ClientOrder>
    {
        protected new IDbContextOneprod db;
        UnitOfWorkOneprod unitOfWork;

        public ClientOrderRepo(IDbContextOneprod db, UnitOfWorkOneprod unitOfWork = null) : base(db)
        {
            this.db = db;
            this.unitOfWork = unitOfWork;
        }

        public override ClientOrder GetById(int id)
        {
            return db.ClientOrders.FirstOrDefault(x => x.Id == id);
        }
        public override IQueryable<ClientOrder> GetList()
        {
            return db.ClientOrders.Where(x => 
                    x.Deleted == false )
                .OrderBy(x => x.StartDate);
        }
        
        public IQueryable<ClientOrder> GetList(ClientOrder filterItem)
        {
            return db.ClientOrders.Where(x =>
                        (x.Deleted == false) &&
                        (filterItem.OrderNo == null || x.OrderNo == filterItem.OrderNo) &&
                        (filterItem.ClientId == null || x.ClientId == filterItem.ClientId) &&
                        (filterItem.ClientItemCode == null || x.ClientItemCode == filterItem.ClientItemCode) &&
                        (filterItem.ClientItemName == null || x.ClientItemName.StartsWith(filterItem.ClientItemName)))
                    .OrderBy(x => x.StartDate)
                    .ThenBy(x => x.InsertDate);
        }

        public IQueryable<ClientOrder> GetListByDates(DateTime from, DateTime to)
        {
            return db.ClientOrders.Where(o =>
                    (from <= o.StartDate && o.StartDate <= to))
                .OrderBy(o => o.StartDate);
        }
        
        public List<ClientOrder> GetListByResourceId(int resourceId, DateTime from, DateTime to)
        {
            return db.ClientOrders.Where(o => 
                    (o.Resource == resourceId.ToString()) && 
                    (from <= o.StartDate && o.StartDate <= to))
                .OrderBy(o => o.StartDate)
                .ToList();
        }
    }
}