using XLIB_COMMON.Repo;
using MDL_PFEP.Interface;
using MDL_PFEP.Model.ELDISY_PFEP;
using MDL_PFEP.Models.DEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace MDL_PFEP.Repo.ELDISY_PFEP
{
    public class CalculationRepo : RepoGenericAbstract<Calculation>
    {
        protected new IDbContextPFEP_Eldisy db;
        PackingInstructionPackageRepo PackingInstructionPackageRepo;
        PackingInstructionRepo PackingInstructionRepo;

       
        public CalculationRepo(IDbContextPFEP_Eldisy db) : base(db)
        {
            this.db = db;
            this.PackingInstructionPackageRepo = new PackingInstructionPackageRepo(db);
            this.PackingInstructionRepo = new PackingInstructionRepo(db);
        }
        public override Calculation GetById(int id)
        {
            return db.Calculations.FirstOrDefault(x => x.Id == id);
        }

        public List<Calculation> GetCalculations(Calculation filter)
        {
            int startIndex = (filter.pageIndex - 1) * filter.pageSize;
            List<Calculation> calculationList = new List<Calculation>();
            Calculation calc = new Calculation();
            List<PackingInstruction> queryPackingInstruction = PackingInstructionRepo.GetList().Skip(startIndex).Take(filter.pageSize).ToList();
            List<PackingInstructionPackage> packingInstructionPackages;
            
            
            foreach (var packingInstruction in queryPackingInstruction)
            {
                packingInstructionPackages = PackingInstructionPackageRepo.GetPackagesForInstruction(packingInstruction.Id);
                calc = PrepareCalculation(packingInstruction, packingInstructionPackages);
                calculationList.Add(calc);
            }
            
            calculationList = calculationList
                .Where(x =>
                    (x.PackingInstructionId == filter.PackingInstructionId || filter.PackingInstructionId == 0) &&
                    (filter.ProfileName == null || x.PackingInstruction.ProfileName.Contains(filter.ProfileName)) &&
                    (filter.ClientProfileCode == null || x.PackingInstruction.ClientProfileCode.Contains(filter.ClientProfileCode)) &&
                    (filter.ProfileCode == null || x.PackingInstruction.ProfileCode.Contains(filter.ProfileCode)))
                .OrderByDescending(x => x.Id)
                .ToList();

            return calculationList;
        }

        public Calculation PrepareCalculation(PackingInstruction item, List<PackingInstructionPackage> packingInstructionPackages)
        {
            Calculation core = new Calculation();
            int MainAmount = 1;
            MainAmount = item.AmountOnPallet != 0 ? item.AmountOnPallet : item.AmountOnBox;
            core.PackingInstruction = item;
            core.SetInstructionPrice = item.CalculationPrice;
            core.PackingInstructionId = item.Id;
            core.ProfileCode = item.ProfileCode != null ? item.ProfileCode.ToString() : string.Empty;
            core.ProfileName = item.ProfileName != null ? item.ProfileName.ToString() : string.Empty;
            core.ClientProfileCode = item.ClientProfileCode != null ? item.ClientProfileCode.ToString() : string.Empty;
            core.CalculatedInstructionPrice = MainAmount == 0 ? 0 : Math.Round(packingInstructionPackages.Sum(x => x.GetValueOfPackage) / MainAmount, 3);
            core.PackingInstructionPrice = Math.Round(packingInstructionPackages.Sum(x => x.GetValueOfPackage), 3);
            return core;
        }

        public IQueryable<Calculation> GetByPackingInstructionId(int packingInstrucionID)
        {
            return db.Calculations.Where(x => x.PackingInstructionId == packingInstrucionID);
        }

        public override IQueryable<Calculation> GetList()
        {
            throw new NotImplementedException();
        }
    }
}