using XLIB_COMMON.Repo;
using MDL_ONEPROD.Common;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Interface;

namespace MDL_ONEPROD.Repo.Scheduling
{
    public class ChangeOverRepo : RepoGenericAbstract<ChangeOver>
    {
        protected new IDbContextOneprodAPS db;
        private UnitOfWorkOneprodAPS unitOfWork;

        public ChangeOverRepo(IDbContextOneprodAPS db, IAlertManager alertManager, UnitOfWorkOneprodAPS unitOfWork = null) : base(db)
        {
            this.db = db;
            this.unitOfWork = unitOfWork;
        }

        public override ChangeOver GetById(int id)
        {
            return db.ChangeOvers.FirstOrDefault(x => x.Id == id);
        }
        public override IQueryable<ChangeOver> GetList()
        {
            return db.ChangeOvers.OrderBy(x => x.Id);
        }

    }
}