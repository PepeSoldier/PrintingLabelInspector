
using MDL_ONEPROD.ComponentWMS._Interfaces;
using MDL_ONEPROD.ComponentWMS.UnitOfWorks;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Manager;
using MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MDL_ONEPROD.Common
{
    public class AtcsAlgorithm : IAlgorithm //: IAtcsCalculation
    {
        public AtcsAlgorithm(IDbContextOneprodAPS dbAPS, CalculationManager calc, ResourceOP area)
        {
            ResourceGroup = area;
            CalcManager = calc;
            this.db = dbAPS;

            UnitOfWorkOneprodAPS uow = new UnitOfWorkOneprodAPS(dbAPS);
            machineMgr = new MachineManager(area.Id, area.Forward, uow);
            
            areaManager = new AreaManager(uow);
            toolManager = new ToolManager(CalcManager.Guid, uow);
            //TODO: 20180722 przeniesc setup managera do task managera ?

            taskManager = new TaskManager(this, area, uow);
            setupManager = new SetupManager(taskManager, toolManager, CalcManager.Guid, uow);
            atcsParam = new AtcsParam(this);

            

        }
        public AtcsAlgorithm(IDbContextOneprodAPS dbAPS, IDbContextOneprodWMS dbWMS, CalculationManager calc, ResourceOP area) : this(dbAPS, calc, area)
        {
            if (dbWMS != null)
            {
                //UnitOfWorkOneprodWMS uowWMS = new UnitOfWorkOneprodWMS(dbWMS);
                bufferMgr = new BufferManager(dbWMS);
            }
        }

        IDbContextOneprod db;
        //UnitOfWorkONEPROD uow;
        BufferManager bufferMgr;
        AreaManager areaManager;
        AtcsParam atcsParam;
        ToolManager toolManager;
        MachineManager machineMgr;
        TaskManager taskManager;
        SetupManager setupManager;

        public ResourceOP ResourceGroup { get; set; }
        public TaskManager TaskManager { get { return taskManager; } }
        public MachineManager MachineManager { get { return machineMgr; } }
        public SetupManager SetupManager { get { return setupManager; } }
        public CalculationManager CalcManager { get; set; }
        
        public void CalculationRuleStart(IDbContextOneprodAPS db2)
        {
            NotificationManager.Instance.AddNotificationLog("Start Kalkulacji. Obszar: " + ResourceGroup.Name, receiver: CalcManager.Guid);
            taskManager.Init();

            if (taskManager.Tasks_ToBeScheduled.Count > 0)
            {
                int line = NotificationManager.Instance.AddNotificationBlock("Działanie algorytmu ATCS - " + ResourceGroup.Name, receiver: CalcManager.Guid, status: "");

                taskManager.Tasks_ToBeScheduled = taskManager.Tasks_ToBeScheduled.OrderByDescending(t => t.DueDate).ToList();
                machineMgr.InitMachineLoad(CalcManager.CalculationDateStart.AddDays(-1), CalcManager.CalculationDateEnd);
                atcsParam.ComputeFactors();

                //IDbContextONEPROD db2 = (IDbContextONEPROD)DependencyResolver.Current.GetService(typeof(IDbContextONEPROD));
                Thread t1 = new Thread(() => taskManager.SaveScheduledTasks(db2) );
                t1.Start();

                ATCSMainLoop(line);
                taskManager.EndCalc = true;

                NotificationManager.Instance.AddNotificationBlock("Działanie algorytmu ATCS - " + ResourceGroup.Name, id: line, receiver: CalcManager.Guid, status: "100%");
            }
            else
            {
                NotificationManager.Instance.AddNotificationBlock("Brak zadań do zaplanowania na obszarze " + ResourceGroup.Name, receiver: CalcManager.Guid, status: "100%");
            }
        }
        public void CalculationLoadLast()
        {
            NotificationManager.Instance.AddNotificationLog("Ładowanie poprzedniej kalkulacji - " + ResourceGroup.Name, receiver: CalcManager.Guid);
            taskManager.LoadLastSchedule();
        }

        protected virtual void ATCSMainLoop(int textBoxLine)
        {
            int n = taskManager.Tasks_ToBeScheduled.Count;
            int nTotal = n;
            int availableTaskCount;
            ResourceOP firstEmptyResource;
            ResourceOP bestMachine;
            int areaMachinesCount = machineMgr.AreaMachinesNumber();
            double avgProcessingTime;
            DateTime current_t = DateTime.Now.AddYears(-10); // env.getMaxLoadMachineIdRev(StageNo);

            while (n > 0)
            {
                ATCSResetIndexexToDefaultValue();
                avgProcessingTime = atcsParam.GetAvgP();
                firstEmptyResource = machineMgr.GetFirstEmptyResource();
                current_t = machineMgr.GetCurrentLoad(firstEmptyResource.Id);

                if (machineMgr.CheckCaledar(current_t, firstEmptyResource.Id, CalcManager.ConsiderCalendar) && current_t >= CalcManager.CalculationDateStart.AddDays(-5))
                {
                    taskManager.ScheduleTaskLineClose(firstEmptyResource.Id, 3600, current_t);
                    continue;
                }

                SortTaskToBeScheduledByDueDate();
                if (bufferMgr != null) { bufferMgr.ReleaseBoxes(current_t); }

                //if(current_t < CalcManager.CalculationDateStart.AddDays(-3) && firstEmptyMachineId == 3)
                //{
                //    int a = 0;
                //}

                availableTaskCount = ATCSCumputeIndexes(current_t, n, firstEmptyResource, avgProcessingTime);
              

                if (IsMinimalTasksAvailable(availableTaskCount, n))
                {
                    SortTaskToBeScheduledDescendingByIndex();
                    bestMachine = ATCSgetBestResource(ResourceGroup.StageNo, firstEmptyResource);
                    taskManager.ScheduleTask(bestMachine, 0, current_t, machineMgr.GetMachineEmptySeconds(bestMachine.Id), ResourceGroup.Forward);

                    NotificationManager.Instance.AddNotificationBlock("ATCS RULE for StageNo " + ResourceGroup.Name, id: textBoxLine, receiver: CalcManager.Guid, status: String.Format("{0:P2}", Convert.ToDouble((double)(nTotal - n) / (double)nTotal)));
                }
                else
                {
                    DateTime goToDate;
                    TimeSpan ts;

                    if (!ResourceGroup.Forward)
                    {
                        goToDate = TaskManager.Tasks_ToBeScheduled.Max(x => x.DueDate);
                        ts = current_t - goToDate;
                    }
                    else
                    {
                        goToDate = TaskManager.Tasks_ToBeScheduled.Min(x => x.ReleaseDate);
                        ts = goToDate - current_t;
                    }

                    int loadSeconds = (int)(ts.TotalSeconds > 0 ? ts.TotalSeconds : 30);

                    machineMgr.LoadMachine(firstEmptyResource.Id, 60, true);
                    toolManager.ReleaseTools(firstEmptyResource.Id);

                    if (current_t < CalcManager.CalculationDateStart.AddDays(-7) || current_t > CalcManager.CalculationDateEnd.AddDays(7))
                    {
                        ThrowInfoMaxTimeExceeded();
                        break;
                    }
                }

                setupManager.ToolManager.ToolModificationManager.ToolModyficationCheckIfEnd(current_t, ResourceGroup.Forward);
                n = taskManager.Tasks_ToBeScheduled.Count;
            }
        }

        private void ThrowInfoMaxTimeExceeded()
        {
            NotificationManager.Instance.AddNotificationLog("Przekroczono maksymalny czas na planowanie", receiver: CalcManager.Guid);
            NotificationManager.Instance.AddNotificationLog("Niezaplanowane zadania:", receiver: CalcManager.Guid);

            foreach (Workorder t1 in taskManager.Tasks_ToBeScheduled)
            {
                List<Workorder> unzipedTasks = taskManager.Batcher.UnzipBatches(t1);

                foreach (Workorder t2 in unzipedTasks)
                {
                    //TODO: 20180720 może dodać tym (niezaplanowanym) taskom specjalny status i jakoś je wyświetlić na wykresie gantta
                    NotificationManager.Instance.AddNotificationLog(t2.ClientOrder.OrderNo + " - " + t2.Item.Code + " - " + t2.Item.ItemGroupOP.Name, receiver: CalcManager.Guid);
                }
            }
        }
        private void SortTaskToBeScheduledDescendingByIndex()
        {
            taskManager.Tasks_ToBeScheduled = taskManager.Tasks_ToBeScheduled.OrderByDescending(d => d.Index).ToList();
        }
        private void SortTaskToBeScheduledByDueDate()
        {
            if (!ResourceGroup.Forward)
            {
                taskManager.Tasks_ToBeScheduled = taskManager.Tasks_ToBeScheduled.OrderByDescending(d => d.DueDate).ToList();
            }
            else
            {
                taskManager.Tasks_ToBeScheduled = taskManager.Tasks_ToBeScheduled.OrderBy(d => d.DueDate).ToList();
            }
        }
        private void ATCSResetIndexexToDefaultValue()
        {
            taskManager.Tasks_ToBeScheduled.ForEach(a => a.Index = -9999.99);
        }
        private bool IsMinimalTasksAvailable(int toBeScheduled, int n)
        {
            int minToBeScheduled = 1;

            //if      (StageNo == "3") minToBeScheduled = 1;
            //else if (StageNo == "2") minToBeScheduled = 1;//10;
            //else if (StageNo == "1") minToBeScheduled = 1;//15;

            int minTasks = 1; //areaMachinesCount * minToBeScheduled;

            return (toBeScheduled >= minToBeScheduled && n > minTasks) || (n <= minTasks && toBeScheduled > 0);
        }
        private bool CanTaskBePlanned(DateTime t, double machineFlowTime, DateTime rj, DateTime dj)
        {
            if (ResourceGroup.Forward)
            {
                //if (t < CalcManager.CalculationDateStart)
                  //  return true;
                //else
                    return t > rj;
            }
            else
            {
                if(dj < new DateTime(1900,1,1))
                {
                    Console.WriteLine("Throw info about wrong duedate");
                }

                return t < dj.AddSeconds(-(machineFlowTime + ResourceGroup.SafetyTime * 60));
            }
        }
        private int ATCSCumputeIndexes(DateTime t, int n, ResourceOP resource, double avgP)
        {
            bool bufforPlace;
            int toBeScheduled = 0;
            ItemOP item;
            int qty;
            string ANC;
            double machineFlowTime = machineMgr.GetMachineFlowTime(resource.Id);
            double wj, pj, SLj;
            double index;
            DateTime rj, dj;
            
            for (int i = 0; i < n; i++)
            {
                //algorithm my variables
                index = AtcsParam.IndexDefaultVal;
                item = taskManager.Tasks_ToBeScheduled[i].Item;//.ItemGroupOP.Id;
                ANC = taskManager.Tasks_ToBeScheduled[i].Item.Code;
                qty = taskManager.Tasks_ToBeScheduled[i].Qty_Total; //Qty_Remain; 

                //algorithm variables 1
                wj = taskManager.Tasks_ToBeScheduled[i].Weight;
                wj = wj > 0 ? wj : 1;
                pj = machineMgr.GetRealProcessingTime(resource, item, qty);
                rj = taskManager.Tasks_ToBeScheduled[i].ReleaseDate; 
                dj = taskManager.Tasks_ToBeScheduled[i].DueDate; 

                if (pj > 0)
                {
                    //check if duedate meet current time
                    if (CanTaskBePlanned(t, machineFlowTime, rj, dj))
                    {
                        //TODO: włączyć bufforplace 
                        //bufforPlace = bufferMgr.CheckBoxAvailibility(partCategoryId, qty, t);
                        bufforPlace = true;
                        if (bufforPlace)
                        {
                            Setup setup = setupManager.PrepareSetup(taskManager.Tasks_ToBeScheduled[i], resource.Id, false);

                            if (setup.Type == SetupType.ChangeNotPossible)
                                continue;

                            SLj = setup.Time;
                            index = atcsParam.computeRankingIndex(wj, pj, rj, dj, t, avgP, SLj, atcsParam.GetAvgS(), ResourceGroup.Forward);

                            if (index > AtcsParam.IndexDefaultVal)
                            {
                                lock (taskManager.Tasks_ToBeScheduled)
                                {
                                    taskManager.Tasks_ToBeScheduled[i].Index = index;
                                    taskManager.Tasks_ToBeScheduled[i].ToolId = setup.NewToolId;
                                    toBeScheduled++;
                                }
                            }
                        }
                    }
                }
            }

            return toBeScheduled;
        }
        private ResourceOP ATCSgetBestResource(int StageNo, ResourceOP resource)
        {
            //TODO: włączyc wybieranie najlepszej maszyny
            //int bestMachineId = resource.Id;

            //if (StageNo == 1 && taskManager.TaskScheduled.Count > 0)
            //{
            //    int currentToolId = taskManager.TaskToBeScheduled[0].Tool.Id;;
            //    int lastToolIdM;
            //    int currentCategoryId = taskManager.TaskToBeScheduled[0].Part.ItemGroup.Id;
            //    int lastCategoryId = 0;
            //    int found = 0;

            //    //sprawdza wszystkie maszyny z tego samego etapu
            //    foreach (Machine machine in machineMgr.Machines)
            //    {
                    
            //        for (int t2 = taskManager.TaskScheduled.Count - 1; t2 >= 0; t2--)
            //        {
            //            //szuka ostatniego zadania dla każdej maszyny
            //            if (taskManager.TaskScheduled[t2].MachineId == machine.Id)
            //            {
            //                if (StageNo == 1)
            //                {
            //                    lastToolIdM = taskManager.TaskScheduled[t2].Tool.Id;

            //                    //sprawdza czy maszyna ma aktualnie potrzebne narzędzie
            //                    if (lastToolIdM == currentToolId)
            //                    {
            //                        //jeżeli tak to wybiera ją
            //                        bestMachineId = machine.Id; //Convert.ToInt32(env.DtMachines.Rows[m]["id"]);
            //                        found = 1;
            //                    }
            //                    break;
            //                }
            //                else
            //                {
            //                    lastCategoryId = taskManager.TaskScheduled[t2].Part.ItemGroup.Id;

            //                    if (lastCategoryId == currentCategoryId)
            //                    {
            //                        bestMachineId = machine.Id; //Convert.ToInt32(env.DtMachines.Rows[m]["id"]);
            //                        found = 1;
            //                    }
            //                }
            //            }
            //        }
                    
            //        if (found == 1)
            //            break;
            //    }
            //}
            return resource;
        }
    }
}
