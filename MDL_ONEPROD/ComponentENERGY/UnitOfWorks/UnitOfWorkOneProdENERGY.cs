using MDL_ONEPROD.Common;
using MDL_ONEPROD.ComponentENERGY;
using MDL_ONEPROD.ComponentENERGY.Repos;
using MDL_ONEPROD.ComponentOEE.Repos;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Repo.OEERepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Model;

namespace MDL_ONEPROD.Repo
{
    public class UnitOfWorkOneProdENERGY : UnitOfWorkOneprod
    {
        public UnitOfWorkOneProdENERGY(IDbContextOneProdENERGY dbContext) : base(dbContext)
        {
            this.db = dbContext;
        }

        IDbContextOneProdENERGY db;
        public IDbContextOneProdENERGY Db
        {
            get
            {
                return db;
            }
        }

        private EnergyCostRepo energyCostRepo;
        private EnergyMeterRepo energyMeterRepo;
        private EnergyConsumptionDataRepo energyConsumptionDataRepo;


        public EnergyCostRepo EnergyCostRepo
        {
            get
            {
                energyCostRepo = (energyCostRepo != null) ? energyCostRepo : new EnergyCostRepo(db, XLIB_COMMON.Model.AlertManager.Instance, this);
                return energyCostRepo;
            }
        }

        public EnergyMeterRepo EnergyMeterRepo
        {
            get
            {
                energyMeterRepo = (energyMeterRepo != null) ? energyMeterRepo : new EnergyMeterRepo(db, XLIB_COMMON.Model.AlertManager.Instance, this);
                return energyMeterRepo;
            }
        }

        public EnergyConsumptionDataRepo EnergyConsumptionDataRepo
        {
            get
            {
                energyConsumptionDataRepo = (energyConsumptionDataRepo != null) ? energyConsumptionDataRepo : new EnergyConsumptionDataRepo(db, XLIB_COMMON.Model.AlertManager.Instance, this);
                return energyConsumptionDataRepo;
            }
        }

    }
}