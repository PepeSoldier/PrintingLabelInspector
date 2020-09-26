using MDL_BASE.Models.IDENTITY;
using MDL_CORE.ComponentCore.Enums;
using MDL_OTHER.ComponentHSE._Interfaces;
using MDL_OTHER.ComponentHSE.Entities;
using MDL_OTHER.ComponentHSE.Repos;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XLIB_COMMON.Repo.Base;

namespace _MPPL_WEB_START.Areas.OTHER.Controllers
{
    public class HSEController : Controller
    {
        SafetyCrossRepo repo;
        IDbContextOtherHSE db;

        public HSEController(IDbContextOtherHSE db)
        {
            this.db = db;
            repo = new SafetyCrossRepo(db);
            ViewBag.Skin = "nasaSkin";
        }

        public ActionResult Cross()
        {

            return View();
        }

        [HttpPost]
        public JsonResult GetData()
        {
            DateTime dateLimit = DateTime.Now.AddMonths(-13);
            List<SafetyCross> list = repo.GetList().Where(x => x.Date > dateLimit).ToList();
            return Json(list);
        }
        [HttpPost]
        public JsonResult GetCounters()
        {
            int NumberOfDaysWithoutAccident = repo.GetNumberOfDaysWithoutAccident();
            int RecordNumberOfDaysWithoutAccident = repo.GetRecordNumberOfDaysWithoutAccident();

            return Json(new { NumberOfDaysWithoutAccident, RecordNumberOfDaysWithoutAccident });
        }

        [AuthorizeRoles(DefRoles.HSE_EDITOR)]
        public ActionResult InfoTextEditor()
        {
            return View();
        }

        [HttpPost, AuthorizeRoles(DefRoles.HSE_EDITOR)]
        public JsonResult InfoTextUpdate(string text)
        {
            SystemVariableRepo svRepo = new SystemVariableRepo(db);
            svRepo.UpdateValue("SafetyCrossInfoText", text, EnumVariableType.String);
            return Json(0);
        }
        [HttpGet]
        public JsonResult InfoTextGet()
        {
            SystemVariableRepo svRepo = new SystemVariableRepo(db);
            string text = svRepo.GetValueString("SafetyCrossInfoText");
            return Json(text, JsonRequestBehavior.AllowGet);
        }
        [HttpPost, AuthorizeRoles(DefRoles.HSE_EDITOR)]
        public JsonResult UpdateData(DateTime date, SafetyCrossState state)
        {
            date = date.Date;
            SafetyCross sc = repo.GetByDate(date);

            if(sc == null)
            {
                sc = new SafetyCross { Date = date, State = state };
            }

            //sc.UserId = User.Identity.GetUserId();
            sc.State = state;
            sc.LastUpdate = DateTime.Now;
            repo.AddOrUpdate(sc);

            return Json(sc);
        }
    }
}