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
    public class WorkplaceRepo: RepoGenericAbstract<Workplace>
    {
        protected new IDbContextOneprodMes db;
        
        public WorkplaceRepo(IDbContextOneprodMes db, IAlertManager alertManager) : base(db)
        {
            this.db = db;
        }

        public override Workplace GetById(int id)
        {
            return db.Workplaces.FirstOrDefault(x => x.Id == id);
        }
        public Workplace GetByIP(string ip)
        {
            return db.Workplaces.FirstOrDefault(x => x.ComputerHostName == ip);
        }
        public Workplace GetBySelectedTaskId(int workorderId)
        {
            return db.Workplaces.FirstOrDefault(x => x.SelectedTaskId == workorderId);
        }

        public override IQueryable<Workplace> GetList()
        {
            return db.Workplaces.Where(x => x.Deleted == false).OrderBy(x => x.Name);
        }
        public IQueryable<Workplace> GetListByMachine(int machineId)
        {
            return db.Workplaces.AsNoTracking().Where(m => m.MachineId == machineId).OrderBy(m => m.Name);
        }

        public int MachineWorkplacesNumber(int machineId)
        {
            return db.Workplaces.Count(o => o.MachineId == machineId);
        }
        public bool SetSelectedTask(Workplace workplace, Workorder task)
        {
            int r = 0;

            if (workplace != null && task != null)
            {
                workplace.SelectedTask = task;
                workplace.SelectedTaskId = task.Id;
                workplace.LabelANC = task.Item.Code;
                r = AddOrUpdate(workplace);
            }

            return r > 0;
        }
    }
}