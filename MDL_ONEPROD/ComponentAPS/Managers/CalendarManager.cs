
using MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDL_ONEPROD.Manager
{
    public class CalendarManager
    {
        private int areaId;
        //private int areaStageNo1;
        private List<Calendar2> dtCalendar;
        private UnitOfWorkOneprodAPS uow;

        public int AreaId { get { return areaId; } }
        //public int AreaStageNo { get { return areaStageNo1; } }
        
        public CalendarManager(UnitOfWorkOneprodAPS unitOfWork)
        {
            uow = unitOfWork;
        }
        public CalendarManager(int areaId, UnitOfWorkOneprodAPS unitOfWork)
        {
            uow = unitOfWork;
            this.areaId = areaId;
            //areaStageNo1 = areaStageNo;
            dtCalendar = new List<Calendar2>();

            List<ResourceOP> machinesOnArea = ((IEnumerable<ResourceOP>)uow.ResourceRepo.GetListByGroup(areaId)).ToList();

            foreach(ResourceOP m in machinesOnArea)
            {
                dtCalendar.AddRange(uow.CalendarRepo.GetListByMachineIdAndDate(m.Id, DateTime.Now.AddDays(-7)));
            }
        }
        
        public bool IsClosedHour(DateTime dateTime, int machineId)
        {
            DateTime dateEnd = dateTime.AddHours(1);
            Calendar2 calendar = dtCalendar.FirstOrDefault(x => x.MachineId == machineId && (x.DateClose <= dateTime && dateTime < x.DateOpen));

            return calendar != null;
        }
        public double RestTimeToClose(DateTime dateTime, int machineId)
        {
            double restTime = 61;
            
            DateTime dateTimeThisHour = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
            DateTime dateTimeNextHour = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);

            dateTimeThisHour = dateTimeThisHour.AddHours(dateTime.Hour);
            dateTimeNextHour = dateTimeNextHour.AddHours(dateTime.Hour - 1);

            if (IsClosedHour(dateTimeThisHour, machineId))
            {
                restTime = (dateTimeThisHour - dateTime).TotalMinutes;
            }
            else if (IsClosedHour(dateTimeNextHour, machineId))
            {
                restTime = (dateTimeNextHour - dateTime).TotalMinutes;
            }

            return restTime;
        }

    }
}
