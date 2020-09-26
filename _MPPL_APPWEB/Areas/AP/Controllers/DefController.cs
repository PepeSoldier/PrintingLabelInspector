using MDL_BASE.Interfaces;

using MDL_AP.Models.DEF;
using XLIB_COMMON.Repo;

using _MPPL_WEB_START.Areas.AP.ViewModel.Def;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _MPPL_WEB_START.Areas._APPWEB.Controllers;
using MDL_AP.Repo;
using MDL_AP.Interfaces;

using _MPPL_WEB_START.Migrations;
using MDL_BASE.Models.MasterData;
using XLIB_COMMON.Interface;
using MDLX_MASTERDATA.Entities;
using MDL_BASE.ComponentBase.Entities;

namespace _MPPL_WEB_START.Areas.AP.Controllers
{
    [Authorize]
    public class DefController : BaseController
    {
        UnitOfWorkActionPlan unitOfWork;

        public DefController(IDbContextAP db)
        {
            unitOfWork = new UnitOfWorkActionPlan(db);
        }

        //-------------------------------------------------------DEF MANAGE
        public ActionResult DefManage(int? id)
        {
            Response.AppendHeader("Access-Control-Allow-Origin", "*");

            IDefManageViewModel vm = new DefViewModel();
            
            switch (id)
            {
                case 1: vm.ObjectList = new List<IDefModel>(unitOfWork.RepoArea.GetList()); break;
                case 2: vm.ObjectList = new List<IDefModel>(unitOfWork.RepoDepartment.GetList()); break;
                case 4: vm.ObjectList = new List<IDefModel>(unitOfWork.RepoShiftCode.GetList()); break;
                case 6: vm.ObjectList = new List<IDefModel>(unitOfWork.RepoCategory.GetList()); break;
                case 7: vm.ObjectList = new List<IDefModel>(unitOfWork.RepoType.GetList()); break;
            }

            if (vm != null)
            {
                return View(vm);
            }

            return RedirectToAction("Index", new { controller = "Home" });
        }
        [HttpPost]
        public ActionResult DefManage(int id, string Name)
        {
            Response.AppendHeader("Access-Control-Allow-Origin", "*");

            switch (id)
            {
                case 1: unitOfWork.RepoCommon.Add(new Area { Name = Name }); break;
                case 2: unitOfWork.RepoCommon.Add(new Department { Name = Name }); break;
                case 3: unitOfWork.RepoCommon.Add(new Resource2 { Name = Name }); break;
                case 4: unitOfWork.RepoCommon.Add(new LabourBrigade { Name = Name }); break;
                case 5: unitOfWork.RepoCommon.Add(new Workstation { Name = Name }); break;
                case 6: unitOfWork.RepoCommon.Add(new Category { Name = Name }); break;
                case 7: unitOfWork.RepoCommon.Add(new MDL_AP.Models.DEF.Type { Name = Name }); break;
            }
            
            return RedirectToAction("DefManage", new { id = id });
        }
        [HttpPost]
        public ActionResult DeleteDefManage(int id, int typeId)
        {
            switch (typeId)
            {
                case 1: unitOfWork.RepoCommon.MakeDeleted(unitOfWork.RepoArea.GetById(id)); break;
                case 2: unitOfWork.RepoCommon.MakeDeleted(unitOfWork.RepoDepartment.GetById(id)); break;
                case 3: unitOfWork.RepoCommon.MakeDeleted(unitOfWork.ResourceRepo.GetById(id)); break;
                case 4: unitOfWork.RepoCommon.MakeDeleted(unitOfWork.RepoShiftCode.GetById(id)); break;
                case 5: unitOfWork.RepoCommon.MakeDeleted(unitOfWork.RepoWorkstation.GetById(id)); break;
                case 6: unitOfWork.RepoCommon.MakeDeleted(unitOfWork.RepoCategory.GetById(id)); break;
                case 7: unitOfWork.RepoCommon.MakeDeleted(unitOfWork.RepoType.GetById(id)); break;
            }
            
            return RedirectToAction("DefManage", new { id = typeId });
        }
        [HttpPost]
        public JsonResult EditDefManage(int typeId, int id, string Name)
        {
            Response.AppendHeader("Access-Control-Allow-Origin", "*");

            IDefModel me = null;

            switch (typeId)
            {
                case 1: me = unitOfWork.RepoArea.GetById(id); break;
                case 2: me = unitOfWork.RepoDepartment.GetById(id); break;
                case 3: me = unitOfWork.ResourceRepo.GetById(id); break;
                case 4: me = unitOfWork.RepoShiftCode.GetById(id); break;
                case 5: me = unitOfWork.RepoWorkstation.GetById(id); break;
                case 6: me = unitOfWork.RepoCategory.GetById(id); break;
                case 7: me = unitOfWork.RepoType.GetById(id); break;
            }

            if (me != null)
            {
                me.Name = Name;
                unitOfWork.RepoCommon.AddOrUpdate(me);
            }

            return Json("");
        }
        //-------------------------------------------------------DEF MANAGE
        public ActionResult DefManageCombo(int? id)
        {
            Response.AppendHeader("Access-Control-Allow-Origin", "*");

            IDefManageComboViewModel vm = new DefComboViewModel();

            switch (id)
            {
                case 3: vm.ObjectList = new List<IDefComboModel>(unitOfWork.ResourceRepo.GetList()); break;
                case 5: vm.ObjectList = new List<IDefComboModel>(unitOfWork.RepoWorkstation.GetList()); break;
            }

            vm.ComboObjectsList = (IEnumerable<SelectListItem>)new SelectList(unitOfWork.RepoArea.GetList().Where(x=>x.Deleted == false).ToList(), "Id", "Name");

            if (vm != null)
            {
                return View(vm);
            }

            return RedirectToAction("Index", new { controller = "Home" });
        }
        [HttpPost]
        public ActionResult DefManageCombo(int id, string Name, int selectedComboId)
        {
            Response.AppendHeader("Access-Control-Allow-Origin", "*");

            switch (id)
            {
                case 3: unitOfWork.RepoCommon.Add(new Resource2 { Name = Name, AreaId = selectedComboId }); break;
                case 5: unitOfWork.RepoCommon.Add(new Workstation { Name = Name, AreaId = selectedComboId }); break;
            }

            return RedirectToAction("DefManageCombo", new { id = id });
        }
        [HttpPost]
        public JsonResult EditDefManageCombo(int typeId, int id, string Name, int selectedComboId)
        {
            Response.AppendHeader("Access-Control-Allow-Origin", "*");

            IDefComboModel me = null;

            switch (typeId)
            {
                case 3: me = unitOfWork.ResourceRepo.GetById(id); break;
                case 5: me = unitOfWork.RepoWorkstation.GetById(id); break;
            }

            if (me != null)
            {
                me.Name = Name;
                me.AreaId = selectedComboId;
                unitOfWork.RepoCommon.AddOrUpdate(me);
            }

            return Json("");
        }


        //-------------------------------------------------------DEF Line
        [HttpPost]
        public JsonResult GetLines(int id)
        {
            List<Resource2> Lines = unitOfWork.ResourceRepo.GetListByArea(id).ToList();
            return Json(Lines);
        }

        //-------------------------------------------------------DEF Workstation
        [HttpPost]
        public JsonResult GetWorkstations(int id)
        {
            List<Workstation> workstations = unitOfWork.RepoWorkstation.GetListByArea(id).ToList();
            return Json(workstations);
        }

        //-------------------------------------------------------DEF1
        public ActionResult Area()
        {
            return View();
        }

    }
}