using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Repo;
using System.Collections.Generic;
using System.Web.Mvc;
using MDL_BASE.ComponentBase.Repos;
using System.Linq;
using MDL_ONEPROD.Model.Scheduling;
using _MPPL_WEB_START.Areas._APPWEB.Models;
using _MPPL_WEB_START.Areas._APPWEB.ViewModels;
using System;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.ComponentOEE.Models;
using _MPPL_WEB_START.Areas.ONEPROD.OEE.ViewModels;
using MDL_ONEPROD.ComponentRTV._Interfaces;
using _MPPL_WEB_START.Areas.ONEPROD.ViewModels.RTV;
using MDL_ONEPROD.ComponentRTV.Entities;
using _MPPL_WEB_START.Areas.ONEPROD.Models;
using MDL_ONEPROD.ComponentRTV.Models;
using MDL_ONEPROD.Model.Scheduling.Interface;
using MDL_BASE.Models.IDENTITY;
using System.IO;
using System.Web.Hosting;
using System.Web;
using System.Drawing;

namespace _MPPL_WEB_START.Areas.ONEPROD.Controllers
{
    public class RTVController : Controller
    {
        UnitOfWorkOneProdRTV uowRTV;
        UnitOfWorkOneprod uowONEPROD;
        RepoLabourBrigade repoLabourBrigade;

        public RTVController(IDbContextOneProdRTV db)
        {
            uowRTV = new UnitOfWorkOneProdRTV(db);
            repoLabourBrigade = new RepoLabourBrigade(db);
            uowONEPROD = new UnitOfWorkOneprod(db);
            ViewBag.Skin = "rtvSkin nasaSkin";
        }

        public int CheckLicence()
        {
            DateTime licenceDueDate = AppClient.appClient.ONEPROD_RTV_Licence;
            int remainingLicenceDays = 0;

            if (AppClient.appClient.ONEPROD_RTV_Licence < DateTime.Now)
            {
                throw new Exception("Licencja wygasła");
            }
            else
            {
                remainingLicenceDays = (int)(licenceDueDate - DateTime.Now).TotalDays;
            }

            return remainingLicenceDays;
        }

        public ActionResult Index()
        {
            ViewBag.RemainingLicenceDays = CheckLicence(); 
            ViewBag.IsUserAllowedForDashboard = User.Identity.IsAuthenticated && User.IsInRole(DefRoles.ONEPROD_VIEWER);
            return View();
            
        }

        [Authorize(Roles = DefRoles.ONEPROD_VIEWER)]
        public ActionResult Dashboard()
        {
            List<DashBoardViewModel> AreaMachineList = new List<DashBoardViewModel>();
            List<ResourceOP> AreaList = uowONEPROD.ResourceGroupRepo.GetList().ToList();

            foreach (ResourceOP area in AreaList)
            {
                if((area.Id >= 16 && area.Id <= 18) || area.IsOEE)
                {
                    DashBoardViewModel dashBoardViewModel = new DashBoardViewModel();
                    dashBoardViewModel.Area = area;
                    dashBoardViewModel.MachineList = uowONEPROD.ResourceRepo.GetListByGroup(area.Id).ToList();
                    AreaMachineList.Add(dashBoardViewModel);
                }
            }

            //uowOEE.OEEReportProductionDataRepo.GetDataForPareto();

            return View(AreaMachineList);
        }
        [HttpPost, Authorize(Roles = DefRoles.ONEPROD_VIEWER)]
        public PartialViewResult DashboardMachine(ResourceOP machine)
        {
            return PartialView(machine);
        }

        public ActionResult MachineDetails(int Id)
        {
            RTVViewModel vm = new RTVViewModel();
            ViewBag.RemainingLicenceDays = CheckLicence();
            vm.Machine = uowONEPROD.ResourceRepo.GetById(Id);
            vm.Reasontypes = uowRTV.ReasonTypeRepo.GetList().ToList();
            return View(vm);
        }

        [HttpPost]
        public JsonResult GetChartRTVDataResults(DateTime dateFrom, DateTime dateTo, int machineId)
        {
            List<RTVOEEProductionData> productionData = uowRTV.RTVOEEProductionDataRepo.GetDataByTimeRangeAndMachineId(dateFrom, dateTo, machineId).ToList();
            ResourceOP resource = uowONEPROD.ResourceRepo.GetById(machineId);
            MachineTargets machineTargets = uowONEPROD.ResourceRepo.GetTargetsForMachine(resource);
            ChartRTV crtv = new ChartRTV(dateFrom, dateTo, 1, productionData, resource);
            ChartViewModel vm = crtv.PrepareChartData(uowRTV.RTVOEEProductionDataRepo, "Real Time visualization");

            return Json(vm);
        }

