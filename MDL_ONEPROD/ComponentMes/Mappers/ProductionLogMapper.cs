using System;
using System.Linq;
using System.Collections.Generic;
using MDL_ONEPROD.ComponentMes.ViewModels;
using MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.ComponentMes.Models;

namespace MDL_iLOGIS.ComponentConfig.Mappers
{
    public static class ProductionLogMapper
    {
        public static List<ProductionLogViewModel> ToList<T>(this IEnumerable<ProductionLog> source)
        {
            return source.Select(x => Map(x)).ToList();
        }

        public static ProductionLogViewModel FirstOrDefault<T>(this ProductionLog source)
        {
            return Map(source);
        }

        public static ProductionLogViewModel Map(ProductionLog pl)
        {
            ProductionLogViewModel vm = new ProductionLogViewModel()
            {
                Id = pl.Id,
                ItemCode = pl.Item != null ? pl.Item.Code : pl.ItemCode,
                ItemName = pl.Item != null ? pl.Item.Name : "",
                ClientWorkOrderNumber = pl.ClientWorkOrderNumber,
                WorkplaceId = pl.WorkplaceId,
                WorkplaceName = pl.Workplace != null ? pl.Workplace.Name : "",
                ResourceId = pl.Workplace != null ? pl.Workplace.MachineId : 0,
                ResourceName = pl.Workplace != null ? pl.Workplace.Machine.Name : "",
                WorkorderTotalQty = pl.WorkorderTotalQty,
                DeclaredQty = pl.DeclaredQty,
                CostCenter = pl.CostCenter,
                SerialNo = pl.SerialNo,
                TimeStamp = pl.TimeStamp,
                UserName = pl.UserName,
                ReasonTypeId = pl.ReasonTypeId,
                ReasonTypeName = pl.ReasonType != null ? pl.ReasonType.Name : "",
                ReasonId = pl.ReasonId,
                ReasonName = pl.Reason != null ? pl.Reason.Name : "",
                InternalWorkOrderNumber = pl.InternalWorkOrderNumber,
                UsedQty = pl.UsedQty,
                TransferNumber = pl.TransferNumber,
                QtyAvailable = pl.QtyAvailable
            };

            return vm;
        }
    }

    public static class ProductionLogToTreantJsNodeMapper
    {
        public static List<TreantJsNode> ToListTreant<T>(this IEnumerable<ProductionLog> source) where T: TreantJsNode
        {
            return source.Select(x => Map(x)).ToList();
        }

        public static TreantJsNode FirstOrDefault<T>(this ProductionLog source)
        {
            return Map(source);
        }

        public static TreantJsNode Map(ProductionLog pl)
        {
            TreantJsNode vm = new TreantJsNode()
            {
                text = new TreantJsNodeText()
                {
                    name = pl.Item != null ? pl.Item.Code : "Kod ?",
                    desc = pl.Item != null ? pl.Item.Name : "Nawa ?",
                    title = "",
                    datetime = pl.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss"),
                    serialNumber = pl.SerialNo,
                    prodLogId = pl.Id.ToString(),
                    declaredQty = pl.DeclaredQty.ToString(),
                    workOrderTotalQty = pl.WorkorderTotalQty.ToString(),
                    //picture = "<i class='far fa-image'></i>"
                },
                image = "<i class='far fa-image'></i>"
            };

            return vm;
        }
    }
}
