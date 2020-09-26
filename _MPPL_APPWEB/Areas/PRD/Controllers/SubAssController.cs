using MDL_PRD.Interface;
using MDL_PRD.Repo;
using MDL_PRD.ViewModel;
using System;
using System.Linq;
using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.PRD.Controllers
{
    public class SubAssController : Controller
    {
        IDbContextPRD _db;
        UnitOfWork _uow;
        UnitOfWork unitofWork
        {
            get {
                if(_uow == null) _uow = new UnitOfWork(_db);
                return _uow;
            }
        }

        public SubAssController(IDbContextPRD db)
        {
            this._db = db;
        }

        public ActionResult Index()
        {
            SubAssViewModel vm = new SubAssViewModel();

            DateTime dtFrom = DateTime.Now.Date.AddHours(6); //new DateTime(2018,03,5, 14,0,0);
            DateTime dtTo = dtFrom.AddHours(12);
            string[] lines = new string[3] {"101","103","104"};

            vm.Lines = unitofWork.RepoLine.GetIQ().Where(x=> lines.Contains(x.Name)).ToList();
            vm.ProductionOrders = unitofWork.ProductionOrderRepo.GetByTimeRange(dtFrom, dtTo);

            return View(vm);
        }
    }
}