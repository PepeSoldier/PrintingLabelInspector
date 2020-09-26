using _MPPL_WEB_START.Areas._APPWEB.Controllers;
using _MPPL_WEB_START.Areas.AP.ViewModel;
using _MPPL_WEB_START.Areas.Models;
using _MPPL_WEB_START.Models;
using MDL_AP.ComponentBase.Models;
using MDL_AP.Interfaces;
using MDL_AP.Models.ActionPlan;
using MDL_AP.Repo;
using MDL_BASE.Models.IDENTITY;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using XLIB_COMMON.Enums;
using XLIB_COMMON.Interface;
using XLIB_COMMON.Model;
using XLIB_COMMON.Repo.IDENTITY;

namespace _MPPL_WEB_START.Areas.AP.Controllers
{
    [Authorize]
    public class ActionController : BaseController
    {
        private IDbContextAP db;
        private Mailer_AP mailerAp;
        private UserRepo UserManager;
        private UnitOfWorkActionPlan UnitOfWorkActionPlan;

        public ActionController(IDbContextAP db, IUserStore<User, string> userStore)
        {
            this.db = db;
            UnitOfWorkActionPlan = new UnitOfWorkActionPlan(db);
            UserManager = new UserRepo(userStore, db);
            mailerAp = new Mailer_AP(Mailer.Create());
        }

        public ActionResult Show(int id = 0)
        {
            ActionShowViewModel vm = new ActionShowViewModel();
            vm.Action = UnitOfWorkActionPlan.RepoAction.GetById(id);

            if (vm.Action != null)
            {
                User user = UserManager.FindById(User.Identity.GetUserId());
                List<User> personList = UserManager.Users.Where(x => x.DepartmentId == vm.Action.DepartmentId).ToList();

                vm.ActionId = id;
                vm.SubactionsCount = UnitOfWorkActionPlan.RepoAction.CountSubActions(id);
                vm.SubactionsFinishedCount = UnitOfWorkActionPlan.RepoAction.CountSubActionsFinished(id);
                vm.SubactionsDistinctResponsibleCount = UnitOfWorkActionPlan.RepoAction.CountSubActionsDistinctResponsible(id);
                vm.ActionStateEnum = vm.Action.State;
                vm.Attachments = UnitOfWorkActionPlan.RepoAttachment.GetListByAction(vm.Action).ToList();
                vm.Users = new SelectList(personList, "Id", "FullName", vm.Action.AssignedId);
                vm.IsUserAllowedToEdit = isUserAllowedToEditAction(user, vm.Action);
                return View(vm);
            }
            AlertManager.Instance.AddAlert(AlertMessageType.warning, "Akcja o numerze: " + id + " nie istnieje", User.Identity.Name);
            return RedirectToAction("Browsenew");
        }

