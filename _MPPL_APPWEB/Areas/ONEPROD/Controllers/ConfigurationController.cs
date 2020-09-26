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
using MDLX_MASTERDATA.Enums;
using MDLX_MASTERDATA.Entities;
//using MDLX_MASTERDATA.Entities;

namespace _MPPL_WEB_START.Areas.ONEPROD.Controllers
{
    [Authorize(Roles = DefRoles.ONEPROD_ADMIN)]
    public class ConfigurationController : Controller
    {
        IDbContextOneprod db;
        RepoPreprodConf repoConf;
        UnitOfWorkOneprod uow;

        public ConfigurationController(IDbContextOneprod db)
        {
            this.db = db;
            uow = new UnitOfWorkOneprod(db);
            repoConf = new RepoPreprodConf(db);
            ViewBag.Skin = "nasaSkin";
        }

        public ActionResult Index()
        {
            return View();
        }

        //ResourceGroup
        public ActionResult ResourceGroup()
        {
            ResourceGroupViewModel vm = new ResourceGroupViewModel();
            vm.ResourceGroups = uow.ResourceGroupRepo.GetList().ToList();
            return View(vm);
        }
        [HttpPost]
        public ActionResult ResourceGroupDelete(ResourceOP item)
        {
            uow.ResourceGroupRepo.Delete(uow.ResourceGroupRepo.GetById(item.Id));
            return RedirectToAction("ResourceGroup");
        }
        [HttpPost]
        public JsonResult ResourceGroupUpdate(ResourceOP item)
        {
            //repoConf.AddOrUpdateObject(area);
            //area.ClientId = area.ClientId > 0 ? area.ClientId : null;
            uow.ResourceGroupRepo.AddOrUpdate(item);
            return Json(item);
        }
        [HttpPost]
        public JsonResult ResourceGroupGetList(ResourceOP filter)
        {
            List<ResourceOP> areas = uow.ResourceGroupRepo.GetList(filter).ToList();
            return Json(areas);
        }

        //Machines
        public ActionResult Resource()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ResourceDelete(ResourceOP machine)
        {
            uow.ResourceRepo.Delete(uow.ResourceRepo.GetById(machine.Id));
            //return RedirectToAction("Resource");
            return Json(0);
        }
        [HttpPost]
        public JsonResult ResourceUpdate(ResourceOP machine)
        {
            machine.ResourceGroupOP = null;
            machine.ResourceGroup = null;
            machine.ResourceGroupId = machine.ResourceGroupId < 1 ? null : machine.ResourceGroupId;
            uow.ResourceRepo.AddOrUpdate(machine);
            return Json(machine);
        }
        [HttpPost]
        public JsonResult ResourceGetList(ResourceOP filterItem,int areaID = 0)
        {
            var query = uow.ResourceRepo.GetAllTypesList(filterItem, areaID);
            List<ResourceOP> machineList = ((IEnumerable<ResourceOP>)query).ToList();
            return Json(machineList);
        }

        [HttpGet]
        public JsonResult ResourceGetList()
        {
            List<ResourceOP> machineList = uow.ResourceRepo.GetList().ToList();
            return Json(machineList);
        }

