using MDL_BASE.Models.MasterData;
using XLIB_COMMON.Repo;
using MDL_PRD.Interface;
using MDL_PRD.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using MDLX_MASTERDATA.Entities;
using MDLX_CORE.ComponentCore.Entities;

namespace MDL_PRD.Repo.Schedule
{
    public class ProductionOrderSequenceRepo : RepoGenericAbstract<ProdOrderSequence>
    {
        protected new IDbContextPRD db;

        public ProductionOrderSequenceRepo(IDbContextPRD db) : base(db)
        {
            this.db = db;
        }

        public override ProdOrderSequence GetById(int id)
        {
            return db.ProdOrderSequence.FirstOrDefault(d => d.Id == id);
        }
        public override IQueryable<ProdOrderSequence> GetList()
        {
            return db.ProdOrderSequence.OrderByDescending(x => x.Id);
        }
        public ProdOrderSequence GetByOrderId(int orderId, int snapshotNo)
        {
            return db.ProdOrderSequence.FirstOrDefault(d => d.OrderId == orderId && d.SnapshotNo == snapshotNo && d.Active == true);
        }
        public List<ProdOrderSequence> GetBySnapshotNo(int snapshotNo)
        {
            return db.ProdOrderSequence.Where(d => d.SnapshotNo == snapshotNo && d.Active == true).ToList();
        }
        public List<ProdOrderSeqGrid> GetListWithChangedSeq(DateTime startDate, string line, string orderNo, string pnc, int snapShotNo)
        {
            List<ProdOrderSeqGrid> list;
            DateTime dateLimit = startDate.AddDays(4);

            list = (from t1 in db.ProductionOrders
                                .Where(o => (o.Deleted == false)
                                        && (startDate <= o.StartDate && o.StartDate < dateLimit)
                                        && (pnc == null || o.Pnc.Code == pnc)
                                        && (orderNo == null || o.OrderNumber == orderNo)
                                        && ((line == o.Line.Name) && o.Line.Name != "901"))
                    join t2a in db.ProdOrderSequence.Where(s => (s.SnapshotNo == snapShotNo || snapShotNo == 0)) on
                        new { Id = t1.Id, Seq = t1.Sequence } equals new { Id = t2a.OrderId, Seq = t2a.SnapshotSeq } into ProrOrderSeq
                    join t3a in db.ProdOrderStatuses.Where(st => st.StatusName == "MAGA") on
                        t1.Id equals t3a.OrderId into ProdOrderStateA
                    join t3b in db.ProdOrderStatuses.Where(st => st.StatusName == "MAGB") on
                        t1.Id equals t3b.OrderId into ProdOrderStateB
                    from t2b in ProrOrderSeq.DefaultIfEmpty()
                    from t3a2 in ProdOrderStateA.DefaultIfEmpty()
                    from t3b2 in ProdOrderStateB.DefaultIfEmpty()
                        //join t3a in db.ProdOrderSequence.Where(s => s.Active == true).on
                    select new ProdOrderSeqGrid
                    {
                        OrderId = t1.Id,
                        StartDate = t1.StartDate,
                        EndDate = t1.EndDate,
                        OrderNo = t1.OrderNumber,
                        Line = t1.Line.Name,
                        PNC = t1.Pnc.Code,
                        QtyPlanned = t1.QtyPlanned,
                        QtyRemain = t1.QtyRemain,
                        QtyProducedInPast = t1.QtyProducedInPast,
                        Notice = t1.Notice,
                        SeqTemp = (t2b != null && t2b.SnapshotSeq > 0) ? t2b.SnapshotSeq : t1.Sequence,
                        SeqOriginal = (t2b != null && snapShotNo > 0) ? t2b.OriginalSequence : t1.Sequence,
                        IsSeqChenged = (t2b != null && t2b.OriginalSequence != t2b.SnapshotSeq),
                        StateA = t3a2 != null ? (int)t3a2.StatusState : -1,
                        StateB = t3b2 != null ? (int)t3b2.StatusState : -1,
                        FirstProductIn = t1.FirstProductIn
                    })
                    .GroupBy(x => x.OrderId)
                    .Select(x => new ProdOrderSeqGrid()
                    {
                        OrderId = x.Key,
                        StartDate = x.Min(y => y.StartDate),
                        EndDate = x.Min(y => y.EndDate),
                        OrderNo = x.Min(y => y.OrderNo),
                        Line = x.Min(y => y.Line),
                        PNC = x.Min(y => y.PNC),
                        QtyPlanned = x.Min(y => y.QtyPlanned),
                        QtyRemain = x.Min(y => y.QtyRemain),
                        QtyProducedInPast = x.Max(y => y.QtyProducedInPast),
                        Notice = x.Min(y => y.Notice),
                        SeqTemp = x.Min(y => y.SeqTemp),
                        SeqOriginal = x.Min(y => y.SeqOriginal),
                        IsSeqChenged = x.Any(y => y.IsSeqChenged),
                        StateA = x.Max(y => y.StateA),
                        StateB = x.Max(y => y.StateB),
                        FirstProductIn = x.Min(y => y.FirstProductIn),
                    })
                    .OrderBy(o => o.SeqTemp).ToList();

            return list;
        }
        public List<ProductionOrder> GetPlanFromSeqToSeq(string lineName, int minSeq, int maxSeq)
        {
            List<ProductionOrder> list = new List<ProductionOrder>();
            //db.ProductionOrders.Where(x=>x.)
            //list.Insert(0, )
            //list.Add();
            return list;
        }
        public List<SnapshotInfo> GetSnapshots()
        {
            List<ProdOrderSequence> prodOrderSeqList = db.ProdOrderSequence.Where(x => x.SnapshotNo > 0).OrderBy(o => o.SnapshotNo).ToList();
            List<SnapshotInfo> snapshots = new List<SnapshotInfo>();
            SnapshotInfo snapshot;

            foreach (ProdOrderSequence pos in prodOrderSeqList)
            {
                snapshot = snapshots.FirstOrDefault(x => x.SnapshotNo == pos.SnapshotNo);
                if (snapshot == null)
                {
                    snapshot = new SnapshotInfo
                    {
                        SnapshotNo = pos.SnapshotNo,
                        CreationDate = pos.CreationDate,
                        CreatorUserName = pos.CreatorUserName,
                        TotalChanges = 0,
                        LineName = pos.SnapshotLineName
                    };
                    snapshots.Add(snapshot);
                }

                snapshot.TotalChanges++;
            }

            return snapshots;
        }
        public List<int> GetSnapshotOrderIds(int snapshotNo)
        {
            return db.ProdOrderSequence.Where(x => x.SnapshotNo == snapshotNo).Select(s => (int)s.OrderId).ToList();
        }

