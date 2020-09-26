using MDL_BASE.Interfaces;
using MDL_BASE.Models.MasterData;
using MDL_BASE.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace XLIB_COMMON.Repo.Base
{
    public class BomRepo : RepoGenericAbstract<Bom>
    {
        protected new IDbContextCore db;

        public BomRepo(IDbContextCore db)
            : base(db)
        {
            this.db = db;
        }

        public override Bom GetById(int id)
        {
            return db.Boms.FirstOrDefault(d => d.Id == id);
        }
        public override IQueryable<Bom> GetList()
        {
            return db.Boms.OrderByDescending(x => x.Id);
        }

        public IQueryable<Bom> GetList(BOMViewModel filter)
        {
            DateTime defaultDate = new DateTime();
            return db.Boms
                .Where(x=>
                    (filter.ChildCode == null || x.Anc.Code.Contains(filter.ChildCode)) &&
                    (filter.ParentCode == null || x.Pnc.Code.Contains(filter.ParentCode)) &&
                    (filter.LV <= 0 || x.LV == filter.LV) &&
                    (filter.IDCO == null || x.IDCO == filter.IDCO) &&
                    (filter.BC == null || x.BC == filter.BC) &&
                    (filter.DEF == null || x.DEF == filter.DEF) &&
                    (filter.Prefix == null || x.Prefix == filter.Prefix) &&
                    (filter.StartDate == defaultDate || x.StartDate >= filter.StartDate) &&
                    (filter.EndDate == defaultDate || x.EndDate <= filter.EndDate)
                )
                .OrderBy(x => x.Pnc.Code)
                .ThenBy(x => x.Id);
        }

        public List<Bom> GetItemsForPNCAndTransporter(string pnc, List<int> pickerItemIds)
        {
            return db.Boms.Where(x => x.Pnc.Code == pnc && (x.AncId != null && pickerItemIds.Contains((int)x.AncId))).ToList();
        }

        public Bom GetByChildItemId(int itemId)
        {
            return db.Boms.Where(x => x.AncId == itemId).FirstOrDefault();
        }

        public Bom GetNextItemOnSameLevel(int lV, int id)
        {
            return db.Boms.Where(x => x.Id > id && x.LV == lV).FirstOrDefault();
        }

        public IQueryable<Bom> GetChildsOfItem(int parentItemId)
        {
            Bom parentItem = GetByChildItemId(parentItemId);

            if (parentItem != null)
            {
                Bom nextItemOnSameLevel = GetNextItemOnSameLevel(parentItem.LV, parentItem.Id);

                if (nextItemOnSameLevel != null)
                {
                    return db.Boms.Where(x =>
                        x.LV == (parentItem.LV + 1) &&
                        x.Id > parentItem.Id && x.Id < nextItemOnSameLevel.Id);
                    //&& x.BC != "-1");
                }
            }
            
            return db.Boms.Where(x => x.Id < 0);
            
        }
    }
}