using XLIB_COMMON.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using MDL_BASE.Interfaces;
using MDL_AP.Models.ActionPlan;
using MDL_AP.Models.Reports;
using MDL_AP.Models.DEF;
using XLIB_COMMON.Interface;
using MDL_AP.Interfaces;
using XLIB_COMMON.Enums;
using MDL_AP.ComponentBase.Models;

namespace MDL_AP.Repo.ActionPlan
{
    public class RepoAction : RepoGenericAbstract<ActionModel>
    {
        protected new IDbContextAP db;
        private UnitOfWorkActionPlan unitOfWork;

        public RepoAction(IDbContextAP db, IAlertManager alertManager, UnitOfWorkActionPlan unitOfWork = null) : base(db, alertManager)
        {
            this.db = db;
            this.unitOfWork = unitOfWork;
        }

        public override ActionModel GetById(int id)
        {
            return db.Actions.FirstOrDefault(d => d.Id == id);
        }
        public override IQueryable<ActionModel> GetList()
        {
            return db.Actions.OrderByDescending(x => x.Id);
        }
        public override void Delete(IModelEntity entity)
        {
            ActionModel actionModelForDelete = GetById(entity.Id);

            unitOfWork.RepoActionActivity.DeleteByAction(actionModelForDelete);
            unitOfWork.RepoAttachment.DeleteByAction(actionModelForDelete);

            DecreaseSubactionsCount(actionModelForDelete.ParentActionId);
            DeleteChildrenActions(actionModelForDelete.Id);

            base.Delete(actionModelForDelete);
        }
        public override int Add(IModelEntity entity)
        {
            base.Add(entity);
            ActionModel am = GetById(entity.Id);
            unitOfWork.RepoActionActivity.AddLogAction(am, am.CreatorId);
            return entity.Id;
        }

        public ActionModel AddChild(int parentActionId, string UserId)
        {
            ActionModel parentActionModel = GetById(parentActionId);
            ActionModel actionModel = new ActionModel();

            actionModel.ParentActionId = parentActionId;
            actionModel.CreatorId = UserId;
            actionModel.DateCreated = DateTime.Now;
            actionModel.StartDate = parentActionModel.StartDate;
            actionModel.PlannedEndDate = parentActionModel.PlannedEndDate;
            actionModel.AreaId = parentActionModel.AreaId;
            actionModel.DepartmentId = parentActionModel.DepartmentId;
            actionModel.StartDate = DateTime.Now;
            actionModel.EndDate = null;
            actionModel.ShiftCodeId = parentActionModel.ShiftCodeId;
            actionModel.WorkstationId = parentActionModel.WorkstationId;
            actionModel.CategoryId = parentActionModel.CategoryId;
            actionModel.LineId = parentActionModel.LineId;
            actionModel.TypeId = parentActionModel.TypeId;
            actionModel.Title = "Nowa niezdefiniowana akcja";
            actionModel.Problem = "...";
            AddOrUpdate(actionModel);

            IncreaseSubactionsCount(parentActionId);

            return actionModel;
        }

        private void IncreaseSubactionsCount(int actionId)
        {
            ActionModel action = GetById(actionId);
            action.SubactionsCount += 1;
            Update(action);
        }
        private void DecreaseSubactionsCount(int actionId)
        {
            ActionModel action = GetById(actionId);
            if (action != null)
            {
                action.SubactionsCount -= (action.SubactionsCount > 0) ? 1 : 0;
                Update(action);
            }
        }
        private void DeleteChildrenActions(int parentActionId)
        {
            List<ActionModel> childrenActionModels = GetChildrens(parentActionId).ToList();
            foreach (ActionModel am in childrenActionModels)
            {
                Delete(am);
            }
        }

