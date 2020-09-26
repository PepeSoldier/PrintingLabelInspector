using MDL_ONEPROD.Common;
using MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDL_ONEPROD.Manager
{
    public class ToolModificationManager
    {
        private string Guid;
        private UnitOfWorkOneprodAPS uow;
        private List<ToolModification> currentModification;
        private List<Tool> Tools;
        private List<ResourceOP> Machines;
        
        public ToolModificationManager(string guid, UnitOfWorkOneprodAPS unitOfWork) //: base()
        {
            this.Guid = guid;
            this.uow = unitOfWork;

            currentModification = new List<ToolModification>();
            Tools = uow.ToolRepo.GetList().ToList();
            Machines = ((IEnumerable<ResourceOP>)uow.ResourceRepo.GetList()).ToList();
        }

        //TOOL_GROUP----------------------------------------------------------------------------
        public bool IsToolModificationRequired(int? toolId)
        {
            Tool t = Tools.FirstOrDefault(x => x.Id == toolId);

            if (t != null && t.ToolGroupId != null)
            {
                return !t.Modified;
            }

            return false;
        }
        public bool IsToolModyficationPossible(int? toolId)
        {
            if (IsToolModificationRequired(toolId))
            {
                if (IsToolModyficationInProcess())
                    return false;
                else
                    return true;
            }
            else
            {
                return true;
            }
        }
        public bool IsModificationMoveRequired(int? toolId)
        {
            Tool t = Tools.FirstOrDefault(x => x.Id == toolId);

            if (t != null && t.ToolGroupId != null)
            {
                return t.Modified;
            }
            return false;
        }

        public void ToolModyficationStart(int toolId, DateTime startTime1, DateTime endTime1)
        {
            ToolModyficationResetModified(GetGroupId(toolId));    //skasuj info o modyfikacji
            LockTool(toolId);                                     //zablokuj narzedzie
            currentModification.Add(
                new ToolModification { ToolId = toolId, EndTime = endTime1, StartTime = startTime1 }
            );
        }
        public void ToolModyficationCheckIfEnd(DateTime t, bool forward)
        {
            if (currentModification.Count > 0)
            {
                int i = 0;
                while (i < currentModification.Count)
                {
                    //sprawdz czy juz koniec modyfikacji
                    if ((forward && t > currentModification[i].EndTime) || (!forward && t < currentModification[i].StartTime))
                    {
                        //oznacz jako ukonczona
                        ToolModyficationEnd(currentModification[i]);
                    }
                    else
                    {
                        i++;
                    }
                }
            }
        }

        private void ToolModyficationEnd(ToolModification tm)
        {
            ModifyTool(tm.ToolId);                                  //zrob modyfikacje
            UnlockTool(tm.ToolId);                                  //odblokuj narzedzie
            currentModification.Remove(tm);                         //zwolnij modyfikacje
        }
        private void ToolModyficationResetModified(int groupId)
        {
            foreach (Tool t in Tools)
            {
                if (t.ToolGroupId == groupId)
                    t.Modified = false;
            }
        }

        private void ModifyTool(int toolId)
        {
            //find parent tool ID
            int groupId = GetGroupId(toolId);

            if (groupId > 0)
            {
                //for each tool in group mark toolId as "in use", rest children as "not in use"
                foreach (Tool tool in Tools)
                {
                    if (groupId == tool.ToolGroupId)
                    {
                        if (toolId == tool.Id)
                        {
                            tool.Modified = true;
                        }
                        else
                        {
                            tool.Modified = false;
                        }
                    }
                }
            }
        }
        private void ModifyTool_LoadSetupLine(int machineId, DateTime setupTime)
        {
            lock (Machines)
            {
                Machines.FirstOrDefault(m => m.Id == machineId).Load = setupTime;
            }
        }
        private void LockTool(int toolId)
        {
            int groupId = GetGroupId(toolId);
            LockToolGroup(groupId);
        }
        private void UnlockTool(int toolId)
        {
            int groupId = GetGroupId(toolId);
            UnlockToolGroup(groupId);
        }
        private void LockToolGroup(int groupId)
        {
            foreach (Tool t in Tools)
            {
                if (t.ToolGroupId == groupId)
                    t.Locked = true;
            }
        }
        private void UnlockToolGroup(int groupId)
        {
            foreach (Tool t in Tools)
            {
                if (t.ToolGroupId == groupId)
                    t.Locked = false;
            }
        }
        private bool IsToolModyficationInProcess()
        {
            if (currentModification.Count > 0)
            {
                return true;
            }
            return false;
        }

        public bool IsToolLocked(int? toolId)
        {
            Tool tool = Tools.FirstOrDefault(t => t.Id == toolId);
            return (tool != null) ? tool.Locked : false;
        }
        public int GetGroupId(int toolId)
        {
            int? id = Tools.FirstOrDefault(x => x.Id == toolId).ToolGroupId;
            return (id == null) ? 0 : (int)id;
        }
       

        //MACHINE------------------------------------------------------------------------------
        private DateTime GetSetupLineLoad(int machineId, double setupTime)
        {
            DateTime retVal = DateTime.Now.AddMonths(6);

            lock (Machines)
            {
                ResourceOP m1 = Machines.FirstOrDefault(m => m.Id == machineId);

                if (m1 != null)
                    retVal = m1.Load;
            }

            return retVal;
        }
        
    }
}
