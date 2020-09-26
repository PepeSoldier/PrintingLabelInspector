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
    //public class MachineElementRepo : RepoGenericAbstract<MachineElement>
    //{
    //    IDbContextOneProdOEE db;
    //    UnitOfWorkOneProdOEE unitOfWork;

    //    public MachineElementRepo(IDbContextOneProdOEE db, IAlertManager alertManager, UnitOfWorkOneProdOEE unitOfWork = null)
    //        : base(db, alertManager)
    //    {
    //        this.db = db;
    //        this.unitOfWork = unitOfWork;
    //    }

    //    public override MachineElement GetById(int id)
    //    {
    //        return db.MachineElements.FirstOrDefault(x => x.Id == id );
    //    }

    //    public IQueryable<MachineElement> GetByElementId(int elementId)
    //    {
    //        return db.MachineElements.Where(x => x.ElementId == elementId  && x.Deleted == false);
    //    }

    //    public IQueryable<MachineElement> GetByMachineId(int machineId)
    //    {
    //        return db.MachineElements.Where(x => x.MachineId == machineId && x.Deleted == false);
    //    }

    //    public override IQueryable<MachineElement> GetList()
    //    {
    //        return db.MachineElements.Where(x => x.Deleted == false).OrderByDescending(x => x.Id);
    //    }
    //}
}