        //Parts
        public ActionResult Item()
        {
            ItemViewModel vm = new ItemViewModel();
            vm.ResourceGroups = new SelectList(uow.ResourceGroupRepo.GetList().ToList(), "Id", "Name");
            return View(vm);
        }
        [HttpPost]
        public ActionResult ItemDelete(int id)
        {
            uow.ItemRepo.SetDeleted(id);
            return Json(0);
        }
        [HttpPost]
        public JsonResult ItemUpdate(ItemOP item)
        {
            Item itemMD = uow.ItemRepo.GetByCode(item.Code);

            if (itemMD == null)
                itemMD = uow.ItemRepo.GetById(item.Id);

            if (itemMD != null)
            {
                ItemOP itemOP = uow.ItemOPRepo.GetById(itemMD.Id);

                if(itemOP == null)
                {
                    itemOP = new ItemOP();
                    itemOP.Id = itemMD.Id;
                    itemOP.MinBatch = 0;
                    itemOP.WorkOrderGenerator = item.WorkOrderGenerator;

                    uow.ItemOPRepo.ManualInsert(itemOP.Id, 1, item.WorkOrderGenerator);
                    uow.ItemRepo.Detach(itemMD);
                    //uow.ItemOPRepo.Attach(itemOP);
                    //uow.ItemOPRepo.Update(itemOP);
                    itemOP = uow.ItemOPRepo.GetById(itemMD.Id);
                }
                
                itemOP.WorkOrderGenerator = item.WorkOrderGenerator;
                
                itemOP.ItemGroupOP = null;
                itemOP.ItemGroup = null;
                itemOP.ItemGroupId = item.ItemGroupId > 0 ? item.ItemGroupId : null;

                itemOP.ResourceGroup = null;
                itemOP.ResourceGroupOP = null;
                itemOP.ResourceGroupId = item.ResourceGroupId > 0 ? item.ResourceGroupId : null;

                itemOP.Type = item.Type;
                itemOP.Color2 = item.Color2;
                //itemOP.Code = item.Code;
                //itemOP.Name = item.GetName;

                uow.ItemOPRepo.AddOrUpdate(itemOP);
                item.Id = itemOP.Id;
            }
            return Json(item);
        }
        [HttpPost]
        public JsonResult ItemGetList(ItemOP item, int pageIndex = 1, int pageSize = 9000)
        {
            IQueryable<ItemOP> query = uow.ItemOPRepo.GetList(item);

            int startIndex = (pageIndex - 1) * pageSize;
            int itemsCount = query.Count();

            List<ItemOP> ItemList = query.Skip(startIndex).Take(pageSize).ToList().Select(x =>
            new ItemOP {
                Id = x.Id,
                Color = x.Color,
                Color1 = x.Color1,
                Color2 = x.Color2,
                ItemGroupId = x.ItemGroupId,
                ItemGroup = x.ItemGroup,
                ResourceGroupId = x.ResourceGroupId,
                ResourceGroup = x.ResourceGroup,
                Type = x.Type,
                WorkOrderGenerator = x.WorkOrderGenerator,
                Code = x.Code,
                OriginalName = x.OriginalName,
                Name = x.Name
            }).ToList();

            ItemList.ForEach(x => { x.ItemGroupOP = null; });
            return Json(new { data = ItemList, itemsCount });
        }

        //ItemGroups
        public ActionResult ItemGroup()
        {           
            return View();
        }
        [HttpPost]
        public ActionResult ItemGroupDelete(int id)
        {
            ItemOP pc1 = uow.ItemGroupRepo.GetById(id);

            List<ItemOP> items = uow.ItemOPRepo.GetListByGroup(pc1.Id).ToList();
            
            foreach(ItemOP item in items)
            {
                item.ItemGroupOP = null;
                item.ItemGroupId = null;
                uow.ItemOPRepo.AddOrUpdate(item);
            }

            uow.ItemGroupRepo.Delete(pc1);
            return Json(0);
        }
        [HttpPost]
        public JsonResult ItemGroupUpdate(ItemOP obj)
        {
            ItemOP itemOrg = uow.ItemOPRepo.GetById(obj.Id);
            itemOrg = itemOrg == null ? new ItemOP() { CreatedDate = DateTime.Now, StartDate = DateTime.Now } : itemOrg;

            itemOrg.Name = obj.Name;
            itemOrg.ResourceGroup = null;
            itemOrg.ResourceGroupOP = null;
            itemOrg.ResourceGroupId = obj.ResourceGroupId > 0? obj.ResourceGroupId : null;
            itemOrg.Process = null;
            itemOrg.ProcessId = obj.ProcessId;
            itemOrg.Type = ItemTypeEnum.ItemGroup;
            itemOrg.Color = obj.Color;
            itemOrg.Color2 = obj.Color2;
            itemOrg.MinBatch = obj.MinBatch;

            uow.ItemGroupRepo.AddOrUpdate(itemOrg);
            obj.Id = itemOrg.Id;
            return Json(obj);
        }
        [HttpPost]
        public JsonResult ItemGroupGetList(ItemOP part)
        {
            part.Type = ItemTypeEnum.ItemGroup;
            List<ItemOP> ItemGroups = new List<ItemOP>();
            ItemGroups = uow.ItemGroupRepo.GetList(part);
            AssignItemGroups(ItemGroups);
            return Json(ItemGroups);
        }
        private void AssignItemGroups(List<ItemOP> ItemGroups)
        {
            foreach (ItemOP pc in ItemGroups)
            {
                if (pc.Type == ItemTypeEnum.ItemGroup && !pc.Deleted)
                {
                    pc.NM_BoxCount = -1; //uowWMS.Buffor_BoxRepo.GetBufforBoxItemGroupCount(pc.Id); //TODO: 20190316 pobrać jakoś tą wartość? brak dostępu do WMS!
                    pc.NM_CycleTimeCount = uow.CycleTimeRepo.GetCycleTimeCount(pc.Id);
                    pc.NM_PartCount = uow.ItemRepo.GetCounter(pc.Id);
                    pc.NM_ToolCount = -1;//uowAPS.ToolRepo.GetToolCount(pc.Id); //TODO: 20190316 pobrać jakoś tą wartość? brak dostępu do APS!
                }
            }
        }

