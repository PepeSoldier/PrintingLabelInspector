using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentConfig.ViewModels;
using MDL_iLOGIS.ComponentWHDOC.Entities;
using MDL_iLOGIS.ComponentWHDOC.ViewModels;
using MDL_iLOGIS.ComponentWMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLIB_COMMON.Model;

namespace MDL_iLOGIS.ComponentWMS.Mappers
{
    public static class WhDocumentItemMapper
    {
        public static List<WhDocumentItemViewModel> ToList<T>(this IQueryable<WhDocumentItem> source)
        {

            IQueryable<WhDocumentItemViewModel> q = source.Select(x => new WhDocumentItemViewModel()
            {
                Id = x.Id,
                ItemWMSId = x.ItemWMSId,
                PackageId = x.PackageId,
                WhDocumentId = x.WhDocumentId,
                ItemCode = x.ItemCode,
                ItemName = x.ItemName,
                DisposedQty = x.DisposedQty,
                IssuedQty = x.IssuedQty,
                UnitPrice = x.UnitPrice,
                Value = x.IssuedQty * x.UnitPrice,
                UnitOfMeasure = x.UnitOfMeasure,
                Deleted = x.Deleted,
                
            }); ;

            return q.ToList();
        }
    }
}
