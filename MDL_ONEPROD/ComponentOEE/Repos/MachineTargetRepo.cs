using XLIB_COMMON.Repo;
using MDL_ONEPROD.Common;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Model.Scheduling.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Interface;

namespace MDL_ONEPROD.Repo.OEERepos
{
    public class MachineTargetRepo : RepoGenericAbstract<MachineTarget>
    {
        protected new IDbContextOneProdOEE db;
        
        public MachineTargetRepo(IDbContextOneProdOEE db)
            : base(db)
        {
            this.db = db;
        }

        public override MachineTarget GetById(int id)
        {
            return db.MachineTargets.FirstOrDefault(x => x.Id == id );
        }
        public override IQueryable<MachineTarget> GetList()
        {
            return db.MachineTargets.Where(x => x.Deleted == false).OrderBy(x => x.Id);
        }
        public MachineTarget GetByResourceIdAndReasonTypeId(int resourceId, int reasonTypeId)
        {
            return db.MachineTargets.FirstOrDefault(x => x.ResourceId == resourceId && x.ReasonTypeId == reasonTypeId);
        }
        public decimal GetTargetByResourceAndReasonType(int resourceId, int reasonTypeId)
        {
            var mt = db.MachineTargets.Where(x => x.ResourceId == resourceId && x.ReasonTypeId == reasonTypeId && x.Deleted == false).FirstOrDefault();
            return mt != null ? mt.Target : 0;
        }        
    }
}