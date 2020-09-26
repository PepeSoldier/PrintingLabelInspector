using MDL_BASE.Models.Base;

using XLIB_COMMON.Repo.Base;
using MDL_PFEP.Interface;
using MDL_PFEP.Models.DEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_PFEP.Repo.PFEP
{
    //public class ProductionOrderPFEPRepo : ProductionOrderRepo
    //{
    //    IDbContextPFEP db;

    //    public ProductionOrderPFEPRepo(IDbContextPFEP db)
    //        : base(db)
    //    {
    //        this.db = db;
    //    }

    //    public new ProductionOrder GetById(int id)
    //    {
    //        return db.ProductionOrders.FirstOrDefault(d => d.Id == id);
    //    }
    //    public new IQueryable<ProductionOrder> GetList()
    //    {
    //        return db.ProductionOrders.OrderByDescending(x => x.Id);
    //    }

    //    public new IQueryable<ProductionOrder> GetListNew(ProductionOrderSort sorter)
    //    {
    //        DateTime StartDate = Convert.ToDateTime(sorter.Date);
    //        DateTime DateLimit = StartDate.AddHours(24);
    //        int PncId = Convert.ToInt32(sorter.Pnc);

    //        IQueryable<ProductionOrder> pO = db.ProductionOrders.AsNoTracking()
    //            .Where(x =>
    //             ((x.StartDate >= StartDate && x.StartDate <= DateLimit) || StartDate.Year == 1) &&
    //             (x.Line.Name == sorter.Line || sorter.Line == null) && (x.Line.Name != "901") &&
    //             (x.OrderNumber == sorter.OrderNumber || sorter.OrderNumber == null) &&
    //             (x.PncId == PncId || PncId == 0)
    //             )
    //             .OrderBy(o => o.StartDate) 
    //             ;
    //        return pO;
    //    }

    //    public new List<string> GetPncIdList(string prefix)
    //    {
    //        return db.ProductionOrders.Where(x => x.PncId.ToString().StartsWith(prefix)).Select(x => x.PncId.ToString()).Distinct().Take(5).ToList();
    //    }

    //    public new List<string> GetProductionOrderList(string prefix)
    //    {
    //        return db.ProductionOrders.Where(x => x.OrderNumber.StartsWith(prefix)).Select(x => x.OrderNumber).Distinct().Take(5).ToList();
    //    }

    //    public new List<Pnc> GetPNCsByTimeRange(DateTime dateFrom, DateTime dateTo)
    //    {
    //        return db.ProductionOrders.Where(x => x.StartDate >= dateFrom && x.StartDate < dateTo).Select(x => x.Pnc).ToList();
    //    }

    //    public new List<ProductionOrder> GetByTimeRange(DateTime dateFrom, DateTime dateTo)
    //    {
    //        return db.ProductionOrders.Where(x => x.StartDate >= dateFrom && x.StartDate < dateTo).ToList();
    //    }


    //}
}