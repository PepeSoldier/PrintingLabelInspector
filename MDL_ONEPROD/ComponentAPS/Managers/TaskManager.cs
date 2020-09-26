

using MDL_ONEPROD.Common;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.Repo;
using MDL_ONEPROD.Repo.Scheduling;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;

namespace MDL_ONEPROD.Manager
{
    public class TaskManager
    {
        private UnitOfWorkOneprod uow;
        private ResourceOP ResourceGroup { get; set; }
        private List<Workorder> TaskToBeScheduledTotal { get; set; }
        private int orderSeq;
        //private int batchNumber;

        private IAlgorithm atcs;
        private BatchManager batcher;

        public bool EndCalc { get; set; }
        public IAlgorithm Atcs { get { return atcs; } }
        public BatchManager Batcher{ get { return batcher; }}
        public List<Workorder> Tasks_ToBeScheduled { get; set; }
        public List<Workorder> Tasks_Scheduled { get; set; }
        public List<Workorder> PlannerOrders { get; set; }
        public ConcurrentQueue<Workorder> Tasks_ToBeSaved { get; set; }

        //---------------------------------------------------------------------CONSTRUCTORS
        public TaskManager(IAlgorithm actsCalc, ResourceOP resourceGroup, UnitOfWorkOneprod unitOfWork)
        {
            ResourceGroup = resourceGroup;
            atcs = actsCalc;
            orderSeq = 1;
            //batchNumber = 1;

            uow = unitOfWork;

            Tasks_ToBeScheduled = new List<Workorder>();
            Tasks_Scheduled = new List<Workorder>();
            Tasks_ToBeSaved = new ConcurrentQueue<Workorder>();
            batcher = new BatchManager(resourceGroup, atcs.CalcManager.Guid, unitOfWork);
        }
        public TaskManager(IDbContextOneprod db, ResourceOP area, UnitOfWorkOneprod unitOfWork)
        {
            atcs = null;
            this.ResourceGroup = area;
            orderSeq = 0;
            
            uow = unitOfWork;
            
            Tasks_Scheduled = new List<Workorder>();
        }
        public TaskManager(UnitOfWorkOneprod unitOfWork) {
            this.uow = unitOfWork;
        }

        //---------------------------------------------------------------------INIT_FUNCTION
        public void Init()
        {
            //1.
            ClearLastSchedule();
            //2.
            LoadTaskToBeScheduled();
            //3.
            //20180821 - uwzględnianie stocku tylko poprzez intentaryzację
            //TaskStockManager tsm = new TaskStockManager(this, this.atcs.CalcManager.Guid, uow);
            //tsm.ConsiderStock(Atcs.CalcManager.CalculationDateStart, Area, Tasks_ToBeScheduled);
            //tsm.MoveStock(Tasks_ToBeScheduled);
            //4.
            //LoadTaskToBeScheduled(); //RELOAD after updates of the stock

            //5.
            if(ResourceGroup.Forward){
                ComputeReleaseDates();
            }
            ComputeDueDates();

            //6.
            if (ResourceGroup.Name == "SAW, SAW-SM")
            {
                List<Workorder> tmp1 = Tasks_ToBeScheduled.Where(x => x.ItemId == 45).ToList();
                List<Workorder> tmp2 = Tasks_ToBeScheduled.Where(x => x.ItemId == 47).ToList();
                List<Workorder> tmp3 = Tasks_ToBeScheduled.Where(x => x.ItemId == 60).ToList();
                List<Workorder> tmp4 = Tasks_ToBeScheduled.Where(x => x.ItemId == 61).ToList();
                List<Workorder> tmp5 = Tasks_ToBeScheduled.Where(x => x.ItemId == 90).ToList();

                Tasks_ToBeScheduled.Clear();
                Tasks_ToBeScheduled.AddRange(batcher.ZipTasksToRawMaterial(tmp1, 2400, 3));
                Tasks_ToBeScheduled.AddRange(batcher.ZipTasksToRawMaterial(tmp2, 2400, 3));
                Tasks_ToBeScheduled.AddRange(batcher.ZipTasksToRawMaterial(tmp3, 2400, 3));
                Tasks_ToBeScheduled.AddRange(batcher.ZipTasksToRawMaterial(tmp4, 2400, 3));
                Tasks_ToBeScheduled.AddRange(batcher.ZipTasksToRawMaterial(tmp5, 2400, 3));
            }
            else if(ResourceGroup.Id == 14)
            {
                Tasks_ToBeScheduled = batcher.ZipTasksToOrderNumber(Tasks_ToBeScheduled);
            }
            else
            {
                Tasks_ToBeScheduled = batcher.ZIPByPart(Tasks_ToBeScheduled, atcs.CalcManager.BatchTasks);
            }   
        }
        public void CheckForGaps()
        {
            TaskGapManager tgm = new TaskGapManager(this, uow);
            tgm.FindGaps(1, new DateTime(2017,5,16), new DateTime(2017, 5, 19), 1, 1);
        }

