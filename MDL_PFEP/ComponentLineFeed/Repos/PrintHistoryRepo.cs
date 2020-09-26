using XLIB_COMMON.Repo;
using MDL_PFEP.Interface;
using MDL_PFEP.Model.PFEP;
using System;
using System.Collections.Generic;
using System.Linq;
using MDL_PFEP.Model;
using MDL_BASE.Models.Base;
using MDL_PRD.Model;

namespace MDL_PFEP.Repo.PFEP
{
    public class PrintHistoryRepo : RepoGenericAbstract<PrintHistory>
    {
        protected new IDbContextPFEP db;

        public PrintHistoryRepo(IDbContextPFEP db)
            : base(db)
        {
            this.db = db;
        }

        public override PrintHistory GetById(int id)
        {
            return db.PrintHistory.FirstOrDefault(d => d.Id == id);
        }
        public override IQueryable<PrintHistory> GetList()
        {
            return db.PrintHistory.OrderByDescending(x => x.Id);
        }

        public List<PrintOrderModel> GetForPrint(int routineId, DateTime startDate, string[] line, string orderNo, string pnc)
        {
            List<PrintOrderModel> list;
            DateTime dateLimit = startDate.AddDays(4);
            bool allLines = !(line.Length > 0);

            var printedSum = db.PrintHistory
                                .Where(o => (startDate <= o.Order20.Order.EndDate && o.Order20.Order.StartDate < dateLimit)
                                        && (pnc == null || o.Order20.Order.Pnc.Code == pnc)
                                        && (orderNo == null || o.Order20.Order.OrderNumber == orderNo)
                                        && ((allLines || line.Contains(o.Order20.Order.Line.Name)) && o.Order20.Order.Line.Name != "901")
                                        && (o.RoutineId == routineId)
                                        )
                                .GroupBy(o => o.Order20.OrderId)
                                .Select(g => new { Id = g.FirstOrDefault().Order20.OrderId, SumQty = g.Sum(s=>s.Order20.PartQty) });


            list = (from t1 in db.ProductionOrders
                            .Where(o => (startDate <= o.EndDate && o.StartDate < dateLimit)
                                    && (pnc == null || o.Pnc.Code == pnc)
                                    && (orderNo == null || o.OrderNumber == orderNo)
                                    && ((allLines || line.Contains(o.Line.Name)) && o.Line.Name != "901")
                                    && (o.Deleted == false))
                        join t2a in printedSum on t1.Id equals t2a.Id into prntH
                        from t2b in prntH.DefaultIfEmpty()
                        select new PrintOrderModel
                        {
                            Id = t1.Id,
                            StartDate = t1.StartDate,
                            OrderNo = t1.OrderNumber,
                            LineName = t1.Line.Name,
                            PNCCode = t1.Pnc.Code,
                            Qty = t1.QtyRemain,
                            QtyOrder = t1.QtyPlanned,
                            QtyPrinted = t2b != null? t2b.SumQty : 0,
                            Printed = false,
                            RoutineId = 0
                        }
                    ).OrderBy(o => o.StartDate).ToList();

            return list;
        }
        public List<PrintOrderModel> GetForPrint20(int routineId, DateTime startDate, string[] lines, string orderNo, string pnc)
        {
            List<Prodorder20> orders = db.ProdOrder20.Take(10).ToList();

            List<PrintOrderModel> list;
            DateTime dateLimit = startDate.AddDays(4);

            bool allLines = !(lines.Length > 0);

            list = (from t1 in db.ProdOrder20
                                .Where(o => (o.Order.Deleted == false) 
                                        && (startDate <= o.PartStartDate && o.PartStartDate < dateLimit)
                                        && (pnc == null || o.Order.Pnc.Code == pnc)
                                        && (orderNo == null || o.Order.OrderNumber == orderNo)
                                        && ((allLines ||lines.Contains(o.Order.Line.Name)) && o.Order.Line.Name != "901"))
                    join t2a in db.PrintHistory
                                .Where(x => x.RoutineId == routineId) on t1.Id equals t2a.Order20Id into prntH
                    from t2b in prntH.DefaultIfEmpty()
                    select new PrintOrderModel
                    {
                        Id = t1.Id,
                        StartDate = t1.PartStartDate,
                        OrderNo = t1.Order.OrderNumber,
                        LineName = t1.Order.Line.Name,
                        PNCCode = t1.Order.Pnc.Code,
                        Qty = t1.PartQtyRemain,
                        QtyOrder = t1.Order.QtyPlanned,
                        Printed = t2b == null ? false : true,
                        RoutineId = t2b == null ? 0 : t2b.RoutineId
                    }).OrderBy(o => o.StartDate).ToList();

            return list;
        }

        public void AddOrUpdatePrintData20order(int routineId, int order20Id, string userId, DateTime printDate)
        {
            PrintHistory ph = db.PrintHistory.FirstOrDefault(x => x.RoutineId == routineId && x.Order20Id == order20Id & x.Order20.PartQtyRemain > 0);

            if(ph == null)
            {
                ph = new PrintHistory{ Order20Id = order20Id, RoutineId = routineId };
            }

            ph.PrintDate = printDate;
            ph.UserId = userId;
            ph.Printnumber = 0;
            AddOrUpdate(ph);
        }
        public void AddOrUpdatePrintDataFullOrder (int routineId, int orderId, string userId, DateTime printDate)
        {
            List<int> orders20Ids = db.ProdOrder20.Where(x => x.OrderId == orderId).Select(o=>o.Id).ToList();

            foreach(int order20id in orders20Ids)
            {
                AddOrUpdatePrintData20order(routineId, order20id, userId, printDate);
            }
        }
    }
}