        [HttpPost]
        public JsonResult GetOeeResult(DateTime dateFrom, DateTime dateTo, int machineId)
        {
            List<OEEReportProductionDataAbstract> productionData = uowRTV.RTVOEEProductionDataRepo.GetDataByTimeRangeAndMachineId(dateFrom, dateTo, machineId).ToList<OEEReportProductionDataAbstract>();
            DashBoardMachineViewModel vm = new DashBoardMachineViewModel();

            //Ustawienie daty w zależności od tego czy jest to OEE rt, czy za poprzednią zmianę.
            dateTo = dateTo < DateTime.Now ? dateTo : DateTime.Now;

             RTVOeeCalculationModel oeeCalc = new RTVOeeCalculationModel(productionData, dateFrom, dateTo);
            oeeCalc.CalculateOEE();

            vm.NumberOfRecords = productionData.Count();
            vm.AvailabilityResult = oeeCalc.Availability;
            vm.PerformanceResult = oeeCalc.Performance;
            vm.QualityResult = oeeCalc.Quality;
            vm.OeeResult = oeeCalc.Result;
            vm.Targets = uowONEPROD.ResourceRepo.GetTargetsForMachine(machineId);
                
            return Json(vm,JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRTVData(DateTime dateFrom, DateTime dateTo, int machineId)
        {
            RTVMachineDetailsViewModel vm = new RTVMachineDetailsViewModel();
            RTVOEEProductionDataDetails rtvDetails = uowRTV.RTVOEEProductionDataDetailsRepo.GetProductionData(machineId, dateFrom, dateTo);

            ResourceOP machine = uowONEPROD.ResourceRepo.GetById(machineId);

            //rtvDetails = new RTVOEEProductionDataDetails { ProgramNo = 100 };

            if(rtvDetails == null)
            {
                PrepareDefaultData(vm);
            }
            else
            {
                //var mIds = AppClient.appClient.SettingsONEPROD.ReportOnlineAllowedMachinesIDs;
                //bool isReportOnline = mIds.Contains(0) || mIds.Contains(machineId);
                string programName = rtvDetails.ProgramName;

                if(!(programName != null && programName.Length > 2))
                {
                    programName = uowONEPROD.CycleTimeRepo.GetList().Where(x => x.ProgramNumber == rtvDetails.ProgramNo).DefaultIfEmpty().Select(x => x != null ? x.ItemGroup.Name : "").FirstOrDefault();
                }

                vm.ProducedQty = rtvDetails.RTVOEEProductionData.ProdQtyShift;
                vm.CycleTime = rtvDetails.RTVOEEProductionData.CycleTime;
                vm.PartCode =  rtvDetails.PartName;
                vm.ProgramName = programName;
                vm.ProgramNo = rtvDetails.ProgramNo;

                vm.Pirometers = new List<RTVPirometersViewModel>();

                if (machine.ResourceGroupOP.Id == 17)
                {
                    AddPirometers(vm, rtvDetails);
                }
                else
                {
                    vm.Pirometers.Add(new RTVPirometersViewModel{ PirometerTemp = 0,PirometerTempMax = 0,PirometerTempMin = 0,});
                }
            }

            return Json(vm,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetProductionDataParameters(int machineId)
        {
            List<RTVOEEProductionDataParameterModel> list = uowRTV.RTVOEEProductionDataParameterRepo.GetActualValues(machineId);
            return Json(list);
        }

        private void AddPirometers(RTVMachineDetailsViewModel vm, RTVOEEProductionDataDetails rtvDetails)
        {
            vm.Pirometers.Add(new RTVPirometersViewModel
            {
                PirometerTemp = rtvDetails.PirometerTemp1,
                PirometerTempMax = rtvDetails.PirometerMax1,
                PirometerTempMin = rtvDetails.PirometerMin1,
            });
            vm.Pirometers.Add(new RTVPirometersViewModel
            {
                PirometerTemp = rtvDetails.PirometerTemp2,
                PirometerTempMax = rtvDetails.PirometerMax2,
                PirometerTempMin = rtvDetails.PirometerMin2,
            });
            vm.Pirometers.Add(new RTVPirometersViewModel
            {
                PirometerTemp = rtvDetails.PirometerTemp3,
                PirometerTempMax = rtvDetails.PirometerMax3,
                PirometerTempMin = rtvDetails.PirometerMin3,
            });
            vm.Pirometers.Add(new RTVPirometersViewModel
            {
                PirometerTemp = rtvDetails.PirometerTemp4,
                PirometerTempMax = rtvDetails.PirometerMax4,
                PirometerTempMin = rtvDetails.PirometerMin4,
            });
            vm.Pirometers.Add(new RTVPirometersViewModel
            {
                PirometerTemp = rtvDetails.PirometerTemp5,
                PirometerTempMax = rtvDetails.PirometerMax5,
                PirometerTempMin = rtvDetails.PirometerMin5,
            });
            vm.Pirometers.Add(new RTVPirometersViewModel
            {
                PirometerTemp = rtvDetails.PirometerTemp6,
                PirometerTempMax = rtvDetails.PirometerMax6,
                PirometerTempMin = rtvDetails.PirometerMin6,
            });
            vm.Pirometers.Add(new RTVPirometersViewModel
            {
                PirometerTemp = rtvDetails.PirometerTemp7,
                PirometerTempMax = rtvDetails.PirometerMax7,
                PirometerTempMin = rtvDetails.PirometerMin7,
            });
            vm.Pirometers.Add(new RTVPirometersViewModel
            {
                PirometerTemp = rtvDetails.PirometerTemp8,
                PirometerTempMax = rtvDetails.PirometerMax8,
                PirometerTempMin = rtvDetails.PirometerMin8,
            });
        }

        private static void PrepareDefaultData(RTVMachineDetailsViewModel vm)
        {
            vm.ProducedQty = 0;
            vm.CycleTime = 0;
            vm.PartCode = "None";
            vm.ProgramName = "None";
            vm.Pirometers = new List<RTVPirometersViewModel>();
            vm.Pirometers.Add(new RTVPirometersViewModel
            {
                PirometerTemp = 0,
                PirometerTempMax = 0,
                PirometerTempMin = 0,
            });
        }

        //[HttpPost]
        //public JsonResult GetMinuteDetails(DateTime date, int machineId)
        //{
        //    List<RTVOEEProductionData> productionData = uowRTV.RTVOEEProductionDataRepo.GetDataOfMinuteMachineId(date, machineId);

        //    if(productionData == null)
        //    {
        //        productionData = new List<RTVOEEProductionData>();
        //        productionData.Add(new RTVOEEProductionData() { CycleTime = 1, ReasonId = null, ProdQty = 1 });
        //    }

        //    decimal cycleTime = 0;
        //    int entryType = -1;
        //    decimal prodQty = 0;
        //    decimal usedTime = 0;

        //    //var jd = new { date = date.ToString("yyyy-MM-dd HH:mm"), CycleTime = productionData.CycleTime, productionData.entryType, productionData.ProdQty };
            
        //    foreach (RTVOEEProductionData productionD in productionData)
        //    {
        //        cycleTime = productionD.CycleTime;
        //        entryType = (int)productionD.ReasonType.EntryType;
        //        prodQty += productionD.ProdQty;
        //        usedTime += productionD.UsedTime;
        //    }

        //    return Json(new { date = date.ToString("yyyy-MM-dd HH:mm"), cycleTime, entryType, prodQty, usedTime }, JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public JsonResult GetMinutesDetails(DateTime dateFrom, DateTime dateTo, int machineId)
        {
            List<RTVOEEProductionData> productionData = uowRTV.RTVOEEProductionDataRepo.GetDataOfMinutesMachineId(dateFrom, dateTo, machineId);
            List<RTVOEEChartMinutesViewModel> data = new List<RTVOEEChartMinutesViewModel>();
            List<RTVOEEProductionData> list = new List<RTVOEEProductionData>();

            for (DateTime currentMinute = dateFrom; currentMinute <= dateTo; currentMinute = currentMinute.AddMinutes(1))
            {
                list = productionData.Where(x => (currentMinute <= x.TimeStamp && x.TimeStamp < currentMinute.AddMinutes(1))).OrderBy(x => x.Id).ToList();

                if (list == null || list.Count < 1)
                {
                    list = productionData.Where(x => (x.ProductionDate <= currentMinute && currentMinute.AddMinutes(1) < x.TimeStamp)).OrderBy(x => x.Id).ToList();
                }
                if (list == null || list.Count < 1)
                {
                    list.Add(new RTVOEEProductionData() { ReasonType = new ReasonType() { Name = "Undefined", Id = -1, EntryType = EnumEntryType.Undefined }, CycleTime = 0, ProdQty = 0, UsedTime = 0 });
                }

                RTVOEEChartMinutesViewModel ds = new RTVOEEChartMinutesViewModel();
                foreach (RTVOEEProductionData prodDt in list)
                {
                    ds.Date = currentMinute.ToString("yyyy-MM-dd HH:mm");
                    ds.Minute = Convert.ToInt32((currentMinute- dateFrom).TotalMinutes);
                    ds.CycleTime = prodDt.CycleTime;
                    ds.ReasonTypeId = prodDt.ReasonTypeId != null && prodDt.ReasonTypeId != (int)EnumEntryType.ReasonNotSelected ? (int)prodDt.ReasonTypeId : 30; //jezeli powod nieznany to zaklada się postój nieplanowany
                    ds.EntryType = prodDt.ReasonType != null && prodDt.ReasonTypeId != (int)EnumEntryType.ReasonNotSelected ? (int)prodDt.ReasonType.EntryType : 30; //(int)EnumEntryType.Undefined;
                    ds.ProdQty += prodDt.ProdQty;
                    ds.UsedTime += prodDt.UsedTime;
                    //ds.Color = productionD.ReasonTypeId != null ? productionD.ReasonType.Color : "gray";
                }
                data.Add(ds);
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CheckConnections(int machineId)
        {
            RTVOEEProductionDataDetails dataDetails = uowRTV.Db.RTVOEEProductionDataDetails.Where(x=>x.RTVOEEProductionData.MachineId == machineId).OrderByDescending(x => x.Id).Take(1).FirstOrDefault();
            RTVOEEPLCData rawPlcData = uowRTV.Db.RTVOEEPLCData.Where(x => x.MachineId == machineId).OrderByDescending(x => x.Id).Take(1).FirstOrDefault();

            DateTime lastRawDataTime = rawPlcData != null? rawPlcData.LastUpdate : new DateTime(2000,1,2);
            DateTime lastDateDetails = dataDetails != null? dataDetails.RTVOEEProductionData.TimeStamp : new DateTime(2000, 1, 1);

            int PLCStatus = Convert.ToInt16(rawPlcData != null ? rawPlcData.PlcStatus : false);
            int PLCConnectorStatus = Convert.ToInt16((DateTime.Now - lastRawDataTime).TotalSeconds < 60);
            int PLCDataAnalyzerStatus = Convert.ToInt16((lastRawDataTime - lastDateDetails).TotalSeconds < 60);
            int Connection = Convert.ToInt16((PLCStatus + PLCConnectorStatus + PLCDataAnalyzerStatus) >= 3);

            return Json(new { PLCStatus, PLCConnectorStatus, PLCDataAnalyzerStatus, Connection }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetWorkorders(DateTime dateFrom, int machineId)
        {
            DateTime dateTo = dateFrom.AddHours(24);

            var data1 = uowRTV.Db.Workorders.Where(x => x.ResourceId == machineId && x.EndTime > dateFrom && x.StartTime < dateTo).
                        Select(x => new {
                            x.StartTime,
                            x.ClientOrder.OrderNo,
                            x.Item.Code,
                            ItemGroupName = x.Item.ItemGroupOP.Name,
                            x.ProcessingTime,
                            x.Qty_Total,
                            x.Qty_Produced
                        })
                        .OrderBy(x => x.StartTime)
                        .ToList();

            return Json(data1);
        }

        [HttpPost]
        public JsonResult SaveScreenshot(string base64image, int machineId, int shift)
        {
            DateTime shiftDate = shift == 3? DateTime.Now.AddDays(-1) : DateTime.Now;
            string fileName = GenerateRTVScreenShotFileName(machineId, DateTime.Now, shift);
            string path = GenerateRTVScreenShotFilePath();

            if (!path.EndsWith(@"\")) { path += @"\"; }

            if (System.IO.File.Exists(Path.Combine(path, fileName)))
            {
                System.IO.File.Delete(Path.Combine(path, fileName));
            }

            if (string.IsNullOrEmpty(base64image))
                return Json(0);

            var t = base64image.Substring(22);  // remove data:image/png;base64,
            byte[] bytes = Convert.FromBase64String(t);

            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }
            var fullPath = Path.Combine(path, fileName);
            image.Save(fullPath, System.Drawing.Imaging.ImageFormat.Jpeg);

            return Json(0);
        }

        public static string GenerateRTVScreenShotFilePath()
        {
            return HostingEnvironment.MapPath("~/Uploads/RTVscreenshots");
        }

        public static string GenerateRTVScreenShotFileName(int machineId, DateTime date, int shift)
        {
            return "rtv_" + machineId + "_" + date.Date.ToString("yyyyMMdd") + "_" + shift + ".jpg";
        }
    }
}