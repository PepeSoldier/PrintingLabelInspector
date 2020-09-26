
using MDL_AP.Models.ActionPlan;
using _MPPL_WEB_START.Areas.AP.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MDL_AP.Repo;
using XLIB_COMMON.Model;
using XLIB_COMMON.Interface;
using _MPPL_WEB_START.Migrations;
using XLIB_COMMON.Repo.IDENTITY;
using Microsoft.AspNet.Identity.EntityFramework;
using MDL_BASE.Models.IDENTITY;
using _MPPL_WEB_START.Areas.Models;
using MDL_AP.Interfaces;
using _MPPL_WEB_START.Areas._APPWEB.Controllers;
using _MPPL_WEB_START.Models;
using System.Web;
using Microsoft.AspNet.Identity.Owin;

namespace _MPPL_WEB_START.Areas.AP.Controllers
{
    [Authorize]
    public class ActionActivityController : BaseController
    {
        private UnitOfWorkActionPlan unitOfWork;
        private Mailer_AP mailerAp;

        DbContextAPP_ElectroluxPLV db = DbContextAPP_ElectroluxPLV.Create();

        public ActionActivityController(IDbContextAP db)
        {
            unitOfWork = new UnitOfWorkActionPlan(db);
            mailerAp = new Mailer_AP(Mailer.Create());
        }
        
        public ActionResult List(int id)
        {
            ActionActivityViewModel vm = new ActionActivityViewModel();
            vm.ActionId = id;
            return View(vm);
        }

        public PartialViewResult ListAction(int id)
        {
            ActionActivityViewModel vm = new ActionActivityViewModel();
            ActionModel action = unitOfWork.RepoAction.GetById(id);
            
            if (action != null)
            {
                vm.ActionActivities = unitOfWork.RepoActionActivity.GetList(action).ToList();
                
                foreach (ActionActivity aa in vm.ActionActivities)
                {
                    aa.Attachments = unitOfWork.RepoAttachment.GetListByActivity(aa).ToList();
                }
                return PartialView(vm);
            }

            return new PartialViewResult();
        }
        public PartialViewResult ListMeeting(int id, int meetingId = 0)
        {
            ActionActivityViewModel vm = new ActionActivityViewModel();
            ActionModel action = unitOfWork.RepoAction.GetById(id);
            vm.MeetingId = meetingId;
            vm.ActionId = id;

            if (action != null)
            {
                vm.ActionActivities = unitOfWork.RepoActionActivity.GetList(action).ToList();
                vm.ActionActivity = new ActionActivity { ActionId = vm.ActionId };

                foreach (ActionActivity aa in vm.ActionActivities)
                {
                    aa.Attachments = unitOfWork.RepoAttachment.GetListByActivity(aa).ToList();
                }
                return PartialView(vm);
            }

            return new PartialViewResult();
        }

        public ActionResult AddForm(int id, int meetingId = 0)
        {
            ActionActivityViewModel vm = new ActionActivityViewModel();
            ActionModel action = unitOfWork.RepoAction.GetById(id);
            vm.ActionId = id;
            vm.MeetingId = meetingId;

            if (action != null)
            {
                return PartialView(vm);
            }

            return new PartialViewResult();
        }
        [HttpPost] [Authorize]
        public JsonResult Add(ActionActivityViewModel vm)
        {
            ActionActivity aa = vm.ActionActivity;
            aa.CreatorId = User.Identity.GetUserId();
            aa.DateCreated = DateTime.Now;
            aa.ActionId = vm.ActionId;

            unitOfWork.RepoActionActivity.AddOrUpdate(aa);

            ActionModel action = unitOfWork.RepoAction.GetById(aa.ActionId);
            string mailReceivers = string.Empty;
            string userFullName = string.Empty;
            UserRepo _userManager = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<UserRepo>();

            mailReceivers = MailHelper.GetUserEmail(action.Creator, mailReceivers, User.Identity.Name);
            mailReceivers = MailHelper.GetUserEmail(action.Assigned, mailReceivers, User.Identity.Name);
            userFullName = _userManager.FindById(aa.CreatorId).FullName;
            //userFullName = new UserRepo(new UserStore<User>(db), db).FindById(aa.CreatorId).FullName;

            mailerAp.ActionActivityAdded(action, mailReceivers, aa.ActivityDescription, userFullName);
            AlertManager.Instance.AddAlert(AlertMessageType.info, "Dodano aktywność", User.Identity.Name);

            if (vm.MeetingId == 0)
            {
                //return RedirectToAction("Show", "Action", new { id = aa.ActionId });
                return Json(aa.ActionId);
            }
            else
            {
                return Json(aa.ActionId);
                //return RedirectToAction("Show", "Meeting", new { id = vm.metingId });
            }
        }
        [HttpPost]
        public JsonResult GetJsonForAction(int id)
        {
            ActionModel temp = unitOfWork.RepoAction.GetById(id);
            List<ActionActivity> activieties = unitOfWork.RepoActionActivity.GetList(temp).ToList();
            return Json(activieties);
        }
        [HttpPost]
        public JsonResult AddJson(int actionId, int meetingId, string description)
        {
            ActionActivity aa = new ActionActivity();
            aa.CreatorId = User.Identity.GetUserId();
            aa.DateCreated = DateTime.Now;
            aa.ActionId = actionId;
            aa.ActivityDescription = description;

            unitOfWork.RepoActionActivity.AddOrUpdate(aa);

            ActionModel action = unitOfWork.RepoAction.GetById(aa.ActionId);
            string mailReceivers = string.Empty;
            string userFullName = string.Empty;

            //mailReceivers = MailHelper.GetUserEmail(action.Creator, mailReceivers, User.Identity.Name);
            //mailReceivers = MailHelper.GetUserEmail(action.Assigned, mailReceivers, User.Identity.Name);
            //userFullName = UnitOfWorkActionPlan.UserRepo.FindById(aa.CreatorId).FullName;

            //mailerAp.ActionActivityAdded(aa.ActionId, action.Title, mailReceivers, aa.ActivityDescription, userFullName);

            return Json(aa.ActionId);
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            ActionActivity aa = unitOfWork.RepoActionActivity.GetById(id);
            int actionId = aa.ActionId;
            unitOfWork.RepoAttachment.DeleteByActivity(aa);
            unitOfWork.RepoActionActivity.Delete(aa);
            alertManager.AddAlert(AlertMessageType.info, "Aktywność została usunięta", User.Identity.Name);
            return Json(actionId);
        }
    }
}