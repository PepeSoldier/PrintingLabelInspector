using Autofac;
using Autofac.Integration.Mvc;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentConfig.Mappers;
using MDL_iLOGIS.ComponentWMS._Interfaces;
using MDL_iLOGIS.ComponentWMS.ViewModels;
using MDL_WMS.ComponentConfig.UnitOfWorks;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.iLOGIS.Controllers
{
    [Authorize]
    public class WarehouseController : Controller
    {
        private IDbContextiLOGIS db;
        private ILocataionManager locataionManager;
        private UnitOfWork_iLogis uow;

        public WarehouseController(IDbContextiLOGIS db)
        {
            ViewBag.Skin = "nasaSkin";
            this.db = db;
            this.locataionManager = AutofacDependencyResolver.Current.ApplicationContainer.Resolve<ILocataionManager>();
            uow = new UnitOfWork_iLogis(db, locataionManager);
        }

        public ActionResult WarehouseSelector()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetWarehouses(int parentWarehouseId = 0, int warehouseId = 0)
        {
            string userId = User.Identity.GetUserId();
            IQueryable<Warehouse> warehouseLocationList;

            if (warehouseId > 0)
            {
                int? parentWarehouseIdtemp = uow.WarehouseRepo.GetById(warehouseId).ParentWarehouseId;
                parentWarehouseId = parentWarehouseIdtemp != null ? (int)parentWarehouseIdtemp : 0;
                //warehouseLocationList = uow.WarehouseRepo.GetParentWarehouses(warehouseId);
            }
            
            warehouseLocationList = uow.WarehouseRepo.GetChildWarehouses(parentWarehouseId);
            

            List<WarehouseSelectorViewModel> vm = warehouseLocationList.Select(x => new WarehouseSelectorViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                ParentWarehouseId = parentWarehouseId,
                ParentWarehouseName = x.ParentWarehouse.Name,
                QtyOfSubLocations = x.QtyOfSubLocations,
            }).ToList();

            return Json(vm);
        }
    }
}