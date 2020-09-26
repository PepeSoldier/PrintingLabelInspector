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
    public class ToolRepo : RepoGenericAbstract<Tool>
    {
        protected new IDbContextOneprodAPS db;
        private UnitOfWorkOneprodAPS unitOfWork;

        public ToolRepo(IDbContextOneprodAPS db, IAlertManager alertManager, UnitOfWorkOneprodAPS unitOfWork = null)
            : base(db)
        {
            this.db = db;
            this.unitOfWork = unitOfWork;
        }

        public override Tool GetById(int id)
        {
            return db.Tools.FirstOrDefault(x => x.Id == id);
        }
        public override IQueryable<Tool> GetList()
        {
            return db.Tools.OrderBy(x => x.Name);
        }

        public Tool GetTool(int id)
        {
            return db.Tools.FirstOrDefault(t => t.Id == id);
        }
        public IQueryable<Tool> GetTools()
        {
            return db.Tools.OrderBy(t => t.Name);
        }
        public int GetToolCount(int partCategoryId)
        {
            //using (DbContextPreprod db2 = new DbContextPreprod())
            //{
                return db.ItemGroupTools.AsNoTracking().Where(t => t.ItemGroupId == partCategoryId).Count();
            //}
        }
        public int DeleteTool(int id)
        {
            Tool tool = GetTool(id);

            if (tool != null)
            {
                db.ToolChangeOvers.RemoveRange(db.ToolChangeOvers.Where(t => t.Tool2Id == id));
                db.SaveChanges();
                Delete(tool);
                return -1;
            }
            return 0;
        }
        //ToolGroup
        public List<ToolGroup> GetToolGroups()
        {
            return db.ToolGroups.OrderBy(tg => tg.Name).ToList();
        }
    }
}