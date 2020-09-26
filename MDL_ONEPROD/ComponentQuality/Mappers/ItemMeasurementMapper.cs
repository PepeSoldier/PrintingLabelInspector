using MDL_ONEPROD.ComponentQuality.Entities;
using MDL_ONEPROD.ComponentQuality.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLIB_COMMON.Model;

namespace MDL_iLOGIS.ComponentWMS.Mappers
{
    public static class ItemMeasurementMapper
    {
        public static List<ItemMeasurementViewModel> ToList<T>(this IQueryable<ItemMeasurement> source)
        {
            IQueryable<ItemMeasurementViewModel> q = source.Select(x => new ItemMeasurementViewModel()
            {
                Id = x.Id,
                ItemOPId = x.ItemOP.Id,
                ItemCode = x.ItemOP.Code,
                ItemName = x.ItemOP.Name,
                SerialNumber = x.SerialNumber,
                Counter = x.Counter,
                Result0 = x.Result0,
                Result1 = x.Result1,
                Result2 = x.Result2,
                Result3 = x.Result3,
                Result4 = x.Result4,
                Result5 = x.Result5,
                Result6 = x.Result6,
                Result7 = x.Result7,
                Result8 = x.Result8,
                Result9 = x.Result9,
                Deleted = x.Deleted,
            });

            return q.ToList();
        }

        public static ItemMeasurementViewModel Map(ItemMeasurement x)
        {
            if (x != null)
            {
                ItemMeasurementViewModel vm = new ItemMeasurementViewModel()
                {
                    Id = x.Id,
                    ItemOPId = x.ItemOP.Id,
                    ItemCode = x.ItemOP.Code,
                    ItemName = x.ItemOP.Name,
                    SerialNumber = x.SerialNumber,
                    Counter = x.Counter,
                    Result0 = x.Result0,
                    Result1 = x.Result1,
                    Result2 = x.Result2,
                    Result3 = x.Result3,
                    Result4 = x.Result4,
                    Result5 = x.Result5,
                    Result6 = x.Result6,
                    Result7 = x.Result7,
                    Result8 = x.Result8,
                    Result9 = x.Result9,
                    Deleted = x.Deleted,
                };

                return vm;
            }
            else
            {
                return null;
            }
        }
    }
}
