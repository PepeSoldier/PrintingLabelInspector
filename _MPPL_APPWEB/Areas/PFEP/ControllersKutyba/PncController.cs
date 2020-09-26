using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using _MPPL_WEB_START.Migrations;
using MDL_PFEP.Repo;
//using MDL_PFEP.Models.PFEP;
using MDL_PFEP.Models.DEF;
using Newtonsoft.Json;

using MDLX_BASE.Repo;
using MDL_BASE.Models.MasterData;
using MDLX_MASTERDATA.Entities;

namespace _MPPL_WEB_START.Areas.PFEP.Controllers
{
    public class PncController : Controller
    {
        private DbContextAPP_ElectroluxPLV db;
        private GenericRepository<Item> repository;
        //private GenericRepository<PncType> repositoryPncType;

        public PncController()
        {
            db = DbContextAPP_ElectroluxPLV.Create();
            repository = new GenericRepository<Item>(db);
            //repositoryPncType = new GenericRepository<PncType>(db);
        }

        // GET: Pnc
        public async Task<ActionResult> Index()
        {
            //var resultsType = await repositoryPncType.Get();
            //var resultsSetType = resultsType as IEnumerable<PncType>;
            //Dictionary<string, string> listType = resultsSetType.ToDictionary(x => x.Id.ToString(), x => x.Name);
            //ViewBag.ListType = JsonConvert.SerializeObject(listType);

            return View(await repository.Get());
        }

        // POST: Pnc/Edit
        [HttpPost]
        public async Task<ActionResult> Edit([Bind(Include = "Id,TypeId")] Item pnc)
        {
            Item currentPnc = await repository.GetByID(pnc.Id);

            if (currentPnc != null)
            {
                int success = 0;
                //currentPnc.TypeId = pnc.TypeId;
                success = await repository.Update(currentPnc);

                if (success == 1)
                {
                    return Json(new { result = "success", id = pnc.Id });
                }
            }
            return Json(new { result = "error" });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
