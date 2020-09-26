using XLIB_COMMON.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDL_BASE.Interfaces;
using MDL_AP.Models;
using MDL_AP.Models.ActionPlan;
using MDL_BASE.Models.IDENTITY;
using XLIB_COMMON.Interface;
using MDL_AP.Interfaces;
using XLIB_COMMON.Enums;

namespace MDL_AP.Repo.ActionPlan
{
    public class RepoActionActivity : RepoGenericAbstract<ActionActivity>
    {
        protected new IDbContextAP db;
        private UnitOfWorkActionPlan unitOfWork;

        public RepoActionActivity(IDbContextAP db, IAlertManager alertManager, UnitOfWorkActionPlan unitOfWork = null) : base(db, alertManager)
        {
            this.db = db;
            this.unitOfWork = unitOfWork;
        }

        public override ActionActivity GetById(int id)
        {
            return db.ActionActivities.FirstOrDefault(d => d.Id == id);
        }
        public override IQueryable<ActionActivity> GetList()
        {
            return db.ActionActivities.OrderByDescending(x => x.Id);
        }
        public IQueryable<ActionActivity> GetList(ActionModel Action)
        {
            return db.ActionActivities.Where(x => x.ActionId == Action.Id).OrderByDescending(x => x.Id);
        }

        public void AddLog(int actionId, string userId, string activityDescription)
        {
            if (activityDescription.Length > 0)
            {
                ActionActivity actionActivity = new ActionActivity();
                actionActivity.ActivityEnum = ActionActivityTypeEnum.LogActivity;
                actionActivity.ActivityDescription = activityDescription;
                actionActivity.CreatorId = userId;
                actionActivity.DateCreated = DateTime.Now;
                actionActivity.ActionId = actionId;

                AddOrUpdate(actionActivity);
            }
        }
        public void AddLogResponsible(ActionModel actionModel, User user)
        {
            ActionActivity actionActivity = new ActionActivity();
            string activityDescription = ((user != null) ? "Przypisanie odpowiedzialności do: " + user.FullName : "Usunięcie osoby odpowiedzialnej");

            AddLog(actionModel.Id, user.Id, activityDescription);
        }
        public void AddLogAction(ActionModel actionModel, string userId)
        {
            string activityDescription = "Dodanie Akcji: " + actionModel.Title;
            AddLog(actionModel.Id, userId, activityDescription);
        }
        public void AddLogDepartment(ActionModel actionModel, string userId)
        {
            string activityDescription = "Zmiana Działu: " + unitOfWork.RepoDepartment.GetById((int)actionModel.DepartmentId).Name;
            AddLog(actionModel.Id, userId, activityDescription);
        }
        public void AddLogChild(ActionModel actionModel, string userId)
        {
            string activityDescription = "Dodanie Nowej Podakcji";
            AddLog(actionModel.Id, userId, activityDescription);
        }
        public void AddLogStateChange(ActionModel actionModel, string userId)
        {
            string activityDescription = "Zmiana statusu na : " + EnumHelper<ActionStateEnum>.GetDisplayValue(actionModel.State);
            AddLog(actionModel.Id, userId, activityDescription);
        }

        public void DeleteByAction(ActionModel action)
        {
            List<ActionActivity> acList = GetList(action).ToList();
            foreach (ActionActivity aa in acList)
            {
                unitOfWork.RepoAttachment.DeleteByActivity(aa);
                Delete(aa);
            }
            
        }

    }
}