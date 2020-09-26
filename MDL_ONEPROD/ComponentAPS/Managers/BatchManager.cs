

using MDL_ONEPROD.Common;
using MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDL_ONEPROD.Manager
{
    public class BatchManager
    {
        public ResourceOP Area { get; set; }
        public List<WorkorderBatch> Batches { get; set; }
        public List<Workorder> TasksTotal { get; set; }
        public List<Workorder> TasksBatched { get; set; }
        private List<ItemOP> itemGroups;
        private UnitOfWorkOneprod uow;
        private bool backward;
        private readonly string guid;
        private int batchNo;

        public BatchManager(ResourceOP area, string guid, UnitOfWorkOneprod unitOfWork)
        {
            this.guid = guid;

            backward = false;
            uow = unitOfWork;
            Area = area;
            TasksBatched = new List<Workorder>();
            TasksTotal = new List<Workorder>();
            itemGroups = uow.ItemGroupRepo.GetListAsNoTracking();

            Workorder wo = uow.WorkorderRepo.GetList().OrderByDescending(x => x.BatchNumber).Take(1).FirstOrDefault();
            batchNo = (wo != null? wo.BatchNumber : 0) + 1;
        }

        //Wszystko robimy Per AREA!!
        //zipuj wszystkie taski, które są poza datą ostatniego batchowania
        //podczas zipowania nowych zadań sprawdzić czy ostatnie bache osiągnęły wartość MinBatch i ewentualnie dobatchować
        //wyszukaj zadania niezbatchowane w zakresie już zbachowanym
        //znajdz najbardziej optymalny batch do którego wypadałoby przypisać niezbachowane zadanie
        //sprawdz czy zadania nie powinny zostać przeniesione do innych batchy.

        //------------------------------------------------------------------------------------------------------
        public List<Workorder> UnzipBatches(Workorder taskZipped)
        {
            List<Workorder> unzipedTasks = new List<Workorder>();
            Workorder originalTask = null;

            if (taskZipped.Batches != null)
            {
                for (int b = 0; b < taskZipped.Batches.Workorders.Count; b++)
                {
                    originalTask = taskZipped.Batches.Workorders[b]; //TasksTotal.FirstOrDefault(d => d.Id == taskId);
                    originalTask.ProcessingTime = taskZipped.Batches.Qty > 0? taskZipped.ProcessingTime * originalTask.Qty_Total / taskZipped.Batches.Qty : 0;
                    originalTask.ToolId = taskZipped.ToolId;
                    originalTask.ResourceId = taskZipped.ResourceId;
                    originalTask.BatchNumber = taskZipped.Batches.Number;
                    unzipedTasks.Add(originalTask);   
                }
                unzipedTasks = unzipedTasks.OrderByDescending(t => t.DueDate).ToList();
            }
            else
            {
                unzipedTasks.Add(taskZipped);
            }

            return unzipedTasks;
        }

        //---------------------------------------------ZIP-BY-PART----------------------------------------------
        //------------------------------------------------------------------------------------------------------
        public List<Workorder> ZIPByPart(List<Workorder> tasksTotal, bool batchingRequired)
        {
            if (batchingRequired)
            {
                NotificationManager.Instance.AddNotificationLog("Batching tasks by part...", receiver: guid);

                TasksTotal = tasksTotal.OrderBy(t => t.DueDate).ToList();

                if (backward)
                    ZIPbyPart_Backward();
                else
                    ZIPbyPart_Forward();

                NotificationManager.Instance.AddNotificationLog("Batching tasks by part 100%", receiver: guid);

                return TasksBatched;
            }
            else
            {
                return tasksTotal;
            }
        }
        private void ZIPbyPart_Backward()
        {
            List<Workorder> tasksSuitableForBatching;
            int r = TasksTotal.Count - 1;

            //pętla od konca do poczatku tabeli
            for (int i = r; i >= 0; i--)
            {
                if (i < TasksTotal.Count)
                {
                    if (TasksTotal[i].Status != TaskScheduleStatus.batched &&
                        TasksTotal[i].Status != TaskScheduleStatus.covered)
                    {
                        tasksSuitableForBatching = ZIPbyPart_FindSuitableTasks_Backward(i, Convert.ToInt32(TasksTotal[i].Item.ItemGroupId), TasksTotal[i].Item.Id, TasksTotal[i].Qty_Total);
                        MakeBatchFromTasks(tasksSuitableForBatching);
                    }
                }
            }
        }
        private void ZIPbyPart_Forward()
        {
            List<Workorder> tasksSuitableForBatching;

            int i = 0;
            while(i < TasksTotal.Count)
            {
                if(TasksTotal[i].BatchNumber == 0)
                {
                    tasksSuitableForBatching = ZIPbyPart_FindSuitableTasks_Forward( i, TasksTotal[i] );

                    if (tasksSuitableForBatching.Count > 0)
                    {
                        MakeBatchFromTasks(tasksSuitableForBatching);
                        i = 0;
                    }
                    else
                    {
                        MakeBatchFromTasks(new List<Workorder> { TasksTotal[i] });
                        i++;
                    }
                }
                else
                {
                    MakeBatchFromTasks(new List<Workorder> { TasksTotal[i] });
                    i++;
                }
            }
            //TODO: 20180826 - 2 pętla łączy po partCategoryId (tylko to co nie osiąga minBatch)
        }
        private List<Workorder> ZIPbyPart_FindSuitableTasks_Backward(int startRow, int partCategoryId, int partId, int qty)
        {
            List<Workorder> tasksSuitableForBatching = new List<Workorder>();

            int totalQty = 0;
            int _minBatch = GetMinBatch(partCategoryId);

            //dodaj pierwsze zlecenie
            tasksSuitableForBatching.Add(TasksTotal[startRow]);

            //pętla szuka tyle zleceń aż łaczna ilosć osiągnie minBatch
            if (startRow > 0)
            {
                for (int i = startRow - 1; i >= 0; i--)
                {
                    if (partId == TasksTotal[i].Item.Id && TasksTotal[i].Status == TaskScheduleStatus.initial)
                    {
                        tasksSuitableForBatching.Add(TasksTotal[i]);
                        totalQty += TasksTotal[i].Qty_Total;
                    }

                    if (totalQty >= _minBatch)
                    {
                        break;
                    }
                }
            }
            return tasksSuitableForBatching;
        }
        private List<Workorder> ZIPbyPart_FindSuitableTasks_Forward(int startRow, Workorder task9)
        {
            //TODO: 20180712 - przenieść parametr do configuracji!!! kurwa mać
            //int[] maxTs = { 0, 24, 16, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8 };
            int[] maxTs = { 96, 96, 96, 96, 96, 96, 96, 96, 96, 8, 8, 8, 8 };

            int partCategoryId = task9.Item.ItemGroupOP.Id;
            int partId = task9.Item.Id;
            int qty = task9.Qty_Total;

            //TODO: INTELLIGENT BATCHING (algorytm musi rozwazyc czy warto przyspieszac o 2 dni zlecenie tylko po to by zrobić batch. (Czasem warto))
            List<Workorder> tasksSuitableForBatching = new List<Workorder>();
            List<Workorder> tasksToBeBatched = TasksTotal.Where(t1 => t1.ItemId == partId && t1.BatchNumber == 0).OrderBy(x=>x.DueDate).ThenBy(x=>x.ClientOrder.OrderNo).ToList();

            if (tasksToBeBatched != null && tasksToBeBatched.Count > 0)
            {
                DateTime FirstTaskDueDate = tasksToBeBatched.Min(t1 => t1.DueDate);
                TimeSpan ts;

                int toBeBatchedQty = tasksToBeBatched.Sum(x => x.Qty_Total); //TasksTotal.Where(t1 => t1.PartId == partId && (t1.Status == TaskScheduleStatus.initial || t1.Status == TaskScheduleStatus.partiallyCovered)).Sum(t1 => t1.Qty_Remain);
                int stageNo = TasksTotal.FirstOrDefault(t1 => t1.ItemId == partId).Item.ItemGroupOP.ResourceGroupOP.StageNo;
                int batchedQty = 0;
                int _minBatch = GetMinBatch(partCategoryId);
 
                foreach (Workorder task7 in tasksToBeBatched)
                {
                    ts = task7.DueDate - FirstTaskDueDate;
                    
                    if ( (batchedQty < _minBatch) &&             //pętla dodaje tyle zleceń aż łaczna ilosć osiągnie minBatch
                         ((ts.TotalHours < maxTs[stageNo]) ||    //batchuj jeżeli odstep nie przekracza 24h, oraz pozostala ilosc 
                         (toBeBatchedQty < 0.25 * _minBatch)) )  //some of intelligence ;)
                    {
                        tasksSuitableForBatching.Add(task7);
                        batchedQty += task7.Qty_Total;
                        toBeBatchedQty -= task7.Qty_Total;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return tasksSuitableForBatching;
        }

        //---------------------------------------ZIP-BY-TROLLEY-CAPACITY----------------------------------------
        //------------------------------------------------------------------------------------------------------
        public List<Workorder> ZipTasksToTrolley(List<Workorder> tasks1, int trolleyCapacity)
        {
            List<Workorder> tasksForBatching = new List<Workorder>();
            //sortuj taski po duedate asc.
            List<Workorder> tasks = tasks1.OrderBy(x => x.DueDate).ToList();
            string orderNo = string.Empty;

            //wybieraj całe zlecenia dopóki SUM <= TROLLEY CAPACITY
            while (tasks.Count > 0 && tasksForBatching.Sum(x => x.Qty_Remain) <= trolleyCapacity)
            {
                orderNo = tasks.FirstOrDefault().ClientOrder.OrderNo;

                tasksForBatching.AddRange(tasks.Where(x => x.ClientOrder.OrderNo == orderNo));
                tasks.RemoveAll(x => x.ClientOrder.OrderNo == orderNo);
            }

            return tasksForBatching;
        }
        public void ZipOrdersToRack(List<Workorder> tasks, int rackCapacity)
        {
            //sortuj taski po duedate desc.
            //wybieraj całe zlecenia dopóki COUNT <= RACK CAPACITY
            //
        }
        //-------------------------------------------ZIP-BY-RAW-MATERIAL-----Knapsack-PROBLEM-------------------
        //------------------------------------------------------------------------------------------------------
        public List<Workorder> ZipTasksToRawMaterial(List<Workorder> tasks, int materialCapacity, int scrapPerPiece)
        {
            TasksBatched.Clear();
            List<Workorder> tasksForBatching = new List<Workorder>();
            List<Workorder> tasks1 = tasks.OrderBy(x => x.DueDate).ToList();
            KnapsackAlgorithm knapsackAlg = new KnapsackAlgorithm();

            //dodaje szerokość cięcia
            //for (int ii = 0; ii < tasks1.Count; ii++)
            //{
            //    tasks1[ii].Param1 += 3;
            //}

            while (tasks1.Count > 0)
            {
                tasksForBatching.Clear();
                knapsackAlg.Start_Method_1(tasks1.Select(x => x.Param1).ToList<int>(), materialCapacity, scrapPerPiece);

                Workorder tmpTask;
                foreach(int val in knapsackAlg.SelectedItems)
                {
                    tmpTask = tasks1.OrderBy(d => d.DueDate).FirstOrDefault(x => x.Param1 == val);

                    if(tmpTask != null)
                    {
                        tasksForBatching.Add(tmpTask);
                        tasks1.RemoveAll(x => x.Id == tmpTask.Id);
                    }
                    else
                    {
                        Console.WriteLine("Task NULL!");
                    }

                    
                }

                MakeBatchFromTasks(tasksForBatching);
            }
            
            return TasksBatched;
        }
        //--------------------------------------------ZIP-BY-ORDERNUMBER----------------------------------------
        //------------------------------------------------------------------------------------------------------
        public List<Workorder> ZipTasksToOrderNumber(List<Workorder> tasks)
        {
            TasksBatched.Clear();
            List<Workorder> tasksForBatching = new List<Workorder>();
            List<Workorder> tasksToBePlanned = tasks.OrderBy(x => x.DueDate).ToList();
            string orderno;
            while (tasksToBePlanned.Count > 0)
            {
                orderno = tasksToBePlanned.FirstOrDefault().ClientOrder.OrderNo;

                tasksForBatching.Clear();
                tasksForBatching.AddRange(tasksToBePlanned.Where(x => x.ClientOrder.OrderNo == orderno).ToList());
                MakeBatchFromTasks(tasksForBatching);

                tasksToBePlanned.RemoveAll(x => x.ClientOrder.OrderNo == orderno);
            }

            return TasksBatched;
        }

        //------------------------------------------------------------------------------------------------------
        private void MakeBatchFromTasks(List<Workorder> tasks)
        {
            Workorder tsk = tasks.Count > 0? tasks.FirstOrDefault().Clone() : null;

            if (tsk != null)
            {
                batchNo++;
                tsk.Batches.Qty = 0;
                tsk.Batches.Number = batchNo;

                //zip tasks
                for (int i = 0; i < tasks.Count; i++)
                {
                    tsk.Batches.Qty += tasks[i].Qty_Total;
                    tsk.Batches.Workorders.Add(tasks[i]);

                    TasksTotal.Remove(tasks[i]);
                }
                //update parameters and add batched task to new list
                tsk.BatchNumber = batchNo;
                tsk.DueDate = CalculateDueDate(tsk);//tasks.Min(t => t.DueDate);
                tsk.ReleaseDate = tasks.Max(t => t.ReleaseDate);
                TasksBatched.Add(tsk);
            }
        }
        private DateTime CalculateDueDate(Workorder task)
        {
            DateTime maxDueDate = task.Batches.Workorders.Max(t => t.DueDate);
            DateTime taskEndTime = maxDueDate;
            TimeSpan maxLatency = new TimeSpan(0, 0, 0);
            TimeSpan taskLatency;

            //algorytm próbuje pozycjonować zadanie do maxymalnego duedate.
            //sprawdza które zadanie byłoby przy takim ustawieniu opóźnione najbardziej (maxLatency)
            //i ostatecznie przesuwa pozycję w lewo o to maxLatency

            task.Batches.Workorders = task.Batches.Workorders.OrderBy(x => x.DueDate).ToList();

            for (int i = task.Batches.Workorders.Count - 2; i >= 0; i--)
            {
                taskEndTime = taskEndTime.AddSeconds(-task.Batches.Workorders[i + 1].ProcessingTime);
                taskLatency = (taskEndTime - task.Batches.Workorders[i].DueDate);
                maxLatency = (taskLatency > maxLatency) ? taskLatency : maxLatency;
            }

            return maxDueDate.AddSeconds(-(maxLatency.TotalSeconds));
        }
        private int GetMinBatch(int itemGroupId)
        {
            ItemOP pc = itemGroups.FirstOrDefault(d => d.Id == itemGroupId);

            if (pc != null)
                return pc.MinBatch;
            else
                return 20;
        }

        //OLD FUNCTIONS
        //private List<Task> TasksBatched_temp;
        //private List<Task> FindTasksSuitableForBatching_Forward_Init_OLD(int startRow, int partCategoryId, int partId, int qty)
        //{
        //    //TODO: INTELLIGENT BATCHING (algorytm musi rozwazyc czy warto przyspieszac o 2 dni zlecenie tylko po to by zrobić batch. (Czasem warto))
        //    List<Task> tasksSuitableForBatching = new List<Task>();
        //    List<Task> tasksToBeBatched = TasksTotal.Where(t1 =>
        //                    t1.PartId == partId &&
        //                        (t1.Status == TaskScheduleStatus.initial ||
        //                         t1.Status == TaskScheduleStatus.partiallyCovered))
        //                .ToList();

        //    if (tasksToBeBatched != null && tasksToBeBatched.Count > 0)
        //    {
        //        DateTime FirstTaskDueDate = tasksToBeBatched.Min(t1 => t1.DueDate);
        //        TimeSpan ts;

        //        //TODO: 20180712 - przenieść parametr do configuracji!!! kurwa mać
        //        int[] maxTs = { 0, 24, 16, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8 };
        //        int toBeBatchedQty = tasksToBeBatched.Sum(x => x.Qty_Total); //TasksTotal.Where(t1 => t1.PartId == partId && (t1.Status == TaskScheduleStatus.initial || t1.Status == TaskScheduleStatus.partiallyCovered)).Sum(t1 => t1.Qty_Remain);
        //        int stageNo = TasksTotal.FirstOrDefault(t1 => t1.PartId == partId).Part.ItemGroup.Area.StageNo;
        //        int batchedQty = 0;
        //        int _minBatch = GetMinBatch(partCategoryId);
        //        int taskCount = TasksTotal.Count - 1;

        //        //dodaj pierwsze zlecenie
        //        tasksSuitableForBatching.Add(TasksTotal[startRow]);
        //        batchedQty += TasksTotal[startRow].Qty_Total;
        //        toBeBatchedQty -= TasksTotal[startRow].Qty_Total; //change: 2018072018 - added this line
        //        //pętla szuka tyle zleceń aż łaczna ilosć osiągnie minBatch
        //        if (startRow < taskCount)
        //        {
        //            for (int i = startRow + 1; i <= taskCount; i++)
        //            {
        //                if (batchedQty >= _minBatch) { break; }

        //                if (partId == TasksTotal[i].Part.Id && (TasksTotal[i].Status == TaskScheduleStatus.initial || TasksTotal[i].Status == TaskScheduleStatus.partiallyCovered))
        //                {
        //                    ts = TasksTotal[i].DueDate - FirstTaskDueDate;
        //                    //batchuj jeżeli odstep nie przekracza 24h, oraz pozostala ilosc 
        //                    if ((ts.TotalHours < maxTs[stageNo]) || (toBeBatchedQty < 0.25 * _minBatch)) //some of intelligence ;)
        //                    {
        //                        tasksSuitableForBatching.Add(TasksTotal[i]);
        //                        batchedQty += TasksTotal[i].Qty_Total;
        //                        toBeBatchedQty -= TasksTotal[i].Qty_Total;
        //                    }
        //                    else
        //                    {
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return tasksSuitableForBatching;
        //}
        //private List<Task> FindTasksSuitableForBatching_Forward_Post_OLD(int partCategoryId, int partId, int startRow, int qty)
        //{
        //    List<Task> tasksSuitableForBatching = new List<Task>();

        //    int totalQty = 0;
        //    int _minBatch = GetMinBatch(partCategoryId);
        //    int t = TasksBatched_temp.Count - 1;
        //    //dodaj pierwsze zlecenie
        //    tasksSuitableForBatching.Add(TasksBatched_temp[startRow]);
        //    totalQty += TasksBatched_temp[startRow].Batches.Qty;
        //    //pętla szuka tyle zleceń aż łaczna ilosć osiągnie minBatch
        //    if (startRow < t)
        //    {
        //        for (int i = startRow + 1; i <= t; i++)
        //        {
        //            if (totalQty >= _minBatch)
        //            {
        //                break;
        //            }

        //            if (partCategoryId == TasksBatched_temp[i].Part.ItemGroupId && TasksBatched_temp[i].Batches.Qty < _minBatch)
        //            {
        //                tasksSuitableForBatching.Add(TasksBatched_temp[i]);
        //                totalQty += TasksBatched_temp[i].Qty_Total;
        //            }
        //        }
        //    }
        //    return tasksSuitableForBatching;
        //}
        //private void BatchUp_Method_1_old(List<Task> tasks)
        //{
        //    int qty = 0;
        //    string batches = "";

        //    //prepare data needed for unziping
        //    for (int i = 0; i < tasks.Count; i++)
        //    {
        //        qty += tasks[i].Qty_Total;
        //        batches += tasks[i].Order.Orderno;
        //        batches += "-" + tasks[i].Qty_Total.ToString();
        //        batches += "-" + tasks[i].Id.ToString();
        //        batches += "@";
        //        tasks[i].Status = TaskScheduleStatus.batched;
        //    }

        //    //update parameters and add batched task to new list
        //    Task tsk = tasks.FirstOrDefault().Clone();
        //    if (tsk != null)
        //    {
        //        //tsk.Qty = qty;
        //        //tsk.Batches = batches;
        //        tsk.Status = TaskScheduleStatus.batched;
        //        tsk.DueDate = tasks.Min(t => t.DueDate);
        //        tsk.ReleaseDate = tasks.Max(t => t.ReleaseDate);
        //        TasksBatched.Add(tsk);
        //    }
        //}
    }
}
