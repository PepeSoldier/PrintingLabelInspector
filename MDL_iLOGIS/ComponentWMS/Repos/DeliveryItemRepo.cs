using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_iLOGIS.ComponentWMS.EntityInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using XLIB_COMMON.Repo;

namespace MDL_iLOGIS.ComponentWMS.Repos
{
    public class DeliveryItemRepo : RepoGenericAbstract<DeliveryItem>
    {
        protected new IDbContextiLOGIS db;

        public DeliveryItemRepo(IDbContextiLOGIS db) : base(db)
        {
            this.db = db;
        }

        public IQueryable<DeliveryItem> GetList(DeliveryItem filter)
        {
            return db.DeliveryItems.Where(x =>
                    (filter.DeliveryId < 0 || x.DeliveryId == filter.DeliveryId) &&
                    (filter.ItemWMSId <= 0 || x.ItemWMSId == filter.ItemWMSId) &&
                    x.Deleted == false)
                .OrderBy(x => x.DeliveryId);
        }

        public IQueryable<int> GetUniqueDeliveryId(IDeliveryFilter filter)
        {
            return db.DeliveryItems
                .Where(x => 
                    (filter.ItemCode == null || x.ItemWMS.Item.Code == filter.ItemCode) &&
                    (filter.SupplierCode == null || x.Delivery.Supplier.Code == filter.SupplierCode) &&
                    (filter.Id == 0 || x.DeliveryId == filter.Id) &&
                    (filter.Guid == "" || x.Delivery.Guid == filter.Guid)
                )
                .Select(x => x.DeliveryId)
                .Distinct();
        }

        public IQueryable<int> GetDeliveryIdByGuid(IDeliveryFilter filter)
        {
            return db.DeliveryItems
                .Where(x =>
                    (filter.ItemCode == null || x.ItemWMS.Item.Code == filter.ItemCode) &&
                    (filter.SupplierCode == null || x.Delivery.Supplier.Code == filter.SupplierCode) &&
                    (filter.Guid == "" || x.Delivery.Guid == filter.Guid)
                )
                .Select(x => x.DeliveryId);
        }
        public IQueryable<DeliveryItem> GetByIds(List<int> ids)
        {
            return db.DeliveryItems.Where(x => ids.Contains(x.Id));
        }
        public IQueryable<DeliveryItem> GetByDeliveryId(int deliveryId)
        {
            return db.DeliveryItems.Where(x => x.DeliveryId == deliveryId).OrderBy(x => x.DeliveryId);
        }

        public List<DeliveryItem> GetGrouppedByItemCde(int deliveryId, bool? operatorEntry =  null)
        {
            return db.DeliveryItems.Where(x => 
                (x.DeliveryId == deliveryId) &&
                (operatorEntry == null || x.OperatorEntry == operatorEntry.Value) &&
                (x.Deleted == false))
            .GroupBy(x => new { x.ItemWMS, x.OperatorEntry, x.UnitOfMeasure })
            .ToList()
            .Select(x => new DeliveryItem() { 
                Id = x.Min(a => a.Id),
                AdminEntry = !x.Key.OperatorEntry,
                OperatorEntry = x.Key.OperatorEntry,
                ItemWMS = x.Key.ItemWMS,
                ItemWMSId = x.Key.ItemWMS.Id,
                MovementType = x.Min(a => a.MovementType),
                NumberOfPackages = x.Sum(a => a.NumberOfPackages),
                QtyInPackage = x.Sum(a => a.QtyInPackage),
                TotalQty = x.Sum(a => a.QtyInPackage * a.NumberOfPackages),
                UnitOfMeasure = x.Key.UnitOfMeasure,
                Deleted = false,
                Delivery = x.FirstOrDefault().Delivery,
                DeliveryId = x.FirstOrDefault().DeliveryId,
                StockStatus = Enums.StatusEnum.Available,
                DestinationWarehouseCode = x.FirstOrDefault().DestinationWarehouseCode,
                IsSpecialStock = x.FirstOrDefault().IsSpecialStock,
                TotalLocatedQty = x.Sum(a => a.TotalLocatedQty),
                User = x.FirstOrDefault().User,
                UserId = x.FirstOrDefault().UserId,
                WasPrinted = false
            })
            .OrderBy(x => x.ItemWMS.Item.Code)
            .ToList();
        }

        public List<DeliveryItem> GetGrouppedByItemCde(string groupGuid, bool? operatorEntry = null)
        {
            return db.DeliveryItems.Where(x =>
                (x.Delivery.Guid == groupGuid) &&
                (operatorEntry == null || x.OperatorEntry == operatorEntry.Value) &&
                (x.Deleted == false))
            .GroupBy(x => new { x.ItemWMS, x.OperatorEntry, x.UnitOfMeasure })
            .ToList()
            .Select(x => new DeliveryItem()
            {
                Id = x.Min(a => a.Id),
                AdminEntry = !x.Key.OperatorEntry,
                OperatorEntry = x.Key.OperatorEntry,
                ItemWMS = x.Key.ItemWMS,
                ItemWMSId = x.Key.ItemWMS.Id,
                MovementType = x.Min(a => a.MovementType),
                NumberOfPackages = x.Sum(a => a.NumberOfPackages),
                QtyInPackage = x.Sum(a => a.QtyInPackage),
                TotalQty = x.Sum(a => a.QtyInPackage * a.NumberOfPackages),
                UnitOfMeasure = x.Key.UnitOfMeasure,
                Deleted = false,
                Delivery = x.FirstOrDefault().Delivery,
                DeliveryId = x.FirstOrDefault().DeliveryId,
                StockStatus = Enums.StatusEnum.Available,
                DestinationWarehouseCode = x.FirstOrDefault().DestinationWarehouseCode,
                IsSpecialStock = x.FirstOrDefault().IsSpecialStock,
                TotalLocatedQty = x.Sum(a => a.TotalLocatedQty),
                User = x.FirstOrDefault().User,
                UserId = x.FirstOrDefault().UserId,
                WasPrinted = false
            })
            .OrderBy(x => x.ItemWMS.Item.Code)
            .ToList();
        }

        public List<DeliveryItem> GetReferenceDeliveryItemsIdsForOperatorEntries(int id, string guid = "")
        {
            List<DeliveryItem> deliveryItems = new List<DeliveryItem>();
            if (guid != "")
            {
                deliveryItems = db.DeliveryItems.Where(x => x.Delivery.Guid == guid && x.Deleted == false && x.OperatorEntry == true).ToList();
            }
            else
            {
                deliveryItems = db.DeliveryItems.Where(x => x.DeliveryId == id &&  x.Deleted == false && x.OperatorEntry == true).ToList();
            }
            return deliveryItems;
        }

        public List<DeliveryItem> GetReferenceDeliveryItemsIdsForOperatorEntriesAndItemId(int itemWmsId,int id, string guid = "")
        {
            List<DeliveryItem> deliveryItems = new List<DeliveryItem>();
            if (guid != "")
            {
                deliveryItems = db.DeliveryItems.Where(x => x.Delivery.Guid == guid && x.Deleted == false && x.OperatorEntry == true && x.ItemWMSId == itemWmsId).ToList();
            }
            else
            {
                deliveryItems = db.DeliveryItems.Where(x => x.DeliveryId == id && x.Deleted == false && x.OperatorEntry == true && x.ItemWMSId == itemWmsId).ToList();
            }
            return deliveryItems;
        }
    }
}