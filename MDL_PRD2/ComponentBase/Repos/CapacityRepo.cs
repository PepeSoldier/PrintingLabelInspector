//using MDL_BASE.Models.MasterData;
using XLIB_COMMON.Repo;
using MDL_PRD.Entity;
using MDL_PRD.Interface;
using MDL_PRD.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MDL_PRD.Repo
{
    public class CapacityRepo : RepoGenericAbstract<Calendar>
    {
        protected new IDbContextPRD db;
        
        public CapacityRepo(IDbContextPRD db)
            : base(db)
        {
            this.db = db;
        }

        public override Calendar GetById(int id)
        {
            return db.Calendar.FirstOrDefault(d => d.Id == id);
        }
        public override IQueryable<Calendar> GetList()
        {
            return db.Calendar.OrderBy(x => x.Date);
        }

        public Calendar GetByDateAndLine(DateTime date, string lineName)
        {
            return db.Calendar.FirstOrDefault(x => x.Date == date && x.LineName == lineName);
        }

        public Calendar GetNextOpenedTimeWindow(DateTime dateTime, string lineName)
        {
            return db.Calendar.Where(x => x.StartTime >= dateTime && x.LineName == lineName).OrderBy(y => y.StartTime).Take(1).FirstOrDefault();
        }

        public DateTime GetEndDateOfProductionInterval(DateTime dateStart, string lineName, int hoursInterval)
        {
            DateTime datePointer = new DateTime(dateStart.Year, dateStart.Month, dateStart.Day, dateStart.Hour, dateStart.Minute, dateStart.Second);

            Calendar calendar = null;
            int remainHours = 0;
            int i = 0;

            while(hoursInterval > 0)
            {
                calendar = GetNextOpenedTimeWindow(dateStart.Date.AddDays(i), lineName);
                if (calendar != null)
                {
                    datePointer = (datePointer > calendar.StartTime || datePointer == dateStart) ? datePointer : calendar.StartTime;
                    remainHours = (int)(calendar.EndTime - datePointer).TotalHours;
                    hoursInterval = hoursInterval - remainHours;
                    i++;
                    datePointer = datePointer.AddHours(remainHours);
                }
                else
                {
                    break;
                }
            }

            if (calendar != null)
                return calendar.EndTime.AddHours(hoursInterval);
            else
                return dateStart.AddHours(hoursInterval);
        }
    }
}