        public ActionResult Edit(int id = 0, int meetingId = 0)
        {
            User user = UserManager.FindByName(User.Identity.Name);
            ActionEditViewModel vm = new ActionEditViewModel();

            vm.NewObject = AssignNewObject(id);

            if (!isUserAllowedToEditAction(user, vm.NewObject))
            {
                return RedirectToAction("Show", new { id = id });
            }

            vm.UserName = (vm.NewObject.Assigned != null) ? vm.NewObject.Assigned.FullName : string.Empty;
            vm.OldDepartmentId = (vm.NewObject.DepartmentId != null) ? (int)vm.NewObject.DepartmentId : 0;

            vm.Workstations = new SelectList(UnitOfWorkActionPlan.RepoWorkstation.GetListByArea(vm.NewObject.AreaId), "Id", "Name", vm.NewObject.WorkstationId);
            vm.Areas = new SelectList(UnitOfWorkActionPlan.RepoArea.GetList(), "Id", "Name", vm.NewObject.AreaId);
            vm.ShiftCodes = new SelectList(UnitOfWorkActionPlan.RepoShiftCode.GetList(), "Id", "Name", vm.NewObject.ShiftCodeId);
            vm.Departments = new SelectList(UnitOfWorkActionPlan.RepoDepartment.GetList().ToList(), "Id", "Name", vm.NewObject.DepartmentId);
            vm.Lines = new SelectList(UnitOfWorkActionPlan.ResourceRepo.GetListByArea(vm.NewObject.AreaId).ToList(), "Id", "Name", vm.NewObject.LineId);
            vm.Categories = new SelectList(UnitOfWorkActionPlan.RepoCategory.GetList().ToList(), "Id", "Name", vm.NewObject.CategoryId);
            vm.Types = new SelectList(UnitOfWorkActionPlan.RepoType.GetList().ToList(), "Id", "Name", vm.NewObject.TypeId);
            vm.MeetingId = meetingId;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ActionEditViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            ActionModel amInDb;
            ActionModel amModified;

            mailerAp = new Mailer_AP(Mailer.Create());

            if (!(vm.NewObject.Id > 0))
            {
                amModified = vm.NewObject;
                amModified.DateCreated = DateTime.Now;
                amModified.CreatorId = User.Identity.GetUserId();

                UnitOfWorkActionPlan.RepoAction.AddOrUpdate(amModified);
                string mailReceivers = MailHelper.GetUserEmails(UserManager.GetManagers((int)amModified.DepartmentId));

                mailerAp.ActionCreated(amModified, mailReceivers);
            }
            else
            {
                amInDb = UnitOfWorkActionPlan.RepoAction.GetList().Where(x => x.Id == vm.NewObject.Id).AsNoTracking().FirstOrDefault();
                amModified = UnitOfWorkActionPlan.RepoAction.GetById(vm.NewObject.Id);
                amModified.Title = vm.NewObject.Title;
                amModified.Problem = vm.NewObject.Problem;
                amModified.AreaId = vm.NewObject.AreaId;
                amModified.DepartmentId = vm.NewObject.DepartmentId;
                amModified.ShiftCodeId = vm.NewObject.ShiftCodeId;
                amModified.WorkstationId = vm.NewObject.WorkstationId;
                amModified.StartDate = vm.NewObject.StartDate;
                amModified.PlannedEndDate = vm.NewObject.PlannedEndDate;
                amModified.TypeId = vm.NewObject.TypeId;
                amModified.CategoryId = vm.NewObject.CategoryId;

                string modDetails = AnalizeModifications(amInDb, amModified);

                UnitOfWorkActionPlan.RepoAction.AddOrUpdate(amModified);
                UnitOfWorkActionPlan.RepoActionActivity.AddLog(amInDb.Id, User.Identity.GetUserId(), modDetails);

                string mailReceivers = string.Empty;
                mailReceivers = MailHelper.GetUserEmail(amModified.Creator, mailReceivers, User.Identity.Name);
                mailReceivers = MailHelper.GetUserEmail(amModified.Assigned, mailReceivers, User.Identity.Name);

                mailerAp.ActionModified(amModified, mailReceivers, modDetails);
            }
            if (vm.MeetingId > 0)
            {
                UnitOfWorkActionPlan.RepoActionObserver.AddActionToObserver(vm.NewObject.Id, vm.MeetingId, 0);
                return RedirectToAction("Show", "Meeting", new { id = vm.MeetingId });
            }
            else
            {
                return RedirectToAction("Show", new { id = amModified.Id });
            }
        }

        private ActionModel AssignNewObject(int id)
        {
            ActionModel NewObject;
            if (id > 0)
            {
                NewObject = UnitOfWorkActionPlan.RepoAction.GetById(id);
            }
            else
            {
                NewObject = new ActionModel();
                NewObject.State = ActionStateEnum.statePlan;
                NewObject.StartDate = DateTime.Now.Date;
                NewObject.PlannedEndDate = DateTime.Now.Date.AddDays(7);
                NewObject.EndDate = null;
                NewObject.DepartmentId = 0;
                NewObject.WorkstationId = 0;
                NewObject.AreaId = 0;
                NewObject.ShiftCodeId = 0;
                NewObject.TypeId = 1;
            }
            return NewObject;
        }

