using MDL_BASE.Interfaces;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_iLOGIS.ComponentWMS.Models;
using MDL_iLOGIS.ComponentWMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLIB_COMMON.Interface;
using XLIB_COMMON.Repo;

namespace MDL_iLOGIS.ComponentWMS.Repos
{
    public class PickingListItemRepo : RepoGenericAbstract<PickingListItem>
    {
        protected new IDbContextiLOGIS db;
        //private UnitOfWorkOneprod unitOfWork;

        public PickingListItemRepo(IDbContextiLOGIS db) : base(db)
        {
            this.db = db;
        }
        
        public override PickingListItem GetById(int id)
        {
            return db.PickingListItems.FirstOrDefault(x => x.Id == id);
        }
        public PickingListItem GetByItemId(int id)
        {
            return db.PickingListItems.Where(x => x.ItemWMSId == id).FirstOrDefault();
        }
        public PickingListItem GetByItem_WarehouseLocation(string itemCode, string locationName)
        {
            return db.PickingListItems.Where(x => x.ItemWMS.Item.Code == itemCode && x.WarehouseLocation.Name == locationName).FirstOrDefault();
        }

        public List<PickingListItem> GetByWorkOrder (int workOrderId)
        {
            return db.PickingListItems.Where(x => x.PickingList.WorkOrderId == workOrderId).ToList();
        }
        public List<PickingListItem> GetByWorkOrderPicker(int workOrderId, int pickerId)
        {
            return db.PickingListItems.Where(x => x.PickingList.WorkOrderId == workOrderId && x.PickingList.TransporterId == pickerId).ToList();
        }
        public List<PickingListItemViewModel_2> GetByWorkOrderAndPickerWithWorkstation(int workOrderId, int pickerId, int lineId, int parameterH)
        {
            var query = from pli in db.PickingListItems
                            .Where(x => x.PickingList.WorkOrderId == workOrderId &&
                                        x.PickingList.TransporterId == pickerId) 
                        join wls in db.WarehouseLocationSorts
                            on pli.WarehouseLocation.RegalNumber equals wls.RegalNumber into wls2
                        join wst in db.WorkstationItems.Where(x => x.Workstation.LineId == lineId)
                            on pli.ItemWMSId equals wst.ItemWMSId into wst2
                        from wls3 in wls2.DefaultIfEmpty()
                        from wst3 in wst2.DefaultIfEmpty()
                        select new PickingListItemViewModel_2()
                        {
                            Id = pli.Id,
                            PickingListId = pli.PickingListId,
                            ItemId = pli.ItemWMSId,
                            TransporterId = pli.PickingList.TransporterId,
                            TransporterName = pli.PickingList.Transporter.Name,
                            PlatformId = pli.Platform != null ? pli.Platform.Id : 0,
                            PlatformName = pli.Platform != null ? pli.Platform.Name : "",
                            ConnectedTransporters = pli.PickingList.Transporter.ConnectedTransporters,
                            ItemCode = pli.ItemWMS.Item.Code,
                            ItemName = pli.ItemWMS.Item.Name,
                            WorkOrderId = workOrderId,
                            WorkOrderNo = pli.PickingList.WorkOrder.OrderNumber,
                            QtyRequested = pli != null ? pli.QtyRequested : 0,
                            UnitOfMeasure = pli != null ? pli.UnitOfMeasure : XLIB_COMMON.Enums.UnitOfMeasure.Undefined,
                            QtyPicked = pli != null ? pli.QtyPicked : 0,
                            WarehouseLocationName = pli.WarehouseLocation.Name,
                            WarehouseLocationId = pli.WarehouseLocationId,
                            Balance = (pli != null ? pli.QtyPicked : 0) - (pli != null ? pli.QtyRequested : 0),
                            Status = pli.Status,
                            StatusLFI = pli.StatusLFI,
                            H = pli.ItemWMS.H,
                            ResourceId = pli.PickingList.WorkOrder.Line.Id,
                            ResourceName = pli.PickingList.WorkOrder.Line.Name,
                            WorkstationId = wst3 != null ? wst3.WorkstationId : 0,
                            WorkstationName = wst3 != null && wst3.Workstation != null ? wst3.Workstation.Name : "",
                            WorkstationSortOrder= wst3 != null && wst3.Workstation != null ? wst3.Workstation.SortOrderTrain : 0,
                            PutTo = wst3.PutTo
                        };

            List<PickingListItemViewModel_2> list = query.Where(x => (parameterH < 0 || x.H == parameterH))
                .OrderBy(x => x.WorkstationSortOrder)
                .ThenBy(x => x.ItemName)
                .ToList();

            return list;
        }
        public List<PickingListPlatformViewModel> GePlatformsDistinct(int workOrderId, int pickerId, int lineId, int parameterH)
        {
            var query = from pli in db.PickingListItems
                            .Where(x => x.PickingList.WorkOrderId == workOrderId &&
                                        x.PickingList.TransporterId == pickerId)
                        join wst in db.WorkstationItems.Where(x => x.Workstation.LineId == lineId)
                            on pli.ItemWMSId equals wst.ItemWMSId into wst2
                        from wst3 in wst2.DefaultIfEmpty()
                        where parameterH < 0 || wst3.ItemWMS.H == parameterH
                        select new PickingListPlatformViewModel()
                        {
                            PlatformId = pli.Platform != null ? pli.Platform.Id : 0,
                            PlatformLocationName = pli.Platform != null && pli.Platform.ParentWarehouseLocation != null ? pli.Platform.ParentWarehouseLocation.Name : "",
                            PlatformName = pli.Platform != null ? pli.Platform.Name : "",
                        };
            List<PickingListPlatformViewModel> list = query.Distinct().Where(x => x.PlatformName != "").OrderBy(x => x.PlatformName).ToList();
            return list;
        }
        public IQueryable<PickingListItemViewModel_2> GetByWorkOrderAndPicker(int workOrderId, int pickerId, string guid = null)
        {
            guid = guid.Length > 0 ? guid : null;
            var query = from pli in db.PickingListItems
                            .Where(x => 
                                    (guid == null && (x.PickingList.WorkOrderId == workOrderId && (pickerId == 0 || x.PickingList.TransporterId == pickerId))) ||
                                    (guid != null && x.PickingList.Guid == guid)
                                ) //&& 
                                        //(paramH == -2 || x.Item.H == paramH))
                        join wls in db.WarehouseLocationSorts
                            on pli.WarehouseLocation.RegalNumber equals wls.RegalNumber into wls2
                        from wls3 in wls2.DefaultIfEmpty()
                        //select pli.QtyPicked;
                        select new { pli, wls3 };
            //var data2 = query.ToList();

            var data = query //.ToList()
                .OrderBy(x => x.wls3 != null ? x.wls3.SortOrder : 9999999)
                .ThenByDescending(x => x.pli.WarehouseLocation.AvailableForPicker)
                .ThenBy(x => x.wls3 != null ? x.wls3.SortColumnAscending == 1 ? x.pli.WarehouseLocation.ColumnNumber : x.pli.WarehouseLocation.ColumnNumber * -1 : 0)
                .Select(x => new PickingListItemViewModel_2()
                {
                    Id = x.pli.Id,
                    PickingListId = x.pli.PickingListId,
                    ItemId = x.pli.ItemWMSId,
                    TransporterId = x.pli.PickingList.TransporterId,
                    TransporterName = x.pli.PickingList.Transporter.Name,
                    PlatformId = x.pli.Platform != null ? x.pli.Platform.Id : 0,
                    PlatformName = x.pli.Platform != null ? x.pli.Platform.Name : "",
                    ConnectedTransporters = x.pli.PickingList.Transporter.ConnectedTransporters,
                    ItemCode = x.pli.ItemWMS.Item.Code,
                    ItemName = x.pli.ItemWMS.Item.Name,
                    WorkOrderId = workOrderId,
                    WorkOrderNo = x.pli.PickingList.WorkOrder.OrderNumber,
                    QtyRequested = x.pli != null ? x.pli.QtyRequested : 0,
                    UnitOfMeasure = x.pli != null? x.pli.UnitOfMeasure : XLIB_COMMON.Enums.UnitOfMeasure.Undefined,
                    QtyPicked = x.pli != null ? x.pli.QtyPicked : 0,
                    WarehouseLocationName = x.pli.WarehouseLocation.Name,
                    WarehouseLocationId = x.pli.WarehouseLocationId,
                    ResourceId = x.pli.PickingList.WorkOrder.Line.Id,
                    ResourceName = x.pli.PickingList.WorkOrder.Line.Name,
                    Balance = (x.pli != null ? x.pli.QtyPicked : 0) - (x.pli != null ? x.pli.QtyRequested : 0),
                    Status = x.pli.Status,
                    H = x.pli.ItemWMS.H
                }); //.ToList();
            //return new List<PickingListItemsStatus>();
            return data;
        }
        public List<PickingListItemViewModel_2> GetSummary(int workOrderId, int pickerId)
        {
            var query = db.PickingListItems
                .Where(x => x.PickingList.WorkOrderId == workOrderId && x.PickingList.TransporterId == pickerId).GroupBy(x => x.ItemWMSId)
                .Select(g => new PickingListItemViewModel_2()
                {
                    Id = g.FirstOrDefault().ItemWMSId,
                    ItemCode = g.FirstOrDefault().ItemWMS.Item.Code,
                    ItemName = g.FirstOrDefault().ItemWMS.Item.Name,
                    WorkOrderId = workOrderId,
                    QtyRequested = g.Sum(x => x.QtyRequested),
                    UnitOfMeasure = g.FirstOrDefault().UnitOfMeasure,
                    QtyPicked = g.Sum(x => x.QtyPicked),
                    TransporterId = g.FirstOrDefault().PickingList.TransporterId,
                    TransporterName = g.FirstOrDefault().PickingList.Transporter.Name,
                    ConnectedTransporters = g.FirstOrDefault().PickingList.Transporter.ConnectedTransporters,
                    ItemId = g.FirstOrDefault().ItemWMSId,
                    Balance = g.Sum(x => x.QtyPicked) - g.Sum(x => x.QtyRequested),
                    WarehouseLocationName = g.FirstOrDefault().WarehouseLocation.Name,
                    WarehouseLocationId = g.FirstOrDefault().WarehouseLocationId,
                    Status = g.FirstOrDefault().Status,
                }).OrderBy(x => x.ItemCode).ToList();
            return query;
        }

        public WarehouseLocation GetPlatformIdByWorkOrderId(int workOrderId)
        {
            List<int> pickingLists = db.PickingLists.Where(x => x.WorkOrderId == workOrderId).Select(x => x.Id).ToList();
            PickingListItem pli =  db.PickingListItems.Where(x => pickingLists.Contains(x.PickingListId) && x.QtyPicked > 0 && x.PlatformId != null).FirstOrDefault();
            return pli != null ? pli.Platform : null;
        }
        public WarehouseLocation GetPlatformIdByWorkOrderIdAndPickerId(PickingList pickingList)
        {
            List<int> pickingLists = db.PickingLists.Where(x => x.WorkOrderId == pickingList.WorkOrderId).Select(x => x.Id).ToList();
            PickingListItem pli = db.PickingListItems.Where(x => pickingLists.Contains(x.PickingListId) && x.PickingList.TransporterId == pickingList.TransporterId && x.QtyPicked > 0 && x.PlatformId != null).FirstOrDefault();
            if(pli == null)
            {
                pli = db.PickingListItems.Where(x => pickingLists.Contains(x.PickingListId) && x.QtyPicked > 0 && x.PlatformId != null).FirstOrDefault();
            }
            return pli != null ? pli.Platform : null;
        }

    }
}
