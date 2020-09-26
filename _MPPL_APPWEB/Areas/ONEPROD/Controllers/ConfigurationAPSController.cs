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

namespace _MPPL_WEB_START.Areas.ONEPROD.Controllers
{
    [Authorize(Roles = DefRoles.ONEPROD_ADMIN)]
    public class ConfigurationAPSController : Controller
    {
        IDbContextOneprodAPS db;
        RepoPreprodConf repoConf;
        UnitOfWorkOneprodAPS uowAPS;

        public ConfigurationAPSController(IDbContextOneprodAPS db4)
        {
            this.db = db4;
            uowAPS = new UnitOfWorkOneprodAPS(db4);
            repoConf = new RepoPreprodConf(db);
            ViewBag.Skin = "nasaSkin";
        }


        //Tools
        public ActionResult Tool()
        {
            ToolViewModel vm = new ToolViewModel();
            vm.Tools = uowAPS.ToolRepo.GetTools().ToList();
            vm.ToolGroups = uowAPS.ToolRepo.GetToolGroups();
            
            foreach(Tool t in vm.Tools)
            {
                t.NM_ItemGroupsCount = uowAPS.ItemGroupToolRepo.GetItemGroupToolCount(t.Id);
            }

            return View(vm);
        }
        [HttpPost]
        public ActionResult ToolAdd(ToolViewModel vm)
        {
            Tool t = vm.NewObject;

            uowAPS.ToolRepo.AddOrUpdate(t);
            return RedirectToAction("Tool");
        }
        public ActionResult ToolDelete(int id)
        {
            uowAPS.ToolRepo.DeleteTool(id);
            return RedirectToAction("Tool");
        }
        public JsonResult ToolUpdate(Tool machine)
        {
            uowAPS.ToolRepo.AddOrUpdate(machine);
            return Json("zapisano");
        }

        public ActionResult ToolGroupAdd(ToolGroup toolGroup)
        {
            repoConf.AddOrUpdateObject(toolGroup);
            return RedirectToAction("Tool");
        }
        public JsonResult ToolGroupUpdate(ToolGroup toolGroup)
        {
            repoConf.AddOrUpdateObject(toolGroup);
            return Json("zapisano");
        }
        public ActionResult ToolGroupDelete(int id)
        {
            return RedirectToAction("Tool");
        }
        [HttpPost]
        public JsonResult ToolGetList()
        {
            return Json(uowAPS.ToolRepo.GetList().ToList());
        }
        public ActionResult ToolMatrix()
        {
            ToolMatrixViewModel vm = new ToolMatrixViewModel();
            vm.Tools = new List<Tool>(uowAPS.ToolRepo.GetTools());
            vm.Tools2 = new List<Tool>(uowAPS.ToolRepo.GetTools());
            vm.ToolChangeOvers = uowAPS.ToolChangeOverRepo.GetToolChangeOvers();

            return View(vm);
        }
        [HttpPost]
        public JsonResult ToolMatrixUpdate(ToolChangeOver obj)
        {
            int id = uowAPS.ToolChangeOverRepo.UpdateToolChangeOver(obj.Tool1Id, (int)obj.Tool2Id, obj.Time);
            return Json(id);
        }
        
        //ToolMachine
        public ActionResult ToolMachine()
        {
            ToolMachineViewModel vm = new ToolMachineViewModel();
            vm.Machines = uowAPS.ResourceRepo.GetResources().Where(m=>m.ToolRequired == true).ToList();
            vm.Tools = uowAPS.ToolRepo.GetTools().ToList();
            vm.ToolMachines = uowAPS.ToolMachineRepo.GetList().ToList();

            return View(vm);
        }
        public JsonResult ToolMachineUpdate(ToolMachineViewModel vm)
        {
            //if (vm.ToolId != null && vm.MachineId != null && vm.Assigned != null && vm.Preffered != null && vm.Placed != null)
            //{
                if(vm.ToolId > 0 && vm.MachineId > 0)
                {
                    uowAPS.ToolMachineRepo.ToolMachineUpdate(vm.MachineId, vm.ToolId, vm.Assigned, vm.Preffered, vm.Placed);
                }
            //}
            return Json("");
        }

        //ItemGroupTools
        public ActionResult ItemGroupTool(int id)
        {
            ItemGroupToolViewModel vm = new ItemGroupToolViewModel();
            vm.ItemGroup = uowAPS.ItemGroupRepo.GetById(id);
            vm.Tools = new SelectList(uowAPS.ToolRepo.GetTools().ToList(), "Id", "Name");
            vm.ItemGroupTools = uowAPS.ItemGroupToolRepo.GetItemGroupTools(id);
            return View(vm);
        }
        public JsonResult ItemGroupToolAdd(ItemGroupToolViewModel vm)
        {
            uowAPS.ItemGroupToolRepo.AddItemGroupTool(vm.ItemGroup.Id, vm.SelectedToolId);
            return Json(vm.ItemGroup.Id);
        }
        [HttpPost]
        public JsonResult ItemGroupToolUpdate(ItemGroupTool Item)
        {
             Item.Id = uowAPS.ItemGroupToolRepo.AddItemGroupTool(Item.ItemGroupId, Item.ToolId, Item.Id);
            return Json(Item);
        }
        [HttpPost]
        public ActionResult ItemGroupToolAdd2(ItemGroupToolViewModel vm)
        {
            uowAPS.ItemGroupToolRepo.AddItemGroupTool(vm.ItemGroup.Id, vm.SelectedToolId);
            return Json(vm.ItemGroup.Id);
        }
        public JsonResult ItemGroupToolDelete(int id)
        {
            int partCategoryId = uowAPS.ItemGroupToolRepo.DeleteItemGroupTool(id);
            return Json(partCategoryId);
        }
        [HttpPost]
        public JsonResult ItemGroupToolDelete(ItemGroupTool Item)
        {
            uowAPS.ItemGroupToolRepo.DeleteItemGroupTool(Item.Id);
            return Json(Item);
        }
        [HttpPost]
        public JsonResult ItemGroupToolGetList(int filterVal)
        {
            return Json(uowAPS.ItemGroupToolRepo.GetList(filterVal).ToList());
        }


        //changeOvers
        public ActionResult ChangeOver()
        {
            ChangeOverViewModel vm = new ChangeOverViewModel();
            vm.ChangeOvers = uowAPS.ChangeOverRepo.GetList().ToList();

            return View(vm);
        }
        public ActionResult ChangeOverUpdate(ChangeOver obj)
        {
            ChangeOver cho = uowAPS.ChangeOverRepo.GetById(obj.Id);

            cho.AncChange = obj.AncChange;
            cho.CatergoyChange = obj.CatergoyChange;
            cho.MachineToolChange = obj.MachineToolChange;
            cho.ToolChange = obj.ToolChange;
            cho.ToolModification = obj.ToolModification;

            uowAPS.ChangeOverRepo.AddOrUpdate(cho);

            return RedirectToAction("ChangeOver");
        }

        
        
    }
}