using XLIB_COMMON.Repo;
using MDL_PFEP.Interface;
using MDL_PFEP.Model.ELDISY_PFEP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_PFEP.Repo.ELDISY_PFEP
{
    public class PackingInstructionPackageRepo : RepoGenericAbstract<PackingInstructionPackage>
    {
        protected new IDbContextPFEP_Eldisy db;

        public PackingInstructionPackageRepo(IDbContextPFEP_Eldisy db) : base(db)
        {
            this.db = db;
        }
        public override PackingInstructionPackage GetById(int id)
        {
            return db.PackingInstructionPackages.FirstOrDefault(d => d.Id == id);
        }

        public override IQueryable<PackingInstructionPackage> GetList()
        {
            return db.PackingInstructionPackages.OrderByDescending(x => x.Id);
        }

        public List<PackingInstructionPackage> GetPackagesForInstruction(int packageInstructionId)
        {
            return db.PackingInstructionPackages.Where(x => x.PackingInstructionId == packageInstructionId).ToList();
        }

        public List<PackingInstructionPackage> CalculateForOneHundred(List<PackingInstructionPackage> Packages, PackingInstruction pi)
        {
            int mainAmount = 1;
            decimal valueCount = 0;
            decimal valueTemp = 0;
            foreach (var item in Packages)
            {
                mainAmount = pi.AmountOnPallet != 0 ? pi.AmountOnPallet : pi.AmountOnBox;
                valueTemp = (decimal)100 / mainAmount;
                valueCount = (decimal)(valueTemp * item.Amount);
                item.PriceForHundredPackages = Math.Round(valueCount, 2).ToString();
            }
            return Packages;
        }

        public PackingInstructionPackage CalculateForOneHundred(PackingInstructionPackage item, PackingInstruction pi)
        {
            int mainAmount = 1;
            decimal valueCount = 0;
            decimal valueTemp = 0;
            mainAmount = pi.AmountOnPallet != 0 ? pi.AmountOnPallet : pi.AmountOnBox;
            valueTemp = (decimal)100 / mainAmount;
            valueCount = (decimal)(valueTemp * item.Amount);
            item.PriceForHundredPackages = Math.Round(valueCount, 2).ToString();

            return item;
        }

    }
}