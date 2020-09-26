using Autofac;
using Autofac.Integration.Mvc;
using MDL_ONEPROD.ComponentBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.ONEPROD.Controllers
{
    public class DataExchangeController : Controller
    {
        // GET: ONEPROD/DataExchange
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ImportOrders()
        {
            ImportData impData = AutofacDependencyResolver.Current.ApplicationContainer.Resolve<ImportData>();
            impData.ImportOrders();
            return View("");
        }
    }
}