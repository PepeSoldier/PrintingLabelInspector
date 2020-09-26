using XLIB_COMMON.Repo;
using MDL_PFEP.Interface;
using MDL_PFEP.Models.PFEP;
using System.Collections.Generic;
using System.Linq;

//namespace MDL_PFEP.Repo.PFEP
//{
//    public class WorkstationItemRepo : RepoGenericAbstract<WorkstationItem>
//    {
//        protected new IDbContextPFEP db;

//        public WorkstationItemRepo(IDbContextPFEP db)
//            : base(db)
//        {
//            this.db = db;
//        }

//        public override WorkstationItem GetById(int id)
//        {
//            return db.WorkstationItems.FirstOrDefault(d => d.Id == id);
//        }
//        public override IQueryable<WorkstationItem> GetList()
//        {
//            return db.WorkstationItems.OrderByDescending(x => x.Id);
//        }
//        public List<int> GetItemIdsByWorkstationId(int workstationId)
//        {
//            return db.WorkstationItems
//                .Where(x => x.WorkstationId == workstationId)
//                .Select(w => w.ItemId)
//                .ToList();
//        }
//        public List<int> GetItemIdsByWorkstationIds(int[] workstationIds)
//        {
//            return db.WorkstationItems
//                .Where(x => workstationIds.Contains(x.WorkstationId))
//                .Select(w => w.ItemId)
//                .ToList();
//        }

//        public WorkstationItem GetByAncId(int ancId)
//        {
//            return db.WorkstationItems.Where(d => d.ItemId == ancId).OrderBy(o=>o.Workstation.SortOrder).FirstOrDefault();
//        }
//        public WorkstationItem GetByAncIdAndLineId(int ancId, int lineId)
//        {
//            List<WorkstationItem> list = db.WorkstationItems.Where(d => d.ItemId == ancId && d.Workstation.LineId == lineId).OrderBy(o => o.Workstation.SortOrder).ToList();

//            if(list.Count > 1)
//            {
//                list.RemoveAll(x => x.Workstation.Name == "PKF");
//            }

//            return list.FirstOrDefault();
//        }
//    }
//}