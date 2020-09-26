using _MPPL_WEB_START.Areas._APPWEB.Models;
using _MPPL_WEB_START.Areas._APPWEB.ViewModels;
using _MPPL_WEB_START.Areas.CORE.ViewModels;
using _MPPL_WEB_START.Areas.iLOGIS.Controllers;
using _MPPL_WEB_START.Areas.ONEPROD.APS.ViewModels;
using _MPPL_WEB_START.Areas.ONEPROD.Models;
using Autofac;
using Autofac.Integration.Mvc;
using MDL_BASE.ComponentBase.Entities;
using MDL_BASE.Models.IDENTITY;
using MDL_BASE.Models.MasterData;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Mappers;
using MDL_ONEPROD;
using MDL_ONEPROD.ComponentCORE.Models;
using MDL_ONEPROD.ComponentMes.Etities;
using MDL_ONEPROD.ComponentMes.Etities.MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.ComponentMes.Models;
using MDL_ONEPROD.ComponentMes.ViewModels;
//using MDL_ONEPROD.ComponentMes.ViewModels;
using MDL_ONEPROD.ComponetMes.Entities;
using MDL_ONEPROD.ComponetMes.Models;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.Model.Scheduling.Interface;
using MDL_ONEPROD.Repo;
using MDLX_CORE.Model;
using MDLX_CORE.Model.PrintModels;
using MDLX_MASTERDATA.Entities;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using XLIB_COMMON.Enums;
using XLIB_COMMON.Interface;
using XLIB_COMMON.Model;

namespace _MPPL_WEB_START.Areas.ONEPROD.Controllers
{
    [System.Web.Mvc.Authorize(Roles = DefRoles.ONEPROD_MES_OPERATOR)]
    public partial class MESController : BaseController
    {
        public MESController(IDbContextOneprodMes dbMes, IDbContextOneProdOEE dbOEE = null, IBarcodeParser barcodeParser = null)
        {
            this.dbMes = dbMes;
            this.uowMes = new UnitOfWorkOneProdMes(dbMes);
            this.uowOneProd = new UnitOfWorkOneprod(dbMes);
            this.uowOEE = new UnitOfWorkOneProdOEE(dbOEE);
            this.barcodeParser = barcodeParser != null ? barcodeParser : new BarcodeManager();
            ViewBag.Skin = "nasaSkin";
        }

        private readonly IDbContextOneprodMes dbMes;
        private IBarcodeParser barcodeParser;
        private UnitOfWorkOneProdMes uowMes;
        private UnitOfWorkOneProdOEE uowOEE;
        private UnitOfWorkOneprod uowOneProd;

        //-------------------------------WORKPLACE------------------------------------
        public ActionResult WorkplaceSelector()
        {
            List<WorkplaceViewModel> vm = new List<WorkplaceViewModel>();
            vm = uowMes.WorkplaceRepo.GetList()
                .OrderBy(x => x.Machine.ResourceGroupOP.StageNo)
                .ThenBy(x => x.Machine.ResourceGroupOP.Name)
                .ToList<WorkplaceViewModel>();
            return View(vm);
        }

        public ActionResult Workplace(int id = 0, string ip = "")
        {
            bool verifyIp = AppClient.appClient != null && AppClient.appClient.SettingsONEPROD != null ? AppClient.appClient.SettingsONEPROD.MesWorkplaceVerifyIP : false;

            Workplace wp = verifyIp ? uowMes.WorkplaceRepo.GetByIP(ip) : uowMes.WorkplaceRepo.GetById(id);
            WorkplaceViewModel vm = new WorkplaceViewModel();
            //vm.SelectedWorkplaceId = id;
            //vm.IP = ip;

            if (wp == null)
            {
                vm = new WorkplaceViewModel { Id = -1, Name = "PC nieznany", ResourceId = 0, ResourceName = "N/A" };
                return View(vm);
            }
            else
            {
                //vm.DateFrom = DateTime.Now.Date.AddDays(-80);
                //vm.DateTo = DateTime.Now.Date.AddDays(300);
                vm = wp.CastToViewModel<WorkplaceViewModel>();
                //vm.ReasonTypes = uowMes.
                //if (id < 1)
                //{
                //    //vm.Workplaces = uowMes.WorkplaceRepo.GetList().ToList();
                //    return Redirect("WorkplaceSelector");
                //}
                return View(vm);
            }
        }

        public ActionResult WorkplaceTest(int id = 0, string ip = "")
        {
            Workplace wp = id > 0 ? uowMes.WorkplaceRepo.GetById(id) : uowMes.WorkplaceRepo.GetByIP(ip);
            WorkplaceViewModel vm = new WorkplaceViewModel();
            //vm.SelectedWorkplaceId = id;
            //vm.IP = ip;

            if (wp == null)
            {
                if (id < 1)
                {
                    return Redirect("WorkplaceSelector");
                }
                else
                {
                    vm = new WorkplaceViewModel { Id = -1, Name = "PC nieznany", ResourceId = 0, ResourceName = "N/A" };
                    return View("Workplace", vm);
                }
            }
            else
            {
                //vm.DateFrom = DateTime.Now.Date.AddDays(-80);
                //vm.DateTo = DateTime.Now.Date.AddDays(300);
                vm = wp.CastToViewModel<WorkplaceViewModel>();
                return View("Workplace", vm);
            }
        }

