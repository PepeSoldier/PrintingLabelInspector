using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MDL_ONEPROD.ComponentMes.ViewModels;
using MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.Repo.Scheduling;

namespace _MPPL_WEB_START.Areas.ONEPROD.Models
{
    public class ProductionLogDataPreparer
    {
        //public List<ProductionLogViewModel> GetPLViewModelFromPL(IQueryable<ProductionLog> productionLogQuery)
        //{
        //    List<ProductionLogViewModel> list = new List<ProductionLogViewModel>();

        //    foreach (ProductionLog pl in productionLogQuery)
        //    {
        //        ProductionLogViewModel temp = new ProductionLogViewModel();
        //        temp.Id = pl.Id;
        //        temp.TimeStamp = pl.TimeStamp;
        //        temp.OEEReportProductionData = pl.OEEReportProductionData;
        //        temp.Workplace = pl.Workplace;
        //        //temp.Item = pl.Item;
        //        temp.ItemCode = pl.ItemCode;
        //        temp.ReasonType = pl.ReasonType;
        //        temp.Reason = pl.Reason;
        //        temp.ClientWorkOrderNumber = pl.ClientWorkOrderNumber;
        //        temp.InternalWorkOrderNumber = pl.InternalWorkOrderNumber;
        //        temp.WorkorderTotalQty = pl.WorkorderTotalQty;
        //        temp.DeclaredQty = pl.DeclaredQty;
        //        temp.UsedQty = pl.UsedQty;
        //        temp.SerialNo = pl.SerialNo;
        //        temp.UserName = pl.UserName;
        //        temp.CostCenter = pl.CostCenter;
        //        temp.TransferNumber = pl.TransferNumber;
        //        temp.QtyAvailable = pl.QtyAvailable;
        //        list.Add(temp);
        //    }

        //    return list;
        //}

        //public List<ProductionLogViewModel> GetPLViewModel(DbSet<ProductionLog> productionLogs, FilterProductionLogViewModel filter)
        //{
        //    List<ProductionLogViewModel> list = new List<ProductionLogViewModel>();
        //    IQueryable<ProductionLog> query = productionLogs.Where(x => x.Deleted == false &&
        //    (filter.MachineId == null || x.Workplace.MachineId == filter.MachineId) &&
        //    (filter.SerialNumber == null || x.SerialNo == filter.SerialNumber) &&
        //            (filter.ItemCode == null || x.Item.Code == filter.ItemCode) &&
        //            (filter.WorkOrder == null || x.InternalWorkOrderNumber == filter.WorkOrder))
        //    .OrderBy(x => x.Id);
        //    //IQueryable<ProductionLog> query = productionLogs.Where(x => x.Deleted == false).OrderBy(x => x.Id).Take(5);
        //    //var zm = query.ToList();
        //    foreach (ProductionLog pl in query)
        //    {
        //        ProductionLogViewModel temp = new ProductionLogViewModel();
        //        temp.Id = pl.Id;
        //        //temp.Item = pl.Item;
        //        //temp.Workplace = pl.Workplace;
        //        //temp.ItemCode = pl.ItemCode;
        //        temp.ClientWorkOrderNumber = pl.ClientWorkOrderNumber;
        //        temp.WorkorderTotalQty = pl.WorkorderTotalQty;
        //        temp.DeclaredQty = pl.DeclaredQty;
        //        temp.CostCenter = pl.CostCenter;
        //        temp.SerialNo = pl.SerialNo;
        //        temp.TimeStamp = pl.TimeStamp;
        //        temp.UserName = pl.UserName;
        //        temp.OEEReportProductionData = pl.OEEReportProductionData;
        //        temp.ReasonType = pl.ReasonType;
        //        temp.Reason = pl.Reason;
        //        temp.InternalWorkOrderNumber = pl.InternalWorkOrderNumber;
        //        temp.UsedQty = pl.UsedQty;
        //        temp.TransferNumber = pl.TransferNumber;
        //        temp.QtyAvailable = pl.QtyAvailable;
        //        list.Add(temp);
        //    }
        //    return list;
        //}
    }

}