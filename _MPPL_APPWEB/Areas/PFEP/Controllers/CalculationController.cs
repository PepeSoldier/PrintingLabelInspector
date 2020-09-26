using MDL_BASE.Models.IDENTITY;

using MDL_PFEP.Interface;
using MDL_PFEP.Model.ELDISY_PFEP;
using MDL_PFEP.Repo.DEF;
using MDL_PFEP.Repo.ELDISY_PFEP;
using MDLX_MASTERDATA.Repos;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.PFEP.Controllers
{
    public class CalculationController : Controller
    {
        private IDbContextPFEP_Eldisy dab;
        private PackingInstructionPackageRepo packingInstructionPackageRepo;
        private PackingInstructionRepo packingInstructionRepo;
        private PackageRepo packageRepo;
        private CalculationRepo CalculationRepo;
        private RepoArea repoArea;
        

        public CalculationController(IDbContextPFEP_Eldisy db)
        {
            this.dab = db;
            packingInstructionPackageRepo = new PackingInstructionPackageRepo(db);
            packingInstructionRepo = new PackingInstructionRepo(db);
            packageRepo = new PackageRepo(db);
            repoArea = new RepoArea(db);
            CalculationRepo = new CalculationRepo(db);
        }

        [HttpGet]
        public ActionResult Index()
        {
            
            return View();
        }
        
        [HttpPost] //,AuthorizeRoles(DefRoles.CalculationManager)]
        public JsonResult AddOrUpdateCalculation(Calculation vm)
        {
            PackingInstruction temp = packingInstructionRepo.GetById(vm.PackingInstructionId);
            temp.CalculationPrice = vm.SetInstructionPrice;
            packingInstructionRepo.AddOrUpdate(temp);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCalculations(Calculation filter)
        {
            List<Calculation> calcList = CalculationRepo.GetCalculations(filter);
            int NumberOfPackingInstructions = packingInstructionRepo.GetList().Count();
            return Json(new { data = calcList, itemsCount = NumberOfPackingInstructions }, JsonRequestBehavior.AllowGet);
        }



    }
}