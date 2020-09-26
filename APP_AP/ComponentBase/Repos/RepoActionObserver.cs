using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDL_BASE.Interfaces;
using MDL_AP.Models;
using MDL_AP.Models.ActionPlan;
using XLIB_COMMON.Repo;
using XLIB_COMMON.Interface;
using MDL_AP.Interfaces;

namespace MDL_AP.Repo.ActionPlan
{
    public class RepoActionObserver : RepoGenericAbstract<ActionObserver>
    {
        protected new IDbContextAP db;
        private UnitOfWorkActionPlan unitOfWork;

        public RepoActionObserver(IDbContextAP db, IAlertManager alertManager, UnitOfWorkActionPlan unitOfWork = null) : base(db, alertManager)
        {
            this.db = db;
            this.unitOfWork = unitOfWork;
        }

        public override ActionObserver GetById(int id)
        {
            return db.ActionObservers.FirstOrDefault(x => x.Id == id);
        }

        public override IQueryable<ActionObserver> GetList()
        {
            return db.ActionObservers.OrderByDescending(x => x.Id);
        }

        public void RemoveObserver(IModelEntity observer, int actionId, string userId, ObserverType observerType)
        {
            ActionObserver aObserver = new ActionObserver();
            if (userId != String.Empty)
            {
                aObserver = db.ActionObservers.Where(x => x.ActionId == actionId && x.UserId == userId).FirstOrDefault();
            }
            else
            {
                aObserver = db.ActionObservers.Where(x => x.ActionId == actionId && x.ObserverId == observer.Id && x.ObserverType == (int)observerType).FirstOrDefault();
            }
            Delete(aObserver);
        }

        public IQueryable<ActionModel> GetActions(int observerId, int observerType)
        {
            var query = from t in db.ActionObservers
                        join k in db.Actions on t.ActionId equals k.Id
                        where t.ObserverId == observerId
                        where t.ObserverType == observerType
                        select k;

            return query;
        }

        public int AddActionToObserver(int actionId, int observerId, int observerType)
        {
            ActionObserver temp = new ActionObserver();
            temp.ActionId = actionId;
            temp.ObserverId = observerId;
            temp.ObserverType = observerType;
            AddOrUpdate(temp);
            return temp.Id;
        }

    }
}