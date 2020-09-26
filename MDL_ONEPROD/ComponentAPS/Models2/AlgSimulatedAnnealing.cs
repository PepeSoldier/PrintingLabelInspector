using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Manager;
using MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.Common
{
    public class AlgSimulatedAnnealing : IAlgorithm
    {
        #region PrivateFields
        UnitOfWorkOneprodAPS uow;
        ResourceOP area;
        MachineManager machineMgr;
        TaskManager taskManager;
        //CalendarManager calendarMgr;
        //BufferManager bufferMgr;
        //AtcsParam atcsParam;
        SetupManager setupManager;
        AreaManager areaManager;
        TaskGapManager taskGapMgr; 
        #endregion

        #region PublicProperties
        public IDbContextOneprod Db { get; set; }
        public ResourceOP ResourceGroup { get; set; }
        public bool ConsiderCalendar { get; set; }
        public bool BatchTasks { get; set; }
        public string Guid { get; set; }
        public DateTime CalculationDateStart { get; set; }
        public DateTime CalculationDateEnd { get; set; }
        public TaskManager TaskManager { get { return taskManager; } }
        public MachineManager MachineManager { get { return machineMgr; } }
        public SetupManager SetupManager { get { return setupManager; } }
        public CalculationManager CalcManager { get; set; }
        #endregion

        public AlgSimulatedAnnealing(IDbContextOneprodAPS db)
        {
            uow = new UnitOfWorkOneprodAPS(db);
            areaManager = new AreaManager(uow);
            area = uow.ResourceGroupRepo.GetByStageNo(3);
            //calendarMgr = new CalendarManager(area.Id, area.StageNo, uow);
            machineMgr = new MachineManager(area.Id, true, uow);
            //bufferMgr = new BufferManager(uow);
            //setupManager = new SetupManager(new ToolManager("test", uow), uow);
            setupManager = new SetupManager(uow);
            taskManager = new TaskManager(this, area, uow);
            //atcsParam = new AtcsParam(this);
            taskGapMgr = new TaskGapManager(taskManager, uow);
        }
        
        public TaskGap AnalizeTask(int taskId)
        {
            string response = string.Empty;
            Workorder task = uow.WorkorderRepo.GetByIdAsNoTracking(taskId);
            DateTime dateSearchFrom = task.DueDate.AddHours(-8);
            DateTime dateSearchTo = task.DueDate;
            List<MCycleTime> cycleTimes = machineMgr.GetCycleTimes((int)task.Item.ItemGroupId);

            taskGapMgr.ClearGaps();

            foreach (MCycleTime ct in cycleTimes)
            {
                response += ct.Machine.Name + ", ";
                FindPossiblePlacesForTask(task, ct, dateSearchFrom, dateSearchTo);
            }

            return FindBestPlaceForTask(task, taskGapMgr.TaskGaps, dateSearchFrom, dateSearchTo);
        }
        private void FindPossiblePlacesForTask(Workorder task, MCycleTime ct, DateTime dateFrom, DateTime dateTo)
        {
            int processingTime = machineMgr.GetRealProcessingTime(ct.Machine, task.Item, task.Qty_Total);

            taskGapMgr.FindGaps(ct.MachineId, dateFrom, dateTo, task.Id, processingTime);
            
            //foreach (TaskGap gap in taskGapMgr.TaskGaps)
            //{
            //    gap.TaskId = task.Id;
            //    gap.TaskProcessingTime = processingTime;
            //}
        }
        private TaskGap FindBestPlaceForTask(Workorder task, List<TaskGap> gaps, DateTime dateFrom, DateTime dateTo)
        {
            DateTime optimalEndDate = task.DueDate.AddMinutes(-area.SafetyTime);

            gaps.RemoveAll(x => x.TotalSeconds < x.TaskProcessingTime);
            
            foreach (TaskGap gap in gaps)
            {

                gap.TaskBestEndDate = optimalEndDate;
                //optymalna data jest za gapem
                gap.TaskBestEndDate = (gap.TaskBestEndDate > gap.EndTime) ? gap.EndTime : gap.TaskBestEndDate;
                //optymalna data jest przed gapem
                gap.TaskBestEndDate = (gap.TaskBestEndDate < gap.StartTime) ? gap.StartTime.AddSeconds(gap.TaskProcessingTime) : gap.TaskBestEndDate;
                //optymalna data powoduje, ze task nie zacznie lub nie zakonczy sie w przedziale gapu
                gap.TaskBestEndDate = (gap.TaskBestEndDate.AddSeconds(-gap.TaskProcessingTime) < gap.StartTime) ? gap.StartTime.AddSeconds(gap.TaskProcessingTime) : gap.TaskBestEndDate;
                //optimalEndDate= (optimalEndDate.AddSeconds(taskGap1.TaskProcessingTime) > taskGap1.EndTime) ? taskGap1.EndTime : optimalEndDate;

                gap.index = Math.Abs((optimalEndDate - gap.TaskBestEndDate).TotalSeconds);                
            }

            TaskGap taskGap1 = gaps.Where(x => x.index >= 0).OrderBy(x => x.index).ThenByDescending(x => x.TotalSeconds).FirstOrDefault();

            if (taskGap1 != null)
            {
                taskGap1.StartTimeStr = taskGap1.StartTime.ToString();
                taskGap1.EndTimeStr = taskGap1.EndTime.ToString();
                ////optymalna data jest za gapem
                //optimalEndDate = (optimalEndDate > taskGap1.EndTime) ? taskGap1.EndTime : optimalEndDate;
                ////optymalna data jest przed gapem
                //optimalEndDate = (optimalEndDate < taskGap1.StartTime) ? taskGap1.StartTime.AddSeconds(taskGap1.TaskProcessingTime) : optimalEndDate;
                ////optymalna data powoduje, ze task nie zacznie lub nie zakonczy sie w przedziale gapu
                //optimalEndDate = (optimalEndDate.AddSeconds(-taskGap1.TaskProcessingTime) < taskGap1.StartTime) ? taskGap1.StartTime.AddSeconds(taskGap1.TaskProcessingTime) : optimalEndDate;
                ////optimalEndDate= (optimalEndDate.AddSeconds(taskGap1.TaskProcessingTime) > taskGap1.EndTime) ? taskGap1.EndTime : optimalEndDate;

                //taskGap1.TaskBestEndDate = optimalEndDate;
            }
            
            return taskGap1;
        }



    }
}