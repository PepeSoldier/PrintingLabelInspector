using MDL_ONEPROD.Common;
using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDL_ONEPROD.Manager
{
    //public interface ITaskManager
    //{
    //    int AreaId { get; set; }
    //    int AreaStageNo { get; set; }
    //    bool EndCalc { get; set; }

    //    IAtcsCalculation Atcs { get; }

    //    //List<Task> TaskToBeScheduledTotal { get; set; }
    //    List<Task> TaskToBeScheduled { get; set; }
    //    List<Task> TaskScheduled { get; set; }

    //    ConcurrentQueue<Task> TaskToBeSaved { get; set; }
        
    //    //-----------------------------------functions

    //    void Init();

    //    void ClearLastSchedule();
    //    void LoadLastSchedule();
    //    void LoadTaskToBeScheduled();
    //    void ScheduleTask(int machineId, int taskNumber, DateTime t);
    //    void ScheduleTaskLineClose(int machineId, int processingTime, DateTime time);

    //    void SaveScheduledTasks();

    //    void ConsiderStock();
    //    List<Task> MakeBatching();

    //    Task GetLastScheduledTask(int machineId);

    //    void ComputeReleaseDatesBasingOnPrevArea(bool considerCategory = true);
    //    void ComputeDueDatesBasingOnNextArea(bool considerCategory = true);
    //}
}
