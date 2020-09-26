using MDL_BASE.Interfaces;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_iLOGIS.ComponentWMS.Enums;
using MDL_iLOGIS.ComponentWMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLIB_COMMON.Interface;
using XLIB_COMMON.Repo;

namespace MDL_iLOGIS.ComponentWMS.Repos
{
    public class PickingListRepo : RepoGenericAbstract<PickingList>
    {
        protected new IDbContextiLOGIS db;
        //private UnitOfWorkOneprod unitOfWork;

        public PickingListRepo(IDbContextiLOGIS db) : base(db)
        {
            this.db = db;
        }
        
        public override PickingList GetById(int id)
        {
            return db.PickingLists.FirstOrDefault(x => x.Id == id);
        }

        public List<PickingList> GetByWorkOrder (int workOrderId)
        {
            return db.PickingLists.Where(x => x.WorkOrderId == workOrderId).ToList();
        }

        public List<PickingList> GetByWorkOrderAndPicker(int workOrderId, int pickerId)
        {
            return db.PickingLists.Where(x => x.WorkOrderId == workOrderId && (pickerId == 0 || x.TransporterId == pickerId)).ToList();
        }

        public List<PickingList> GetByWorkOrdersAndPicker(List<int> workOrderIds, int pickerId)
        {
            return db.PickingLists.Where(x => workOrderIds.Contains(x.WorkOrderId) && (pickerId == 0 || x.TransporterId == pickerId)).ToList();
        }
        public List<PickingList> GetByGuid(string pickingListGuid)
        {
            return db.PickingLists.Where(x => x.Guid == pickingListGuid).ToList();
        }

        public List<PickingListWorkOrdersStatus> GetListFiltered(DateTime dateFrom, DateTime dateTo, List<int> lineIds, string orderNo, string pnc)
        {
            var query = from wo in db.ProductionOrders.Where(x =>
                        (x.EndDate >= dateFrom && x.StartDate < dateTo) &&
                        (lineIds.Count == 0 || lineIds.Contains(x.LineId)) &&
                        (orderNo == null || orderNo == x.OrderNumber) &&
                        (pnc == null || pnc == x.Pnc.Code))
                    join pi in db.PickingLists on wo.OrderNumber equals pi.WorkOrder.OrderNumber into pi2
                    from pi3 in pi2.DefaultIfEmpty()
                    select new PickingListWorkOrdersStatus() {
                        PickingListId = pi3 != null ? pi3.Id : 0,
                        Status = pi3 != null? pi3.Status : EnumPickingListStatus.Pending,
                        //StatusLF = pi3 != null ? pi3.StatusLF : EnumDeliveryListStatus.Pending,
                        ItemCode = wo.Pnc.Code,
                        WorkOrderNumber = wo.OrderNumber,
                        QtyPlanned = wo.QtyPlanned,
                        QtyRemain = wo.QtyRemain,
                        StartDate = wo.StartDate,
                        WorkOrderId = wo.Id,
                        ResourceId = wo.LineId,
                        ResourceName = wo.Line.Name
                    };
            var list = query.ToList();
            list.ForEach(x => { x.StartDateStr = x.StartDate.ToString("yyyy-MM-dd"); x.StartTimeStr = x.StartDate.ToString("HH:mm"); });

            return list;
        }

        public List<PickingListWorkOrdersStatus> GetListFiltered(DateTime dateFrom, DateTime dateTo, int pickerId, List<int> lineIds, string orderNo, string pnc)
        {
            var query = (from wo in db.ProductionOrders.Where(x =>
                            (x.Deleted == false) &&
                            (x.EndDate >= dateFrom && x.StartDate < dateTo) &&
                            (lineIds.Count == 0 || lineIds.Contains(x.LineId)) &&
                            (orderNo == null || orderNo == x.OrderNumber) &&
                            (pnc == null || pnc == x.Pnc.Code))
                        join pi in db.PickingLists.Where(x=> (pickerId <= 0 || pickerId == x.TransporterId) && x.Status < EnumPickingListStatus.Closed) on wo.OrderNumber equals pi.WorkOrder.OrderNumber into pi2
                        from pi3 in pi2.DefaultIfEmpty()
                        select new PickingListWorkOrdersStatus()
                        {
                            PickingListId = pi3 != null ? pi3.Id : 0,
                            Status = pi3 != null ? pi3.Status : EnumPickingListStatus.Unassigned,
                            //StatusLF = pi3 != null ? pi3.StatusLF : EnumDeliveryListStatus.Pending,
                            ItemCode = wo.Pnc.Code,
                            Guid = pi3.Guid,
                            GuidCreationDateTime = pi3.GuidCreationDate == null ? new DateTime(1900,1,1) : pi3.GuidCreationDate,
                            WorkOrderNumber = wo.OrderNumber,
                            QtyPlanned = wo.QtyPlanned,
                            QtyRemain = wo.QtyRemain,
                            StartDate = wo.StartDate,
                            WorkOrderId = wo.Id,
                            ResourceId = wo.LineId,
                            ResourceName = wo.Line.Name
                        }).OrderBy(x => x.StartDate);
            var list = query.ToList();
            list.ForEach(x => { x.StartDateStr = x.StartDate.ToString("yyyy-MM-dd"); x.StartTimeStr = x.StartDate.ToString("HH:mm"); });

            return list;
        }

        

        
    }
}
