 using _MPPL_WEB_START.Areas.ONEPROD.Models;
using MDL_iLOGIS.ComponentConfig.Mappers;
using MDL_ONEPROD.ComponentMes.ViewModels;
using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.ONEPROD.Controllers
{
    public partial class MESController : BaseController
    {
        //_WAŻNE_!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //ProductionLog ma się nazywać traceability
        //TRACEABILITY jest częścią MES'a więc zrobiłem partial class żeby traceability było w innym pliku a jednak w tym samym kontrolerze.
        //W związku, że jest to partial class nie potrzebna jest ponowna deklaracja tych samych zmiennych więc wykomentowałem

        //IDbContextOneProdMes db;
        //UnitOfWorkONEPROD uow;
        //UnitOfWorkOneProdOEE uowOEE;
        //UnitOfWorkOneProdMes uowMes;

        //public MesController(IDbContextOneProdMes db, IDbContextOneProdOEE db2)
        //{
        //    Console.WriteLine("MES.TRACEABILITY");
        //}
        // GET: ONEPROD/Traceability

        public ActionResult Index()
        {
            return View();
        }

        //Traceability
        public ActionResult Traceability()
        {
            FilterProductionLogViewModel fovm = new FilterProductionLogViewModel();
            fovm.MachineList = new List<ResourceOP>();
            fovm.MachineList.AddRange(uowMes.ResourceRepo.GetListForDropDown());
            return View(fovm);
        }

        [HttpPost]
        public ActionResult TraceabilityDelete(int id)
        {
            uowMes.ProductionLogRepo.DeleteProductionLog(id);
            return Json(0);
        }
        [HttpPost]
        public JsonResult TraceabilityUpdate(ProductionLog item)
        {
            uowMes.ProductionLogRepo.AddOrUpdate(item);
            return Json(item);
        }
        [HttpPost]
        public JsonResult TraceabilityGetList(FilterProductionLogViewModel filters, int pageIndex, int pageSize)
        {
            //ProductionLogDataPreparer pldp = new ProductionLogDataPreparer();
            //IQueryable<ProductionLog> ProductionLogQuery = uowMes.ProductionLogRepo.GetList().Take(5);
            //List<ProductionLogViewModel> vm = pldp.GetPLViewModel(dbMes.ProductionLogs,filters);
            //List<ProductionLogViewModel> ItemList = uowMes.ProductionLogRepo.GetPLViewModelList(filters).Take(5).ToList();
            //List<ProductionLog> ItemList = uowMes.ProductionLogRepo.GetList(filter).ToList();
            //ItemList.ForEach(x => { x.Workplace = null; x.WorkplaceId = 0; });
            IQueryable<ProductionLog> query = uowMes.ProductionLogRepo.GetList(filters);
            int startIndex = (pageIndex - 1) * pageSize;
            int itemsCount = query.Count();
            List<ProductionLogViewModel> vm = query.Skip(startIndex).Take(pageSize).ToList<ProductionLogViewModel>();

            return Json(new { data = vm, itemsCount });
        }


        //[HttpPost]
        //public JsonResult TraceabilityGetList_Fake(ProductionLog item)
        //{
        //    List<ProductionLog> ItemList = new List<ProductionLog>() {
        //        new ProductionLog(){
        //            Id = 1,
        //            TimeStamp = DateTime.Now,
        //            SerialNo = "BatchSerialNo - 1",
        //            CostCenter = "CC - Koszty na projekty",
        //            DeclaredQty = 30,
        //            InternalWorkOrderNumber = "IWON342345",
        //            ItemCode = "PC3412",
        //            WorkorderTotalQty = 100,
        //            ClientWorkOrderNumber = "WON10002",
        //            UserName = "Piotr Kaśków",
        //            WorkplaceId = 1,
        //            Workplace = new Workplace() {Id = 1,
        //                                         ComputerHostName = "CHN- komp 1",
        //                                         LabelANC = "ANCLabel - 123",
        //                                         LabelName = "LN - Etykieta do rurek",
        //                                         Name = "Magazyn główny",
        //                                         MachineId = 4,
        //                                         PrinterIPv4 = "123.234.123.12",
        //                                         SelectedTaskId = 1,
        //                                         LoggedUserName = "Janusz",
        //                                        },
        //            OEEReportProductionDataId = 1,
        //            OEEReportProductionData = new OEEReportProductionData() {
        //                                         Id = 1,
        //                                         CycleTime = 3,
        //                                         DetailId = 1,
        //                                         //EntryType = MDL_ONEPROD.Model.Scheduling.Interface.EnumEntryType.Production,
        //                                         ReasonType = new ReasonType(){ Name = "Prod.", Id = 2, EntryType = MDL_ONEPROD.Model.Scheduling.Interface.EnumEntryType.Production, GenerateCharts = false },
        //                                         ReasonTypeId = 2,
        //                                         ItemId = 3,
        //                                         ProdQty = 4,
        //                                         ProductionDate = DateTime.Now,
        //                                         TimeStamp = DateTime.Now,
        //                                        },
        //            Deleted = false,
        //            TransferNumber = ""
        //        },
        //        new ProductionLog(){
        //            Id = 2,
        //            TimeStamp = DateTime.Now.AddHours(1),
        //            SerialNo = "BatchSerialNo - 2",
        //            CostCenter = "CC - Koszty na projekty-2",
        //            DeclaredQty = 40,
        //            InternalWorkOrderNumber = "IWON342346",
        //            ItemCode = "PC3413",
        //            WorkorderTotalQty = 150,
        //            ClientWorkOrderNumber = "WON10003",
        //            UserName = "Piotr Kacprzycki",
        //            WorkplaceId = 2,
        //            Workplace = new Workplace() {Id = 2,
        //                                         ComputerHostName = "CHN- komp 2",
        //                                         LabelANC = "ANCLabel - 124",
        //                                         LabelName = "LN - Etykieta do rurek-2",
        //                                         Name = "Magazyn główny - 2",
        //                                         MachineId = 5,
        //                                         PrinterIPv4 = "123.234.123.13",
        //                                         SelectedTaskId = 2,
        //                                         LoggedUserName = "Józek",
        //                                        },
        //            OEEReportProductionDataId = 2,
        //            OEEReportProductionData = new OEEReportProductionData() {
        //                                         Id = 2,
        //                                         CycleTime = 4,
        //                                         DetailId = 2,
        //                                         //EntryType = MDL_ONEPROD.Model.Scheduling.Interface.EnumEntryType.Production,
        //                                         ReasonType = new ReasonType(){ Name = "Prod.", Id = 2, EntryType = MDL_ONEPROD.Model.Scheduling.Interface.EnumEntryType.Production, GenerateCharts = false },
        //                                         ItemId = 4,
        //                                         ProdQty = 5,
        //                                         ProductionDate = DateTime.Now.AddHours(1),
        //                                         TimeStamp = DateTime.Now.AddHours(1),
        //                                        },
        //            Deleted = false,
        //            TransferNumber = ""
        //        },
        //        new ProductionLog(){
        //            Id = 3,
        //            TimeStamp = DateTime.Now.AddHours(2),
        //            SerialNo = "BatchSerialNo - 3",
        //            CostCenter = "CC - Koszty na projekty-3",
        //            DeclaredQty = 50,
        //            InternalWorkOrderNumber = "IWON342347",
        //            ItemCode = "PC3414",
        //            WorkorderTotalQty = 200,
        //            ClientWorkOrderNumber = "WON10004",
        //            UserName = "Piotr Koscielski",
        //            WorkplaceId = 3,
        //            Workplace = new Workplace() {Id = 3,
        //                                         ComputerHostName = "CHN- komp 3",
        //                                         LabelANC = "ANCLabel - 125",
        //                                         LabelName = "LN - Etykieta do rurek-3",
        //                                         Name = "Magazyn główny - 3",
        //                                         MachineId = 6,
        //                                         PrinterIPv4 = "123.234.123.14",
        //                                         SelectedTaskId = 3,
        //                                         LoggedUserName = "Jasiu",
        //                                        },
        //            OEEReportProductionDataId = 3,
        //            OEEReportProductionData = new OEEReportProductionData() {
        //                                         Id = 3,
        //                                         CycleTime = 5,
        //                                         DetailId = 3,
        //                                         //EntryType = MDL_ONEPROD.Model.Scheduling.Interface.EnumEntryType.Production,
        //                                         ReasonType = new ReasonType(){ Name = "Prod.", Id = 2, EntryType = MDL_ONEPROD.Model.Scheduling.Interface.EnumEntryType.Production, GenerateCharts = false },
        //                                         ItemId = 5,
        //                                         ProdQty = 6,
        //                                         ProductionDate = DateTime.Now.AddHours(2),
        //                                         TimeStamp = DateTime.Now.AddHours(2),
        //                                        },
        //            Deleted = false,
        //            TransferNumber = ""
        //        },
        //    };
        //    List<ProductionLog> filterList = GetItemsP(item, ItemList, 0);
        //    return Json(filterList);
        //}

        public List<ProductionLog> GetItemsP(ProductionLog part,List<ProductionLog> prodList, int areaId = 0)
        {
            return prodList.Where(p => (p.Deleted != true) &&
                            (part.SerialNo == null || p.SerialNo.StartsWith(part.SerialNo)) &&
                            (part.ClientWorkOrderNumber == null || p.ClientWorkOrderNumber.StartsWith(part.ClientWorkOrderNumber) ) &&
                            (part.UserName == null || p.UserName.StartsWith(part.UserName)) &&
                            (part.ItemCode == null || p.ItemCode.StartsWith(part.ItemCode)) &&
                            (part.InternalWorkOrderNumber == null || p.InternalWorkOrderNumber.StartsWith(part.InternalWorkOrderNumber)) &&
                            (part.CostCenter == null || p.CostCenter.StartsWith(part.CostCenter)))
                            .OrderBy(p => p.Id)
                            .ToList();
        }


    }
}