using XLIB_COMMON.Repo;
using MDL_ONEPROD.ComponentOEE.Models;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Interface;
using MDLX_MASTERDATA.Enums;
using MDLX_MASTERDATA.Repos;

namespace MDL_ONEPROD.Repo.Scheduling
{
    public class ResourceOPRepo : ResourceAbstractRepo<ResourceOP>
    {
        protected new IDbContextOneprod db;

        public ResourceOPRepo(IDbContextOneprod db, IAlertManager alertManager) : base(db)
        {
            this.db = db;
        }

        //public override ResourceOP GetById(int id)
        //{
        //    return db.ResourcesOP.FirstOrDefault(x => x.Id == id);
        //}
        //public override IQueryable<ResourceOP> GetList()
        //{
        //    return db.ResourcesOP.Where(x =>
        //            x.Deleted == false &&
        //            x.Type == ResourceTypeEnum.Resource)
        //        .OrderBy(x => x.Name);
        //}
        //public IQueryable<ResourceOP> GetList(int areaId)
        //{
        //    return db.ResourcesOP.Where(x =>
        //            x.Deleted == false &&
        //            x.Type == ResourceTypeEnum.Resource &&
        //            x.ResourceGroupId == areaId)
        //        .OrderBy(x => x.Name);
        //}
        public IQueryable<ResourceOP> GetList(ResourceOP filterItem, int areaId)
        {
            return db.ResourcesOP.Where(x =>
                        (x.Deleted == false) &&
                        (x.Type == ResourceTypeEnum.Resource) &&
                        (x.ResourceGroupId == areaId || areaId == 0) &&
                        (x.Name.StartsWith(filterItem.Name) || filterItem.Name == null))
                    .OrderBy(x => x.ResourceGroupId ?? x.Id)
                    .ThenBy(x => x.Type)
                    .ThenBy(x => x.Name);
        }
        public IQueryable<ResourceOP> GetAllTypesList(ResourceOP filterItem, int areaId)
        {
            return db.ResourcesOP.Where(x =>
                        (x.Deleted == false) &&
                        (filterItem.Type == ResourceTypeEnum.Group || x.Type == filterItem.Type) &&
                        (x.ResourceGroupId == areaId || areaId == 0) &&
                        (x.Name.StartsWith(filterItem.Name) || filterItem.Name == null))
                    .OrderBy(x => x.ResourceGroupId ?? x.Id)
                    .ThenBy(x => x.Type)
                    .ThenBy(x => x.Name);
        }
        public List<ResourceOP> GetListForDropDown()
        {
            return db.ResourcesOP.Where(x =>
                    x.Deleted == false &&
                    x.Type == ResourceTypeEnum.Resource &&
                    x.ResourceGroupOP.StageNo > 0)
                .OrderBy(x => x.ResourceGroupOP.StageNo)
                .ThenBy(x => x.Name)
                .ToList();
        }


        //public int AreaMachinesNumber(int areaId)
        //{
        //    //int areaId = GetAreaId(stageNo);
        //    return db.ResourcesOP.Count(o => o.ResourceGroupId == areaId && o.Type == ResourceTypeEnum.Resource);
        //}
        //public ResourceOP GetMachine(int id)
        //{
        //    return db.ResourcesOP.FirstOrDefault(m => m.Id == id);
        //}
        //public decimal getOEE(int machineId)
        //{
        //    return db.ResourcesOP.FirstOrDefault(m => m.Id == machineId).TargetOee;
        //}

        public MachineTargets GetTargetsForMachine(ResourceOP resource)
        {
            MachineTargets mt = PrepareMachineTargets(resource);
            return mt;
        }
        public MachineTargets GetTargetsForMachine(int machineId)
        {
            ResourceOP resource = db.ResourcesOP.FirstOrDefault(x1 => x1.Id == machineId);
            MachineTargets mt = PrepareMachineTargets(resource);
            return mt;
        }
        private static MachineTargets PrepareMachineTargets(ResourceOP resource)
        {
            return new MachineTargets()
            {
                OeeTarget = resource.TargetOee,
                AvailabilityTarget = resource.TargetAvailability,
                PerformanceTarget = resource.TargetPerformance,
                QualityTarget = resource.TargetQuality
            };
        }
    }
}