using XLIB_COMMON.Repo;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Repo;
using System;
using System.Linq;
using XLIB_COMMON.Enums;
using MDL_ONEPROD.ComponentENERGY.Entities;
using MDL_BASE.Interfaces;
using System.Collections.Generic;
using MDL_ONEPROD.ComponentENERGY.Models;

namespace MDL_ONEPROD.ComponentENERGY.Repos
{
    public class EnergyCostRepo : RepoGenericAbstract<EnergyCost>
    {
        protected new IDbContextOneProdENERGY db;
        UnitOfWorkOneProdENERGY unitOfWork;

        public EnergyCostRepo(IDbContextOneProdENERGY db, XLIB_COMMON.Interface.IAlertManager alertManager, UnitOfWorkOneProdENERGY unitOfWork = null)
           : base(db, alertManager)
        {
            this.db = db;
            this.unitOfWork = unitOfWork;
        }

        public override EnergyCost GetById(int id)
        {
            return db.EnergyCosts.FirstOrDefault(x => x.Id == id);
        }
        public override IQueryable<EnergyCost> GetList()
        {
            return db.EnergyCosts.Where(x => x.Deleted == false).OrderByDescending(x => x.Id);
        }

        public int AddOrUpdate(EnergyCost item)
        {
            if(item.StartDate < item.EndDate)
            {
                EnergyCost temp = db.EnergyCosts.Where(x => x.Deleted == false && x.EnergyType == item.EnergyType && x.Id != item.Id).FirstOrDefault();
                if(temp == null)
                {
                    return base.AddOrUpdate(item);
                }
                else
                {
                    temp = db.EnergyCosts.Where(x => x.Deleted == false && x.Id == item.Id).FirstOrDefault();
                    if (isProperDateForEnergyCost(item))
                    {
                        temp.EnergyType = item.EnergyType;
                        temp.EndDate = item.EndDate;
                        temp.PricePerUnit = item.PricePerUnit;
                        temp.StartDate = item.StartDate;
                        temp.UnitOfMeasure = item.UnitOfMeasure;
                        temp.kWhConverter = item.kWhConverter;
                        temp.UseConverter = item.UseConverter;
                        return base.AddOrUpdate(temp);
                    }
                }
            }
            return -1;
        }

        private bool isProperDateForEnergyCost(EnergyCost item)
        {
            DateTime? minStartDate = db.EnergyCosts.Where(x => x.StartDate < item.StartDate && x.EnergyType == item.EnergyType && x.Deleted == false).Max(x => (DateTime?)x.StartDate);
            minStartDate = minStartDate == null ? db.EnergyCosts.Where(x => x.Deleted == false && x.EnergyType == item.EnergyType && x.Id != item.Id).Min(x => x.EndDate) : (DateTime?)db.EnergyCosts.Where(x => x.StartDate == minStartDate && x.EnergyType == item.EnergyType && x.Deleted == false).FirstOrDefault().EndDate;

            DateTime? maxEndDate = db.EnergyCosts.Where(x => x.EndDate > item.EndDate && x.EnergyType == item.EnergyType && x.Deleted == false).Min(x => (DateTime?)x.EndDate);
            maxEndDate = maxEndDate == null ? db.EnergyCosts.Where(x => x.Deleted == false && x.EnergyType == item.EnergyType && x.Id != item.Id).Max(x => x.StartDate) : (DateTime?)db.EnergyCosts.Where(x => x.EndDate == maxEndDate && x.EnergyType == item.EnergyType && x.Deleted == false).FirstOrDefault().StartDate;

            if( minStartDate > maxEndDate ||  item.StartDate > minStartDate && item.EndDate < maxEndDate)
            {
                return true;
            }
           
            return false;
        }

        public IQueryable<EnergyCost> GetList(EnergyCost filter)
        {
            return db.EnergyCosts.Where(x => x.Deleted == false).OrderByDescending(x => x.Id);
        }

        public EnergyCost GetCurrentSettlement(EnumEnergyType enumMediumType)
        {
             return db.EnergyCosts.Where(x => x.EnergyType == enumMediumType && x.Deleted == false && x.EndDate == null).FirstOrDefault();
        }

        public EnergyCost GetSettlementByDate(EnumEnergyType enumMediumType, DateTime startDate, DateTime endDate)
        {
            EnergyCost medium = db.EnergyCosts.Where(x => x.EnergyType == enumMediumType && x.Deleted == false && x.StartDate <= startDate && x.EndDate >= endDate).FirstOrDefault();
            if(medium == null)
            {
                medium = GetCurrentSettlement(enumMediumType);
            }
            return medium;
        }

        public EnergyCost GetPaymentPeriod(EnergyMeterImportData item, EnumEnergyType energyType )
        {
            EnergyCost temp = db.EnergyCosts.Where(x => x.Deleted == false && x.EnergyType == energyType && x.StartDate <= item.TimeStamp && x.EndDate >= item.TimeStamp).FirstOrDefault();
            if(temp == null)
            {
                temp = db.EnergyCosts.Where(x => x.Deleted == false && x.EnergyType == energyType && x.EndDate <= item.TimeStamp).OrderBy(x => x.EndDate).FirstOrDefault();
                if(temp == null)
                {
                    temp = db.EnergyCosts.Where(x => x.Deleted == false && x.EnergyType == energyType).OrderBy(x => x.EndDate).FirstOrDefault();
                    if(temp == null)
                    {
                        temp = new EnergyCost();
                        temp.PricePerUnit = 0;
                        temp.EnergyType = energyType;
                    }
                }
            }
            return temp;
        }
    }
}