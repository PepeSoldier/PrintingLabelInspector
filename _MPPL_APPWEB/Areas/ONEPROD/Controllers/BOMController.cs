using MDL_BASE.Interfaces;
using MDL_BASE.Models.IDENTITY;
using MDL_BASE.Models.MasterData;
using MDL_BASE.ViewModel;
using MDLX_CORE.ComponentCore.UnitOfWorks;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace _MPPL_WEB_START.Areas.ONEPROD.Controllers
{
    [Authorize(Roles = DefRoles.USER)]
    public partial class BOMController : BaseController
    {
        UnitOfWorkCore uow;

        public BOMController(IDbContextCore db)
        {
            this.uow = new UnitOfWorkCore(db);
            ViewBag.Skin = "nasaSkin";
        }


        public ActionResult BOM()
        {
            Bom vm = new Bom();
            return View();
        }
        [HttpPost, Authorize(Roles = DefRoles.ADMIN)]
        public ActionResult BOMDelete(int id)
        {
            Bom wI = uow.BomRepo.GetById(id);
            uow.BomRepo.Delete(wI);
            return Json("");
        }
        [HttpPost, Authorize(Roles = DefRoles.ADMIN)]
        public JsonResult BOMUpdate(BOMViewModel item)
        {
            Bom bomEntry = uow.BomRepo.GetById(item.Id);
            if (bomEntry == null) { bomEntry = new Bom(); }

            bomEntry.AncId = item.ChildId;
            bomEntry.PncId = item.ParentId;
            bomEntry.BC = item.BC;
            bomEntry.DEF = item.DEF;
            bomEntry.LV = item.LV;
            bomEntry.PCS = item.QtyUsed;
            bomEntry.IDCO = item.IDCO;
            bomEntry.Prefix = item.Prefix;
            bomEntry.StartDate = item.StartDate;
            bomEntry.EndDate = item.EndDate;

            uow.BomRepo.AddOrUpdate(bomEntry);
            return Json(item);
        }
        [HttpPost, AllowAnonymous]
        public JsonResult BOMGetList(BOMViewModel filter, int pageIndex = 1, int pageSize = 100)
        {
            IQueryable<Bom> BomList = uow.BomRepo.GetList(filter);
            int startIndex = (pageIndex - 1) * pageSize;
            int itemsCount = BomList.Count();
            List<BOMViewModel> items = BomList.Skip(startIndex).Take(pageSize).Select(x => new BOMViewModel()
            {
                ChildId = x.AncId,
                ChildCode = x.Anc.Code,
                ChildName = x.Anc.Name,
                ParentId = x.PncId,
                ParentCode = x.Pnc.Code,
                ParentName = x.Pnc.Name,
                BC = x.BC,
                DEF = x.DEF,
                LV = x.LV,
                QtyUsed = x.PCS,
                IDCO = x.IDCO,
                Prefix = x.Prefix,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
            }).ToList();

            return Json(new { data = items, itemsCount });
        }
    }
}