using MDL_ONEPROD;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Manager;
using MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.Repo;
using _MPPL_WEB_START.Areas.ONEPROD.Base.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Model.Scheduling.Interface;
using MDL_BASE.Models.IDENTITY;
using MDL_ONEPROD.ComponetMes.Models;
using MDL_ONEPROD.ComponentWMS.UnitOfWorks;
using MDL_ONEPROD.ComponentWMS._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_ONEPROD.ComponentENERGY;
using MDL_ONEPROD.ComponentENERGY.Entities;
using MDL_CORE.ComponentCore.Entities;
using _MPPL_WEB_START.Areas.ONEPROD.ViewModels.OEE;
using _MPPL_WEB_START.Areas.ONEPROD.Models;
using _MPPL_WEB_START.Areas._APPWEB.ViewModels;
using System.Net;
using System.IO;
using System.Data;
using Quartz;
using System.Threading.Tasks;
using MDLX_CORE.ComponentCore.Entities;
using MDL_ONEPROD.ComponentENERGY.Models;
using _MPPL_WEB_START.Areas._APPWEB.Models;
using _MPPL_WEB_START.App_Start;
using XLIB_COMMON.Model;

namespace _MPPL_WEB_START.Areas.ONEPROD.Controllers
{
    [Authorize(Roles = DefRoles.ONEPROD_VIEWER)]
    public class EnergyController : Controller
    {
        IDbContextOneProdENERGY db;
        UnitOfWorkOneProdENERGY uow;
        UnitOfWorkOneprod uowONEPROD;
        UnitOfWorkOneProdOEE uowOEE;

