using System;
using System.Linq;
using System.Collections.Generic;
using MDL_ONEPROD.ComponentMes.Etities.MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.ComponentMes.ViewModels;
using MDL_ONEPROD.Model.Scheduling;

namespace MDL_iLOGIS.ComponentConfig.Mappers
{
    public static class WorkplaceEnumerable
    {
        public static List<WorkplaceViewModel> ToList<T>(this IEnumerable<Workplace> source)
        {
            return source.Select(x => Map(x)).ToList();
        }

        public static WorkplaceViewModel FirstOrDefault<T>(this Workplace source)
        {
            return Map(source);
        }

        public static WorkplaceViewModel CastToViewModel<T>(this Workplace source)
        {
            return Map(source);
        }

        public static WorkplaceViewModel Map(Workplace x)
        {
            WorkplaceViewModel vm = new WorkplaceViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                ResourceId = x.Machine.Id,
                ResourceName = x.Machine.Name,
                ResourceGroupId = x.Machine.ResourceGroupId,
                SelectedWorkorderId = x.SelectedTaskId,
                ComputerHostName = x.ComputerHostName,
                PrinterIPv4 = x.PrinterIPv4,
                LoggedUserName = x.LoggedUserName,
                LabelANC = x.LabelANC,
                LabelName = x.LabelName,
                PrintLabel = x.PrintLabel,
                SerialNumberType = x.SerialNumberType,
                PrinterType = x.PrinterType,
                Type = x.Type,
                LabelLayoutNo = x.LabelLayoutNo,
                IsTraceability = x.IsTraceability,
                IsReportOnline = x.IsReportOnline,
                Deleted = x.Deleted
            };

            return vm;
        }
    }
}
