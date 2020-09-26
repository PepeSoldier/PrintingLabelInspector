using _MPPL_WEB_START.Migrations;
using MDL_PFEP.Repo;
//using MDL_PFEP.Models.PFEP;
using MDL_PFEP.Models.DEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using _MPPL_WEB_START.Areas.PFEP.ViewModels;

namespace _MPPL_WEB_START.Areas.PFEP.Controllers
{
    public class HomeController : Controller
    {
        //DbContextAPP_Electrolux db;
        UnitOfWork uow;

        public HomeController()
        {
            DbContextAPP_ElectroluxPLV db = DbContextAPP_ElectroluxPLV.Create();
            uow = new UnitOfWork(db, db);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Print()
        {
            PrintDefViewModel vm = new PrintDefViewModel();
            vm.Defs = "154, 164";
            vm.DateRange = "14:00 - 18:00";
            vm.PrintDate = DateTime.Now;
            vm.Routine = "";
            vm.PrintNumber = 432;
            //vm.CodesAndOrders = new List<PrintModelAnc>();

            //AncPFEP anc1 = uow.RepoAnc.GetByID2(1096);
            //PrintModelAnc pdm = new PrintModelAnc(anc1);
            //pdm.AddOrders(uow.ProductionOrderRepo.GetByTimeRange(new DateTime(2018, 1, 4, 6, 0, 0), new DateTime(2018, 1, 4, 10, 0, 0)));

            //AncPFEP anc2 = uow.RepoAnc.GetByID2(1122);
            //PrintModelAnc pdm2 = new PrintModelAnc(anc2);
            //pdm2.AddOrders(uow.ProductionOrderRepo.GetByTimeRange(new DateTime(2018, 1, 4, 6, 0, 0), new DateTime(2018, 1, 4, 10, 0, 0)));

            //vm.CodesAndOrders.Add(pdm);
            //vm.CodesAndOrders.Add(pdm2);

            return View(vm);
        }

    }
}