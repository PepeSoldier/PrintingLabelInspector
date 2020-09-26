using XLIB_COMMON.Repo;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Model.Scheduling.Interface;
using MDL_ONEPROD.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Enums;

namespace MDL_ONEPROD.ComponentOEE.Repos
{
    public class OEEReportProductionDataDetailsRepo : RepoGenericAbstract<OEEReportProductionDataDetails>
    {
        protected new IDbContextOneProdOEE db;
        UnitOfWorkOneProdOEE unitOfWork;

        public OEEReportProductionDataDetailsRepo(IDbContextOneProdOEE db, XLIB_COMMON.Interface.IAlertManager alertManager, UnitOfWorkOneProdOEE unitOfWork = null)
           : base(db, alertManager)
        {
            this.db = db;
            this.unitOfWork = unitOfWork;
        }

        public override OEEReportProductionDataDetails GetById(int id)
        {
            return db.OEEReportProductionDataDetails.FirstOrDefault(x => x.Id == id);
        }

        public override IQueryable<OEEReportProductionDataDetails> GetList()
        {
            return db.OEEReportProductionDataDetails.Where(x => x.Deleted == false).OrderBy(x => x.Id);
        }
    }
   
}