        [HttpPost]
        public ActionResult WorkplaceWorkorder(DateTime dateFrom, DateTime dateTo, int workplaceId, int workorderId = 0, int pageIndex = 1, int pageSize = 30)
        {
            Workplace w = uowMes.WorkplaceRepo.GetById(workplaceId);
            List<WorkorderViewModel> tasksVM = new List<WorkorderViewModel>();

            if (w != null)
            {
                List<Workorder> workorders = new List<Workorder>();
                int startIndex = Math.Max(pageIndex - 1, 0) * pageSize;

                if (pageIndex == 1 && workorderId == 0)
                {
                    workorders.AddRange(uowMes.WorkorderRepo
                        .GetWorkordersOfResource(w.Machine.Id, dateFrom.AddYears(-1), dateTo.AddDays(60))
                        .Where(x => x.Qty_Produced >= x.Qty_Total)
                        .OrderByDescending(x => x.StartTime)
                        .Take(5)
                        .ToList().OrderBy(x => x.StartTime).ToList());

                    //pageSize -= workorders.Count;
                }

                workorders.AddRange(uowMes.WorkorderRepo
                    .GetWorkordersOfResource(w.Machine.Id, dateFrom.AddYears(-1), dateTo.AddDays(60))
                    .Where(x => (workorderId == 0 && x.Qty_Produced < x.Qty_Total) || x.Id == workorderId)
                    .Skip(startIndex)
                    .Take(pageSize)
                    .ToList());

                foreach (Workorder wo in workorders)
                {
                    tasksVM.Add(new WorkorderViewModel
                    {
                        Id = wo.Id,
                        BatchNumber = wo.BatchNumber,
                        DueDate = wo.DueDate,
                        EndTime = wo.EndTime,
                        Line = wo.ClientOrder != null ? wo.ClientOrder.Resource.ToString() : wo.Resource.Name,
                        MachineId = wo.ResourceId ?? 0,
                        OrderNumber = wo.ClientOrder != null ? wo.ClientOrder.OrderNo : wo.UniqueNumber,
                        Param1 = wo.Param1,
                        Param2 = wo.Param2,
                        PartCode = wo.Item.Code,
                        PartId = wo.ItemId ?? 0,
                        PartName = wo.Item.Name,
                        ProcessingTime = wo.ProcessingTime,
                        Qty_Produced = wo.Qty_Produced,
                        Qty_Total = wo.Qty_Total,
                        Qty_Used = wo.Qty_Used,
                        ReleaseDate = wo.ReleaseDate,
                        StartTime = wo.StartTime,
                        Status = wo.Status,
                        UniqueTaskNumber = wo.UniqueNumber
                    });
                }
            }

            return View(tasksVM);
        }

        [HttpPost]
        public JsonResult SetSelectedWorkorder(int resourceId, int workorderId)
        {
            Workplace wrkpl = uowMes.WorkplaceRepo.GetListByMachine(resourceId).Take(1).FirstOrDefault();
            Workorder task = uowMes.WorkorderRepo.GetById(workorderId);

            uowMes.WorkplaceRepo.SetSelectedTask(wrkpl, task);

            return Json(1);
        }

        //-------------------------------RawMaterialProductionLog------------------------------------
        [HttpPost]
        public JsonResult GetRawMaterials(int selectedProductionLogId)
        {
            //uowMes.ProductionLogRawMaterialRepo.GetByProductionLogId(selectedProductionLogId);
            List<ProductionLogRawMaterial> rmList = new List<ProductionLogRawMaterial>() { };
            rmList.Add(new ProductionLogRawMaterial() { ProductionLogId = selectedProductionLogId, PartCode = "Kod partii", UsedQty = 13.4M, BatchSerialNo = "Numer" });
            return Json(rmList);
        }

        [HttpGet]//, Authorize(Roles = DefRoles.TechnologyOperator)]
        public ActionResult PlanMonitor()
        {
            PlanViewModel vm = new PlanViewModel();
            vm.DateFrom = DateTime.Now.Date.AddDays(-80);
            vm.DateTo = DateTime.Now.Date.AddDays(300);
            vm.Machines = new SelectList(uowMes.ResourceRepo.GetResources().ToList(), "Id", "Name");
            return View(vm);
        }

        [HttpPost]//, Authorize(Roles = DefRoles.TechnologyOperator)]
        public ActionResult PlanMonitor(PlanViewModel vm)
        {
            if (vm.SelectedMachineId > 0)
            {
                RepoPreprodConf repoConf = new RepoPreprodConf(dbMes);
                vm.Machines = new SelectList(uowMes.ResourceRepo.GetResources().ToList(), "Id", "Name", vm.SelectedMachineId);
                vm.SelectedMachine = uowMes.ResourceRepo.GetById(vm.SelectedMachineId);

                if (vm.SelectedMachine != null)
                {
                    vm.Tasks = uowMes.WorkorderRepo.GetWorkordersOfResource(vm.SelectedMachineId, vm.DateFrom.AddYears(-1), vm.DateTo).ToList();
                }
            }
            return View(vm);
        }

        [HttpGet]//, Authorize(Roles = DefRoles.TechnologyOperator)]
        public ActionResult PlanMonitorArea()
        {
            PlanMonitorAreaViewModel vm = new PlanMonitorAreaViewModel();
            vm.DateFrom = DateTime.Now.Date.AddHours(-4); //.AddDays(-80);
            vm.DateTo = DateTime.Now.Date.AddDays(2);
            vm.Areas = new SelectList(uowMes.ResourceGroupRepo.GetList().ToList(), "Id", "Name");
            return View(vm);
        }

        [HttpPost]//, Authorize(Roles = DefRoles.TechnologyOperator)]
        public ActionResult PlanMonitorArea(PlanMonitorAreaViewModel vm)
        {
            if (vm.SelectedAreaId > 0)
            {
                RepoPreprodConf repoConf = new RepoPreprodConf(dbMes);
                vm.Areas = new SelectList(uowMes.ResourceGroupRepo.GetList().ToList(), "Id", "Name", vm.SelectedAreaId);
                vm.SelectedArea = uowMes.ResourceGroupRepo.GetById(vm.SelectedAreaId);
                vm.Tasks = new List<Workorder>();

                if (vm.SelectedArea != null)
                {
                    vm.MachinesList = uowMes.ResourceRepo.GetListByGroup(vm.SelectedArea.Id).ToList();
                    vm.MachineCount = vm.MachinesList.Count;
                    foreach (ResourceOP m in vm.MachinesList)
                    {
                        vm.Tasks.AddRange(uowMes.WorkorderRepo.GetWorkordersOfResource(m.Id, vm.DateFrom, vm.DateTo));
                    }
                }
            }
            return View(vm);
        }

