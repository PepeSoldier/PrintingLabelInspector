using MDL_BASE.Interfaces;
using MDL_AP.Models;
using MDL_AP.Models.DEF;
using XLIB_COMMON.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Interface;
using MDL_AP.Interfaces;

namespace MDL_AP.Repo.DEF
{
    public class RepoCategory : RepoGenericAbstract<Category>
    {
        protected new IDbContextAP db;
        private UnitOfWorkActionPlan unitOfWork;

        public RepoCategory(IDbContextAP db, IAlertManager alertManager, UnitOfWorkActionPlan unitOfWork = null) : base(db, alertManager)
        {
            this.db = db;
            this.unitOfWork = unitOfWork;
        }

        public override Category GetById(int id)
        {
            return db.Categories.FirstOrDefault(d => d.Id == id);
        }

        public override IQueryable<Category> GetList()
        {
            return db.Categories.Where(x => x.Deleted == false).OrderBy(x => x.Name);
        }
    }
}