        private string AnalizeModifications(ActionModel amInDb, ActionModel amModified)
        {
            string modifications = string.Empty;

            if (amModified.DepartmentId != amInDb.DepartmentId)
            {
                string mailReceivers = string.Empty;
                string newDepartment = UnitOfWorkActionPlan.RepoDepartment.GetById((int)amModified.DepartmentId).Name;

                amModified.AssignedId = null;
                UnitOfWorkActionPlan.RepoAction.Update(amModified);
                UnitOfWorkActionPlan.RepoActionActivity.AddLogDepartment(amModified, User.Identity.GetUserId());

                mailReceivers = MailHelper.GetUserEmails(UserManager.GetManagers((int)amModified.DepartmentId));
                mailerAp.ActionDepartmentManagerInfo(amModified, mailReceivers, newDepartment);

                mailReceivers = MailHelper.GetUserEmail(amModified.Creator, string.Empty, User.Identity.Name);
                mailerAp.ActionDepartmentChanged(amModified, mailReceivers, newDepartment);
            }
            if (amModified.Title != amInDb.Title)
            {
                modifications += "Ustawiono tytuł: " + amModified.Title + ". ";
                modifications += System.Environment.NewLine;
            }
            if (amModified.PlannedEndDate != amInDb.PlannedEndDate)
            {
                modifications += "Ustawiono Planowaną datę ukończenia: " + amModified.PlannedEndDate.ToShortDateString() + ". ";
                modifications += System.Environment.NewLine;
            }
            if (amModified.EndDate != amInDb.EndDate)
            {
                modifications += "Ustawiono datę ukończenia: " + amModified.EndDate.Value.ToShortDateString() + ". ";
                modifications += System.Environment.NewLine;
            }

            return modifications;
        }

        [HttpPost]
        public JsonResult UpdateDates(ActionModel action1)
        {
            ActionModel actionDb = UnitOfWorkActionPlan.RepoAction.GetById(action1.Id);
            actionDb.StartDate = action1.StartDate;
            actionDb.PlannedEndDate = action1.PlannedEndDate;

            UnitOfWorkActionPlan.RepoAction.Update(actionDb);

            return Json(actionDb.Id);
        }

        [HttpPost]
        public JsonResult ChangeStatus(ActionShowViewModel vm)
        {
            if (vm.ActionId > 0)
            {
                ActionActivity actionActivity = new ActionActivity();
                ActionModel action = UnitOfWorkActionPlan.RepoAction.GetById(vm.ActionId);
                action.State = vm.ActionStateEnum;
                UnitOfWorkActionPlan.RepoActionActivity.AddLogStateChange(action, User.Identity.GetUserId());
                UnitOfWorkActionPlan.RepoAction.AddOrUpdate(action);

                string mailReceivers = string.Empty;
                mailReceivers = MailHelper.GetUserEmail(action.Creator, mailReceivers, User.Identity.Name);
                mailReceivers = MailHelper.GetUserEmail(action.Assigned, mailReceivers, User.Identity.Name);

                mailerAp.ActionStatusChanged(action, mailReceivers, vm.ActionStateEnum.ToString());

                if (action.State == ActionStateEnum.stateAct)
                {
                    mailReceivers = MailHelper.GetUserEmails(UserManager.GetManagers((int)action.DepartmentId));
                    mailerAp.ActionStatusChanged(action, mailReceivers, vm.ActionStateEnum.ToString());
                }
                AlertManager.Instance.AddAlert(AlertMessageType.info, "Ustawiono status: " + EnumHelper<ActionStateEnum>.GetDisplayValue(vm.ActionStateEnum), User.Identity.Name);
            }

            //return RedirectToAction("Show", new { id = vm.ActionId });
            return Json(vm.ActionId);
        }

        [HttpPost]
        public JsonResult ChangeAssigned(ActionShowViewModel vm)
        {
            if (vm.ActionId > 0)
            {
                ActionActivity actionActivity = new ActionActivity();
                ActionModel action = UnitOfWorkActionPlan.RepoAction.GetById(vm.ActionId);
                User newResponsible = UserManager.FindById(vm.AssignedId);

                if (newResponsible != null && action.AssignedId != newResponsible.Id)
                {
                    action.AssignedId = newResponsible.Id;

                    UnitOfWorkActionPlan.RepoAction.AddOrUpdate(action);
                    UnitOfWorkActionPlan.RepoActionActivity.AddLogResponsible(action, UserManager.FindByName(User.Identity.Name));

                    string mailReceivers = string.Empty;
                    mailReceivers = MailHelper.GetUserEmail(action.Creator, mailReceivers, User.Identity.Name);
                    mailReceivers = MailHelper.GetUserEmail(action.Assigned, mailReceivers, User.Identity.Name);

                    mailerAp.ActionResponsibleChanged(action, mailReceivers, newResponsible.FullName);

                    AlertManager.Instance.AddAlert(AlertMessageType.info, "Przypisano odpowiedzialnego: " + newResponsible.FullName, User.Identity.Name);
                }
                else if (newResponsible == null)
                {
                    AlertManager.Instance.AddAlert(AlertMessageType.warning, "Wybierz odpowiedzialnego z listy. Jeżeli nie ma na liście odpowiedniej osoby spróbuj zmienić przypisany dział.", User.Identity.Name);
                }
            }

            //return RedirectToAction("Show", new { id = vm.ActionId });
            return Json(vm.ActionId);
        }

