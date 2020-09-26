using System;
using System.Linq;
using XLIB_COMMON.Interface;
using XLIB_COMMON.Model;
using XLIB_COMMON.Repo;
using System.Data.Entity;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentConfig.EntityInterfaces;

namespace MDL_iLOGIS.ComponentConfig.Repos
{
    public class PackageItemRepo : RepoGenericAbstract<PackageItem>
    {
        protected new IDbContextiLOGIS db;

        public PackageItemRepo(IDbContextiLOGIS db, IAlertManager alertManager = null)
            : base(db)
        {
            this.db = db;
        }

        public override PackageItem GetById(int id)
        {
            return db.PackageItems
                    .Include(x => x.Package)
                    .Include(x => x.ItemWMS)
                    .Include(x => x.Warehouse)
                    .Include(x => x.WarehouseLocationType)
                .FirstOrDefault(x => x.Id == id);
        }
        public override IQueryable<PackageItem> GetList()
        {
            return db.PackageItems.OrderBy(x => x.Id);
        }
        public IQueryable<PackageItem> GetList(IPackageItemFilter filter)
        {
            var query = db.PackageItems
                .Where(x =>
                    (x.Deleted == false) &&
                    (filter.PackageCode == null || filter.PackageCode.Length <= 0 || x.Package.Code == filter.PackageCode) &&
                    (filter.PackageName == null || filter.PackageName.Length <= 0 || x.Package.Name.Contains(filter.PackageName)) &&
                    //(filter.PackageId <= 0 || x.PackageId == filter.PackageId) &&
                    (filter.QtyPerPackage <= 0 || x.QtyPerPackage == filter.QtyPerPackage) &&
                    (filter.ItemName == null || x.ItemWMS.Item.Name.Contains(filter.ItemName)) &&
                    (filter.ItemCode == null || x.ItemWMS.Item.Code.Contains(filter.ItemCode)) &&
                    (filter.PickingStrategy <= 0 || x.PickingStrategy == filter.PickingStrategy) &&
                    (filter.WarehouseName == null || x.Warehouse.Name.Contains(filter.WarehouseName)) &&
                    (filter.WarehouseLocationTypeName == null || x.WarehouseLocationType.Name.Contains(filter.WarehouseLocationTypeName))
                )
                .OrderByDescending(x => x.Id);

            return query;
        }
        public decimal GetQtyPerPackage(int packageId, int itemWMSId)
        {
            return db.PackageItems.Where(x => x.PackageId == packageId && x.ItemWMSId == itemWMSId && x.Deleted == false).FirstOrDefault().QtyPerPackage;
        }

        public decimal GetWeightPerPackage(int packageId, int itemWMSId)
        {
            return db.PackageItems.Where(x => x.PackageId == packageId && x.ItemWMSId == itemWMSId && x.Deleted == false).FirstOrDefault().WeightNet;
        }

        public PackageItem GetByItemId(int itemId = 0, int itemWMSId = 0)
        {
            return db.PackageItems.FirstOrDefault(d =>
                (d.Deleted == false) &&
                (itemId > 0 || itemWMSId > 0) &&
                (itemId <= 0 || d.ItemWMS.ItemId == itemId) &&
                (itemWMSId <= 0 || d.ItemWMSId == itemWMSId));
        }

        public PackageItem Get(ItemWMS itemWMS, decimal qtyPerPackage = 0, int warehouseId = 0, int locationTypeId = 0, int packageId = 0, int locationId = 0, int? parentWarehouseId = null)
        {
            return GetList(itemWMS, qtyPerPackage, warehouseId, locationTypeId, packageId, locationId, parentWarehouseId).FirstOrDefault();
            //if(locationTypeId <= 0 && locationId > 0)
            //{
            //    WarehouseLocation wl = db.WarehouseLocations.FirstOrDefault(x => x.Id == locationId);
            //    locationTypeId = wl != null && wl.TypeId != null ? (int)wl.TypeId : 0;
            //}

            //return db.PackageItems.FirstOrDefault(x => 
            //    (x.ItemWMSId == itemWMS.Id || (itemWMS.Item.ItemGroupId != null && itemWMS.Item.ItemGroupId == x.ItemWMS.Item.Id)) 
            //    &&
            //    (
            //        (
            //          ((warehouseId <= 0 || x.WarehouseId == warehouseId) || 
            //           (x.Warehouse.ParentWarehouseId != null && x.Warehouse.ParentWarehouseId == warehouseId) ||
            //           (parentWarehouseId != null && x.WarehouseId == parentWarehouseId)) 
            //          &&
            //         (packageId <= 0 || x.PackageId == packageId) &&
            //         (locationTypeId <= 0 || x.WarehouseLocationTypeId == locationTypeId)
            //        ) 
            //        || 
            //        (x.QtyPerPackage == qtyPerPackage)
            //    )
            //);
        }

        public IQueryable<PackageItem> GetList(ItemWMS itemWMS, decimal qtyPerPackage = 0, int warehouseId = 0, int locationTypeId = 0, int packageId = 0, int locationId = 0, int? parentWarehouseId = null)
        {
            if (locationTypeId <= 0 && locationId > 0)
            {
                WarehouseLocation wl = db.WarehouseLocations.FirstOrDefault(x => x.Id == locationId);
                locationTypeId = wl != null && wl.TypeId != null ? (int)wl.TypeId : 0;
            }

            if (itemWMS != null)
            {
                return db.PackageItems.Where(x =>
                    (x.Deleted == false) &&
                    (x.ItemWMSId == itemWMS.Id || (itemWMS.Item.ItemGroupId != null && itemWMS.Item.ItemGroupId == x.ItemWMS.Item.Id))
                    &&
                    (
                        (
                          ((warehouseId <= 0 || x.WarehouseId == warehouseId) ||
                           (x.Warehouse.ParentWarehouseId != null && x.Warehouse.ParentWarehouseId == warehouseId) ||
                           (parentWarehouseId != null && x.WarehouseId == parentWarehouseId))
                          &&
                         (
                          (locationTypeId <= 0 || x.WarehouseLocationTypeId == locationTypeId) &&
                          ((packageId <= 0 || x.PackageId == packageId) ||
                           (x.QtyPerPackage == qtyPerPackage))
                         )
                        )
                        //|| (x.QtyPerPackage == qtyPerPackage)
                    )
                );
            }
            else
            {
                return db.PackageItems.Where(x => x.Id < 0 && x.Deleted == false);
            }
        }

        public AlertModel SynchronizeWithOldDB()
        {
            AlertModel alert = new AlertModel();
            try
            {
                db.Database.ExecuteSqlCommand("EXEC [dbo].[PFEP_SynchronizeWith_PFEP_PRD]");
                alert.MessageType = XLIB_COMMON.Interface.AlertMessageType.success;
                alert.Message = "Synchronizacja zakończona sukcesem";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + ". " + e.InnerException.Message);
                alert.MessageType = XLIB_COMMON.Interface.AlertMessageType.danger;
                alert.Message = "Synchronizacja nie powiodła się.";
            }

            return alert;
        }
    }
}
