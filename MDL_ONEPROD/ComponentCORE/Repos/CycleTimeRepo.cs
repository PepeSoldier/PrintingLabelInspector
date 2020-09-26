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
    public class CycleTimeRepo : RepoGenericAbstract<MCycleTime>
    {
        protected new IDbContextOneprod db;
        private UnitOfWorkOneprod unitOfWork;

        public CycleTimeRepo(IDbContextOneprod db, IAlertManager alertManager, UnitOfWorkOneprod unitOfWork = null) : base(db)
        {
            this.db = db;
            this.unitOfWork = unitOfWork;
        }

        public override MCycleTime GetById(int id)
        {
            return db.CycleTimes.FirstOrDefault(x => x.Id == id);
        }
        public override IQueryable<MCycleTime> GetList()
        {
            return db.CycleTimes.OrderBy(x => x.Id);
        }
        public IQueryable<MCycleTime> GetList(int partCategoryId)
        {
            return db.CycleTimes.Where(x => x.ItemGroupId == partCategoryId).OrderBy(x => x.Id);
        }
        public IQueryable<MCycleTime> GetList(MCycleTime filter,int partCategoryId)
        {
            return db.CycleTimes.Where(x => (x.ItemGroupId == partCategoryId || partCategoryId == -1) &&
                                            (x.ItemGroupId == filter.ItemGroupId || filter.ItemGroupId == -1 || filter.ItemGroupId == 0) &&
                                            (x.MachineId == filter.MachineId || filter.MachineId == -1 || filter.MachineId == 0))
                                            .OrderBy(x => x.Machine.Name)
                                            .ThenBy(x => x.ItemGroup.Name);
        }

        //CycleTime
        //
        public MCycleTime GetCycleTime(int id)
        {
            return db.CycleTimes.FirstOrDefault(c => c.Id == id);
        }
        public MCycleTime GetCycleTime(int machineId, int partCategoryId)
        {
            return db.CycleTimes.FirstOrDefault(x => x.ItemGroupId == partCategoryId && x.MachineId == machineId);
        }
       
        public List<MCycleTime> GetCycleTimesByMachine(int machineId)
        {
            return db.CycleTimes.Where(c => c.MachineId == machineId).OrderBy(c => c.ItemGroup.Name).ToList();
        }
        public List<MCycleTime> GetCycleTimesByItemGroup(int partCategoryId)
        {
            return db.CycleTimes.Where(c => c.ItemGroupId == partCategoryId).OrderBy(c => c.Machine.Name).ToList();
        }
        public List<ItemOP> GetItemsForResource(int resourceId)
        {
            List<int?> itemGroupIds = db.CycleTimes.Where(x => x.MachineId == resourceId).Select(x => (int?)x.ItemGroupId).ToList();
            List<ItemOP> items = db.ItemsOP.Where(x => itemGroupIds.Contains(x.ItemGroupId)).ToList();
            return items;
        }
        public int GetCycleTimeCount(int partCategoryId)
        {
            //using (DbContextPreprod db2 = new DbContextPreprod(db))
            //{
                return db.CycleTimes.AsNoTracking().Where(c => c.ItemGroupId == partCategoryId).Count();
            //}
        }
        public int DeleteCycleTime(int cycleTimeId)
        {
            MCycleTime ct = db.CycleTimes.FirstOrDefault(p => p.Id == cycleTimeId);
            if (ct != null)
            {
                int partCategoryId = ct.ItemGroupId;
                Delete(ct);
                return partCategoryId;
            }
            return 0;
        }

        public int GetRealProcessingTime(int machineId, int partCategoryId, int qty)
        {
            MCycleTime ct = GetCycleTime(machineId, partCategoryId);
            decimal OEE = 0;
            decimal cycleTime = 0;

            if (ct != null)
            {
                cycleTime = ct.CycleTime;
                OEE = ct.Machine.TargetOee;

                if (OEE > 0)
                {
                    cycleTime = cycleTime / OEE;
                }

                return (int)(cycleTime * qty);
            }
            else
            {
                return 0;
            }
        }

        public int AddorUpdateItemGroupCycleTime(MCycleTime item)
        {
            MCycleTime mCycleTime = db.CycleTimes.Where(x => x.Id == item.Id).FirstOrDefault();

            if (mCycleTime == null)
            {
                mCycleTime = new MCycleTime { ItemGroupId = item.ItemGroupId, MachineId = item.MachineId, CycleTime = item.CycleTime};
            }
            else
            {
                mCycleTime.MachineId = item.MachineId;
                mCycleTime.CycleTime= item.CycleTime;
                mCycleTime.ProgramNumber = item.ProgramNumber;
                mCycleTime.PiecesPerPallet = item.PiecesPerPallet;
                mCycleTime.ItemGroup = null;
                mCycleTime.ItemGroupId = item.ItemGroupId;
            }
            return AddOrUpdate(mCycleTime);
        }
    }
}