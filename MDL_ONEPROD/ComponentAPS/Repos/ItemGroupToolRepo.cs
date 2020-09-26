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
    public class ItemGroupToolRepo : RepoGenericAbstract<ItemGroupTool>
    {
        protected new IDbContextOneprodAPS db;
        private UnitOfWorkOneprodAPS unitOfWork;

        public ItemGroupToolRepo(IDbContextOneprodAPS db, IAlertManager alertManager, UnitOfWorkOneprodAPS unitOfWork = null) : base(db)
        {
            this.db = db;
            this.unitOfWork = unitOfWork;
        }

        public override ItemGroupTool GetById(int id)
        {
            return db.ItemGroupTools.FirstOrDefault(x => x.Id == id);
        }
        public override IQueryable<ItemGroupTool> GetList()
        {
            return db.ItemGroupTools.OrderBy(x => x.Id);
        }

        public IQueryable<ItemGroupTool> GetList(int partCategoryId)
        {
            return db.ItemGroupTools.Where(x => x.ItemGroupId == partCategoryId).OrderBy(x => x.Id);
        }

        public List<ItemGroupTool> GetListAsNoTracking()
        {
            return db.ItemGroupTools.AsNoTracking().OrderBy(x => x.Id).ToList();
        }

        public List<ItemGroupTool> GetItemGroupTools(int partCategoryId)
        {
            return db.ItemGroupTools.Where(p => p.ItemGroupId == partCategoryId).ToList();
        }
        public int GetItemGroupToolCount(int toolId)
        {
            //using (DbContextPreprod db5 = new DbContextPreprod())
            //{
                return db.ItemGroupTools.AsNoTracking().Where(p => p.ToolId == toolId).Count();
            //}
        }
        public int AddItemGroupTool(int partCategoryId, int toolId, int Id = 0)
        {
            ItemGroupTool pcTool = db.ItemGroupTools.Where(x => x.Id == Id).FirstOrDefault();

            if (pcTool == null)
            {
                pcTool = new ItemGroupTool { ItemGroupId = partCategoryId, ToolId = toolId };
            }
            else
            {
                pcTool.ItemGroupId = partCategoryId;
                pcTool.ToolId = toolId;
            }
            return AddOrUpdate(pcTool);
        }
        public int DeleteItemGroupTool(int partCategoryToolId)
        {
            ItemGroupTool pct = db.ItemGroupTools.FirstOrDefault(p => p.Id == partCategoryToolId);
            int partCategoryId = pct.ItemGroupId;
            if (pct != null)
            {
                Delete(pct);
            }
            return partCategoryId;
        }

    }
}