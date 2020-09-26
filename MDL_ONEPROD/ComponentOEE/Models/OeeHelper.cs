using MDL_ONEPROD.ComponentOEE.ViewModels;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Enums;
using XLIB_COMMON.Model;

namespace MDL_ONEPROD.ComponentOEE.Models
{
    public class OeeHelper
    {
        public static decimal _VerifyReport(UnitOfWorkOneProdOEE uowOEE, int reportId, string userId)
        {
            List<OEEReportProductionDataAbstract> productionData = uowOEE.OEEReportProductionDataRepo
                .GetListByReportId(reportId).ToList<OEEReportProductionDataAbstract>();

            if (productionData.Count > 0)
            {
                Logger2FileSingleton.Instance.SaveLog("OEECreateReportController._VerifyReport productionData.Count=" + productionData.Count + ", raportId:" + reportId);

                OeeCalculationModel oeeCalc = new OeeCalculationModel(productionData);
                oeeCalc.CalculateOEE();

                int microstopsAutoReasonId = uowOEE.SystemVariableRepo.GetValueInt("MicrostopsAutoReasonId");
                Reason microstopAuto = uowOEE.ReasonRepo.GetById(microstopsAutoReasonId);

                OEEReportProductionDataAbstract pd = uowOEE.OEEReportProductionDataRepo
                    .GetListByReportId(reportId).Where(x => x.ReasonId == microstopsAutoReasonId).FirstOrDefault();

                if (microstopAuto != null && pd != null)
                {
                    if (oeeCalc.MicrostopsAuto > 0 && oeeCalc.MicrostopsAuto < 28800)
                    {
                        pd.UsedTime = Math.Max(0, pd.UsedTime + oeeCalc.MicrostopsAuto);
                    }
                    else
                    {
                        Logger2FileSingleton.Instance.SaveLog("OEECreateReportController._VerifyReport MicrostopsAuto=:" + oeeCalc.MicrostopsAuto);
                        pd.UsedTime = Math.Max(0, pd.UsedTime - Math.Abs(oeeCalc.MicrostopsAuto));
                    }
                    uowOEE.OEEReportProductionDataRepo.AddOrUpdate(pd);
                }
                else
                {
                    if (oeeCalc.MicrostopsAuto > 0 && oeeCalc.MicrostopsAuto < 28800)
                    {
                        CreateReportStoppageViewModel vm = new CreateReportStoppageViewModel();
                        vm.ReportId = reportId;
                        vm.ReasonId = microstopAuto.Id;
                        vm.UsedTime = oeeCalc.MicrostopsAuto;
                        //vm.EntryType = EnumEntryType.StopUnplannedPreformance;
                        vm.ReasonTypeId = microstopAuto.ReasonTypeId; // (int)EnumEntryType.StopPerformance; //TODO: uzupełnić prawidłowy ID !!!!!!!!!!!!!!!!!!!
                        StoppadeUpdateFunction(uowOEE, vm, userId);
                    }
                    else
                    {
                        Logger2FileSingleton.Instance.SaveLog("OEECreateReportController._VerifyReport MicrostopsAuto=:" + oeeCalc.MicrostopsAuto);
                    }
                }

                OEEReport report = uowOEE.OEEReportRepo.GetById(reportId);
                report.IsDraft = false;
                uowOEE.OEEReportRepo.Update(report);

                return oeeCalc.MicrostopsAuto;
            }
            else
            {
                Logger2FileSingleton.Instance.SaveLog("OEECreateReportController._VerifyReport productionData.Count=0, raportId:" + reportId);
                return 0;
            }
        }
        public static void StoppadeUpdateFunction(UnitOfWorkOneProdOEE uowOEE, CreateReportStoppageViewModel vm, string userId)
        {
            OEEReport report = uowOEE.OEEReportRepo.GetById(vm.ReportId);

            if (report != null)
            {
                vm.ProductionDate = GetProductionDate(report.ReportDate, vm.Hour, report.Shift);
                OEEReportProductionData pd = uowOEE.OEEReportProductionDataRepo
                    .AddOrUpdateStoppageData(vm, userId);

                vm.DetailId = pd.DetailId;
                pd.DetailId = UpdateProductionDataDetails(uowOEE, vm);

                if (vm.DetailId != pd.DetailId)
                {
                    uowOEE.OEEReportProductionDataRepo.AddOrUpdate(pd);
                }
            }
        }
        public static DateTime GetProductionDate(DateTime reportDate, string hourFromVm, Shift shift)
        {
            int hour = (((int)shift - 1) * 8) + 6;
            DateTime? hour2 = null;

            if (hourFromVm != null)
            {
                try { hour2 = Convert.ToDateTime(hourFromVm); }
                catch { }

                return reportDate.Add(hour2.Value.TimeOfDay);
            }

            return reportDate.AddHours(hour);
        }
        public static int UpdateProductionDataDetails(UnitOfWorkOneProdOEE uowOEE, CreateReportProdDataDetailViewModel vm)
        {
            OEEReportProductionDataDetails pdd = new OEEReportProductionDataDetails();
            if (vm.DetailId != null && vm.DetailId > 0)
            {
                pdd = uowOEE.OEEReportProductionDataDetailsRepo.GetById(Convert.ToInt32(vm.DetailId));
            }

            pdd.Comment = vm.Comment;
            pdd.ProductionCycleTime = vm.ProductionCycleTime;

            pdd.FormWeightProcess = vm.FormWeightProcess;
            pdd.FormWeightScrap = vm.FormWeightScrap;
            pdd.PaperWeight = vm.PaperWeight;
            pdd.TubeWeight = vm.TubeWeight;
            pdd.CoilId = vm.CoilId;

            uowOEE.OEEReportProductionDataDetailsRepo.AddOrUpdate(pdd);

            return pdd.Id;
        }
    }
}