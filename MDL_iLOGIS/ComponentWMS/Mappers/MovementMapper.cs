using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_iLOGIS.ComponentWMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_iLOGIS.ComponentWMS.Mappers
{
    public static class MovementMapper
    {
        public static List<MovementHistoryViewModel> ToList<T>(this IEnumerable<Movement> source)
        {
            return source.Select(x => Map(x)).ToList();
        }

        public static MovementHistoryViewModel FirstOrDefault<T>(this Movement source)
        {
            return Map(source);
        }

        public static MovementHistoryViewModel Map(Movement source)
        {
            MovementHistoryViewModel vm;

            if (source == null)
            {
                vm = new MovementHistoryViewModel();
            }
            else
            {
                vm = new MovementHistoryViewModel()
                {
                    Id = source.Id,
                    ItemCode = source.ItemWMS.Item.Code,
                    ItemName = source.ItemWMS.Item.Name,
                    SourceAccountingWarehouseCode = source.SourceWarehouse.AccountingWarehouse == null ? source.SourceWarehouse.Code : source.SourceWarehouse.AccountingWarehouse.Code,
                    SourceAccountingWarehouseName = source.SourceWarehouse.AccountingWarehouse == null ? source.SourceWarehouse.Name : source.SourceWarehouse.AccountingWarehouse.Name,
                    SourceLocationName = source.SourceLocation.Name,
                    SourceWarehouseName = source.SourceWarehouse.Name,
                    SourceStockUnitSerialNumber = source.SourceStockUnitSerialNumber,
                    DestinationAccountingWarehouseCode = source.DestinationWarehouse.AccountingWarehouse == null ? source.DestinationWarehouse.Code : source.DestinationWarehouse.AccountingWarehouse.Code,
                    DestinationAccountingWarehouseName = source.DestinationWarehouse.AccountingWarehouse == null ? source.DestinationWarehouse.Name : source.DestinationWarehouse.AccountingWarehouse.Name,
                    DestinationLocationName = source.DestinationLocation.Name,
                    DestinationWarehouseName = source.DestinationWarehouse.Name,
                    DestinationStockUnitSerialNumber = source.DestinationStockUnitSerialNumber,
                    QtyMoved = source.QtyMoved,
                    UnitOfMeasure = source.UnitOfMeasure,
                    MovementType = (int)source.Type,
                    UserName = source.User != null? source.User.UserName : source.ExternalUserName,
                    MovementDate = source.Date.ToString("yyyy-MM-dd HH:mm")
                };
            }
            return vm;
        }
    }
}
