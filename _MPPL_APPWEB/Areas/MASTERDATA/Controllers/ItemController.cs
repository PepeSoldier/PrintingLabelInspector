using _MPPL_WEB_START.Areas._APPWEB.Models;
using _MPPL_WEB_START.Areas.MASTERDATA.ViewModels;
using MDL_ONEPROD.Repo;
using MDLX_MASTERDATA._Interfaces;
using MDLX_MASTERDATA.Entities;
using MDLX_MASTERDATA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XLIB_COMMON.Enums;

namespace _MPPL_WEB_START.Areas.MASTERDATA.Controllers
{
    public class ItemController : Controller
    {
        private readonly IDbContextMasterData db;
        UnitOfWorkMasterData uow;

        public ItemController(IDbContextMasterData db)
        {
            this.db = db;
            uow = new UnitOfWorkMasterData(db);
        }

        public ActionResult Item()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ItemDelete(Item items)
        {
            uow.ItemRepo.Delete(uow.ItemRepo.GetById(items.Id));
            return RedirectToAction("Item");
        }
        [HttpPost]
        public JsonResult ItemUpdate(Item items)
        {
            uow.ItemRepo.AddOrUpdate(items);
            return Json(items);
        }
        [HttpPost]
        public JsonResult ItemGetList(Item filter, int pageIndex, int pageSize)
        {
            int startIndex = (pageIndex - 1) * pageSize;
            var list = uow.ItemRepo.GetList(filter);
            int itemsCount = list.Count();
            List<ItemGridViewModel> items = list.Skip(startIndex).Take(pageSize).Select(x => 
                new ItemGridViewModel() {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name
                })
                .ToList();
            return Json(new { data = items, itemsCount });
        }

        [HttpPost]
        public JsonResult Autocomplete(string prefix)
        {
            uow = new UnitOfWorkMasterData(db);
            List<AutocompleteViewModel> acData = uow.ItemRepo.GetList()
                .Where(x => x.Code.StartsWith(prefix))
                .OrderBy(x => x.Code)
                .Take(10)
                .Select(x => new AutocompleteViewModel { TextField = x.Code, ValueField = x.Id.ToString(), Data1 = x.Name, Data6 = (int)x.UnitOfMeasure })
                .ToList();

            if(acData.Count <= 0)
            {
                acData = uow.ItemRepo.GetList()
                .Where(x => x.Name.Contains(prefix))
                .OrderBy(x => x.Code)
                .Take(10)
                .Select(x => new AutocompleteViewModel { TextField = x.Code, ValueField = x.Id.ToString(), Data1 = x.Name, Data6 = (int)x.UnitOfMeasure })
                .ToList();
            }

            return Json(acData);
        }

        [HttpPost]
        public JsonResult ItemAlternativeUnitOfMeasures(string itemCode)
        {
            Item item = uow.ItemRepo.GetByCode(itemCode);
            
            List<UnitOfMeasure> unitOfMeasures = ConverterUoM.AlternativeUnitOfMeasures(item.UnitOfMeasure);
            unitOfMeasures.AddRange(item.UnitOfMeasures.Select(x => x.AlternativeUnitOfMeasure));

            return Json(unitOfMeasures.Distinct());
        }

    }
}