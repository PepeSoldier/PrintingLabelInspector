
using MDL_ONEPROD.Common;
using MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDL_ONEPROD.Manager
{
    public class SetupManager
    {
        private string Guid;
        private List<ResourceOP> SetupLines;
        private List<SetupStatus> setupStatuses;
        private List<ChangeOver> changeOvers;
        private List<ChangeOver> ChangeOvers { get { return changeOvers; } }
        private List<ToolChangeOver> ToolChOvMatrix { get; set; }
        
        private UnitOfWorkOneprodAPS uow;
        private TaskManager taskManager;
        private ToolManager toolManager;
        public ToolManager ToolManager{ get { return toolManager; }}

        public SetupManager(TaskManager taskManager, ToolManager toolManager, string guid, UnitOfWorkOneprodAPS unitOfWork) : this(unitOfWork)
        {
            this.taskManager = taskManager;
            this.toolManager = toolManager;
            this.Guid = guid;

            changeOvers = uow.ChangeOverRepo.GetList().ToList();
            SetupLines = ((IEnumerable<ResourceOP>)uow.ResourceRepo.GetList().Where(s => s.ResourceGroupOP.Name == "Setup Line")).ToList();

            if (!(SetupLines.Count > 0))
                NotificationManager.Instance.AddNotificationLog("Brak Linii Przezbrojeń. Uwtórz Obszar o nazwie 'Setup Line'", receiver: guid);
                    
            //TODO: Init setup line load
            SetupLines.ForEach(s=>s.Load = DateTime.Now.AddDays(8));
        }
        public SetupManager(UnitOfWorkOneprodAPS unitOfWork)
        {
            this.uow = unitOfWork;
            setupStatuses = new List<SetupStatus>();
        }

        public int AvgS(int areaId)
        {
            int st = 0;

            for (int r = 0; r < ChangeOvers.Count; r++)
            {
                if (ChangeOvers[r].AreaID == areaId)
                {
                    st += ChangeOvers[r].AncChange;
                    st += ChangeOvers[r].CatergoyChange; //Convert.ToDouble(dtChangeOvers.Rows[r][Repository.ChangeOver.col_CategoryChange]);
                    st += ChangeOvers[r].MachineToolChange; // Convert.ToDouble(dtChangeOvers.Rows[r][Repository.ChangeOver.col_MachineToolChange]);
                    st += ChangeOvers[r].ToolChange; //Convert.ToDouble(dtChangeOvers.Rows[r][Repository.ChangeOver.col_ToolChange]);
                }
                //st = st / 4;
            }
            st = st > 0 ? st : 1;

            return st * 60 / 4;
        }
        public int avgS(int stageNo)
        {
            int st = 0;

            st += GetChangeOverToolModTime(stageNo);
            st += GetChangeOverToolMoveTime(stageNo);
            st += GetChangeOverToolChangeTime(stageNo);
            st += GetChangeOverAncChangeTime(stageNo);
            st += GetChangeOverCategoryChangeTime(stageNo);
            st = st / 5;

            return st * 60;
        }
        public int GetChangeOverToolModTime(int areaId)
        {
            var cho = ChangeOvers.FirstOrDefault(a => a.AreaID == areaId);
            return (cho != null) ? cho.ToolModification * 60 : 1;
        }
        public int GetChangeOverToolMoveTime(int areaId)
        {
            var cho = ChangeOvers.FirstOrDefault(a => a.AreaID == areaId);
            return (cho != null) ? cho.MachineToolChange * 60 : 1;
        }
        public int GetChangeOverToolChangeTime(int areaId)
        {
            var cho = ChangeOvers.FirstOrDefault(a => a.AreaID == areaId);
            return (cho != null) ? cho.ToolChange * 60 : 1;
        }
        public int GetChangeOverAncChangeTime(int areaId)
        {
            var cho = ChangeOvers.FirstOrDefault(a => a.AreaID == areaId);
            return (cho != null) ? cho.AncChange * 60 : 1;
        }
        public int GetChangeOverCategoryChangeTime(int areaId)
        {
            var cho = ChangeOvers.FirstOrDefault(a => a.AreaID == areaId);
            return (cho != null)? cho.CatergoyChange * 60 : 1;
        }
        
        public void ScheduleModificationSetup(Setup setup, DateTime t, int? orderId, bool forward)
        {
            ResourceOP sl = SetupLines.OrderByDescending(s => s.Load).Take(1).FirstOrDefault();

            t = (t < sl.Load) ? t : sl.Load;

            Workorder tskSetupM = new Workorder
            {
                UniqueNumber = "SETUP M",
                ClientOrderId = orderId,
                ResourceId = sl.Id,
                ToolId = setup.NewToolId,
                OrderSeq = 1,
                StartTime = t,
                EndTime = t.AddSeconds(setup.ModificationTime),
                ReleaseDate = new DateTime(1900, 1, 1),
                DueDate = new DateTime(1900, 1, 1),
                ProcessingTime = setup.ModificationTime,
                //LV = 0,
                Status = TaskScheduleStatus.planned
            };

            if (!forward)
            {
                ToolChangeOver tco = uow.ToolChangeOverRepo.GetByNewToolId(setup.OldToolId);
                int modificationTime = ((tco != null) ? tco.Time : 20) * 60;

                tskSetupM.EndTime = t.AddSeconds(modificationTime);
                tskSetupM.ProcessingTime = modificationTime;
                tskSetupM.ToolId = setup.OldToolId;
                tskSetupM.StartTime = t.AddSeconds(-setup.ModificationTime);
                tskSetupM.EndTime = t;
            }

            //uow.TaskRepo.AddOrUpdate(tskSetupM);
            taskManager.Tasks_ToBeSaved.Enqueue(tskSetupM);
            
            sl.Load = tskSetupM.StartTime;
            toolManager.ToolModificationManager.ToolModyficationStart((int)setup.NewToolId, tskSetupM.StartTime, tskSetupM.EndTime);
        }
        public void ScheduleSetup(Setup setup, DateTime t, int? orderId, int? machineId, int batchNumber, int orderSeq, bool forward)
        {
            Workorder tskSetup = new Workorder
            {
                UniqueNumber = "SETUP",
                ClientOrderId = orderId,
                ResourceId = machineId,
                ToolId = setup.NewToolId,
                OrderSeq = orderSeq,
                BatchNumber = batchNumber,
                StartTime = t,
                EndTime = t.AddSeconds(setup.Time),
                ReleaseDate = new DateTime(1900, 1, 1),
                DueDate = new DateTime(1900, 1, 1),
                ProcessingTime = setup.Time,
                //LV = 0,
                Status = TaskScheduleStatus.planned
            };

            if (!forward)
            {
                tskSetup.ToolId = setup.OldToolId;
                tskSetup.StartTime = t.AddSeconds(-setup.Time);
                tskSetup.EndTime = t;
            }

            //uow.TaskRepo.AddOrUpdate(tskSetup);
            taskManager.Tasks_ToBeSaved.Enqueue(tskSetup);

            if (setup.Type == SetupType.ToolMove)
            {
                toolManager.MoveToolAndDoChangeOver((int)machineId, setup.NewToolId);
            }
            if (setup.Type == SetupType.ToolModyfication)
            {
                ScheduleModificationSetup(setup, t, orderId, forward);
                toolManager.MoveToolAndDoChangeOver((int)machineId, setup.NewToolId);
            }
            if (setup.Type == SetupType.ToolChange)
            {
                //toolManager.ToolModificationManager.DeleteLastModification(setup.NewToolId);
                //ScheduleModificationSetup(t, orderId, setup.NewToolId, setup.ModificationTime);
                toolManager.DoChangeOver((int)machineId, setup.NewToolId);
            }
            SetSetupStatus((int)machineId, null, null, setup.NewToolId);
        }

        public Setup PrepareSetup(Workorder TaskToBeScheduled, int machineId, bool Planning)
        {
            List<int> toolIds = null;

            if (TaskToBeScheduled != null)
            {
                toolIds = toolManager.GetAssignedToolIds((int)TaskToBeScheduled.Item.ItemGroupId);
                toolIds = toolManager.GetAvailableTools(machineId, toolIds);
            }

            if (toolIds != null) //true possible only for area1. Tool is required
            {
                if (toolIds.Count > 0)
                    return PrepareSetup_LoopInToolIds(TaskToBeScheduled, machineId, Planning, toolIds);
                else
                    return new Setup { Time = 0, Type = SetupType.ChangeNotPossible, NewToolId = null, OldToolId = null };
            }
            else
            {
                SetupStatus stpst = GetSetupStatus(machineId);
                if (TaskToBeScheduled != null && stpst.PartId == TaskToBeScheduled.ItemId)
                {
                    Setup objSetup = new Setup();
                    objSetup.Type = SetupType.NoChange;
                    objSetup.Time = 0;
                    objSetup.NewToolId = stpst.ToolId;
                    objSetup.OldToolId = null;
                    return objSetup;
                }
                else
                {
                    return PrepareSetup_priv(TaskToBeScheduled, machineId, Planning);
                }
            }
        }
        private Setup PrepareSetup_LoopInToolIds(Workorder TaskToBeScheduled, int machineId, bool Planning, List<int> toolIDs)
        {
            int minSetupTime = int.MaxValue;
            Setup setupTemp = new Setup();
            Setup objSetup = new Setup { Time = 99990, Type = SetupType.ChangeNotPossible };

            foreach (int toolId in toolIDs)
            {
                setupTemp = PrepareSetup_priv(TaskToBeScheduled, machineId, Planning, toolId);
                if (setupTemp.Time < minSetupTime || setupTemp.Type != SetupType.ChangeNotPossible)
                {
                    objSetup = setupTemp;
                    minSetupTime = setupTemp.Time;
                }
            }

            return objSetup;
        }
        private Setup PrepareSetup_priv(Workorder TaskToBeScheduled, int machineId, bool Planning, int toolId = 0)
        {
            if (toolId > 0)
                return PrepareSetup_ToolArea(TaskToBeScheduled, machineId, Planning, toolId);
            else
                return PrepareSetup_NoToolArea(TaskToBeScheduled, machineId);
        }
        private Setup PrepareSetup_ToolArea(Workorder TaskToBeScheduled, int machineId, bool Planning, int? toolId = null)
        {
            SetupStatus stpst = GetSetupStatus(machineId);
            Setup objSetup = new Setup();
            objSetup.Type = SetupType.ChangeNotPossible;
            objSetup.Time = Planning ? 0 : 604800;
            objSetup.NewToolId = toolId;
            objSetup.OldToolId = stpst.ToolId;

            if (!toolManager.IsToolPlacedOnMachine(machineId, objSetup.NewToolId))
            {
                objSetup.Time = GetChangeOverToolMoveTime(TaskToBeScheduled.Item.ItemGroupOP.ResourceGroupOP.Id);
                objSetup.Type = SetupType.ToolMove;
            }
            else
            {
                if (objSetup.OldToolId != objSetup.NewToolId)
                {
                    objSetup.Time = GetChangeOverToolChangeTime(TaskToBeScheduled.Item.ItemGroupOP.ResourceGroupOP.Id);
                    objSetup.Type = SetupType.ToolChange;
                }
                else if (TaskToBeScheduled.ItemId != stpst.PartId)
                {
                    objSetup.Time = GetChangeOverAncChangeTime(TaskToBeScheduled.Item.ItemGroupOP.ResourceGroupOP.Id);
                    objSetup.Type = SetupType.AncChange;
                }
                else
                {
                    objSetup.Time = 0;
                    objSetup.Type = SetupType.NoChange;
                }
            }

            if (toolManager.ToolModificationManager.IsToolModificationRequired(objSetup.NewToolId))
            {
                ToolChangeOver tco = uow.ToolChangeOverRepo.GetByNewToolId(objSetup.NewToolId);
                objSetup.ModificationTime = ((tco != null) ? tco.Time : 20) * 60;
                objSetup.Type = SetupType.ToolModyfication;

                int timeTemp = GetChangeOverToolMoveTime(TaskToBeScheduled.Item.ItemGroupOP.ResourceGroupOP.Id);
                objSetup.Time = (objSetup.ModificationTime < timeTemp) ? objSetup.ModificationTime : timeTemp;
            }

            return objSetup;
        }
        private Setup PrepareSetup_NoToolArea(Workorder TaskToBeScheduled, int machineId)
        {
            SetupStatus stpst = GetSetupStatus(machineId);
            Setup objSetup = new Setup();
            objSetup.Type = SetupType.NoChange;
            objSetup.Time = 0;
            objSetup.NewToolId = null;
            objSetup.OldToolId = null;

            if (TaskToBeScheduled != null)
            {
                int areaId = TaskToBeScheduled.Item.ItemGroupOP.ResourceGroupOP.Id;

                if (stpst.ItemGroupId == TaskToBeScheduled.Item.ItemGroupOP.Id)
                {
                    if (stpst.PartId == TaskToBeScheduled.ItemId)
                    {
                        objSetup.Time = 0;
                        objSetup.Type = SetupType.NoChange;
                    }
                    else
                    {
                        objSetup.Time = GetChangeOverAncChangeTime(areaId);
                        objSetup.Type = SetupType.AncChange;
                    }
                }
                else
                {
                    objSetup.Type = SetupType.CategoryChange;
                    objSetup.Time = GetChangeOverCategoryChangeTime(areaId);
                }
            }

            return objSetup;
        }

        public void SetSetupStatus(int machineId, int? partId, int? partCategoryId, int? toolId)
        {
            SetupStatus ss = setupStatuses.FirstOrDefault(x => x.MachineId == machineId);
            if(ss == null)
            {
                ss = new SetupStatus { MachineId = machineId, ItemGroupId = null, PartId = null, ToolId = null };
                setupStatuses.Add(ss);
            }
            ss.ToolId = toolId != null ? toolId : ss.ToolId;
            ss.PartId = partId != null? partId : ss.PartId;
            ss.ItemGroupId = partCategoryId != null? partCategoryId : ss.ItemGroupId;
        }
        public SetupStatus GetSetupStatus(int machineId)
        {
            SetupStatus stpst = setupStatuses.FirstOrDefault(x => x.MachineId == machineId);
            if(stpst == null)
            {
                stpst = new SetupStatus { MachineId = machineId, ToolId = null, ItemGroupId = null, PartId = null };
                setupStatuses.Add(stpst);
            }
            return stpst;
        }
    }
    public class Setup
    {
        public int Time;
        public int ModificationTime;
        public SetupType Type;
        public int MachineId;
        public string OldToolName;
        public string NewToolName;
        public int? OldToolId;
        public int? NewToolId;
    }
    public class SetupToolModification
    {
        public int Time;
        public double T;
        public string CategoryName;
        public int ToolId;
        public string ToolName;
        public int MachineId;
    }
    public class SetupStatus
    {
        public int MachineId { get; set; }
        public int? PartId { get; set; }
        public int? ItemGroupId { get; set; }
        public int? ToolId { get; set; }
    }

    public enum SetupType
    {
        NoChange,
        AncChange,
        CategoryChange,
        ToolChange,
        ToolMove,
        ToolModyfication,
        ChangeNotPossible
    }

}
