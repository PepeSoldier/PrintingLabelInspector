using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Rotativa;
using _MPPL_WEB_START.Areas.PFEP.ViewModels;
using MDL_PFEP.Repo;
using MDL_PFEP.Models.DEF;
using MDL_PFEP.Model;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Web.Hosting;
using MDL_PRD.Model;
using MDL_PFEP.Interface;
using MDL_PRD.Interface;
using MDL_PFEP.Repo.PFEP;
using System.Diagnostics;
using XLIB_COMMON.Model;
using MDL_PFEP.ComponentLineFeed.Models;
using MDL_ONEPROD.Repo.Scheduling;
using MDL_PRD.Repo;
using MDL_BASE.Models.MasterData;
using _MPPL_WEB_START.Areas._APPWEB.Models;
using MDLX_CORE.ComponentCore.Entities;
using MDL_iLOGIS.ComponentConfig.Entities;
//using Printing.NET;
//using MDL_ONEPROD.Common;

namespace _MPPL_WEB_START.Areas.PFEP.Controllers
{
    [Authorize]
    public class PrintController : Controller
    {
        //DbContextAPP_Electrolux db;
        private MDL_PFEP.Repo.UnitOfWork uow;
        private readonly IDbContextPFEP db;
        private readonly IDbContextPRD db2;

        public PrintController(IDbContextPFEP db, IDbContextPRD db2)
        {
            //DbContextAPP_Electrolux db = new DbContextAPP_Electrolux();
            uow = new MDL_PFEP.Repo.UnitOfWork(db, db2);
            this.db = db;
            this.db2 = db2;
        }

