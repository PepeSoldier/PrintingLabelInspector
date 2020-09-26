using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentConfig.ViewModels;
using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_iLOGIS.ComponentWMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLIB_COMMON.Model;

namespace MDL_iLOGIS.ComponentWMS.Mappers
{
    public static class DeliveryItemMapper
    {
        public static List<DeliveryItemViewModel> ToList<T>(this IQueryable<DeliveryItem> source)
        {
            IQueryable<DeliveryItemViewModel> q = source.Select(x => new DeliveryItemViewModel()
            {
                Id = x.Id,
                PackageItemId = x.PackageItemId??0,
                DeliveryId = x.DeliveryId,
                DeliveryGroupGuid = x.Delivery.Guid,
                SupplierId = x.Delivery.SupplierId,
                ItemWMSId = x.ItemWMSId,
                ItemCode = x.ItemWMS.Item.Code,
                ItemName = x.ItemWMS.Item.Name,
                AdminEntry = x.AdminEntry,
                OperatorEntry = x.OperatorEntry,
                QtyInPackage = x.QtyInPackage,
                UnitOfMeasure = x.UnitOfMeasure,
                NumberOfPackages = x.NumberOfPackages,
                TotalQty = x.QtyInPackage * x.NumberOfPackages,
                TotalQtyDocument = x.AdminEntry? x.QtyInPackage * x.NumberOfPackages : 0,
                TotalQtyFound = x.OperatorEntry? x.QtyInPackage * x.NumberOfPackages : 0,
                WasPrinted = x.WasPrinted,
                IsLocationAssigned = x.IsLocationAssigned,
                IsLocated = x.IsLocated,
                TotalLocatedQty = x.TotalLocatedQty,
                Deleted = x.Deleted
            });

            return q.ToList();
        }

        public static List<DeliveryItemViewModel> ToList<T>(this List<DeliveryItem> source)
        {
            return source.Select(x => Map(x)).ToList();
        }

        public static DeliveryItemViewModel FirstOrDefault<T>(this DeliveryItem source)
        {
            return Map(source);
        }

        public static DeliveryItemViewModel CastToViewModel<T>(this DeliveryItem source)
        {
            return Map(source);
        }

        public static DeliveryItemViewModel Map(DeliveryItem x)
        {
            DeliveryItemViewModel vm = new DeliveryItemViewModel()
            {
                Id = x.Id,
                PackageItemId = x.PackageItemId??0,
                DeliveryId = x.DeliveryId,
                DeliveryGroupGuid = x.Delivery.Guid,
                SupplierId = x.Delivery.SupplierId,
                ItemWMSId = x.ItemWMSId,
                ItemCode = x.ItemWMS.Item.Code,
                ItemName = x.ItemWMS.Item.Name,
                AdminEntry = x.AdminEntry,
                OperatorEntry = x.OperatorEntry,
                QtyInPackage = x.QtyInPackage,
                UnitOfMeasure = x.UnitOfMeasure,
                NumberOfPackages = x.NumberOfPackages,
                TotalQty = x.TotalQty,
                TotalQtyDocument = x.AdminEntry ? x.TotalQty : 0,
                TotalQtyFound = x.OperatorEntry ? x.TotalQty : 0,
                WasPrinted = x.WasPrinted,
                IsLocationAssigned = x.IsLocationAssigned,
                IsLocated = x.IsLocated,
                TotalLocatedQty = x.TotalLocatedQty,
                Deleted = x.Deleted,
            };

            return vm;
        }

    }
}
