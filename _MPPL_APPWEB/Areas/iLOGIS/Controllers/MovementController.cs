using _MPPL_WEB_START.Areas._APPWEB.ViewModels;
using MDL_BASE.Models.Base;
using MDL_BASE.Models.IDENTITY;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_iLOGIS.ComponentWMS.Enums;
using MDL_iLOGIS.ComponentWMS.Mappers;
using MDL_iLOGIS.ComponentWMS.ViewModels;
using MDL_ONEPROD.ComponentMes.Models;
using MDL_WMS.ComponentConfig.UnitOfWorks;
using MDLX_CORE.Model.PrintModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using XLIB_COMMON.Model;

namespace _MPPL_WEB_START.Areas.iLOGIS.Controllers
{
    [Authorize]
    public partial class MovementController : Controller
    {
        readonly IDbContextiLOGIS db;
        UnitOfWork_iLogis uow;

        public MovementController(IDbContextiLOGIS db)
        {
            this.db = db;
            uow = new UnitOfWork_iLogis(db);
            ViewBag.Skin = "nasaSkin";
        }

        //Movement
        public ActionResult Movement()
        {
            ViewBag.TemplateBarcode = uow.SystemVariableRepo.GetValueString("BarcodeTemplate_WH");
            ViewBag.MovementTypes = EnumHelper.GetSelectList(typeof(EnumMovementType)).ToList(); //List<SelectListItem> 
            return View();
        }
        public ActionResult MovementMobile()
        {
            ViewBag.TemplateBarcode = uow.SystemVariableRepo.GetValueString("BarcodeTemplate_WH");
            return View();
        }
        public ActionResult MovementMobileLocation()
        {
            //ViewBag.TemplateBarcode = uow.SystemVariableRepo.GetValueString("BarcodeTemplate_WH");
            return View();
        }
        public ActionResult MovementMobileConfirmation()
        {
            ViewBag.TemplateBarcode = uow.SystemVariableRepo.GetValueString("BarcodeTemplate_WH");
            return View();
        }
        
        //MovementHistory
        public ActionResult History()
        {
            return View();
        }
        [HttpPost]
        public JsonResult HistoryGetList(MovementHistoryViewModel item, int pageIndex, int pageSize)
        {
            IQueryable<Movement> query = uow.MovementRepo.GetByFilter(item);
            int itemsCount = query.Count();
            int startIndex = (pageIndex - 1) * pageSize;
            
            List<MovementHistoryViewModel> movementHistoryList = query.Skip(startIndex).Take(pageSize).ToList<MovementHistoryViewModel>();
            return Json(new { data = movementHistoryList, itemsCount });
        }
    }
}