using MDL_ONEPROD.Common;
using MDL_ONEPROD.ComponentQuality._Interfaces;
using MDL_ONEPROD.ComponentQuality.Repos;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Repo;
using MDL_ONEPROD.Repo.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Model;

namespace MDL_ONEPROD.ComponentQuality.UnitOfWorks
{
    public class UnitOfWorkOneProdQuality : UnitOfWorkOneprod
    {
        IDbContextOneprodQuality db;
        public UnitOfWorkOneProdQuality(IDbContextOneprodQuality dbContext) : base(dbContext)
        {
            db = dbContext;
        }
                
        private ItemMeasurementRepo itemMeasurementRepo;
        private ItemParameterRepo itemParameterRepo;
        

        public ItemMeasurementRepo ItemMeasurementRepo
        {
            get
            {
                itemMeasurementRepo = (itemMeasurementRepo != null) ? itemMeasurementRepo : new ItemMeasurementRepo(db);
                return itemMeasurementRepo;
            }
        }

        public ItemParameterRepo ItemParameterRepo
        {
            get
            {
                itemParameterRepo = (itemParameterRepo != null) ? itemParameterRepo : new ItemParameterRepo(db);
                return itemParameterRepo;
            }
        }

    }
}