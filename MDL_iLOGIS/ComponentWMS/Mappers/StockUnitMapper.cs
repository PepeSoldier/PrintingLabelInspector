using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_iLOGIS.ComponentWMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_iLOGIS.ComponentWMS.Mappers
{
    public static class StockUnitMapper
    {
        public static List<StockUnitViewModel> ToList<T>(this IEnumerable<StockUnit> source)
        {
            return source.Select(x => Map(x)).ToList();

            //return source.Select(x => new StockUnitViewModel()
            //{
            //    Id = x.Id,
            //    CurrentQtyinPackage = x.CurrentQtyinPackage,
            //    MaxQtyPerPackage = x.MaxQtyPerPackage,
            //    WarehouseLocationName = x.WarehouseLocation.Name,
            //    WarehouseLocationFormatted = x.WarehouseLocation.NameFormatted,
            //    WarehouseLocationId = x.WarehouseLocationId,
            //    SerialNumber = x.SerialNumber,
            //    ItemWMSId = x.ItemWMSId,
            //    ItemCode = x.ItemWMS.Item.Code,
            //    ReservedQty = x.ReservedQty,
            //    ReservedForPickingListId = x.ReservedForPickingListId,
            //    Status = x.Status,
            //    IsLocated = x.IsLocated,
            //    WarehouseName = x.WarehouseLocation.Warehouse.Name,
            //    WarehouseId = x.WarehouseLocation.WarehouseId,
            //    AccountingWarehouseName = x.WarehouseLocation.Warehouse.AccountingWarehouse.Name,
            //    AccountingWarehouseId = x.WarehouseLocation.Warehouse.AccountingWarehouseId
            //}).ToList();
        }

        public static StockUnitViewModel FirstOrDefault<T>(this StockUnit source)
        {
            return Map(source);
        }

        public static StockUnitViewModel Map(StockUnit source)
        {
            StockUnitViewModel vm;

            if (source == null)
            {
                vm = new StockUnitViewModel();
            }
            else
            {
                vm = new StockUnitViewModel()
                {
                    Id = source.Id,
                    CurrentQtyinPackage = source.CurrentQtyinPackage,
                    MaxQtyPerPackage = source.MaxQtyPerPackage,
                    WarehouseLocationId = source.WarehouseLocationId,
                    SerialNumber = source.SerialNumber,
                    ItemWMSId = source.ItemWMSId,
                    ItemCode = source.ItemWMS.Item.Code,
                    ItemName = source.ItemWMS.Item.Name,
                    ItemGroup = source.ItemWMS.Item.ItemGroup != null? source.ItemWMS.Item.ItemGroup.Name : "",
                    ReservedQty = source.ReservedQty,
                    Status = source.Status,
                    IsLocated = source.IsLocated,
                    UnitOfMeasure = source.UnitOfMeasure,
                };

                if (source.WarehouseLocation != null)
                {
                    vm.WarehouseLocationName = source.WarehouseLocation.Name;
                    vm.WarehouseLocationFormatted = source.WarehouseLocation.NameFormatted;
                    vm.WarehouseLocationUtilization = source.WarehouseLocation.Utilization;

                    if (source.WarehouseLocation.Warehouse != null)
                    {
                        vm.WarehouseCode = source.WarehouseLocation.Warehouse.Code;
                        vm.WarehouseName = source.WarehouseLocation.Warehouse.Name;
                        vm.WarehouseId = source.WarehouseLocation.WarehouseId;

                        if (source.WarehouseLocation.Warehouse.AccountingWarehouse != null)
                        {
                            vm.AccountingWarehouseName = source.WarehouseLocation.Warehouse.AccountingWarehouse.Name;
                            vm.AccountingWarehouseId = source.WarehouseLocation.Warehouse.AccountingWarehouseId;
                        }
                    }
                }
            }

            return vm;
        }
    }
}
