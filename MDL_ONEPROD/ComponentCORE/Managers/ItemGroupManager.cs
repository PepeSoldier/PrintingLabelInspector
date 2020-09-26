
using MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDL_ONEPROD.Manager
{
    public class ItemGroupManager
    {
        UnitOfWorkOneprod uow;
        List<ItemOP> itemGroups;

        public ItemGroupManager(UnitOfWorkOneprod unitOfWork)
        {
            uow = unitOfWork;
            itemGroups = uow.ItemGroupRepo.GetListAsNoTracking();
        }

        public int GetMinBatch(int itemGroupId)
        {
            ItemOP pc = itemGroups.FirstOrDefault(b => b.Id == itemGroupId);

            if (pc != null)
                return pc.MinBatch;
            else
                return 20;
        }
        public int GetParentProcessId(int processId)
        {
            return uow.ProcessRepo.GetParentProcessId(processId);
        }
    }
}
