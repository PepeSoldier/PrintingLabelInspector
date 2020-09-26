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
    public class ClientOrderController : Controller
    {
        IDbContextOneprod db;
        RepoPreprodConf repoConf;
        UnitOfWorkOneprod uow;

        public ClientOrderController(IDbContextOneprod db)
        {
            this.db = db;
            uow = new UnitOfWorkOneprod(db);
            repoConf = new RepoPreprodConf(db);
            ViewBag.Skin = "nasaSkin";
            ViewBag.ClientName = Properties.Settings.Default.Client;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(ClientOrder clientOrder)
        {
            uow.ClientOrderRepo.Delete(uow.ResourceRepo.GetById(clientOrder.Id));
            //return RedirectToAction("Resource");
            return Json(0);
        }
        [HttpPost]
        public JsonResult Update(ClientOrder clientOrder)
        {
            clientOrder.Client = null;
            clientOrder.ClientId = clientOrder.ClientId < 1 ? null : clientOrder.ClientId;
            clientOrder.LastUpdateDate = DateTime.Now;
            clientOrder.InsertDate = clientOrder.InsertDate > new DateTime() ? clientOrder.InsertDate : DateTime.Now;
            uow.ClientOrderRepo.AddOrUpdate(clientOrder);
            return Json(clientOrder);
        }
        [HttpPost]
        public JsonResult GetList(ClientOrder filterItem, int areaID = 0)
        {
            var query = uow.ClientOrderRepo.GetList(filterItem);
            List<ClientOrder> machineList = query.ToList();
            return Json(machineList);
        }

    }
}