using MDL_ONEPROD;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Manager;
using MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.Repo;
using _MPPL_WEB_START.Areas.ONEPROD.Base.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Model.Scheduling.Interface;
using MDL_BASE.Models.IDENTITY;
using MDL_ONEPROD.ComponetMes.Models;
using MDL_ONEPROD.ComponentWMS.UnitOfWorks;
using MDL_ONEPROD.ComponentWMS._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;

namespace _MPPL_WEB_START.Areas.ONEPROD.Controllers
{
    [Authorize(Roles = DefRoles.ONEPROD_ADMIN)]
    public class ConfigurationWMSController : Controller
    {
        IDbContextOneprodWMS db;
        //RepoPreprodConf repoConf; //= new RepoPreprodConf();
        UnitOfWorkOneprodWMS uowWMS;

        public ConfigurationWMSController(IDbContextOneprodWMS db3)
        {
            this.db = db3;
            uowWMS = new UnitOfWorkOneprodWMS(db3);
            ViewBag.Skin = "nasaSkin";
        }


        //Box
        public ActionResult Box()
        {
            BoxViewModel vm = new BoxViewModel();
            vm.Boxes = uowWMS.WarehouseRepo.GetBufforBoxes();
            
            return View(vm);
        }
        public ActionResult BoxAdd(BoxViewModel vm)
        {
            uowWMS.WarehouseRepo.AddOrUpdate(vm.NewObject);
            return RedirectToAction("Box");
        }
        public JsonResult BoxUpdate(Warehouse obj)
        {
            obj.Id = uowWMS.WarehouseRepo.AddOrUpdate(obj);
            return Json(obj);
        }
        public ActionResult BoxDelete(int id)
        {
            uowWMS.WarehouseRepo.DeleteBox(id);
            return RedirectToAction("Box");
        }
        [HttpPost]
        public JsonResult BoxGetList(Warehouse filterItem)
        {
            List<Warehouse> buffor_Boxelist = uowWMS.WarehouseRepo.GetList(filterItem).ToList();
            return Json(buffor_Boxelist);
        }

        //BoxItemGroup
        public ActionResult BoxItemGroup(int id)
        {
            BoxItemGroupViewModel vm = new BoxItemGroupViewModel();
            vm.BoxId = id;
            vm.Box = uowWMS.WarehouseRepo.GetById(id);
            vm.BoxItemGroups = uowWMS.WarehouseRepo.GetBufforBoxItemGroups(vm.BoxId);
            vm.ItemGroups = new SelectList(uowWMS.ItemGroupRepo.GetList().ToList(), "Id", "Name");

            return View(vm);
        }
        public ActionResult BoxItemGroupAdd(BoxItemGroupViewModel vm)
        {
            WarehouseItem bpc = new WarehouseItem();
            bpc.QtyPerLocation = vm.NewObject.QtyPerLocation;
            bpc.ItemGroupId = vm.SelectedItemGroupId;
            bpc.WarehouseId = vm.BoxId;

            uowWMS.WarehouseItemRepo.AddOrUpdate(bpc);
            return RedirectToAction("Box");
        }
        public ActionResult BoxItemGroupDelete(int id)
        {
            WarehouseItem bpc = uowWMS.WarehouseRepo.GetBufforBoxItemGroup(id);
            uowWMS.WarehouseItemRepo.Delete(bpc);

            return RedirectToAction("Box");
        }
        [HttpPost]
        public JsonResult BoxItemGroupUpdate(WarehouseItem Item)
        {
            WarehouseItem bpc = uowWMS.WarehouseItemRepo.GetById(Item.Id);
            if(bpc == null)
            {
                bpc = new WarehouseItem() { WarehouseId = Item.WarehouseId, HoursCoverage = Item.HoursCoverage, ItemGroupId = Item.ItemGroupId, QtyPerLocation = Item.QtyPerLocation };
               bpc.Id = uowWMS.WarehouseItemRepo.AddOrUpdate(bpc);
            }
            else
            {
                bpc.QtyPerLocation = Item.QtyPerLocation;
                bpc.Id = uowWMS.WarehouseItemRepo.AddOrUpdate(bpc);
            }
            return Json(bpc);
        }
        [HttpPost]
        public JsonResult BoxItemGroupGetList(int filterVal = 0)
        {
            if (filterVal > 0)
            {
                List<WarehouseItem> list = uowWMS.WarehouseItemRepo.GetList(filterVal).ToList();
                return Json(list);
            }
            return Json(uowWMS.WarehouseItemRepo.GetList().ToList());
        }
        
    }
}