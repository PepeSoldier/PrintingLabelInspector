using _MPPL_WEB_START.Migrations;
using MDL_BASE.ViewModel;
using MDL_PRD.Interface;
using MDL_PRD.Model;
using MDL_PRD.Repo;
using MDL_PRD.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.PRD.Controllers
{
    public class PSIController : Controller
    {
        IDbContextPRD db;
        
        public PSIController(IDbContextPRD db)
        {
            this.db = db;
        }

        public ActionResult Index()
        {
            PsiFormViewModel vm = new PsiFormViewModel();

            Hashtable lines = new Hashtable();
            lines.Add(104, 104);
            lines.Add(103, 103);
            lines.Add(101, 101);

            Hashtable Shifts = new Hashtable();
            Shifts.Add(3, "Zm. 3");
            Shifts.Add(2, "Zm. 2");
            Shifts.Add(1, "Zm. 1");

            vm.SelectedDate = DateTime.Now.Date;
            vm.Lines = new SelectList(lines, "key", "value");
            vm.Shifts = new SelectList(Shifts, "key", "value");

            return View(vm);
        }
        public ActionResult Analyze()
        {
            PsiFormViewModel vm = new PsiFormViewModel();

            Hashtable lines = new Hashtable();
            lines.Add(104, 104);
            lines.Add(103, 103);
            lines.Add(101, 101);
            
            Hashtable Shifts = new Hashtable();
            Shifts.Add(3, "Zm. 3");
            Shifts.Add(2, "Zm. 2");
            Shifts.Add(1, "Zm. 1");

            vm.SelectedDate = DateTime.Now.Date;
            vm.Lines = new SelectList(lines, "key", "value");
            vm.Shifts = new SelectList(Shifts, "key", "value");

            return View(vm);
        }
        public ActionResult Results()
        {
            PsiResultFormViewModel vm = new PsiResultFormViewModel();

            Hashtable lines = new Hashtable();
            lines.Add(104, 104);
            lines.Add(103, 103);
            lines.Add(101, 101);
            lines.Add(0, "*");

            Hashtable Shifts = new Hashtable();
            Shifts.Add(3, "Zm. 3");
            Shifts.Add(2, "Zm. 2");
            Shifts.Add(1, "Zm. 1");
            Shifts.Add(0, "*");

            vm.SelectedDateFrom = DateTime.Now.Date.AddDays(-1);
            vm.SelectedDateTo = DateTime.Now.Date;
            vm.Lines = new SelectList(lines, "key", "value");
            vm.Shifts = new SelectList(Shifts, "key", "value");

            return View(vm);
        }

        public JsonResult ResultsCalc(DateTime dateFrom, DateTime dateTo, int shift = 0, string line = "")
        {
            dateFrom = dateFrom.Date;
            dateTo = dateTo.Date;

            PsiViewModel vm = new PsiViewModel();
            PlanComparator planComparator = new PlanComparator();
            ChartJSViewModel chartJsData = new ChartJSViewModel();

            ComparePlan(planComparator, dateFrom, dateTo, shift, line);
            PrepareChartData(planComparator.kpi.OrdersWithErrorTotal, chartJsData);

            vm.DSA = string.Format("{0:P2}", planComparator.kpi.Dsa);
            vm.SeqCnt = string.Format("{0:P2}", planComparator.kpi.CounterAccuracy);
            vm.SeqSum = string.Format("{0:P2}", planComparator.kpi.SumAccuracy);
            vm.PSI = string.Format("{0:P2}", planComparator.kpi.PSI);
            vm.ChartJsData = chartJsData;
            //vm.ChartJsData = new ChartJSViewModel { Data = new List<int> { 79, 60, 10 }, Label = "test", Labels = new List<string> { "test1" , "test2", "test3" } };
            
            return Json(vm, JsonRequestBehavior.AllowGet);
        }
        private void ComparePlan(PlanComparator planComparator, DateTime dateFrom, DateTime dateTo, int shift = 0, string line = "")
        {
            List<OrderArchiveModel> ordersArch = new List<OrderArchiveModel>();
            List<OrderArchiveModel> ordersCrnt = new List<OrderArchiveModel>();
            OrderArchRepo repo = new OrderArchRepo(db);

            int[] shifts = { 1, 2, 3 };
            string[] lines = { "101", "103", "104" };

            for (DateTime date = dateFrom; date <= dateTo; date = date.AddDays(1))
            {
                foreach (string line1 in lines)
                {
                    if (line == "" || line1 == line)
                    {
                        foreach (int shift1 in shifts)
                        {
                            if (shift == 0 || shift1 == shift)
                            {
                                ordersArch = repo.LoadData_OrdersArch(date, shift1, line1);
                                ordersCrnt = repo.LoadData_Orders(date, shift1, line1);
                                RemoveDoubledOrders(ordersArch, date.AddDays(-2));
                                RemoveDoubledOrders(ordersCrnt, date);
                                planComparator.ReplaceData(ordersArch, ordersCrnt);
                                planComparator.CheckAdditionalOrders();
                                planComparator.CheckRemovedOrders();
                                planComparator.CompareOrdersSequence();
                                planComparator.CheckProductionSequence();
                            }
                        }
                    }
                }
            }
        }
        private void PrepareChartData(List<OrderArchiveModel> ordersWithError, ChartJSViewModel chartJsData)
        {
            ReasonModel r = new ReasonModel {
                Id = 0, Active = true,
                Name = "Brak Opisu",
                ParentId = 0,
                ReasonDetails = string.Empty
            };

            ordersWithError.ForEach(x => { if (x.Reason == null) { x.Reason = r; } });

            var results = from p in ordersWithError
                          group p by p.Reason into g
                          select new { ReasonName = g.Key.Name, Sum = g.Sum(m => m.QtyRemain) };

            Dictionary<string, int> dict = results.OrderByDescending(x => x.Sum).ToDictionary(mc => mc.ReasonName, mc => mc.Sum);
            
            foreach (var pair in dict)
            {
                chartJsData.Label = "powody";
                chartJsData.Labels.Add(pair.Key);
                chartJsData.Data.Add(pair.Value);
            }
        }

        public JsonResult RefreshPlan(DateTime orderDate, int shift, string line)
        {
            PsiViewModel vm = new PsiViewModel();

            vm.Orders = new OrderArchRepo(db).LoadData_Orders(orderDate, shift, line);
            vm.OrdersArch = new OrderArchRepo(db).LoadData_OrdersArch(orderDate, shift, line);

            RemoveDoubledOrders(vm.Orders, orderDate);
            RemoveDoubledOrders(vm.OrdersArch, orderDate.AddDays(-2));

            PlanComparator planComparator = new PlanComparator(vm.OrdersArch, vm.Orders);
            planComparator.ResetKPIData();
            planComparator.CheckAdditionalOrders();
            planComparator.CheckRemovedOrders();
            planComparator.CompareOrdersSequence();
            planComparator.CheckProductionSequence();

            vm.DSA = string.Format("{0:P2}", planComparator.kpi.Dsa);
            vm.SeqCnt = string.Format("{0:P2}", planComparator.kpi.CounterAccuracy);
            vm.SeqSum = string.Format("{0:P2}", planComparator.kpi.SumAccuracy);
            vm.PSI = string.Format("{0:P2}", planComparator.kpi.PSI);

            return Json(vm, JsonRequestBehavior.AllowGet);
        }
        private void RemoveDoubledOrders_old(List<OrderArchiveModel> l)
        {
            string order1 = "0";
            string order2 = "1";

            for (int i = l.Count - 1; i >= 0; i--)
            {
                order2 = l[i].OrderNo;

                if (order1 == order2)
                {
                    l.RemoveAt(i);
                }

                order1 = l[i].OrderNo;
            }
        }
        private void RemoveDoubledOrders(List<OrderArchiveModel> l, DateTime freezeDate)
        {
            int i = 0;
            bool deleted = false;

            while (i < l.Count)
            {
                if (l[i].FreezeDate.Date != freezeDate.Date)
                {
                    i++;
                    continue;
                }

                for (int j = l.Count - 1; j >= 0; j--)
                {
                    if (l[i].Id != l[j].Id && l[i].OrderNo == l[j].OrderNo)
                    {
                        l.RemoveAt(j);
                        deleted = true;
                    }
                }

                if (deleted)
                {
                    i = 0;
                    deleted = false;
                }
                else
                {
                    i++;
                }
            }
        }

        public ActionResult Reason(int id)
        {
            ResonViewModel vm = new ResonViewModel();
            vm.Resons = db.PA_Reason.ToList();
            vm.SelectedOrderId = id;
            return View(vm);
        }
        public ActionResult ReasonComment(int[] orders)
        {
            return View(orders);
        }
        public JsonResult ReasonClick(int id)
        {
            //id -> btnId
            int parentId = id;
            List<ReasonModel> reasons = db.PA_Reason.Where(r => r.ParentId == parentId).ToList<ReasonModel>();

            if (!(reasons.Count > 0))
            {
                //wpisz reason
            }

            return Json(reasons);
        }
        [HttpPost]
        public JsonResult SaveReason(int selectedOrderId, int selectedReasonId)
        {
            ReasonModel reason = db.PA_Reason.FirstOrDefault(r => r.Id == selectedReasonId);
            OrderArchiveModel order = new OrderArchRepo(db).GetById(selectedOrderId);

            if (reason.Name == "Skasuj")
            {
                order.Reason = null;
            }
            else
            {
                order.Reason = reason;
                order.ReasonDetails = reason.ReasonDetails;
            }
            db.SaveChanges();

            return Json(reason.Name);
        }
        public JsonResult SaveComment(int selectedOrderId, string comment, string supplier, string anc)
        {
            OrderArchiveModel order = new OrderArchRepo(db).GetById(selectedOrderId);
            order.CommentText = comment;
            order.CommentSupplier = supplier;
            order.CommentAnc = anc;
            db.SaveChanges();
            return Json(comment + "-" + supplier + "-" + anc);
        }

        public JsonResult GetReasons()
        {
            return Json(db.PA_Reason.Where(x=>x.Active == true).ToList(), JsonRequestBehavior.AllowGet);
        }
        
    }
}