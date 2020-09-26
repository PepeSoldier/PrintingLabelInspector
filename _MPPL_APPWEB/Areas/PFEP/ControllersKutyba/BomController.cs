using System.Threading.Tasks;
using System.Web.Mvc;
using _MPPL_WEB_START.Migrations;
using MDL_BASE.Models.MasterData;
using MDLX_BASE.Repo;

namespace _MPPL_WEB_START.Areas.PFEP.Controllers
{
    public class BomController : Controller
    {
        private DbContextAPP_ElectroluxPLV db;
        private GenericRepository<Bom> repository;

        public BomController()
        {
            db = DbContextAPP_ElectroluxPLV.Create();
            repository = new GenericRepository<Bom>(db);
        }

        // GET: Bom
        public async Task<ActionResult> Index()
        {
            return View(await repository.Get());
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
