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
    public class ToolChangeOverRepo : RepoGenericAbstract<ToolChangeOver>
    {
        protected new IDbContextOneprodAPS db;
        private UnitOfWorkOneprodAPS unitOfWork;

        public ToolChangeOverRepo(IDbContextOneprodAPS db, IAlertManager alertManager, UnitOfWorkOneprodAPS unitOfWork = null) : base(db)
        {
            this.db = db;
            this.unitOfWork = unitOfWork;
        }

        public override ToolChangeOver GetById(int id)
        {
            return db.ToolChangeOvers.FirstOrDefault(x => x.Id == id);
        }
        public override IQueryable<ToolChangeOver> GetList()
        {
            return db.ToolChangeOvers.OrderBy(x => x.Id);
        }

        public ToolChangeOver GetByNewToolId(int? newToolId)
        {
            return db.ToolChangeOvers.FirstOrDefault(t => (t.Tool1Id == newToolId) || (t.Tool2Id == newToolId));
        }

        public List<ToolChangeOver> GetToolChangeOvers()
        {
            return db.ToolChangeOvers.ToList();
        }
        public int UpdateToolChangeOver(int Tool1Id, int Tool2Id, int time)
        {
            ToolChangeOver tco = db.ToolChangeOvers
                .FirstOrDefault(t => 
                    (t.Tool1Id == Tool1Id && t.Tool2Id == Tool2Id) || 
                    (t.Tool1Id == Tool2Id && t.Tool2Id == Tool1Id));

            if (tco != null)
            {
                tco.Time = time;
            }
            else
            {
                tco = new ToolChangeOver { Tool1Id = Tool1Id, Tool2Id = Tool2Id, Time = time };
            }

            return AddOrUpdate(tco);
        }
    }
}