        [HttpGet]
        public PartialViewResult PlanMonitorWorkorder(Workorder workorder)
        {
            Workorder task2 = uowMes.WorkorderRepo.GetById(workorder.Id);
            return PartialView(task2);
        }

        //-------------------------------PRINT-&-DECLARE------------------------------------
        [HttpPost]
        public JsonResult GenerateSerialNumber(int resourceId)
        {
            //int serialNumber = uowMes.ProductionLogRepo.GenerateSerialNumber(resourceId);
            string serialNumber = OneprodSerialNumberManager.GenerateSerialNumber(dbMes, resourceId);
            return Json(serialNumber);
        }

        [HttpPost]
        public JsonResult PrintoutLabel(int workplaceId, int workorderId, int qty, int serialNo)
        {
            Workplace wrkp = uowMes.WorkplaceRepo.GetById(workplaceId);
            Workorder wo = uowMes.WorkorderRepo.GetById(workorderId);

            bool isPrinted = false;

            if (wrkp.PrintLabel)
            {
                isPrinted = new PrintLabelManager().PrepareAndPrintLabel(wrkp, wo.UniqueNumber, wo.Item.Code, serialNo, qty);
            }
            else
            {
                isPrinted = true;
            }

            return Json(isPrinted);
        }

        [HttpPost]
        public JsonResult PrintLabel(int workplaceId, int workorderId, int qty)
        {
            Workplace wrkp = uowMes.WorkplaceRepo.GetById(workplaceId);
            Workorder wo = uowMes.WorkorderRepo.GetById(workorderId);
            string serialNumber = "0";

            PrintLabelModelAbstract p = null;
            switch (wrkp.PrinterType)
            {
                case PrinterType.Zebra: p = new PrintLabelModelZEBRA(wrkp.PrinterIPv4); break;
                case PrinterType.CAB: p = new PrintLabelModelCAB(wrkp.PrinterIPv4); break;
            }

            if (p != null && wrkp.PrintLabel == true)
            {
                //serialNumber = uowMes.ProductionLogRepo.GenerateSerialNumber(Convert.ToInt32(wrkp.Machine.ResourceGroupId));
                serialNumber = OneprodSerialNumberManager.GenerateSerialNumber(dbMes, Convert.ToInt32(wrkp.Machine.ResourceGroupId));

                LabelData labelData = new LabelData();
                labelData.Code = wo.Item.Code; //wrkp.LabelANC;
                labelData.Barcode = "";
                labelData.ClientName = "";
                labelData.MachineNumber = wrkp.LabelName;
                labelData.OrderNo = wo.UniqueNumber;
                labelData.PrintDateTime = DateTime.Now.ToString("dd-MM-yyyy;HH:mm");
                labelData.PrintDate = DateTime.Now.ToString("dd-MM-yyyy");
                labelData.PrintTime = DateTime.Now.ToString("HH:mm:ss");
                labelData.SerialNumber = serialNumber;
                labelData.Qty = qty.ToString();

                p.PrepareLabelFromLayout(wrkp.LabelLayoutNo, labelData);
                p.Print();
            }

            return Json(serialNumber);
        }

        [HttpPost]
        public JsonResult PrintTestLabel(int workplaceId)
        {
            Workplace wrkp = uowMes.WorkplaceRepo.GetById(workplaceId);
            int serialNumber = 0;

            PrintLabelModelAbstract p = null;
            switch (wrkp.PrinterType)
            {
                case PrinterType.Zebra: p = new PrintLabelModelZEBRA(wrkp.PrinterIPv4); break;
                case PrinterType.CAB: p = new PrintLabelModelCAB(wrkp.PrinterIPv4); break;
            }

            if (p != null && wrkp.PrintLabel == true)
            {
                serialNumber = 0;

                LabelData labelData = new LabelData();
                labelData.Code = "999999999";
                labelData.Barcode = "";
                labelData.ClientName = "";
                labelData.MachineNumber = "99";
                labelData.OrderNo = "0"; //TODO: wo - trzeba pobrać numer z bazy
                labelData.PrintDateTime = DateTime.Now.ToString("dd-MM-yyyy;HH:mm");
                labelData.PrintDate = DateTime.Now.ToString("dd-MM-yyyy");
                labelData.PrintTime = DateTime.Now.ToString("HH:mm:ss");
                labelData.SerialNumber = serialNumber.ToString();

                p.PrepareLabelFromLayout(wrkp.LabelLayoutNo, labelData);
                p.Print();
            }

            return Json(serialNumber);
        }

        [HttpPost]
        public JsonResult TestPrint()
        {
            PrintLabelModelCAB p = new PrintLabelModelCAB("10.26.11.206");
            //p.PrepareData(DateTime.Now, "5", "32998", "A00056111");
            p.PrepareLabelFromLayout(1, new LabelData());
            p.Print();

            return Json(0);
        }

        //[Authorize(Roles = DefRoles.TechnologyOperator)]
        public ActionResult ConfirmWorkorder(int workorderId, int workplaceId = 0)
        {
            Workorder task = uowMes.WorkorderRepo.GetById(workorderId);
            ViewBag.WorkplaceId = workplaceId;
            //SaveProductionLog(workorderId, -1);
            return View(task);
        }

