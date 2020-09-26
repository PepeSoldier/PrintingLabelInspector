using XLIB_COMMON.Repo;
using MDL_ONEPROD.Common;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.Scheduling;
using MDLX_MASTERDATA.Enums;
//using MDLX_MASTERDATA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Interface;

namespace MDL_ONEPROD.Repo.Scheduling
{
    public class ResourceGroupRepo : RepoGenericAbstract<ResourceOP>
    {
        protected new IDbContextOneprod db;
        private UnitOfWorkOneprod unitOfWork;

        public ResourceGroupRepo(IDbContextOneprod db, IAlertManager alertManager, UnitOfWorkOneprod unitOfWork = null)
            : base(db)
        {
            this.db = db;
            this.unitOfWork = unitOfWork;
        }

        public override ResourceOP GetById(int id)
        {
            return (ResourceOP)db.ResourcesOP.FirstOrDefault(x => x.Id == id && x.Type == ResourceTypeEnum.Group);
        }
        public override IQueryable<ResourceOP> GetList()
        {
            return (IQueryable<ResourceOP>)db.ResourcesOP.Where(x => x.Type == ResourceTypeEnum.Group).OrderBy(x => x.StageNo);
        }
        public IQueryable<ResourceOP> GetList(ResourceOP area)
        {
            return (IQueryable<ResourceOP>)db.ResourcesOP.Where(x => 
                    (x.Name.StartsWith(area.Name) || area.Name == null)
                    && x.Type == ResourceTypeEnum.Group)
                    .OrderBy(x => x.ResourceGroupId ?? x.Id)
                    .ThenBy(x => x.Name);
        }
        public ResourceOP GetByStageNo(int stageNo)
        {
            return (ResourceOP)db.ResourcesOP.FirstOrDefault(x => x.StageNo == stageNo && x.Type == ResourceTypeEnum.Group);
        }
        public int GetIdByMachineId(int machineId)
        {
            try
            {
                return db.ResourcesOP.Where(x => x.Id == machineId)
                    .Take(1).Select(x => x.ResourceGroup.Id)
                    .FirstOrDefault();
            }
            catch
            {
                return 0;
            }
        }
        public int GetAreasCount()
        {
            return db.ResourcesOP.Where(x=> x.Type == ResourceTypeEnum.Group).Count();
        }
    }
}