using System;
using System.Linq;
using System.Collections.Generic;
using MDL_ONEPROD.ComponentMes.Etities.MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.ComponentMes.ViewModels;

namespace MDL_iLOGIS.ComponentConfig.Mappers
{
    public static class WorkplaceBufferEnumerable
    {
        public static List<WorkplaceBufferViewModel> ToList<T>(this IEnumerable<WorkplaceBuffer> source)
        {
            return source.Select(x => Map(x)).ToList();
        }

        public static WorkplaceBufferViewModel FirstOrDefault<T>(this WorkplaceBuffer source)
        {
            return Map(source);
        }

        public static WorkplaceBufferViewModel Map(WorkplaceBuffer x)
        {
            WorkplaceBufferViewModel vm = new WorkplaceBufferViewModel()
            {
                Id = x.Id,
                Barcode = x.Barcode,
                SerialNumber = x.SerialNumber,
                ChildCode = x.Child.Code,
                ChildItemId = x.Child.Id,
                ChildName = x.Child.Name,
                ParentItemId = x.Parent.Id,
                ProcessId = x.ProcessId,
                QtyAvailable = x.QtyAvailable,
                QtyInBom = x.QtyInBom,
                QtyRequested = x.ParentWorkorder.Qty_Remain * x.QtyInBom,
                TimeLoaded = x.TimeLoaded.ToString("yyyy-MM-dd HH:mm:ss"),
                WorkorderId = x.ParentWorkorderId,
                WorkorderNumber = x.ParentWorkorder.ClientOrder.OrderNo
            };

            return vm;
        }
    }
}
