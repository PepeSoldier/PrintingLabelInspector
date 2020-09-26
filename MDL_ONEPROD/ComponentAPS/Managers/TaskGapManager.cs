using MDL_ONEPROD.Common;
using MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.Manager
{
    public class TaskGapManager
    {
        UnitOfWorkOneprod uow;
        TaskManager taskManager;
        private List<TaskGap> taskGaps { get; set; }
        public List<TaskGap> TaskGaps { get { return taskGaps; } }

        public TaskGapManager(TaskManager taskManager, UnitOfWorkOneprod unitOfWork)
        {
            this.taskManager = taskManager;
            this.uow = unitOfWork;
            
            taskGaps = new List<TaskGap>();
        }

        public void ClearGaps()
        {
            taskGaps = new List<TaskGap>();
        }
        public List<TaskGap> FindGaps(int machineId, DateTime dtFrom, DateTime dtTo, int taskId, int taskProcessingTime)
        {
            DateTime dateCountFrom = dtFrom;
            List<Workorder> machineTasks = uow.WorkorderRepo.GetWorkorderOfResource_LimitedStatusProd(machineId, dtFrom, dtTo).ToList();
            machineTasks.RemoveAll(x=>x.Id == taskId);          
            
            if (machineTasks.Count > 0)
            {
                for (int i = 0; i < machineTasks.Count; i++)
                {
                    if(!(i - 1 >= 0))
                    {   //First Task
                        AddGap(machineId, dateCountFrom, machineTasks[i].StartTime, taskId, taskProcessingTime);
                    }
                    else
                    {   //Others Tasks
                        AddGap(machineId, machineTasks[i - 1].EndTime, machineTasks[i].StartTime, taskId, taskProcessingTime);
                    }

                    if(i == machineTasks.Count - 1)
                    {   //Last Task
                        AddGap(machineId, machineTasks[i].EndTime, dtTo, taskId, taskProcessingTime);
                    }
                }
            }
            else
            {
                AddGap(machineId, dtFrom, dtTo, taskId, taskProcessingTime);
            }

            return taskGaps;
        }

        private void AddGap(int machineId, DateTime dtStartTime, DateTime dtEndTime, int taskId, int taskProcessingTime)
        {
            if (dtEndTime > dtStartTime)
            {
                taskGaps.Add(new TaskGap
                {
                    StartTime = dtStartTime,
                    EndTime = dtEndTime,
                    TotalSeconds = (int)(dtEndTime - dtStartTime).TotalSeconds,
                    MachineId = machineId,
                    TaskId = taskId,
                    TaskProcessingTime = taskProcessingTime
                });
            }
        }
    }
}