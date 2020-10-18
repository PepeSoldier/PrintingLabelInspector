using MDL_BASE.Interfaces;
using MDLX_CORE.ComponentCore.UnitOfWorks;
using System.Web.Mvc;

namespace _LABELINSP_APPWEB.Areas.CORE.Controllers
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