        //ItemGroups Group
        public ActionResult Process()
        {
            return View();
        }
        [HttpPost]
        public JsonResult ProcessGetList(MDLX_MASTERDATA.Entities.Process item)
        {
            var list = uow.ProcessRepo.GetList(item).OrderBy(x=>x.ParentId).ToList();
            return Json(list);
        }
        [HttpPost]
        public JsonResult ProcessUpdate(MDLX_MASTERDATA.Entities.Process item)
        {
            uow.ProcessRepo.AddOrUpdate(item);
            return Json(0);
        }
        [HttpPost]
        public JsonResult ProcessDelete(MDLX_MASTERDATA.Entities.Process item)
        {
            item.Deleted = true;
            uow.ProcessRepo.AddOrUpdate(item);
            return Json(0);
        }


        //Part Category Cycle time
        public ActionResult ItemGroupCycleTime(int id = 0)
        {
            CycleTimeViewModel vm = new CycleTimeViewModel();
            vm.SelectedItemGroupId = id;
            vm.CycleTimes = new List<MCycleTime>();

            if (vm.SelectedItemGroupId > 0)
            {
                vm.CycleTimes = uow.CycleTimeRepo.GetCycleTimesByItemGroup(vm.SelectedItemGroupId);
                vm.SelectedItemGroupName = uow.ItemGroupRepo.GetById(vm.SelectedItemGroupId).Name;
            }

            ItemOP pc = uow.ItemGroupRepo.GetById(id);
            int? areaId = pc != null? pc.ResourceGroupId : null;
            vm.Machines = new SelectList(uow.ResourceRepo.GetResources().Where(x=> areaId == null || x.ResourceGroupId == areaId), "Id", "Name");
            vm.ItemGroups = new SelectList(uow.ItemGroupRepo.GetList().ToList(), "Id", "Name");
            vm.areaId = areaId;
            return View(vm);
        }
        public JsonResult ItemGroupCycleTimeAdd(CycleTimeViewModel vm)
        {
            MCycleTime ct = uow.CycleTimeRepo.GetList().Where(x =>
                x.MachineId == vm.SelectedMachineId && x.ItemGroupId == vm.SelectedItemGroupId)
                .FirstOrDefault();

            if (ct == null)
            {
                ct = new MCycleTime();
            }

            ct.ItemGroupId = vm.SelectedItemGroupId;
            ct.MachineId = vm.SelectedMachineId;
            ct.CycleTime = vm.NewObject.CycleTime;
            ct.Preferred = vm.NewObject.Preferred;
            ct.ProgramNumber = vm.ProgramNumber;
            ct.ProgramName = vm.ProgramName;

            if (ct.MachineId > 0 && ct.ItemGroupId > 0)
            {
                uow.CycleTimeRepo.AddOrUpdate(ct);
            }
            return Json(vm.SelectedItemGroupId);
        }
        public JsonResult ItemGroupCycleTimeDelete(int id)
        {
            int partCategoryId = uow.CycleTimeRepo.DeleteCycleTime(id);
            return Json(partCategoryId);
        }
        [HttpPost]
        public JsonResult ItemGroupCycleTimeUpdate(MCycleTime Item)
        {
            MCycleTime ct = uow.CycleTimeRepo.GetList().Where(x =>
               x.MachineId == Item.MachineId && x.ItemGroupId == Item.ItemGroupId)
               .FirstOrDefault();
            
            if (ct == null)
            {
                ct = new MCycleTime();
            }

            ct.MachineId = Item.MachineId;
            ct.ItemGroupId = Item.ItemGroupId;
            ct.Preferred = Item.Preferred;
            ct.Active = Item.Active;
            ct.CycleTime = Item.CycleTime;
            ct.ProgramNumber = Item.ProgramNumber;
            ct.ProgramName = Item.ProgramName;
            ct.PiecesPerPallet = Item.PiecesPerPallet;

            uow.CycleTimeRepo.AddorUpdateItemGroupCycleTime(ct);

            return Json(Item);
        }
        public JsonResult ItemGroupCycleTimeGetList(MCycleTime filter,int filterVal = -1)
        {
            List<MCycleTime> mCycleTimeList = uow.CycleTimeRepo.GetList(filter, filterVal).ToList();
            return Json(mCycleTimeList);
        }

