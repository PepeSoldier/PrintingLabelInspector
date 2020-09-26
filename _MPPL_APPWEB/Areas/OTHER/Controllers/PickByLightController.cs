using MDL_BASE.Models.IDENTITY;
using MDL_OTHER.ComponentHSE._Interfaces;
using MDL_OTHER.ComponentJobItemConfig.UnitOfWorks;
using MDL_OTHER.ComponentPickByLight._Interfaces;
using MDL_OTHER.ComponentPickByLight.Entities;
using MDL_OTHER.ComponentPickByLight.UnitOfWorks;
using MDL_OTHER.ComponentVisualControl.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.OTHER.Controllers
{
    public class PickByLightController : Controller
    {
        IDbContextPickByLight db;
        UnitOfWorkPickByLight uow;

        public PickByLightController(IDbContextPickByLight db)
        {
            this.db = db;
            uow = new UnitOfWorkPickByLight(db);
        }

        public ActionResult Config()
        {
            return View();
        }

        public ActionResult Tester()
        {
            return View();
        }

        public ActionResult PickByLightInstance()
        {
            return View();
        }
        [HttpPost] //, Authorize(Roles = DefRoles.Engineer)]
        public ActionResult PickByLightInstanceDelete(int id)
        {
            PickByLightInstance item = uow.PickByLightInstanceRepo.GetById(id);
            uow.PickByLightInstanceRepo.Delete(item);
            return Json("");
        }
        [HttpPost] //, Authorize(Roles = DefRoles.Engineer)]
        public JsonResult PickByLightInstanceUpdate(PickByLightInstance item)
        {
            item.LastChange = DateTime.Now;
            item.UserName = User.Identity.GetUserName();
            uow.PickByLightInstanceRepo.AddOrUpdate(item);
            return Json(item);
        }
        [HttpPost]
        public JsonResult PickByLightInstanceGetList(PickByLightInstance filter, int pageIndex, int pageSize)
        {
            IQueryable<PickByLightInstance> query = uow.PickByLightInstanceRepo.GetListFiltered(filter);
            int startIndex = (pageIndex - 1) * pageSize;
            int itemsCount = query.Count();
            List<PickByLightInstance> items = query.Skip(startIndex).Take(pageSize).ToList();

            return Json(new { data = items, itemsCount });
        }

        public ActionResult PickByLightInstanceElement(int pickByLightInstanceId)
        {
            return View(pickByLightInstanceId);
        }
        [HttpPost] //, Authorize(Roles = DefRoles.Engineer)]
        public ActionResult PickByLightInstanceElementDelete(int id)
        {
            PickByLightInstanceElement item = uow.PickByLightInstanceElementRepo.GetById(id);
            uow.PickByLightInstanceElementRepo.Delete(item);
            return Json("");
        }
        [HttpPost] //, Authorize(Roles = DefRoles.Engineer)]
        public JsonResult PickByLightInstanceElementUpdate(PickByLightInstanceElement item)
        {
            item.LastChange = DateTime.Now;
            item.UserName = User.Identity.GetUserName();
            uow.PickByLightInstanceElementRepo.AddOrUpdate(item);
            return Json(item);
        }
        [HttpPost]
        public JsonResult PickByLightInstanceElementGetList(PickByLightInstanceElement filter, int pageIndex, int pageSize)
        {
            IQueryable<PickByLightInstanceElement> query = uow.PickByLightInstanceElementRepo.GetListFiltered(filter);
            int startIndex = (pageIndex - 1) * pageSize;
            int itemsCount = query.Count();
            List<PickByLightInstanceElement> items = query.Skip(startIndex).Take(pageSize).ToList();

            return Json(new { data = items, itemsCount });
        }


    }
}