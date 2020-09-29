using _MPPL_WEB_START.Areas.CORE.ViewModels;
using _MPPL_WEB_START.Areas.ONEPROD.Models;
using MDL_BASE.Interfaces;
using MDL_CORE.ComponentCore.ViewModel;
using MDLX_CORE.ComponentCore.UnitOfWorks;
using Microsoft.AspNet.SignalR;
using System.Web.Mvc;
using XLIB_COMMON.Model;

namespace _MPPL_WEB_START.Areas.CORE.Controllers
{
    public class CommonController : Controller
    {
        private readonly UnitOfWorkCore uow;
        private readonly IDbContextCore db;
        public CommonController(IDbContextCore db)
        {
            this.db = db;
            ViewBag.Skin = "nasaSkin";
            uow = new UnitOfWorkCore(db);
        }
    }
}