        [HttpPost]//, Authorize(Roles = DefRoles.TechnologyOperator)]
        public JsonResult ConfirmWorkorder(int workorderId, int qty = 0, int workplaceId = 0)
        {
            Workorder wo = uowMes.WorkorderRepo.GetById(workorderId);

            if (qty > 0 && wo != null)
            {
                uowMes.WorkorderRepo.ConfirmWorkorder(wo, qty);
                SaveProductionLog(wo, qty, "", workplaceId, EnumEntryType.Production);
            }
            string taskView = RenderViewToString(this.ControllerContext, "PlanMonitorWorkorder", uowMes.WorkorderRepo.GetById(workorderId));
            return Json(taskView);
        }

        [HttpPost]//, Authorize(Roles = DefRoles.TechnologyOperator)]
        public JsonResult DeleteWorkorder(int workorderId)
        {
            Workorder wo = uowMes.WorkorderRepo.GetById(workorderId);
            Workplace wp = uowMes.WorkplaceRepo.GetBySelectedTaskId(workorderId);
            wp.SelectedTask = null;

            if (wo != null)
            {
                uowMes.WorkplaceRepo.AddOrUpdate(wp);
                uowMes.WorkorderRepo.Delete(wo);
            }
            return Json("");
        }

        public ActionResult AddWorkorder(int workplaceId)
        {
            Workplace wrkp = uowMes.WorkplaceRepo.GetById(workplaceId);
            IEnumerable<SelectListItem> ItemsList = new SelectList(uowMes.CycleTimeRepo.GetItemsForResource(wrkp.MachineId), "Id", "GetCodeAndName");
            ViewBag.Items = ItemsList;
            return View(wrkp);
        }

        [HttpPost]
        public JsonResult AddWorkorder(int workplaceId, string itemCode, int qty)
        {
            ItemOP item = uowMes.ItemOPRepo.GetList().FirstOrDefault(x => x.Code == itemCode);
            Workorder wo = new Workorder();
            Workplace wp = uowMes.WorkplaceRepo.GetById(workplaceId);

            if (wp != null && item != null && qty > 0)
            {
                wo.ItemId = item.Id;
                wo.Item = item;
                wo.UniqueNumber = uowMes.WorkorderRepo.GenerateNewWONumber();
                wo.ResourceId = wp.MachineId;
                wo.Resource = wp.Machine;
                wo.Qty_Total = qty;
                wo.LV = 1;
                wo.ReleaseDate = DateTime.Now;
                wo.StartTime = DateTime.Now;
                wo.DueDate = DateTime.Now.AddHours(8);
                wo.EndTime = DateTime.Now.AddMonths(1);
                uowMes.WorkorderRepo.Add(wo);
            }

            return Json(wo);
        }

        [HttpPost]//, Authorize(Roles = DefRoles.TechnologyOperator)]
        public JsonResult ConfirmWorkorderByTrolleyQty(int workorderId, int qty = -1, string serialNo = "", int workplaceId = 0, EnumEntryType entryType = EnumEntryType.Production)
        {
            List<int> updatedWorkordersIds = new List<int>();
            List<int> createdProdLogsIds = new List<int>();
            List<WorkplaceBuffer> wbList = new List<WorkplaceBuffer>();
            Workplace workplace = uowMes.WorkplaceRepo.GetById(workplaceId);
            Workorder wo = uowMes.WorkorderRepo.GetById(workorderId);
            DateTime startTime = DateTime.Now;
            int limit = 100;
            int itemId = 0;
            decimal qtyTemp;
            int prodLogId;

            if (wo == null || workplace == null) { 
                return Json(new { updatedWorkordersIds }); 
            }

            List<string> bomChildsCodes = uowMes.BomRepo.GetChildsOfItem(wo.ItemId ?? 0).Select(x => x.Anc.Code).ToList();

            while (qty > 0 && limit > 0)
            {
                if (wo == null)
                {
                    wo = uowMes.WorkorderRepo.GetList()
                        .Where(x => x.StartTime > startTime && x.ItemId == itemId && x.ResourceId == workplace.MachineId)
                        .OrderBy(o => o.StartTime)
                        .Take(1)
                        .FirstOrDefault();
                }
                
                if(wo != null) 
                { 
                    qtyTemp = qty;
                    qty = uowMes.WorkorderRepo.WorkorderQty_Declare(wo, qty);

                    prodLogId = SaveProductionLog(wo, (int)qtyTemp - qty, serialNo, workplaceId, entryType);
                    GeneratePLogTraceabilityForChildCodes(bomChildsCodes, wo, prodLogId, workplace.MachineId, (int)qtyTemp - qty);
                    createdProdLogsIds.Add(prodLogId);

                    itemId = Convert.ToInt32(wo.ItemId);
                    startTime = wo.StartTime;
                    updatedWorkordersIds.Add(wo.Id);
                    wo = null;
                }
                limit--;
            }

            var context = GlobalHost.ConnectionManager.GetHubContext<WorkplaceHub>();
            context.Clients.Group("workplace_" + workplaceId).broadcastMessage("RefreshProdLogs");

            return Json(new JsonModel() { Data = new { updatedWorkordersIds, createdProdLogsIds } } );
        }

        private void GeneratePLogTraceabilityForChildCodes(List<string> bomChildsCodes, Workorder wo, int prodLogId, int resourceId, int qty)
        {
            List<WorkplaceBuffer> wbuffList = uowMes.WorkplaceBufferRepo.FindByWorkorderAndResource(wo.Id, resourceId, null).ToList();
            WorkplaceBuffer wbuff;

            foreach (string childCode in bomChildsCodes)
            {
                wbuff = wbuffList.FirstOrDefault(x => x.Child.Code == childCode);

                if (wbuff != null)
                {
                    wbuff.QtyAvailable = (wbuff.QtyAvailable - qty * wbuff.QtyInBom);
                    uowMes.WorkplaceBufferRepo.AddOrUpdate(wbuff);

                    SaveProductionLogTraceability(
                        prodLogId,
                        wbuff.ProductionLogId,
                        wbuff.ProductionLogId != null ? wbuff.ProductionLog.ItemCode : wbuff.Code,
                        wbuff.ProductionLogId != null ? wbuff.ProductionLog.SerialNo : wbuff.SerialNumber);
                }
                else
                {
                    //"jest lipa. brakuje kodu na buforze";
                }
            }   
        }

