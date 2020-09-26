using MDL_OTHER.ComponentHSE._Interfaces;
using MDL_OTHER.ComponentJobItemConfig.UnitOfWorks;
using MDL_OTHER.ComponentVisualControl.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.OTHER.Controllers
{
    public class VisualControlController : Controller
    {
        IDbContextVisualControl db;

        public VisualControlController(IDbContextVisualControl db)
        {
            this.db = db;
        }

        public ActionResult Config()
        {
            return View();
        }

        public ActionResult Tester()
        {
            return View();
        }

        [HttpGet]
        public JsonResult TesterGetData(string itemCode)
        {
            UnitOfWorkVisualControl uow = new UnitOfWorkVisualControl(db);
            List<JobItemConfig> JobsForItem = uow.JobItemConfigRepo.GetJOBsByItemCode(itemCode);
            List<JobItemConfig> JobsList = uow.JobItemConfigRepo.GetList().ToList();

            foreach(JobItemConfig j in JobsForItem)
            {
                JobItemConfig job = JobsList.FirstOrDefault(x => x.CameraNo == j.CameraNo && x.PairNo == j.PairNo);
                if(job != null)
                {
                    j.Description = job.Description;
                    j.ItemCode = job.Item.Code;
                    j.ItemName = job.Item.Name;
                }
            }

            return Json(JobsForItem, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult JobItemConfigGetList(JobItemConfig filter)
        {
            UnitOfWorkVisualControl uow = new UnitOfWorkVisualControl(db);

            var list = uow.JobItemConfigRepo.GetListFiltered(filter).Select(x => 
                new { Id = x.Id,
                    ItemId = x.ItemId,
                    ItemCode = x.Item.Code,
                    ItemName = x.Item.Name,
                    x.Description,
                    x.JobNo,
                    x.PairNo,
                    x.CameraNo,
                    x.Location,
                    x.Type
                })
                .ToList();

            return Json(list);
        }
        [HttpPost]
        public JsonResult JobItemConfigUpdate(JobItemConfig editedItem)
        {
            UnitOfWorkVisualControl uow = new UnitOfWorkVisualControl(db);
            int id = uow.JobItemConfigRepo.AddOrUpdate(editedItem);

            //JobItemConfig item2 = uow.JobItemConfigRepo.GetById(id);
            //JobItemConfig item3 = uow.JobItemConfigRepo.GetById(11);

            //var itemView = new
            //{
            //    Id = item2.Id,
            //    ItemCode = item2.Item.Code,
            //    ItemName = item2.Item.Name,
            //    item2.JobNo,
            //    item2.PairNo,
            //    item2.CameraNo,
            //    item2.Location,
            //    item2.Type
            //};

            return Json(editedItem);
        }
        [HttpPost]
        public JsonResult JobItemConfigDelete(JobItemConfig item)
        {
            UnitOfWorkVisualControl uow = new UnitOfWorkVisualControl(db);
            uow.JobItemConfigRepo.Delete(item);
            return Json(0);
        }


    }
}