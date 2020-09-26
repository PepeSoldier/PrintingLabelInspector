using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Manager;
using MDL_ONEPROD.Model.Scheduling;
using System;

namespace MDL_ONEPROD.Common
{
    public interface IAlgorithm
    {
        //DateTime CalculationDateStart { get; set; }
        //DateTime CalculationDateEnd { get; set; }
        //bool ConsiderCalendar { get; set; }
        //bool BatchTasks { get; set; }
        //string Guid { get; set; }
        //IDbContextONEPROD Db { get; set; }

        ResourceOP ResourceGroup { get; set; }
        TaskManager TaskManager { get; }
        MachineManager MachineManager { get; }
        SetupManager SetupManager { get; }
        CalculationManager CalcManager { get; set; }
    }
}
