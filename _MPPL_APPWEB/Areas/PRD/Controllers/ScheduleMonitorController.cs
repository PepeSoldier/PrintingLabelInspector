using _MPPL_WEB_START.Areas.PRD.ViewModels;
using MDL_BASE.Models.Base;
using MDL_BASE.Models.IDENTITY;
using MDL_BASE.Models.MasterData;
using XLIB_COMMON.Repo.Base;
using MDL_PRD.Interface;
using MDL_PRD.Model;
using MDL_PRD.Repo;
using MDL_PRD.Repo.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MDLX_CORE.ComponentCore.Entities;
using MDLX_CORE.ComponentCore.Repos;
using MDLX_CORE.ComponentCore.UnitOfWorks;
using MDL_CORE.ComponentCore.Models;

namespace _MPPL_WEB_START.Areas.PRD.Controllers
{
    public class ScheduleMonitorController : Controller
    {
        private readonly IDbContextPRD db;
        UnitOfWorkCore uow;
        public ScheduleMonitorController(IDbContextPRD db)
        {
            this.db = db;
            uow = new UnitOfWorkCore(db);
        }

        public ActionResult Index(string line)
        {
            ViewBag.Skin = "nasaSkin";
            ViewBag.Line = line;
            return View();
        }
        public ActionResult IndexOrder(int index)
        {
            return View(index);
        }
        public ActionResult IndexOrderLite(int index)
        {
            return View(index);
        }

        public ActionResult VIndex(string line)
        {
            string[] si = uow.SystemVariableRepo.GetValueString(DefSystemVariables.PICKING_STATUSES_INTERNAL_LIST).Split(',');
            string[] se = uow.SystemVariableRepo.GetValueString(DefSystemVariables.PICKING_STATUSES_EXTERNAL_LIST).Split(',');

            ViewBag.Skin = "nasaSkin";
            ViewBag.Line = line;
            ViewBag.StatusesInternal = si;
            ViewBag.StatusesExternal = se;

            return View();
        }
        public ActionResult VIndexOrder(int index)
        {
            string[] si = uow.SystemVariableRepo.GetValueString(DefSystemVariables.PICKING_STATUSES_INTERNAL_LIST).Split(',');
            string[] se = uow.SystemVariableRepo.GetValueString(DefSystemVariables.PICKING_STATUSES_EXTERNAL_LIST).Split(',');
            ViewBag.StatusesInternal = si;
            ViewBag.StatusesExternal = se;

            return View(index);
        }
        public ActionResult VIndexOrderLite(int index)
        {
            string[] si = uow.SystemVariableRepo.GetValueString(DefSystemVariables.PICKING_STATUSES_INTERNAL_LIST).Split(',');
            string[] se = uow.SystemVariableRepo.GetValueString(DefSystemVariables.PICKING_STATUSES_EXTERNAL_LIST).Split(',');
            ViewBag.StatusesInternal = si;
            ViewBag.StatusesExternal = se;

            return View(index);
        }

        [HttpPost]
        public JsonResult GetSchedule(ProdOrderSeqGrid filter)
        {
            ProductionOrderStatusRepo repoStatus = new ProductionOrderStatusRepo(db);
            ProductionOrderRepo repo = new ProductionOrderRepo(db);
            List<ProductionOrder> workorders = new List<ProductionOrder>();
            List<ProductionOrder> workorders_temp = repo.GetByTimeRangeAndLine(filter.StartDate.AddHours(-8), filter.StartDate.AddDays(2), new string[] { filter.Line }).ToList();
            
            ProductionOrder n = workorders_temp.OrderByDescending(x => x.LastProductIn).FirstOrDefault();
            int index = n != null? workorders_temp.IndexOf(n) : 0;

            if (workorders_temp.Count > 0)
            {
                int from = Math.Max(0, index - 4);
                int count = Math.Min(36, workorders_temp.Count - from);
                if(index - 4 < 0)
                {
                    int ordersToBeAdded = Math.Abs(index - 4);
                    DateTime dateFrom = workorders_temp[0].FirstProductIn != null ? workorders_temp[0].FirstProductIn.Value : DateTime.Now.Date;
                    workorders.AddRange(repo.GetPreviousOrdersOfLine(dateFrom, filter.Line, ordersToBeAdded));
                }
                workorders.AddRange(workorders_temp.GetRange(from, count));
                //if(count <  )
            }

            List<ScheduleMonitorOrderViewModel> workordersVM = workorders.Select(x => new ScheduleMonitorOrderViewModel() {
                Id = x.Id,
                Line = x.Line.Name,
                OrderNo = x.OrderNumber,
                PNC = x.Pnc.Code,
                QtyFGW = x.CounterProductsFGW,
                QtyIN = x.CounterProductsIn,
                QtyOUT = x.CounterProductsOut,
                QtyPlanned = x.QtyPlanned,
                StartDate = x.StartDate,
                EndDate = x.EndDate
            }).ToList();

            foreach(ScheduleMonitorOrderViewModel woVM in workordersVM)
            {
                woVM.Statuses = repoStatus.GetListByOrderId(woVM.Id).Select(x => new OrderStatusViewModel()
                {
                    Id = x.Id,
                    StatusInfo = x.StatusInfo != null? x.StatusInfo : string.Empty,
                    StatusInfoExtra = x.StatusInfoExtra,
                    StatusName = x.StatusName,
                    StatusState = x.StatusState,
                    StatusInfoExtra2 = x.StatusInfoExtra2,
                    StausInfoExtraNumber = x.StausInfoExtraNumber
                }).OrderBy(x=>x.StatusName).ThenBy(x=>x.StatusState).ThenBy(x=>x.StatusInfo).ToList();
            }

            return Json(workordersVM, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult StatusGetList(OrderStatusViewModel filter)
        {
            ProductionOrderStatusRepo repoStatus = new ProductionOrderStatusRepo(db);
            var list = repoStatus.GetListByOrderId(filter.OrderId);
            return Json(list);
        }
        [HttpPost]
        public JsonResult StatusUpdate(OrderStatusViewModel item)
        {
            if (!(item.OrderId > 0))
            {
                return Json(null);
            }

            ProductionOrderStatusRepo repoStatus = new ProductionOrderStatusRepo(db);
            var status = repoStatus.GetById(item.Id);
            status = (status == null)? new ProdOrderStatus() { OrderId = item.OrderId } : status;
            status.StatusName = item.StatusName;
            status.StatusState = item.StatusState;
            status.StatusInfo = item.StatusInfo;
            status.StatusInfoExtra = item.StatusInfoExtra;
            status.StatusInfoExtra2 = item.StatusInfoExtra2;
            status.StausInfoExtraNumber = item.StausInfoExtraNumber;

            repoStatus.AddOrUpdate(status);

            return Json(status);
        }
        [HttpPost]
        public JsonResult StatusDelete(OrderStatusViewModel item)
        {
            ProductionOrderStatusRepo repoStatus = new ProductionOrderStatusRepo(db);
            var status = repoStatus.GetById(item.Id);
            repoStatus.Delete(status);
            return Json(0);
        }
    }
}