        public IQueryable<ActionModel> GetListByDepartmentId(int departmentId)
        {
            List<ActionActivity> lista = new List<ActionActivity>();

            return db.Actions.Where(o => o.DepartmentId == departmentId).OrderByDescending(x => x.Id);
        }
        public IQueryable<ActionModel> GetListByCreatorId(string creatorId)
        {
            return db.Actions.Where(o => o.CreatorId == creatorId).OrderByDescending(x => x.Id);
        }
        public IQueryable<ActionModel> GetList(ActionFilterModel vm, string state)
        {
            ActionStateEnum eaS = (ActionStateEnum)Convert.ToInt32(state);
            //ObservationTypeEnum ote = (ObservationTypeEnum)Convert.ToInt32(vm.Type);

            string creatorId = vm.CreatorId;
            string assignedId = vm.AssignedId;

            IQueryable<ActionModel> dL = db.Actions.AsNoTracking()
            .Where(o =>
                (o.Id == vm.Id || vm.Id == 0) &&
                (o.Assigned.Id == assignedId || assignedId == null) &&
                (o.Creator.Id == creatorId || creatorId == null) &&
                (o.AreaId == vm.AreaId || vm.AreaId == null) &&
                (o.WorkstationId == vm.WorkstationId || vm.WorkstationId == null) &&
                (o.ShiftCodeId == vm.ShiftCodeId || vm.ShiftCodeId == null) &&
                (o.DepartmentId == vm.DepartmentId || vm.DepartmentId == null) &&
                (o.LineId == vm.LineId || vm.LineId == null) &&
                (o.CategoryId == vm.CategoryId || vm.CategoryId == null) &&
                (o.PlannedEndDate <= vm.PlannedEndDate || (vm.PlannedEndDate == null || vm.PlannedEndDate == new DateTime(1, 1, 1))) &&
                (o.EndDate <= vm.EndDateTo || (vm.EndDateTo == null || vm.EndDateTo == new DateTime(1, 1, 1))) &&
                (o.State == eaS || state == null) &&
                (o.TypeId == vm.TypeId || vm.TypeId == null) &&
                ((o.ParentActionId == 0 && !vm.ShowChildActions) || vm.ShowChildActions) &&
                (!vm.ShowOnlyOpenedActions || (o.State != ActionStateEnum.stateAct))
            ).OrderByDescending(o => o.Id);

            return dL;
        }
        //public IQueryable<ActionModel> GetList(ActionBrowseViewModel vm, string state)
        //{
        //    ActionStateEnum eaS = (ActionStateEnum)Convert.ToInt32(state);
        //    //ObservationTypeEnum ote = (ObservationTypeEnum)Convert.ToInt32(vm.Type);

        //    string creatorId = vm.CreatorId;
        //    string assignedId = vm.AssignedId;
            
        //    IQueryable<ActionModel> dL = db.Actions.AsNoTracking()
        //    .Where(o =>
        //        (o.Id == vm.FilterObject.Id || vm.FilterObject.Id == 0) &&
        //        (o.Assigned.Id == assignedId || assignedId == null) &&
        //        (o.Creator.Id == creatorId || creatorId == null) &&
        //        (o.AreaId == vm.AreaId || vm.AreaId == null) &&
        //        (o.WorkstationId == vm.WorkstationId || vm.WorkstationId == null) &&
        //        (o.ShiftCodeId == vm.ShiftCodeId || vm.ShiftCodeId == null) &&
        //        (o.DepartmentId == vm.DepartmentId || vm.DepartmentId == null) &&
        //        (o.LineId == vm.LineId || vm.LineId == null) &&
        //        (o.CategoryId == vm.CategoryId || vm.CategoryId == null) &&
        //        (o.PlannedEndDate <= vm.FilterObject.PlannedEndDate || (vm.FilterObject.PlannedEndDate == null || vm.FilterObject.PlannedEndDate == new DateTime(1,1,1))) &&
        //        (o.EndDate <= vm.FilterObject.EndDate || (vm.FilterObject.EndDate == null || vm.FilterObject.EndDate == new DateTime(1,1,1))) &&
        //        (o.State == eaS || state == null) &&
        //        (o.TypeId == vm.TypeId || vm.TypeId == null) &&
        //        ((o.ParentActionId == 0 && !vm.ShowChildActions) || vm.ShowChildActions) &&
        //        (!vm.ShowOnlyOpenedActions || (o.State != ActionStateEnum.stateAct))
        //    ).OrderByDescending(o => o.Id);

