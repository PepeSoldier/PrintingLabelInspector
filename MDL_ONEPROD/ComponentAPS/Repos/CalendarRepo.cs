using XLIB_COMMON.Repo;
using MDL_ONEPROD.Common;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Interface;

namespace MDL_ONEPROD.Repo.Scheduling
{
    public class CalendarRepo : RepoGenericAbstract<Calendar2>
    {
        protected new IDbContextOneprodAPS db;
        private UnitOfWorkOneprod unitOfWork;

        public CalendarRepo(IDbContextOneprodAPS db, IAlertManager alertManager, UnitOfWorkOneprod unitOfWork = null) : base(db)
        {
            this.db = db;
            this.unitOfWork = unitOfWork;
        }

        public override Calendar2 GetById(int id)
        {
            return db.Calendar2.FirstOrDefault(x => x.Id == id);
        }
        public override IQueryable<Calendar2> GetList()
        {
            return db.Calendar2.OrderBy(x => x.DateClose);
        }

        public Calendar2 GetByMachineIdAndDate(int machineId, DateTime dateTime)
        {
            return db.Calendar2.FirstOrDefault(c => c.MachineId == machineId && (c.DateClose <= dateTime && dateTime < c.DateOpen));
        }
        public List<Calendar2> GetListByMachineIdAndMonth(int machineId, int month)
        {
            return db.Calendar2.Where(c => c.MachineId == machineId && (c.DateClose.Month == month || c.DateOpen.Month == month)).ToList();
        }
        public List<Calendar2> GetListByMachineIdAndDate(int machineId, DateTime date){
            return db.Calendar2.AsNoTracking().Where(c => c.MachineId == machineId && c.DateClose > date).ToList().OrderBy(c => c.DateClose).ToList();
        }

        public List<Calendar2> GetListOfHours(int machineId, DateTime date)
        {
            DateTime dateStart = date.AddHours(-2);
            DateTime dateEnd = dateStart.AddHours(32);
            return db.Calendar2.Where(c => c.MachineId == machineId && (dateStart <= c.DateClose && c.DateClose < dateEnd)).ToList();
        }

        public int AddOrDelete(int machineId, DateTime dateTime)
        {
            Calendar2 c = GetByMachineIdAndDate(machineId, dateTime);
            if (c != null)
            {
                Delete(c);
                return 0;
            }
            else
            {
                c = new Calendar2 { MachineId = machineId, DateClose = dateTime, DateOpen = dateTime.AddHours(1), Date = dateTime.Date };
                return AddOrUpdate(c);
            }
        }
    }
}