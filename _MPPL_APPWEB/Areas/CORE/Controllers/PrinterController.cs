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

namespace _LABELINSP_APPWEB.Areas.CORE.Controllers
{
    public class PrinterController : Controller
    {
        readonly IDbContextCore db;
        UnitOfWorkCore uow;

        public PrinterController(IDbContextCore dbContext)
        {
            ViewBag.Skin = "nasaSkin";
            db = dbContext;
            uow = new UnitOfWorkCore(db);
        }

        // GET: CORE/Printer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Printer()
        {
            Printer vm = new Printer();
            return View();
        }
        [HttpPost]
        public JsonResult PrinterGetList(PrinterViewModel item, int pageIndex, int pageSize)
        {
            Printer filter = new Printer();
            ReflectionHelper.CopyProperties(item, filter);

            IQueryable<Printer> query = uow.PrinterRepo.GetList(filter);
            int startIndex = (pageIndex - 1) * pageSize;
            int itemsCount = query.Count();
            List<PrinterViewModel> list = query.ToList().Skip(startIndex).Take(pageSize)
                .Select(x => new PrinterViewModel()
                {
                    Id = x.Id,
                    IpAdress = x.IpAdress,
                    Name = x.Name,
                    Model = x.Model,
                    SerialNumber = x.SerialNumber,
                    PrinterType = x.PrinterType,
                    Password = x.Password,
                    User = x.User
                })
                .ToList();

            return Json(new { data = list, itemsCount });
        }

        public ActionResult PrinterDelete(int id)
        {
            Printer wI = uow.PrinterRepo.GetById(id);
            uow.PrinterRepo.Delete(wI);
            return Json("");
        }
        
        public JsonResult PrinterUpdate(Printer item)
        {
            uow.PrinterRepo.AddOrUpdate(item);
            item = uow.PrinterRepo.GetById(item.Id);
            PrinterViewModel vm = new PrinterViewModel();
            ReflectionHelper.CopyProperties(item, vm);
            return Json(vm);
        }

    }
}