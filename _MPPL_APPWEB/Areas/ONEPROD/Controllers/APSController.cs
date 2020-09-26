using _MPPL_WEB_START.Areas._APPWEB.Controllers;
using _MPPL_WEB_START.Areas.ONEPROD.APS.ViewModels;
using _MPPL_WEB_START.Areas.ONEPROD.ViewModels.APS;
using _MPPL_WEB_START.Migrations;
using Autofac;
using Autofac.Integration.Mvc;
using MDL_BASE.Models.IDENTITY;
using MDL_ONEPROD;
using MDL_ONEPROD.Common;
using MDL_ONEPROD.ComponentWMS._Interfaces;
using MDL_ONEPROD.ComponentWMS.UnitOfWorks;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Manager;
using MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.Repo;
using MDLX_MASTERDATA.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace _MPPL_WEB_START.Areas.ONEPROD.Controllers
{
    public class APSController : BaseController
    {
        IDbContextOneprodAPS dbAPS;
        IDbContextOneprodWMS dbWMS;

        UnitOfWorkOneprodAPS uowAPS;
        //UnitOfWorkOneprodWMS uowWMS;
        //int zoom = 30;

        public APSController(IDbContextOneprodAPS db, IDbContextOneprodWMS dbWMS)
        {
            this.dbAPS = db;
            this.dbWMS = dbWMS;
            uowAPS = new UnitOfWorkOneprodAPS(db);
            ViewBag.Skin = "nasaSkin";
        }
        public APSController(IDbContextOneprodAPS db)
        {
            this.dbAPS = db;
            uowAPS = new UnitOfWorkOneprodAPS(db);
            ViewBag.Skin = "nasaSkin";
        }

        //--------------------------------------CALCULATION
        [Authorize(Roles = DefRoles.ONEPROD_VIEWER)]
        public ActionResult Calculation()
        {
            ParamManager pm = new ParamManager(dbAPS);
            CalculationViewModel vm = new CalculationViewModel();

            vm.Areas = uowAPS.ResourceGroupRepo.GetList().ToList();
            vm.DateStart = pm.ProdStartDate;
            vm.DateEnd = pm.ProdEndDate;
            vm.Guid = Guid.NewGuid().ToString();

            return View(vm);
        }
        [HttpPost, Authorize(Roles = DefRoles.ONEPROD_VIEWER)]
        public JsonResult Calculation(CalculationViewModel vm)
        {
            NotificationManager.Instance.AddNotificationLog("Rozpoczęcie kalkulacji...", receiver: vm.Guid);

            ParamManager pm = new ParamManager(dbAPS);
            pm.ProdStartDate = vm.DateStart;
            pm.ProdEndDate = vm.DateEnd;

            List<ResourceOP> areas = uowAPS.ResourceGroupRepo.GetList().OrderByDescending(a => a.StageNo).ToList();
            areas.ForEach(x => x.Consider = (vm.ConsideredAreas.IndexOf(x.Id) > -1));

            //GrandHome
            //areas.ForEach(x => x.Forward = (x.Id == 11 || x.Id == 12 || x.Id == 13 || x.Id == 14 || x.Id == 15 ));
            areas.ForEach(x => x.Forward = (x.Id == 14));
            vm.Areas = uowAPS.ResourceGroupRepo.GetList().ToList();

            CalculationManager calcManager = new CalculationManager(dbAPS, dbWMS, areas);
            calcManager.CalculationDateStart = vm.DateStart;
            calcManager.CalculationDateEnd = vm.DateEnd;
            calcManager.ConsiderCalendar = vm.ConsiderCalendar;
            calcManager.BatchTasks = vm.BatchTasks;
            calcManager.Guid = vm.Guid;
            //calcManager.Start();

            calcManager.LoadLastCalculations();

            foreach (AtcsAlgorithm alg in calcManager.Calculations)
            {
                if (alg.ResourceGroup.Consider)
                {
                    //var db3 = (IDbContextONEPROD)DependencyResolver.Current.GetService(typeof(IDbContextONEPROD));
                    //IDbContextONEPROD db3 = new DbContextAPP_Electrolux();
                    var _db = AutofacDependencyResolver.Current.ApplicationContainer.Resolve<IDbContextOneprodAPS>();
                    alg.CalculationRuleStart(_db);
                }
            }

            int line = NotificationManager.Instance.AddNotificationBlock("Algorytm...", receiver: vm.Guid, status: "");
            NotificationManager.Instance.AddNotificationBlock("Zakończono! ", receiver: vm.Guid, id: line, status: "100%");
            return Json("Calculation Finished");
        }
        [HttpPost, Authorize(Roles = DefRoles.ONEPROD_VIEWER)]
        public JsonResult ClearArea(int id)
        {
            ResourceOP area = uowAPS.ResourceGroupRepo.GetById(id);
            if (area != null)
            {
                uowAPS.WorkorderRepo.ClearTasks(area);
            }
            return Json("");
        }
        
        //--------------------------------------GANTTCHART
        public ActionResult GanttChart()
        {
            ParamManager pm = new ParamManager(dbAPS);
            GantChartViewModel vm = new GantChartViewModel();

            vm.SelectedDateFrom = pm.ProdStartDate.AddDays(-1);
            vm.SelectedDateTo = pm.ProdEndDate;
            vm.Zoom = pm.GanttChartZoom;

            return View("GanttChart", vm);
        }
        //[HttpPost]
        //public ActionResult GanttChart(GantChartViewModel vm)
        //{
        //    var req = Request;
        //    //int zoom = vm.Zoom;
        //    new ParamManager(dbAPS).GanttChartZoom = vm.Zoom;
        //    vm.CurrentTimeRedLinePosition = 0; //(int)((double)(DateTime.Now - vm.SelectedDateFrom).TotalSeconds * GetSecToPixelRatio(zoom));
        //    //vm.SecToPixelRatio = GetSecToPixelRatio(zoom);

        //    //var x1 = uowAPS.ItemRepo.GetList().Where(x=>x.ItemGroupOP != null).Take(10).ToList();
        //    //var x2 = uowAPS.ResourceRepo.GetResources().ToList();

        //    vm.Resources = uowAPS.ResourceRepo.GetResources()
        //        .Select(x => new ResourceViewModel()
        //        {
        //            Id = x.Id,
        //            Name = x.Name,
        //            Color = x.Color,
        //            ResourceGroupId = x.ResourceGroupId?? 0,
        //            ResourceGroupStageNo = x.ResourceGroupOP != null? x.ResourceGroupOP.StageNo: 0,
        //            ResourceGroupSafetyTime = x.ResourceGroupOP != null ? x.ResourceGroupOP.SafetyTime : 0,
        //            ShowBatches = x.ResourceGroupOP != null ? x.ResourceGroupOP.ShowBatches : false,
        //            ToolRequired = x.ToolRequired,
        //            Type = x.Type
        //        })
        //        .OrderBy(m => m.ResourceGroupStageNo).ThenBy(m => m.Name)
        //        .ToList();

        //    foreach (ResourceViewModel resource in vm.Resources)
        //    {
        //        resource.Workorders = new List<WorkorderViewModel>();
        //        resource.Batches = new List<WorkorderBatchViewModel>();

        //        if (resource.ShowBatches)
        //        {
        //            List<int> batchNumbers = uowAPS.WorkorderRepo.GetBatchNumbers(resource.Id, vm.SelectedDateFrom, vm.SelectedDateTo);
        //            IQueryable<Workorder> query = uowAPS.WorkorderRepo.GetWorkordersByBatchNumbers((int)resource.ResourceGroupId, batchNumbers);
        //            resource.Batches = CastWorkordersToBatchViewModel(query, batchNumbers);
        //            //CalcBatchParams(vm.SelectedDateFrom, resource.Batches, resource.ResourceGroupSafetyTime, zoom);
        //        }
        //        else
        //        {
        //            IQueryable<Workorder> query = uowAPS.WorkorderRepo.GetWorkordersOfResource(resource.Id, vm.SelectedDateFrom, vm.SelectedDateTo);
        //            resource.Workorders = CastWorkordersToViewModel(query);
        //            //CalcTasksParams(vm.SelectedDateFrom, resource.Workorders, resource.ResourceGroupSafetyTime, zoom);
        //        }
        //    }

        //    List<ClientOrderViewModel> clientOrders = uowAPS.ClientOrderRepo.GetListByDates(vm.SelectedDateFrom, vm.SelectedDateTo)
        //        .Select(x => new ClientOrderViewModel() {
        //            Id = x.Id,
        //            ClientId = x.ClientId,
        //            ClientName = x.Client != null ? x.Client.Name : "",
        //            ItemCode = x.ItemCode,
        //            OrderNo = x.OrderNo,
        //            ResourceName = x.Resource,
        //            EndDate = x.EndDate,
        //            StartDate = x.StartDate,
        //            Qty_Produced = x.Qty_Produced,
        //            Qty_Total = x.Qty_Total
        //        }).ToList();
        //    vm.ClientOrderResource = clientOrders
        //            .Select(y=>y.ResourceName).Distinct()
        //            .Select(x => new ClientOrderResourceViewModel() { Id = 0, Name = x })
        //            .ToList();

        //    foreach (ClientOrderResourceViewModel resource in vm.ClientOrderResource)
        //    {
        //        resource.ClientOrders = clientOrders.Where(x => x.ResourceName == resource.Name).ToList();

        //        foreach (ClientOrderViewModel o1 in resource.ClientOrders)
        //        {
        //            o1.Width = (int)((o1.EndDate - o1.StartDate).TotalSeconds / 30);
        //            o1.MarginLeft = (int)((o1.StartDate - vm.SelectedDateFrom).TotalMinutes * 2);
        //            o1.Width = (o1.Width > 500) ? o1.Qty_Total * 38 / 30 : o1.Width;
        //        }
        //    }

        //    return View("GanttChart", vm);
        //}
        [HttpPost]
        public JsonResult GanttChartJS(int zoom, List<int> resourceGroupsIds = null)
        {
            //var req = Request;
            //int zoom = vm.Zoom;
            GantChart2ViewModel vm = new GantChart2ViewModel();
            new ParamManager(dbAPS).GanttChartZoom = zoom;
            //vm.CurrentTimeRedLinePosition = (int)((double)(DateTime.Now - vm.SelectedDateFrom).TotalSeconds * GetSecToPixelRatio(zoom));
            //vm.SecToPixelRatio = GetSecToPixelRatio(zoom);

            bool isListEmpty = resourceGroupsIds == null || resourceGroupsIds.Count == 0 || (resourceGroupsIds.Count == 1 && resourceGroupsIds[0] == 0);

            vm.ResourceGroups = uowAPS.ResourceRepo.GetGroups()
                .Where(x => isListEmpty || resourceGroupsIds.Contains(x.Id))
                .Select(x => new ResourceGroupViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    StageNo = x.StageNo
                })
                .OrderBy(x => x.StageNo)
                .ToList();

            foreach(var rgvm in vm.ResourceGroups)
            {
                rgvm.Resources = uowAPS.ResourceRepo.GetResources()
                    .Where(x => x.ResourceGroupId == rgvm.Id)
                    .Select(x => new ResourceViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Color = x.Color,
                        ResourceGroupId = x.ResourceGroupId ?? 0,
                        ResourceGroupStageNo = x.ResourceGroupOP != null ? x.ResourceGroupOP.StageNo : 0,
                        ResourceGroupSafetyTime = x.ResourceGroupOP != null ? x.ResourceGroupOP.SafetyTime : 0,
                        ShowBatches = x.ResourceGroupOP != null ? x.ResourceGroupOP.ShowBatches : false,
                        ToolRequired = x.ToolRequired,
                        Type = x.Type
                    })
                    .OrderBy(m => m.ResourceGroupStageNo).ThenBy(m => m.Name)
                    .ToList();
            }

            return Json(vm);
        }
        [HttpPost]
        public JsonResult GanttChartResource(int resourceId, DateTime selectedDateFrom, DateTime selectedDateTo, int zoom)
        {
            ResourceViewModel resourceVM = uowAPS.ResourceRepo.GetList()
                .Where(x => x.Id == resourceId).Take(1)
                .Select(x => new ResourceViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Color = x.Color,
                    ResourceGroupId = x.ResourceGroupId ?? 0,
                    ResourceGroupStageNo = x.ResourceGroupOP != null ? x.ResourceGroupOP.StageNo : 0,
                    ResourceGroupSafetyTime = x.ResourceGroupOP != null ? x.ResourceGroupOP.SafetyTime : 0,
                    ShowBatches = x.ResourceGroupOP != null ? x.ResourceGroupOP.ShowBatches : false,
                    ToolRequired = x.ToolRequired,
                    Type = x.Type
                }).FirstOrDefault();

            resourceVM.Workorders = new List<WorkorderViewModel>();
            resourceVM.Batches = new List<WorkorderBatchViewModel>();

            //if (resourceVM.Type == ResourceTypeEnum.VirtualResource) 
            if (resourceVM.ResourceGroupId == 99946) 
            {
                List<WorkorderViewModel> clientOrders = uowAPS.ClientOrderRepo
                .GetListByDates(selectedDateFrom, selectedDateTo)
                .Where(x => x.Resource == resourceVM.Name)
                .Select(x => new WorkorderViewModel() {
                    Id = x.Id,
                    Number = x.OrderNo,
                    BatchNumber = 0,
                    ClientOrderId = x.Id,
                    ClientOrderNo = x.OrderNo,
                    StartTime = x.StartDate,
                    EndTime = x.EndDate,
                    ItemCode = x.ItemCode,
                    ItemName = x.ItemName,
                    Qty_Total = x.Qty_Total,
                    Qty_Produced = x.Qty_Produced,
                    Qty_Used = 0,
                    ResourceId = resourceId,
                    ResourceGroupShowBatches = false,
                    BackgroundColor = "lightblue",
                    Param1 = -2
                    //ClientName = x.Client != null ? x.Client.Name : "",
                }).ToList();

                foreach(var c in clientOrders)
                {
                    c.ProcessingTime = (int)(c.EndTime - c.StartTime).TotalSeconds;
                }

                resourceVM.Workorders = clientOrders;
            }
            else {
                if (resourceVM.ShowBatches)
                {
                    List<int> batchNumbers = uowAPS.WorkorderRepo.GetBatchNumbers(resourceVM.Id, selectedDateFrom, selectedDateTo).ToList();
                    IQueryable<Workorder> query = uowAPS.WorkorderRepo.GetWorkordersByBatchNumbers((int)resourceVM.ResourceGroupId, batchNumbers);
                    resourceVM.Batches = CastWorkordersToBatchViewModel(query, batchNumbers);
                    //CalcBatchParams(selectedDateFrom, resourceVM.Batches, resourceVM.ResourceGroupSafetyTime, zoom);
                }
                else
                {
                    IQueryable<Workorder> query = uowAPS.WorkorderRepo.GetWorkordersOfResource(resourceVM.Id, selectedDateFrom, selectedDateTo);
                    resourceVM.Workorders = CastWorkordersToViewModel(query);
                    //CalcTasksParams(selectedDateFrom, resourceVM.Workorders, resourceVM.ResourceGroupSafetyTime, zoom);
                }
            }

            return Json(resourceVM);
        }
        [HttpPost]
        public JsonResult GanttChartClientOrderResource(string resourceName, DateTime selectedDateFrom, DateTime selectedDateTo)
        {
            ClientOrderResourceViewModel vm = new ClientOrderResourceViewModel();

            List<ClientOrderViewModel> clientOrders = uowAPS.ClientOrderRepo.GetListByDates(selectedDateFrom, selectedDateTo)
                .Where(x => x.Resource == resourceName)
                .Select(x => new ClientOrderViewModel()
                {
                    Id = x.Id,
                    ClientId = x.ClientId,
                    ClientName = x.Client != null ? x.Client.Name : "",
                    ItemCode = x.ItemCode,
                    OrderNo = x.OrderNo,
                    ResourceName = x.Resource,
                    EndDate = x.EndDate,
                    StartDate = x.StartDate,
                    Qty_Produced = x.Qty_Produced,
                    Qty_Total = x.Qty_Total
                }).ToList();

            vm = clientOrders
                    .Select(y => y.ResourceName).Distinct()
                    .Select(x => new ClientOrderResourceViewModel() { Id = 0, Name = x })
                    .FirstOrDefault();

            vm.ClientOrders = clientOrders;
            
            //foreach (ClientOrderViewModel o1 in vm.ClientOrders)
            //{
            //    o1.Width = (int)((o1.EndDate - o1.StartDate).TotalSeconds / 30);
            //    o1.MarginLeft = (int)((o1.StartDate - selectedDateFrom).TotalMinutes * 2);
            //    o1.Width = (o1.Width > 500) ? o1.Qty_Total * 38 / 30 : o1.Width;
            //}

            return Json(vm);
        }

        private List<WorkorderBatchViewModel> CastWorkordersToBatchViewModel(IQueryable<Workorder> query, List<int> batchNumbers)
        {
            
            List<WorkorderViewModel> woVM = CastWorkordersToViewModel(query);

            List<WorkorderBatchViewModel> WoBatchList = new List<WorkorderBatchViewModel>();

            foreach(int batchNumber in batchNumbers)
            {
                WorkorderBatchViewModel batch = new WorkorderBatchViewModel()
                {
                    BatchNumber = batchNumber
                };

                batch.Workorders = woVM.Where(x => x.BatchNumber == batchNumber).ToList();

                WorkorderViewModel tmpTask = batch.Workorders.FirstOrDefault(x => x.ItemId != null);

                batch.Qty = batch.Workorders.Sum(x => x.Qty_Total);
                batch.StartTime = batch.Workorders.Min(t => t.StartTime);
                batch.EndTime = batch.Workorders.Max(t => t.EndTime);
                batch.SetupTime = 0; // batch.Workorders.Where(x => x.Item == null).Sum(x => x.ProcessingTime);
                batch.ProcessingTime = (int)(batch.EndTime - batch.StartTime).TotalSeconds;
                batch.Color = (tmpTask != null && tmpTask.ItemColor1 != null) ? tmpTask.ItemColor1 : "gray";
                batch.ToolId = tmpTask != null ? tmpTask.ToolId : null;
                batch.ItemId = tmpTask != null ? tmpTask.ItemId : null;
                batch.ResourceId = tmpTask != null ? (int)tmpTask.ResourceId : 0;
                batch.ItemGroupName = tmpTask.ItemGroupName;
                batch.ResourceGroupId = tmpTask.ResourceGroupId;

                WoBatchList.Add(batch);
            }


            return WoBatchList;
        }
        private List<WorkorderViewModel> CastWorkordersToViewModel(IQueryable<Workorder> woQuery)
        {
            
            IQueryable<WorkorderViewModel> query = woQuery.Select(x =>
                new WorkorderViewModel
                {
                    Id = x.Id,
                    BatchNumber = x.BatchNumber,
                    ClientOrderId = x.ClientOrder != null? x.ClientOrder.Id : 0,
                    ClientOrderNo = x.ClientOrder != null? x.ClientOrder.OrderNo : x.UniqueNumber,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    ItemCode = x.Item.Code,
                    ItemColor1 = x.Item.Color1,
                    ItemColor2 = x.Item.Color2,
                    ItemGroupId = (x.Item != null? x.Item.ItemGroupId : null),
                    ItemGroupName = x.Item.ItemGroupOP.Name,
                    ItemGroupColor = (x.Item != null && x.Item.ItemGroupOP != null ? x.Item.ItemGroupOP.Color : "gray"),
                    ItemName = x.Item.Name != null ? x.Item.Name : x.Item.OriginalName,
                    Param1 = x.Param1,
                    Qty_Total = x.Qty_Total,
                    Qty_Produced = x.Qty_Produced,
                    Qty_Used = x.Qty_Used,
                    //Qty_Remain = x.Qty_Remain,
                    LV = x.LV,
                    ProcessingTime = x.ProcessingTime,
                    ResourceId = x.ResourceId,
                    ResourceGroupId = (x.Resource != null? x.Resource.ResourceGroupId : null),
                    ResourceGroupShowBatches = (x.Resource != null && x.Resource.ResourceGroupOP != null ? x.Resource.ResourceGroupOP.ShowBatches : false),
                    ToolName = x.Tool.Name,
                    ReleaseDate = x.ReleaseDate,
                    DueDate = x.DueDate,
                    ToolId = x.ToolId,
                    ItemId = x.ItemId,
                    Status = x.Status
                }
            );

            List<WorkorderViewModel> woList = query.ToList();
            return woList;
        }

        public PartialViewResult GanttChartItemWorkorder(Workorder vm)
        {
            if (vm.Item == null)
                return default(PartialViewResult);

            return PartialView(vm);
        }
        //public PartialViewResult GantChartWorkorder_JS(int id, DateTime dateFrom)
        //{
        //    Workorder t = uowAPS.WorkorderRepo.GetById(id);
        //    if (t.Item == null)
        //        return default(PartialViewResult);

        //    CalcTaskParams(dateFrom, t, 120);

        //    return PartialView("GantChartWorkorder", t);
        //}
        public PartialViewResult GanttChartItemChangeOver(Workorder vm)
        {
            return PartialView(vm);
        }
        public PartialViewResult GanttChartItemClientOrder(ClientOrder vm)
        {
            return PartialView(vm);
        }
        public PartialViewResult GanttChartTimer(GantChartViewModel vm)
        {
            return PartialView(vm);
        }

        //public JsonResult GetTasks(DateTime dateFrom, DateTime dateTo, int machineId)
        //{
        //    List<WorkorderViewModel> workorders;
        //    var query = uowAPS.WorkorderRepo.GetWorkordersOfResource(machineId, dateFrom, dateTo);
        //    workorders = CastWorkordersToViewModel(query);
        //    CalcTasksParams(dateFrom, workorders, 120, 2);
        //    return Json(workorders, JsonRequestBehavior.AllowGet);
        //}
        [HttpPost, Authorize(Roles = DefRoles.ONEPROD_VIEWER)]
        public JsonResult FindBestPlace(int taskId, DateTime startDate)
        {
            AlgSimulatedAnnealing simAnn = new AlgSimulatedAnnealing(dbAPS);
            TaskGap tg = simAnn.AnalizeTask(taskId);

            if (tg != null)
            {
                tg.MarginLeft = (int)(tg.TaskBestEndDate.AddSeconds(-tg.TaskProcessingTime) - startDate).TotalMinutes * 2;
            }

            return Json(tg);
        }

        //private void CalcTasksParams(DateTime SelectedDateFrom, List<WorkorderViewModel> tasks, int areaSafetyTime, int zoom)
        //{
        //    foreach (WorkorderViewModel wo in tasks)
        //    {
        //        CalcTaskParams(SelectedDateFrom, wo, areaSafetyTime, zoom);
        //    }
        //}
        //private void CalcTaskParams(DateTime SelectedDateFrom, WorkorderViewModel wo, int areaSafetyTime, int zoom)
        //{
        //    if (wo.StartTime >= SelectedDateFrom)
        //    {
        //        wo.Width = (double)((double)wo.ProcessingTime * GetSecToPixelRatio(zoom));
        //        wo.MarginLeft = (double)((double)((wo.StartTime - SelectedDateFrom).TotalSeconds) * GetSecToPixelRatio(zoom));
        //        wo.MarginRight = 0; //ustawić dla production order....               

        //        wo.BackgroundColor = (wo.ItemId == null) ? "#d9d9d9" : wo.ItemGroupColor;

        //        wo.SpecialCssClass =
        //            (wo.ItemId == null) ? "taskSetup" :
        //            (wo.EndTime > wo.DueDate) ? "taskDangerous" :
        //            (wo.EndTime > wo.DueDate.AddMinutes(-areaSafetyTime * 0.5)) ? "taskWarning" : string.Empty;

        //        //wo.Status = uowAPS.WorkorderRepo.StatusVerification(wo);

        //        if (wo.Status == TaskScheduleStatus.used || wo.Qty_Used == wo.Qty_Total)
        //        {
        //            wo.BackgroundColor = "#377316";
        //            wo.FontColor = "white";
        //        }
        //        else if (wo.Status == TaskScheduleStatus.produced || wo.Qty_Remain == 0)
        //        {
        //            wo.BackgroundColor = "#35ad32";
        //            wo.FontColor = "rgb(0,109,69)";
        //        }
        //        else
        //        {
        //            wo.BackgroundColor = wo.BackgroundColor;
        //            wo.FontColor = wo.FontColor;
        //        }
        //    }
        //}
        //private void CalcBatchParams(DateTime SelectedDateFrom, List<WorkorderBatchViewModel> batches, int areaSafetyTime, int zoom)
        //{
        //    foreach (WorkorderBatchViewModel batch in batches)
        //    {
        //        if (batch.StartTime > SelectedDateFrom)
        //        {
        //            batch.Width = (int)(batch.ProcessingTime * GetSecToPixelRatio(zoom));
        //            batch.MarginLeft = (int)((batch.StartTime - SelectedDateFrom).TotalSeconds * GetSecToPixelRatio(zoom));
        //            CalcTasksParams(batch.StartTime, batch.Workorders, areaSafetyTime, zoom);
        //        }
        //    }
        //}
        //private double GetSecToPixelRatio(int zoom)
        //{
        //    //1 minuta to 2 pixele
        //    //60 sec to 2 pixele
        //    //return 60 / 2;

        //    return ((double)zoom)/ (double)60;
        //}
        //private double GetPixelToMinutes(int zoom)
        //{
        //    //2 pixele to 1 minuta
        //    //return 1/2;
        //    return (double)1 / (double)zoom;
        //}

        //[HttpPost, Authorize(Roles = DefRoles.TechnologyUser)]
        //public JsonResult UpdateWorkorder(DateTime date, int leftMargin, int workorderId, int resourceId)
        //{
        //    Workorder wo = uowAPS.WorkorderRepo.GetById(workorderId);
        //    ResourceOP resource = uowAPS.ResourceRepo.GetById(resourceId);
        //    int processingTime = uowAPS.CycleTimeRepo.GetRealProcessingTime(resourceId, (int)wo.Item.ItemGroupId, wo.Qty_Total);

        //    int zoom = new ParamManager(dbAPS).GanttChartZoom;
        //    wo.Resource = resource;
        //    wo.ResourceId = resourceId;
        //    wo.ProcessingTime = (processingTime > 0)? processingTime : wo.ProcessingTime;
        //    wo.StartTime = date.AddMinutes(leftMargin * GetPixelToMinutes(zoom));
        //    wo.EndTime = wo.StartTime.AddSeconds(wo.ProcessingTime);
        //    uowAPS.WorkorderRepo.AddOrUpdate(wo);


        //    var query = uowAPS.WorkorderRepo.GetList().Where(x => x.Id == wo.Id);
        //    WorkorderViewModel woVM = CastWorkordersToViewModel(query).FirstOrDefault();
        //    CalcTaskParams(date, woVM, resource.ResourceGroupOP.SafetyTime, zoom);
        //    return Json(woVM);
        //}
        [HttpPost, Authorize(Roles = DefRoles.ONEPROD_VIEWER)]
        public JsonResult UpdateWorkorder(int workorderId, int resourceId, DateTime newStartTime)
        {
            Workorder wo = uowAPS.WorkorderRepo.GetById(workorderId);
            //ResourceOP resource = uowAPS.ResourceRepo.GetById(resourceId);
            int processingTime = uowAPS.CycleTimeRepo.GetRealProcessingTime(resourceId, (int)wo.Item.ItemGroupId, wo.Qty_Total);

            int zoom = new ParamManager(dbAPS).GanttChartZoom;
            //wo.Resource = resource;
            wo.ResourceId = resourceId;
            wo.ProcessingTime = (processingTime > 0) ? processingTime : wo.ProcessingTime;
            wo.StartTime = newStartTime; //date.AddMinutes(leftMargin * GetPixelToMinutes(zoom));
            wo.EndTime = wo.StartTime.AddSeconds(wo.ProcessingTime);
            uowAPS.WorkorderRepo.AddOrUpdate(wo);

            var query = uowAPS.WorkorderRepo.GetList().Where(x => x.Id == wo.Id).Take(1);
            WorkorderViewModel woVM = CastWorkordersToViewModel(query).FirstOrDefault();
            
            return Json(woVM);
        }
        [HttpPost, Authorize(Roles = DefRoles.ONEPROD_VIEWER)]
        public JsonResult UpdateBatch(int batchNumber, int resourceId, DateTime newStartTime)
        {
            //DbContextPreprod db = new DbContextPreprod();
            ResourceOP resource = uowAPS.ResourceRepo.GetById(resourceId);
            List<Workorder> tasks = uowAPS.WorkorderRepo.GetTasksbyAreaIdAndBatchNumber(resource.ResourceGroupOP.Id, batchNumber).ToList();

            int zoom = new ParamManager(dbAPS).GanttChartZoom;

            //DateTime newStartTime = date.AddMinutes(leftMargin * GetPixelToMinutes(zoom));
            int timeShift = (int)(newStartTime - tasks.FirstOrDefault().StartTime).TotalSeconds; 

            foreach (Workorder task in tasks)
            {
                //int processingTime = uow.CycleTimeRepo.GetRealProcessingTime(machineId, (int)task.Part.ItemGroupId, task.Qty_Remain);
                task.Resource = resource;
                task.ResourceId = resourceId;
                //task.ProcessingTime = (processingTime > 0) ? processingTime : task.ProcessingTime;
                task.StartTime = task.StartTime.AddSeconds(timeShift);
                task.EndTime = task.EndTime.AddSeconds(timeShift);
                uowAPS.WorkorderRepo.AddOrUpdate(task);
            }

            WorkorderBatchViewModel batchVM;
            List<int> batchNumbers = new List<int>() { batchNumber };
            IQueryable<Workorder> query = uowAPS.WorkorderRepo.GetWorkordersByBatchNumbers((int)resource.ResourceGroupId, batchNumbers);
            batchVM = CastWorkordersToBatchViewModel(query, batchNumbers).FirstOrDefault();

            //WorkorderBatch batch = uowAPS.WorkorderRepo.GetBatchByNumberAndResourceGroupId(resourceId, batchNumber);
            //IQueryable<Workorder> query = uowAPS.WorkorderRepo.GetWorkordersByBatchNumbers((int)resource.ResourceGroupId, new List<int>() { batchNumber });
            //CastWorkordersToBatchViewModel(query, new List<int>() { batchNumber });
            //CalcBatchParams(date, CastWorkordersToBatchViewModel(query, new List<int>() { batchNumber }), resource.ResourceGroupOP.SafetyTime, zoom);
            return Json(batchVM);
        }
        
        //--------------------------------------CALENDAR
        public ActionResult Calendar()
        {
            CalendarViewModel vm = new CalendarViewModel();

            vm.Machines = uowAPS.ResourceRepo.GetResources().ToList();
            vm.Months = CalendarMonths.Get();
            vm.Month = DateTime.Now.Date.Month;
            vm.Year = DateTime.Now.Date.Year;   

            if (vm.Machines.Count > 0)
            {
                vm.MachineId = vm.Machines.FirstOrDefault().Id;
            }
            vm.Calendar = uowAPS.CalendarRepo.GetListByMachineIdAndMonth(vm.MachineId, vm.Month);

            return View(vm);
        }
        [HttpPost]
        public ActionResult Calendar(CalendarViewModel vm)
        {
            vm.Machines = uowAPS.ResourceRepo.GetResources().ToList();
            vm.Months = CalendarMonths.Get();
            vm.Year = DateTime.Now.Date.Year;
            vm.Calendar = uowAPS.CalendarRepo.GetListByMachineIdAndMonth(vm.MachineId, vm.Month);
            
            return View(vm);
        }
        [HttpPost]
        public ActionResult CalendarHour(CalendarHourViewModel vm)
        {
            vm.Calendar = uowAPS.CalendarRepo.GetListOfHours(vm.SelectedMachineId, vm.SelectedDate.Date);
            return View(vm);
        }
        [HttpPost, Authorize(Roles = DefRoles.ONEPROD_VIEWER)]
        public JsonResult CalendarHourUpdate(CalendarHourViewModel vm)
        {
            int result = uowAPS.CalendarRepo.AddOrDelete(vm.SelectedMachineId, vm.SelectedHour);
            return Json(result);
        }

        //--------------------------------------PLAN
        public ActionResult Plan()
        {
            PlanViewModel vm = new PlanViewModel();
            vm.DateFrom = DateTime.Now.Date.AddHours(-2);
            vm.DateTo = DateTime.Now.Date.AddDays(15);
            vm.Machines = new SelectList(uowAPS.ResourceRepo.GetResources().ToList(),"Id","Name");
            return View(vm);
        }
        [HttpPost]
        public ActionResult Plan(PlanViewModel vm)
        {
            if (vm.SelectedMachineId > 0)
            {
                RepoPreprodConf repoConf = new RepoPreprodConf(dbAPS);
                vm.Machines = new SelectList(uowAPS.ResourceRepo.GetResources().ToList(), "Id", "Name", vm.SelectedMachineId);
                vm.SelectedMachine = uowAPS.ResourceRepo.GetById(vm.SelectedMachineId);

                if (vm.SelectedMachine != null)
                {
                    vm.Tasks = uowAPS.WorkorderRepo.GetWorkorderOfResource_LimitedStatusProd(vm.SelectedMachineId, vm.DateFrom.AddDays(-5), vm.DateTo).ToList();
                }
                return View(vm);
            }

            return RedirectToAction("Plan");
        }
        
        
    }
}