        public ActionResult Delete(int id = 0)
        {
            ActionModel am = UnitOfWorkActionPlan.RepoAction.GetById(id);

            if (am.Creator.UserName == User.Identity.Name || User.IsInRole(DefRoles.ADMIN))
            {
                UnitOfWorkActionPlan.RepoAction.Delete(am);
                return RedirectToAction("Browsenew");
            }

            return RedirectToAction("Edit", new { id = id });
        }

        public JsonResult CreateChild(string parentId)
        {
            int ParentId = Convert.ToInt32(parentId);
            string UserId = User.Identity.GetUserId();

            if (ParentId > 0)
            {
                UnitOfWorkActionPlan.RepoAction.AddChild(ParentId, UserId);
                AlertManager.Instance.AddAlert(AlertMessageType.info, "Dodano nową podakcję. Edytuj ją aby ustawić jej parametry.", User.Identity.Name);
            }

            return Json(ParentId, JsonRequestBehavior.AllowGet);
        }

        //-----------------------------------------------------------------------------------------------
        public ActionResult Browse(int id = 0)
        {
            ActionBrowseViewModel vm = new ActionBrowseViewModel();
            vm.FilterObject = new MDL_AP.ComponentBase.Models.ActionFilterModel();

            vm.ActionModels = UnitOfWorkActionPlan.RepoAction.GetList().Where(x => x.ParentActionId == 0);

            vm.Workstations = (new SelectList(UnitOfWorkActionPlan.RepoWorkstation.GetList().ToList(), "Id", "Name"));
            vm.Areas = new SelectList(UnitOfWorkActionPlan.RepoArea.GetList().ToList(), "Id", "Name");
            vm.ShiftCodes = new SelectList(UnitOfWorkActionPlan.RepoShiftCode.GetList().ToList(), "Id", "Name");
            vm.Departments = new SelectList(UnitOfWorkActionPlan.RepoDepartment.GetList().ToList(), "Id", "Name");
            vm.Lines = new SelectList(UnitOfWorkActionPlan.ResourceRepo.GetList().ToList(), "Id", "Name");
            vm.Categories = new SelectList(UnitOfWorkActionPlan.RepoCategory.GetList().ToList(), "Id", "Name");
            vm.Types = new SelectList(UnitOfWorkActionPlan.RepoType.GetList().ToList(), "Id", "Name");
            vm.FilterObject.ShowChildActions = false;

            if (id > 0)
            {
                vm.Meeting = UnitOfWorkActionPlan.RepoMeeting.GetById(id);
                vm.ActionModelMeetings = UnitOfWorkActionPlan.RepoMeeting.GetActions(id).ToList();
            }
            return View(vm);
        }

        [HttpPost]
        public ActionResult Browse(ActionBrowseViewModel vm)
        {
            vm.ActionModels = UnitOfWorkActionPlan.RepoAction.GetList(vm.FilterObject, vm.FilterObject.State);
            vm.FilterObject.PlannedEndDate = Convert.ToDateTime(vm.FilterObject.PlannedEndDate.Value);
            vm.FilterObject.EndDateTo = Convert.ToDateTime(vm.FilterObject.EndDateTo.Value);

            vm.Workstations = new SelectList(UnitOfWorkActionPlan.RepoWorkstation.GetList().ToList(), "Id", "Name");
            vm.Departments = new SelectList(UnitOfWorkActionPlan.RepoDepartment.GetList().ToList(), "Id", "Name");
            vm.Categories = new SelectList(UnitOfWorkActionPlan.RepoCategory.GetList().ToList(), "Id", "Name");
            vm.ShiftCodes = new SelectList(UnitOfWorkActionPlan.RepoShiftCode.GetList().ToList(), "Id", "Name");
            vm.Areas = new SelectList(UnitOfWorkActionPlan.RepoArea.GetList().ToList(), "Id", "Name");
            vm.Lines = new SelectList(UnitOfWorkActionPlan.ResourceRepo.GetList().ToList(), "Id", "Name");
            vm.Types = new SelectList(UnitOfWorkActionPlan.RepoType.GetList().ToList(), "Id", "Name");

            return View(vm);
        }

