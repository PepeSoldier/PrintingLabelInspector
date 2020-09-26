using _MPPL_WEB_START.Areas.ONEPROD.OEE.ViewModels;
using MDL_ONEPROD.ComponentOEE.Models;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Repo;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using XLIB_COMMON.Enums;
using MDL_BASE.ComponentBase.Repos;
using System.Linq;
using MDL_ONEPROD.Model.Scheduling;
using _MPPL_WEB_START.Areas._APPWEB.Models;
using MDL_ONEPROD.Model.Scheduling.Interface;
using _MPPL_WEB_START.Areas._APPWEB.ViewModels;
using MDL_BASE.ComponentBase.Entities;
using Microsoft.AspNet.Identity;
using MDL_BASE.Models.IDENTITY;
using MDL_ONEPROD.ComponentMes.Models;
using XLIB_COMMON.Repo.Base;
using XLIB_COMMON.Model;
using MDL_ONEPROD.ComponentOEE.ViewModels;

namespace _MPPL_WEB_START.Areas.ONEPROD.Controllers
{
    [AuthorizeRoles(DefRoles.ONEPROD_ADMIN, DefRoles.ONEPROD_MES_OPERATOR, DefRoles.ONEPROD_MES_SUPEROPERATOR)]
    public class OEECreateReportController : Controller
    {
        UnitOfWorkOneProdOEE uowOEE;
        RepoLabourBrigade repoLabourBrigade;

        public OEECreateReportController(IDbContextOneProdOEE db)
        {
            uowOEE = new UnitOfWorkOneProdOEE(db);
            repoLabourBrigade = new RepoLabourBrigade(db);
        }

        [HttpGet]
        public ActionResult Index(int id = 0)
        {
            if (id == 0)
            {
                OEEReport oeeReport = uowOEE.OEEReportRepo.GetDraftByUser(User.Identity.GetUserId());
                oeeReport = oeeReport != null? oeeReport : CreateReport();

                return RedirectToAction("Index", new { id = oeeReport.Id });
            }

            OeeViewModel vm = new OeeViewModel();
            vm.OeeReport = uowOEE.OEEReportRepo.GetById(id);

            if(vm.OeeReport == null || vm.OeeReport.Deleted == true)
            {
                return RedirectToAction("NotFound");
            }

            return View(vm);
        }
        public ActionResult Step1(int reportId = 0)
        {
            OeeViewModel vm = new OeeViewModel();
            vm.OeeReport = uowOEE.OEEReportRepo.GetById(reportId);
            vm.Machines = uowOEE.ResourceRepo.GetListForDropDown();

            if (vm.OeeReport != null)
            {
                IQueryable<LabourBrigade> labBrigades = repoLabourBrigade.GetList();
                vm.LabourBrigades = new SelectList(labBrigades, "Id", "Name");
                vm.MachineId = (vm.OeeReport.Machine != null)? vm.OeeReport.Machine.Id : 0;
                vm.ReportId = vm.OeeReport.Id;
            }
            else
            {
                return RedirectToAction("NotFound");
            }
            return View(vm);
        }
        public ActionResult Step2(int reportId, int machineId)
        {
            OeeViewModel vm = new OeeViewModel();
            vm.ReportId = reportId;
            vm.OeeReport = uowOEE.OEEReportRepo.GetById(reportId);
            vm.MachineId = machineId;
            return View(vm);
        }
        public ActionResult Step3(int reportId, int machineId)
        {
            OeeViewModel vm = new OeeViewModel();
            vm.ReportId = reportId;
            vm.MachineId = machineId;
            vm.AreaId = uowOEE.ResourceGroupRepo.GetIdByMachineId(machineId);
            return View(vm);
        }
        public ActionResult Step4(int reportId, int machineId)
        {
            OeeViewModel vm = new OeeViewModel();
            vm.ReportId = reportId;
            vm.MachineId = machineId;
            vm.ReasonTypes = uowOEE.ReasonTypeRepo.GetListStoppages().ToList();
            return View(vm);
        }

