using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using _MPPL_WEB_START.Migrations;
using MDL_PFEP.Repo;
//using MDL_PFEP.Models.PFEP;
using MDL_PFEP.Models.DEF;

using MDLX_BASE.Repo;
using MDL_BASE.Models.MasterData;
using MDLX_MASTERDATA.Entities;

namespace _MPPL_WEB_START.Areas.PFEP.Controllers
{
    public class AncController : Controller
    {
        private DbContextAPP_ElectroluxPLV db;
        private GenericRepository<Item> repository;
        //private GenericRepository<AncType> repositoryAncType;

        public AncController()
        {
            db = DbContextAPP_ElectroluxPLV.Create();
            repository = new GenericRepository<Item>(db);
            //repositoryAncType = new GenericRepository<AncType>(db);
        }

        // GET: Anc
        public async Task<ActionResult> Index()
        {
            //var resultsType = await repositoryAncType.Get();
            //var resultsSetType = resultsType as IEnumerable<AncType>;
            //Dictionary<string, string> listType = resultsSetType.ToDictionary(x => x.Id.ToString(), x => x.Name);
            //ViewBag.ListType = JsonConvert.SerializeObject(listType);

            return View(await repository.Get());
        }

        // POST: Anc/Edit
        [HttpPost]
        public async Task<ActionResult> Edit([Bind(Include = "Id,TypeId,New")] Item anc)
        {
            Item currentAnc = await repository.GetByID(anc.Id);

            if (currentAnc != null)
            {
                int success = 0;
                //currentAnc.TypeId = anc.TypeId; TODO: TypeId ogarnąć
                success = await repository.Update(currentAnc);

                if (success == 1)
                {
                    return Json(new { result = "success", id = anc.Id });
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
