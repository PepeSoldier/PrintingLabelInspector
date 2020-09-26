using MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.Manager
{
    public class ToolManager
    {
        private string Guid;
        private UnitOfWorkOneprodAPS uow;
        private List<ToolMachine> MachinesTools;
        private List<Tool> Tools;
        private List<ResourceOP> Machines;
        private List<ItemGroupTool> ItemGroupTools;

        private ToolModificationManager toolModificationManager;
        public ToolModificationManager ToolModificationManager
        {
            get { return toolModificationManager; }
        }

        public ToolManager(string guid, UnitOfWorkOneprodAPS unitOfWork) // : base()
        {
            this.Guid = guid;
            this.uow = unitOfWork;
            
            toolModificationManager = new ToolModificationManager(guid, uow);

            Tools = uow.ToolRepo.GetList().ToList();
            MachinesTools = uow.ToolMachineRepo.GetList().ToList();
            Machines = ((IEnumerable<ResourceOP>)uow.ResourceRepo.GetList()).ToList();
            ItemGroupTools = uow.ItemGroupToolRepo.GetList().ToList();
        }

        //TOOL_MACHINE----------------------------------------------------------------------------
        public bool IsToolPlacedOnMachine(int? machineId, int? toolId)
        {
            if (IsToolRequired(machineId))
            {
                ToolMachine tm = MachinesTools.FirstOrDefault(mt => mt.ToolId == toolId && mt.MachineId == machineId);
                return (tm != null) ? tm.Placed : false;
            }
            else
            {
                return true;
            }
        }
        public void MoveToolAndDoChangeOver(int machineId, int? toolId)
        {
            //toolId = toolId != null ? toolId : 0;
            if (toolId != null)
            {
                List<ToolMachine> tmList = MachinesTools.Where(mt => mt.ToolId == toolId).ToList();

                foreach (ToolMachine tm in tmList)
                {
                    if (tm.MachineId == machineId)
                        tm.Placed = true;
                    else
                        tm.Placed = false;
                }
            }
            DoChangeOver(machineId, toolId);
        }
        public void DoChangeOver(int MachineId, int? toolId)
        {
            if (toolId != null && IsToolRequired(MachineId))
            {
                List<ToolMachine> tmList = MachinesTools.Where(mt => mt.ToolId == toolId).ToList();
                foreach (ToolMachine tm in tmList)
                {
                    if (tm.MachineId == MachineId)
                    {
                        //zarezerwuj
                        tm.InUse = true;
                        tm.Placed = true;
                    }
                    else
                    {
                        //zwolnij
                        tm.InUse = false;
                        tm.Placed = false;
                    }
                }
            }
        }
        public void ReleaseTools(int machineId)
        {
            if (IsToolRequired(machineId))
            {
                List<ToolMachine> tmList = MachinesTools.Where(mt => mt.MachineId == machineId).ToList();
                foreach (ToolMachine tm in tmList)
                {
                    tm.InUse = false;
                }
            }
        }

        public List<int> GetAvailableTools(int machineId, List<int> toolIds)
        {
            List<int> toolsAvailable = null;

            //czy w ogole narzędzie jest wymagane dla tej maszyny
            if (IsToolRequired(machineId))
            {
                toolsAvailable = new List<int>();

                foreach (int toolId in toolIds)
                {
                    //sprawdz czy można toolId używać na wybranej maszynie
                    if (IsToolAssignedToMachine(machineId, toolId))
                    {
                        //narzędzie nie moze być zablokowane ani w użyciu na innej maszynie
                        if (!ToolModificationManager.IsToolLocked(toolId) && !IsToolInUseOnOtherMachine(machineId, toolId))
                        {
                            //ewentualna modyfikacja musi być możliwa
                            if (toolModificationManager.IsToolModyficationPossible(toolId))
                            {
                                toolsAvailable.Add(toolId);
                            }
                        }
                    }
                }
            }

            return toolsAvailable;
        }
        private bool IsToolAssignedToMachine(int machineId, int toolId)
        {
            return (MachinesTools.FirstOrDefault(m => m.MachineId == machineId && m.ToolId == toolId) != null) ? true : false;
        }
        private bool IsToolInUseOnOtherMachine(int machineId, int toolId)
        {
            //sprawdza czy tool oraz wszystkie powiązane z nim toole sa w uzyciu
            List<ToolMachine> toolMachine = new List<ToolMachine>();
            List<int> connectedToolIds = GetConnectedToolIDs(toolId);

            foreach (int toolId1 in connectedToolIds)
            {
                toolMachine.AddRange(MachinesTools.Where(mt => mt.ToolId == toolId1).ToList());
            }

            for (int i = 0; i < toolMachine.Count - 1; i++)
            {
                if (toolMachine[i].InUse)
                {
                    return (toolMachine[i].MachineId == machineId) ? false : true;
                }
            }

            return false;
        }

        protected bool IsToolRequired(int? machineId)
        {
            if (machineId == 0)
            {
                Common.NotificationManager.Instance.AddNotificationLog("<<WARNING>> MachineId = 0!", receiver: this.Guid);
                return false;
            }
            else
            {
                ResourceOP mch = Machines.FirstOrDefault(m => m.Id == machineId);
                return (mch != null) ? mch.ToolRequired : false;
            }
        }
        private List<int> GetConnectedToolIDs(int toolId)
        {
            int groupId = toolModificationManager.GetGroupId(toolId);

            if (groupId > 0)
                return Tools.Where(t => t.ToolGroupId == groupId).Select(t => t.Id).ToList();
            else
                return new List<int> { toolId };
        }
        public List<int> GetAssignedToolIds(int partCategoryId)
        {
            return ItemGroupTools.Where(p => p.ItemGroupId == partCategoryId).Select(p => p.ToolId).ToList();
        }
        
        //Tool
        public string GetToolName(int toolId)
        {
            Tool tool = uow.ToolRepo.GetTool(toolId);
            return (tool != null) ? tool.Name : string.Empty;
        }

    }
}