        //------------------STEP1-----------------------------------------------
        [HttpPost]
        public JsonResult SaveReport(OEEReport oeeReport)
        {
            int existingReportId = CheckIfReportExists(oeeReport);
            int reportMachineId = -1;

            if(existingReportId <= 0)
            {
                OEEReport report = uowOEE.OEEReportRepo.GetById(oeeReport.Id);

                uowOEE.OEEReportProductionDataRepo.AddAvailableTimeEntryIfNotExists(report, User.Identity.GetUserId());
                UpdateProductionDateIfMachineOrDateOrShiftChanged(oeeReport, report);

                report.LabourBrigadeId = oeeReport.LabourBrigadeId;
                report.MachineId = oeeReport.MachineId;
                report.ReportDate = oeeReport.ReportDate;
                report.Shift = oeeReport.Shift;
                report.IsDraft = report.IsDraft; //było false 20190316
                uowOEE.OEEReportRepo.AddOrUpdate(report);

                reportMachineId = report.MachineId != null ? (int)report.MachineId : 0;
            }

            return Json(new { reportMachineId, existingReportId }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult AddMicrostops(DateTime dateFrom, DateTime dateTo)
        {
            var list = uowOEE.OEEReportRepo.GetList().Where(x => x.ReportDate >= dateFrom && x.ReportDate < dateTo).ToList();

            foreach(OEEReport report in list)
            {
                OeeHelper._VerifyReport(uowOEE, report.Id, User.Identity.GetUserId());
            }

            return Json(list.Count, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteReport(int reportId, DateTime reportDate, int reportShift, int machineId)
        {
            OEEReport report = uowOEE.OEEReportRepo.GetById(reportId);

            if(report.ReportDate == reportDate && (int)report.Shift == reportShift && report.MachineId == machineId)
            {
                report.Deleted = true;
                uowOEE.OEEReportRepo.Update(report);

                List<OEEReportProductionData> reportData = uowOEE.OEEReportProductionDataRepo.GetListByReportId(reportId).ToList();

                foreach(OEEReportProductionData rd in reportData)
                {
                    rd.Deleted = true;
                }
                uowOEE.OEEReportProductionDataRepo.Save();
            }
            
            return Json(1);
        }

        public ActionResult MoveStops()
        {
            ViewBag.Machines = uowOEE.ResourceRepo.GetListForDropDown().ToList();
            ViewBag.Reasons = uowOEE.ReasonRepo.GetList().ToList();
            ViewBag.ReasonTypes = uowOEE.ReasonTypeRepo.GetList().ToList();

            return View();
        }
        [HttpPost]
        public JsonResult MoveStops(DateTime dateFrom, DateTime dateTo, int machineId, int seconds, int sourceReasonId = 235, int destReasonId = 21)
        {
            //http://localhost:49962/ONEPROD/OEECreateReport/MoveMicrostops?dateFrom=2019-01-01&dateTo=2019-02-01&machineId=5&seconds=0&sourceReasonId=235&destReasonId=21
            List<OEEReport> repots = uowOEE.OEEReportRepo.GetList().Where(x => x.ReportDate >= dateFrom && x.ReportDate < dateTo && x.MachineId == machineId).ToList();

            Reason reason = uowOEE.ReasonRepo.GetById(destReasonId);

            foreach (OEEReport report in repots)
            {
                OEEReportProductionDataAbstract sourdeStop = uowOEE.OEEReportProductionDataRepo.GetListByReportId(report.Id).Where(x => x.ReasonId == sourceReasonId && x.UsedTime >= seconds).FirstOrDefault();
                OEEReportProductionDataAbstract destStop = uowOEE.OEEReportProductionDataRepo.GetListByReportId(report.Id).Where(x => x.ReasonId == destReasonId).FirstOrDefault();

                if (sourdeStop != null && sourdeStop.UsedTime >= seconds)
                {
                    if (destStop == null)
                    {
                        CreateReportStoppageViewModel vm = new CreateReportStoppageViewModel();
                        vm.ReportId = report.Id;
                        vm.ReasonId = destReasonId;
                        vm.UsedTime = seconds;
                        vm.ReasonTypeId = reason.ReasonTypeId;
                        OeeHelper.StoppadeUpdateFunction(uowOEE, vm, User.Identity.GetUserId());
                    }
                    else
                    {
                        destStop.UsedTime += seconds;
                        uowOEE.OEEReportProductionDataRepo.AddOrUpdate(destStop);
                    }

                    sourdeStop.UsedTime -= seconds;
                    uowOEE.OEEReportProductionDataRepo.AddOrUpdate(sourdeStop);
                }
            }

            return Json(repots.Count, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult MoveMicrostops(DateTime dateFrom, DateTime dateTo, int machineId, int seconds)
        {
            //http://localhost:49962/ONEPROD/OEECreateReport/MoveMicrostops?dateFrom=2019-01-01&dateTo=2019-02-01&machineId=5&seconds=0
            var list = uowOEE.OEEReportRepo.GetList().Where(x => x.ReportDate >= dateFrom && x.ReportDate < dateTo && x.MachineId == machineId).ToList();

            Reason reason = uowOEE.ReasonRepo.GetById(21);

            foreach (OEEReport report in list)
            {
                OEEReportProductionDataAbstract MikroP = uowOEE.OEEReportProductionDataRepo.GetListByReportId(report.Id).Where(x=>x.ReasonId == 235).FirstOrDefault();//235).FirstOrDefault();//85
                OEEReportProductionDataAbstract Szkol = uowOEE.OEEReportProductionDataRepo.GetListByReportId(report.Id).Where(x => x.ReasonId == 21).FirstOrDefault();//21).FirstOrDefault();
                //OEEReportProductionDataAbstract PPlan = uowOEE.OEEReportProductionDataRepo.GetListByReportId(report.Id).Where(x => x.ReasonId == 20).FirstOrDefault();//20).FirstOrDefault();

                if (MikroP != null && MikroP.UsedTime >= seconds)
                {
                    if(Szkol == null)
                    {
                        CreateReportStoppageViewModel vm = new CreateReportStoppageViewModel();
                        vm.ReportId = report.Id;
                        vm.ReasonId = 21;
                        vm.UsedTime = seconds;
                        //vm.EntryType = EnumEntryType.StopPlanned;
                        vm.ReasonTypeId = reason.ReasonTypeId;
                        OeeHelper.StoppadeUpdateFunction(uowOEE,vm,User.Identity.GetUserId());
                    }
                    else
                    {
                        Szkol.UsedTime += seconds;
                        uowOEE.OEEReportProductionDataRepo.AddOrUpdate(Szkol);
                    }

                    //if (PPlan == null)
                    //{
                    //    CreateReportStoppageViewModel vm = new CreateReportStoppageViewModel();
                    //    vm.ReportId = report.Id;
                    //    vm.ReasonId = 20;
                    //    vm.UsedTimeInMinutes = seconds / 60;
                    //    vm.EntryType = EnumEntryType.StopPlanned;
                    //    StoppadeUpdateFunction(vm);
                    //}
                    //else
                    //{
                    //    PPlan.UsedTime += seconds;
                    //    uowOEE.OEEReportProductionDataRepo.AddOrUpdate(PPlan);
                    //}

                    MikroP.UsedTime -= seconds;
                    uowOEE.OEEReportProductionDataRepo.AddOrUpdate(MikroP);
                }
            }

            return Json(list.Count, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult MoveTrainingTo(DateTime dateFrom, DateTime dateTo, int machineId)
        {
            var list = uowOEE.OEEReportRepo.GetList().Where(x => x.ReportDate >= dateFrom && x.ReportDate < dateTo && x.MachineId == machineId).ToList();
            Reason reason = uowOEE.ReasonRepo.GetById(21);

            foreach (OEEReport report in list)
            {
                OEEReportProductionDataAbstract MikroP = uowOEE.OEEReportProductionDataRepo.GetListByReportId(report.Id).Where(x => x.ReasonId == 235).FirstOrDefault();
                OEEReportProductionDataAbstract Szkol = uowOEE.OEEReportProductionDataRepo.GetListByReportId(report.Id).Where(x => x.ReasonId == 21).FirstOrDefault();

                if (MikroP.UsedTime >= 600)
                {
                    if (Szkol == null)
                    {
                        CreateReportStoppageViewModel vm = new CreateReportStoppageViewModel();
                        vm.ReportId = report.Id;
                        vm.ReasonId = 21;
                        vm.UsedTime = 600;
                        //vm.EntryType = EnumEntryType.StopPlanned;
                        vm.ReasonTypeId = reason.ReasonTypeId;
                        OeeHelper.StoppadeUpdateFunction(uowOEE,vm,User.Identity.GetUserId());
                    }
                    else
                    {
                        Szkol.UsedTime += 600;
                        uowOEE.OEEReportProductionDataRepo.AddOrUpdate(Szkol);
                    }

                    MikroP.UsedTime -= 600;
                    uowOEE.OEEReportProductionDataRepo.AddOrUpdate(MikroP);
                }
            }

            return Json(list.Count, JsonRequestBehavior.AllowGet);
        }

        private int CheckIfReportExists(OEEReport oeeReport)
        {
            OEEReport existingReport = uowOEE.OEEReportRepo.GetByMachineIdDateAndShift(Convert.ToInt32(oeeReport.MachineId), oeeReport.ReportDate, oeeReport.Shift);
            int existingReportId = 0;

            if (existingReport != null && existingReport.Id != oeeReport.Id)
            {
                existingReportId = existingReport.Id;
            }

            return existingReportId;
        }
        private void UpdateProductionDateIfMachineOrDateOrShiftChanged(OEEReport reportForm, OEEReport reportDb)
        {
            if (IsMachineOrDateOrShiftChanged(reportForm, reportDb))
            {
                uowOEE.OEEReportProductionDataRepo.UpdateProductionDateForReportEntries(reportForm.Id, Convert.ToInt32(reportForm.MachineId), reportForm.ReportDate, reportForm.Shift);
            }
        }
        private bool IsMachineOrDateOrShiftChanged(OEEReport reportForm, OEEReport reportDb)
        {
            return  reportForm.MachineId != reportDb.MachineId ||
                    reportForm.ReportDate.Date != reportDb.ReportDate.Date || 
                    reportForm.Shift != reportDb.Shift;
        }
        

        public OEEReport CreateReport()
        {
            OEEReport draft = new OEEReport();
            draft.Shift = Shift.I_zmiana;
            draft.ReportDate = DateTime.Now.Date;
            draft.TimeStamp = DateTime.Now;
            draft.UserId = User.Identity.GetUserId();
            draft.IsDraft = true;
            uowOEE.OEEReportRepo.AddOrUpdate(draft);

            return draft;
        }

        //------------------STEP2-----------------------------------------------
        [HttpPost]
        public JsonResult ReportEmployeesGetList(OEEReportEmployee newObject)
        {
            List<OEEReportEmployee> employees = new List<OEEReportEmployee>();
            if (newObject.ReportId > 0)
            {
                employees = uowOEE.OEEReportEmployeeRepo.GetListByReportId(newObject.ReportId).ToList();
            }
            return Json(employees);
        }
        [HttpPost]
        public JsonResult ReportEmployeesUpdate(OEEReportEmployee newObject)
        {
            uowOEE.OEEReportEmployeeRepo.AddOrUpdate(newObject);
            return Json(newObject, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ReportEmployeesDelete(OEEReportEmployee deleteObject)
        {
            deleteObject.Deleted = true;
            uowOEE.OEEReportEmployeeRepo.AddOrUpdate(deleteObject);
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        //------------------STEP3-----------------------------------------------
        [HttpPost]
        public JsonResult ProductionDataGetList(CreateReportProdDataViewModel filter)
        {
            //zamiast OEEReportProductionData biore ten model z details ponieważ on ma relację na OEEReportProductionData
            //i mogę bez dodatkowego zapytania mieć mieć szczegóły i główne dane
            //Założenie jest takie, że dla każdego OEEReportProductionData tworze OEEReportProductionDataDetails nawet jeżeli ma być pusty więc to działa.
            //List<OEEReportProductionDataDetails> listDetails = new List<OEEReportProductionDataDetails>();
            List<OEEReportProductionData> listTemp = new List<OEEReportProductionData>();

            if (filter.ReportId > 0)
            {
                //listTemp.AddRange(uowOEE.OEEReportProductionDataRepo.GetListByReportId(filter.ReportId, EnumEntryType.Production).ToList());
                //listTemp.AddRange(uowOEE.OEEReportProductionDataRepo.GetListByReportId(filter.ReportId, EnumEntryType.ScrapMaterial).ToList());
                //listTemp.AddRange(uowOEE.OEEReportProductionDataRepo.GetListByReportId(filter.ReportId, EnumEntryType.ScrapProcess).ToList());

                listTemp.AddRange(uowOEE.OEEReportProductionDataRepo.GetListByReportId(filter.ReportId)
                    .Where(x => x.ReasonType.EntryType >= EnumEntryType.Production && x.ReasonType.EntryType < EnumEntryType.StopPlanned)
                    .ToList());
            }

            List<CreateReportProdDataViewModel> list = new List<CreateReportProdDataViewModel>();

            foreach (OEEReportProductionData pd in listTemp)
            {
                list.Add(new CreateReportProdDataViewModel
                {
                    ItemCode = pd.Item.Code,
                    ItemName = (pd.Item.Name == null) ? pd.Item.OriginalName : pd.Item.Name,
                    //EntryType = pd.EntryType,
                    ReasonTypeName = pd.ReasonType.Name,
                    ReasonTypeId = pd.ReasonTypeId,
                    ReasonTypeEntryType = pd.ReasonType.EntryType,
                    ReasonId = pd.ReasonId,
                    UsedTime = pd.UsedTime,
                    CycleTime = pd.CycleTime,
                    ProductionCycleTime = pd.Detail == null? 0 : pd.Detail.ProductionCycleTime, 
                    Id = pd.Id,
                    ProdQty = pd.ProdQty,
                    ReportId = pd.ReportId,
                    CoilId = pd.Detail == null ? 0 : pd.Detail.CoilId,
                    FormWeightProcess = pd.Detail == null ? 0 : pd.Detail.FormWeightProcess,
                    FormWeightScrap = pd.Detail == null ? 0 : pd.Detail.FormWeightScrap,
                    PaperWeight = pd.Detail == null ? 0 : pd.Detail.PaperWeight,
                    TubeWeight = pd.Detail == null ? 0 : pd.Detail.TubeWeight
                });
            }

            return Json(list);
        }
        [HttpPost]
        public JsonResult ProductionDataUpdate(CreateReportProdDataViewModel vm)
        {
            ItemOP item = uowOEE.ItemOPRepo.GetList().FirstOrDefault(x => x.Code == vm.ItemCode);
            OEEReport report = uowOEE.OEEReportRepo.GetById(vm.ReportId);

            if (item != null && report != null && report.MachineId != null)
            {
                int hour = (((int)report.Shift - 1) * 8) + 6;
                DateTime productionDate = report.ReportDate.Date.AddHours(hour);

                if (vm.CycleTime <= 0) { 
                    MCycleTime cycleTime = uowOEE.CycleTimeRepo.GetCycleTime((int)report.MachineId, (int)item.ItemGroupId);
                    vm.CycleTime = (cycleTime != null) ? cycleTime.CycleTime : 0;
                }

                vm.ItemId = item.Id;
                vm.ProductionDate = productionDate;

                OEEReportProductionData pd = uowOEE.OEEReportProductionDataRepo
                    .AddOrUpdateProductionData(vm, User.Identity.GetUserId());

                vm.DetailId = pd.DetailId;
                pd.DetailId = OeeHelper.UpdateProductionDataDetails(uowOEE, vm);

                if (vm.DetailId != pd.DetailId)
                {
                    uowOEE.OEEReportProductionDataRepo.AddOrUpdate(pd);
                }

                vm.Id = pd.Id;
                vm.ItemCode = item.Code;
                vm.ItemName = item.GetName;
                vm.UsedTime = pd.UsedTime;
            }
            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        
        [HttpPost]
        public JsonResult ProductionDataDelete(CreateReportProdDataViewModel vm)
        {
            OEEReportProductionData pd = uowOEE.OEEReportProductionDataRepo.GetById(vm.Id);

            if (pd != null)
            {
                if (pd.Detail != null)
                {
                    OEEReportProductionDataDetails pdpl = uowOEE.OEEReportProductionDataDetailsRepo.GetById(pd.Detail.Id);
                    if (pdpl != null)
                    {
                        pdpl.Deleted = true;
                        uowOEE.OEEReportProductionDataDetailsRepo.AddOrUpdate(pdpl);
                    }
                }

                pd.Deleted = true;
                uowOEE.OEEReportProductionDataRepo.AddOrUpdate(pd);
            }
            
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        //------------------STEP4-----------------------------------------------
        [HttpPost, AllowAnonymous]
        public JsonResult ReasonsTypesGetList(int machineId)
        {
            List<MachineReason> ReasonList = uowOEE.MachineReasonRepo.GetByMachineId(machineId).Distinct().ToList();
            List<ReasonType> list = new List<ReasonType>();

            if (ReasonList.Count > 0)
            {
                list = ReasonList.Select(x => x.Reason.ReasonType).Distinct().ToList();
            }

            list.Add(uowOEE.ReasonTypeRepo.GetById(5));

            return Json(list);
        }
        [HttpPost, AllowAnonymous]
        public JsonResult ReasonsByEntryTypeAndMachineIdGetList(int? reasonTypeId, int machineId = 0, int[] machineIds = null)
        {
            List<MachineReason> ReasonList = uowOEE.MachineReasonRepo
                .GetListByEntryTypeAndMachineId(reasonTypeId, machineId, machineIds).Distinct().ToList();

            //var unDescribed = uowOEE.ReasonTypeRepo.GetById(5);


            //List<GridSelectListDataModel> sli = ReasonList.Select(x => new GridSelectListDataModel { Id = x.Reason.Id, Name = x.Reason.Name }).Distinct().ToList();

            List<GridSelectListDataModel> sli = new List<GridSelectListDataModel>();

            foreach(var r in ReasonList)
            {
                var rsldm = sli.FirstOrDefault(y => y.Id == r.Reason.Id);

                if(rsldm == null)
                {
                    sli.Add(new GridSelectListDataModel { Id = r.Reason.Id, Name = r.Reason.Name });
                }
            }

            //if (unDescribed != null)
            //{
            //    sli.Add(new GridSelectListDataModel { Id = unDescribed.Reason.Id, Name = r.Reason.Name });
            //}

            return Json(sli);
        }
        [HttpPost, AllowAnonymous]
        public JsonResult StoppageGetList(CreateReportStoppageViewModel filter)
        {
            var data = uowOEE.OEEReportProductionDataRepo.GetListByReportId(filter.ReportId, filter.ReasonTypeId).ToList();
            List<CreateReportStoppageViewModel> list = new List<CreateReportStoppageViewModel>();

            foreach (OEEReportProductionData pd in data)
            {
                list.Add(new CreateReportStoppageViewModel
                {
                    Id = pd.Id,
                    //EntryType = pd.EntryType,
                    ReasonTypeName = pd.ReasonType.Name,
                    ReasonTypeId = pd.ReasonTypeId,
                    ReasonTypeEntryType = pd.ReasonType.EntryType,
                    ReportId = pd.ReportId,
                    Hour = pd.ProductionDate.ToString("HH:mm"),
                    ReasonId = pd.ReasonId != null? (int?)pd.ReasonId.Value : null,
                    Comment = pd.Detail != null? pd.Detail.Comment : "",
                    UsedTime = pd.UsedTime
                });
            }

            return Json(list);
        }
        [HttpPost]
        public JsonResult StoppageUpdate(CreateReportStoppageViewModel vm)
        {
            OeeHelper.StoppadeUpdateFunction(uowOEE, vm, User.Identity.GetUserId());
            return Json(vm);
        }
       
        [HttpPost]
        public JsonResult StoppageDelete(CreateReportStoppageViewModel vm)
        {
            OEEReportProductionData pd = uowOEE.OEEReportProductionDataRepo.GetById(vm.Id);

            if (pd != null)
            {
                if (pd.Detail != null)
                {
                    OEEReportProductionDataDetails pdpl = uowOEE.OEEReportProductionDataDetailsRepo.GetById(pd.Detail.Id);
                    if (pdpl != null)
                    {
                        pdpl.Deleted = true;
                        uowOEE.OEEReportProductionDataDetailsRepo.AddOrUpdate(pdpl);
                    }
                }

                pd.Deleted = true;
                uowOEE.OEEReportProductionDataRepo.AddOrUpdate(pd);
            }

            return Json(0, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, AuthorizeRoles(DefRoles.ADMIN, DefRoles.ONEPROD_MES_OPERATOR)]
        public JsonResult VerifyReport(int reportId)
        {
            decimal microstopsAuto = OeeHelper._VerifyReport(uowOEE, reportId, User.Identity.GetUserId());

            return Json(new { MicroStopsAuto = microstopsAuto });
        }

        

    }
}