        //cycleTime
        public ActionResult CycleTime(int id = 0)
        {
            CycleTimeViewModel vm = new CycleTimeViewModel();
            vm.SelectedMachineId = id;
            vm.CycleTimes = new List<MCycleTime>();

            if (vm.SelectedMachineId > 0)
            {
                vm.CycleTimes = uow.CycleTimeRepo.GetCycleTimesByMachine(vm.SelectedMachineId);
            }

            vm.Machines = new SelectList(uow.ResourceRepo.GetResources(), "Id", "Name");
            vm.ItemGroups = new SelectList(uow.ItemGroupRepo.GetList().ToList(), "Id", "Name");
            return View(vm);
        }
        [HttpPost]
        public ActionResult CycleTime(CycleTimeViewModel vm)
        {
            return RedirectToAction("CycleTime", new { id = vm.SelectedMachineId } );
        }
        public ActionResult CycleTimeAdd(CycleTimeViewModel vm)
        {
            MCycleTime ct = new MCycleTime();
            ct.ItemGroupId = vm.SelectedItemGroupId;
            ct.MachineId = vm.SelectedMachineId;
            ct.CycleTime = vm.NewObject.CycleTime;
            ct.Preferred = vm.NewObject.Preferred;
            
            if (ct.MachineId > 0 && ct.ItemGroupId > 0)
            {
                uow.CycleTimeRepo.AddOrUpdate(ct);
            }
            
            return RedirectToAction("CycleTime", new { id = vm.SelectedMachineId });
        }
        public ActionResult CycleTimeDelete(int id)
        {
            MCycleTime ct = new MCycleTime();

            if(id > 0)
            {
                ct = uow.CycleTimeRepo.GetCycleTime(id);
                if (ct != null)
                {
                    uow.CycleTimeRepo.Delete(ct);
                    return RedirectToAction("CycleTime", new { id = ct.MachineId });
                }
            }

            return RedirectToAction("CycleTime");
        }
        public JsonResult CycleTimeUpdate(MCycleTime obj)
        {
            MCycleTime ct = uow.CycleTimeRepo.GetCycleTime(obj.Id);

            if(ct != null)
            {
                ct.CycleTime = obj.CycleTime;
                uow.CycleTimeRepo.AddOrUpdate(ct);
            }
            
            return Json("zapisano");
        }
        
    }
}