        //---------------------------------------------------------------------SCHEDULE_TASK
        public void ScheduleTaskLineClose(int machineId, int processingTime, DateTime time)
        {
            //Task tsk = new Task
            //{
            //    UniqueTaskNumber = "CLOSE",
            //    OrderId = null,
            //    MachineId = machineId,
            //    ToolId = null,
            //    OrderSeq = orderSeq,
            //    BatchNumber = batchNumber,
            //    StartTime = time.AddSeconds(-processingTime),
            //    EndTime = time,
            //    ReleaseDate = new DateTime(1900, 1, 1),
            //    DueDate = new DateTime(1900, 1, 1),
            //    ProcessingTime = processingTime,
            //    LV = 0,
            //    Status = TaskScheduleStatus.planned
            //};
            //uow.TaskRepo.Add(tsk);

            atcs.MachineManager.LoadMachine(machineId, processingTime, false);
        }
        public void ScheduleTask(ResourceOP resource, int taskIndex, DateTime t, int emptySeconds, bool forward)
        {
            Workorder task = Tasks_ToBeScheduled[taskIndex];
            task.ResourceId = resource.Id;

            SetupType setupType = ScheduleSetup(resource.Id, taskIndex, t, task.ClientOrderId, emptySeconds, forward);

            if (setupType != SetupType.ToolModyfication && setupType != SetupType.ChangeNotPossible)
            {
                DateTime t2 = atcs.MachineManager.GetCurrentLoad(resource.Id); //check again t after schedule setup
                int totalQty = task.Batches.Qty;
                int processingTime = atcs.MachineManager.GetRealProcessingTime(resource, task.Item, totalQty, true);

                ScheduleTaskZipped(forward, task, t2, totalQty, processingTime);
                Tasks_ToBeScheduled.Remove(task);
                atcs.MachineManager.LoadMachine(resource.Id, processingTime, false);
                //batchNumber++;
            }
            else
            {
                //nie można zaplanować tego zadania ponieważ wymagana jest modyfikacja narzędzia
                //przesuwamy wskaźnik czasu aby algorytm się nie zapętlił
                atcs.MachineManager.LoadMachine(resource.Id, 300, true);
            }

            atcs.SetupManager.SetSetupStatus(resource.Id, task.Item.Id, task.Item.ItemGroupOP.Id, null);
        }

