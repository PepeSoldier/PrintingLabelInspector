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
    public class MachineReasonRepo : RepoGenericAbstract<MachineReason>
    {
        protected new IDbContextOneProdOEE db;
        UnitOfWorkOneProdOEE unitOfWork;

        public MachineReasonRepo(IDbContextOneProdOEE db, IAlertManager alertManager, UnitOfWorkOneProdOEE unitOfWork = null)
            : base(db, alertManager)
        {
            this.db = db;
            this.unitOfWork = unitOfWork;
        }

        public override MachineReason GetById(int id)
        {
            return db.MachineReasons.FirstOrDefault(x => x.Id == id );
        }
        public override IQueryable<MachineReason> GetList()
        {
            return db.MachineReasons.Where(x => x.Deleted == false).OrderByDescending(x => x.Id);
        }

        public IQueryable<MachineReason> GetByReasonId(int ReasonId)
        {
            return db.MachineReasons.Where(x => x.ReasonId == ReasonId  && x.Deleted == false);
        }
        public IQueryable<MachineReason> GetByMachineId(int machineId)
        {
            return db.MachineReasons.Where(x => x.MachineId == machineId && x.Deleted == false);
        }
        public MachineReason GetByReasonIdAndMachineId(int reasonId, int machineId)
        {
            return db.MachineReasons.FirstOrDefault(x => x.ReasonId == reasonId && x.MachineId == machineId && x.Deleted == false);
        }
        public bool DeleteByReasonIdAndMachineId(int reasonId, int machineId)
        {
            List<MachineReason> list = db.MachineReasons.Where(x => x.ReasonId == reasonId && x.MachineId == machineId).ToList();
            int count = 0;

            foreach(MachineReason mr in list)
            {
                Delete(mr);
                count++;
            }
            return count > 0;
        }
        public IQueryable<MachineReason> GetListByEntryTypeAndMachineId(int? reasonTypeId, int machineId, int[] machineIds = null)
        {
            bool machineIdsEmpty = machineIds == null;
            if (machineIdsEmpty)
            {
                machineIds = new int[] { };
            }

            var query = db.MachineReasons.Where(x => 
                        (x.Deleted == false) &&
                        (reasonTypeId == null || x.Reason.ReasonTypeId == reasonTypeId) &&
                        (machineIdsEmpty == true || machineIds.Contains(x.MachineId)) &&
                        (machineId <= 0 || x.MachineId == machineId))
                    .OrderByDescending(x => x.Id);

            return query;
        }
    }
}