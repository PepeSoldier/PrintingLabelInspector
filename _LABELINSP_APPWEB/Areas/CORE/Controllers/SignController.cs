using MDLX_CORE.Interfaces;
using MDLX_CORE.Models.Base;
using MDLX_CORE.Models.IDENTITY;
using MDLX_CORE.ViewModel;
using MDLX_CORE.ComponentCore.UnitOfWorks;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using XLIB_COMMON.Model;

namespace _LABELINSP_APPWEB.Areas.CORE.Controllers
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