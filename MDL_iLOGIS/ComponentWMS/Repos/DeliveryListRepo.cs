using MDL_BASE.Interfaces;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_iLOGIS.ComponentWMS.Enums;
using MDL_iLOGIS.ComponentWMS.ViewModels;
using MDLX_CORE.ComponentCore.Entities;
using MDLX_MASTERDATA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLIB_COMMON.Interface;
using XLIB_COMMON.Repo;

namespace MDL_iLOGIS.ComponentWMS.Repos
{
    public class DeliveryListRepo : RepoGenericAbstract<DeliveryList>
    {
        protected new IDbContextiLOGIS db;

        public DeliveryListRepo(IDbContextiLOGIS db) : base(db)
        {
            this.db = db;
        }
        
        public override DeliveryList GetById(int id)
        {
            return db.DeliveryLists.FirstOrDefault(x => x.Id == id);
        }
        
        public IQueryable<DeliveryList> GetList(int transporterId, int woId)
        {
            var query = db.DeliveryLists.Where(x =>
                x.TransporterId == transporterId && x.WorkOrderId == woId)
                .OrderBy(x=>x.WorkOrder.StartDate);

            return query;
        }

        public List<DeliveryListViewModel> GetListFiltered(DateTime dateFrom, DateTime dateTo, int transporterId, List<int> lineIds, string orderNo, string pnc)
        {
            var query = (from wo in db.ProductionOrders.Where(x =>
                    //(x.Deleted == false) &&
                    (x.EndDate >= dateFrom && x.StartDate < dateTo) &&
                    (lineIds.Count == 0 || lineIds.Contains(x.LineId)) &&
                    (orderNo == null || orderNo == x.OrderNumber) &&
                    (pnc == null || pnc == x.Pnc.Code))
                join di in db.DeliveryLists.Where(x => (transporterId <= 0 || transporterId == x.TransporterId)) on wo.OrderNumber equals di.WorkOrder.OrderNumber into di2
                from di3 in di2.DefaultIfEmpty()
                select new DeliveryListViewModel()
                {
                    Id = di3 != null? di3.Id : 0,
                    TransporterId = di3 != null ? di3.TransporterId : 0,
                    TransporterName = di3 != null ? di3.Transporter.Name : "",
                    WorkorderId = wo.Id,
                    WorkorderNumber = wo.OrderNumber,
                    WorkorderDeleted = wo.Deleted,
                    Qty = wo.QtyPlanned,
                    ResourceName = wo.Line.Name,
                    StartDateTime = wo.StartDate,
                    ItemCode = wo.Pnc.Code,
                    Status = di3 != null ? di3.Status : EnumDeliveryListStatus.Pending,
                    PickingListStatuses = db.PickingLists.Where(x => x.WorkOrder.OrderNumber == wo.OrderNumber).Select(x => x.Status),
                })
                .Where(x => x.WorkorderDeleted == false || x.Status < EnumDeliveryListStatus.Completed)
                .OrderBy(x => x.StartDateTime);

            List<DeliveryListViewModel> list = query.ToList();

            list.ForEach(x => { 
                x.StartDateStr = x.StartDateTime.ToString("yyyy-MM-dd"); 
                x.StartTimeStr = x.StartDateTime.ToString("HH:mm"); 
            });

            return list;
        }
    }
}
