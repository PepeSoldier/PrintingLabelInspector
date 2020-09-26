using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentConfig.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLIB_COMMON.Model;

namespace MDL_iLOGIS.ComponentConfig.Mappers
{
    public static class WarehouseLocationMapper
    {
        public static List<WarehouseLocationViewModel> ToList<T>(this IQueryable<WarehouseLocation> source)
        {
            IQueryable<WarehouseLocationViewModel> q = source.Select(x => new WarehouseLocationViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                //NameFormatted = x.NameFormatted,
                QtyOfSubLocations = x.QtyOfSubLocations,
                WarehouseId = x.WarehouseId,
                WarehouseName = x.Warehouse != null ? x.Warehouse.Name : "",
                WarehouseCode = x.Warehouse != null ? x.Warehouse.Code : "",
                TypeId = x.TypeId,
                TypeName = x.Type != null ? x.Type.Name : "",
                TypeDisplayFormat = x.Type.DisplayFormat,
                Utilization = x.Utilization,
                ParentLocationId = x.ParentWarehouseLocationId,
                ParentLocationName = x.ParentWarehouseLocationId != null ? x.ParentWarehouseLocation.Name : "",
                ParentWarehouseName = x.ParentWarehouseLocation != null && x.ParentWarehouseLocation.Warehouse != null ? x.ParentWarehouseLocation.Warehouse.Name : ""
            });

            List<WarehouseLocationViewModel> whlocListVM =  q.ToList();

            foreach(WarehouseLocationViewModel whlocVM in whlocListVM)
            {
                whlocVM.NameFormatted = StringFormatter.Format(whlocVM.TypeDisplayFormat, whlocVM.Name);
            }

            return whlocListVM;
        }

        public static WarehouseLocationViewModel FirstOrDefault<T>(this WarehouseLocation source)
        {
            return Map(source);
        }

        public static WarehouseLocationViewModel CastToViewModel<T>(this WarehouseLocation source)
        {
            return Map(source);
        }

        public static WarehouseLocationViewModel Map(WarehouseLocation x)
        {
            WarehouseLocationViewModel vm = new WarehouseLocationViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                NameFormatted = x.NameFormatted,
                QtyOfSubLocations = x.QtyOfSubLocations,
                WarehouseId = x.WarehouseId,
                WarehouseName = x.Warehouse != null ? x.Warehouse.Name : "",
                WarehouseCode = x.Warehouse != null ? x.Warehouse.Code : "",
                TypeId = x.TypeId,
                TypeName = x.Type != null ? x.Type.Name : "",
                Utilization = x.Utilization,
                ParentLocationId = x.ParentWarehouseLocationId,
                ParentLocationName = x.ParentWarehouseLocationId != null ? x.ParentWarehouseLocation.Name : "",
                ParentWarehouseName = x.ParentWarehouseLocation != null && x.ParentWarehouseLocation.Warehouse != null ? x.ParentWarehouseLocation.Warehouse.Name : ""
            };

            return vm;
        }

    }
}