        private void ScheduleTaskZipped(bool forward, Workorder task, DateTime t, int totalQty, int processingTime)
        {
            int taskProcessingTime;
            List<Workorder> unzipedTasks = batcher.UnzipBatches(task);
            foreach (Workorder tsk in unzipedTasks)
            {
                taskProcessingTime = (processingTime * tsk.Qty_Total) / totalQty;
                ScheduleTaskUnzipped(tsk, taskProcessingTime, t, forward);
                t = t.AddSeconds((forward ? taskProcessingTime : -taskProcessingTime));
            }
        }
        private void ScheduleTaskUnzipped(Workorder task, int processingTime, DateTime t, bool forward)
        {
            orderSeq++;

            task.ProcessingTime = processingTime;
            task.OrderSeq = orderSeq;
            task.Status = TaskScheduleStatus.planned;
            task.Status = uow.WorkorderRepo.StatusVerification(task);
            //task.BatchNumber = batchNumber;
            task.StartTime = forward? t : t.AddSeconds(-processingTime);
            task.EndTime = forward? t.AddSeconds(processingTime) : t;
          
            if(Tasks_Scheduled.FirstOrDefault(t1=>t1.UniqueNumber == task.UniqueNumber) != null)
            {
                NotificationManager.Instance.AddNotificationLog("WARNING!! Zadanie było już zaplanowane (unique task number: " + task.UniqueNumber + ")", receiver: atcs.CalcManager.Guid);
            }

            Tasks_Scheduled.Add(task);
            Tasks_ToBeSaved.Enqueue(task);
        }
        private SetupType ScheduleSetup(int machineId, int taskIndex, DateTime t, int? orderId, int emptySeconds, bool forward)
        {
            Workorder lastTask = GetLastScheduledTask(machineId);
            DateTime t2 = t;
            Setup s = atcs.SetupManager.PrepareSetup(Tasks_ToBeScheduled[taskIndex], machineId, true);
                                    
            if (s.Time > 0)
            {
                if (s.Time > 240 || !(s.Type == SetupType.AncChange || s.Type == SetupType.CategoryChange)) //240 - 4minuty
                {
                    orderSeq++;
                    if(lastTask != null)
                        atcs.SetupManager.ScheduleSetup(s, lastTask.StartTime, lastTask.ClientOrderId, machineId, lastTask.BatchNumber, orderSeq, forward);
                    else
                        atcs.SetupManager.ScheduleSetup(s, t, orderId, machineId, 0, orderSeq, forward);
                }

                if (s.Time > emptySeconds) //obciaz maszyne jezeli czas przezbrojenia wiekszy od ilosci wolnych sekund
                {
                    atcs.MachineManager.LoadMachine(machineId, s.Time - emptySeconds, false);
                }
            }

            return s.Type;
        }

        //---------------------------------------------------------------------SAVE_TASK_TO_DB
        public void SaveScheduledTasks(IDbContextOneprodAPS db2)
        {
            int line;
            int lastSavedRow = -1;
            bool noTasks = false;
            Workorder taskDequeued = null;
            //Task taskId = null;

            //nowa instancja jest potrzebna ponieważ ta funkcja działa w osobnym wątku
            WorkorderRepo taskRepo = new WorkorderRepo(db2);

            line = NotificationManager.Instance.AddNotificationBlock("Zapisywanie obliczeń do bazy - " + ResourceGroup.StageNo.ToString() + " (0)", id: -1, receiver: atcs.CalcManager.Guid, status: "");

            while (!EndCalc || !noTasks)
            {
                Tasks_ToBeSaved.TryDequeue(out taskDequeued);

                if (taskDequeued != null)
                {
                    lastSavedRow++;

                    taskRepo.UpdateByFastQuery(taskDequeued);

                    ////uow.TaskRepo.SaveScheduledTask(task1);
                    //taskId = taskRepo.GetById(taskDequeued.Id);

                    //if (taskId != null)
                    //{
                    //    taskId.StartTime = taskDequeued.StartTime;
                    //    taskId.EndTime = taskDequeued.EndTime;
                    //    taskId.MachineId = taskDequeued.MachineId;
                    //    taskId.OrderSeq = taskDequeued.OrderSeq;
                    //    taskId.BatchNumber = taskDequeued.BatchNumber;
                    //    taskId.Status = taskDequeued.Status; //TaskScheduleStatus.planned;
                    //    taskId.ToolId = taskDequeued.ToolId;
                    //    //taskId.Qty_Remain = taskDequeued.Qty_Remain;
                    //    taskId.Qty_Produced = taskDequeued.Qty_Produced;
                    //    taskId.DueDate = taskDequeued.DueDate;
                    //    taskId.ReleaseDate = taskDequeued.ReleaseDate;
                    //    taskId.ProcessingTime = taskDequeued.ProcessingTime;

                    //    taskRepo.Update(taskId);
                    //}

                    noTasks = false;
                    NotificationManager.Instance.AddNotificationBlock("Zapisywanie obliczeń do bazy - " + ResourceGroup.StageNo.ToString() + " (" + lastSavedRow + ")", id: line, receiver: atcs.CalcManager.Guid, status: "");
                }
                else
                {
                    if (EndCalc) noTasks = true;

                    Thread.Sleep(500);
                }
            }

            NotificationManager.Instance.AddNotificationBlock("Zapisywanie obliczeń do bazy - " + ResourceGroup.StageNo.ToString() + " (" + lastSavedRow + ")", id: line, receiver: atcs.CalcManager.Guid, status: "100%");
        }

