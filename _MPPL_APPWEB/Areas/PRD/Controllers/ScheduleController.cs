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
using MDLX_MASTERDATA.Entities;
using MDLX_CORE.ComponentCore.Entities;
using MDLX_CORE.ComponentCore.Repos;

namespace _MPPL_WEB_START.Areas.PRD.Controllers
{
    [Authorize(Roles = DefRoles.PRD_SCHEDULER + "," + DefRoles.PFEP_DEFPRINT_EDITOR)]
    public class ScheduleController : Controller
    {
        private readonly IDbContextPRD db;
        public ScheduleController(IDbContextPRD db)
        {
            this.db = db;
        }


        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetSchedule(ProdOrderSeqGrid filter)
        {
            ProductionOrderSequenceRepo repo = new ProductionOrderSequenceRepo(db);
            List<ProdOrderSeqGrid> orders;
            orders = repo.GetListWithChangedSeq(filter.StartDate, filter.Line, filter.OrderNo, filter.PNC, 0);

            return Json(orders, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateSequence(int orderId, int prevSeq, int nextSeq)
        {
            ProductionOrderSequenceRepo repo = new ProductionOrderSequenceRepo(db);
            ProdOrderSequence prodOrderSeq = repo.GetByOrderId(orderId, 0);

            prodOrderSeq.SnapshotSeq = prevSeq + (nextSeq - prevSeq) / 2;
            repo.Add(prodOrderSeq);

            return Json(prodOrderSeq.SnapshotSeq, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ChangeSequence(int orderId, int seq)
        {
            ProductionOrderSequenceRepo repo = new ProductionOrderSequenceRepo(db);
            repo.ChangeSequence(orderId, seq, User.Identity.Name);
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ChangeQty(int orderId, int newQty)
        {
            ProductionOrderSequenceRepo repo = new ProductionOrderSequenceRepo(db);
            
            repo.ChangeQty(orderId, newQty, User.Identity.Name);
            
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ChangeLine(int orderId, string lineName)
        {
            ProductionOrderSequenceRepo repo = new ProductionOrderSequenceRepo(db);
            Resource2 line = db.Resources2.FirstOrDefault(x => x.Name == lineName);
            if (line != null)
            {
                repo.ChangeLine(orderId, line, User.Identity.Name);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteSnapshot(int snapshotNo, string lineName = null)
        {
            ProductionOrderSequenceRepo posRepo = new ProductionOrderSequenceRepo(db);
            List<ProdOrderSequence> posList = posRepo.GetBySnapshotNo(snapshotNo);
            //bool isLineChanged = false;
            lineName = (lineName != null)? lineName : posList.FirstOrDefault().SnapshotLineName;

            if (posRepo.GetLastSnapshotNoByLine(lineName) > snapshotNo && snapshotNo != 0)
            {
                return Json("Snapshoty należy kasować od ostatniego", JsonRequestBehavior.AllowGet);
            }

            foreach (ProdOrderSequence pos in posList)
            {
                //isLineChanged = (pos.OriginalLineName != pos.SnapshotLineName);   
                pos.Order.Sequence = pos.OriginalSequence;
                pos.Order.LineId = pos.OriginalLineId;
                posRepo.Update(pos);
            }

            if (snapshotNo > 0)
            {
                CalculateOrdersBetweenSequences(posRepo, lineName, snapshotNo);
            }
            posRepo.DeleteSnapshot(snapshotNo);

            return Json("Snapshot został usunięty", JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveChanges(string lineName)
        {
            ProductionOrderSequenceRepo repo = new ProductionOrderSequenceRepo(db);
            CalculateOrdersBetweenSequences(repo, lineName);
            repo.ApplyChanges(lineName);
            repo.CreateSnapshot(lineName);

            return Json(0, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SetQtyProducedInPast(int orderId, int qtyProducedInPast)
        {
            ProductionOrderRepo repo = new ProductionOrderRepo(db);
            ProductionOrder prodOrder = repo.GetById(orderId);

            prodOrder.QtyProducedInPast = qtyProducedInPast;
            repo.AddOrUpdate(prodOrder);

            return Json(0, JsonRequestBehavior.AllowGet);
        }
        private void CalculateOrdersBetweenSequences(ProductionOrderSequenceRepo repo, string lineName, int snapShotNo = 0)
        {
            int startAtSequence = repo.GetMinChanedSeqByLine(snapShotNo, lineName);
            int endAtSequence = repo.GetMaxChanedSeqByLine(snapShotNo, lineName);

            ProductionOrderRepo prodOrdRepo = new ProductionOrderRepo(db);
            List<ProductionOrder> poList = prodOrdRepo.GetScheduleRange(lineName, startAtSequence, endAtSequence);

            ProdOrderScheduler scheduler = new ProdOrderScheduler(db, lineName);
            scheduler.CalculateNewStartTime(poList);

            foreach (ProductionOrder ord in poList)
            {
                prodOrdRepo.Update(ord);
            }
        }
        [HttpPost]
        public JsonResult CalculateAllLine(int orderId, string lineName, DateTime newStartTime)
        {
            ProductionOrderSequenceRepo repo = new ProductionOrderSequenceRepo(db);
            ProductionOrderRepo prodOrdRepo = new ProductionOrderRepo(db);
            ProdOrderScheduler scheduler = new ProdOrderScheduler(db, lineName);
            ProductionOrder firstPo = prodOrdRepo.GetById(orderId);

            int shiftSeconds = (int)(newStartTime- firstPo.StartDate).TotalSeconds;
            //shiftSeconds = firstPo.StartDate > newStartTime ? shiftSeconds * -1 : shiftSeconds; //TODO: pkt.3 tu jest błąd
            firstPo.StartDate = newStartTime;

            CalculateFutherOrders(lineName, prodOrdRepo, scheduler, firstPo);
            CalculatePreviousOrders(lineName, prodOrdRepo, scheduler, firstPo, shiftSeconds);

            repo.ApplyChanges(lineName);
            repo.CreateSnapshot(lineName);

            //0.041666 = 1h (1/24). Od ostatniej sztuki produkukowanyj musi upłynąć godzina czasu, żeby ilość została pomniejszona poprzez ustawienie 'QtyProducedInPast'
            prodOrdRepo.DbSet.SqlQuery("UPDATE [PRD].[ProdOrder] " +
                "SET [QtyProducedInPast] = [CounterProductsIn] " +
                "WHERE Deleted = 0 " +
                "AND ([LastProductIn] < StartDate - 0.041666) " +
                "AND CounterProductsIn > 0 " +
                "AND CounterProductsIn < QtyPlanned");

            return Json(0, JsonRequestBehavior.AllowGet);
        }
        private static void CalculatePreviousOrders(string lineName, ProductionOrderRepo prodOrdRepo, ProdOrderScheduler scheduler, ProductionOrder firstPo, int shiftSeconds)
        {
            List<ProductionOrder> ordersForShift = prodOrdRepo.GetListSortedBySequence()
                                                .Where(x => x.Line.Name == lineName && x.Sequence < firstPo.Sequence)
                                                .OrderBy(o => o.Sequence)
                                                .ToList();

            scheduler.ShiftOrders(ordersForShift, shiftSeconds);
            foreach (ProductionOrder ord in ordersForShift)
            {
                prodOrdRepo.Update(ord);
            }
        }
        private static void CalculateFutherOrders(string lineName, ProductionOrderRepo prodOrdRepo, ProdOrderScheduler scheduler, ProductionOrder firstPo)
        {
            List<ProductionOrder> ordersForRecalc = prodOrdRepo.GetListSortedBySequence()
                                                .Where(x => x.Line.Name == lineName && x.Sequence >= firstPo.Sequence)
                                                .OrderBy(o => o.Sequence)
                                                .ToList();

            scheduler.CalculateNewStartTime(ordersForRecalc);
            foreach (ProductionOrder ord in ordersForRecalc)
            {
                prodOrdRepo.Update(ord);
            }
        }

        public JsonResult LoadOriginalSchedule()
        {
            ProductionOrderSequenceRepo repo = new ProductionOrderSequenceRepo(db);
            repo.RestoreOriginalSequence();

            return Json("Przywrócono oryginalny plan z FAS'a", JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSnapshots()
        {
            ProductionOrderSequenceRepo repo = new ProductionOrderSequenceRepo(db);
            return Json(repo.GetSnapshots(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSnapshotOrders(int snapshotNo)
        {
            ProductionOrderSequenceRepo repo = new ProductionOrderSequenceRepo(db);
            return Json(repo.GetSnapshotOrderIds(snapshotNo), JsonRequestBehavior.AllowGet);
        }
    }
}