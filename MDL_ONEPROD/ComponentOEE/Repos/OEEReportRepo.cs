using XLIB_COMMON.Repo;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Enums;

namespace MDL_ONEPROD.ComponentOEE.Repos
{
    public class OEEReportRepo : RepoGenericAbstract<OEEReport>
    {
        protected new IDbContextOneProdOEE db;
        UnitOfWorkOneProdOEE unitOfWork;

        public OEEReportRepo(IDbContextOneProdOEE db, XLIB_COMMON.Interface.IAlertManager alertManager, UnitOfWorkOneProdOEE unitOfWork = null)
           : base(db, alertManager)
        {
            this.db = db;
            this.unitOfWork = unitOfWork;
        }

        public override OEEReport GetById(int id)
        {
            return db.OEEReports.FirstOrDefault(x => x.Id == id && x.Deleted == false);
        }
        public override IQueryable<OEEReport> GetList()
        {
            return db.OEEReports.Where(x => x.Deleted == false).OrderByDescending(x => x.Id);
        }
        public OEEReport GetByMachineIdDateAndShift(int machineId, DateTime date, Shift shift)
        {
            return db.OEEReports.FirstOrDefault(x => 
                x.MachineId == machineId && 
                x.ReportDate == date && 
                x.Shift == shift && 
                //x.IsDraft == false &&
                x.Deleted == false
            );
        }
        public OEEReport GetDraftByUser(string userId)
        {
            return db.OEEReports.FirstOrDefault(x => x.UserId == userId && x.IsDraft == true && x.Deleted == false);
        }
    }
}