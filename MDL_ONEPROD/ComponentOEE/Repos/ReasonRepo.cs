using XLIB_COMMON.Repo;
using MDL_ONEPROD.Common;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.OEEProd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Interface;

namespace MDL_ONEPROD.Repo.OEERepos
{
    public class ReasonRepo : RepoGenericAbstract<Reason>
    {
        protected new IDbContextOneProdOEE db;
        private UnitOfWorkOneProdOEE unitOfWork;

        public ReasonRepo(IDbContextOneProdOEE db, XLIB_COMMON.Interface.IAlertManager alertManager, UnitOfWorkOneProdOEE unitOfWork = null)
            : base(db, alertManager)
        {
            this.db = db;
            this.unitOfWork = unitOfWork;
        }

        public override Reason GetById(int id)
        {
            return db.Reasons.FirstOrDefault(x => x.Id == id);
        }

        public override IQueryable<Reason> GetList()
        {
            return db.Reasons.Where(x => x.Deleted == false).OrderBy(x => x.Name);
        }
    }
}