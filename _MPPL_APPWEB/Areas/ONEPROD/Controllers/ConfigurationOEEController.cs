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
    public class ConfigurationOEEController : Controller
    {
        IDbContextOneProdOEE db2;
        UnitOfWorkOneProdOEE uowOEE;
        
        public ConfigurationOEEController(IDbContextOneProdOEE db2)
        {
            this.db2 = db2;
            uowOEE = new UnitOfWorkOneProdOEE(db2);
            ViewBag.Skin = "nasaSkin";
        }


        //Reasons OEE stoppages
        public ActionResult Reason()
        {
            ReasonGridViewModel vm = new ReasonGridViewModel();

            vm.Reasons = uowOEE.ReasonRepo.GetList().ToList();
            vm.Machines = uowOEE.ResourceRepo.GetList()
                .Where(x=>x.IsOEE == true && x.Type == MDLX_MASTERDATA.Enums.ResourceTypeEnum.Resource)
                .OrderBy(x=>x.ResourceGroupOP.StageNo)
                .ThenBy(x=>x.Name)
                .Select(x => new SelectListItem{
                    Value = x.Id.ToString(),
                    Text = x.Name })
                .ToList();
            vm.MachineReason = uowOEE.MachineReasonRepo.GetList().ToList()
                .Select(x => new MachineReason {
                    Id = x.Id,
                    MachineId = x.MachineId,
                    ReasonId = x.ReasonId})
                .ToList();

            return View(vm);
        }
        [HttpPost]
        public JsonResult ReasonGetList(Reason filter)
        {
            List<Reason> reasons = uowOEE.ReasonRepo.GetList()
                .Where(x=> 
                    (filter.Name == null || x.Name.Contains(filter.Name))  &&
                    //(filter.ReasonType.EntryType == EnumEntryType.Undefined || x.ReasonType.EntryType == filter.ReasonType.EntryType) &&
                    (filter.ReasonTypeId <= 0 || x.ReasonTypeId == filter.ReasonTypeId) &&
                    (filter.Color == null || x.Color == filter.Color))
                .OrderBy(x=>x.ReasonType.EntryType)
                .ThenBy(x=>x.ReasonType.Name)
                .ThenBy(x=>x.Name)
                .ToList();
            return Json(reasons);
        }
        [HttpPost]
        public JsonResult ReasonUpdate(Reason item)
        {
            Reason reason = uowOEE.ReasonRepo.GetById(item.Id);
            if(reason == null)
            {
                reason = new Reason();
            }
            reason.Name = item.Name;
            reason.NameEnglish = item.NameEnglish;
            //reason.EntryType = item.EntryType;
            reason.ReasonType = null;
            reason.ReasonTypeId = item.ReasonTypeId;
            reason.Color = item.Color;
            reason.ColorGroup = item.ColorGroup;
            reason.Deleted = false;
            reason.IsGroup = item.IsGroup;
            reason.GroupId = item.GroupId > 0 && item.GroupId != item.Id? item.GroupId : null;
            uowOEE.ReasonRepo.AddOrUpdate(reason);

            return Json(reason);
        }
        [HttpPost]
        public JsonResult ReasonDelete(Reason item)
        {
            Reason reason = uowOEE.ReasonRepo.GetById(item.Id);
            uowOEE.ReasonRepo.Delete(reason);

            return Json(0);
        }

        [HttpPost]
        public JsonResult ReasonMachineConnect(int reasonId, int machineId)
        {
            MachineReason mr = uowOEE.MachineReasonRepo.GetByReasonIdAndMachineId(reasonId, machineId);

            if (mr != null)
            {
                //mr.Deleted = !mr.Deleted;
                uowOEE.MachineReasonRepo.DeleteByReasonIdAndMachineId(reasonId, machineId);
                mr = new MachineReason();
            }
            else
            {
                mr = new MachineReason { MachineId = machineId, ReasonId = reasonId, Deleted = false };
                uowOEE.MachineReasonRepo.AddOrUpdate(mr);
            }

            return Json(mr);
        }

        [HttpPost]
        public JsonResult ReasonTypesGetList(int reasonTypeId = 0)
        {
            List<ReasonType> reasonTypes = uowOEE.ReasonTypeRepo.GetList().Where(x=>x.EntryType > EnumEntryType.Production || x.EntryType == EnumEntryType.TimeClosed).ToList();
            return Json(reasonTypes);
        }

        [HttpPost]
        public JsonResult ReasonGroupsGetList(int reasonTypeId = 0)
        {
            var reasonGroups = uowOEE.ReasonRepo.GetList()
                .Where(x => 
                    x.IsGroup == true &&
                    x.Deleted == false
                )
                .Select(x => new { 
                    Id = x.Id,
                    Name = x.Name + " [" + x.ReasonType.Name + "]"
                })
                .ToList();
            return Json(reasonGroups);
        }

        public ActionResult ResourceTarget(int machineId)
        {
            return View(machineId);
        }
        [HttpPost]
        public JsonResult ResourceTargetGetList(MachineTarget filter)
        {
            List<MachineTarget> targets = uowOEE.MachineTargetRepo.GetList()
                .Where(x => (filter.ResourceId <= 0 || x.ResourceId == filter.ResourceId))
                .OrderBy(x => x.ReasonTypeId)
                .ToList();
            return Json(targets);
        }
        [HttpPost]
        public JsonResult ResourceTargetUpdate(MachineTarget item)
        {
            MachineTarget machineTarget = uowOEE.MachineTargetRepo.GetById(item.Id);
            if (machineTarget == null)
            {
                machineTarget = uowOEE.MachineTargetRepo.GetByResourceIdAndReasonTypeId(item.ResourceId, item.ReasonTypeId);
                if (machineTarget == null)
                {
                    machineTarget = new MachineTarget();
                }
            }

            machineTarget.Deleted = false;
            machineTarget.ReasonTypeId = item.ReasonTypeId;
            machineTarget.Resource = null;
            machineTarget.ResourceId = item.ResourceId;
            machineTarget.Target = item.Target;

            uowOEE.ReasonRepo.AddOrUpdate(machineTarget);

            return Json(machineTarget);
        }
        [HttpPost]
        public JsonResult ResourceTargetDelete(MachineTarget item)
        {
            MachineTarget machineTarget = uowOEE.MachineTargetRepo.GetById(item.Id);
            uowOEE.ReasonRepo.Delete(machineTarget);

            return Json(0);
        }
    }
}