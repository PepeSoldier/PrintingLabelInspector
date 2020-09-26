using XLIB_COMMON.Repo;
using MDL_ONEPROD.Common;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.OEEProd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Interface;
using MDL_ONEPROD.Model.Scheduling.Interface;
using MDL_ONEPROD.ComponentRTV._Interfaces;
using System.Data.Entity;

namespace MDL_ONEPROD.Repo.OEERepos
{
    public class ReasonTypeRepo : RepoGenericAbstract<ReasonType>
    {
        protected new IDbContextOneProdOEE db;
        protected IDbContextOneProdRTV dbRTV;

        private DbSet<ReasonType> reasonTypes { get { return db != null ? db.ReasonTypes : dbRTV.ReasonTypes; } }

        public ReasonTypeRepo(IDbContextOneProdOEE db)
            : base(db)
        {
            this.db = db;
        }

        public ReasonTypeRepo(IDbContextOneProdRTV db)
            : base(db)
        {
            this.dbRTV = db;
        }

        public override ReasonType GetById(int id)
        {
            return reasonTypes.FirstOrDefault(x => x.Id == id);
        }

        public override IQueryable<ReasonType> GetList()
        {
            return reasonTypes.Where(x => x.Deleted == false).OrderBy(x => x.SortOrder).ThenBy(x=>x.Name);
        } 
        
        public IQueryable<ReasonType> GetListProduction()
        {
            return reasonTypes.Where(x => 
                    x.Deleted == false &&
                    x.EntryType >= EnumEntryType.Production && x.EntryType < EnumEntryType.StopPlanned)
                .OrderBy(x => x.SortOrder).ThenBy(x=>x.Name);
        }
        public IQueryable<ReasonType> GetListStoppages()
        {
            return reasonTypes.Where(x => 
                    x.Deleted == false &&
                    x.EntryType >= EnumEntryType.StopPlanned)
                .OrderBy(x => x.SortOrder).ThenBy(x=>x.Name);
        }

        public ReasonType GetByEntryType(EnumEntryType entryType)
        {
            return reasonTypes.FirstOrDefault(x => x.EntryType == entryType);
        }
        public int? GetIdByEntryType(EnumEntryType entryType)
        {
            var reasonType =  reasonTypes.FirstOrDefault(x => x.EntryType == entryType);
            return reasonType != null ? (int?)reasonType.Id : null;
        }
    }
}