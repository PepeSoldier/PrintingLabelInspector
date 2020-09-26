using XLIB_COMMON.Repo;
using MDL_ONEPROD.Common;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Interface;
using MDL_ONEPROD.ComponentMes.Etities.MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.ComponentMes.Models;

namespace MDL_ONEPROD.Repo.Scheduling
{
    public class WorkplaceBufferRepo: RepoGenericAbstract<WorkplaceBuffer>
    {
        protected new IDbContextOneprodMes db;
        
        public WorkplaceBufferRepo(IDbContextOneprodMes db, IAlertManager alertManager) : base(db)
        {
            this.db = db;
        }

        public override WorkplaceBuffer GetById(int id)
        {
            return db.WorkplaceBuffers.FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<WorkplaceBuffer> GetByWorkorderId(int selectedWorkorderId)
        {
            return db.WorkplaceBuffers.Where(x => x.ParentWorkorderId == selectedWorkorderId);
        }

        public IQueryable<WorkplaceBuffer> GetByWorkplaceId(int workplaceId)
        {
            return db.WorkplaceBuffers.Where(x => x.WorkplaceId == workplaceId && x.QtyAvailable > 0);
        }

        public IQueryable<WorkplaceBuffer> FindByWorkorderAndResource(int workorderId, int resourceId, string code)
        {
            return db.WorkplaceBuffers.Where(x => 
                        x.QtyAvailable > 0 &&
                        x.Workplace.MachineId == resourceId && 
                        x.ParentWorkorderId == workorderId &&
                        (code == null || x.Code == code || x.Child.Code == code))
                    .OrderBy(x=>x.TimeLoaded);
        }
    }
}