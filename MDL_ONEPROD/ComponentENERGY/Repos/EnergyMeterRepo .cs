using XLIB_COMMON.Repo;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Repo;
using System;
using System.Linq;
using XLIB_COMMON.Enums;
using MDL_ONEPROD.ComponentENERGY.Entities;
using MDL_BASE.Enums;
using System.Collections.Generic;

namespace MDL_ONEPROD.ComponentENERGY.Repos
{
    public class EnergyMeterRepo : RepoGenericAbstract<EnergyMeter>
    {
        protected new IDbContextOneProdENERGY db;
        UnitOfWorkOneProdENERGY unitOfWork;

        public EnergyMeterRepo(IDbContextOneProdENERGY db, XLIB_COMMON.Interface.IAlertManager alertManager, UnitOfWorkOneProdENERGY unitOfWork = null)
           : base(db, alertManager)
        {
            this.db = db;
            this.unitOfWork = unitOfWork;
        }

        public override EnergyMeter GetById(int id)
        {
            return db.EnergyMeters.FirstOrDefault(x => x.Id == id);
        }
        public override IQueryable<EnergyMeter> GetList()
        {
            return db.EnergyMeters.Where(x => x.Deleted == false).OrderByDescending(x => x.Id);
        }

        public IQueryable<EnergyMeter> GetList(EnergyMeter filter)
        {
            string Name = filter.Name != null ? filter.Name.Trim() : null;
            string MarkedName = filter.MarkedName != null ? filter.MarkedName.Trim() : null;
            EnumEnergyType EnergyType = filter.EnergyType != 0 ? filter.EnergyType : 0;
            UnitOfMeasure unitOfMeasure = filter.UnitOfMeasure != UnitOfMeasure.Undefined ? filter.UnitOfMeasure : UnitOfMeasure.Undefined;

            var query = db.EnergyMeters.Where(x =>
                (Name == null || x.Name == Name) &&
                (MarkedName == null || x.MarkedName == MarkedName) &&
                (EnergyType ==  0 || x.EnergyType == EnergyType) &&
                (unitOfMeasure == UnitOfMeasure.Undefined || x.UnitOfMeasure == unitOfMeasure) &&
                (x.Deleted == false )).
                OrderByDescending(x => x.Id);
            return query;
        }

        public List<EnergyMeter> GetEnergyMetersByResource(int resourceId)
        {
            return db.EnergyMeters.Where(x => x.ResourceId == resourceId && x.Deleted == false).ToList();
        }

        public List<EnergyMeter> GetEnergyMetersByType(EnumEnergyType enumEnergyType)
        {
            return db.EnergyMeters.Where(x => x.EnergyType == enumEnergyType && x.Deleted == false).ToList();
        }

        public List<EnergyMeter> GetEnergyMetersByResourceAndType(int resourceId, EnumEnergyType enumEnergyType)
        {
            return db.EnergyMeters.Where(x => x.ResourceId == resourceId && x.EnergyType == enumEnergyType && x.Deleted == false).ToList();
        }

        public List<EnergyMeter> GetAllEnergyMeters()
        {
            return db.EnergyMeters.Where(x => x.Deleted == false).ToList();
        }
    }
}