        public ActionResult Browsenew(string states = null, int departmentId = 0, string dateFrom = null, string assignedId = null, string creatorId = null)
        {
            ActionBrowsenewViewModel vm = new ActionBrowsenewViewModel();
            vm.FilterObject = new ActionFilterModel();

            vm.FilterObject.CreatorId = creatorId;
            vm.FilterObject.AssignedId = assignedId;
            vm.FilterObject.CreatorFullName = (creatorId != null) ? UserManager.FindById(creatorId).FullName : string.Empty;
            vm.FilterObject.AssignedFullName = (assignedId != null) ? UserManager.FindById(assignedId).FullName : string.Empty;
            vm.FilterObject.EndDateFrom = dateFrom != null ? (DateTime?)Convert.ToDateTime(dateFrom) : null;
            vm.FilterObject.ShowChildActions = states != null;

            vm.Workstations = new SelectList(UnitOfWorkActionPlan.RepoWorkstation.GetListByArea(0).ToList(), "Id", "Name");
            vm.Areas = new SelectList(UnitOfWorkActionPlan.RepoArea.GetList().ToList(), "Id", "Name");
            vm.ShiftCodes = new SelectList(UnitOfWorkActionPlan.RepoShiftCode.GetList().ToList(), "Id", "Name");
            vm.Departments = new SelectList(UnitOfWorkActionPlan.RepoDepartment.GetList().ToList(), "Id", "Name", departmentId);
            vm.Lines = new SelectList(UnitOfWorkActionPlan.ResourceRepo.GetListByArea(0).ToList(), "Id", "Name");
            vm.Categories = new SelectList(UnitOfWorkActionPlan.RepoCategory.GetList().ToList(), "Id", "Name");
            vm.Types = new SelectList(UnitOfWorkActionPlan.RepoType.GetList().ToList(), "Id", "Name");

            vm.ActionStates = PrepareActionStateList(null, (states != null ? states.Split(',') : null));

            return View(vm);
        }

        public ActionResult BrowseGrid(int currentPage = 1, int rowsOnPage = 5)
        {
            ActionBrowsenewViewModel vm = new ActionBrowsenewViewModel();

            IQueryable<ActionModel> am = UnitOfWorkActionPlan.RepoAction.GetList().Where(x => x.ParentActionId == 0);

            vm.CurrentPage = currentPage;
            vm.RowsOnPage = rowsOnPage;
            vm.ActionStates = PrepareActionStateList(am);
            vm.TotalRows = am.Count();
            vm.ActionModels = am.Skip((currentPage - 1) * rowsOnPage).Take(rowsOnPage);

            return View(vm);
        }

        [HttpPost]
        public ActionResult BrowseGrid(ActionBrowsenewViewModel vm)
        {
            IQueryable<ActionModel> am;
            am = UnitOfWorkActionPlan.RepoAction.GetListNew(vm.FilterObject, null);
            vm.ActionStates = PrepareActionStateList(am);
            am = FilterByStates(am, vm.States);
            vm.TotalRows = am.Count();
            vm.ActionModels = am.Skip((vm.CurrentPage - 1) * vm.RowsOnPage).Take(vm.RowsOnPage);

            return View(vm);
        }

        private List<ActionState> PrepareActionStateList(IQueryable<ActionModel> baseQuery = null, string[] selectedValues = null)
        {
            List<ActionState> asl = new List<ActionState>();

            foreach (ActionStateEnum state in Enum.GetValues(typeof(ActionStateEnum)))
            {
                asl.Add(new ActionState
                {
                    Id = (int)state,
                    Name = EnumHelper<ActionStateEnum>.GetDisplayValue(state),
                    Count = 0,
                    Selected = true
                });
            }

            foreach (ActionState astate in asl)
            {
                if (baseQuery != null)
                {
                    astate.Count = baseQuery.Where(x => x.State == (ActionStateEnum)astate.Id).Count();
                }

                if (selectedValues != null && !selectedValues.Contains(astate.Name))
                {
                    astate.Selected = false;
                }
            }

            return asl;
        }

