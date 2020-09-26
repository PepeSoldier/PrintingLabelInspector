using XLIB_COMMON.Repo;
using MDL_ONEPROD.ComponentRTV._Interfaces;
using MDL_ONEPROD.ComponentRTV.Entities;
using MDL_ONEPROD.Model.OEEProd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.ComponentRTV.Repos
{
    public class RTVOEEProductionDataDetailsRepo : RepoGenericAbstract<RTVOEEProductionDataDetails>
    {
        protected new IDbContextOneProdRTV db;

        public RTVOEEProductionDataDetailsRepo(IDbContextOneProdRTV db) : base(db)
        {
            this.db = db;
        }

        public override RTVOEEProductionDataDetails GetById(int id)
        {
            return db.RTVOEEProductionDataDetails.FirstOrDefault(x => x.Id == id);
        }
        public override IQueryable<RTVOEEProductionDataDetails> GetList()
        {
            return db.RTVOEEProductionDataDetails.Where(x => x.Deleted == false).OrderByDescending(x => x.Id);
        }
        public RTVOEEProductionDataDetails GetProductionData(int machineId, DateTime dateFrom, DateTime dateTo)
        {
            return db.RTVOEEProductionDataDetails
                .Where(x => 
                    x.RTVOEEProductionData.MachineId == machineId &&
                    (dateFrom <= x.RTVOEEProductionData.ProductionDate && x.RTVOEEProductionData.TimeStamp <= dateTo))
                .OrderByDescending(x => x.Id).Take(1).FirstOrDefault();
        }
    }
}