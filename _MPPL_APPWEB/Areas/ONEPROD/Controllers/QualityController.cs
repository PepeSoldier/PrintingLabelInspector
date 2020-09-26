using MDL_BASE.Models.IDENTITY;
using MDL_iLOGIS.ComponentConfig.Mappers;
using MDL_iLOGIS.ComponentWMS.Mappers;
using MDL_ONEPROD.ComponentQuality._Interfaces;
using MDL_ONEPROD.ComponentQuality.Entities;
using MDL_ONEPROD.ComponentQuality.UnitOfWorks;
using MDL_ONEPROD.ComponentQuality.ViewModels;

//using MDL_ONEPROD.ComponentMes.ViewModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.ONEPROD.Controllers
{
    [System.Web.Mvc.Authorize(Roles = DefRoles.ONEPROD_MES_OPERATOR)]
    public partial class QualityController : BaseController
    {
        private readonly IDbContextOneprodQuality db;
        private UnitOfWorkOneProdQuality uow;

        public QualityController(IDbContextOneprodQuality db)
        {
            this.db = db;
            this.uow = new UnitOfWorkOneProdQuality(db);
            ViewBag.Skin = "nasaSkin";
        }

        public ActionResult ItemParameter()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ItemParameterDelete(int id)
        {
            uow.ItemRepo.SetDeleted(id);
            return Json(0);
        }

        [HttpPost]
        public JsonResult ItemParameterUpdate(ItemParameter item)
        {
            uow.ItemParameterRepo.AddOrUpdate(item);
            return Json(item);
        }

        [HttpPost]
        public JsonResult ItemParameterGetList(ItemParameterViewModel filter, int pageIndex = 1, int pageSize = 20)
        {
            IQueryable<ItemParameter> query = uow.ItemParameterRepo.GetList();
            int startIndex = (pageIndex - 1) * pageSize;
            int itemsCount = query.Count();

            List<ItemParameterViewModel> list = query.Skip(startIndex).Take(pageSize).ToList<ItemParameterViewModel>();

            return Json(new { data = list, itemsCount });
        }

        public ActionResult ItemMeasurement()
        {
            return View();
        }

        public JsonResult ItemMeasurementGetList(ItemMeasurementViewModel filter, int pageIndex = 1, int pageSize = 20)
        {
            IQueryable<ItemMeasurement> query = uow.ItemMeasurementRepo.GetList(filter);
            int startIndex = (pageIndex - 1) * pageSize;
            int itemsCount = query.Count();

            List<ItemMeasurementViewModel> list = query.Skip(startIndex).Take(pageSize).ToList<ItemMeasurementViewModel>();

            return Json(new { data = list, itemsCount });
        }
    }
}