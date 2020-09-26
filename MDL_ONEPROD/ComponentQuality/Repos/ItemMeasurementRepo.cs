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
using MDL_ONEPROD.ComponentQuality.ViewModels;

namespace MDL_ONEPROD.ComponentQuality.Repos
{
    public class ItemMeasurementRepo : RepoGenericAbstract<ItemMeasurement>
    {
        protected new IDbContextOneprodQuality db;
        
        public ItemMeasurementRepo(IDbContextOneprodQuality db) : base(db)
        {
            this.db = db;
        }

        public IQueryable<ItemMeasurement> GetList(ItemMeasurementViewModel filter)
        {
            return db.ItemMeasurements.Where(x => 
                (x.Deleted == false) &&
                (filter.ItemCode == "" || x.ItemOP.Code == filter.ItemCode) &&
                (filter.SerialNumber == "" || x.SerialNumber == filter.SerialNumber) &&
                (filter.Counter == 0 || x.Counter == filter.Counter)
            ).OrderBy(x => x.Id);
        }
    }
}