using XLIB_COMMON.Repo;
using MDL_ONEPROD.Common;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Interface;
using MDL_ONEPROD.ComponentQuality.Entities;
using MDL_ONEPROD.ComponentQuality._Interfaces;

namespace MDL_ONEPROD.ComponentQuality.Repos
{
    public class ItemParameterRepo : RepoGenericAbstract<ItemParameter>
    {
        protected new IDbContextOneprodQuality db;
        
        public ItemParameterRepo(IDbContextOneprodQuality db) : base(db)
        {
            this.db = db;
        }

        public override IQueryable<ItemParameter> GetList()
        {
            return db.ItemParameters.Where(x => x.Deleted == false).OrderBy(x => x.Id);
        }
    }
}