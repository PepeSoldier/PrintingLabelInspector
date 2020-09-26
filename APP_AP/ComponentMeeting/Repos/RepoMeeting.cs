using MDL_AP.Models.ActionPlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDL_BASE.Interfaces;
using MDL_AP.Models;
using XLIB_COMMON.Repo;
using XLIB_COMMON.Interface;
using MDL_AP.Interfaces;

namespace MDL_AP.Repo.ActionPlan
{
    public class RepoMeeting : RepoGenericAbstract<Meeting>
    {
       protected new IDbContextAP db;
       private UnitOfWorkActionPlan unitOfWork;

        public RepoMeeting(IDbContextAP db, IAlertManager alertManager = null, UnitOfWorkActionPlan unitOfWork = null) : base(db, alertManager)
        {
            this.db = db;
            this.unitOfWork = unitOfWork;
        }


        public override Meeting GetById(int id)
        {
            return db.Meetings.FirstOrDefault(x => x.Id == id);
        }
        public override IQueryable<Meeting> GetList()
        {
            return db.Meetings.OrderByDescending(x => x.Id);
        }

        public IQueryable<ActionModel> GetActions(int ObserverId)
        {
            return unitOfWork.RepoActionObserver.GetActions(ObserverId, 0).OrderByDescending(x => x.Id);   
        }
        public int AddActionToMeeting(int actionId, int observerId)
        {
            return  unitOfWork.RepoActionObserver.AddActionToObserver(actionId, observerId, 0);
        }

    }
}