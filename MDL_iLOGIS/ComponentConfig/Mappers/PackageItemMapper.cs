using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentConfig.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_iLOGIS.ComponentConfig.Mappers
{
    public static class PackageItemMapper
    {
        public static List<PackageItemViewModel> ToList<T>(this IQueryable<PackageItem> source)
        {
            IQueryable<PackageItemViewModel> q = source.Select(x => new PackageItemViewModel()
            {
                Id = x.Id,
                PackageId = x.PackageId,
                PackageName = x.Package.Code + " - " + x.Package.Name + " (D:" + x.Package.Depth + ", W:" + x.Package.Width + ", H:" + x.Package.Height + ")",
                ItemWMSId = x.ItemWMS.Id,
                ItemCode = x.ItemWMS.Item.Code,
                ItemName = x.ItemWMS.Item.Name,
                WarehouseId = x.WarehouseId,
                WarehouseName = x.Warehouse != null ? x.Warehouse.Name : "",
                WarehouseLocationTypeId = x.WarehouseLocationTypeId,
                WarehouseLocationTypeName = x.WarehouseLocationType != null ? x.WarehouseLocationType.Name : "",
                PickingStrategy = x.PickingStrategy,
                QtyPerPackage = x.QtyPerPackage,
                PackagesPerPallet = x.PackagesPerPallet,
                UnitOfMeasure = x.ItemWMS.Item.UnitOfMeasure,
                PalletD = x.PalletD,
                PalletW = x.PalletW,
                PalletH = x.PalletH,
                WeightGross = x.WeightGross,
                WeightNet = x.ItemWMS != null && x.Package != null ? x.Package.Weight + x.ItemWMS.Weight * x.QtyPerPackage : 0,
            });

            //list.ForEach(x => { x. })
            return q.ToList();
        }

        public static PackageItemViewModel FirstOrDefault<T>(this PackageItem source)
        {
            return Map(source);
        }

        public static PackageItemViewModel CastToViewModel<T>(this PackageItem source)
        {
            return Map(source);
        }

        public static PackageItemViewModel Map(PackageItem x)
        {
            if (x == null) return null;

            PackageItemViewModel vm = new PackageItemViewModel()
            {
                Id = x.Id,
                PackageId = x.PackageId,
                PackageName = x.Package.Code + " - " + x.Package.Name + " (D:" + x.Package.Depth + ", W:" + x.Package.Width + ", H:" + x.Package.Height + ")",
                ItemWMSId = x.ItemWMS.Id,
                ItemCode = x.ItemWMS.Item.Code,
                ItemName = x.ItemWMS.Item.Name,
                WarehouseId = x.WarehouseId,
                WarehouseName = x.Warehouse != null ? x.Warehouse.Name : "",
                WarehouseLocationTypeId = x.WarehouseLocationTypeId,
                WarehouseLocationTypeName = x.WarehouseLocationType != null ? x.WarehouseLocationType.Name : "",
                PickingStrategy = x.PickingStrategy,
                QtyPerPackage = x.QtyPerPackage,
                PackagesPerPallet = x.PackagesPerPallet,
                UnitOfMeasure = x.ItemWMS.Item.UnitOfMeasure,
                PalletD = x.PalletD,
                PalletW = x.PalletW,
                PalletH = x.PalletH,
                WeightGross = x.WeightGross,
                WeightNet = x.WeightNet
            };

            return vm;
        }

    }
}
