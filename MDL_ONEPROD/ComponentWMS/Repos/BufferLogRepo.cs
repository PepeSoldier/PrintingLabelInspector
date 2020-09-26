using XLIB_COMMON.Repo;
using MDL_ONEPROD.Common;
using MDL_ONEPROD.ComponentWMS._Interfaces;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Interface;

namespace MDL_ONEPROD.Repo.Scheduling
{
    public class BufforLogRepo : RepoGenericAbstract<BufforLog>
    {
        protected new IDbContextOneprodWMS db;
        private UnitOfWorkOneprod unitOfWork;

        public BufforLogRepo(IDbContextOneprodWMS db, IAlertManager alertManager, UnitOfWorkOneprod unitOfWork = null) : base(db)
        {
            this.db = db;
            this.unitOfWork = unitOfWork;
        }

        public override BufforLog GetById(int id)
        {
            return db.BufforLog.FirstOrDefault(x => x.Id == id);
        }
        public override IQueryable<BufforLog> GetList()
        {
            return db.BufforLog.OrderBy(x => x.Id);
        }
        public IQueryable<BufforLog> GetListAsNoTracking()
        {
            return db.BufforLog.AsNoTracking().OrderBy(x => x.Id);
        }
        public void Add(BufforLog bufferLog)
        {
            db.BufforLog.Add(bufferLog);
            db.SaveChanges();
        }
    }
}