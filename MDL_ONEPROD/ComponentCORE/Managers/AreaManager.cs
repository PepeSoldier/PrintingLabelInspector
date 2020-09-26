
using MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDL_ONEPROD.Manager
{
    public class AreaManager
    {
        UnitOfWorkOneprod uow;
        public List<ResourceOP> Areas { get; set; }

        public AreaManager(UnitOfWorkOneprod unitOfWork)
        {
            uow = unitOfWork;
            Areas = uow.ResourceGroupRepo.GetList().ToList();
        }

        public int GetSafetyTime(int areaId)
        {
            return Areas.FirstOrDefault(a => a.Id == areaId).SafetyTime * 60;
        }
    }
}
