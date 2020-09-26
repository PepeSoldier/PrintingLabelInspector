using MDL_BASE.Interfaces;
using XLIB_COMMON.Repo;
using MDL_PFEP.Interface;
using MDL_PFEP.Model.ELDISY_PFEP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_PFEP.Repo.ELDISY_PFEP
{
    public class CorrectionRepo : RepoGenericAbstract<Correction>
    {
        protected new IDbContextPFEP_Eldisy db;

        public CorrectionRepo(IDbContextPFEP_Eldisy db) : base(db)
        {
            this.db = db;
        }

        public override Correction GetById(int id)
        {
            return db.Corrections.FirstOrDefault(x => x.Id == id);
        }
        public override IQueryable<Correction> GetList()
        {
            return db.Corrections.Where(x => x.Deleted == false).OrderByDescending(x => x.Id);
        }

        public IQueryable<Correction> GetByPackingInstructionId(int packingInstrucionID)
        {
            return db.Corrections.Where(x => x.PackingInstructionId == packingInstrucionID && x.Deleted == false);
        }
        public bool HasInstructionCorrectionByInstructionId(int packingInstrucionID)
        {
            return db.Corrections.Where(x => x.PackingInstructionId == packingInstrucionID && x.CorrectionOpen == true && x.Deleted == false).Count() > 0 ? true : false;
        }
    }
}