        //--------------USER-INTERFACE-------------------
        [HttpGet]
        public ActionResult Index()
        {
            PrintViewModel vm = new PrintViewModel();
            vm.FilterObject = new ProductionOrderFilter();
            vm.FilterObject.Date = SetHourRange(4); //new DateTime(2018,1,31,14,0,0));  //"2018-01-03 14:00";
            vm.ProductionOrders = uow.ProductionOrderRepo.GetListNew(vm.FilterObject);
            vm.Lines = new List<SelectListItem> {
                new SelectListItem { Value = "134", Text = "1/3/4" },
                new SelectListItem { Value = "101", Text = "101" },
                new SelectListItem { Value = "103", Text = "103" },
                new SelectListItem { Value = "104", Text = "104" }
            };
            vm.Shifts = new List<SelectListItem> {
                new SelectListItem { Value = "1", Text = "1" },
                new SelectListItem { Value = "2", Text = "2" },
                new SelectListItem { Value = "3", Text = "3" },
            };
            vm.Routines = Routines.GetRoutines();
            return View(vm);
        }
        [HttpPost]
        public ActionResult Index(PrintViewModel vm)
        {
            IQueryable<ProductionOrder> am;
            am = uow.ProductionOrderRepo.GetListNew(vm.FilterObject);
            vm.ProductionOrders = am;
            vm.Routines = Routines.GetRoutines();

            return View(vm);
        }
        [HttpGet]
        public JsonResult GetStartTime(int routine)
        {
            string BeginningDate = SetHourRange(routine);
            return Json(BeginningDate, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetCalculatedTimeRange(string refTime, int selectedRoutine, int spinnerVal)
        {
            DateTime refferenceTime = Convert.ToDateTime(refTime);
            string[] timeTable = new string[2];

            timeTable[0] = refferenceTime.AddHours(selectedRoutine * (spinnerVal - 1)).ToString("yyyy-MM-dd HH:mm");
            timeTable[1] = refferenceTime.AddHours(selectedRoutine * spinnerVal).ToString("yyyy-MM-dd HH:mm");

            return Json(timeTable);
        }

        public string SetHourRange(int routineHour)
        {
            DateTime BeginDate = DateTime.Now;
            return SetHour(routineHour, BeginDate);
        }
        public static string SetHour1(int routineHour, DateTime BeginDate)
        {
            int BegHour = BeginDate.Hour;

            int Diff = BegHour - 6;
            int counter = 0;
            if (Diff > 0)
            {
                counter = Diff / routineHour;
                BegHour += counter * routineHour - Diff;
                BeginDate = DateTime.Now.Date.AddHours(BegHour);
            }
            else if (Diff < 0)
            {
                counter = Diff / routineHour;
                BegHour = BegHour - (Diff + routineHour - counter * routineHour);
                BeginDate = DateTime.Now.Date.AddHours(BegHour);
            }
            else
            {
                BeginDate = DateTime.Now.Date.AddHours(BegHour);
            }
            return BeginDate.ToString("yyyy-MM-dd HH:mm");
        }
        public static string SetHour(int routineHour, DateTime BeginDate)
        {
            int beginhHour = BeginDate.Hour;
            if (BeginDate.Year == 1)
            {
                BeginDate = DateTime.Now;
            }
            int hStart = beginhHour - routineHour;
            hStart = (int)Math.Round((double)hStart / routineHour) * routineHour;
            hStart = 6 + hStart;
            hStart = (hStart > beginhHour) ? hStart - routineHour : hStart;
            hStart = (hStart < -12) ? 6 : hStart;
            BeginDate = BeginDate.Date.AddHours(hStart);
            return BeginDate.ToString("yyyy-MM-dd HH:mm");
        }

        [HttpPost]
        public JsonResult GetPrintHistoryOrders20(PrintOrderModel filter = null)
        {
            filter = filter ?? new PrintOrderModel { StartDate = new DateTime(2018, 1, 4, 6, 0, 0) };
            string[] filterLine = filter.LineName != null ? filter.LineName.Split(',') : new string[] { };
            List<PrintOrderModel> list = uow.PrintHistoryRepo.GetForPrint20(filter.RoutineId, filter.StartDate, filterLine, filter.OrderNo, filter.PNCCode);

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetPrintHistoryOrders(PrintOrderModel filter = null)
        {
            filter = filter ?? new PrintOrderModel { StartDate = new DateTime(2018, 1, 4, 6, 0, 0) };
            string[] filterLine = filter.LineName != null ? filter.LineName.Split(',') : new string[] { };
            //List<ProductionOrder> list = uow.ProductionOrderRepo.GetListFiltered(filter.StartDate, filter.StartDate.AddDays(4), filterLine, filter.OrderNo, filter.PNCCode).ToList();
            List<PrintOrderModel> list = uow.PrintHistoryRepo.GetForPrint(filter.RoutineId, filter.StartDate, filterLine, filter.OrderNo, filter.PNCCode);

            //foreach(ProductionOrder po in list)
            //{
            //    list2.Add(new PrintOrderModel {
            //        Id = po.Id,
            //        LineName = po.Line.Name,
            //        OrderNo = po.OrderNumber,
            //        PNCCode = po.Pnc.Code,
            //        Qty = po.
            //    });
            //}

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdatePlan(DateTime date)
        {
            List<ProductionOrder> poList = uow.ProductionOrderRepo.GetList().Where(x => DbFunctions.TruncateTime(x.StartDate) == date).ToList();
            foreach (ProductionOrder po in poList)
            {
                uow.ProdOrder20Repo.AddOrUpdate_Orders20(po);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RecalculatePlan20(int days = 2)
        {
            DateTime dateStart = DateTime.Now.Date; //new DateTime(2018, 5, 1);
            DateTime dateEnd = dateStart.AddDays(days);

            IQueryable<ProductionOrder> po1 = uow.ProductionOrderRepo.GetList()
                .Where(x => (dateStart <= DbFunctions.TruncateTime(x.EndDate) && DbFunctions.TruncateTime(x.StartDate) < dateEnd));
            IQueryable<ProductionOrder> po2 = uow.ProdOrder20Repo.GetList()
                .Where(x => dateStart <= DbFunctions.TruncateTime(x.PartStartDate) && DbFunctions.TruncateTime(x.PartStartDate) < dateEnd).Select(o => o.Order);
            IQueryable<ProductionOrder> po3 = po1.Union(po2);

            List<ProductionOrder> poList = po3.Distinct().ToList();
            //List<ProductionOrder> poList2 = uow.ProductionOrderRepo.GetList()
            //    .Where(x => (dateStart <= DbFunctions.TruncateTime(x.EndDate) && DbFunctions.TruncateTime(x.StartDate) < dateEnd)).ToList();

            Stopwatch stopwatch = Stopwatch.StartNew();
            
            foreach (ProductionOrder po in poList)
            {
                uow.ProdOrder20Repo.AddOrUpdate_Orders20(po);
            }

            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);

            return Json(stopwatch.ElapsedMilliseconds, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SynchronizeOldPFEPdb()
        {
            AlertModel alert = uow.RepoAncPackage.SynchronizeWithOldDB();
            return Json(alert);
        }

        //--------------PRINTING------------------------
        [HttpPost, AllowAnonymous]
        public ActionResult SendToPrinter(int routine, string printerName, int[] ProdOrders)
        {
            ActionAsPdf pdfResult = null;
            string s = string.Join(",", ProdOrders);
            pdfResult = new ActionAsPdf("Print", new { area = "PFEP", routine = routine, sProdOrders = s })
            {
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = new Rotativa.Options.Margins(3,3,3,3)
            };
            var binary = pdfResult.BuildFile(this.ControllerContext); //change from BuildPDF 20181006

            //new Printer().PrintRawStream(printerName, new MemoryStream(binary), "Print.pdf", false);
            //new Printer().PrintRawFile(printerName,"Print.pdf", false);
            return RedirectToAction("Print", new { routine = routine, sProdOrders = s });
        }
        [HttpPost, AllowAnonymous]
        public JsonResult SavePDF(int routine, string printerName, int[] ProdOrders)
        {
            ActionAsPdf pdfResult = null;
            DateTime dt1 = DateTime.Now;
            string s = string.Join(",", ProdOrders);
            string dt = dt1.Year.ToString() + dt1.Month.ToString() + dt1.Day.ToString() + dt1.Hour.ToString() + dt1.Minute.ToString() + dt1.Second.ToString();
            string fileName = "Print_" + dt + ".pdf";
            string filePath = "Uploads/PfepPrints";
            string path = Path.Combine(HostingEnvironment.MapPath("~/" + filePath), fileName);

            pdfResult = new ActionAsPdf("Print", new { area = "PFEP", routine = routine, sProdOrders = s })
            {
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = new Rotativa.Options.Margins(3, 3, 3, 3),
                FileName = "PFEP_Printout_" + dt
            };

            byte[] binary = pdfResult.BuildFile(this.ControllerContext); //change from BuildPDF 20181006
            MemoryStream ms = new MemoryStream(binary);

            using (var fileStream = System.IO.File.Create(path))
            {
                ms.Seek(0, SeekOrigin.Begin);
                ms.CopyTo(fileStream);
            }

            return Json(new string[] { filePath, fileName });
        }
        [HttpPost, AllowAnonymous]
        public ActionResult ViewPDF(int routine, int[] ProdOrders, bool orders20mode = true)
        {
            ProdOrders = ProdOrders != null ? ProdOrders : new int[0] { };

            DateTime printDate = DateTime.Now;
            Routine routineObj = Routines.GetRoutines().First(x => x.Id == routine);
            List<ProductionOrder> orders = PrepareOrders(routineObj, ProdOrders, orders20mode);

            PrintDefViewModel vm = PrepareVmForPrintMatrix(routineObj, orders);
            UpdatePrintHistory(routine, ProdOrders, orders20mode, printDate);

            return View("PrintMatrix", vm);
        }
        [HttpPost, AllowAnonymous]
        public ActionResult ViewPDF2(PrintDefViewModel vm)
        {
            return View("PrintMatrix", vm);
        }
        [AllowAnonymous]
        public ActionResult Print(int routine, string sProdOrders, bool orders20mode = true)
        {
            int[] prodOrders = ConvertStringChainToIntArray(sProdOrders, ',');

            DateTime printDate = DateTime.Now;
            Routine routineObj = Routines.GetRoutines().First(x => x.Id == routine);
            List<ProductionOrder> orders = PrepareOrders(routineObj, prodOrders, orders20mode);

            PrintDefViewModel vm = PrepareVmForPrintMatrix(routineObj, orders);
            UpdatePrintHistory(routine, prodOrders, orders20mode, printDate);

            return View("PrintMatrix", vm);
        }
        [AllowAnonymous]
        public ActionResult PrintView(int routine, string sOrders)
        {
            ViewBag.Routine = routine;
            ViewBag.Orders = sOrders;
            return View();
        }
        [AllowAnonymous]
        public ActionResult PrintViewAuto(DateTime date, int shift, string lineName)
        {
            if(!(shift > 0 && shift <= 3)) { return View(); }

            PrintDefAutoViewModel vm = new PrintDefAutoViewModel();
            List<Routine> routines = Routines.GetRoutines();
            
            vm.Date = date;
            vm.Shift = shift;
            vm.Routines = routines;
            vm.PrintDefsViewModels = new List<PrintDefViewModel>();

            foreach(Routine routine in routines)
            {
                PrintDefViewModel vmPrintDef = new PrintDefViewModel();
                DateTime dateFrom = date.Date.AddHours(6 + ((shift - 1) * 8)).AddHours(routine.AutoprintShiftHours);
                DateTime dateTo = new CapacityRepo(db2).GetEndDateOfProductionInterval(dateFrom, lineName, routine.Hours);
                List<ProductionOrder> orders = new List<ProductionOrder>();

                if (lineName == "134")
                {
                    if(routine.PrintAllLinesTogether == true)
                        orders = uow.ProductionOrderRepo.GetByTimeRange(dateFrom, dateTo);
                }
                else
                {
                    if(routine.PrintAllLinesTogether == false)
                        orders = uow.ProductionOrderRepo.GetByTimeRangeAndLine(dateFrom, dateTo, new string[] { lineName }).ToList();
                }

                if (orders.Count > 0)
                {
                    UpdatePrintHistory(routine.Id, orders.Select(x => x.Id).ToArray(), false, DateTime.Now);
                    vmPrintDef = PrepareVmForPrintMatrix(routine, orders);
                    vm.PrintDefsViewModels.Add(vmPrintDef);
                }
            }

            return View(vm);
        }

        private PrintDefViewModel PrepareVmForPrintMatrix(Routine routineObj, List<ProductionOrder> SelectedOrders)
        {
            PrintDefViewModel vm = new PrintDefViewModel();
            vm.Defs = "";
            vm.DateRange = CalculateDateTimeRangeOfSelectedOrders(SelectedOrders);
            vm.PrintNumber = 1;
            vm.Routine = routineObj.ToString();
            vm.PrintDate = DateTime.Now;
            vm.PrintDataMatrixes = new List<PrintDataMatrix>();
            vm.RoutineObj = routineObj;
            vm.Lines = string.Join(",", SelectedOrders.Select(x => x.Line).Select(y => y.Name).Distinct().ToList());
            var line = SelectedOrders.Select(x => x.Line).FirstOrDefault();
            vm.LineId = line != null? line.Id : 0;
             
            vm.PrintDataMatrixes.AddRange(PrepareSplitedPrintDataMatrix(routineObj, SelectedOrders, vm.LineId));

            return vm;
        }

        private List<PrintDataMatrix> PrepareSplitedPrintDataMatrix(Routine routineObj, List<ProductionOrder> SelectedOrders, int lineId)
        {
            List<PrintDataMatrix> printDataMatrixes = new List<PrintDataMatrix>();

            int opp = routineObj.ShowOrders ? 10 : 999;
            int i = 0;
            while (i < SelectedOrders.Count)
            {
                List<ProductionOrder> SelectedOrders1 = SelectedOrders.Skip(i).Take(opp).ToList();
                printDataMatrixes.Add(PreparePrintDataMatrix(routineObj, SelectedOrders1, lineId));
                i += opp;
            }

            return printDataMatrixes;
        }
        private PrintDataMatrix PreparePrintDataMatrix(Routine routineObj, List<ProductionOrder> SelectedOrders1, int lineId)
        {
            PrintDataMatrix printDataMatrix; //= PrintMatrix_AssignANCsToOrders(SelectedOrders1, routineObj);
            printDataMatrix = PrintMatrix_AssignANCsToOrders(SelectedOrders1, routineObj);
            printDataMatrix.Ancs = PrintMatrix_AssignPackageAndWorkstationToANCs(printDataMatrix.Ancs, lineId);
            printDataMatrix = RemoveAncsWithNoChangeOver(printDataMatrix, routineObj);
            printDataMatrix = RemoveWorkstaions(printDataMatrix, routineObj);

            return printDataMatrix;
        }
        private PrintDataMatrix PrintMatrix_AssignANCsToOrders(List<ProductionOrder> SelectedOrders, Routine rtn)
        {
            PrintDataMatrix PrintDataMatrix = new PrintDataMatrix();

            List<PrintModel_ANC> ancs;
            foreach (ProductionOrder po in SelectedOrders)
            {
                ancs = uow.RepoBomWorkorder.GetAncsByPNCAndRoutine(po, rtn).ToList();

                foreach (PrintModel_ANC anc in ancs)
                {
                    PrintDataMatrix.AddANCToOrder(po, anc);
                }
            }
            PrintDataMatrix.ORDER_Ancs = PrintDataMatrix.ORDER_Ancs.OrderBy(x => x.order.StartDate).ToList();
            return PrintDataMatrix;
        }
        private PrintDataMatrix RemoveAncsWithNoChangeOver(PrintDataMatrix pdm, Routine rtn)
        {
            foreach(var anc in pdm.Ancs)
            {
                if(rtn.ShowOnlyChanges && anc.Count >= pdm.ORDER_Ancs.Count)
                {
                    anc.Code = string.Empty;
                }
            }
            pdm.Ancs.RemoveAll(x => x.Code.Length < 1);
            return pdm;
        }
        private PrintDataMatrix RemoveWorkstaions(PrintDataMatrix pdm, Routine rtn)
        {
            foreach (var anc in pdm.Ancs)
            {
                if (rtn.Workstaitons != null && rtn.Workstaitons.Length > 0 && !(rtn.Workstaitons.Contains(anc.WorkstationName)))
                {
                    anc.Code = string.Empty;
                }
            }
            pdm.Ancs.RemoveAll(x => x.Code.Length < 1);
            return pdm;
        }
        private List<PrintModel_ANC> PrintMatrix_AssignPackageAndWorkstationToANCs(List<PrintModel_ANC> Ancs, int LineId)
        {
            AncFixedLocationRepo ancFixedLocationRepo = new AncFixedLocationRepo(db);

            foreach (PrintModel_ANC anc in Ancs)
            {
                PackageItem package = uow.RepoAncPackage.GetByItemId(anc.Id);
                anc.SetPackageQtyPerBox(package);
                anc.SetPackagesCount(package);
                anc.SetWorkstation(uow.RepoAncWorkstation.GetByAncIdAndLineId(LineId, anc.Id));
                anc.SetLocation(ancFixedLocationRepo.GetByAncId(anc.Id));
            }
            Ancs = Ancs.OrderBy(x => x.WorkstationOrder).ThenBy(x => x.WorkstationName).ThenBy(x => x.Code).ToList();
            return Ancs;
        }
        private List<ProductionOrder> PrepareOrders(Routine routineObj, int[] ProdOrders, bool orders20mode)
        {
            ProdOrders = ProdOrders != null ? ProdOrders : new int[0] { };

            List<ProductionOrder> SelectedOrders = null;

            if (orders20mode)
            {
                List<Prodorder20> selectedOrders20 = uow.ProdOrder20Repo.GetListByIds(ProdOrders);
                SelectedOrders = routineObj.ShowFullOrders ?
                    SelectedFullProdOrdersFromProdOrders20(selectedOrders20) : SelectedProdOrdersFromProdOrders20(selectedOrders20);
            }
            else
            {
                SelectedOrders = uow.ProductionOrderRepo.GetListByIds(ProdOrders);
            }

            return SelectedOrders;
        }
        private List<ProductionOrder> SelectedProdOrdersFromProdOrders20(List<Prodorder20> selectedOrders20)
        {
            //tutaj powstaje lista zaznaczonych zlecen bazując na zleceniach rozbitych po 20sztuk
            //zaznaczone zlecenia nie otrzymują parametrów dokładnie takich samych jakie mają w bazie ponieważ
            //wybrana może być tylko część spośród wszystkich dostępnych 20stek
            List<ProductionOrder> SelectedOrders = new List<ProductionOrder>();
            ProductionOrder prodOrder;
            foreach (Prodorder20 prodOrder20 in selectedOrders20)
            {
                prodOrder = SelectedOrders.FirstOrDefault(x => x.Id == prodOrder20.OrderId);

                if (prodOrder == null)
                {
                    prodOrder = prodOrder20.Order;
                    prodOrder.QtyPlanned = prodOrder20.Order.QtyPlanned;
                    prodOrder.QtyRemain = prodOrder20.PartQtyRemain;
                    prodOrder.StartDate = prodOrder20.PartStartDate;
                    SelectedOrders.Add(prodOrder);
                }
                else
                {
                    prodOrder.QtyRemain += prodOrder20.PartQtyRemain;
                }
            }
            return SelectedOrders;
        }
        private List<ProductionOrder> SelectedFullProdOrdersFromProdOrders20(List<Prodorder20> selectedOrders20)
        {
            //tutaj powstaje lista zaznaczonych zlecen bazując na zleceniach rozbitych po 20sztuk
            //zaznaczanie do pełnych zleceń
            List<ProductionOrder> SelectedOrders = new List<ProductionOrder>();

            List<int> orderIds = selectedOrders20.Select(x => x.OrderId).Distinct().ToList();

            //SelectedOrders.AddRange(selectedOrders20.Select(x => x.Order).Distinct().ToList());
            foreach(int orderId in orderIds)
            {
                SelectedOrders.Add(selectedOrders20.FirstOrDefault(x => x.OrderId == orderId).Order);
            }

            
            return SelectedOrders;
        }
        private string CalculateDateTimeRangeOfSelectedOrders(List<Prodorder20> selectedOrders20)
        {
            if (selectedOrders20.Count > 0)
                return selectedOrders20[0].PartStartDate.ToShortTimeString() + " - " + selectedOrders20[selectedOrders20.Count - 1].PartStartDate.ToShortTimeString();
            else
                return string.Empty;
        }
        private string CalculateDateTimeRangeOfSelectedOrders(List<ProductionOrder> selectedOrders)
        {
            if (selectedOrders.Count > 0)
                return selectedOrders[0].StartDate.ToShortTimeString() + " - " + selectedOrders[selectedOrders.Count - 1].StartDate.ToShortTimeString();
            else
                return string.Empty;
        }
        private int[] ConvertStringChainToIntArray(string sProdOrders, char spliter)
        {
            string[] ProdOrders1 = sProdOrders.Split(spliter).ToArray();
            int[] ProdOrders2 = new int[ProdOrders1.Length];

            int i = 0;
            foreach (string val in ProdOrders1)
            {
                ProdOrders2[i] = Convert.ToInt32(val);
                i++;
            }

            return ProdOrders2;
        }
        private void UpdatePrintHistory(int routine, int[] ProdOrders, bool orders20mode, DateTime printDate)
        {
            foreach (int orderId in ProdOrders)
            {
                if (orders20mode)
                    uow.PrintHistoryRepo.AddOrUpdatePrintData20order(routine, orderId, User.Identity.GetUserId(), printDate);
                else
                    uow.PrintHistoryRepo.AddOrUpdatePrintDataFullOrder(routine, orderId, User.Identity.GetUserId(), printDate);
            }
        }

        //--------------AUTOCOMPLETES------------------------
        [HttpPost]
        public JsonResult LineAutocomplete(string Prefix)
        {
            List<string> LineAutocomplete = uow.RepoLineExtend.GetNameList(Prefix);
            return Json(LineAutocomplete, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult OrderNumberAutocomplete(string Prefix)
        {
            List<string> ProductionOrderAutocomplete = uow.ProductionOrderRepo.GetProductionOrderList(Prefix);
            return Json(ProductionOrderAutocomplete, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult PNCAutocomplete(string Prefix)
        {
            List<string> PncIdAutoComplete = uow.ProductionOrderRepo.GetPncIdList(Prefix);
            return Json(PncIdAutoComplete, JsonRequestBehavior.AllowGet);
        }
    }
}



//public ActionResult PrintOrders_File(int routine, int[] ProdOrders)
//{
//    return new Rotativa.ActionAsPdf("Print", new { routine = routine, ProdOrders = ProdOrders });
//}
//public ActionResult PrintView()
//{
//    return View();
//}
//private PrintDefViewModel PreparePrint(int routine, int[] ProdOrders)
//{
//    List<ProductionOrder> SelectedOrders = uow.ProductionOrderRepo.GetList().Where(x => ProdOrders.Contains(x.Id)).ToList();
//    Routine rtn = Routines.GetRoutines().First(x => x.Id == routine);

//    PrintDefViewModel vm = new PrintDefViewModel();
//    vm.Defs = "";
//    vm.DateRange = "";
//    vm.PrintNumber = 1;
//    vm.Routine = rtn.ToString();
//    vm.PrintDate = DateTime.Now;
//    vm.PrintData = new PrintData();

//    List<Anc2> ancs;
//    foreach (ProductionOrder po in SelectedOrders)
//    {
//        ancs = uow.RepoBom.GetAncsByPNCAndRoutine(po.Pnc, rtn).ToList();

//        foreach (Anc2 anc in ancs)
//        {
//            vm.PrintData.AddOrderToANC(anc, po);
//            vm.PrintData.AddANCToOrder(po,anc);
//        }
//    }
//    vm.PrintData.ORDER_Ancs = vm.PrintData.ORDER_Ancs.OrderBy(x => x.order.StartDate).ToList();
//    PrintDataPreparator pdp = new PrintDataPreparator();

//    vm.PrintData.ANCs_Orders_ManyToMany = rtn.PrintType == 1 ? pdp.PrepareDataView1(vm.PrintData.ORDER_Ancs) : pdp.PrepareDataView2(vm.PrintData.ORDER_Ancs);

//    foreach(PrintModelANCs_Orders pmao in  vm.PrintData.ANCs_Orders_ManyToMany)
//    {
//        for(int u=0;u< pmao.Ancs.Count;u++)
//        {
//            pmao.Ancs[u].SetPackage(uow.RepoAncPackage.GetByAncId(pmao.Ancs[u].Id), pmao.Sum);
//            pmao.Ancs[u].SetWorkstation(uow.RepoAncWorkstation.GetByAncId(pmao.Ancs[u].Id));
//        }

//        pmao.Ancs = pmao.Ancs.OrderBy(x => x.WorkstationOrder).ToList();
//    }


//    return vm;
//}