        private int SaveProductionLog(Workorder wo, int qty, string serialNo, int workplaceId, EnumEntryType entryType)
        {
            ProductionLog pLog = null;

            if (wo != null)
            {
                //20200625 zmiana bo deklarował 1szt + 120sztuk.
                //int qtyToSave = qty > 0 ? qty : wo.Qty_Total;
                int qtyToSave = qty;

                if (workplaceId > 0 && qtyToSave > 0)
                {
                    pLog = new ProductionLog();
                    pLog.ItemId = wo.Item.Id;
                    pLog.DeclaredQty = qtyToSave;
                    pLog.WorkorderTotalQty = wo.Qty_Total;
                    pLog.TimeStamp = DateTime.Now;
                    pLog.UserName = User.Identity.Name;
                    pLog.ClientWorkOrderNumber = wo.ClientOrder != null ? wo.ClientOrder.OrderNo : wo.UniqueNumber;
                    pLog.WorkplaceId = workplaceId;
                    pLog.SerialNo = serialNo;
                    pLog.ReasonTypeId = (int)entryType; //TODO: 20200329 to jest niebezpieczne. Nie zawsze istnieje w bazie odpowiedni typ pod ID = EntryTypeEnum
                    uowMes.ProductionLogRepo.AddOrUpdate(pLog);
                }
            }

            return pLog != null ? pLog.Id : 0;
        }

        private void SaveProductionLogTraceability(int parentProdLogId, int? childProdLogId, string itemCode = "", string serialNo = "")
        {
            ProductionLogTraceability plt = new ProductionLogTraceability();
            plt.ParentId = parentProdLogId;
            plt.ChildId = childProdLogId;
            plt.Deleted = false;
            plt.ItemCode = itemCode;
            plt.SerialNumber = serialNo;
            plt.ProductionDate = DateTime.Now;

            uowMes.ProductionLogTraceabilityRepo.AddOrUpdate(plt);
        }

        [HttpPost]
        public JsonResult GetProductionLogs(int workplaceId, DateTime dateFrom, DateTime dateTo, int pageIndex = 0, int pageSize = 12)
        {
            var prodLogQuery = uowMes.ProductionLogRepo.GetList()
                .Where(x => x.WorkplaceId == workplaceId &&
                    x.TimeStamp >= dateFrom &&
                    x.TimeStamp < dateTo)
                .OrderByDescending(x => x.Id);

            int count = prodLogQuery.Count();
            int startIndex = Math.Max((pageIndex - 1) * pageSize, 0);
            var ProdLogList = prodLogQuery.Skip(startIndex).Take(pageSize).ToList();

            var ProdQtyTotalGood = prodLogQuery.Where(x => x.ReasonType != null && x.ReasonType.EntryType == EnumEntryType.Production).DefaultIfEmpty().Sum(x => x != null ? x.DeclaredQty : 0);
            //var ProdQtyTotalScrapMaterial = query.Where(x => x.ReasonType.EntryType == EnumEntryType.ScrapMaterial).Sum(x => x.DeclaredQty);
            var ProdQtyTotalScrapProcess = prodLogQuery.Where(x => x.ReasonType != null && x.ReasonType.EntryType == EnumEntryType.ScrapProcess).DefaultIfEmpty().Sum(x => x != null ? x.DeclaredQty : 0);
            var ProdQtyTotalScrapLabel = prodLogQuery.Where(x => x.ReasonType != null && x.ReasonType.EntryType == EnumEntryType.ScrapLabel).DefaultIfEmpty().Sum(x => x != null ? x.DeclaredQty : 0);

            var prodLogs = ProdLogList
                .Select(pLog => new
                {
                    pLog.Id,
                    pLog.SerialNo,
                    ItemCode = pLog.Item.Code,
                    ReasonTypeName = pLog.ReasonType != null ? pLog.ReasonType.Name : string.Empty,
                    pLog.ReasonTypeId,
                    ReasonTypeEntryType = pLog.ReasonType != null ? pLog.ReasonType.EntryType : EnumEntryType.Undefined, //EntryType = (int)pLog.EntryType,
                    ReasonName = pLog.Reason != null? pLog.Reason.Name : "",
                    pLog.ClientWorkOrderNumber,
                    pLog.TimeStamp,
                    pLog.DeclaredQty
                    //ProdQtyTotalGood,
                    //ProdQtyTotalScrap
                }).ToList();

            var prodLogSummary = new[] {
                new { Qty = ProdQtyTotalGood, EntryType = EnumEntryType.Production },
                //new { Qty = ProdQtyTotalScrapMaterial, EntryType = EnumEntryType.ScrapMaterial },
                new { Qty = ProdQtyTotalScrapProcess, EntryType = EnumEntryType.ScrapProcess },
                new { Qty = ProdQtyTotalScrapLabel, EntryType = EnumEntryType.ScrapLabel }
            }.ToList();

            return Json(new { prodLogs, prodLogSummary });
        }

        [HttpPost]
        public JsonResult SetProductionReason(int id, int? reasonId, int reasonTypeId) //EnumEntryType entrytype)
        {
            ReasonType reasonType = uowOEE.ReasonTypeRepo.GetById(reasonTypeId);
            ProductionLog pLog = uowMes.ProductionLogRepo.GetById(id);

            if (pLog.ReasonType.EntryType == EnumEntryType.ScrapLabel)
            {
                //Nie mozna juz zmienic typu, bo wiązałoby się to z koniecznością ponownej deklaracji oddeklarowanych wpisów
                return Json(new { result = -1, data = 0 });
            }

            if (reasonType != null && pLog != null)
            {
                if (reasonType.EntryType != EnumEntryType.ScrapLabel)
                {
                    pLog.ReasonId = reasonId;
                    pLog.ReasonTypeId = reasonTypeId;
                    uowMes.ProductionLogRepo.AddOrUpdate(pLog);
                }
                else
                {
                    List<int> involvedWoIds = UndoDeclarations(reasonId, reasonTypeId, pLog);
                    UndoStockUnitCreation(pLog.SerialNo);

                    return Json(new { result = 1, data = involvedWoIds });
                }
            }

            return Json(new { result = 0, data = 0 });
        }