        public void ChangeSequence(int orderId, int sequence, string userName)
        {
            ProdOrderSequence prodOrdSeq = Create0(orderId, sequence);
            prodOrdSeq.SnapshotSeq = sequence;
            prodOrdSeq.Order.Sequence = sequence;
            prodOrdSeq.CreatorUserName = userName;

            if (prodOrdSeq.SnapshotSeq != prodOrdSeq.OriginalSequence)
                AddOrUpdate(prodOrdSeq);
            else
                Delete(prodOrdSeq);

        }
        public void ChangeQty(int orderId, int newQty, string userName)
        {
            ProductionOrder order = db.ProductionOrders.FirstOrDefault(x => x.Id == orderId);

            if (order != null && newQty >= 0 && newQty <= order.QtyPlanned)
            {
                order.QtyRemain = newQty;
                order.QtyProducedInPast = order.QtyPlanned - order.QtyRemain;
                AddOrUpdate(order);
            }
        }
        public void ChangeLine(int orderId, Resource2 line, string userName)
        {
            ProductionOrder order = db.ProductionOrders.FirstOrDefault(x => x.Id == orderId);

            if (line != null && line != order.Line)
            {
                ProdOrderSequence prodOrdSeq = Create0(orderId, 0);
                prodOrdSeq.Order.Line = line;
                prodOrdSeq.Order.LineId = line.Id;
                prodOrdSeq.Order.Sequence = 0;
                prodOrdSeq.SnapshotLineName = line.Name;
                prodOrdSeq.SnapshotSeq = 0;
                prodOrdSeq.CreatorUserName = userName;

                if (prodOrdSeq.SnapshotSeq != prodOrdSeq.OriginalSequence)
                    AddOrUpdate(prodOrdSeq);
                else
                    Delete(prodOrdSeq);
            }
        }
        public void CreateSnapshot(string lineName)
        {
            int currentSnapshotNo = GetLastSnapshotNo();
            int newSnapshotNo = currentSnapshotNo + 1;

            List<ProdOrderSequence> list = db.ProdOrderSequence
                                            .Where(x => x.SnapshotNo == 0 &&
                                                        x.SnapshotLineName == lineName &&
                                                        x.Applied == true).ToList();

            foreach (ProdOrderSequence pos in list)
            {
                pos.SnapshotNo = newSnapshotNo;
                AddOrUpdate(pos);
            }
        }
        public void DeleteSnapshot(int snapshotNo)
        {
            List<ProdOrderSequence> list = db.ProdOrderSequence.Where(x => x.SnapshotNo == snapshotNo).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
        public void ApplyChanges(string lineName)
        {
            List<ProdOrderSequence> posList = GetList().Where(x => (x.Order != null && x.Order.Line.Name == lineName) && x.SnapshotNo == 0).ToList();
            foreach (ProdOrderSequence pos in posList)
            {
                if (pos.SnapshotSeq > 0)
                {
                    pos.Applied = true;
                    Update(pos);
                }
            }
            ChangeLastUpdateDate(lineName);
        }
        public void ChangeLastUpdateDate(string lineName)
        {
            //NOTE: Function required by ETISOFT Etiszop software
            Resource2 line = db.Resources2.FirstOrDefault(x => x.Name == lineName);
            if (line != null)
            {
                db.Database.ExecuteSqlCommand("UPDATE [PRD].[ProdOrder] SET [LastUpdate] = GETDATE() WHERE Deleted = 0 AND LineId = " + line.Id);
            }
        }
        public void RestoreOriginalSequence()
        {
            db.Database.ExecuteSqlCommand("EXEC dbo.PRD_ProdOrder_import");
        }
        public int GetLastSnapshotNoByLine(string lineName)
        {
            return db.ProdOrderSequence.Where(x => x.Active == true && x.SnapshotLineName == lineName).Max(x => x.SnapshotNo);
        }
        public int GetMinChanedSeqByLine(int snapshotNo, string lineName = null)
        {
            int minC = 0;
            int minT = 0;
            try
            {
                minC = (int)db.ProdOrderSequence.Where(x =>
                    x.Active == true &&
                    (lineName == null || (x.Order != null && x.Order.Line.Name == lineName)) &&
                    x.SnapshotNo == snapshotNo)
                .Min(m => m.OriginalSequence);

                minT = (int)db.ProdOrderSequence.Where(x =>
                    x.Active == true &&
                    (lineName == null || (x.Order != null && x.Order.Line.Name == lineName)) &&
                    x.SnapshotNo == snapshotNo)
                .Min(m => m.SnapshotSeq);
            }
            catch { }

            return Math.Min(minC, minT);
        }
        public int GetMaxChanedSeqByLine(int snapshotNo, string lineName = null)
        {
            int minC = 0;
            int minT = 0;
            try
            {
                minC = db.ProdOrderSequence.Where(x =>
                    x.Active == true &&
                    (lineName == null || (x.Order != null && x.Order.Line.Name == lineName)) &&
                    x.SnapshotNo == snapshotNo)
                .Max(m => m.OriginalSequence);

                minT = db.ProdOrderSequence.Where(x =>
                    x.Active == true &&
                    (lineName == null ||
                    (x.Order != null && x.Order.Line.Name == lineName)) &&
                    x.SnapshotNo == snapshotNo)
                .Max(m => m.SnapshotSeq);
            }
            catch { }

            return Math.Max(minC, minT);
        }

        private ProdOrderSequence Create0(int orderId, int sequence)
        {
            ProdOrderSequence pos = GetByOrderId(orderId, 0);

            if (pos == null)
            {
                ProductionOrder order = db.ProductionOrders.FirstOrDefault(x => x.Id == orderId);
                if (order != null)
                {
                    pos = new ProdOrderSequence
                    {
                        OrderId = orderId,
                        SnapshotNo = 0,
                        OriginalSequence = order.Sequence,
                        OriginalLineName = order.Line.Name,
                        OriginalLineId = order.LineId,
                        OriginalStartDate = order.StartDate,
                        CreationDate = DateTime.Now,
                        CreatorUserName = "null",
                        Applied = false,
                        Active = true
                    };

                    pos.SnapshotSeq = sequence;
                    pos.SnapshotStartDate = order.StartDate;
                    pos.SnapshotLineName = order.Line.Name;

                    AddOrUpdate(pos);
                }
                else
                {
                    return null;
                }
            }
            pos.Applied = false;

            return pos;
        }
        private int GetLastSnapshotNo()
        {
            int m = 0;
            try
            {
                m = db.ProdOrderSequence.Where(x => x.Active == true).Max(x => x.SnapshotNo);
            }
            catch
            { }

            return m;
        }
        private void RestoreOriginalSchedule_old()
        {
            List<ProdOrderSequence> list = db.ProdOrderSequence.Where(x => x.SnapshotNo == 0).ToList();
            foreach (ProdOrderSequence pos in list)
            {
                pos.Order.Sequence = pos.OriginalSequence;
            }
        }
    }
}