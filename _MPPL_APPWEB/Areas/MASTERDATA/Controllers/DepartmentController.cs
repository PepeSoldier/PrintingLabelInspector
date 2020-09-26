using MDL_BASE.Models.MasterData;
using MDL_ONEPROD.Repo;
using MDLX_MASTERDATA._Interfaces;
using MDLX_MASTERDATA.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.MASTERDATA.Controllers
{
    public class DepartmentController : Controller
    {

        private readonly IDbContextMasterData db;
        UnitOfWorkMasterData uow;

        public DepartmentController(IDbContextMasterData db)
        {
            this.db = db;
            uow = new UnitOfWorkMasterData(db);
            ViewBag.Skin = "nasaSkin";
        }

        // GET: MASTERDATA/Department
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult DepartmentGetList()
        {
            List<Department> departments = new RepoDepartment(db).GetList().ToList();
            return Json(departments);
        }
    }
}