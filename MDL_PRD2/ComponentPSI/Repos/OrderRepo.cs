using XLIB_COMMON.Repo;
using MDL_PRD.Interface;
using MDL_PRD.Model;
using System.Linq;

namespace MDL_PRD.Repo
{
    public class OrderRepo : RepoGenericAbstract<OrderModel>
    {
        protected new IDbContextPRD db;

        public OrderRepo(IDbContextPRD db) : base(db)
        {
            this.db = db;
        }

        public override OrderModel GetById(int id)
        {
            return db.PA_Order.FirstOrDefault(d => d.Id == id);
        }
        public override IQueryable<OrderModel> GetList()
        {
            return db.PA_Order.OrderByDescending(x => x.Id);
        }

        //public List<OrderArchiveModel> LoadData_Orders(DateTime _orderDate, int _shift, string _line)
        //{
        //    List<OrderArchiveModel> orders = null;

        //    if (_orderDate.Year > 2010)
        //    {
        //        _orderDate = _orderDate.Date;

        //        if (_shift == 1)
        //            _orderDate = _orderDate.AddHours(6);
        //        else if (_shift == 2)
        //            _orderDate = _orderDate.AddHours(14);
        //        else
        //            _orderDate = _orderDate.AddHours(22);

        //        DateTime orderDateU = _orderDate.AddHours(8);
        //        DateTime freezeDayArch = _orderDate.Date.AddDays(-3);
        //        DateTime checkDay = _orderDate.Date;
        //        DateTime freezeDayFrom = _orderDate.AddHours(-1);
        //        DateTime freezeDayTo = _orderDate;

        //        orders = db.PA_OrderArch
        //            .Where(o => (
        //                        ((o.StartDate >= _orderDate || o.EndDate >= _orderDate) &&
        //                        o.StartDate < orderDateU &&
        //                        (!(o.StartDate < _orderDate && o.EndDate > orderDateU)) &&
        //                        (freezeDayFrom < o.FreezeDate && o.FreezeDate < freezeDayTo))
        //                        ||
        //                        (o.FirstScanTime >= _orderDate && o.FirstScanTime < orderDateU)
        //                    )
        //                    &&
        //                    o.Line == _line
        //                ).OrderBy(o => o.StartDate).ToList<Model.OrderArchiveModel>();
        //    }

        //    return orders;
        //}

        //public List<OrderArchiveModel> LoadData_OrdersArch(DateTime _orderDate, int _shift, string _line)
        //{
        //    List<OrderArchiveModel> ordersArch = null;

        //    if (_orderDate.Year > 2010)
        //    {
        //        _orderDate = _orderDate.Date;

        //        if (_shift == 1)
        //            _orderDate = _orderDate.AddHours(6);
        //        else if (_shift == 2)
        //            _orderDate = _orderDate.AddHours(14);
        //        else
        //            _orderDate = _orderDate.AddHours(22);

        //        DateTime orderDateU = _orderDate.AddHours(8);
        //        DateTime freezeDayArch = _orderDate.Date.AddDays(-3);
        //        DateTime checkDay = _orderDate.Date;
        //        DateTime freezeDayFrom = _orderDate.AddHours(-1);
        //        DateTime freezeDayTo = _orderDate;

        //        ordersArch = db.PA_OrderArch
        //            .Where(o => (o.StartDate >= _orderDate || o.EndDate >= _orderDate) &&
        //                        o.StartDate < orderDateU &&
        //                        (!(o.StartDate < _orderDate && o.EndDate > orderDateU)) &&
        //                        DbFunctions.TruncateTime(o.FreezeDate) == freezeDayArch &&
        //                        o.Line == _line).OrderBy(o => o.StartDate).ToList<Model.OrderArchiveModel>();
        //    }

        //    return ordersArch;
        //}
    }
}