        private List<int> UndoDeclarations(int? reasonId, int reasonTypeId, ProductionLog pLog)
        {
            List<ProductionLog> productionLogsConnectedWithLabel = uowMes.ProductionLogRepo.GetList()
                .Where(x => x.ItemId == pLog.ItemId && x.SerialNo == pLog.SerialNo && x.WorkplaceId == pLog.WorkplaceId && DbFunctions.TruncateTime(x.TimeStamp) == pLog.TimeStamp.Date)
                .ToList();

            List<int> involvedWoIds = new List<int>();
            
            foreach (ProductionLog pl in productionLogsConnectedWithLabel)
            {
                if (pl.ReasonType != null && pl.ReasonType.EntryType != EnumEntryType.ScrapLabel) //tylko wpisy, które nie były wcześniej zescrapowane
                {
                    Workorder wo = uowMes.WorkorderRepo.GetList().FirstOrDefault(x =>
                        x.ItemId == pl.ItemId &&
                        ((x.ClientOrderId != null && x.ClientOrder.OrderNo == pl.ClientWorkOrderNumber) ||
                         (x.ClientOrderId == null && x.UniqueNumber == pl.ClientWorkOrderNumber))
                    );

                    if (wo != null)
                    {
                        uowMes.WorkorderRepo.WorkorderQty_Undeclare(wo, pl.DeclaredQty);
                        involvedWoIds.Add(wo.Id);
                    }

                    pl.ReasonId = reasonId;
                    pl.ReasonTypeId = reasonTypeId;
                    uowMes.ProductionLogRepo.AddOrUpdate(pl);
                }
            }

            return involvedWoIds;
        }
        private void UndoStockUnitCreation(string serialNumber)
        {
            IDbContextiLOGIS dbiLOGIS = AutofacDependencyResolver.Current.ApplicationContainer.ResolveOptional<IDbContextiLOGIS>();

            if(dbiLOGIS != null)
            {
                var c = new StockUnitController(dbiLOGIS);
                c.ControllerContext = this.ControllerContext;
                var response = c.DeleteStockUnit(serialNumber);
            }
        }

        //TODO: Wypierdolić to do MasterData
        [HttpPost]
        public JsonResult GetLabourBrigades()
        {
            List<LabourBrigade> lB = uowOneProd.LabourBrigadeRepo.GetList().ToList();
            return Json(lB);
        }

        [HttpPost, AllowAnonymous]
        public JsonResult GetCurrentProducedItem(int resourceId)
        {
            ProductionLog pLog = uowMes.ProductionLogRepo.GetList()
                .Where(x => x.Workplace.Machine.Id == resourceId &&
                            x.ReasonType.EntryType == EnumEntryType.Production)
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();

            return Json(new { pLog.Item.Code, pLog.Item.Name, pLog.ClientWorkOrderNumber });
        }

        //-------------------------------Traceability------------------------------------
        public ActionResult TraceabilityTreant()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetTreantBase(int productionLogId)
        {
            //productionLogId = 476052;
            TreantJsModel treantModel = new TreantJsModel(uowMes);
            TreantJsNode treantTree = new TreantJsNode();
            //TreantJsNode parent = treantModel.GetParent(productionLogId);
            List<TreantJsNode> childs = treantModel.GetChildNodes(productionLogId);
            treantTree = treantModel.ConnectParentWithChilds(productionLogId, childs);
            return Json(treantTree);
        }

        [HttpPost]
        public JsonResult GetChilds(int productionLogId)
        {
            //productionLogId = 476052;
            TreantJsModel treantModel = new TreantJsModel(uowMes);
            List<TreantJsNode> childs = treantModel.GetChildNodes(productionLogId);
            return Json(childs);
        }

        [HttpPost]
        public JsonResult SaveProductionLogTraceability(int prodLogId, int workorderId)
        {
            ProductionLog plParent = uowMes.ProductionLogRepo.GetById(prodLogId); // Nowo utworzony produkt
            string ItemCode = plParent.Item.Code;

            ProductionLogTraceability plt = new ProductionLogTraceability();
            string SerialNo = plParent.SerialNo;

            //TODO: 20200624 - może być kilka deklaracji z tego samego serialnumberu.
            ProductionLog plChild = uowMes.ProductionLogRepo.GetBySerialNumberAndItemCode(SerialNo, ItemCode).FirstOrDefault(); // Produkt z którego jest wykonanywany obecny
            if (plParent != null)
            {
                plt.Child = plChild;
                plt.Parent = plParent;
                plt.SerialNumber = plParent.SerialNo;
                plt.ItemCode = ItemCode;
                uowMes.ProductionLogTraceabilityRepo.AddOrUpdate(plt);
                return Json(plt);
            }
            else
            {
                return Json("");
            }
        }

        [HttpPost]
        public JsonResult CheckProperItemCode(string barcode, int workorderId)
        {
            //bool isItemCodeCorrect = ItemCode == ItemCodeFromBarcode ? true : false;
            string ItemCodeFromWorkOrder = uowMes.WorkorderRepo.GetById(workorderId).Item.Code;
            string ItemCodeFromBarcode = GetItemCodeFromBarcode(barcode);
            bool isItemCodeProper = ItemCodeFromWorkOrder == ItemCodeFromBarcode ? true : false;
            if (isItemCodeProper)
            {
                return Json("success");
            }
            else
            {
                return Json("error");
            }
        }