        //    return dL;
        //}
        public IQueryable<ActionModel> GetListNew(ActionFilterModel vm, string state)
        {
            ActionStateEnum eaS = (ActionStateEnum)Convert.ToInt32(state);
            //ObservationTypeEnum ote = (ObservationTypeEnum)Convert.ToInt32(vm.Type);

            string creatorId = vm.CreatorId;
            string assignedId = vm.AssignedId;

            IQueryable<ActionModel> dL = db.Actions.AsNoTracking()
            .Where(o =>
                (o.Id == vm.Id || vm.Id == 0) &&
                (o.Assigned.Id == assignedId || assignedId == null) &&
                (o.Creator.Id == creatorId || creatorId == null) &&
                (o.AreaId == vm.AreaId || vm.AreaId == null) &&
                (o.WorkstationId == vm.WorkstationId || vm.WorkstationId == null) &&
                (o.ShiftCodeId == vm.ShiftCodeId || vm.ShiftCodeId == null) &&
                (o.DepartmentId == vm.DepartmentId || vm.DepartmentId == null) &&
                (o.LineId == vm.LineId || vm.LineId == null) &&
                (o.CategoryId == vm.CategoryId || vm.CategoryId == null) &&
                (o.StartDate >= vm.EndDateFrom || (vm.EndDateFrom == null || vm.EndDateFrom == new DateTime(1, 1, 1))) &&
                (o.PlannedEndDate <= vm.EndDateTo || (vm.EndDateTo == null || vm.EndDateTo == new DateTime(1, 1, 1))) &&
                (o.State == eaS || state == null) &&
                (o.TypeId == vm.TypeId || vm.TypeId == null) &&
                ((o.ParentActionId == 0 && !vm.ShowChildActions) || vm.ShowChildActions)
            ).OrderByDescending(o => o.Id);

            return dL;
        }
        public IQueryable<ActionModel> GetChildrens(int parentId)
        {
            return db.Actions.Where(x => x.ParentActionId == parentId).OrderByDescending(x => x.Id);
        }
        public IQueryable<ActionModel> GetList(int observationId)
        {
            //return db.Actions.Where(x => x.ObservationId == observationId).OrderByDescending(x => x.Id);
            return Enumerable.Empty<ActionModel>().AsQueryable();
        }

        public string GetColor(ActionModel action)
        {
            switch(action.State)
            {
                case ActionStateEnum.statePlan: return ColorPCDA.PlanColorHex;
                case ActionStateEnum.stateDo: return ColorPCDA.DoColorHex;
                case ActionStateEnum.stateCheck: return ColorPCDA.CheckColorHex;
                case ActionStateEnum.stateAct: return ColorPCDA.ActColorHex;
                default: return "#CCCCCC";
            }
        }

        public int CountActionsInAll(ActionStateEnum? state = null)
        {
            return db.Actions.Count(x => x.State == state || state == null);
        }
        public int CountActionsInDepartment(int departmentId, ActionStateEnum? state = null)
        {
            return db.Actions.Count(x => x.DepartmentId == departmentId
                                    && (x.State == state || state == null)
                                    );
        }
        public int CountActionsCreator(string userId, ActionStateEnum? state = null)
        {
            return db.Actions.Count(x => x.CreatorId == userId
                                    && (x.State == state || state == null)
                                    );
        }
        public int CountSubActions(int actionId)
        {
            return db.Actions.Count(x => x.ParentActionId == actionId);
        }
        public int CountSubActionsFinished(int actionId)
        {
            return 0; //db.Actions.Count(x => x.ParentActionId == actionId && x.State == ActionStateEnum.stateFinished);
        }
        public int CountSubActionsDistinctResponsible(int actionId)
        {
            return db.Actions.Where(x => x.ParentActionId == actionId).Select(x=>x.AssignedId).Distinct().Count();
        }

        //TODO: 20181006 czemu ta funkcja jest tutaj ?
        public List<MainReport> GetReport(IQueryable<ActionModel> allActions)
        {
            List<MainReport> list = new List<MainReport>();
            //TODO: random colors
            string[] colors = new string[] { "#d6dae0", ColorPCDA.PlanColorHex, ColorPCDA.DoColorHex, ColorPCDA.CheckColorHex, ColorPCDA.ActColorHex, "#2F3585", "#3F9555" };
            foreach (ActionStateEnum state in Enum.GetValues(typeof(ActionStateEnum)))
            {
                MainReport mr = new MainReport();
                mr.name = EnumHelper<ActionStateEnum>.GetDisplayValue(state);
                mr.value = allActions.Count(x => x.State  == state);
                mr.color = colors[(int)state];
                //mr.chartType = chartType;
                list.Add(mr);
            }
         
            return list;
        }
        
        public int RegisterObserver(IModelEntity observer, int actionId, string userId, ObserverType observerType)
        {
            ActionObserver aObserver = new ActionObserver();
            aObserver.ObserverId = observer.Id;
            aObserver.ActionId = actionId;
            aObserver.ObserverType = (int)observerType;
            aObserver.UserId = userId;

            //TODO: 20181006 jaki to miało sens?
            //if (userId != String.Empty)
            //{
            //    aObserver.UserId = userId;
            //}
            //else
            //{
            //    aObserver.UserId = String.Empty;
            //}

            unitOfWork.RepoActionObserver.AddOrUpdate(aObserver);

            return aObserver.Id;
        }
        public void DeleteObserver(IModelEntity ob, int actionId, string userId, ObserverType observerType)
        {
            unitOfWork.RepoActionObserver.RemoveObserver(ob, actionId, userId, observerType);
        }

    }
}