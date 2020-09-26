using MDL_BASE.Interfaces;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_iLOGIS.ComponentWMS.ViewModels;
using MDLX_CORE.ComponentCore.Entities;
using MDLX_MASTERDATA.Entities;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLIB_COMMON.Interface;
using XLIB_COMMON.Repo;

namespace MDL_iLOGIS.ComponentWMS.Repos
{
    public class DeliveryListItemRepo : RepoGenericAbstract<DeliveryListItem>
    {
        protected new IDbContextiLOGIS db;

        public DeliveryListItemRepo(IDbContextiLOGIS db) : base(db)
        {
            this.db = db;
        }
        
        public override DeliveryListItem GetById(int id)
        {
            return db.DeliveryListItems.FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<DeliveryListItem> GetList(int deliveryListId)
        {
            var query = db.DeliveryListItems.Include(x => x.ItemWMS).Where(x => x.DeliveryListId == deliveryListId)
                .OrderBy(x => x.Workstation.SortOrder);

            return query;
        }
        public IQueryable<DeliveryListItem> GetList(int trainId, int woId)
        {
            var query = db.DeliveryListItems.Where(x =>
                x.TransporterId == trainId && x.WorkOrderId == woId)
                .OrderBy(x=>x.Workstation.SortOrder);

            return query;
        }
        public IQueryable<DeliveryListItem> GetListByItemIdAndWorkstationId(int itemWMSId, int? workstationId, int transporterId, int? lineId = null)
        {
            var query = db.DeliveryListItems.Where(x =>
                        x.WorkOrder.Deleted == false &&
                        x.TransporterId == transporterId &&
                        x.ItemWMSId == itemWMSId &&
                        (workstationId == null || workstationId == 0 || x.WorkstationId == workstationId) &&
                        (lineId == null || lineId == x.WorkOrder.LineId)
                        )
                .OrderBy(x => x.WorkOrder.StartDate);

            return query;
               
        }

        public List<DeliveryListItem> Simulate(List<int> connectedTransporters, int qtyRemain, int trainId, int woId, string woNumber, string itemCode, string[] DedicatedResourcesArray)
        {
            //trzeba filtrować db.Items po connected transorters najpierw bo 
            //inaczej za długa lista jest.

            var query = from itemWMS in db.ItemWMS.Where(x => connectedTransporters.Contains(x.TrainNo))
                        join wb in db.BomWorkorders.Where(x=>x.OrderNo == woNumber)
                            on itemWMS.Item.Id equals wb.ChildId
                        join wi in db.WorkstationItems.Where(x => x.CheckOnly == false && DedicatedResourcesArray.Contains(x.Workstation.Line.Name))
                            //on wb.ChildId equals wi.ItemId into wi2
                            on itemWMS.Id equals wi.ItemWMSId into wi2
                        join pi in db.PackageItems 
                            //on wb.ChildId equals pi.ItemId into pi2
                            on itemWMS.Id equals pi.ItemWMSId into pi2
                        from pi3 in pi2.DefaultIfEmpty()
                        from wi3 in wi2.DefaultIfEmpty()
                        select new
                        {
                            //ItemId = wb.ChildId,
                            ItemId = itemWMS.Id,
                            QtyDelivered = 0,
                            QtyPerPackage = pi3 != null ? pi3.QtyPerPackage > 0? pi3.QtyPerPackage : 1 : 1,
                            QtyUsed = 0,
                            WorkstationId = wi3 != null? (int?)wi3.WorkstationId : null,
                            QtyRequested = qtyRemain * wb.QtyUsed,
                            TransporterId = trainId,
                            WorkOrderId = woId,
                            BomQty = wb.QtyUsed
                        };

            var list = query.ToList();

            List<DeliveryListItem>  list2 = list.Select(x => new DeliveryListItem()
            {
                ItemWMSId = x.ItemId,
                QtyDelivered = x.QtyDelivered,
                QtyPerPackage = x.QtyPerPackage,
                QtyRequested = (int)x.QtyRequested,
                QtyUsed = x.QtyUsed,
                TransporterId = x.TransporterId,
                WorkOrderId = x.WorkOrderId,
                WorkstationId = x.WorkstationId,
                BomQty = x.BomQty,
                IsSubstituteData = false
            }).ToList();

            if(list2.Count <= 0)
            {
                list2 = SimulateFromBOM(connectedTransporters, qtyRemain, trainId, woId, itemCode, DedicatedResourcesArray);
            }

            return list2;
        }
        private List<DeliveryListItem> SimulateFromBOM(List<int> connectedTransporters, int qtyRemain, int trainId, int woId, string itemCode, string[] DedicatedResourcesArray)
        {
            //trzeba filtrować db.Items po connected transorters najpierw bo 
            //inaczej za długa lista jest.

            var query = from itemWMS in db.ItemWMS.Where(x => connectedTransporters.Contains(x.TrainNo))
                        join wb in db.Boms.Where(x => x.Pnc.Code == itemCode && (x.LV == 1)) //|| x.Anc.Code == "156122200"))
                            on itemWMS.Item.Id equals wb.AncId
                        join wi in db.WorkstationItems.Where(x => x.CheckOnly == false && DedicatedResourcesArray.Contains(x.Workstation.Line.Name))
                            //on wb.AncId equals wi.ItemId into wi2
                            on itemWMS.Id equals wi.ItemWMSId into wi2
                        join pi in db.PackageItems
                            //on wb.AncId equals pi.ItemId into pi2
                            on itemWMS.Id equals pi.ItemWMSId into pi2
                        from pi3 in pi2.DefaultIfEmpty()
                        from wi3 in wi2.DefaultIfEmpty()
                        select new
                        {
                            //ItemId = wb.AncId != null? (int)wb.AncId : 0,
                            ItemId = itemWMS.Id,
                            QtyDelivered = 0,
                            QtyPerPackage = pi3 != null ? pi3.QtyPerPackage > 0 ? pi3.QtyPerPackage : 1 : 1,
                            QtyUsed = 0,
                            WorkstationId = wi3 != null ? (int?)wi3.WorkstationId : null,
                            QtyRequested = qtyRemain * wb.PCS,
                            TransporterId = trainId,
                            WorkOrderId = woId,
                            BomQty = wb.PCS
                        };

            var list = query.ToList();

            List<DeliveryListItem> list2 = list.Select(x => new DeliveryListItem()
            {
                ItemWMSId = x.ItemId,
                QtyDelivered = x.QtyDelivered,
                QtyPerPackage = x.QtyPerPackage,
                QtyRequested = (int)x.QtyRequested,
                QtyUsed = x.QtyUsed,
                TransporterId = x.TransporterId,
                WorkOrderId = x.WorkOrderId,
                WorkstationId = x.WorkstationId,
                BomQty = x.BomQty,
                IsSubstituteData = true
            }).ToList();

            return list2;
        }

        public List<DeliveryListItemViewModel> GetDeliveryListItemsLF(int workOrderId, int pickerId, int lineId, int parameterH)
        {
            var query = from dli in db.DeliveryListItems
                            .Where(x => x.WorkOrderId == workOrderId &&
                                        x.TransporterId == pickerId)
                        join wst in db.WorkstationItems.Where(x => x.Workstation.LineId == lineId)
                            on dli.ItemWMSId equals wst.ItemWMSId into wst2
                        from wst3 in wst2.DefaultIfEmpty()
                        select new DeliveryListItemViewModel()
                        {
                            Id = dli.Id,
                            DeliveryListId = dli.DeliveryListId??0,
                            ItemWMSId = dli.ItemWMSId,
                            TransporterId = dli.TransporterId,
                            TransporterName = dli.Transporter.Name,
                            WarehouseLocationId = dli.WarehouseLocation != null ? dli.WarehouseLocation.Id : 0,
                            WarehouseLocationName = dli.WarehouseLocation != null ? dli.WarehouseLocation.Name : "",
                            //ConnectedTransporters = pli.Transporter.ConnectedTransporters,
                            Code = dli.ItemWMS.Item.Code,
                            Name = dli.ItemWMS.Item.Name,
                            WorkorderId = workOrderId,
                            WorkorderNumber = dli.WorkOrder.OrderNumber,
                            QtyRequested = dli != null ? dli.QtyRequested : 0,
                            QtyDelivered = dli != null ? dli.QtyDelivered : 0,
                            QtyUsed = dli.QtyUsed,
                            QtyPerPackage = dli.QtyPerPackage,
                            BomQty = dli.BomQty,
                            StockUnitId = dli.StockUnitId??0,
                            Status = dli.Status,
                            WorkstationId = wst3 != null ? wst3.Workstation.Id : 0,
                            Workstation = wst3 != null? wst3.Workstation.Name : "",
                            WorkstationName = wst3 != null ? wst3.Workstation.Name : "",
                            WorkstationOrder = wst3 != null ? wst3.Workstation.SortOrderTrain + (wst3.PutTo == "L"? wst3.Workstation.FlowRackLOverride : 0) : -1000000,
                            WorkstationPutTo = wst3 != null ? wst3.PutTo : "",
                            ParameterH = dli.ItemWMS.H,
                            ResourceId = dli.WorkOrder.LineId,
                            ResourceName = dli.WorkOrder.Line.Name,
                            IsSubstituteData = dli.IsSubstituteData
                        };

            List<DeliveryListItemViewModel> list = query.Where(x => (parameterH < 0 || x.ParameterH == parameterH))
                .OrderBy(x => x.WorkstationOrder)
                .ThenBy(x => x.Name)
                .ToList();

            return list;
        }

        public void DeleteByWorkorderId(int workorderId)
        {
            string query = "DELETE FROM [iLOGIS].WMS_DeliveryListItem WHERE WorkOrderId = " + workorderId.ToString();
            db.Database.ExecuteSqlCommand(query);
        }
    }
}
