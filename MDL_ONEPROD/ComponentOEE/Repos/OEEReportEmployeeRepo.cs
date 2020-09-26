using XLIB_COMMON.Repo;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.ComponentOEE.Repos
{
    public class OEEReportEmployeeRepo : RepoGenericAbstract<OEEReportEmployee>
    {
        protected new IDbContextOneProdOEE db;
        UnitOfWorkOneProdOEE unitOfWork;

        public OEEReportEmployeeRepo(IDbContextOneProdOEE db, XLIB_COMMON.Interface.IAlertManager alertManager, UnitOfWorkOneProdOEE unitOfWork = null)
           : base(db, alertManager)
        {
            this.db = db;
            this.unitOfWork = unitOfWork;
        }

        public override OEEReportEmployee GetById(int id)
        {
            return db.OEEReportEmployees.FirstOrDefault(x => x.Id == id);
        }

        public override IQueryable<OEEReportEmployee> GetList()
        {
            return db.OEEReportEmployees.Where(x => x.Deleted == false).OrderByDescending(x => x.Id);
        }

        public IQueryable<OEEReportEmployee> GetListByReportId(int reportId)
        {
            return db.OEEReportEmployees.Where(x => x.Deleted == false && x.ReportId == reportId).OrderByDescending(x => x.Id);
        }
    }
   
}