        //---------------------------------------------------------------------COMPUTE_RELEASE|DUE|DATES
        private void ComputeDueDates()
        {
            Workorder nextStageTask = null;
            List<Workorder> nextStageTasks = new List<Workorder>();
            List<AtcsAlgorithm> atcsClc = atcs.CalcManager.Calculations.Where(d => d.ResourceGroup.StageNo > ResourceGroup.StageNo).ToList();

            if (atcsClc != null)
            {
                foreach(AtcsAlgorithm atcsc in atcsClc)
                {
                    nextStageTasks.AddRange(atcsc.TaskManager.Tasks_Scheduled);
                }
                
                if (nextStageTasks.Count > 0)
                {
                    foreach (Workorder task in Tasks_ToBeScheduled)
                    {
                        nextStageTask = null;
                        nextStageTask = nextStageTasks
                                            .Where(t =>
                                                (t.ClientOrder != null && t.ClientOrder.Id == task.ClientOrder.Id) &&
                                                (t.Item != null && t.Item.ItemGroup != null && t.Item.ItemGroup.Process != null && 
                                                 t.Item.ItemGroupOP.Process.Id == task.Item.ItemGroupOP.Process.ParentId
                                                )
                                             )
                                            .OrderBy(x => x.StartTime)
                                            .Take(1).FirstOrDefault();

                        if (nextStageTask != null && nextStageTask.StartTime.Year > 2000)
                        {
                            task.DueDate = nextStageTask.StartTime.AddSeconds(-15);
                        }
                    }
                }
            }
        }
        //private void ComputeReleaseDatesBasingOnPrevArea_UNUSED(bool considerCategory = true) 
        //{
        //    List<Task> prevStageTasks = new List<Task>();
        //    Task prevStageTask = null;

        //    int stageNo1 = Area.StageNo;
        //    int stageNo2 = stageNo1 - 1;

        //    AtcsAlgorithm atcsClc = atcs.CalcManager.Calculations.FirstOrDefault(d => d.Area.StageNo == stageNo2);
        //    if (atcsClc != null)
        //    {
        //        prevStageTasks = atcsClc.TaskManager.Tasks_Scheduled; //atcs.AtcsList.FirstOrDefault(a => a.Area.StageNo == stageNo2).TaskManager.TaskScheduled;

        //        if (prevStageTasks.Count > 0)
        //        {
        //            foreach (Task task in Tasks_ToBeScheduled)
        //            {
        //                prevStageTask = null;
        //                prevStageTask = prevStageTasks.FirstOrDefault(t => 
        //                                                t.Order.Orderno == task.Order.Orderno 
        //                                                && t.LV == task.LV + 1 
        //                                                && (t.Part.ItemGroup.Process.Id == task.Part.ItemGroup.Process.Id || considerCategory == false));

        //                if (prevStageTask != null)
        //                {
        //                    task.ReleaseDate = prevStageTask.EndTime.AddMinutes(15);
        //                }
        //            }
        //        }
        //    }
        //}
        //private void ComputeDueDatesBasingOnNextArea_UNUSED(bool considerCategory = true) 
        //{
        //    List<Task> nextStageTasks = new List<Task>();
        //    Task nextStageTask = null;

