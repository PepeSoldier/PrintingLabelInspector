using MDL_BASE.Interfaces;
using MDL_BASE.Models.Base;
using MDL_BASE.Models.IDENTITY;
using MDL_BASE.ViewModel;
using MDL_CORE.ComponentCore.Entities;
using MDLX_CORE.ComponentCore.UnitOfWorks;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using XLIB_COMMON.Model;

namespace _MPPL_WEB_START.Areas.CORE.Controllers
{
    public class SignController : Controller
    {
        public SignController()
        {
            ViewBag.Skin = "nasaSkin";
        }

        // GET: CORE/Sign
        public ActionResult Sign(int id)
        {
            return View(id);
        }
    }
}