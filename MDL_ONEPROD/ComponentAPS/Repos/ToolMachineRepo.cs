using XLIB_COMMON.Repo;
using MDL_ONEPROD.Common;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Interface;

namespace MDL_ONEPROD.Repo.Scheduling
{
    public class ToolMachineRepo: RepoGenericAbstract<ToolMachine>
    {
        protected new IDbContextOneprodAPS db;
        private UnitOfWorkOneprodAPS unitOfWork;

        public ToolMachineRepo(IDbContextOneprodAPS db, IAlertManager alertManager, UnitOfWorkOneprodAPS unitOfWork = null) : base(db)
        {
            this.db = db;
            this.unitOfWork = unitOfWork;
        }

        public override ToolMachine GetById(int id)
        {
            return db.ToolMachines.Include("Area").FirstOrDefault(x => x.Id == id);
        }
        public override IQueryable<ToolMachine> GetList()
        {
            return db.ToolMachines.OrderBy(x => x.Id);
        }

        public void ToolMachineUpdate(int machineId, int toolId, bool assigned, bool preffered, bool placed)
        {
            ToolMachine tm = db.ToolMachines.FirstOrDefault(m => m.MachineId == machineId && m.ToolId == toolId);

            if (tm == null)
            {
                tm = new ToolMachine { ToolId = toolId, MachineId = machineId };
            }

            if (!assigned)
            {
                Delete(tm);
            }
            else
            {
                tm.Preffered = preffered;
                tm.Placed = placed;
                AddOrUpdate(tm);
            }
        }
    }
}