        [HttpGet]
        public ActionResult WorkplaceBufferView()
        {
            ViewBag.Layout = "null";
            return View();
        }
        [HttpGet]
        public ActionResult WorkplaceBufferViewMobile()
        {
            ViewBag.Layout = "";
            return View("WorkplaceBufferView");
        }

        [HttpPost]
        public JsonResult WorkplaceBufferParseBarcode(string barcode)
        {
            JsonModel jsonModel = new JsonModel();

            //TODO: qty nie moze byc pobierane po prostu z bacode, poniewaz po odstawieniu na bufor nikt nie zmienia etykiety na nowa
            //trzeba wykorzystac mechanizm lokalizacji
            barcodeParser.Parse(barcode, "");
            jsonModel.Data = new { barcodeParser };

            return Json(jsonModel);
        }

        [HttpPost]
        public JsonResult WorkplaceBufferAddItem(int workplaceId, int selectedWorkorderId, string itemCode, int qty, string serialNumber, string barcode)
        {
            JsonModel jsonModel = new JsonModel();
            Workorder wo = uowMes.WorkorderRepo.GetById(selectedWorkorderId);

            if (wo != null)
            {
                ItemOP parentItem = wo.Item;
                Item scannedChildItem = uowMes.ItemRepo.GetByCode(itemCode);

                if (scannedChildItem != null)
                {
                    Bom bomChild = uowMes.BomRepo.GetChildsOfItem(parentItem.Id)
                        .Where(x => x.AncId == scannedChildItem.Id).FirstOrDefault();

                    if (bomChild != null)
                    {
                        //TODO: qty nie moze byc pobierane po prostu z bacode, poniewaz po odstawieniu na bufor nikt nie zmienia etykiety na nowa
                        //trzeba wykorzystac mechanizm lokalizacji
                        //TODO: 20200624 rozwiazac problem wielu production logów pod tym samym numerem seryjnym

                        decimal qtyOnContainer = 0;
                        decimal qtyAvailableOnBuffer = 0;
                        decimal qtyToBeLoadedOnBuffer = 0;
                        decimal qtyLoadedOnBuffer = 0;

                        List<ProductionLog> prodLogs = uowMes.ProductionLogRepo.GetBySerialNumberAndItemCode(serialNumber, itemCode).ToList();

                        //if (prodLogs == null || prodLogs.Count <= 0)
                        //{
                        //    ProductionLog prodLog = new ProductionLog();
                        //    prodLog.WorkplaceId
                        //}

                        if (prodLogs != null && prodLogs.Count > 0)
                        {
                            //qtyOnContainer = Math.Min(qty, pl.Sum(x => x.DeclaredQty));
                            qtyAvailableOnBuffer = uowMes.WorkplaceBufferRepo.GetByWorkorderId(selectedWorkorderId).Where(x => x.Child.Code == itemCode).DefaultIfEmpty().Sum(x => x != null ? x.QtyAvailable : 0);
                            //qtyToBeLoadedOnBuffer = Math.Max(0, Math.Min(wo.Qty_Remain * bomChild.PCS - qtyAvailableOnBuffer, qtyOnContainer));

                            foreach(ProductionLog pl in prodLogs)
                            {
                                qtyOnContainer = pl.QtyAvailable;
                                qtyToBeLoadedOnBuffer = Math.Max(0, Math.Min(wo.Qty_Remain * bomChild.PCS - qtyAvailableOnBuffer, qtyOnContainer));

                                if (qtyToBeLoadedOnBuffer > 0)
                                {
                                    WorkplaceBuffer wb = new WorkplaceBuffer();
                                    wb.ParentWorkorderId = selectedWorkorderId;
                                    wb.WorkplaceId = workplaceId;
                                    wb.Parent = parentItem;
                                    wb.ChildId = scannedChildItem.Id;
                                    wb.ProductionLog = pl;
                                    wb.ProductionLogId = wb.ProductionLog == null ? null : (int?)wb.ProductionLog.Id;
                                    wb.ProcessId = (int)parentItem.ItemGroup.ProcessId;
                                    wb.Barcode = barcode;
                                    wb.SerialNumber = serialNumber; //barcodeParser.GetSerialNumber(barcode);
                                    wb.QtyAvailable = qtyToBeLoadedOnBuffer;
                                    wb.QtyInBom = bomChild.PCS;
                                    wb.Code = itemCode;
                                    wb.Name = ""; //barcodeParser.GetName(barcode);
                                    wb.TimeLoaded = DateTime.Now;
                                    uowMes.WorkplaceBufferRepo.AddOrUpdate(wb);

                                    qtyAvailableOnBuffer += qtyToBeLoadedOnBuffer;
                                    qtyLoadedOnBuffer += qtyToBeLoadedOnBuffer;
                                }
                            }
                        }
                        else
                        {
                            qtyOnContainer = qty;
                            qtyToBeLoadedOnBuffer = Math.Max(0, Math.Min(wo.Qty_Remain * bomChild.PCS - qtyAvailableOnBuffer, qtyOnContainer));

                            WorkplaceBuffer wb = new WorkplaceBuffer();
                            wb.ParentWorkorderId = selectedWorkorderId;
                            wb.WorkplaceId = workplaceId;
                            wb.Parent = parentItem;
                            wb.ChildId = scannedChildItem.Id;
                            wb.ProductionLog = null;
                            wb.ProductionLogId = null;
                            wb.ProcessId = (int)parentItem.ItemGroup.ProcessId;
                            wb.Barcode = barcode;
                            wb.SerialNumber = serialNumber; //barcodeParser.GetSerialNumber(barcode);
                            wb.QtyAvailable = qtyToBeLoadedOnBuffer;
                            wb.QtyInBom = bomChild.PCS;
                            wb.Code = itemCode;
                            wb.Name = scannedChildItem.Name; //barcodeParser.GetName(barcode);
                            wb.TimeLoaded = DateTime.Now;
                            uowMes.WorkplaceBufferRepo.AddOrUpdate(wb);

                            qtyLoadedOnBuffer += qtyToBeLoadedOnBuffer;
                        }

                        jsonModel.Message = "Załadowano na bufor " + qtyLoadedOnBuffer + "" +
                            " sztuk. Pozostało " + (qtyOnContainer - qtyLoadedOnBuffer) + " szt w kontenerze";
                        jsonModel.MessageType = JsonMessageType.success;
                        
                    }
                    else
                    {
                        jsonModel.SetMessage("Zeskanowany kod nie pasuje do wybranego workorderu", JsonMessageType.warning);
                    }
                }
                else
                {
                    jsonModel.SetMessage("Nie znaleziono zeskanowanego kodu w bazie", JsonMessageType.warning);
                }
            }
            else
            {
                jsonModel.SetMessage("Nie znaleziono workorderu", JsonMessageType.warning);
            }

            return Json(jsonModel);
        }

