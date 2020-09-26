using MDL_AP.Models.ActionPlan;
using MDL_BASE.Models.IDENTITY;
using _MPPL_WEB_START.Areas.AP.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _MPPL_WEB_START.Areas._APPWEB.Controllers;
using MDL_AP.Repo;
using _MPPL_WEB_START.Migrations;
using XLIB_COMMON.Repo.IDENTITY;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using XLIB_COMMON.Enums;
using MDL_AP.Interfaces;

namespace _MPPL_WEB_START.Areas.AP.Controllers
{
    [Authorize]
    public class DashboardController : BaseController
    {
        private IDbContextAP db;
        private UserRepo UserManager;
        private UnitOfWorkActionPlan UnitOfWorkActionPlan;

        public DashboardController(IDbContextAP db, IUserStore<User, string> userStore)
        {
            this.db = db;
            UserManager = new UserRepo(userStore, db);
            UnitOfWorkActionPlan = new UnitOfWorkActionPlan(db);
        }

        public ActionResult ActivitiesLog(ActionDashBoardFilterViewModel vmFilter = null)
        {
            ActionActivityLogViewModel vm = new ActionActivityLogViewModel();

            IQueryable<ActionActivity> iQAC = UnitOfWorkActionPlan.RepoActionActivity.GetList();
            User currentUser = UserManager.FindById(User.Identity.GetUserId());

            DateTime today = DateTime.Now.Date;

            if (vmFilter.TimeFilter == 1) iQAC = iQAC.Where(x => x.DateCreated >= DbFunctions.AddDays(today, -31));
            if (vmFilter.TimeFilter == 2) iQAC = iQAC.Where(x => x.DateCreated >= DbFunctions.AddDays(today, -7));
            if (vmFilter.TimeFilter == 3) iQAC = iQAC.Where(x => x.DateCreated >= today);

            if (vmFilter.PersonalFilter == 1) iQAC = iQAC.Where(x => x.Action.DepartmentId == currentUser.DepartmentId);
            if (vmFilter.PersonalFilter == 2) iQAC = iQAC.Where(x => x.Action.AssignedId == currentUser.Id);
            if (vmFilter.PersonalFilter == 3) iQAC = iQAC.Where(x => x.Action.CreatorId == currentUser.Id);

            vm.ActionActivities = iQAC.Skip(vmFilter.LogCurrentRow).Take(vmFilter.LogLoadRows).ToList();

            foreach (ActionActivity aa in vm.ActionActivities)
            {
                aa.Attachments = UnitOfWorkActionPlan.RepoAttachment.GetListByActivity(aa).ToList();
            }

            return PartialView(vm);
        }
        public ActionResult Controlls()
        {
            ActionDashBoardFilterViewModel vmFilter = new ActionDashBoardFilterViewModel();

            vmFilter.TimeFilter = 1;
            vmFilter.PersonalFilter = 2;

            return View(vmFilter);
        }
        public ActionResult PieChart(ActionDashBoardFilterViewModel vmFilter = null)
        {
            ActionDashBoardViewModel2 vm = new ActionDashBoardViewModel2();

            DateTime dateFrom = DateTime.Now.Date;
            User user = UserManager.FindById(User.Identity.GetUserId());
            int departmentId = (user != null && user.DepartmentId != null) ? (int)user.DepartmentId : 0;

            IQueryable<ActionModel> AllActionsAll = UnitOfWorkActionPlan.RepoAction.GetList();

            if (vmFilter.TimeFilter == 0) dateFrom = DateTime.Now.Date.AddYears(-10);
            if (vmFilter.TimeFilter == 1) dateFrom = DateTime.Now.Date.AddDays(-31);
            if (vmFilter.TimeFilter == 2) dateFrom = DateTime.Now.Date.AddDays(-7);
            if (vmFilter.TimeFilter == 3) dateFrom = DateTime.Now.Date; 

            AllActionsAll = AllActionsAll.Where(x => x.DateCreated >= dateFrom || (dateFrom == null));

            if (vmFilter.PersonalFilter == 1) AllActionsAll = AllActionsAll.Where(x => x.DepartmentId == user.DepartmentId);
            if (vmFilter.PersonalFilter == 2) AllActionsAll = AllActionsAll.Where(x => x.AssignedId == user.Id);
            if (vmFilter.PersonalFilter == 3) AllActionsAll = AllActionsAll.Where(x => x.CreatorId == user.Id);

            vm.DashBoardTitleAll = vmFilter.ChartTitle;
            vm.ChartDataAll = new ChartData{ data = UnitOfWorkActionPlan.RepoAction.GetReport(AllActionsAll), chartType = vmFilter.ChartTitle, user = user, dateFrom = dateFrom.ToShortDateString() };
            vm.TotalActionsAll = AllActionsAll.Count();
            vm.ActionsStatusAll = new List<string>();
            vm.ChartDivId = "myChart" + vmFilter.PersonalFilter.ToString();

            foreach (ActionStateEnum actionState in Enum.GetValues(typeof(ActionStateEnum)))
            {
                vm.ActionsStatusAll.Add(EnumHelper<ActionStateEnum>.GetDisplayValue(actionState).ToString());
            }

            return View(vm);
        }
    }
}