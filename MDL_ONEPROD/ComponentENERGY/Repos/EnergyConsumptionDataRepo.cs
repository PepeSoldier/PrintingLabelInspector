using XLIB_COMMON.Repo;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Repo;
using System;
using System.Linq;
using XLIB_COMMON.Enums;
using MDL_ONEPROD.ComponentENERGY.Entities;
using MDL_BASE.Enums;
using MDL_ONEPROD.Model.Scheduling;
using System.Collections.Generic;

namespace MDL_ONEPROD.ComponentENERGY.Repos
{
    public class EnergyConsumptionDataRepo : RepoGenericAbstract<EnergyConsumptionData>
    {
        protected new IDbContextOneProdENERGY db;
        UnitOfWorkOneProdENERGY unitOfWork;

        public EnergyConsumptionDataRepo(IDbContextOneProdENERGY db, XLIB_COMMON.Interface.IAlertManager alertManager, UnitOfWorkOneProdENERGY unitOfWork = null)
           : base(db, alertManager)
        {
            this.db = db;
            this.unitOfWork = unitOfWork;
        }

        public override EnergyConsumptionData GetById(int id)
        {
            return db.EnergyConsumptionDatas.FirstOrDefault(x => x.Id == id);
        }
        public override IQueryable<EnergyConsumptionData> GetList()
        {
            return db.EnergyConsumptionDatas.Where(x => x.Deleted == false).OrderByDescending(x => x.Id);
        }
        
        public decimal? GetTotalValueById(int id)
        {
            return db.EnergyConsumptionDatas.Where(x => x.Id == id).FirstOrDefault().TotalValue;
        }

        [Obsolete]
        public decimal GetCostByDate(int energyMeterId, DateTime dateFrom, DateTime dateTo)
        {
            return db.EnergyConsumptionDatas.Where(x => x.EnergyMeterId == energyMeterId && x.DateFrom >= dateFrom && x.DateTo <= dateTo).Sum(x => x.Cost);
        }

        public List<EnergyConsumptionData> GetDataByEnergyMetersAndDate(List<EnergyMeter> energyMeters, DateTime dateFrom, DateTime dateTo)
        {
            return db.EnergyConsumptionDatas.Where(x =>energyMeters.Select(y => y.Id).Contains(x.EnergyMeterId) && x.DateFrom >= dateFrom && x.DateTo <= dateTo).ToList();
        }

        public IQueryable<EnergyConsumptionData> GetDataByTimeRangeAndMachineIdAndEnergyType(DateTime dateFrom, DateTime dateTo, int machineId = -1, EnumEnergyType energyType = EnumEnergyType.Undefined)
        {
            return db.EnergyConsumptionDatas
                .Where(x => (x.Deleted == false ) &&
                            x.DateFrom >= dateFrom &&
                            x.DateTo < dateTo &&
                            (machineId == -1 || x.EnergyMeter.ResourceId == machineId) &&
                            (energyType == EnumEnergyType.ALL || x.EnergyMeter.EnergyType == energyType))
                .OrderByDescending(x => x.Id);
        }

        
    }
}