        [HttpPost]
        public JsonResult GetScannedItems_fake(int workplaceId)
        {
            List<WorkplaceBufferViewModel> childs = new List<WorkplaceBufferViewModel>();
            WorkplaceBufferViewModel child = new WorkplaceBufferViewModel()
            {
                Id = 1,
                ProcessId = 10,
                QtyAvailable = 20,
                QtyInBom = 14,
                Barcode = "B-code",
                ChildCode = "ANC102934",
                ChildName = "Name",
                ChildItemId = 3,
                ParentItemId = 4,
                TimeLoaded = DateTime.Now.ToShortDateString()
            };
            WorkplaceBufferViewModel child2 = new WorkplaceBufferViewModel()
            {
                Id = 2,
                ProcessId = 11,
                QtyAvailable = 20,
                QtyInBom = 14,
                Barcode = "B-code",
                ChildCode = "ANC102934",
                ChildName = "Name",
                ChildItemId = 3,
                ParentItemId = 4,
                TimeLoaded = DateTime.Now.ToShortDateString()
            };
            childs.Add(child);
            childs.Add(child2);
            return Json(childs);
        }

        [HttpPost]
        public JsonResult WorkplaceBufferGetItems(int workplaceId, int selectedWorkorderId)
        {
            List<WorkplaceBuffer> bufferedItems = null;
            List<WorkplaceBufferViewModel> bufferedItemsVM = null;
            Workorder wo = uowMes.WorkorderRepo.GetById(selectedWorkorderId);

            if (wo == null)
            {
                bufferedItemsVM = uowMes.WorkplaceBufferRepo.GetByWorkplaceId(workplaceId).ToList<WorkplaceBufferViewModel>();
            }
            else
            {
                bufferedItems = uowMes.WorkplaceBufferRepo.GetByWorkorderId(wo.Id).ToList();
                int itemId = Convert.ToInt32(uowMes.WorkorderRepo.GetById(selectedWorkorderId).ItemId);

                bufferedItemsVM = uowMes.BomRepo.GetChildsOfItem(itemId)
                    .OrderBy(x => x.DEF)
                    .Select(x => new WorkplaceBufferViewModel()
                    {
                        Id = x.Id,
                        ParentItemId = x.PncId ?? 0,
                        ChildItemId = x.AncId ?? 0,
                        ChildName = x.Anc.Name,
                        ChildCode = x.Anc.Code,
                        QtyInBom = x.PCS,
                        QtyAvailable = 0,
                        QtyRequested = 0,
                        ProcessId = x.Anc.ProcessId ?? 0
                    })
                    .ToList();


                int i = 0;
                foreach (WorkplaceBufferViewModel child in bufferedItemsVM)
                {
                    child.QtyRequested = wo.Qty_Remain * child.QtyInBom;
                    child.QtyAvailable = bufferedItems.Where(x => x.ChildId == child.ChildItemId).Sum(x => x.QtyAvailable);

                    var timeLoaded = bufferedItems.Where(x => x.ChildId == child.ChildItemId).ToList();

                    if (child.QtyAvailable >= child.QtyRequested)
                    {
                        i++;
                    }

                    if (timeLoaded != null && timeLoaded.Count > 0)
                    {
                        child.TimeLoaded = timeLoaded.Max(x => x.TimeLoaded).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }
            }

            //TODO: sprawdzić statusy w procedurach
            //if (i >= childs.Count && wo.Status < TaskScheduleStatus.inProduction) {
            //    wo.Status = TaskScheduleStatus.ready;
            //}

            return Json(bufferedItemsVM);
        }

        [HttpPost]
        public JsonResult WorkplaceBufferRemoveItem(int workplaceBufferId)
        {
            WorkplaceBuffer deleteItem = uowMes.WorkplaceBufferRepo.GetById(workplaceBufferId);
            uowMes.WorkplaceBufferRepo.Delete(deleteItem);
            return Json("");
        }

        private List<WorkplaceBufferViewModel> GetDataFromBomListToScan(List<WorkplaceBufferViewModel> data)
        {
            List<WorkplaceBufferViewModel> list = new List<WorkplaceBufferViewModel>();

            foreach (var scanItemVM in data)
            {
                WorkplaceBufferViewModel temp = new WorkplaceBufferViewModel();
                temp.Id = scanItemVM.Id;
                temp.ChildItemId = scanItemVM.ChildItemId;
                temp.ChildCode = scanItemVM.ChildCode;
                temp.ChildName = scanItemVM.ChildName;
                list.Add(temp);
            }
            return list;
        }

        private string GetItemCodeFromBarcode(string barcode)
        {
            return barcode;
        }

        private string GetSerialNumberFromBarcode(string barcode)
        {
            return barcode;
        }
    }
}