        //    int stageNo1 = Area.StageNo;
        //    int stageNo2 = stageNo1 + 1;
        //    //Area area1 = uow.AreaRepo.GetByStageNo(stageNo2);

        //    AtcsAlgorithm atcsClc = atcs.CalcManager.Calculations.FirstOrDefault(d => d.Area.StageNo == stageNo2);
        //    if (atcsClc != null)
        //    {
        //        nextStageTasks = atcsClc.TaskManager.Tasks_Scheduled; //uow.TaskRepo.GetScheduledTasks(area1);

        //        if (nextStageTasks.Count > 0)
        //        {
        //            foreach (Task task in Tasks_ToBeScheduled)
        //            {   
        //                nextStageTask = null;
        //                nextStageTask = nextStageTasks.FirstOrDefault(
        //                                    t => t.Order.Orderno == task.Order.Orderno 
        //                                    && t.LV == (task.LV-1)
        //                                    && (t.Part.ItemGroup.Process.Id == task.Part.ItemGroup.Process.Id || considerCategory == false));

        //                if (nextStageTask != null)
        //                {
        //                    //TODO: gdzies jest błąd z obliczaniem duedate
        //                    task.DueDate = nextStageTask.StartTime.AddMinutes(-1);
        //                }
        //            }
        //        }
        //    }
        //}
        private void ComputeReleaseDates()
        {
            Workorder prevStageTask = null;
            List<Workorder> prevStageTasks = new List<Workorder>();
            List<AtcsAlgorithm> atcsClc = atcs.CalcManager.Calculations.Where(d => d.ResourceGroup.StageNo < ResourceGroup.StageNo).ToList();

            if (atcsClc != null)
            {
                foreach (AtcsAlgorithm atcsc in atcsClc)
                {
                    prevStageTasks.AddRange(atcsc.TaskManager.Tasks_Scheduled);
                }

                if (prevStageTasks.Count > 0)
                {
                    foreach (Workorder task in Tasks_ToBeScheduled)
                    {
                        prevStageTask = null;
                        prevStageTask = prevStageTasks
                                            .Where(t =>
                                                t.ClientOrder.Id == task.ClientOrder.Id
                                                && (t.Item.ItemGroupOP.Process.ParentId == task.Item.ItemGroupOP.Process.Id))
                                            .OrderByDescending(x => x.EndTime)
                                            .Take(1).FirstOrDefault();

                        if (prevStageTask != null && prevStageTask.StartTime.Year > 2000)
                        {
                            task.ReleaseDate = prevStageTask.EndTime;
                        }
                    }
                }
            }
        }
        //---------------------------------------------------------------------LOAD_DATA
        private void LoadTaskToBeScheduled()
        {
            TaskToBeScheduledTotal = uow.WorkorderRepo.GetWorkorderOfResourceGroup(ResourceGroup.Id, new DateTime(2000,1,1), atcs.CalcManager.CalculationDateEnd).ToList();
            //TaskToBeScheduledTotal = uow.TaskRepo.GetTaskForArea(Area.Id, atcs.CalcManager.CalculationDateStart, atcs.CalcManager.CalculationDateEnd);
            //Change: 20180728 zmiana sposobu zarządzania stockiem
            //TaskToBeScheduledTotal.RemoveAll(t => t.Status >= TaskScheduleStatus.produced);
            //foreach(Task t in TaskToBeScheduledTotal)
            //{
            //    if (t.Status <= TaskScheduleStatus.planned)
            //    {
            //        t.Status = TaskScheduleStatus.initial; 
            //        t.Qty_Remain = t.Qty_Total;
            //    }
            //    else if (t.Status > TaskScheduleStatus.planned)
            //    {
            //        t.Status = TaskScheduleStatus.initial;
            //    }
            //}

            //CutBorderingOrders(TaskToBeScheduledTotal.Where(t => t.Order.StartDate < atcs.CalcManager.CalculationDateStart).ToList(), atcs.CalcManager.CalculationDateStart);
            //TaskToBeScheduledTotal.ForEach(t => t.Qty_Remain = t.Qty_Total - t.Qty_Used);
            TaskToBeScheduledTotal.ForEach(t => t.Batches.Qty = t.Qty_Total);
            Tasks_ToBeScheduled = new List<Workorder>(TaskToBeScheduledTotal);
        }
        public void LoadLastSchedule()
        {
            if (atcs != null)
                Tasks_Scheduled = uow.WorkorderRepo
                    .GetWorkorderOfResourceGroup(ResourceGroup.Id, atcs.CalcManager.CalculationDateStart, atcs.CalcManager.CalculationDateEnd)
                    .ToList();
            else
                Tasks_Scheduled = new List<Workorder>();
        }
        //private void CutBorderingOrders(List<Task> tasks, DateTime calculationDateStart)
        //{
        //    int orderTimeTotalSec = 0;
        //    int orderTimeInRangeTotalSec = 0;
        //    foreach(Task t in tasks)
        //    {
        //        orderTimeTotalSec = (int)(t.Order.EndDate - t.Order.StartDate).TotalSeconds;
        //        orderTimeTotalSec = orderTimeTotalSec > 14400 ? t.Qty_Total * 40 : orderTimeTotalSec;
        //        orderTimeInRangeTotalSec = (int)(t.Order.EndDate - calculationDateStart).TotalSeconds;
        //        orderTimeInRangeTotalSec = (orderTimeInRangeTotalSec > orderTimeTotalSec) ? (orderTimeTotalSec - (int)(calculationDateStart - t.Order.StartDate).TotalSeconds) : orderTimeInRangeTotalSec;

