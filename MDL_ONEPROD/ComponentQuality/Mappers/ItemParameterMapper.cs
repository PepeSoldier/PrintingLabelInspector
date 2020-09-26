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
    public static class ItemParameterMapper
    {
        public static List<ItemParameterViewModel> ToList<T>(this IQueryable<ItemParameter> source)
        {
            IQueryable<ItemParameterViewModel> q = source.Select(x => new ItemParameterViewModel()
            {
                Id = x.Id,
                ItemOPId = x.ItemOP.Id,
                ItemCode = x.ItemOP.Code,
                ItemName = x.ItemOP.Name,
                Length = x.Length,
                Width = x.Width,
                Depth = x.Depth,
                Length_Tolerance = x.Length_Tolerance,
                Width_Tolerance = x.Width_Tolerance,
                Depth_Tolerance = x.Depth_Tolerance,
                Weight = x.Weight,
                Color = x.Color,
                ProgramNumber = x.ProgramNumber,
                ProgramName = x.ProgramName,
                Deleted = x.Deleted,
            });

            return q.ToList();
        }

        public static ItemParameterViewModel Map(ItemParameter x)
        {
            if (x != null)
            {
                ItemParameterViewModel vm = new ItemParameterViewModel()
                {
                    Id = x.Id,
                    ItemOPId = x.ItemOP.Id,
                    ItemCode = x.ItemOP.Code,
                    ItemName = x.ItemOP.Name,
                    Length = x.Length,
                    Width = x.Width,
                    Depth = x.Depth,
                    Length_Tolerance = x.Length_Tolerance,
                    Width_Tolerance = x.Width_Tolerance,
                    Depth_Tolerance = x.Depth_Tolerance,
                    Weight = x.Weight,
                    Color = x.Color,
                    ProgramNumber = x.ProgramNumber,
                    ProgramName = x.ProgramName,
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
