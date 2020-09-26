

using MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.Repo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MDL_ONEPROD.Manager
{
    public class MachineManager
    {
        UnitOfWorkOneprodAPS uow;
        CalendarManager calendarMgr;
        List<ResourceOP> machines;
        List<ResourceOP> areas;
        bool forw;

        public List<ResourceOP> Machines { get { return machines; } set { machines = value; } }
        public List<MCycleTime> CycleTimes { get; set; }

        public MachineManager(int areaId, bool forward, UnitOfWorkOneprodAPS unitOfWork)
        {
            forw = forward;
            uow = unitOfWork;
            calendarMgr = new CalendarManager(areaId, uow);
            machines = ((IEnumerable<ResourceOP>)uow.ResourceRepo.GetListByGroup(areaId)).ToList();
            areas = uow.ResourceGroupRepo.GetList().ToList();
            CycleTimes = uow.CycleTimeRepo.GetList().ToList();
        }

        public int AreaMachinesNumber()
        {
            return machines.Count();
        }
        public ResourceOP GetFirstEmptyResource()
        {
            if (forw)
            {
                machines = machines.OrderBy(m => m.Load).ToList();
                return machines.FirstOrDefault();
            }
            else
            {
                machines = machines.OrderByDescending(m => m.Load).ToList();
                return machines.FirstOrDefault();
            }
        }
        public int GetMachineFlowTime(int machineId)
        {
            ResourceOP m1 = machines.FirstOrDefault(m => m.Id == machineId);
            if (m1 != null)
                return m1.FlowTime;
            else
                return 0;
        }

        public void LoadMachine(int MachineId, int seconds, bool empty)
        {
            ResourceOP m = machines.FirstOrDefault(o => o.Id == MachineId);
            if (forw)
            {
                m.Load = m.Load.AddSeconds(seconds);
                m.EmptySeconds = empty ? m.EmptySeconds + seconds : 0;
            }
            else
            {
                m.Load = m.Load.AddSeconds(seconds * -1);
                m.EmptySeconds = empty ? m.EmptySeconds + seconds : 0;
            }
        }
        public void InitMachineLoad(DateTime minRD, DateTime maxDD)
        {
            if(forw)
                machines.ForEach(a => a.Load = minRD);
            else
                machines.ForEach(a => a.Load = maxDD);
        }
       
        public DateTime GetCurrentLoad(int machineId)
        {
            ResourceOP mch = machines.FirstOrDefault(m => m.Id == machineId);

            if (mch != null)
                return mch.Load;
            else
                return DateTime.Now.AddMonths(-12);
        }
        public int GetMachineEmptySeconds(int machineId)
        {
            ResourceOP mch = machines.FirstOrDefault(m => m.Id == machineId);

            if (mch != null)
                return mch.EmptySeconds;
            else
                return 0;
        }

        public bool CheckCaledar(DateTime t, int machineId, bool ConsiderCalendar)
        {
            if (calendarMgr != null && ConsiderCalendar) //(calendarMgr.UseCalendar)
            {
                double restTimeToClose = calendarMgr.RestTimeToClose(t, machineId);

                if (restTimeToClose < 1)
                {
                    return true;
                }
            }
            return false;
        }

        //CycleTime RepoEnv
        public int GetRealProcessingTime(ResourceOP resource, ItemOP item, int qty, bool ShowException = false)
        {
            MCycleTime ct = GetCycleTime(item, resource);
            decimal OEE = 0;
            decimal cycleTime = 0;

            if (ct != null)
            {
                cycleTime = ct.CycleTime;
                OEE = ct.Machine.TargetOee; //getOEE(ct.MachineId);

                if (OEE > 0)
                {
                    cycleTime = cycleTime / OEE;
                }

                return (int)(cycleTime * qty);
            }
            else
            {
                if (ShowException)
                {
                    //TODO: dodać notyfication managera do MachineManager
                    //NotificationManager.Instance.AddNotificationLog("WARNING! Brak czasu cyklu dla ItemGroupId:" + partCategoryId.ToString() + ", machineId:" + machineId.ToString(), receiver: "calc", status: "");
                }
                return 0;
            }


        }
        public MCycleTime GetCycleTime(ItemOP item, ResourceOP resource)
        {
            if (CycleTimes == null)
            {
                CycleTimes = uow.CycleTimeRepo.GetList().ToList();
            }

            //TODO: 20200329 sprawdzić czy znajduje według priorytetu od szczegółu do ogółu
            MCycleTime ct = CycleTimes.FirstOrDefault(c => 
                (c.MachineId == resource.Id && c.ItemGroupId == item.Id) ||
                (c.MachineId == resource.Id && c.ItemGroupId == item.ItemGroupId) ||
                (c.MachineId == resource.ResourceGroupId && c.ItemGroupId == item.Id) ||
                (c.MachineId == resource.ResourceGroupId && c.ItemGroupId == item.ItemGroupId)
            );

            return ct;
        }
        public List<MCycleTime> GetCycleTimes(int partCategoryId)
        {
            if (CycleTimes == null)
            {
                CycleTimes = uow.CycleTimeRepo.GetList().ToList();
            }

            return CycleTimes.Where(c => c.ItemGroupId == partCategoryId).ToList();
        }
    }
}
