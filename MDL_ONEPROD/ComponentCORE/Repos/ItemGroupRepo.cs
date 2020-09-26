using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.Scheduling;
using MDLX_MASTERDATA._Interfaces;
using MDLX_MASTERDATA.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Interface;
using XLIB_COMMON.Repo;

namespace MDL_ONEPROD.Repo.Scheduling
{
    public class ItemGroupRepo : RepoGenericAbstract<ItemOP>
    {
        protected new IDbContextOneprod db;

        public ItemGroupRepo(IDbContextOneprod db, IAlertManager alertManager) : base(db)
        {
            this.db = db;
        }

        public override ItemOP GetById(int id)
        {
            return db.ItemsOP.FirstOrDefault(x => x.Id == id);
        }
        public override IQueryable<ItemOP> GetList()
        {
            return db.ItemsOP.Where(x=> x.Deleted == false && x.Type == ItemTypeEnum.ItemGroup).OrderBy(x => x.Name);
        }
        public List<ItemOP> GetList(ItemOP Part)
        {
            return db.ItemsOP.Where(x =>
                    (x.Deleted == false) && 
                    (x.Type == ItemTypeEnum.ItemGroup) &&
                    (Part.ResourceGroupId == null || Part.ResourceGroupId == -1 || x.ResourceGroupId == Part.ResourceGroupId) &&
                    //(Part.ProcessId == null || Part.ProcessId == -1 || x.ProcessId == Part.ProcessId) &&
                    (Part.Name == null || x.Name.StartsWith(Part.Name)))
                .OrderBy(x => x.ResourceGroupOP.Name)//.OrderBy(x => x.ResourceGroup.StageNo)
                .ThenBy(x=>x.Name)
                .ToList();
        }
        public List<ItemOP> GetListAsNoTracking()
        {
            return db.ItemsOP
                .AsNoTracking()
                .Where(x => x.Deleted == false && x.Type == ItemTypeEnum.ItemGroup)
                .OrderBy(x => x.Name)
                .ToList();
        }
    }
}