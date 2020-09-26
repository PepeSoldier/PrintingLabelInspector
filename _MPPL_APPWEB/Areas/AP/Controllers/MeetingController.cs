using MDL_AP.Models;
using MDL_AP.Models.ActionPlan;
using MDL_BASE.Models.IDENTITY;
using XLIB_COMMON.Repo;
using _MPPL_WEB_START.Areas.AP.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _MPPL_WEB_START.Areas._APPWEB.Controllers;
using MDL_AP.Repo;
using XLIB_COMMON.Repo.IDENTITY;
using _MPPL_WEB_START.Migrations;
using _MPPL_WEB_START.Models;
using MDL_AP.Interfaces;

namespace _MPPL_WEB_START.Areas.AP.Controllers
{
    [Authorize]
    public class MeetingController : BaseController
    {
        private IDbContextAP db;
        private UnitOfWorkActionPlan unitOfWork;
        private UserRepo UserManager;

        public MeetingController(IDbContextAP db, IUserStore<User, string> userStore)
        {
            this.db = db;
            unitOfWork = new UnitOfWorkActionPlan(db);
            UserManager = new UserRepo(userStore, db);
            LicenceManager.CheckMeetingLicence();
        }

        public ActionResult Show(int id)
        {
            MeetingViewModel vm = new MeetingViewModel();
            vm.NewObject = unitOfWork.RepoMeeting.GetById(id);
            vm.ActionList = unitOfWork.RepoMeeting.GetActions(id).Where(x => x.ParentActionId == 0);
            return View(vm);
        }

        // GET: ActionObserver/Details/5
        public ActionResult Browse()
        {
            MeetingViewModel vm = new MeetingViewModel();
            vm.MeetingList = unitOfWork.RepoMeeting.GetList();
            vm.NewObject = new Meeting();
            return View(vm);
        }
        
        // GET: ActionObserver/Edit/5
        public ActionResult Edit(int id)
        {
            MeetingViewModel vm = new MeetingViewModel();
            return View(vm);
        }

        // POST: ActionObserver/Edit/5
        [HttpPost]
        public ActionResult Edit(MeetingViewModel vm)
        {
            vm.NewObject.OwnerId = User.Identity.GetUserId();
            unitOfWork.RepoMeeting.AddOrUpdate(vm.NewObject);
            return RedirectToAction("Browse");
        }

        public JsonResult AddAction(int actionId, int meetingId)
        {
            unitOfWork.RepoMeeting.AddActionToMeeting(actionId, meetingId);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        
        // GET: ActionObserver/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ActionObserver/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
