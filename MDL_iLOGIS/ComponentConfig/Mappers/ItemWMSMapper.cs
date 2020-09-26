using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentConfig.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_iLOGIS.ComponentConfig.Mappers
{
    public static class ItemWMSRepoEnumerable
    {
        public static List<ItemWMSGridViewModel> ToList<T>(this IEnumerable<ItemWMS> source)
        {
            return source.Select(x => new ItemWMSGridViewModel()
            {
                Id = x.Id,
                Code = x.Item.Code,
                Name = x.Item.Name,
                ItemGroupId = x.Item.ItemGroup != null ? x.Item.ItemGroup.Id : 0,
                ItemGroupName = x.Item.ItemGroup != null ? x.Item.ItemGroup.Name : string.Empty,
                DEF = x.Item.DEF,
                BC = x.Item.BC,
                H = x.H,
                UnitOfMeasure = x.Item.UnitOfMeasure,
                PREFIX = x.Item.PREFIX,
                PickerNo = x.PickerNo,
                TrainNo = x.TrainNo,
                StartDateTmp = x.Item.StartDate,
                Type = x.Item.Type,
                Weight = x.Weight
            }).ToList();
        }
    }
}