        //        //t.Qty_Remain = (t.Order.Qty_Remain * orderTimeInRangeTotalSec) / orderTimeTotalSec;
        //        t.Qty_Produced = t.Qty_Total - (t.Order.Qty_Remain * orderTimeInRangeTotalSec) / orderTimeTotalSec;
        //    }
        //}
        private Workorder GetLastScheduledTask(int machineId)
        {
            //return TaskScheduled.Where(t => t.MachineId == machineId).OrderByDescending(t=>t.OrderSeq).Take(1).FirstOrDefault();
            return Tasks_Scheduled.LastOrDefault(t => t.ResourceId == machineId && t.Item != null);
        }
        //---------------------------------------------------------------------CLEAR_DATA
        private void ClearLastSchedule()
        {
            if (atcs != null)
                NotificationManager.Instance.AddNotificationLog("Czyszczenie obszaru (tryb szybki) " + ResourceGroup.Name + " 0%", receiver: atcs.CalcManager.Guid);

            uow.WorkorderRepo.ClearTasks(ResourceGroup);

            if (atcs != null)
                NotificationManager.Instance.AddNotificationLog("Czyszczenie obszaru (tryb szybki) " + ResourceGroup.Name + " 100%", receiver: atcs.CalcManager.Guid);
        }
        //public void ClearTaskInArea(Area area, DateTime from, DateTime to)
        //{
        //    int l = NotificationManager.Instance.AddNotificationBlock("Clear Area " + area.Name + " from: " + from.ToString() + " - to: " + to.ToString() + " started...", receiver: atcs.CalcManager.Guid);

        //    uow.TaskRepo.ClearTaskInArea(area, from, to);

        //    NotificationManager.Instance.AddNotificationBlock("Clear Area " + area.Name + " 100%", receiver: atcs.CalcManager.Guid, id: l);
        //}
        //public void ClearTaskInArea(Area area)
        //{
        //    NotificationManager.Instance.AddNotificationLog("Czyszczenie obszaru  " + area.Name + " 0%", receiver: atcs.CalcManager.Guid);

        //    uow.TaskRepo.ClearTaskInArea(area);

        //    NotificationManager.Instance.AddNotificationLog("Czyszczenie obszaru  " + area.Name + " 100%", receiver: atcs.CalcManager.Guid);
        //}
    }
}