        private IQueryable<ActionModel> FilterByStates(IQueryable<ActionModel> baseQuery, string statuses)
        {
            List<int> StatusesList = new JavaScriptSerializer().Deserialize<List<int>>(statuses);

            foreach (int status in StatusesList)
            {
                baseQuery = baseQuery.Where(x => x.State != (ActionStateEnum)status);
            }
            return baseQuery;
        }

        public PartialViewResult ShowChildrenActions(int id)
        {
            ActionShowViewModel vm = new ActionShowViewModel();
            vm.Action = UnitOfWorkActionPlan.RepoAction.GetById(id);
            vm.ActionId = id;

            if (vm.Action != null)
            {
                vm.ChildActions = UnitOfWorkActionPlan.RepoAction.GetChildrens(vm.Action.Id).ToList();
                return PartialView(vm);
            }

            return new PartialViewResult();
        }

        [HttpPost]
        public JsonResult GetChildrenActions(int id)
        {
            List<ActionModel> actions = UnitOfWorkActionPlan.RepoAction.GetChildrens(id).ToList();
            AssignIndicatorClass(actions);
            return Json(actions);
        }

        private void AssignIndicatorClass(List<ActionModel> actions)
        {
            foreach (ActionModel u in actions)
            {
                if ((u.State != ActionStateEnum.stateAct && u.PlannedEndDate.Date < DateTime.Now.Date))
                {
                    u.Variable1 = "ActionDelayed";
                }
                else if ((u.State == ActionStateEnum.stateAct))
                {
                    u.Variable1 = "ActionFinished";
                }
                else
                {
                    u.Variable1 = "ActionNormal";
                }
            }
        }

        //--------------------------------------OTHER-----------------------------------------------------
        public JsonResult UserAutoComplete(string Prefix, string DepartmentId)
        {
            List<UserLight> userList = UserManager.GetUserListByDepartmentAutocomplete(Prefix, DepartmentId);
            return Json(userList, JsonRequestBehavior.AllowGet);
        }

        private bool isUserAllowedToEditAction(User user, ActionModel action)
        {
            if ((action.Department != null && user.DepartmentId == action.DepartmentId) ||
                user.Id == action.AssignedId ||
                user.Id == action.CreatorId ||
                !(action.Id > 0) ||
                User.IsInRole(DefRoles.ADMIN)
                )
            {
                return true;
            }

            return false;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        //-------------------------------------MEETING----------------------------------------------------
        public ActionResult CyclicMeeting()
        {
            return View();
        }
    }
}

//public ActionResult MainChartView()
//{
//    IQueryable<ActionModel> AllActions = UnitOfWorkActionPlan.RepoAction.GetList();
//    List<MainReport> report = UnitOfWorkActionPlan.RepoAction.GetReport(AllActions);

//    return View(report);
//}

//public JsonResult BrowseGridData(int current = 1, int rowCount = 2)
//{
//    ActionBrowseViewModel vm = new ActionBrowseViewModel();
//    vm.ActionModels = UnitOfWorkActionPlan.RepoAction.GetList().Where(x => x.ParentActionId == 0)
//        .Skip((current-1) * rowCount).Take(rowCount);

//    GridDataBrowse gd = new GridDataBrowse();
//    gd.current = current;
//    gd.rowCount = rowCount;
//    gd.total = UnitOfWorkActionPlan.RepoAction.GetList().Where(x => x.ParentActionId == 0).Count();
//    gd.rows = GridDataMapping(vm.ActionModels.ToList());

//    return Json(gd, JsonRequestBehavior.AllowGet);
//}
//private List<GridDataTemplate> GridDataMapping(List<ActionModel> ActionModels)
//{
//    List<GridDataTemplate> rows = new List<GridDataTemplate>();

//    foreach(ActionModel am in ActionModels)
//    {
//        rows.Add(new GridDataTemplate
//        {
//            Id = am.Id,
//            AreaName = am.Area.Name,
//            CreatorFullName = am.Creator.FullName,
//            Title = am.Title
//        });
//    }

//    return rows;
//}
//private class GridData
//{
//    public int current { get; set; }
//    public int rowCount { get; set; }
//    public int total { get; set; }
//}
//private class GridDataBrowse : GridData
//{
//    public List<GridDataTemplate> rows { get; set; }
//}
//private class GridDataTemplate
//{
//    public int Id { get; set; }
//    public string Title { get; set; }
//    public string CreatorFullName { get; set; }
//    public string AreaName { get; set; }
//}