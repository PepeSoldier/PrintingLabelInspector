using MDL_BASE.Interfaces;
using MDL_BASE.Models.MasterData;
using MDL_ONEPROD.ComponentCORE.ViewModels;
using MDL_PFEP.ComponentLineFeed.Models;
using MDL_PFEP.Interface;
using MDL_PFEP.Models.DEF;
using MDLX_MASTERDATA.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MDL_PFEP.Repo.PFEP
{
    public class BomRepo : XLIB_COMMON.Repo.Base.BomRepo
    {
        protected new IDbContextCore db;

        public BomRepo(IDbContextCore db)
            : base(db)
        {
            this.db = db;
        }

        public List<PrintModel_ANC> GetAncsByPNCAndRoutine(Item pnc, Routine routine)
        {
            //routine.AddPrefixes = routine.AddPrefixes == null ? new string[0] { } : routine.AddPrefixes;

            List <Bom>  boms = db.Boms.AsNoTracking().Where(x => 
                                      x.Pnc.Id == pnc.Id &&
                                      routine.DEFs.Contains(x.DEF) &&
                                      (x.LV == 1 || x.LV == routine.AddLevel || (routine.AddPrefixes.Contains(x.Prefix)))  &&
                                      (!(routine.RemovePrefixes.Contains(x.Prefix)))
                                   ).ToList();

            List<PrintModel_ANC> ancs = new List<PrintModel_ANC>();

            foreach(Bom b in boms)
            {
                ancs.Add(new PrintModel_ANC(b.Anc, b.PCS));
            }

            return ancs;
        }

        //public IQueryable<Anc> GetANCsByPNCAndDEFs(Pnc pnc, string[] defs)
        //{
        //    IQueryable<Anc> ancs = db.Boms.AsNoTracking().Where(x => x.Pnc.Id == pnc.Id && defs.Contains(x.DEF)).Select(x => x.Anc);
        //    return ancs;
        //}
        //public IQueryable<Anc> GetANCsByPncIdsAndDEFs(int[] pncIds, string[] defs)
        //{
        //    IQueryable<Anc> ancs = db.Boms.AsNoTracking().Where(x => pncIds.Contains(x.Pnc.Id) && defs.Contains(x.DEF)).Select(x => x.Anc);
        //    return ancs;
        //}

        public List<JobListItem> GetItemsForPncAndWorkstation(Item pnc, List<int> WorkstationItemsIds)
        {
            List<JobListItem> items =
            db.Boms.Where(x => x.PncId == pnc.Id && WorkstationItemsIds.Contains(x.AncId ?? -1))
                .Select(i => new JobListItem()
                {
                    ItemId = i.Anc.Id,
                    ItemCode = i.Anc.Code,
                    ItemName = i.Anc.Name,
                    PhotoPosition = i.LV,
                    Qty = i.PCS,
                    Prefix = i.Prefix
                }).ToList();
            
            return items;
        }

        public List<JobListItem> GetItemsForWorkorderAndPrefixes(Item pnc, List<string> prefixes)
        {
            List<JobListItem> items =
            db.Boms.Where(x => x.PncId == pnc.Id && prefixes.Contains(x.Prefix))
                .Select(i => new JobListItem()
                {
                    ItemId = i.Anc.Id,
                    ItemCode = i.Anc.Code,
                    ItemName = i.Anc.Name,
                    PhotoPosition = i.LV,
                    Qty = i.PCS,
                    Prefix = i.Prefix
                }).Distinct().ToList();

            return items;
        }

        public IQueryable<Item> GetItemsForPncAndPrefix(Item pnc, string prefix)
        {
            IQueryable<Item> items =
            db.Boms.Where(x => x.PncId == pnc.Id && x.Prefix == prefix)
                .Select(i => i.Anc);

            return items;
        }

    }
}