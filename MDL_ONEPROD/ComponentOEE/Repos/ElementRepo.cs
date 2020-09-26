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
    //public class ElementRepo : RepoGenericAbstract<Element>
    //{
    //    IDbContextOneProdOEE db;
    //    UnitOfWorkOneProdOEE unitOfWork;

    //    public ElementRepo(IDbContextOneProdOEE db, IAlertManager alertManager, UnitOfWorkOneProdOEE unitOfWork = null)
    //        : base(db, alertManager)
    //    {
    //        this.db = db;
    //        this.unitOfWork = unitOfWork;
    //    }

    //    public override Element GetById(int id)
    //    {
    //        return db.Elements.FirstOrDefault(x => x.Id == id);
    //    }

    //    public override IQueryable<Element> GetList()
    //    {
    //        return db.Elements.Where(x => x.Deleted == false).OrderByDescending(x => x.Id);
    //    }
    //}
}