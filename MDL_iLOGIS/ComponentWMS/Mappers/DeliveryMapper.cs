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
    public static class DeliveryMapper
    {
        public static List<DeliveryViewModel> ToList<T>(this IQueryable<Delivery> source)
        {
            IQueryable<DeliveryViewModel> q = source.Select(x => new DeliveryViewModel()
            {
                Id = x.Id,
                DocumentDate = x.DocumentDate,
                DocumentNumber = x.DocumentNumber,
                StampTime = x.StampTime,
                SupplierId = x.SupplierId,
                SupplierName = x.Supplier.Name,
                SupplierCode = x.Supplier.Code,
                Status = x.EnumDeliveryStatus,
                Guid = x.Guid,
                ItemsCount = x.DeliveryItems.DefaultIfEmpty().Sum(s => s != null? s.QtyInPackage * s.NumberOfPackages : 0)
            });

            return q.ToList();
        }

        public static DeliveryViewModel FirstOrDefault<T>(this Delivery source)
        {
            return Map(source);
        }

        public static DeliveryViewModel CastToViewModel<T>(this Delivery source)
        {
            return Map(source);
        }

        public static DeliveryViewModel Map(Delivery x)
        {
            if (x != null)
            {
                DeliveryViewModel vm = new DeliveryViewModel()
                {
                    Id = x.Id,
                    DocumentDate = x.DocumentDate,
                    DocumentNumber = x.DocumentNumber,
                    StampTime = x.StampTime,
                    SupplierId = x.SupplierId,
                    SupplierName = x.Supplier.Name,
                    SupplierCode = x.Supplier.Code,
                    Status = x.EnumDeliveryStatus,
                    Guid = x.Guid,
                    ItemsCount = x.DeliveryItems != null? x.DeliveryItems.DefaultIfEmpty().Sum(s => s != null ? s.QtyInPackage * s.NumberOfPackages : 0) : 0
                };

                return vm;
            }
            else
            {
                return new DeliveryViewModel();
            }
        }

    }
}
