using MDL_BASE.Interfaces;
using MDL_BASE.Models.MasterData;
using MDLX_CORE.ComponentCore.Entities;
using MDL_PFEP.ComponentLineFeed.Models;
using MDL_PFEP.Interface;
using MDL_PFEP.Models.DEF;
using MDL_PRD.Model;
using MDLX_MASTERDATA.Entities;
using System.Collections.Generic;
using System.Linq;
using MDL_ONEPROD.ComponentCORE.ViewModels;

namespace MDL_PFEP.Repo.PFEP
{
    public class BomWorkorderRepo : XLIB_COMMON.Repo.Base.BomWorkorderRepo
    {
        protected new IDbContextCore db;

        public BomWorkorderRepo(IDbContextCore db)
            : base(db)
        {
            this.db = db;
        }

        public List<PrintModel_ANC> GetAncsByPNCAndRoutine(ProductionOrder po, Routine routine)
        {
            routine.AddPrefixes = routine.AddPrefixes == null ? new string[0] { } : routine.AddPrefixes;

            List<BomWorkorder> BomWorkorders = db.BomWorkorders.AsNoTracking().Where(x => 
                                      x.OrderNo == po.OrderNumber &&
                                      routine.DEFs.Contains(x.DEF) &&
                                      (x.LV > 0 || (routine.AddPrefixes.Contains(x.Prefix)))
                                   ).ToList();

            //if (BomWorkorders == null || BomWorkorders.Count < 1)
            //{
            //    BomWorkorders = db.BomWorkorders.AsNoTracking().Where(x =>
            //                          (x.OrderNo != null && x.Parent.Id == po.Pnc.Id) &&
            //                          routine.DEFs.Contains(x.DEF) &&
            //                          (x.LV == 1 || x.LV == routine.AddLevel || (routine.AddPrefixes.Contains(x.Prefix)))
            //                       ).ToList();
            //}

            if (BomWorkorders == null || BomWorkorders.Count < 1)
            {
                BomWorkorders = db.BomWorkorders.AsNoTracking().Where(x =>
                                      (x.OrderNo == null && x.Parent.Id == po.Pnc.Id) &&
                                      routine.DEFs.Contains(x.DEF) &&
                                      (x.LV == 1 || x.LV == routine.AddLevel || (routine.AddPrefixes.Contains(x.Prefix)))
                                   ).ToList();
            }

            List<PrintModel_ANC> ancs = new List<PrintModel_ANC>();

            foreach (BomWorkorder b in BomWorkorders)
            {
                ancs.Add(new PrintModel_ANC(b.Child, b.QtyUsed));
            }

            return ancs;
        }

        public List<JobListItem> GetItemsForWorkorderAndWorkstation(ProductionOrder po, List<int> WorkstationItemsIds)
        {
            List<JobListItem> items =
            db.BomWorkorders.Where(x => x.OrderNo == po.OrderNumber && WorkstationItemsIds.Contains(x.ChildId))
                .Select(i => new JobListItem() {
                    ItemId = i.Child.Id,
                    ItemCode = i.Child.Code,
                    ItemName = i.Child.Name,
                    PhotoPosition = i.LV,
                    ParentItemId = i.ParentId,
                    Qty = i.QtyUsed,
                    Prefix = i.Prefix,
                }).Distinct().ToList();

            return items;
        }
        public List<JobListItem> GetItemsForWorkorderAndPrefixes(ProductionOrder po, List<string> prefixes)
        {
            List<JobListItem> items =
            db.BomWorkorders.Where(x => x.OrderNo == po.OrderNumber && prefixes.Contains(x.Prefix))
                .Select(i => new JobListItem()
                {
                    ItemId = i.Child.Id,
                    ItemCode = i.Child.Code,
                    ItemName = i.Child.Name,
                    PhotoPosition = i.LV,
                    ParentItemId = i.ParentId,
                    Qty = i.QtyUsed,
                    Prefix = i.Prefix,
                }).Distinct().ToList();

            return items;
        }


    }
}