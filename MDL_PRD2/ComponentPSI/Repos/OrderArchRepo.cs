using XLIB_COMMON.Repo;
using MDL_PRD.Interface;
using MDL_PRD.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace MDL_PRD.Repo
{
    public class OrderArchRepo : RepoGenericAbstract<OrderArchiveModel>
    {
        protected new IDbContextPRD db;

        public OrderArchRepo(IDbContextPRD db) : base(db)
        {
            this.db = db;
        }

        public override OrderArchiveModel GetById(int id)
        {
            return db.PA_OrderArch.FirstOrDefault(d => d.Id == id);
        }
        public override IQueryable<OrderArchiveModel> GetList()
        {
            return db.PA_OrderArch.OrderByDescending(x => x.Id);
        }

        public List<OrderArchiveModel> LoadData_Orders(DateTime dateFrom, int shift, string line = null)
        {
            List<OrderArchiveModel> orders = null;

            if (dateFrom.Year > 2010)
            {
                dateFrom = dateFrom.Date;

                if (shift == 1)
                    dateFrom = dateFrom.AddHours(6);
                else if (shift == 2)
                    dateFrom = dateFrom.AddHours(14);
                else
                    dateFrom = dateFrom.AddHours(22);

                DateTime orderDateU = dateFrom.AddHours(8);
                DateTime freezeDayArch = dateFrom.Date.AddDays(-3);
                DateTime checkDay = dateFrom.Date;
                DateTime freezeDayFrom = dateFrom.AddHours(-1);
                DateTime freezeDayTo = dateFrom;

                orders = db.PA_OrderArch
                    .Where(o => (
                                ((o.StartDate >= dateFrom || o.EndDate >= dateFrom) &&
                                o.StartDate < orderDateU &&
                                (!(o.StartDate < dateFrom && o.EndDate > orderDateU)) &&
                                (freezeDayFrom < o.FreezeDate && o.FreezeDate < freezeDayTo))
                            )
                            &&
                            (line == null || o.Line == line)
                        ).OrderBy(o => o.StartDate).ToList<Model.OrderArchiveModel>();

                //List<OrderArchiveModel> orders2 = db.PA_OrderArch
                //    .Where(o => (
                //                !((o.StartDate >= dateFrom || o.EndDate >= dateFrom) &&
                //                o.StartDate < orderDateU &&
                //                (!(o.StartDate < dateFrom && o.EndDate > orderDateU)) &&
                //                (freezeDayFrom < o.FreezeDate && o.FreezeDate < freezeDayTo))
                //                &&
                //                (o.FirstScanTime >= dateFrom && o.FirstScanTime < orderDateU)
                //            )
                //            &&
                //            (line == null || o.Line == line)
                //        ).GroupBy(x => x.OrderNo).OrderBy(o => o.StartDate).ToList<Model.OrderArchiveModel>();

                orders = orders.OrderBy(o => o.StartDate).ToList<Model.OrderArchiveModel>();
            }

            return orders;
        }
        public List<OrderArchiveModel> LoadData_OrdersArch(DateTime dateFrom, int shift, string line)
        {
            List<OrderArchiveModel> ordersArch = null;

            if (dateFrom.Year > 2010)
            {
                dateFrom = dateFrom.Date;

                if (shift == 1)
                    dateFrom = dateFrom.AddHours(6);
                else if (shift == 2)
                    dateFrom = dateFrom.AddHours(14);
                else
                    dateFrom = dateFrom.AddHours(22);

                DateTime orderDateU = dateFrom.AddHours(8);
                DateTime freezeDayArch = dateFrom.Date.AddDays(-2);
                DateTime checkDay = dateFrom.Date;
                DateTime freezeDayFrom = dateFrom.AddHours(-1);
                DateTime freezeDayTo = dateFrom;

                ordersArch = db.PA_OrderArch
                    .Where(o => (o.StartDate >= dateFrom || o.EndDate >= dateFrom) &&
                                o.StartDate < orderDateU &&
                                (!(o.StartDate < dateFrom && o.EndDate > orderDateU)) &&
                                DbFunctions.TruncateTime(o.FreezeDate) == freezeDayArch &&
                                o.Line == line).OrderBy(o => o.StartDate).ToList<Model.OrderArchiveModel>();
            }

            return ordersArch;
        }

        public List<OrderArchiveModel> GetDataCurrent(DateTime dateFrom, int shift = 0, string line = null)
        {
            List<OrderArchiveModel> orders = new List<OrderArchiveModel>();
            int[] shifts = { 1,2,3 };

            foreach(int shift1 in shifts)
            {
                if(shift == 0 || shift1 == shift)
                orders.AddRange(LoadData_Orders(dateFrom, shift1, line));
            }
            
            return orders;
        }
        public List<OrderArchiveModel> GetDataArch(DateTime dateFrom, int shift = 0, string line = null)
        {
            List<OrderArchiveModel> ordersArch = new List<OrderArchiveModel>();
            int[] shifts = { 1, 2, 3 };

            foreach (int shift1 in shifts)
            {
                if (shift == 0 || shift1 == shift)
                    ordersArch.AddRange(LoadData_OrdersArch(dateFrom, shift1, line));
            }
            return ordersArch;
        }

    }
}