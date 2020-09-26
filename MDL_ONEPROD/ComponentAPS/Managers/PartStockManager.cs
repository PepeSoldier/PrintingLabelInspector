
using MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDL_ONEPROD.Manager
{
    public class PartStockManager
    {
        UnitOfWorkOneprod uow;

        public PartStockManager(UnitOfWorkOneprod unitOfWork)
        {
            uow = unitOfWork;
        }
    }
}