        public EnergyController(IDbContextOneProdENERGY db3 = null, IDbContextOneProdOEE db2 = null)
        {
            this.db = db3;
            uow = new UnitOfWorkOneProdENERGY(db3);
            uowONEPROD = new UnitOfWorkOneprod(db3);
            uowOEE = new UnitOfWorkOneProdOEE(db2);
            ViewBag.Skin = "defaultSkin";

        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EnergyDetails()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetEnergySummaryData(DateTime dateFrom, DateTime dateTo, int machineId, int intervalInHours)
        {
            if (AppClient.appClient.SettingsONEPROD.MediaEnabled == true)
            {
                List<OEEReportProductionData> ProdDataRepo = uowOEE.OEEReportProductionDataRepo.GetDataByTimeRangeAndMachineId(dateFrom, dateTo, machineId).ToList();
                List<EnergyConsumptionData> EnConsumptData = uow.EnergyConsumptionDataRepo.GetDataByTimeRangeAndMachineIdAndEnergyType(dateFrom, dateTo, machineId, EnumEnergyType.ALL).ToList();

                EnergyAnalyzeModel enAnModel = new EnergyAnalyzeModel(EnConsumptData, ProdDataRepo);
                OEEEnergyReport EnergyReport = new OEEEnergyReport();
                EnergyReport.PricePerProductionUnit = enAnModel.PricePerProductionUnit(dateFrom, dateTo);
                EnergyReport.TotalCost = enAnModel.TotalCostOnMachine();
                EnergyReport.UsePerProductionUnit = enAnModel.UsePerUnit();
                EnergyReport.ElectricityMetersData = enAnModel.SetDataForEnergyMeter(EnumEnergyType.Electricity);
                EnergyReport.WaterMetersData = enAnModel.SetDataForEnergyMeter(EnumEnergyType.Air);
                EnergyReport.GasMetersData = enAnModel.SetDataForEnergyMeter(EnumEnergyType.Gas);
                EnergyReport.HeatMetersData = enAnModel.SetDataForEnergyMeter(EnumEnergyType.Heat);

                ChartEnergyOEEReport chartEnergyOEEReport = new ChartEnergyOEEReport(enAnModel, dateFrom, dateTo, intervalInHours);
                EnergyReport.ChartPricePerUnit = chartEnergyOEEReport.PrepareChartPricePerUnit("PPUnit");
                EnergyReport.ChartTotalCostByType = chartEnergyOEEReport.PrepareChartTotalCost("TCBytype");
                EnergyReport.ChartUseEnergyPerUnit = chartEnergyOEEReport.PrepareChartUseEnergyPerUnit("UEPUnit");
                EnergyReport.ChartEnergyConsumption = chartEnergyOEEReport.PrepareChartEnergyConsumption("Production");

                return Json(EnergyReport);
            }
            else
            {
                return Json(null);
            }

        }

        [HttpPost]
        public JsonResult CheckAndImportNewDataFromCSV()
        {
            uow.SystemVariableRepo.UpdateJobEnergyMetersImportLastRun();
            DateTime lastImportedFileDate = uow.SystemVariableRepo.GetLastEnergyImportDate().Date;
            DateTime todayDate = DateTime.Now.Date;

            List<EnergyMeter> energyMeters = new List<EnergyMeter>();
            energyMeters = uow.EnergyMeterRepo.GetAllEnergyMeters();

            for (DateTime date = lastImportedFileDate; date <= todayDate; date = date.AddMonths(1))
            {
                CheckAndImportNewDataFromCSVByDate(date, energyMeters);
            }
            return Json(0);
        }
        private void CheckAndImportNewDataFromCSVByDate(DateTime currentMonthDate, List<EnergyMeter> energyMeters)
        {
            Logger2FileSingleton.Instance.SaveLog("EnergyController_CheckAndImportNewDataFromCSVByDate");
            DateTime lastImpFileDate = uow.SystemVariableRepo.GetLastEnergyImportDate();

            if (currentMonthDate.Date.Month > lastImpFileDate.Date.Month)
            {
                uow.SystemVariableRepo.UpdateEnergyImportRow(0);
                uow.SystemVariableRepo.UpdateEnergyImportFileSize(0);
                uow.SystemVariableRepo.UpdateEnergyImportDate(currentMonthDate);
            }

            Stream stream = GetFileFromFTP(currentMonthDate);
            //Stream stream = GetFileFromDisk(currentMonthDate);

            if (HasFileNewData(stream))
            {
                long streamLength = stream.Length;
                List<EnergyMeterImport> importedData = ReadDataFromCSV(energyMeters, stream, out long lastReadRow);
                List<EnergyConsumptionData> energyConsumptionData = AdaptImportedDataToEnergyConsumptionData(importedData);
                DateTime maxDateFound = importedData != null && importedData.Count > 0? importedData[0].Data.Max(x => x.TimeStamp) : currentMonthDate;

                using (var transaction = uow.BeginTransaction())
                {
                    try
                    {
                        SaveNewDataToDB(energyConsumptionData);
                        uow.SystemVariableRepo.UpdateEnergyImportRow(lastReadRow);
                        uow.SystemVariableRepo.UpdateEnergyImportFileSize(streamLength);
                        uow.SystemVariableRepo.UpdateEnergyImportDate(maxDateFound);
                        transaction.Commit();
                    }
                    catch(Exception ex)
                    {
                        transaction.Rollback();
                        Logger2FileSingleton.Instance.SaveLog("EnergyController_CheckAndImportNewDataFromCSVByDate Transaction Exception: " + ex.Message);
                    }
                }
            }
        }
        private Stream GetFileFromFTP(DateTime yearMothDate)
        {
            //string filepath = "80.211.131.243";
            //string filename = "devices_" + date.ToString("yyyy-MM") + ".csv";
            //string ftpUserName = "impleaftp";
            //string ftpPassword = "Implea123$";
            string filepath = "10.92.25.73/energy";
            string filename = "devices_" + yearMothDate.ToString("yyyy-MM") + ".csv";
            string ftpUserName = "wsmeasure";
            string ftpPassword = "wsmeasure";

            try
            {
                FtpWebRequest objFTPRequest;
                objFTPRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + filepath + "/" + filename));
                objFTPRequest.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
                objFTPRequest.KeepAlive = false;
                objFTPRequest.UseBinary = true;
                objFTPRequest.Method = WebRequestMethods.Ftp.DownloadFile;

                FtpWebResponse response = (FtpWebResponse)objFTPRequest.GetResponse();
                Stream responseStream = response.GetResponseStream();

                var ms = new MemoryStream();
                responseStream.CopyTo(ms);

                return ms;
            }
            catch(Exception ex)
            {
                Logger2FileSingleton.Instance.SaveLog("EnergyController_GetFileFromFTP Exception: " + ex.Message);
                return null;
            }
        }
        private bool HasFileNewData(Stream stream)
        {
            if(stream != null)
            {
                long LastFileSize = uow.SystemVariableRepo.GetLastEnergyImportFileSize();
                return stream.Length > LastFileSize;
            }
            else
            {
                return false;
            }
        }
        private List<EnergyMeterImport> ReadDataFromCSV(List<EnergyMeter> energyMeters, Stream stream, out long lastReadRow)
        {
            Logger2FileSingleton.Instance.SaveLog("EnergyController_ReadDataFromCSV");
            List<EnergyMeterImport> energyMeterImportList = new List<EnergyMeterImport>();
            stream.Position = 0;
            lastReadRow = uow.SystemVariableRepo.GetLastEnergyImportRow();

            using (var reader = new StreamReader(stream))
            {
                reader.DiscardBufferedData();

                long currentRow = 0;
                while (!reader.EndOfStream)
                {
                    if (currentRow == 0)
                    {
                        PrepareEnergyMetersList(energyMeters, energyMeterImportList, reader);
                        currentRow++;
                    }
                    else if (currentRow <= lastReadRow)
                    {
                        currentRow += SkipToLastReadRow(lastReadRow, reader);
                    }
                    else
                    {
                        ReadNewRow(energyMeterImportList, reader);
                        currentRow++;
                    }
                }
                lastReadRow = currentRow;
            }
            return energyMeterImportList;
        }
        private void ReadNewRow(List<EnergyMeterImport> energyMeterImportList, StreamReader reader)
        {
            try
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                foreach (EnergyMeterImport emi in energyMeterImportList)
                {
                    decimal _value = Decimal.Parse(values[emi.ColumnIndexCsv]);
                    DateTime _date = DateTime.Parse(values[0]);

                    EnergyMeterImportData energyMeterImportData = new EnergyMeterImportData();
                    energyMeterImportData.ReadValue = _value;
                    energyMeterImportData.TimeStamp = _date;
                    emi.Data.Add(energyMeterImportData);
                }
            }
            catch(Exception ex)
            {
                Logger2FileSingleton.Instance.SaveLog("EnergyController_ReadNewRow Exception: " + ex.Message);
            }
        }
        private long SkipToLastReadRow(long lastReadRow, StreamReader reader)
        {
            try
            {
                int numberOfReadRows = 0;
                for (int i = 1; i <= lastReadRow; i++)
                {
                    reader.ReadLine();
                    numberOfReadRows++;
                }
                return numberOfReadRows;
            }
            catch (Exception ex)
            {
                Logger2FileSingleton.Instance.SaveLog("EnergyController_SkipToLastReadRow Exception: " + ex.Message);
                return 0;
            }
        }
        private void PrepareEnergyMetersList(List<EnergyMeter> energyMeters, List<EnergyMeterImport> energyMeterImportList, StreamReader reader)
        {
            try
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                foreach (EnergyMeter energyMeter in energyMeters)
                {
                    if (values.Contains(energyMeter.MarkedName))
                    {
                        EnergyMeterImport energyMeterImport = new EnergyMeterImport();
                        energyMeterImport.ColumnIndexCsv = Array.FindIndex(values, x => x == energyMeter.MarkedName);
                        energyMeterImport.EnergyMeterId = energyMeter.Id;
                        energyMeterImport.EnergyType = energyMeter.EnergyType;
                        energyMeterImportList.Add(energyMeterImport);
                    }
                }
            }
            catch(Exception ex)
            {
                Logger2FileSingleton.Instance.SaveLog("EnergyController_PrepareEnergyMetersList Exception: " + ex.Message);
            }
        }
        private List<EnergyConsumptionData> AdaptImportedDataToEnergyConsumptionData(List<EnergyMeterImport> energyMeterImportList)
        {
            List<EnergyConsumptionData> energConsumptionDataList = new List<EnergyConsumptionData>();
            EnergyCost energyCostLast = new EnergyCost();
            decimal kWhPriceLast = 0.0m;

            foreach (EnergyMeterImport energyMeterImport in energyMeterImportList)
            {
                energyCostLast = new EnergyCost();

                foreach (EnergyMeterImportData emi_data in energyMeterImport.Data)
                {
                    if (emi_data.ReadValue > 0)
                    {
                        if (kWhPriceLast != 0.0m &&
                           energyCostLast != null &&
                           energyCostLast.StartDate <= emi_data.TimeStamp &&
                           energyCostLast.EndDate >= emi_data.TimeStamp &&
                           energyCostLast.EnergyType == energyMeterImport.EnergyType)
                        {
                            energConsumptionDataList.Add(CreateEnergyConsumptionData(energyMeterImport, emi_data, energyCostLast, kWhPriceLast));
                        }
                        else
                        {
                            EnergyCost energyCost = uow.EnergyCostRepo.GetPaymentPeriod(emi_data, energyMeterImport.EnergyType);
                            decimal kWhPrice = energyMeterImport.EnergyType == EnumEnergyType.Electricity ? energyCost.PricePerUnit : uow.EnergyCostRepo.GetPaymentPeriod(emi_data, EnumEnergyType.Electricity).PricePerUnit;
                            kWhPriceLast = kWhPrice;
                            energyCostLast = energyCost;

                            energConsumptionDataList.Add(CreateEnergyConsumptionData(energyMeterImport, emi_data, energyCost, kWhPrice));
                        }
                    }
                }
            }
            return energConsumptionDataList;
        }
        private EnergyConsumptionData CreateEnergyConsumptionData(EnergyMeterImport energyMeterImport, EnergyMeterImportData energyImportData, EnergyCost energyCost, decimal kwhPrice)
        {
            EnergyConsumptionData energyConsumptData = new EnergyConsumptionData();
            energyConsumptData.EnergyMeterId = energyMeterImport.EnergyMeterId;
            energyConsumptData.ImportDate = DateTime.Now;
            energyConsumptData.DateFrom = energyImportData.TimeStamp;
            energyConsumptData.DateTo = energyImportData.TimeStamp.AddMinutes(15);
            energyConsumptData.Qty = energyImportData.ReadValue;
            energyConsumptData.PricePerUnit = energyCost.UseConverter? energyCost.kWhConverter * kwhPrice : kwhPrice;
            energyConsumptData.TotalValue = energyConsumptData.PricePerUnit * energyImportData.ReadValue;

            return energyConsumptData;
        }
        private void SaveNewDataToDB(List<EnergyConsumptionData> energyConsumptionData)
        {
            //foreach (var ecd in energyConsumptionData)
            //{
            //uow.EnergyConsumptionDataRepo.Add(ecd);
            //}
            uow.EnergyConsumptionDataRepo.AddOrUpdateRange(energyConsumptionData);
        }
        [Obsolete]
        private Stream GetFileFromDisk(DateTime date)
        {
            //string filepath = "?";
            try
            {
                string filename = "devices_" + date.ToString("yyyy-MM") + ".csv";
                //string filetemp = "devices_2019-08.csv";
                string mapPath2 = @"C:\temp\";
                Stream responseStream = System.IO.File.OpenRead(mapPath2 + filename);
                return responseStream;
            }
            catch
            {
                return null;
            }
        }

    }
}